using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


/// <summary>
/// This is the player class. It inherits most properties and methods from the Character_base class.
/// Functions and properties specific to players can be found here
/// </summary>
namespace BattleCity
{
    class Player : Character_base
    {
        public bool Player2 { get; set; } // Tells which player is being used
        public int score { get; set; }

        public Player()
        {
            this.InitializeComponent();
            // Setting up the key presses
            Window.Current.CoreWindow.KeyDown += onKeyDown;
            Window.Current.CoreWindow.KeyUp += onKeyUp;
        }


        // This method is used to draw the player on the canvas
        public void DrawPlayer()
        {
            if (Player2 == false)
            {
                TankSpriteSheetOffset.Y = 0;
            }
            else if (Player2 == true)
            {
                TankSpriteSheetOffset.Y = -43;
            }
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        // Method when pressing down on a key
        public void onKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            // Moving the player
            if (Player2 == false)
            {
                if (args.VirtualKey == VirtualKey.Left)
                {
                    left = true;
                    right = false;
                    down = false;
                    up = false;
                }
                else if (args.VirtualKey == VirtualKey.Right)
                {
                    right = true;
                    left = false;
                    down = false;
                    up = false;
                }
                else if (args.VirtualKey == VirtualKey.Up)
                {
                    up = true;
                    right = false;
                    down = false;
                    left = false;
                }
                else if (args.VirtualKey == VirtualKey.Down)
                {
                    down = true;
                    up = false;
                    left = false;
                    right = false;
                }

                if (args.VirtualKey == VirtualKey.Add)
                {
                    CreateBullet();
                }
            }
            else if (Player2 == true)
            {
                if (args.VirtualKey == VirtualKey.A)
                {
                    left = true;
                    right = false;
                    down = false;
                    up = false;
                }
                else if (args.VirtualKey == VirtualKey.D)
                {
                    right = true;
                    left = false;
                    down = false;
                    up = false;
                }
                else if (args.VirtualKey == VirtualKey.W)
                {
                    up = true;
                    right = false;
                    down = false;
                    left = false;
                }
                else if (args.VirtualKey == VirtualKey.S)
                {
                    down = true;
                    up = false;
                    left = false;
                    right = false;
                }
                if (args.VirtualKey == VirtualKey.J)
                {
                    CreateBullet();
                }
            }
        }
        // Method for releasing the keyboard press
        private void onKeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (Player2 == false)
            {
                if (left == true && args.VirtualKey == VirtualKey.Left) // checks is bool left is true and if left button is actually pressed
                {
                    Debug.WriteLine("BEEP-BOOP");
                    left = false;
                }
                if (up == true && args.VirtualKey == VirtualKey.Up) // checks is bool up is true and if up button is actually pressed
                {
                    up = false;
                }
                if (right == true && args.VirtualKey == VirtualKey.Right) // checks is bool right is true and if right button is actually pressed
                {
                    right = false;
                }
                if (down == true && args.VirtualKey == VirtualKey.Down) // checks is bool down is true and if down button is actually pressed
                {
                    down = false;
                }
            }

            if (Player2 == true)
            {
                if (left == true && args.VirtualKey == VirtualKey.A) // checks is bool left is true and if left button is actually pressed
                {
                    Debug.WriteLine("BEEP-BOOP");
                    left = false;
                }
                if (up == true && args.VirtualKey == VirtualKey.W) // checks is bool up is true and if up button is actually pressed
                {
                    up = false;
                }
                if (right == true && args.VirtualKey == VirtualKey.D) // checks is bool right is true and if right button is actually pressed
                {
                    right = false;
                }
                if (down == true && args.VirtualKey == VirtualKey.S) // checks is bool down is true and if down button is actually pressed
                {
                    down = false;
                }
            }
        }

    }
}
