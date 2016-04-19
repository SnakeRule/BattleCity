using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
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
        public static int LevelNumber;

        private Random random;
        private int meowSoundNumber;
        private string filePath;
      
        List<double> HSlines = new List<double>(); // The highest score
       
        //Creating the highscore files
        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        private StorageFile HSFile; // This is the file that holds the highest score
        private StorageFile HSNimet; // This is the gile that holds the name of current highest score

        StreamReader hsreader;
        private MediaElement mediaElement;

        public static bool MP; // Bool used for checking if 2-player mode was selected
        private bool playerHit = false;
        private bool goalHit = false;
        private int p1PreviousScore;
        private int p2PreviousScore;
        public static int P1Lives, P2Lives;
        public static bool P1Dead, P2Dead;

        // These rectangles are used as hitboxes
        private Rect playerRect;
        private Rect blockRect;
        private Rect bulletRect;
        private Rect enemyBulletRect;
        private Rect enemyRect;

        private double canvasWidth;
        private double canvasHeight;
        private DispatcherTimer dispatcherTimer;
        public static bool GameRunning { get; set; }

        private List<Block> blocks = new List<Block>();
        private List<Player> players = new List<Player>();
        private List<Enemy> enemies = new List<Enemy>();

        public GamePage()
        {
            this.InitializeComponent();
            VolumeSlider.Value = BackgroundMediaPlayer.Current.Volume * 100;
            VolumeSlider.IsTabStop = false;
            MuteButton.IsTabStop = false;
            // Setting up the canvas
            Canvas.Width = 680;
            Canvas.Height = 680;
            canvasWidth = Canvas.Width;
            canvasHeight = Canvas.Height;

            // Reading the highscores
            ReadScores();
            ShowScores();

            blocks = level.blocks;
            players = level.players;
            enemies = level.enemies;

            random = new Random(); // setting up rng for enemy movement


            P1Lives = 3;
            P2Lives = 3;
            P1NameTextBlock.Text = MenuPage.P1Name + " score:";
            P2NameTextBlock.Text = MenuPage.P2Name + " score:";

            level.LoadLevel();
            level.BuildLevel(Canvas);

            // Setting up the timer that runs the Game method
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
            GameRunning = true;
            dispatcherTimer.Start();
        }

        public void Game(object sender, object e)
        {
                foreach (Player player in players)
                {
                BlockCollisionCheck();
                if (player.Invincible == true)
                {
                    player.Invincibility();
                }
                if (player.SpeedUp == true)
                {
                    player.PowerUpSpeed();
                }
                player.CollisionRelease();
                player.UpdatePlayer(Canvas, BackgroundMediaPlayer.Current.Volume);
                player.UpdateBullet(Canvas);
                player.AnimationUpdate();
                if (playerHit == true) // This is used to break out of the foreach loop if a player dies, preventing crashing
                    break;
                }
                foreach(Enemy enemy in enemies)
                {
                enemy.CollisionRelease();
                enemy.Move(random.Next(1, 5), random.Next(3, 6), random.Next(1, 3), random.Next(0, 15));
                enemy.UpdatePlayer(Canvas, BackgroundMediaPlayer.Current.Volume);
                enemy.UpdateBullet(Canvas);
                enemy.AnimationUpdate();
            }
            //foreach(Bullet bullet in Character_base.bullets)
            foreach (Block block in blocks)
            {
                BulletCollisionCheck();
                break;
            }
            if(P1Dead == true && P1Lives > 0)
            {
                    level.RespawnPlayer1(Canvas, p1PreviousScore);
            }
            if(P2Dead == true && P2Lives > 0)
            {
                    level.RespawnPlayer2(Canvas, p2PreviousScore);
            }
            UpdatePoints(); // Goes to the method that updates player scores to the screen
            CheckGameOver(); // Goes to the method that checks if any game over criterias are met
        }
        // Back to mainmenu button method
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// The BlockClollisionCheck checks if players, enemies or bullets are colliding with blocks
        /// </summary>
        private void BlockCollisionCheck()
        {
            // Collision detection between blocks and players
            foreach (Player player in players) // Checking each player in players list
            {
                // Each time collision is checked the booleans that are used to stop the player from moving in a certain direction are changed back to false to avoid getting stuck
                player.StopDown = false;
                player.StopLeft = false;
                player.StopRight = false;
                player.StopUp = false;

                playerRect = player.GetRect(); // Creating rectangle for player to be used in collision detection

                foreach (Block block in blocks) // Checking each block in blocks list
                {
                    blockRect = block.GetRect(); // Creating rectangle
                    blockRect.Intersect(playerRect); // Checking for intersections between player and block

                    int tickCounter = 0;
                    // Collision detection between
                    if (!blockRect.IsEmpty && block.IsPowerUp == true)
                    {
                        tickCounter++;
                        switch (block.PowerUpType)
                        {
                            case 1:
                                {
                                    player.SpeedUp = true;
                                    player.SpeedUpTickCounter = 0;
                                    Canvas.Children.Remove(block);
                                    blocks.Remove(block);
                                }       
                                break;
                            case 2:
                                Canvas.Children.Remove(block);
                                blocks.Remove(block);
                                break;
                            case 3:
                                Canvas.Children.Remove(block);
                                blocks.Remove(block);
                                break;
                            case 4:
                                Canvas.Children.Remove(block);
                                blocks.Remove(block);
                                break;
                            default: break;
                        }
                    }

                    if (!blockRect.IsEmpty && block.CanGoTrough == false) //player and block collisions
                    {
                        if (player.LocationX > block.LocationX && player.CatDirection == 1) // Checking if player1 is intersecting the block from the right. This uses the tank's direction
                        {
                            player.StopLeft = true;
                        }

                        if (player.LocationY > block.LocationY && player.CatDirection == 2) // Checking if player1 is intersecting the block from the bottom. This uses the tank's direction
                        {
                            player.StopUp = true;
                        }

                        if (player.LocationX < block.LocationX && player.CatDirection == 3) // Checking if player1 is intersecting the block from the left. This uses the tank's direction
                        {
                            player.StopRight = true;
                        }

                        if (player.LocationY < block.LocationY && player.CatDirection == 4) // Checking if player1 is intersecting the block from the top. This uses the tank's direction
                        {
                            player.StopDown = true;
                        }
                        break;
                    }

                }

                foreach (Block block in blocks) // Checking each block in blocks list
                {
                    blockRect = block.GetRect(); // Creating rectangle
                    blockRect.Intersect(playerRect); // Checking for intersections between player and block
                                                     // PlayerRect.Intersect(PlayerRect); between players
                    
                    if (!blockRect.IsEmpty && block.CanGoTrough == true && player.StopLeft == false && player.StopRight == false && player.StopUp == false && player.StopDown == false) // Slower speed while moving on magic block
                    {
                        player.Speed = 2;
                        break;                  
                    }
                    else { player.Speed = player.BaseSpeed; }
                }



                // Collision detection between blocks and enemies. Mostly identical to the player collision detection above
                foreach (Enemy enemy in enemies)
                {
                    enemy.StopDown = false;
                    enemy.StopLeft = false;
                    enemy.StopRight = false;
                    enemy.StopUp = false;

                    enemyRect = enemy.GetRect();

                    foreach (Block block in blocks)
                    {
                        blockRect = block.GetRect();
                        blockRect.Intersect(enemyRect);

                        if (!blockRect.IsEmpty && block.Goal == true) // Checking if enemy tank drives into goal
                        {
                            goalHit = true;
                            GameSounds(filePath = "GameOver.mp3");
                        }

                        if(!blockRect.IsEmpty && block.IsPowerUp == true)
                        {
                            break;
                        }

                        if (!blockRect.IsEmpty && block.CanGoTrough == false)
                        {
                            if (enemy.LocationX > block.LocationX && enemy.CatDirection == 1) // Checking if enemy is intersecting block from the right
                            {
                                enemy.StopLeft = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
                            }

                            if (enemy.LocationY > block.LocationY && enemy.CatDirection == 2) // Checking if enemy is intersecting block from the bottom
                            {
                                enemy.StopUp = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
                            }

                            if (enemy.LocationX < block.LocationX && enemy.CatDirection == 3) // Checking if enemy is intersecting block from the left
                            {
                                enemy.StopRight = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
                            }

                            if (enemy.LocationY < block.LocationY && enemy.CatDirection == 4) // Checking if enemy is intersecting block from the top
                            {
                                enemy.StopDown = true;
                                enemy.Move(random.Next(1, 5), random.Next(1, 3), random.Next(1, 3), random.Next(1, 9));
                            }
                            break;
                        }
                    }

                    foreach (Block block in blocks) // Checking each block in blocks list
                    {
                        blockRect = block.GetRect(); // Creating rectangle
                        blockRect.Intersect(enemyRect); // Checking for intersections between player and block

                        if (!blockRect.IsEmpty && block.CanGoTrough == true && enemy.StopLeft == false && enemy.StopRight == false && enemy.StopUp == false && enemy.StopDown == false) // Slower speed while moving on magic block
                        {
                            enemy.Speed = 2;
                            break;
                        }
                        else { enemy.Speed = player.BaseSpeed; }
                    }
                }

                foreach (Enemy enemy in enemies) // This is where the collision between players and enemies is detected
                {
                    enemyRect = enemy.GetRect();
                    enemyRect.Intersect(playerRect);

                    if (!enemyRect.IsEmpty && player.Invincible == false)
                    {
                        playerHit = true; // This is used to break out of the foreach player loop in the game loop if a player dies, preventing crashing
                        enemy.RemoveBullet(Canvas);
                        enemy.LoadMeowSound(meowSoundNumber = 7);
                        Canvas.Children.Remove(enemy);
                        enemies.Remove(enemy);
                        break;
                    }
                    else
                        playerHit = false;
                }
                if(playerHit == true)
                {
                    player.RemoveBullet(Canvas);
                    Canvas.Children.Remove(player);
                    player.LoadMeowSound(meowSoundNumber = 6);
                    player.ResetControls();
                    if (player.Player2 == false)
                    {
                        P1Dead = true;
                        P1Lives--;
                    }
                    else
                    {
                        P2Dead = true;
                        P2Lives--;
                    }
                    UpdatePoints();
                    players.Remove(player);
                    player.ResetControls();
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
                        // Creating rectangles for collision detection
                        blockRect = block.GetRect();
                        bulletRect = bullet.GetRect();
                        blockRect.Intersect(bulletRect); // Checking for intersection

                        if (!blockRect.IsEmpty && block.IsPowerUp == true)
                        {
                            break;
                        }
                        if (!blockRect.IsEmpty && block.CanDestroy == true) // If an intersection happens and the block can be destroyed
                        {
                            if(block.Goal == true)
                            {
                                break; // The player cannot destroy the goal block
                            }
                            // Removing bullet and block from canvas
                            player.RemoveBullet(Canvas);
                            Canvas.Children.Remove(block);
                            blocks.Remove(block);
                            player.Score += block.PointValue;
                            break;
                        }
                        else if (!blockRect.IsEmpty && block.CanDestroy == false && block.CanGoTrough == false) // if block can't be destroyed
                        {
                            player.RemoveBullet(Canvas); // only the bullet is removed
                            break;
                        }
                    }
                    foreach (Enemy enemy in enemies)
                    {
                        enemyRect = enemy.GetRect();
                        bulletRect = bullet.GetRect();
                        enemyRect.Intersect(bulletRect);

                        if (!enemyRect.IsEmpty)
                        {
                            player.RemoveBullet(Canvas);
                            Canvas.Children.Remove(enemy);
                            enemy.LoadMeowSound(meowSoundNumber = 7);
                            enemy.RemoveBullet(Canvas);
                            enemies.Remove(enemy);
                            player.Score += enemy.PointValue;
                            break;
                        }
                        foreach(Bullet enemybullet in enemy.bullets)
                        {
                            bulletRect = bullet.GetRect();
                            enemyBulletRect = enemybullet.GetRect();
                            bulletRect.Intersect(enemyBulletRect);
                            if (!bulletRect.IsEmpty)
                            {
                                player.RemoveBullet(Canvas);
                                enemy.RemoveBullet(Canvas);
                                break;
                            }
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
                        blockRect = block.GetRect(); // creating rectangle for block to use in collision detection
                        bulletRect = bullet.GetRect(); // creating rectangle for bullet to use in collision detection
                        blockRect.Intersect(bulletRect); // Checking if Block and bullet rects intersect

                        if (!blockRect.IsEmpty && block.IsPowerUp == true)
                        {
                            break;
                        }

                        if (!blockRect.IsEmpty && block.CanDestroy == true)
                        {
                            if (block.Goal == true) // The block that was hit was the goal block
                            {
                                GameSounds(filePath = "GameOver.mp3");
                                goalHit = true; // The GoalHit bool changes to true resulting in a game over
                            }
                            // Removing bullet and block
                            enemy.RemoveBullet(Canvas);
                            Canvas.Children.Remove(block);
                            blocks.Remove(block);
                            break;
                        }
                        else if (!blockRect.IsEmpty && block.CanDestroy == false && block.CanGoTrough == false) // If block.Candestroy is false
                        {
                            enemy.RemoveBullet(Canvas); // only the bullet is destroyed
                            break;
                        }

                    }
                    foreach (Player player in players) // Checking collision between enemy bullets and players
                    {
                        // Creating rectangles for collision detection
                        playerRect = player.GetRect();
                        bulletRect = bullet.GetRect();
                        playerRect.Intersect(bulletRect); // Checking for intersection

                        if (!playerRect.IsEmpty && player.Invincible == false) // If collision happens
                        {
                            enemy.RemoveBullet(Canvas);
                            Canvas.Children.Remove(player);
                            player.LoadMeowSound(meowSoundNumber = 6);
                            if (player.Player2 == false)
                            {
                                P1Dead = true; // P1Dead & P2Dead are used for the respawn feature
                                P1Lives--; // Taking one life away for dying
                            }
                            else
                            {
                                P2Dead = true;
                                P2Lives--;
                            }
                            UpdatePoints();
                            players.Remove(player);
                            player.ResetControls();
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

                    p1PreviousScore = player.Score; // the points are saved to the PreviousScore ints to keep points between levels and deaths
                    Player1ScoreTextBlock.Text = player.Score.ToString(); // Player score displayed in the PlayerScoreTextBlock
                    //Player1LivesTextBlock.Text = P1Lives.ToString(); // Player lives displayed in the PlayerLivesTextBlock

                    // Show and hide player lives 
                    switch (P1Lives)
                    {
                        case 0:
                            P1Live1.Opacity = 0;
                            break;
                        case 1:
                            P1Live2.Opacity = 0;
                            break;
                        case 2:
                            P1Live3.Opacity = 0;
                            break;
                        case 3:
                            P1Live1.Opacity = 100;
                            P1Live2.Opacity = 100;
                            P1Live3.Opacity = 100;
                            break;
                        default: break;
                    }
                }
                if (player.Player2 == true)
                {
                    p2PreviousScore = player.Score;
                    Player2ScoreTextBlock.Text = player.Score.ToString();

                    // Show and hide player lives 
                    switch (P2Lives)
                    {
                        case 0:
                            P2Live1.Opacity = 0;
                            break;
                        case 1:
                            P2Live2.Opacity = 0;
                            break;
                        case 2:
                            P2Live3.Opacity = 0;
                            break;
                        case 3:
                            P2Live1.Opacity = 100;
                            P2Live2.Opacity = 100;
                            P2Live3.Opacity = 100;
                            break;
                        default: break;
                    }
                }
            }
            enemyCountText.Text = enemies.Count.ToString();
        }

        //Checking if game is over
        private void CheckGameOver()
        {
            if (!players.Any()) // Checks if there are any player objects in the list
            {
                SavePoints();// Saves the points to file
                GameEndImage.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/Lose.jpg")); // Loads the "You Lose" image to GameEndImage
                GameEndImage.Visibility = Visibility.Visible; // Shows the GameEndImage
                NextLevelButton.Visibility = Visibility.Visible;
                GameRunning = false;
                dispatcherTimer.Stop(); // Stops the game
            }
            else if(goalHit == true)
            {
                SavePoints();// Saves the points to file
                GameEndImage.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/Lose.jpg")); // Loads the "You Lose" image to GameEndImage
                GameEndImage.Visibility = Visibility.Visible; // Shows the GameEndImage
                NextLevelButton.Visibility = Visibility.Visible;
                GameRunning = false;
                dispatcherTimer.Stop(); // Stops the game
            }
            else if (!enemies.Any())
            {
                SavePoints(); // Saves the points to file
                GameEndImage.Source = new BitmapImage(new Uri(this.BaseUri, "/Assets/Youwin_icon.png")); // Loads the "You Win" image to GameEndImage
                GameEndImage.Visibility = Visibility.Visible; // Shows the GameEndImage
                NextLevelButton.Visibility = Visibility.Visible;
                GameRunning = false;
                dispatcherTimer.Stop(); // Stops the game
            }
        }

        private void NextLevelButton_Click(object sender, RoutedEventArgs e) // This is the method that runs when the Next Level button is clicked on the GamePage
        {
            GameEndImage.Visibility = Visibility.Collapsed; // The Game end image gets hidden back
            //NextLevelButton.Visibility = Visibility.Collapsed;
            level.DestroyLevel(Canvas); // The current level is destroyed
            LevelNumber++; // The LevelNumber goes up by one so the next level is loaded
            level.LoadLevel(); // The Level is loaded from file
            level.BuildLevel(Canvas); // The level is built
            // Each player's score is carried over from the PreviousScore int
            foreach (Player player in players)
            {
                if (player.Player2 == false)
                {
                    player.Score = p1PreviousScore;
                }
                else if (player.Player2 == true)
                {
                    player.Score = p2PreviousScore;
                }
            }
            GameRunning = true;
            dispatcherTimer.Start(); // Game starts
        }

        // Method for reading lines from Highscore.dat
           private async void ReadScores()
            {
            //Create the file to hold the highscores
            HSFile = await storageFolder.CreateFileAsync("Highscores.dat", CreationCollisionOption.OpenIfExists);
            IList<string> readLines = await FileIO.ReadLinesAsync(HSFile);
            foreach (var line in readLines)
            {
                HSlines.Add(double.Parse(line));
            }
            Debug.Write(HSlines.Count);
        }

        private async void GameSounds(string filePath)
        {
            mediaElement = new MediaElement();
            mediaElement.Volume = BackgroundMediaPlayer.Current.Volume;
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file =
                await folder.GetFileAsync(filePath); // Game sounds
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream, file.ContentType);
            mediaElement.AutoPlay = true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (dispatcherTimer.IsEnabled)
            {
                PauseButton.Content = "Resume";
                BackgroundMediaPlayer.Current.Pause();
                GameRunning = false;
                dispatcherTimer.Stop();
            }
            else
            {
                PauseButton.Content = "Pause";
                BackgroundMediaPlayer.Current.Play();
                GameRunning = true;
                dispatcherTimer.Start();
            }
        }
        //Method for saving player 1 name
        private async void SaveName1()
        {
            HSNimet = await storageFolder.CreateFileAsync("Highscorenames.dat", CreationCollisionOption.ReplaceExisting);// the file that holds the name of player
            await FileIO.AppendTextAsync(HSNimet,MenuPage.P1Name);
        }
        //Method for saving player 2 name
        private async void SaveName2()
        {
            HSNimet = await storageFolder.CreateFileAsync("Highscorenames.dat", CreationCollisionOption.ReplaceExisting);// the file that holds the name of player
            await FileIO.AppendTextAsync(HSNimet, MenuPage.P2Name);
        }
        // Method for saving points to a file
        private async void SavePoints()
        {
            Debug.Write("TÄMÄ ON KUTSU");
            //Create the file to hold the data
            HSFile = await storageFolder.CreateFileAsync("Highscores.dat", CreationCollisionOption.ReplaceExisting);
            //Write data to file      
            foreach (Player player in players)
            {
                if (player.Player2 == false)
                {
                    //Creating the string to write
                    string Player1pisteet = Player1ScoreTextBlock.Text;
                    double player1points = double.Parse(Player1pisteet);
                    string Highscore = CurrentHS.Text; //Current highscore, hidden                   
                    double highscored = double.Parse(Highscore); //FIX THIS
                    if (player1points > highscored) // Comparing if the current score is bigger than the highest score
                    {
                        HSlines.Add(player1points);
                        HSlines.Sort();
                        HSlines.Reverse();
                        SaveName1();
                    }             
                }
                else if (player.Player2 == true)
                {
                    //Creating the string to write
                    string Player2pisteet = Player2ScoreTextBlock.Text;
                    double player2points = double.Parse(Player2pisteet);
                    string Highscore = CurrentHS.Text; //Current highscore, hidden
                    double highscored = double.Parse(Highscore); // FIX THIS
                    if (player2points > highscored) // Comparing if the current score is bigger than the highest score
                    {
                        HSlines.Add(player2points);
                        HSlines.Sort();
                        HSlines.Reverse();
                        SaveName2(); //Saves player 2 name 
                    }
                }         
              }
            int k = 0;
            foreach (double d in HSlines)
            {
                await FileIO.AppendTextAsync(HSFile, d + Environment.NewLine);
                k++;
                if (k >= 1) break; // only save the highest score
            }
        }
        private void RetryButton_Click(object sender, RoutedEventArgs e) // This is the method that runs when the retry button is clicked on the GamePage
        {
            GameEndImage.Visibility = Visibility.Collapsed; // The Game end picture is hidden
            goalHit = false; // GoalHit is restored back to false, so the game will continue even if the goal was hit
            // P1 and P2 lives are restored to 3
            P1Lives = 3;
            P2Lives = 3;
            // Show P1 and P2 lives
            P1Live1.Opacity = 100;
            P1Live2.Opacity = 100;
            P1Live3.Opacity = 100;
            if(MP == true)
            {
                P2Live1.Opacity = 100;
                P2Live2.Opacity = 100;
                P2Live3.Opacity = 100;
            }
            
            // P1 and P2 scores are restored to 0
            p1PreviousScore = 0;
            p2PreviousScore = 0;
            level.DestroyLevel(Canvas); // The level is destroyed
            level.LoadLevel(); // The same level is reloaded
            level.BuildLevel(Canvas); // The level is built
            GameRunning = true;
            dispatcherTimer.Start(); // Game starts
        }
        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            BackgroundMediaPlayer.Current.Volume = (double)VolumeSlider.Value / 100;
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            VolumeSlider.Value = 0;
            BackgroundMediaPlayer.Current.Volume = 0;
        }
        //This is here so you can compare it to the current score
        private async void ShowScores()
        {
            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile HSFile =
            await storageFolder.GetFileAsync("Highscores.dat");
            string Highscoretext = await Windows.Storage.FileIO.ReadTextAsync(HSFile);      
            if(Highscoretext=="")
            {
                CurrentHS.Text = "10";
            }
            else
            {
                CurrentHS.Text = Highscoretext; // Collapsed textblock that holds the current highest score 
            }           
        }
    }
}
