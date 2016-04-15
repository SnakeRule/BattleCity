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
    ///This is the button that pops up when you press "Play"
    /// </summary>

    public sealed partial class MenuPage : Page
    { 
        private int levelNumber;
        private bool levelExists;
        public static int P1Colour;
        public static int P2Colour;
        public static string P1Name;
        public static string P2Name;

        StreamReader reader;

        public MenuPage()
        {
            this.InitializeComponent();
            P1Colour = 1;
            P2Colour = 1;
            levelNumber = 1;
            GamePage.MP = false; // = 1 player is default
        }

        //Clicking the button takes you to 1 player mode
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if(reader != null)
                reader.Dispose();
            GamePage.LevelNumber = int.Parse(LevelNumberTextBlock.Text);
            P1Name = P1NameTextBox.Text;
            P2Name = P2NameTextBox.Text;
            this.Frame.Navigate(typeof(GamePage));
        }
        //Clicking the button takes you to 2 player mode
        private void MultiPlayer_Click(object sender, RoutedEventArgs e)
        {
            GamePage.MP = true; // = 2 player
            MultiPlayer.Visibility = Visibility.Collapsed;
            P2NameTextBox.Visibility = Visibility.Visible;
            P2TextBlock.Visibility = Visibility.Visible;
            P2WhiteCatImage.Visibility = Visibility.Visible;
            P2PinkCatImage.Visibility = Visibility.Visible;
        }
        //Back to main menu
        private void BackMenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void LevelUpButton_Click(object sender, RoutedEventArgs e)
        {
            levelNumber++;
            levelExists = LoadLevel();
            if (levelExists == true)
            {
                LevelNumberTextBlock.Text = levelNumber.ToString();
                GamePage.LevelNumber = levelNumber;
            }
            else
                levelNumber--;

        }

        private void LevelDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (levelNumber > 1)
            {
                levelNumber--;
                LevelNumberTextBlock.Text = levelNumber.ToString();
            }
        }

        public bool LoadLevel()
        {
            // create or open local file
            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                using (StreamReader reader = File.OpenText(@"Levels\Level" + levelNumber + ".txt"))
                {
                    GamePage.LevelNumber++;
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }

        private void P1WhiteCatImage_Click(object sender, RoutedEventArgs e)
        {
            P1Colour = 1;
        }

        private void P1PinkCatImage_Click(object sender, RoutedEventArgs e)
        {
            P1Colour = 2;
        }

        private void P2WhiteCatImage_Click(object sender, RoutedEventArgs e)
        {
            P2Colour = 1;
        }

        private void P2PinkCatImage_Click(object sender, RoutedEventArgs e)
        {
            P2Colour = 2;
        }
    }
}
