//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: StopRequest.cs
//Version: 20151220

using System;
using System.Windows.Threading;
using xZune.Vlc.Interop.Media;

namespace xZune.Vlc.Wpf
{
    internal class StopRequest : IDisposable
    {
        private Action _callBack;

        private VlcPlayer _player;

        private MediaState _oldState;

        public StopRequest(VlcPlayer player, Action callback)
        {
            _player = player;
            _callBack = callback;

            _oldState = _player.VlcMediaPlayer.State;

            _player.VlcMediaPlayer.Playing += _player_SateChanged;
            _player.VlcMediaPlayer.Paused += _player_SateChanged;
            _player.VlcMediaPlayer.Stoped += _player_SateChanged;
        }

        public void Send()
        {
            switch (_player.VlcMediaPlayer.State)
            {
                case MediaState.Opening:
                case MediaState.Buffering:
                    break;

                case MediaState.Playing:
                    _player.VlcMediaPlayer.Pause();

                    break;

                case MediaState.Paused:
                case MediaState.Ended:
                    _player.VlcMediaPlayer.Stop();

                    break;

                default:
                    Dispose();
                    _player.Dispatcher.BeginInvoke(DispatcherPriority.Normal, _callBack);
                    break;
            }
        }

        private void _player_SateChanged(object sender, EventArgs e)
        {
            if (_player.VlcMediaPlayer.State != _oldState)
            {
                _oldState = _player.VlcMediaPlayer.State;
                _player.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(Send));
            }
        }

        public void Dispose()
        {
            _player.VlcMediaPlayer.Playing -= _player_SateChanged;
            _player.VlcMediaPlayer.Paused -= _player_SateChanged;
            _player.VlcMediaPlayer.Stoped -= _player_SateChanged;
        }
    }
}