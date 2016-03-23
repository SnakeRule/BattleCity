﻿using System;
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
        private Block block1;
        private Block block2;
        private Block block3;
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

        private List<Block> blocks = new List<Block>(); // All blocks
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

            // Add Blocks       
            block2 = new Block { LocationX = 65, LocationY = 65 };
            blocks.Add(block2);
            Canvas.Children.Add(block2);
            block2.drawMagic(); // canGoTrough = true, canDestroy = false
            block2.UpdatePosition();

            block3 = new Block { LocationX = 165, LocationY = 165 };
            blocks.Add(block3);
            Canvas.Children.Add(block3);
            block3.drawStone(); // canGoTrough = false, canDestroy = false
            block3.UpdatePosition();

            int x = 0;
            for (int i = 0; i < 17; i++)
            {
                block1 = new Block { LocationX = x, LocationY = 425 };
                blocks.Add(block1);
                Canvas.Children.Add(block1);
                block1.drawTile(); // canGoTrough = false, canDestroy = true
                block1.UpdatePosition();
                x = x + 40;
            }

            // Add player
            player1 = new Player { LocationX = 325, LocationY = 325, Player2 = false };
            Canvas.Children.Add(player1);
            player1.DrawPlayer();
            players.Add(player1);          
          
            // Setting up the timer that runs the Game method
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Game;
            dispatcherTimer.Interval = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 60);
            dispatcherTimer.Start();
        }

        public void Game(object sender, object e)
        {
            foreach (Player player in players)
            {
                CollisionCheck();
                player.UpdatePlayer(Canvas);
            }

            if(player1.StopBottom == true)
            {
                player1.LocationY += 5;
                player1.StopBottom = false;
            }
            if (player1.StopTop == true)
            {
                player1.LocationY -= 5;
                player1.StopTop = false;
            }
            if (player1.StopLeft == true)
            {
                player1.LocationX -= 5;
                player1.StopLeft = false;
            }
            if (player1.StopRight == true)
            {
                player1.LocationX += 5;
                player1.StopRight = false;
            }
        }
        /* This has to be bound to a virtualkey, also make the bullet move to the direction tank is facing  
        public void CreateBullet()
        {
            {
                bullet = new Bullet()
                {
                    LocationX = player1.LocationX,
                    LocationY = player1.LocationY
                };
            }
            Canvas.Children.Add(bullet);
        } */      
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
                player.StopLeft = false;
                player.StopRight = false;
                player.StopTop = false;
                player.StopBottom = false;


                if (player.Player2 == false)
                {
                    Player1Rect = player.GetRect();
                }
                else if (player.Player2 == true)
                {
                    Player2Rect = player.GetRect();
                }

                // Yritin tehdä pelaajien välistä collision detectionia. Toimii osittain

                    Player1Rect.Intersect(Player2Rect);
                    if (!Player1Rect.IsEmpty) // Unfinished, shit and not working
                    {
                        if (player1.LocationX > player2.LocationX && players[0].tankDirection == 1) // Checking if player1 is intersecting player 2 from the right
                        {
                            Debug.WriteLine("HITTING RIGHT");
                            player.StopRight = true;
                        }

                        if (player1.LocationY > player2.LocationY && players[0].tankDirection == 2) // Checking if player1 is intersecting player 2 from the bottom
                    {
                            Debug.WriteLine("HITTING BOTTOM");
                            player.StopBottom = true;
                        }

                        if (player1.LocationX < player2.LocationX && players[0].tankDirection == 3) // Checking if player1 is intersecting player 2 from the left
                    {
                            Debug.WriteLine("HITTING LEFT");
                            player.StopLeft = true;
                        }

                        if (player1.LocationY < player2.LocationY && players[0].tankDirection == 4) // Checking if player1 is intersecting player 2 from the top
                    {
                            Debug.WriteLine("HITTING TOP");
                            player.StopTop = true;
                        }
                    }

                
                foreach (Block block1 in blocks) // Collision detection for Tile blocks
                {
                    BlockRect = block1.GetRect();
                    PlayerRect = player.GetRect();
                    BlockRect.Intersect(PlayerRect);
                    // PlayerRect.Intersect(PlayerRect); between players

                    if (!BlockRect.IsEmpty && block1.CanGoTrough == false) //player and block collision
                    {
                        if (player1.LocationX > block1.LocationX && players[0].tankDirection == 1) // Checking if player1 is intersecting player 2 from the right
                        {
                            Debug.WriteLine("HITTING RIGHT");
                            player.StopRight = true;
                        }

                        if (player1.LocationY > block1.LocationY && players[0].tankDirection == 2) // Checking if player1 is intersecting player 2 from the bottom
                        {
                            Debug.WriteLine("HITTING BOTTOM");
                            player.StopBottom = true;
                        }

                        if (player1.LocationX < block1.LocationX && players[0].tankDirection == 3) // Checking if player1 is intersecting player 2 from the left
                        {
                            Debug.WriteLine("HITTING LEFT");
                            player.StopLeft = true;
                        }

                        if (player1.LocationY < block1.LocationY && players[0].tankDirection == 4) // Checking if player1 is intersecting player 2 from the top
                        {
                            Debug.WriteLine("HITTING TOP");
                            player.StopTop = true;
                        }
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

    }
}
