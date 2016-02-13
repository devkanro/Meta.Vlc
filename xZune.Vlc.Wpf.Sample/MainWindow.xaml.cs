//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: MainWindow.xaml.cs
//Version: 20160109

//Note: can find VLC stream URLs for testing at http://www.vlchistory.eu.pn/
//      and http://dveo.com/downloads/TS-sample-files/San_Diego_Clip.ts

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace xZune.Vlc.Wpf.Sample
{
    public partial class MainWindow : Window
    {
        //VlcPlayer Player = new VlcPlayer(); //uncomment if adding the player dynamically

        #region --- Initialization ---

        public MainWindow()
        {
            InitializeComponent();

            //uncomment if adding the player dynamically
            /*
            Player.SetValue(Canvas.ZIndexProperty, -1);
            LayoutParent.Children.Add(Player);
            */
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Player.Dispose();
        }

        #endregion --- Cleanup ---

        #region --- Events ---

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            String pathString = path.Text;
            /*
            Uri uri;
            if (!Uri.TryCreate(pathString, UriKind.Absolute, out uri)) return;
            */

            Player.BeginStop(() =>
            {
                Player.LoadMedia(pathString); //if you pass a string instead of a Uri, LoadMedia will see if it is an absolute Uri, else will treat it as a file path
                Player.Play();
            });
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            //Player.PauseOrResume();

            var devices = Player.EnumAudioDeviceList();
            var outputs = Player.GetAudioOutputList();
            Player.SetAudioDevice(null, devices[2]);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Player.BeginStop();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close(); //closing the main window will also terminate the application
        }

        private void AspectRatio_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Player.AspectRatio = AspectRatio.Default;
                    break;

                case 1:
                    Player.AspectRatio = AspectRatio._16_9;
                    break;

                case 2:
                    Player.AspectRatio = AspectRatio._4_3;
                    break;
            }
        }


        private void ProgressBar_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var value = (float)(e.GetPosition(ProgressBar).X / ProgressBar.ActualWidth);
            ProgressBar.Value = value;
        }

        #endregion --- Events ---

    }
}