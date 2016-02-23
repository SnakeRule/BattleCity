using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace BattleCity
{
    public class Player
    {
        Image tankSprite;
        private int v { get; set; }
        private int speed { get; set; }
        private int tankDirection { get; set; }

        private bool left;
        private bool right;
        private bool up;
        private bool down;

        // Default constructor for player
        public Player(Canvas canvas)
        {
            // Setting up the key presses
            Window.Current.CoreWindow.KeyDown += onKeyDown;
            Window.Current.CoreWindow.KeyUp += onKeyUp;

            // Setting up the player sprite
            tankSprite = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("ms-appx:///Assets/tank.png");
            bitmapImage.UriSource = uri;
            tankSprite.Source = bitmapImage;
            tankSprite.Width = 400;
            tankSprite.Height = 400;

            // Setting player position on canvas
            Canvas.SetLeft(tankSprite, 600);
            Canvas.SetTop(tankSprite, 300);

            // Adding sprite to canvas
            canvas.Children.Add(tankSprite);

            //Setting tank speed
            speed = 6;
        }


        // Method when pressing down on a key
        public void onKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            // Moving the player
            if (args.VirtualKey == Windows.System.VirtualKey.Left)
            {
                left = true;
                right = false;
                down = false;
                up = false;
            } 
            else if (args.VirtualKey == Windows.System.VirtualKey.Right)
            {
                right = true;
                left = false;
                down = false;
                up = false;
            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Up)
            {
                up = true;
                right = false;
                down = false;
                left = false;
            }
            else if (args.VirtualKey == Windows.System.VirtualKey.Down)
            {
                down = true;
                up = false;
                left = false;
                right = false;
            }

        }

        // This is the method where the player is drawn on the screen each frame
        public void drawPlayer()
        {
            // these set the tanksprite to the canvas. The position is calculated from the tanksprite's current position and added or decreased speed
            if (left == true)
            {
                Canvas.SetLeft(tankSprite, Canvas.GetLeft(tankSprite) - speed);
                tankDirection = 1;
            }
            if (up == true)
            {
                Canvas.SetTop(tankSprite, Canvas.GetTop(tankSprite) - speed);
                tankDirection = 2;
            }
            if(right == true)
            {
                Canvas.SetLeft(tankSprite, Canvas.GetLeft(tankSprite) + speed);
                tankDirection = 3;
            }
            if(down == true)
            {
                Canvas.SetTop(tankSprite, Canvas.GetTop(tankSprite) + speed);
                tankDirection = 4;
            }
        }

        // Method for releasing the keyboard press
        private void onKeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (left == true && args.VirtualKey == Windows.System.VirtualKey.Left) // checks is bool left is true and if left button is actually pressed
            {
                left = false;
            }
            if (up == true && args.VirtualKey == Windows.System.VirtualKey.Up) // checks is bool up is true and if up button is actually pressed
            {
                up = false;
            }
            if (right == true && args.VirtualKey == Windows.System.VirtualKey.Right) // checks is bool right is true and if right button is actually pressed
            {
                right = false;
            }
            if(down == true && args.VirtualKey == Windows.System.VirtualKey.Down) // checks is bool down is true and if down button is actually pressed
            {
                down = false;
            }
        }


        public Image getPlayer()
        {
            return tankSprite;
        }
    }
}
