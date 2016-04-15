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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BattleCity
{

    /// <summary>
    /// This is the page that launches when opening the application
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MediaElement BgMusic;

        public MainPage()
        {
            this.InitializeComponent();
            LoadAudio();
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
        public async void LoadAudio()
        {
            if (BgMusic == null)
            {
                BgMusic = new MediaElement();
                BgMusic.AutoPlay = false;
                BgMusic.RealTimePlayback = true;
                BgMusic.IsLooping = true;
                BgMusic.Volume = 0.5;
                StorageFolder folder =
                    await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
                StorageFile file =
                    await folder.GetFileAsync("Blood Pressure.mp3"); // Music
                var stream = await file.OpenAsync(FileAccessMode.Read);
                BgMusic.SetSource(stream, file.ContentType);
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(BgMusic != null)
            {
                BgMusic.Volume = (double)VolumeSlider.Value / 100;
            }
        }
    }
}
