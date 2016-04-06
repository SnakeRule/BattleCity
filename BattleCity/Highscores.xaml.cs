using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    // Get highscores from file to this page
    public sealed partial class Highscores : Page
    {
        public Highscores()
        {
            this.InitializeComponent();
        }
        //Back to main page
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        // Shows the current High scores
        private async void HSButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.StorageFolder storageFolder =
          Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile HSFile =
                await storageFolder.GetFileAsync("Highscore.txt");
            string Highscoretext = await Windows.Storage.FileIO.ReadTextAsync(HSFile);
            HStext.Text = Highscoretext;
        }
    }
}
