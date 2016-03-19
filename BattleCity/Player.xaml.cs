using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BattleCity
{
    public sealed partial class Player : UserControl
    {
        private int v { get; set; }
        private int speed = 6;
        private int tankDirection { get; set; }

        public double LocationX { get; set; }
        public double LocationY { get; set; }

        private bool left;
        private bool right;
        private bool up;
        private bool down;

        // Default constructor for player
        public Player()
        {
            this.InitializeComponent();
            // Setting up the key presses
            Window.Current.CoreWindow.KeyDown += onKeyDown;
            Window.Current.CoreWindow.KeyUp += onKeyUp;
        }


        // Method when pressing down on a key
        public void onKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            // Moving the player
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

        }

        public void DrawPlayer()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }

        // This is the method where the player is drawn on the screen each frame
        public void UpdatePlayer(Canvas canvas)
        {
            // these set the tanksprite to the canvas. The position is calculated from the tanksprite's current position and added or decreased speed
            if (left == true && LocationX >= 2)
            {
                PlayerRotate.Angle = 180;
                SetValue(Canvas.LeftProperty, LocationX -= speed);

                tankDirection = 1;
            }
            if (up == true && LocationY >= 2)
            {
                PlayerRotate.Angle = 270;
                SetValue(Canvas.TopProperty, LocationY -= speed);
                tankDirection = 2;
            }
            if (right == true && LocationX <= (canvas.ActualWidth - tankRectangle.Width))
            {
                PlayerRotate.Angle = 0;
                SetValue(Canvas.LeftProperty, LocationX += speed);
                tankDirection = 3;
            }
            if (down == true && LocationY <= (canvas.ActualHeight - tankRectangle.Height))
            {
                PlayerRotate.Angle = 90;
                SetValue(Canvas.TopProperty, LocationY += speed);
                tankDirection = 4;
            }
        }

        // Method for releasing the keyboard press
        private void onKeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
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
    }
}
