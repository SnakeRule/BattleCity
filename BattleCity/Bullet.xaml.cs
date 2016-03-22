using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public double BulletSpeed { get; set; }
        public double BulletDirection { get; set; }

        public Bullet()
        {
            this.InitializeComponent();
        }
        public void DrawBullet()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
        public void CheckDirection(Canvas canvas)
        {
            if (BulletDirection==1)
            {
                SetValue(Canvas.LeftProperty, LocationX -= BulletSpeed);
            }
            if (BulletDirection==2)
            {
                SetValue(Canvas.TopProperty, LocationY -= BulletSpeed);
            }
            if (BulletDirection==3)
            {
                SetValue(Canvas.LeftProperty, LocationX += BulletSpeed);
            }
            if (BulletDirection==4)
            {
                SetValue(Canvas.TopProperty, LocationY += BulletSpeed);
            }
        }

    }
}
