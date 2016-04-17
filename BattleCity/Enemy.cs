using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int tickCounter; // Used by the Move method to move the enemy in certain intervals
        private readonly int pointValue = 20; // How many points from destroying enemy
        public int PointValue
        {
            get
            {
                return pointValue;
            }
        }

        public Enemy()
        {
            this.InitializeComponent();
        }

        // This method is used to draw the enemy on the canvas
        public void DrawPlayer()
        {
            CatSpriteSheetOffset.Y = -112; // setting the enemy picture from the character base xaml
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        // This is the method that decides which direction the enemy should move. It sometimes moves by random and sometimes towards the goal depending on multiple randomly generated values
        // EnemyRandomDirection = Determines which way the enemy moves if a random movement is performed
        // EnemyFocus = Determines if the enemy moves by random or towards the goal
        // EnemyShoot = Determines if the enemy shoots or not
        public void Move(int EnemyRandomDirection, int EnemyFocus, int EnemyKnownDirection, int EnemyShoot)
        {
            tickCounter++;

            if ((EnemyFocus == 1 || EnemyFocus == 2) && tickCounter >= 10) // This is where the random enemy movement happens
            {
                Debug.WriteLine("RANDOM");
                left = false;
                right = false;
                up = false;
                down = false;

                switch (EnemyRandomDirection)
                {
                    case 1:
                        left = true;
                        break;
                    case 2:
                        up = true;
                        break;
                    case 3:
                        right = true;
                        break;
                    case 4:
                        down = true;
                        break;
                }
                tickCounter = 0;
            }
            // If the enemy is above and to the left of the goal, the enemy moves down or right
            if((EnemyFocus == 3 || EnemyFocus == 4 || EnemyFocus == 5) && (tickCounter >= 15 && LocationX <= Level.GoalLocationX && LocationY <= Level.GoalLocationY))
            {
                Debug.WriteLine("DOWN OR RIGHT");
                left = false;
                right = false;
                up = false;
                down = false;

                switch (EnemyKnownDirection)
                {
                    case 1:
                        right = true;
                        break;
                    case 2:
                        down = true;
                        break;
                }
                tickCounter = 0;
            }
            // If the enemy is above and to the right of the goal, the enemy moves down or left
            if ((EnemyFocus == 3 || EnemyFocus == 4 || EnemyFocus == 5) && tickCounter >= 15 && LocationX >= Level.GoalLocationX && LocationY <= Level.GoalLocationY)
            {
                Debug.WriteLine("DOWN OR LEFT");
                left = false;
                right = false;
                up = false;
                down = false;

                switch (EnemyKnownDirection)
                {
                    case 1:
                        left = true;
                        CreateBullet();
                        break;
                    case 2:
                        down = true;
                        CreateBullet();
                        break;
                }
                tickCounter = 0;
            }
            // If the enemy is below and to the right of the goal, the enemy moves up or left
            if ((EnemyFocus == 3 || EnemyFocus == 4 || EnemyFocus == 5) && tickCounter >= 15 && LocationX >= Level.GoalLocationX && LocationY >= Level.GoalLocationY)
            {
                Debug.WriteLine("DOWN OR LEFT");
                left = false;
                right = false;
                up = false;
                down = false;

                switch (EnemyKnownDirection)
                {
                    case 1:
                        left = true;
                        break;
                    case 2:
                        up = true;
                        break;
                }
                tickCounter = 0;
            }
            // If the enemy is below and to the left of the goal, the enemy moves up or right
            if ((EnemyFocus == 3 || EnemyFocus == 4 || EnemyFocus == 5) && tickCounter >= 15 && LocationX <= Level.GoalLocationX && LocationY >= Level.GoalLocationY)
            {
                Debug.WriteLine("DOWN OR LEFT");
                left = false;
                right = false;
                up = false;
                down = false;

                switch (EnemyKnownDirection)
                {
                    case 1:
                        right = true;
                        break;
                    case 2:
                        up = true;
                        break;
                }
                tickCounter = 0;
            }
            // If the randomly generated int EnemyShoot is 5 and tickCounter is more than 4, a bullet is created by the enemy
            if(EnemyShoot == 5 && tickCounter >= 5)
            {
                CreateBullet();
            }
        }
    }
}
