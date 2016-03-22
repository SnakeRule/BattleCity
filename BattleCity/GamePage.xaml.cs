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

// Hei olen Tiia :)
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
        private Block block; // help
        private Bullet bullet;

        private bool MP; // Bool used for checking if 2-player mode was selected

        // These rectangles are used as hitboxes
        private Rect PlayerRect;
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
            Canvas.Width = 650;
            Canvas.Height = 650;
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
            for (int i = 0; i < 10; i++)
            {
                block = new Block { LocationX = x, LocationY = 425 };
                blocks.Add(block);
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
            foreach (Player player in players)
            {
                PointsCheck();
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
                foreach (Block block in blocks)
                {
                    Rect BlockRect = block.GetRect();
                    PlayerRect = new Rect(player.LocationX, player.LocationY, player.ActualWidth, player.ActualHeight);
                    BlockRect.Intersect(PlayerRect);
                    // PlayerRect.Intersect(PlayerRect); between players


                    if (!BlockRect.IsEmpty) //player and block collision
                    {
                        blocks.Remove(block);
                        Canvas.Children.Remove(block);
                        player.score += block.PointValue;
                        Debug.WriteLine("HIT");
                        break;
                    }
                    if (!PlayerRect.IsEmpty) // Unfinished, shit and not working
                    {
                        player.LocationX = player.LocationX;
                        player.LocationY = player.LocationY;
                    }
                }
            }

        }
    


        private void PointsCheck()
        {
            foreach(Player player in players)
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
