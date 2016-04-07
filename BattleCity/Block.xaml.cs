using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BattleCity
{
    public sealed partial class Block : UserControl
    {
        private readonly int pointValue = 5; // How many points from destroying block
        public bool CanDestroy; // Tells if block is destroyable
        public bool CanGoTrough; // Tells if player can move trough block
        public bool IsPowerUp; // Tells if block is power up
        public bool Goal; // Tells if block is goal

        public int PowerUpType;

        public double LocationX { get; set; } // Blocks location X on canvas
        public double LocationY { get; set; } // Blocks location Y on canvas
        public int PointValue // Points from destroying a block
        {
            get
            {
                return pointValue; // Returns pointValue
            }
        }

        public Block() // Constructor for block class
        {
            this.InitializeComponent();
        }

        public void drawTile() // Method to draw Tile blocks
        {            
            BlockSpriteSheetOffset.Y = 0; // Sets sprite sheet offset
            // Sets CanDestroy and CanGoThrough bools for block
            CanDestroy = true;
            CanGoTrough = false;
        }

        public void drawMagic() // Method to draw Magic blocks
        {
            BlockSpriteSheetOffset.Y = -40; // Sets sprite sheet offset
            // Sets CanDestroy and CanGoThrough bools for block
            CanDestroy = false;
            CanGoTrough = true;
        }

        public void drawStone() // Method to draw Stone blocks
        {
            BlockSpriteSheetOffset.Y = -(2 * 40); // Sets sprite sheet offset
            // Sets CanDestroy and CanGoThrough bools for block
            CanDestroy = false;
            CanGoTrough = false;
        }

        public void drawSpeedUp() // Method to draw Speed up blocks
        {
            BlockSpriteSheetOffset.Y = -(4 * 40); // Sets sprite sheet offset
            // Sets IsPowerUp bool and PowerUpType
            IsPowerUp = true;
            PowerUpType = 1;
        }
        public void drawPowerUp() // Method to draw Power up blocks
        {
            BlockSpriteSheetOffset.Y = -(5 * 40); // Sets sprite sheet offset
            // Sets IsPowerUp bool and PowerUpType
            IsPowerUp = true;
            PowerUpType = 2;
        }
        public void drawStarPower1() // Method to draw StarPower blocks
        {
            BlockSpriteSheetOffset.Y = -(6 * 40); // Sets sprite sheet offset
            // Sets IsPowerUp bool and PowerUpType
            IsPowerUp = true;
            PowerUpType = 3;
        }
        public void drawStarPower2() // Method to draw Star power blocks
        {
            BlockSpriteSheetOffset.Y = -(7 * 40); // Sets sprite sheet offset
            // Sets IsPowerUp bool and PowerUpType
            IsPowerUp = true;
            PowerUpType = 4;
        }

        public void drawGoal()
        {
            BlockSpriteSheetOffset.Y = -(3 * 40); // Sets sprite sheet offset
            // Sets CanDestroy and CanGoThrough bools for block
            CanDestroy = true;
            CanGoTrough = false;
            Goal = true; // Sets goal bool
        }

        public void UpdatePosition() // Method for updating blocks positions
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        public Rect GetRect() // Method for getting Rectangles for blocks
        {
            return new Rect(LocationX, LocationY, ActualWidth, ActualHeight);
        }
    }
}
