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
        private int catDirection; // This value is used to tell which direction the tank is currently facing.
        private int speed = 4; // Used to tell how fast the character moves on screen
        private int AnimationCycleCounter = 0; // Used to tell which animation picture is currently in use
        private int animationTickCounter = 0; // Used to count when the next animatin picture should be loaded

        public double LocationX { get; set; } // This value is used to tell the character's location on the X-axis
        public double LocationY { get; set; } // This value is used to tell the character's location on the Y-axis
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        protected MediaElement mediaElement;
        // The following bools are used to tell which direction the tank should be moving.
        protected bool left;
        protected bool right;
        protected bool up;
        protected bool down;
        
        // The following bools are used by the collision detection to tell the character that it can't move in a certain direction
        public bool StopRight { get; set; }
        public bool StopDown { get; set; }
        public bool StopLeft { get; set; }
        public bool StopUp { get; set; }

        private int bulletTickCounter; // This value is used for counting how long the bullet has existed

        public int CatDirection // Does some checks before accepting the incoming value to tankDirection
        {
            get
            {
                return catDirection;
            }
            set
            {
                if (value < 1)
                    catDirection = 1;
                if (value > 4)
                    catDirection = 4;
                if (value > 0 && value < 5)
                    catDirection = value;
            }
        }
        public int Speed // Does some checks before accepting the incoming value to speed
        {
            get
            {
                return speed;
            }
            set
            {
                if (value < 0)
                    speed = 0;
                if (value > 10)
                    speed = 10;
                if (value > -1 && value < 11)
                    speed = value;
            }
        }

        protected Bullet bullet; // Introducing object bullet, which comes from the Bullet class
        public List<Bullet> bullets = new List<Bullet>(); // Creating the list where the bullets go to
        public Canvas canvas { get; set; }



        // Default constructor for player
        public Character_base()
        {
            this.InitializeComponent();
            LoadAudio(); //Loads the pew sound
        }
        public Rect GetRect() // This method is used in collision detection for creating a rectangle for the player in its current position
        {
            return new Rect(LocationX, LocationY, ActualWidth, ActualHeight);
        }

        /// <summary>
        /// The purpose of this method is to stop the characters from getting stuck on the blocks after collision
        /// </summary>
        public void CollisionRelease()
        {
                if (StopUp == true && CatDirection != 4) // Tank has hit the bottom of something and is moving somewhere other than down
                {
                    LocationY += 4; // The tank is moved down by 4 to avoid getting stuck
                    StopUp = false;
                }
                if (StopDown == true && CatDirection != 2) // Tank has hit the top of something and is moving somewhere other than up
                {
                    LocationY -= 4; // The tank is moved up by 4 to avoid getting stuck
                    StopDown = false;
                }
                if (StopRight == true && CatDirection != 1) // Tank has hit the right side of something and is moving somewhere other than left
                {
                    LocationX -= 4; // The tank is moved left by 4 to avoid getting stuck
                    StopRight = false;
                }
                if (StopLeft == true && CatDirection != 3) // Tank has hit the left side of something and is moving somewhere other than right
                {
                    LocationX += 4; // The tank is moved right by 4 to avoid getting stuck
                    StopLeft = false;
                }
            }

        // This is the method where the player is drawn on the screen each frame
        public void UpdatePlayer(Canvas canvas)
        {
        // these move the tanksprite on the canvas. The position is calculated from the tanksprite's current position and added or decreased speed

        if (StopLeft == false) //Checking if collision detection has stopped the character from moving left
        {
            if (left == true && LocationX >= ((CatRectangle.ActualWidth/2)/2))
            {
                    PlayerRotate.Angle = 270; // Changes the value of the RotateTransform in the xaml file to rotate the picture
                    SetValue(Canvas.LeftProperty, LocationX -= Speed); // Adds speed value to the character Location
                    CatDirection = 1; // Changes the tank direction value
                    animationTickCounter++; // The animationTickCounter goes up by one
            }
        }

        if (StopUp == false) //Checking if collision detection has stopped the character from moving up
        {
            if (up == true && LocationY >= CatRectangle.ActualHeight / 2 / 2 - 14)
            {
                    PlayerRotate.Angle = 0; // Changes the value of the RotateTransform in the xaml file to rotate the picture
                    SetValue(Canvas.TopProperty, LocationY -= Speed); // Adds speed value to the character Location
                    CatDirection = 2; // Changes the tank direction value
                    animationTickCounter++; // The animationTickCounter goes up by one
            }
        }

        if (StopRight == false) //Checking if collision detection has stopped the character from moving right
        {
            if (right == true && LocationX <= (canvas.ActualWidth - CatRectangle.ActualWidth - 6))
            {
                    PlayerRotate.Angle = 90; // Changes the value of the RotateTransform in the xaml file to rotate the picture
                    SetValue(Canvas.LeftProperty, LocationX += Speed); // Adds speed value to the character Location
                    CatDirection = 3; // Changes the tank direction value
                    animationTickCounter++; // The animationTickCounter goes up by one
            }
        }

        if (StopDown == false) //Checking if collision detection has stopped the character from moving down
        {
            if (down == true && LocationY <= (canvas.ActualHeight - CatRectangle.ActualHeight +6))
            {
                    PlayerRotate.Angle = 180; // Changes the value of the RotateTransform in the xaml file to rotate the picture
                    SetValue(Canvas.TopProperty, LocationY += Speed); // Adds speed value to the character Location
                    CatDirection = 4; // Changes the tank direction value
                    animationTickCounter++; // The animationTickCounter goes up by one
            }
        }
}



        //Method for drawing the bullet every tick
        public void UpdateBullet(Canvas canvas)
        {
            foreach (Bullet bullet in bullets)
            {
                bulletTickCounter++; // bulletTickCounter + 1 to know how long the bullet has been on screen
                if (bulletTickCounter > 25) // If the bullet has been on screen for 25 ticks, it is deleted
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
                if (CatDirection == 1)
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
            if (CatDirection == 2)
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
            if (CatDirection == 3)
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
           if (CatDirection == 4)
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

        public void AnimationUpdate() // This method is used for updating the character picture to produce animations
        {
            if(animationTickCounter >= 5) // If the animation has been on screen and moving for 5 ticks
            {
                AnimationCycleCounter++; // The animation cycle goes up by one, which should load the next picture
                if (AnimationCycleCounter >= 7) // If the counter goes up to 7, the cycle is reset
                    AnimationCycleCounter = 0;
                animationTickCounter = 0;
            }
            switch (AnimationCycleCounter) // The animation works by moving to the right or left on the picture in the xaml file
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
