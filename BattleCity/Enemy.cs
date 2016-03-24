using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

/// <summary>
/// This is the enemy class. It inherits most properties and methods from the Character_base class.
/// Functions and properties specific to enemies can be found here
/// </summary>
namespace BattleCity
{
    class Enemy : Character_base
    {
        private int tickCounter;

        public Enemy()
        {
            this.InitializeComponent();
        }

        // This method is used to draw the enemy on the canvas
        public void DrawPlayer()
        {
            TankSpriteSheetOffset.Y = -86;
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        // This is the method that decides which direction the enemy should move. Maybe one day we'll have some proper AI calculations here. RNG at the moment
        public void Move(int tankDirection)
        {
            tickCounter++;

            if (tickCounter >= 30 || LocationX <= 0 || LocationX >= canvas.Width - ActualWidth || LocationY <= 0 || LocationY >= canvas.Height - ActualHeight)
            {
                left = false;
                right = false;
                up = false;
                down = false;

                switch (tankDirection)
                {
                    case 1:
                        left = true;
                        CreateBullet();
                        break;
                    case 2:
                        up = true;
                        CreateBullet();
                        break;
                    case 3:
                        right = true;
                        CreateBullet();
                        break;
                    case 4:
                        down = true;
                        CreateBullet();
                        break;
                }
                tickCounter = 0;
            }
        }
    }
}
