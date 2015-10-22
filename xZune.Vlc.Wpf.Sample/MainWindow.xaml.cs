using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace xZune.Vlc.Wpf.Sample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            VlcPlayer.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VlcPlayer.Stop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VlcPlayer.PauseOrResume();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            VlcPlayer.Play();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(path.Text);
            //String pathString = path.Text;

            VlcPlayer.BeginStop((ar) =>
            {
                VlcPlayer.LoadMedia(uri);
                VlcPlayer.Play();
            });
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    VlcPlayer.AspectRatio = AspectRatio.Default;
                    break;
                case 1:
                    VlcPlayer.AspectRatio = AspectRatio._16_9;
                    break;
                case 2:
                    VlcPlayer.AspectRatio  = AspectRatio._4_3;
                    break;
            }
        }
    }
}
