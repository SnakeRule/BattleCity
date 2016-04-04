using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BattleCity
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private Level level = new Level();

        private Random random;

        private bool MP; // Bool used for checking if 2-player mode was selected
        private bool PlayerHit = false;
        private bool GoalHit = false;

        // These rectangles are used as hitboxes
        private Rect PlayerRect;
        private Rect BlockRect;
        private Rect BulletRect;
        private Rect EnemyRect;

        private double CanvasWidth;
        private double CanvasHeight;
        private DispatcherTimer dispatcherTimer;

        private List<Block> blocks = new List<Block>();
        private List<Player> players = new List<Player>();
        private List<Enemy> enemies = new List<Enemy>();


        public GamePage()
        {
            this.InitializeComponent();
            // Setting up the canvas
            Canvas.Width = 680;
            Canvas.Height = 680;
            CanvasWidth = Canvas.Width;
            CanvasHeight = Canvas.Height;

            blocks = level.blocks;
            players = level.players;
            enemies = level.enemies;

            level.Level1(Canvas);
          
            random = new Random(); // setting up rng for enemy movement

            // Setting up the timer that runs the Game method
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
            dispatcherTimer.Start();

        }

        public void Game(object sender, object e)
        {
                foreach (Player player in players)
                {
                player.AnimationUpdate();
                BlockCollisionCheck();               
                if (PlayerHit == true)
                    break;
                player.CollisionRelease();
                player.UpdatePlayer(Canvas);
                player.UpdateBullet(Canvas);
                }
                foreach(Enemy enemy in enemies)
                {
                enemy.AnimationUpdate();
                enemy.CollisionRelease();
                enemy.Move(random.Next(1,5), random.Next(1,3), random.Next(1,3));
                enemy.UpdatePlayer(Canvas);
                enemy.UpdateBullet(Canvas);
                }
            //foreach(Bullet bullet in Character_base.bullets)
            foreach (Block block in blocks)
            {
                BulletCollisionCheck();
                break;
            }
            UpdatePoints(); // Goes to the method that updates player scores to the screen
            CheckGameOver();
        }
 
        // Back to mainmenu button method
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            this.Frame.Navigate(typeof(MainPage));
            /* unnecessary?, afraid to delete 
            // get root frame (which show pages)
            Frame rootFrame = Window.Current.Content as Frame;
            // did we get it correctly
            if (rootFrame == null) return;
            // navigate back if possible
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }*/
        }

        private void BlockCollisionCheck()
        {
            // Collision detection between blocks and players
            foreach (Player player in players)
            {
                player.StopTop = false;
                player.StopRight = false;
                player.StopLeft = false;
                player.StopBottom = false;

                PlayerRect = player.GetRect();

                foreach (Block block in blocks)
                {
                    BlockRect = block.GetRect();
                    BlockRect.Intersect(PlayerRect);
                    // PlayerRect.Intersect(PlayerRect); between players

                    if (!BlockRect.IsEmpty && block.CanGoTrough == false) //player and block collisions
                    {
                        if (player.LocationX > block.LocationX && player.tankDirection == 1) // Checking if player1 is intersecting player 2 from the right
                        {
                            player.StopRight = true;
                        }

                        if (player.LocationY > block.LocationY && player.tankDirection == 2) // Checking if player1 is intersecting player 2 from the bottom
                        {
                            player.StopBottom = true;
                        }

                        if (player.LocationX < block.LocationX && player.tankDirection == 3) // Checking if player1 is intersecting player 2 from the left
                        {
                            player.StopLeft = true;
                        }

                        if (player.LocationY < block.LocationY && player.tankDirection == 4) // Checking if player1 is intersecting player 2 from the top
                    {
                            player.StopTop = true;
                        }
                        break;
                    }
                    if(!BlockRect.IsEmpty && block.CanGoTrough == true) // Slower speed while moving on magic block
                    {
                        player.speed = 2;
                        break;                  
                    } else { player.speed = 4; }
                }

                // Collision detection between blocks and enemies
                foreach (Enemy enemy in enemies)
                {
                    enemy.StopTop = false;
                    enemy.StopRight = false;
                    enemy.StopLeft = false;
                    enemy.StopBottom = false;

                    EnemyRect = enemy.GetRect();

                    foreach (Block block in blocks)
                    {
                        BlockRect = block.GetRect();
                        BlockRect.Intersect(EnemyRect);

                        if(!BlockRect.IsEmpty && block.Goal == true) // Checking if enemy tank drives into goal
                        {
                            GoalHit = true;
                        }

                        if (!BlockRect.IsEmpty && block.CanGoTrough == false)
                        {
                            if (enemy.LocationX > block.LocationX && enemy.tankDirection == 1) // Checking if enemy is intersecting player 2 from the right
                            {
                                enemy.StopRight = true;
                                if (block.CanDestroy == true)
                                {
                                    enemy.CreateBullet();
                                }
                            }

                            if (enemy.LocationY > block.LocationY && enemy.tankDirection == 2) // Checking if enemy is intersecting player 2 from the bottom
                            {
                                enemy.StopBottom = true;
                                if (block.CanDestroy == true)
                                {
                                    enemy.CreateBullet();
                                }
                            }

                            if (enemy.LocationX < block.LocationX && enemy.tankDirection == 3) // Checking if enemy is intersecting player 2 from the left
                            {
                                enemy.StopLeft = true;
                                if (block.CanDestroy == true)
                                {
                                    enemy.CreateBullet();
                                }
                            }

                            if (enemy.LocationY < block.LocationY && enemy.tankDirection == 4) // Checking if enemy is intersecting player 2 from the top
                            {
                                enemy.StopTop = true;
                                if (block.CanDestroy == true)
                                {
                                    enemy.CreateBullet();
                                }
                            }
                            break;
                        }
                        if (!BlockRect.IsEmpty && block.CanGoTrough == true) // Slower speed while moving on magic block
                        {
                            enemy.speed = 2;
                            break;
                        }
                        else { enemy.speed = 4; }
                    }
                }

                foreach (Enemy enemy in enemies) // This is where the collision between players and enemies is detected
                {
                    EnemyRect = enemy.GetRect();
                    EnemyRect.Intersect(PlayerRect);

                    if (!EnemyRect.IsEmpty)
                    {
                        PlayerHit = true;
                        enemy.RemoveBullet();
                        Canvas.Children.Remove(enemy);
                        enemies.Remove(enemy);
                        break;
                    }
                    else
                        PlayerHit = false;
                }
                if(PlayerHit == true)
                {
                    player.RemoveBullet();
                    Canvas.Children.Remove(player);
                    players.Remove(player);
                    break;
                }
            }
        }

        private void BulletCollisionCheck() // Bullet-block collision detection
        {
            foreach (Player player in players) // Collision detection for player bullets
            {
                foreach (Bullet bullet in player.bullets)
                {
                    foreach (Block block in blocks)
                    {
                        BlockRect = block.GetRect();
                        BulletRect = bullet.GetRect();
                        BlockRect.Intersect(BulletRect);

                        if (!BlockRect.IsEmpty && block.CanDestroy == true)
                        {
                            if(block.Goal == true)
                            {
                                break;
                            }
                            Canvas.Children.Remove(bullet);
                            player.bullets.Remove(bullet);
                            Canvas.Children.Remove(block);
                            blocks.Remove(block);
                            player.score += block.PointValue;
                            break;
                        }
                        else if (!BlockRect.IsEmpty && block.CanDestroy == false && block.CanGoTrough == false)
                        {
                            Canvas.Children.Remove(bullet);
                            player.bullets.Remove(bullet);
                            break;
                        }
                    }
                    foreach (Enemy enemy in enemies)
                    {
                        EnemyRect = enemy.GetRect();
                        BulletRect = bullet.GetRect();
                        EnemyRect.Intersect(BulletRect);

                        if (!EnemyRect.IsEmpty)
                        {
                            Canvas.Children.Remove(bullet);
                            player.bullets.Remove(bullet);
                            Canvas.Children.Remove(enemy);
                            enemies.Remove(enemy);
                            enemy.RemoveBullet();
                            enemy.bullets.Remove(bullet);
                            player.score += enemy.PointValue;
                            break;
                        }
                    }
                    break;
                }
            }

            foreach (Enemy enemy in enemies) // Collision detection for enemy bullets
            {
                foreach (Bullet bullet in enemy.bullets)
                {
                    foreach (Block block in blocks)
                    {
                        BlockRect = block.GetRect();
                        BulletRect = bullet.GetRect();
                        BlockRect.Intersect(BulletRect);

                        if (!BlockRect.IsEmpty && block.CanDestroy == true)
                        {
                            if (block.Goal == true)
                            {
                                GoalHit = true;
                            }
                            Canvas.Children.Remove(bullet);
                            enemy.bullets.Remove(bullet);
                            Canvas.Children.Remove(block);
                            blocks.Remove(block);
                            break;
                        }
                        else if (!BlockRect.IsEmpty && block.CanDestroy == false && block.CanGoTrough == false)
                        {
                            Canvas.Children.Remove(bullet);
                            enemy.bullets.Remove(bullet);
                            break;
                        }

                    }
                    foreach (Player player in players)
                    {
                        PlayerRect = player.GetRect();
                        BulletRect = bullet.GetRect();
                        PlayerRect.Intersect(BulletRect);

                        if (!PlayerRect.IsEmpty)
                        {
                            Canvas.Children.Remove(bullet);
                            enemy.bullets.Remove(bullet);
                            Canvas.Children.Remove(player);
                            players.Remove(player);
                            player.RemoveBullet();
                            player.bullets.Remove(bullet);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        // Method for updating the player points on the screen
        private void UpdatePoints()
        {
            foreach (Player player in players)
            {
                if (player.Player2 == false)
                {
                    Player1Score.Text = player.score.ToString();
                }
                if (player.Player2 == true)
                {
                    Player2Score.Text = player.score.ToString();
                }
            }

        }

        // This is for the two player mode, navigated to from the main page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is bool) // Checks if a bool was passed from MainPage
            {
                MP = (bool)e.Parameter; // If so, then MP gets the value

                if(MP == true) // If MP is true, a second player is added
                {
                    level.CreatePlayer2(Canvas);
                }
            }
            base.OnNavigatedTo(e);
        }
        //Checking if game is over
        private void CheckGameOver()
        {
            if (!players.Any())
            {
                //SavePoints();
                dispatcherTimer.Stop();
            }
            if(GoalHit == true)
            {
                //SavePoints();
                dispatcherTimer.Stop();
            }
            if (!enemies.Any())
            {
                dispatcherTimer.Stop();
            }
        }

        // Method for saving points to a file
        /*
        private async void SavePoints()
        {
            string Player1pisteet = Player1Score.Text;
            StorageFolder storageFolder =
            ApplicationData.Current.LocalFolder;           
            StorageFile Highscores =
                await storageFolder.GetFileAsync("Highscores.txt");
            await FileIO.WriteTextAsync(Highscores, Player1pisteet);
        }
        */
        //Method for controlling pew volume
        /* not working, will have to change mediaelement creation
         public void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
            {
         if (player1 != null)
         {
           player1.SetVolume(VolumeSlider.Value);
        }
        }
        */

    }

}
