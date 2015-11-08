//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: MainWindow.xaml.cs
//Version: 20151108

//Note: can find VLC stream URLs for testing at http://www.vlchistory.eu.pn/

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace xZune.Vlc.Wpf.Sample
{

  public partial class MainWindow : Window
  {

    #region --- Initialization ---

    public MainWindow()
    {
      InitializeComponent();
    }

    #endregion

    #region --- Cleanup ---

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
      Player.Dispose();
    }

    #endregion

    #region --- Events ---

    private void Load_Click(object sender, RoutedEventArgs e)
    {
      Uri uri = new Uri(path.Text);
      //String pathString = path.Text;

      Player.BeginStop((ar) =>
      {
        Player.LoadMedia(uri);
        Player.Play();
      });
    }

    private void Play_Click(object sender, RoutedEventArgs e)
    {
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

    #endregion

  }
}
