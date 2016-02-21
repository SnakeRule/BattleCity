using System;
using System.Collections.Generic;
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
        protected int v { get; set; }

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
        }


        // Method when pressing down on a key
        public void onKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {

            // Moving the player
            if (args.VirtualKey == Windows.System.VirtualKey.Left) v = 1;
            else if (args.VirtualKey == Windows.System.VirtualKey.Right) v = 3;
            else if (args.VirtualKey == Windows.System.VirtualKey.Up) v = 2;
            else if (args.VirtualKey == Windows.System.VirtualKey.Down) v = 4;

        }

        // This is the method where the player is drawn on the screen each frame
        public void drawPlayer()
        {
            switch (v)
            {
                case 1:
                    {
                        Canvas.SetLeft(tankSprite, Canvas.GetLeft(tankSprite) - 6);
                        
                        break;
                    }
                case 2:
                    {
                        Canvas.SetTop(tankSprite, Canvas.GetTop(tankSprite) - 6);
                        break;
                    }
                case 3:
                    {
                        Canvas.SetLeft(tankSprite, Canvas.GetLeft(tankSprite) + 6);
                        break;
                    }
                case 4:
                    {
                        Canvas.SetTop(tankSprite, Canvas.GetTop(tankSprite) + 6);
                        break;
                    }
                case 5:
                    {
                        Canvas.SetLeft(tankSprite, Canvas.GetLeft(tankSprite));
                        break;
                    }
                case 6:
                    {
                        Canvas.SetLeft(tankSprite, Canvas.GetLeft(tankSprite));
                        break;
                    }
                case 7:
                    {
                        Canvas.SetLeft(tankSprite, Canvas.GetLeft(tankSprite));
                        break;
                    }
                case 8:
                    {
                        Canvas.SetTop(tankSprite, Canvas.GetTop(tankSprite));
                        break;
                    }
            }
        }

        // Method for releasing the keyboard press
        private void onKeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Left) v = 5;
            if (args.VirtualKey == Windows.System.VirtualKey.Up) v = 6;
            if (args.VirtualKey == Windows.System.VirtualKey.Right) v = 7;
            if (args.VirtualKey == Windows.System.VirtualKey.Down) v = 8;
        }


        public Image getPlayer()
        {
            return tankSprite;
        }
    }
}
