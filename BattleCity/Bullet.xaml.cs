using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BattleCats
{
    public sealed partial class Bullet : UserControl
    {
        public double LocationX { get; set; }//Location of bullet on x-axis
        public double LocationY { get; set; } //Location of bullet on y-axis
        public double SpeedX { get; set; } //used for moving the bullet on x-axis
        public double SpeedY { get; set; } //used for moving the bullet on y-axis

        public Bullet()
        {
            this.InitializeComponent();
        }    
        //This moves the bullet on the canvas
        public void Shoot()
        {
            SetValue(Canvas.LeftProperty,LocationX);
            SetValue(Canvas.TopProperty,LocationY);
        }
        //Bullet hitbox
        public Rect GetRect()
        {
            return new Rect(LocationX, LocationY, ActualWidth, ActualHeight);
        }
    }
}
