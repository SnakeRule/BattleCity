/*
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ControlsPage1 : Page
    {
        public ControlsPage1()
        {
            this.InitializeComponent();
            VolumeSlider.Value = BackgroundMediaPlayer.Current.Volume * 100;
            VolumeSlider.IsTabStop = false;
            MuteButton.IsTabStop = false;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
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
