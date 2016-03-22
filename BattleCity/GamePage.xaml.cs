using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
        // Introducing the objects used
        private Player player1;
        private Player player2;
        private Block block;
        private Bullet bullet;

        private bool MP; // Bool used for checking if 2-player mode was selected

        // These rectangles are used as hitboxes
        private Rect PlayerRect;
        private Rect Player1Rect;
        private Rect Player2Rect;
        private Rect BlockRect;
        private Rect BulletRect;

        private double CanvasWidth;
        private double CanvasHeight;
        private DispatcherTimer dispatcherTimer;

        private List<Block> blocks = new List<Block>();
        private List<Player> players = new List<Player>();
        private List<Bullet> bullets = new List<Bullet>();

        public GamePage()
        {
            this.InitializeComponent();

            // Setting up the canvas
            Canvas.Width = 680;
            Canvas.Height = 680;
            CanvasWidth = Canvas.Width;
            CanvasHeight = Canvas.Height;

            // Add player
            player1 = new Player { LocationX = 325, LocationY = 325, Player2 = false };
            Canvas.Children.Add(player1);
            player1.DrawPlayer();
            players.Add(player1);

            // Add Blocks       
            block = new Block { LocationX = 65, LocationY = 65 };
            blocks.Add(block);
            Canvas.Children.Add(block);
            block.drawMagic();
            block.UpdatePosition();

            int x = 0;
            for (int i = 0; i < 17; i++)
            {
                block = new Block { LocationX = x, LocationY = 425 };
                blocks.Add(block);
                Canvas.Children.Add(block);
                block.drawDirt();
                block.UpdatePosition();
                x = x + 40;
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
            foreach (Player player in players)
            {
                player.UpdatePlayer(Canvas);
            }
            // bullet.Move(); ???
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
            foreach (Player player in players)
            {
                player.StopTop = false;
                player.StopRight = false;
                player.StopLeft = false;
                player.StopBottom = false;
                if (player.Player2 == false)
                {
                    Player1Rect = player.GetRect();
                }
                else if (player.Player2 == true)
                {
                    Player2Rect = player.GetRect();
                }
                Player1Rect.Intersect(Player2Rect);

                // Yritin tehdä pelaajien välistä collision detectionia. Toimii osittain
                /*
                if (!Player1Rect.IsEmpty) // Unfinished, shit and not working
                {
                    if (Player1Rect.Left > Player2Rect.Left && Player2Rect.Left < Player1Rect.Right && Player2Rect.Top < Player2Rect.Bottom)
                    {
                        Debug.WriteLine("LEFT");
                        player.StopRight = true;
                    }
                    if (Player1Rect.Top > Player2Rect.Top && Player2Rect.Top < Player1Rect.Bottom && Player1Rect.Left < Player1Rect.Right)
                    {
                        Debug.WriteLine("TOP");
                        player.StopBottom = true;
                    }
                    if (Player1Rect.Left < Player2Rect.Right && Player2Rect.Right > Player1Rect.Right)
                    {
                        Debug.WriteLine("Right");
                        player.StopLeft = true;
                    }
                    if (Player1Rect.Top < Player2Rect.Bottom && Player2Rect.Bottom > Player1Rect.Bottom)
                    {
                        Debug.WriteLine("BOTTOM");
                        player.StopTop = true;
                    }
                } */
                  
                foreach (Block block in blocks)
                {
                    BlockRect = block.GetRect();
                    PlayerRect = player.GetRect();
                    BlockRect.Intersect(PlayerRect);

                    if (!BlockRect.IsEmpty) //player and block collision
                    {
                        blocks.Remove(block);
                        Canvas.Children.Remove(block);
                        player.score += block.PointValue;
                        UpdatePoints();
                        Debug.WriteLine("HIT");
                        break;
                    }
                }
            }


        }

        // Method for updating the player points on the screen
        private void UpdatePoints()
        {
            foreach (Player player in players)
            {
                if (player.Player2 == false)
                {
                    Player1Score.Text = player.score.ToString();
                }
                else if (player.Player2 == true)
                {
                    Player2Score.Text = player.score.ToString();
                }
            }
        }

        // This is for the two player mode, navigated to from the main page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is bool) // Checks if a bool was passed from MainPage
            {
                MP = (bool)e.Parameter; // If so, then MP gets the value

                if(MP == true) // If MP is true, a second player is added
                {
                    player2 = new Player { LocationX = 225, LocationY = 225, Player2 = true };
                    Canvas.Children.Add(player2);
                    player2.DrawPlayer();
                    players.Add(player2);
                }
            }
            base.OnNavigatedTo(e);
        }

        //Shooting a bullet, not done yet
        private void ShootBullet(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Q)
            bullet = new Bullet
            {
                LocationX = player1.LocationX,
                LocationY = player1.LocationY
            };
            Canvas.Children.Add(bullet);
        }
    }
}
