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
    public sealed partial class Block : UserControl
    {
        private readonly int pointValue = 5; // How many points from destroying block
        private bool canDestroy;

        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public Block()
        {
            this.InitializeComponent();
        }

        public void drawDirt()
        {            
            BlockSpriteSheetOffset.Y = 0;
            canDestroy = false;
        }

        public void drawMagic()
        {
            BlockSpriteSheetOffset.Y = -65;
            canDestroy = true;
        }

        public void UpdatePosition()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
