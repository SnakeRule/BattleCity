using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace BattleCity
{
    class Level
    {
        // Introducing the objects used
        private Player player1; // Player 1
        private Player player2; // Player 2
        private Enemy enemy; // Enemy
        private Block block1; // Stone
        private Block block2; // Tile
        private Block block3; // Magic
        private Block block4; // Speed up
        private Block block5; // Power up
        private Block block6; // Starpower 1
        private Block block7; // Starpower 2
        private Block goal; // Goal
        public static double GoalLocationX { get; set; } // Goal location X on canvas
        public static double GoalLocationY { get; set; } // Goal location Y on canvas
        private int P1SpawnX, P1SpawnY, P2SpawnX, P2SpawnY; // These integers are used for saving the player spawn points during level building. Used for respawning

        private string LevelData; // String for current leveldata

        public List<Block> blocks = new List<Block>(); // All blocks
        public List<Player> players = new List<Player>(); // All players
        public List<Enemy> enemies = new List<Enemy>(); // All enemies

        public Canvas canvas { get; set; } // Get canvas here
        StreamReader reader;

        public void level()
        {
           
        }

        public async void LoadLevel()
        {
            // create or open local file
            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                using (StreamReader reader = File.OpenText(@"Levels\Level" + GamePage.LevelNumber + ".txt"))
                {
                    Debug.WriteLine("Opened File");
                    LevelData = await reader.ReadToEndAsync();
                }
            }
            catch (Exception)
            {
                var errormessage = new Windows.UI.Popups.MessageDialog("Level not found! Reloading previous level!");
                await errormessage.ShowAsync();
                GamePage.LevelNumber--;
                
                using (StreamReader reader = File.OpenText(@"Levels\Level" + GamePage.LevelNumber + ".txt"))
                {
                    Debug.WriteLine("Opened File");
                    LevelData = await reader.ReadToEndAsync();
                }
            }
            finally
            {
                if(reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        public void RespawnPlayer1(Canvas canvas, int OldScore) // This method respawns a dead player and keeps the player's score with the OldScore int
        {
            player1 = new Player { LocationX = P1SpawnX, LocationY = P1SpawnY, Player2 = false, canvas = canvas, CatDirection = 2, Score = OldScore };
            GamePage.P1Dead = false; // Sets the player to alive state
            canvas.Children.Add(player1);
            player1.DrawPlayer();
            players.Add(player1);
        }

        public void RespawnPlayer2(Canvas canvas, int OldScore)
        {
            player2 = new Player { LocationX = P2SpawnX, LocationY = P2SpawnY, Player2 = true, canvas = canvas, CatDirection = 2, Score = OldScore };
            GamePage.P2Dead = false; // Sets the player to alive state
            canvas.Children.Add(player2);
            player2.DrawPlayer();
            players.Add(player2);
        }

        public void CreatePlayer1(Canvas canvas, int col, int row) // This method creates Player 1. It gets location data from the levelbuilder values col and row
        {
            player1 = new Player { LocationX = col + 2, LocationY = row, Player2 = false, canvas = canvas, CatDirection = 2 };
            GamePage.P1Dead = false; // Sets the player to alive state
            P1SpawnX = col + 2; // The spawn location data is saved to the P1spawn ints for respawning
            P1SpawnY = row;
            canvas.Children.Add(player1);
            player1.DrawPlayer();
            players.Add(player1);
        }

        public void CreatePlayer2(Canvas canvas, int col, int row)
        {
            player2 = new Player { LocationX = col + 2, LocationY = row, Player2 = true, canvas = canvas, CatDirection = 2 };
            GamePage.P2Dead = false; // Sets the player to alive state
            P2SpawnX = col + 2; // The spawn location data is saved to the P1spawn ints for respawning
            P2SpawnY = row;
            canvas.Children.Add(player2);
            player2.DrawPlayer();
            players.Add(player2);
        }

        public void BuildLevel(Canvas canvas)
        {

            int columns = 17; // Local int for map columns (map 17x17)
            int rows = 17; // Local int for map rows
            int row = 0; // Local int for calculating current row
            int col = 0; // Local int for calculatin current column
            int x = 0; // Local int for level data list 
            int c = 0; // Local int for column loop
            int r = 0; // Local int for row loop

            // Converts Level data to List
            List<int> CurrentLevelData = new List<int>(LevelData.Split(',').Select(int.Parse).ToList());

            // Loop circus for setting blocks to right location
            for(r = 0; r < rows; r++) // Loop for setting blocks to right row
            {
                for (c = 0; c < columns; c++) // Loop for setting blocks to right column
                {
                    switch (CurrentLevelData[x]) // Switch - case to check what block has to be drawn
                    {                          
                        case 0:
                            x++;
                            break;
                        case 1: // Stone
                            col = c * 40;
                            block1 = new Block { LocationX = col, LocationY = row };
                            blocks.Add(block1);
                            canvas.Children.Add(block1);
                            block1.drawStone(); // canGoTrough = false, canDestroy = false
                            block1.UpdatePosition();
                            x++;
                            break;
                        case 2: // Tile
                            col = c * 40;
                            block2 = new Block { LocationX = col, LocationY = row };
                            blocks.Add(block2);
                            canvas.Children.Add(block2);
                            block2.drawTile(); // canGoTrough = false, canDestroy = true
                            block2.UpdatePosition();
                            x++;
                            break;
                        case 3: // Magic
                            col = c * 40;
                            block3 = new Block { LocationX = col, LocationY = row };
                            blocks.Add(block3);
                            canvas.Children.Add(block3);
                            block3.drawMagic(); // canGoTrough = true, canDestroy = false
                            block3.UpdatePosition();
                            x++;
                            break;
                        case 4: // Goal
                            col = c * 40;
                            goal = new Block { LocationX = col, LocationY = row };
                            GoalLocationX = goal.LocationX;
                            GoalLocationY = goal.LocationY;
                            blocks.Add(goal);
                            canvas.Children.Add(goal);
                            goal.drawGoal(); // CanGoThrough = false, canDestroy = true
                            goal.UpdatePosition();
                            x++;
                            break;
                        case 5: // Speed up
                            col = c * 40;
                            block4 = new Block { LocationX = col, LocationY = row };
                            blocks.Add(block4);
                            canvas.Children.Add(block4);
                            block4.drawSpeedUp(); // canGoTrough = true, canDestroy = false
                            block4.UpdatePosition();
                            x++;
                            break;
                        case 6:
                            // Add enemies
                            col = c * 40;
                            enemy = new Enemy { LocationX = col, LocationY = row, canvas = canvas, CatDirection = 2, };
                            canvas.Children.Add(enemy);
                            enemy.DrawPlayer();
                            enemies.Add(enemy);
                            x++;
                            break;
                        case 7:
                            // Add player
                            if (GamePage.P1Lives > 0)
                            {
                                col = c * 40;
                                CreatePlayer1(canvas, col, row);
                                x++;
                            }
                            else
                                x++;
                            break;
                        case 8:
                            if (GamePage.MP == true && GamePage.P2Lives > 0)
                            {
                                col = c * 40;
                                CreatePlayer2(canvas, col, row);
                                x++;
                            }
                            else
                            {
                                x++;
                            }          
                            break;
                        case 9: // Power up
                            col = c * 40;
                            block5 = new Block { LocationX = col, LocationY = row };
                            blocks.Add(block5);
                            canvas.Children.Add(block5);
                            block5.drawPowerUp(); // canGoTrough = true, canDestroy = false
                            block5.UpdatePosition();
                            x++;
                            break;
                        case 10: // Starpower 1
                            col = c * 40;
                            block6 = new Block { LocationX = col, LocationY = row };
                            blocks.Add(block6);
                            canvas.Children.Add(block6);
                            block6.drawStarPower1(); // canGoTrough = true, canDestroy = false
                            block6.UpdatePosition();
                            x++;
                            break;
                        case 11: // Starpower 2
                            col = c * 40;
                            block7 = new Block { LocationX = col, LocationY = row };
                            blocks.Add(block7);
                            canvas.Children.Add(block7);
                            block7.drawStarPower2(); // canGoTrough = true, canDestroy = false
                            block7.UpdatePosition();
                            x++;
                            break;
                        default: // If level data contains any wrong numbers loop continues to build level anyways by adding 1 to int x
                            x++;
                            break;
                    }
                }
                col = 0; // Resets columns
                c = 0; // Resets c int so column loop starts again at next row
                row += 40; // Moves to next row
            }
            foreach (Block block in blocks) // Sets blocks z index to -1 so players moves top of them
            {
                Canvas.SetZIndex(block, -1);
            }
            foreach (Player player in players) // Sets players z index to 0
            {
                Canvas.SetZIndex(player, 0);
            }
        }

        public void DestroyLevel(Canvas canvas) // This method is used for Destroying the level that was in use before loading another level
        {
            foreach(Player player in players) // Removing players and their bullets
            {
                player.ResetControls();
                player.RemoveBullet(canvas);
                player.bullets.Clear();
                canvas.Children.Remove(player);
            }
            foreach(Enemy enemy in enemies) // Removing players and their bullets
            {
                enemy.RemoveBullet(canvas);
                enemy.bullets.Clear();
                canvas.Children.Remove(enemy);
            }
            foreach(Block block in blocks) // Removing blocks
            {
                canvas.Children.Remove(block);
            }
            players.Clear();
            enemies.Clear();
            blocks.Clear();
        }

    }
}
