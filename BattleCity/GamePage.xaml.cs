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
using Windows.UI.Xaml.Media.Imaging;
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

        public static bool MP; // Bool used for checking if 2-player mode was selected
        private bool PlayerHit = false;
        private bool GoalHit = false;
        private int LevelNumber;

        // These rectangles are used as hitboxes
        private Rect PlayerRect;
        private Rect BlockRect;
        private Rect BulletRect;
        private Rect EnemyRect;

        private double CanvasWidth;
        private double CanvasHeight;
        public static DispatcherTimer dispatcherTimer;

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

            LevelNumber = 1;
            level.LoadLevel(LevelNumber);
            level.BuildLevel(Canvas);

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
                player.CollisionRelease();
                player.UpdatePlayer(Canvas);
                player.UpdateBullet(Canvas);
                if (PlayerHit == true)
                    break;
                }
                foreach(Enemy enemy in enemies)
                {
                enemy.AnimationUpdate();
                enemy.CollisionRelease();
                enemy.Move(random.Next(1,5), random.Next(3,6), random.Next(1,3), random.Next(1,31));
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
            CheckGameOver(); // Goes to the method that checks if any game over criterias are met
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
                    if (!BlockRect.IsEmpty && block.CanGoTrough == true) // Slower speed while moving on magic block
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
                            if (enemy.LocationX > block.LocationX && enemy.tankDirection == 1) // Checking if enemy is intersecting block from the right
                            {
                                enemy.StopRight = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
                            }

                            if (enemy.LocationY > block.LocationY && enemy.tankDirection == 2) // Checking if enemy is intersecting block from the bottom
                            {
                                enemy.StopBottom = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
                            }

                            if (enemy.LocationX < block.LocationX && enemy.tankDirection == 3) // Checking if enemy is intersecting block from the left
                            {
                                enemy.StopLeft = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
                            }

                            if (enemy.LocationY < block.LocationY && enemy.tankDirection == 4) // Checking if enemy is intersecting block from the top
                            {
                                enemy.StopTop = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
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
                        enemy.RemoveBullet(Canvas);
                        Canvas.Children.Remove(enemy);
                        enemies.Remove(enemy);
                        break;
                    }
                    else
                        PlayerHit = false;
                }
                if(PlayerHit == true)
                {
                    player.RemoveBullet(Canvas);
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
                            player.RemoveBullet(Canvas);
                            Canvas.Children.Remove(block);
                            blocks.Remove(block);
                            player.score += block.PointValue;
                            break;
                        }
                        else if (!BlockRect.IsEmpty && block.CanDestroy == false && block.CanGoTrough == false)
                        {
                            player.RemoveBullet(Canvas);
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
                            player.RemoveBullet(Canvas);
                            Canvas.Children.Remove(enemy);
                            enemy.RemoveBullet(Canvas);
                            enemies.Remove(enemy);
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
                            enemy.RemoveBullet(Canvas);
                            Canvas.Children.Remove(block);
                            blocks.Remove(block);
                            break;
                        }
                        else if (!BlockRect.IsEmpty && block.CanDestroy == false && block.CanGoTrough == false)
                        {
                            enemy.RemoveBullet(Canvas);
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
                            enemy.RemoveBullet(Canvas);
                            Canvas.Children.Remove(player);
                            players.Remove(player);
                            player.RemoveBullet(Canvas);
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

        //Checking if game is over
        private void CheckGameOver()
        {
            if (!players.Any())
            {
                SavePoints();
                GameEndImage.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/Lose.jpg"));
                GameEndImage.Visibility = Visibility.Visible;
                dispatcherTimer.Stop();
            }
            if(GoalHit == true)
            {
                SavePoints();
                GameEndImage.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/Lose.jpg"));
                GameEndImage.Visibility = Visibility.Visible;
                dispatcherTimer.Stop();
            }
            if (!enemies.Any())
            {
                SavePoints();
                GameEndImage.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/win.jpg"));
                GameEndImage.Visibility = Visibility.Visible;
                NextLevelButton.Visibility = Visibility.Visible;
                dispatcherTimer.Stop();
            }
        }

        private void NextLevelButton_Click(object sender, RoutedEventArgs e)
        {
            GameEndImage.Visibility = Visibility.Collapsed;
            level.DestroyLevel(Canvas);
            LevelNumber++;
            
            level.LoadLevel(LevelNumber);
            level.BuildLevel(Canvas);
            dispatcherTimer.Start();
        }

        // Method for saving points to a file
        private async void SavePoints()
        {
            //Creating the string to write
            string Player1pisteet = Player1Score.Text;
            //string Player2pisteet = Player2Score.Text; if 2player mode true
            int player1points = int.Parse(Player1pisteet);
            //int player2points = int.Parse(Player2pisteet); if 2player mode true
            //Create the text file to hold the data
            StorageFolder storageFolder =
            ApplicationData.Current.LocalFolder;           
            StorageFile HSFile =
                await storageFolder.CreateFileAsync("Highscore.txt",
                    CreationCollisionOption.ReplaceExisting);

            //Write data to file
            
            await FileIO.WriteTextAsync(HSFile, "Player 1 Highscore:" +Player1pisteet+Environment.NewLine /*+"Player 2 Highscore:" + Player2pisteet if2player true*/);
        }
  
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
