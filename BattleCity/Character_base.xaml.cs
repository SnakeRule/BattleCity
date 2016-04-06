using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

    /// <summary>
    /// This is the Base user control class for both the player and enemy characters in the game
    /// </summary>
    namespace BattleCity
    {
         public partial class Character_base : UserControl
        {

        public int speed = 3;
        public int tankDirection { get; set; }
        private int AnimationCycleCounter = 0;
        private int animationTickCounter = 0;

        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        protected MediaElement mediaElement;
        protected bool left;
        protected bool right;
        protected bool up;
        protected bool down;
        public bool StopLeft { get; set; }
        public bool StopTop { get; set; }
        public bool StopRight { get; set; }
        public bool StopBottom { get; set; }
        private int bulletTickCounter;
        protected Bullet bullet;
        public List<Bullet> bullets = new List<Bullet>();
        public Canvas canvas { get; set; }



        // Default constructor for player
        public Character_base()
        {
            this.InitializeComponent();
            LoadAudio(); //Loads the pew sound
        }
        public Rect GetRect()
        {
            return new Rect(LocationX, LocationY, ActualWidth, ActualHeight);
        }

        public void CollisionRelease()
        {
                if (StopBottom == true && tankDirection != 4)
                {
                    LocationY += 4;
                    StopBottom = false;
                }
                if (StopTop == true && tankDirection != 2)
                {
                    LocationY -= 4;
                    StopTop = false;
                }
                if (StopLeft == true && tankDirection != 1)
                {
                    LocationX -= 4;
                    StopLeft = false;
                }
                if (StopRight == true && tankDirection != 3)
                {
                    LocationX += 4;
                    StopRight = false;
                }
            }

        // This is the method where the player is drawn on the screen each frame
        public void UpdatePlayer(Canvas canvas)
        {
        // these set the tanksprite to the canvas. The position is calculated from the tanksprite's current position and added or decreased speed

        if (StopRight == false)
        {
            if (left == true && LocationX >= ((CatRectangle.ActualWidth/2)/2))
            {
                    PlayerRotate.Angle = 270;
                    SetValue(Canvas.LeftProperty, LocationX -= speed);
                    tankDirection = 1;
                    animationTickCounter++;
            }
        }

        if (StopBottom == false)
        {
            if (up == true && LocationY >= CatRectangle.ActualHeight / 2 / 2 - 14)
            {
                    PlayerRotate.Angle = 0;
                    SetValue(Canvas.TopProperty, LocationY -= speed);
                    tankDirection = 2;
                    animationTickCounter++;
            }
        }

        if (StopLeft == false)
        {
            if (right == true && LocationX <= (canvas.ActualWidth - CatRectangle.ActualWidth - 6))
            {
                    PlayerRotate.Angle = 90;
                    SetValue(Canvas.LeftProperty, LocationX += speed);
                    tankDirection = 3;
                    animationTickCounter++;
            }
        }

        if (StopTop == false)
        {
            if (down == true && LocationY <= (canvas.ActualHeight - CatRectangle.ActualHeight +6))
            {
                    PlayerRotate.Angle = 180;
                    SetValue(Canvas.TopProperty, LocationY += speed);
                    tankDirection = 4;
                    animationTickCounter++;
            }
        }
}



        //Method for drawing the bullet every frame
        public void UpdateBullet(Canvas canvas)
        {
            foreach (Bullet bullet in bullets)
            {
                bulletTickCounter++;
                if (bulletTickCounter > 25)
                {
                    RemoveBullet(canvas);
                    break;
                }
                if (bullet.LocationX <= 0 || bullet.LocationX >= (canvas.Width - bullet.ActualWidth) || bullet.LocationY <= 0 || bullet.LocationY >= (canvas.Height - bullet.ActualHeight))
                {
                    RemoveBullet(canvas);
                    break;
                }
                else
                {
                    bullet.LocationX += bullet.SpeedX;
                    bullet.LocationY += bullet.SpeedY;
                    bullet.Shoot();
                }
            }
        }
        //Method to load audio from assets
        public async void LoadAudio()
        {
            mediaElement = new MediaElement();
            mediaElement.AutoPlay = false;
            mediaElement.Volume = 50;
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file =
                await folder.GetFileAsync("Pew.mp3");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream, file.ContentType);
        }

        public void CreateBullet()
        {
            //Creating a bullet, movement and spawn location depending on tankDirection
            if (bullets.Count < 1)
            {
                if (tankDirection == 1)
                {
                    bullet = new Bullet
                    {
                        SpeedX = -10, // left
                        SpeedY = 0,
                        LocationX = LocationX - 23,
                        LocationY = LocationY + 15.5
                    };
                    bullet.Shoot();
                    canvas.Children.Add(bullet);
                    bullets.Add(bullet);
                    //mediaElement.Play();
                 }
            if (tankDirection == 2)
            {
                bullet = new Bullet
                {
                    SpeedX = 0,
                    SpeedY = -10, //up
                    LocationX = LocationX + 12,
                    LocationY = LocationY - 21,
                };
                bullet.Shoot();
                canvas.Children.Add(bullet);
                bullets.Add(bullet);
                //mediaElement.Play();
            }
            if (tankDirection == 3)
            {
                bullet = new Bullet()
                {
                    SpeedX = 10, //right
                    SpeedY = 0,
                    LocationX = LocationX + 44.5,
                    LocationY = LocationY + 15.5
                };
                bullet.Shoot();
                canvas.Children.Add(bullet);
                bullets.Add(bullet);
                //mediaElement.Play();
            }
           if (tankDirection == 4)
            {
                bullet = new Bullet()
                {
                    SpeedX = 0,
                    SpeedY = 10, //down
                    LocationX = LocationX + 12,
                    LocationY = LocationY + 50,
                };
                bullet.Shoot();
                canvas.Children.Add(bullet);
                bullets.Add(bullet);
                //mediaElement.Play();
            }
        }
    }
        public void RemoveBullet(Canvas canvas) // method for removing the bullet
        {
            foreach (Bullet bullet in bullets)
            {
                bulletTickCounter = 0;
                canvas.Children.Remove(bullet);
                bullets.Remove(bullet);
                break;
            }
        }

        public void AnimationUpdate()
        {
            if(animationTickCounter >= 5)
            {
                AnimationCycleCounter++;
                if (AnimationCycleCounter >= 7)
                    AnimationCycleCounter = 0;
                animationTickCounter = 0;
            }
            switch (AnimationCycleCounter)
            {
                case 0:
                    CatSpriteSheetOffset.X = 0;
                    break;
                case 1:
                    CatSpriteSheetOffset.X = -37.5 * 1;
                    break;
                case 2:
                    CatSpriteSheetOffset.X = -37.5 * 2;
                    break;
                case 3:
                    CatSpriteSheetOffset.X = -37.5 * 3;
                    break;
                case 4:
                    CatSpriteSheetOffset.X = -37.5 * 4;
                    break;
                case 5:
                    CatSpriteSheetOffset.X = -37.5 * 3;
                    break;
                case 6:
                    CatSpriteSheetOffset.X = -37.5 * 2;
                    break;
                case 7:
                    CatSpriteSheetOffset.X = -37.5 * 1;
                    break;
            }
        }
    }
}
