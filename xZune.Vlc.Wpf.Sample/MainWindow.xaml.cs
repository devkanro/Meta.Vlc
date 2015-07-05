using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace xZune.Vlc.Wpf.Sample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Closing += MainWindow_Closing;
            InitializeComponent();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            player.Dispose();
            ApiManager.ReleaseAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            player.LoadMedia(@"F:\");
            player.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            player.Navigate(Interop.MediaPlayer.NavigateMode.Down);
            //player.PauseOrResume();
            //await player.Stop();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            player.Navigate(Interop.MediaPlayer.NavigateMode.Activate);
            //player.TakeSnapshot(@"C:\Users\HIGAN\Desktop\", SnapshotFormat.PNG, 0);
            //player.Rate = float.Parse(volume.Text);
        }
    }
}
