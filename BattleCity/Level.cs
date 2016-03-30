using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace BattleCity
{
    class Level
    {
        private Block block1;
        private Block block2;
        private Block block3;
        public Block goal;

        public List<Block> blocks = new List<Block>(); // All blocks    

        public Canvas canvas { get; set; }

        public void level()
        {
           
        }

        public void Level1(Canvas canvas)
        {
            blocks.Clear();
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
        }
    }
}
