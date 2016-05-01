// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: MainWindow.xaml.cs
// Version: 20160404

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace Meta.Vlc.Wpf.Sample
{
    public partial class MainWindow : Window
    {
        //VlcPlayer Player = null; //uncomment if adding the player dynamically or use other control to render video

        #region --- Initialization ---

        public MainWindow()
        {
            InitializeComponent();
            //uncomment if adding the player dynamically
            /*
            Player = new VlcPlayer();
            Player.SetValue(Canvas.ZIndexProperty, -1);
            LayoutParent.Children.Add(Player);
            */

            //uncomment if you use Image or ThreadSeparatedImage to render video
            /*
            Player.Initialize(@"..\..\libvlc", new string[] { "-I", "dummy", "--ignore-config", "--no-video-title" });
            Player.VideoSourceChanged += PlayerOnVideoSourceChanged;
            */
        }

        //uncomment if you use Image or ThreadSeparatedImage to render video
        /*
        private void PlayerOnVideoSourceChanged(object sender, VideoSourceChangedEventArgs videoSourceChangedEventArgs)
        {
            DisplayImage.Dispatcher.BeginInvoke(new Action(() =>
            {
                DisplayImage.Source = videoSourceChangedEventArgs.NewVideoSource;
            }));
        }
        */

        #endregion --- Initialization ---

        #region --- Cleanup ---

        protected override void OnClosing(CancelEventArgs e)
        {
            Player.Dispose();
            ApiManager.ReleaseAll();
            base.OnClosing(e);
        }

        #endregion --- Cleanup ---

        #region --- Events ---

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var openfiles = new OpenFileDialog();
            if (openfiles.ShowDialog() == true)
            {
                Player.Stop();
                Player.LoadMedia(openfiles.FileName);
                Player.Play();
            }
            return;

            String pathString = path.Text;

            Uri uri = null;
            if (!Uri.TryCreate(pathString, UriKind.Absolute, out uri)) return;

            Player.Stop();
            Player.LoadMedia(uri);
            //if you pass a string instead of a Uri, LoadMedia will see if it is an absolute Uri, else will treat it as a file path
            Player.Play();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(10000);
            Player.Play();
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            Player.PauseOrResume();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close(); //closing the main window will also terminate the application
        }

        private void AspectRatio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Player == null) return;
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
