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
        protected MediaElement mediaElement; // This is the pew sound
        protected bool left;
        protected bool right;
        protected bool up;
        protected bool down;
        public bool StopRight { get; set; }
        public bool StopDown { get; set; }
        public bool StopLeft { get; set; }
        public bool StopUp { get; set; }
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

        /// <summary>
        /// The purpose of this method is to stop the characters from getting stuck on the blocks after collision
        /// </summary>
        public void CollisionRelease()
        {
                if (StopUp == true && tankDirection != 4) // Tank has hit the bottom of something and is moving somewhere other than down
                {
                    LocationY += 4; // The tank is moved down by 4 to avoid getting stuck
                    StopUp = false;
                }
                if (StopDown == true && tankDirection != 2) // Tank has hit the top of something and is moving somewhere other than up
                {
                    LocationY -= 4; // The tank is moved up by 4 to avoid getting stuck
                    StopDown = false;
                }
                if (StopRight == true && tankDirection != 1) // Tank has hit the right side of something and is moving somewhere other than left
                {
                    LocationX -= 4; // The tank is moved left by 4 to avoid getting stuck
                    StopRight = false;
                }
                if (StopLeft == true && tankDirection != 3) // Tank has hit the left side of something and is moving somewhere other than right
                {
                    LocationX += 4; // The tank is moved right by 4 to avoid getting stuck
                    StopLeft = false;
                }
            }

        // This is the method where the player is drawn on the screen each frame
        public void UpdatePlayer(Canvas canvas)
        {
        // these set the tanksprite to the canvas. The position is calculated from the tanksprite's current position and added or decreased speed

        if (StopLeft == false)
        {
            if (left == true && LocationX >= ((CatRectangle.ActualWidth/2)/2))
            {
                    PlayerRotate.Angle = 270;
                    SetValue(Canvas.LeftProperty, LocationX -= speed);
                    tankDirection = 1;
                    animationTickCounter++;
            }
        }

        if (StopUp == false)
        {
            if (up == true && LocationY >= CatRectangle.ActualHeight / 2 / 2 - 14)
            {
                    PlayerRotate.Angle = 0;
                    SetValue(Canvas.TopProperty, LocationY -= speed);
                    tankDirection = 2;
                    animationTickCounter++;
            }
        }

        if (StopRight == false)
        {
            if (right == true && LocationX <= (canvas.ActualWidth - CatRectangle.ActualWidth - 6))
            {
                    PlayerRotate.Angle = 90;
                    SetValue(Canvas.LeftProperty, LocationX += speed);
                    tankDirection = 3;
                    animationTickCounter++;
            }
        }

        if (StopDown == false)
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



        //Method for updating bullet position every frame
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
                //Removes the bullet if it goes off the canvas
                if (bullet.LocationX <= 0 || bullet.LocationX >= (canvas.Width - bullet.ActualWidth) || bullet.LocationY <= 0 || bullet.LocationY >= (canvas.Height - bullet.ActualHeight))
                {
                    RemoveBullet(canvas);
                    break;
                }
                else
                {
                    bullet.LocationX += bullet.SpeedX; //Moves the bullet on x-axis
                    bullet.LocationY += bullet.SpeedY; //Moves the bullet on y-axis
                    bullet.Shoot(); //Updates the location to canvas
                }
            }
        }
        //Loads the audio from assets
        public async void LoadAudio()
        {
            mediaElement = new MediaElement();
            mediaElement.AutoPlay = false; 
            StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file =
                await folder.GetFileAsync("Pew.mp3"); // Gun sound
            var stream = await file.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream, file.ContentType);
        }

        public void CreateBullet()
        {
            //Creating a bullet, movement and spawn location depending on tankDirection
            if (bullets.Count < 1) //only one bullet on canvas at a time from a player
            {
                if (tankDirection == 1) // left
                {
                    bullet = new Bullet
                    {
                        SpeedX = -10, // left
                        SpeedY = 0,
                        LocationX = LocationX - 23, //creating the bullet in a position that it looks good compared to sprites position
                        LocationY = LocationY + 15.5 //creating the bullet in a position that it looks good compared to sprites position
                    };
                    bullet.Shoot(); 
                    canvas.Children.Add(bullet); //Adding the bullet to canvas
                    bullets.Add(bullet); //Adding bullet to list
                    //mediaElement.Play();
                 }
            if (tankDirection == 2) //up
            {
                bullet = new Bullet
                {
                    SpeedX = 0,
                    SpeedY = -10, //up
                    LocationX = LocationX + 12, //creating the bullet in a position that it looks good compared to sprites position
                    LocationY = LocationY - 21, //creating the bullet in a position that it looks good compared to sprites position
                };
                bullet.Shoot();
                canvas.Children.Add(bullet);//Adding the bullet to canvas
                bullets.Add(bullet); //Adding bullet to list
                //mediaElement.Play();
            }
            if (tankDirection == 3) //right
            {
                bullet = new Bullet()
                {
                    SpeedX = 10, //right
                    SpeedY = 0,
                    LocationX = LocationX + 44.5, //creating the bullet in a position that it looks good compared to sprites position
                    LocationY = LocationY + 15.5 //creating the bullet in a position that it looks good compared to sprites position
                };
                bullet.Shoot();
                canvas.Children.Add(bullet);//Adding the bullet to canvas
                bullets.Add(bullet);//Adding bullet to list
                //mediaElement.Play();
            }
           if (tankDirection == 4)//down
            {
                bullet = new Bullet()
                {
                    SpeedX = 0,
                    SpeedY = 10, //down
                    LocationX = LocationX + 12, //creating the bullet in a position that it looks good compared to sprites position
                    LocationY = LocationY + 50, //creating the bullet in a position that it looks good compared to sprites position
                };
                bullet.Shoot();
                canvas.Children.Add(bullet);//Adding the bullet to canvas
                bullets.Add(bullet); //Adding bullet to list
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
