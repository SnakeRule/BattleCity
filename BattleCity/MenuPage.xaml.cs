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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BattleCity
{
    /// <summary>
    ///This is the button that pops up when you press "Play"
    /// </summary>

    public sealed partial class MenuPage : Page
    { 
        public MenuPage()
        {
            this.InitializeComponent();
        }
        //Clicking the button takes you to 1 player mode
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            GamePage.MP = false; // = 1 player
            this.Frame.Navigate(typeof(GamePage));
        }
        //Clicking the button takes you to 2 player mode
        private void MultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            GamePage.MP = true; // = 2 player
            this.Frame.Navigate(typeof(GamePage));
        }
        //Back to main menu
        private void BackMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
