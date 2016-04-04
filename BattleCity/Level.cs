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
        private Player player1;
        private Player player2;
        private Enemy enemy;
        private Block block1;
        private Block block2;
        private Block block3;
        public Block goal;
        public static double GoalLocationX { get; set; }
        public static double GoalLocationY { get; set; }

        private Windows.Storage.StorageFile levelFile;

        public List<Block> blocks = new List<Block>(); // All blocks
        public List<Player> players = new List<Player>();
        public List<Enemy> enemies = new List<Enemy>();

        public Canvas canvas { get; set; }

        public void level()
        {
           
        }

        public void Level1(Canvas canvas)
        {
            blocks.Clear();
            enemies.Clear();
            players.Clear();

            // Add Blocks 
            int m = 0;
            for (int i = 0; i < 17; i++)
            {
                block2 = new Block { LocationX = m, LocationY = 65 };
                blocks.Add(block2);
                canvas.Children.Add(block2);
                block2.drawMagic(); // canGoTrough = true, canDestroy = false
                block2.UpdatePosition();
                m = m + 40;
            }

            block3 = new Block { LocationX = 165, LocationY = 165 };
            blocks.Add(block3);
            canvas.Children.Add(block3);
            block3.drawStone(); // canGoTrough = false, canDestroy = false
            block3.UpdatePosition();

            int x = 0;
            for (int i = 0; i < 17; i++)
            {
                block1 = new Block { LocationX = x, LocationY = 425 };
                blocks.Add(block1);
                canvas.Children.Add(block1);
                block1.drawTile(); // canGoTrough = false, canDestroy = true
                block1.UpdatePosition();
                x = x + 40;
            }

            // Add Goal
            goal = new Block { LocationX = (680 / 2), LocationY = (680 - 40) };
            blocks.Add(goal);
            canvas.Children.Add(goal);
            goal.drawGoal(); // CanGoThrough = false, canDestroy = true
            goal.UpdatePosition();
            GoalLocationX = goal.LocationX;
            GoalLocationY = goal.LocationY;

            // Add player
            player1 = new Player { LocationX = 425, LocationY = 525, Player2 = false, canvas = canvas, tankDirection = 2 };
            canvas.Children.Add(player1);
            player1.DrawPlayer();
            players.Add(player1);

            // Adding enemies
            int xx = 125;
            for (int i = 0; i < 5; i++)
            {
                enemy = new Enemy { LocationX = xx, LocationY = 125, canvas = canvas, tankDirection = 2 };
                canvas.Children.Add(enemy);
                enemy.DrawPlayer();
                enemies.Add(enemy);
                xx += 125;
            }
        }
        

        /*public async void Level2(Canvas canvas)
        {
            // create or open local file
            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            levelFile =
                await storageFolder.CreateFileAsync(@"Levels\Level1.txt", Windows.Storage.CreationCollisionOption.OpenIfExists);

            // read and display file content
            Debug.WriteLine("ASDDDDDDDDDDDDDDDDSAÖLFÖSALFÖSAFAÖÄFSLLFÄA");
            string text = await Windows.Storage.FileIO.ReadTextAsync(levelFile);
            Debug.WriteLine("Tiedosto: " + text);

        }
        */

        public void CreatePlayer2(Canvas canvas)
        {
            player2 = new Player { LocationX = 225, LocationY = 525, Player2 = true, canvas = canvas, tankDirection = 3 };
            canvas.Children.Add(player2);
            player2.DrawPlayer();
            players.Add(player2);
            //Player2Score.Text = "";
        }
    }
}
