using System;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.Storage;
using Windows.Media.Playback;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BattleCity
{
    /// <summary>
    /// This is the page that launches when opening the application
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static bool musicOn;
        public MainPage()
        {
            this.InitializeComponent();
            VolumeSlider.IsTabStop = false;
            MuteButton.IsTabStop = false;
            LoadBgMusic();
            VolumeSlider.Value = BackgroundMediaPlayer.Current.Volume * 100;
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720); //Setting up the launch size as 1280x720
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }
        //Exits the application
        private void ExitButton_Click(object sender, RoutedEventArgs e)
         {
             Application.Current.Exit();
         }
        //Clicking this button takes you to 1 player/2 player selection
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MenuPage));
        }
        //Clicking this button takes you to credits
        private void CreditsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Credits));
        }
        //Clicking this button takes you to highscores
        private void HSButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Highscores));
        }
        //Loads the audio from assets
        public static void LoadBgMusic()
        {
            if(musicOn == false)
            {
                BackgroundMediaPlayer.Current.SetUriSource(new Uri("ms-appx:///Assets/BgMusic.mp3"));
                BackgroundMediaPlayer.Current.IsLoopingEnabled = true;
                BackgroundMediaPlayer.Current.Volume = 0.5;
                BackgroundMediaPlayer.Current.AutoPlay = true;
                musicOn = true;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
                BackgroundMediaPlayer.Current.Volume = (double)VolumeSlider.Value / 100;
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
                VolumeSlider.Value = 0;
                BackgroundMediaPlayer.Current.Volume = 0;
        }
    }
}
