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

namespace BattleCity
{
    public sealed partial class Bullet : UserControl
    {
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public double SpeedX { get; set; }
        public double SpeedY { get; set; }
        public double BulletDirection { get; set; }

        public Bullet()
        {
            this.InitializeComponent();
            SpeedX = 10;
            SpeedY = 10;
        }
        //This should move the bullet in some way, does not right now
         public void Move()
         {
            LocationX = LocationX + SpeedX;
            LocationY = LocationY + SpeedY;
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
         }
        //Not 100% if this is going to work at all
        public void CheckDirection(Canvas canvas)
        {
            if (BulletDirection==1)
            {
                SetValue(Canvas.LeftProperty, LocationX -= SpeedX);
            }
            if (BulletDirection==2)
            {
                SetValue(Canvas.TopProperty, LocationY -= SpeedY);
            }
            if (BulletDirection==3)
            {
                SetValue(Canvas.LeftProperty, LocationX += SpeedX);
            }
            if (BulletDirection==4)
            {
                SetValue(Canvas.TopProperty, LocationY += SpeedY);
            }
        }

    }
}
