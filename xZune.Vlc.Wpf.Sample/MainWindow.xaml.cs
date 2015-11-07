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
            Player.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Player.PauseOrResume();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(path.Text);
            //String pathString = path.Text;

            Player.BeginStop((ar) =>
            {
                Player.LoadMedia(uri);
                Player.Play();
            });
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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
                    Player.AspectRatio  = AspectRatio._4_3;
                    break;
            }
        }
    }
}
