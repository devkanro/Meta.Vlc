using System.Threading;
using System.Windows;

namespace Meta.Vlc.Wpf.Sample
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    Thread thread = new Thread(() =>
        //        {
        //            MainWindow w = new MainWindow();

        //            w.Show();
        //            w.Closed += (sender2, e2) => w.Dispatcher.InvokeShutdown();
        //            System.Windows.Threading.Dispatcher.Run();
        //        });

        //    thread.SetApartmentState(ApartmentState.STA);
        //    thread.Start();
        //}
    }
}