﻿using System;
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
        private Player player1;
        private Player player2;
        private Enemy enemy;
        private Block block1; // Stone
        private Block block2; // Tile
        private Block block3; // Magic
        private Block block4; // Speed up
        private Block block5; // Power up
        private Block block6; // Starpower 1
        private Block block7; // Starpower 2
        public Block goal;
        public static double GoalLocationX { get; set; }
        public static double GoalLocationY { get; set; }

        private Windows.Storage.StorageFile levelFile;
        private string LevelData;

        public List<Block> blocks = new List<Block>(); // All blocks
        public List<Player> players = new List<Player>();
        public List<Enemy> enemies = new List<Enemy>();

        public Canvas canvas { get; set; }
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

        public void CreatePlayer2(Canvas canvas, int col, int row)
        {
                player2 = new Player { LocationX = col, LocationY = row, Player2 = true, canvas = canvas, tankDirection = 2 };
                canvas.Children.Add(player2);
                player2.DrawPlayer();
                players.Add(player2);
        }

        public void BuildLevel(Canvas canvas)
        {

            int columns = 17;
            int rows = 17;
            int row = 0;
            int col = 0;
            int x = 0;
            int c = 0;

            // Convert Level data to List
            List<int> CurrentLevelData = new List<int>(LevelData.Split(',').Select(int.Parse).ToList());


            for(int r = 0; r < rows; r++)
            {
                for (c = 0; c < columns; c++)
                {
                    switch (CurrentLevelData[x])
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
                            enemy = new Enemy { LocationX = col, LocationY = row, canvas = canvas, tankDirection = 2, };
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
                                player1 = new Player { LocationX = col, LocationY = row, Player2 = false, canvas = canvas, tankDirection = 2 };
                                canvas.Children.Add(player1);
                                player1.DrawPlayer();
                                players.Add(player1);
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
                        default: break;
                    }
                }
                col = 0;
                c = 0;
                row += 40;
            }
            foreach (Block block in blocks)
            {
                Canvas.SetZIndex(block, -1);
            }
            foreach (Player player in players)
            {
                Canvas.SetZIndex(player, 0);
            }
            //Debug.WriteLine("LISTA" + k);
        }

        public void DestroyLevel(Canvas canvas)
        {
            foreach(Player player in players)
            {
                player.ResetControls();
                player.RemoveBullet(canvas);
                player.bullets.Clear();
                canvas.Children.Remove(player);
            }
            foreach(Enemy enemy in enemies)
            {
                enemy.RemoveBullet(canvas);
                enemy.bullets.Clear();
                canvas.Children.Remove(enemy);
            }
            foreach(Block block in blocks)
            {
                canvas.Children.Remove(block);
            }
            players.Clear();
            enemies.Clear();
            blocks.Clear();
        }

    }
}
