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
        public int Score { get; set; } // Player score
        public string Name { get; set; }
        public bool Invincible { get; set; }
        private int invincibleTimer;
        public int PlayerColour { get; set; }
        private int speedUp = 2; // Used for boosting player speed

        public Player()
        {
            this.InitializeComponent();
            // Setting up the key presses
            Window.Current.CoreWindow.KeyDown += onKeyDown;
            Window.Current.CoreWindow.KeyUp += onKeyUp;
        }
        // Controls volume of the pew sound
        public void SetVolume(double PewVolume)
        {
            Debug.WriteLine(PewVolume);
            base.mediaElement.Volume=PewVolume;           
        }

        // This method is used to draw the player on the canvas
        public void DrawPlayer()
        {
            if (PlayerColour == 1)
            {
                CatSpriteSheetOffset.Y = 0;
            }
            else if (PlayerColour == 2)
            {
                CatSpriteSheetOffset.Y = -55.5;
            }
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        // This method is used when a key is pressed on the keyboard
        public void onKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            // Button pressed for player 1
            // The bools left right down up are used in character_base to move the player on screen
            if (Player2 == false)
            {
                if (args.VirtualKey == VirtualKey.Left) // If pressing the left key, bool left is set to true
                {
                    left = true;
                    right = false;
                    down = false;
                    up = false;
                }
                else if (args.VirtualKey == VirtualKey.Right) // If pressing the right key, bool right is set to true
                {
                    right = true;
                    left = false;
                    down = false;
                    up = false;
                }
                else if (args.VirtualKey == VirtualKey.Up) // If pressing the up key, bool up is set to true
                {
                    up = true;
                    right = false;
                    down = false;
                    left = false;
                }
                else if (args.VirtualKey == VirtualKey.Down) // If pressing the down key, bool down is set to true
                {
                    down = true;
                    up = false;
                    left = false;
                    right = false;
                }
                if (args.VirtualKey == VirtualKey.Add && GamePage.dispatcherTimer.IsEnabled == true) // If pressing the shoot key, a bullet is created
                {
                    CreateBullet();
                }
            }
            // Controls for Player 2. Identical to player 1 except the buttons it uses are different
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
                if (args.VirtualKey == VirtualKey.J && GamePage.dispatcherTimer.IsEnabled == true)
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

        public void Invincibility()
        {
            invincibleTimer++;
            CatRectangle.Opacity = 0.4;
            if (invincibleTimer >= 80)
            {
                CatRectangle.Opacity = 1;
                Invincible = false;
            }
        }

        public void PowerUpSpeed()
        {
            speedUpTickCounter++;
            Speed = Speed + speedUp;
            if (speedUpTickCounter == 120)
            {
                speedUpTickCounter = 0;
                Speed = 4;
                SpeedUp = false;
            }
        }

        public void ResetControls() // when the level changes, the KeyDown and KeyUp methods have to be reset to prevent double button presses
        {
            Window.Current.CoreWindow.KeyDown -= onKeyDown;
            Window.Current.CoreWindow.KeyUp -= onKeyUp;
        }
    }
}
