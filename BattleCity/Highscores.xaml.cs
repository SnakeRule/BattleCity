﻿/*
* Copyright (C) Tiia Aarnio, Lauri Moilanen, Jere-Joonas Valtanen
*
* This file is part of JAMK object oriented programming course
* BattleCats project
*
* Created: 26.4.2016
*Author: Tiia Aarnio, Lauri Moilanen, Jere-Joonas Valtanen
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BattleCats
{
    /// <summary>
    /// This page shows the local highscores
    /// </summary>

    // Get highscores from file to this page
    public sealed partial class Highscores : Page
    {
        public Highscores()
        {
            this.InitializeComponent();
            VolumeSlider.Value = BackgroundMediaPlayer.Current.Volume * 100;
            VolumeSlider.IsTabStop = false;
            MuteButton.IsTabStop = false;
            ShowScores(); // Shows the current highest score
            ShowNames(); // Shows the name of the player who got the highest score
        }
        //Back to main page
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        // Method for showing the highest score
        private async void ShowScores()
        {
            try // Exception management
            {
                Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile HSFile =
                    await storageFolder.GetFileAsync("Highscores.dat");
                string Highscoretext = await Windows.Storage.FileIO.ReadTextAsync(HSFile);
                HStext.Text = Highscoretext;
            }
            catch(Exception)
            {
                Debug.Write("FILE1 NOT FOUND");
            }

        }
        // Method for showing the name
        private async void ShowNames()
        {
            try // Exception management
            {
                Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile HSNimet =
                    await storageFolder.GetFileAsync("Highscorenames.dat");
                string Highscoretext = await Windows.Storage.FileIO.ReadTextAsync(HSNimet);
                HSnametext.Text = Highscoretext;
            }
            catch(Exception)
            {
                Debug.Write("FILE1 NOT FOUND");
                HSnametext.Text = "No highscore yet!";
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
