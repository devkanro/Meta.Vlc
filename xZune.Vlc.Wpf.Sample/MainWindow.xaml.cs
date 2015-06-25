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
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await player.StopAsync();
            player.LoadMedia(@"E:\Video\NFS11-2.mp4");
            player.Play();

            //if(player.VlcMediaPlayer.Media == null)
            //{
            //    player.LoadMedia(@"E:\Video\NFS11-2.mp4");
            //    player.Play();
            //}
            //else
            //{
            //    if(player.State == Interop.Media.MediaState.Stopped)
            //    {
            //        player.Play();
            //    }
            //    else
            //    {
            //        player.PauseOrResume();
            //    }
            //}
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            player.LoadMedia(new Uri("http://download.blender.org/peach/bigbuckbunny_movies/big_buck_bunny_480p_surround-fix.avi"));
            player.Play();
            //player.Volume = int.Parse(volume.Text);
        }
    }
}
