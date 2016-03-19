using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BattleCity
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private Player player;
        private Block block;

        private double CanvasWidth;
        private double CanvasHeight;
        private DispatcherTimer dispatcherTimer;

        public GamePage()
        {
            this.InitializeComponent();

            Canvas.Width = 650;
            Canvas.Height = 650;
            CanvasWidth = Canvas.Width;
            CanvasHeight = Canvas.Height;

            // Add player
            player = new Player { LocationX = 325, LocationY = 325 };
            Canvas.Children.Add(player);
            player.DrawPlayer();

            // Add Blocks
            block = new Block { LocationX = 65, LocationY = 65 };
            Canvas.Children.Add(block);
            block.drawMagic();
            block.UpdatePosition();

            int x = 0;
            for(int i = 0;i < 10; i++)
            {
                block = new Block { LocationX = x, LocationY = 0 };
                Canvas.Children.Add(block);
                block.drawDirt();
                block.UpdatePosition();
                x = x + 65;
            }
            

            // Setting up the timer that runs the Game method
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
            dispatcherTimer.Start();
        }

        public void Game(object sender, object e)
        {
            CollisionCheck();
            player.UpdatePlayer(Canvas);
        }

        // Back to mainmenu button method
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            // get root frame (which show pages)
            Frame rootFrame = Window.Current.Content as Frame;
            // did we get it correctly
            if (rootFrame == null) return;
            // navigate back if possible
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void CollisionCheck()
        {
            Rect Player1Rect = new Rect(player.LocationX, player.LocationY, player.ActualWidth, player.ActualHeight);
            Rect BlockRect = new Rect(block.LocationX, block.LocationY, block.ActualWidth, block.ActualHeight);

            BlockRect.Intersect(Player1Rect);

            if (!BlockRect.IsEmpty)
            {
                Canvas.Children.Remove(block);
            }
        }
    }
}
