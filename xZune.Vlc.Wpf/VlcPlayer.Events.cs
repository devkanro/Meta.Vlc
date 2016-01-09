//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: VlcPlayer.Events.cs
//Version: 20160109

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using xZune.Vlc.Interop.Media;

namespace xZune.Vlc.Wpf
{
    public partial class VlcPlayer
    {
        /// <summary>
        /// <see cref="VlcPlayer.Position"/>
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// <see cref="VlcPlayer.Time"/>
        /// </summary>
        public event EventHandler TimeChanged;

        /// <summary>
        /// <see cref="VlcPlayer.IsMute"/>
        /// </summary>
        public event EventHandler IsMuteChanged;

        /// <summary>
        /// <see cref="VlcPlayer.IsSeekableChanged"/>
        /// </summary>
        public event EventHandler IsSeekableChanged;

        /// <summary>
        /// <see cref="VlcPlayer.Volume"/>
        /// </summary>
        public event EventHandler VolumeChanged;

        /// <summary>
        /// <see cref="VlcPlayer.LengthChanged"/>
        /// </summary>
        public event EventHandler LengthChanged;

        /// <summary>
        /// <see cref="VlcPlayer.State"/>
        /// </summary>
        public event EventHandler<ObjectEventArgs<MediaState>> StateChanged;

        #region VlcMediaPlayer event handlers

        private void VlcMediaPlayerPositionChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                OnPropertyChanged(() => Position);
                if (PositionChanged != null)
                {
                    PositionChanged(this, new EventArgs());
                }
            }));
        }

        private void VlcMediaPlayerTimeChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                OnPropertyChanged(() => Time);
                if (TimeChanged != null)
                {
                    TimeChanged(this, new EventArgs());
                }
            }));
        }

        private void VlcMediaPlayerSeekableChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                OnPropertyChanged(() => IsSeekable);
                if (IsSeekableChanged != null)
                {
                    IsSeekableChanged(this, new EventArgs());
                }
            }));
        }

        private void VlcMediaPlayerStateChanged(object sender, EventArgs e)
        {
            Debug.WriteLine(String.Format("StateChanged : {0}", State));

            if (StateChanged != null)
                StateChanged(this, new ObjectEventArgs<MediaState>(State));
            Dispatcher.BeginInvoke(new Action(() =>
            {
                switch (State)
                {
                    case MediaState.Ended:
                        switch (EndBehavior)
                        {
                            case EndBehavior.Nothing:

                                break;

                            case EndBehavior.Stop:
                                Stop();
                                break;

                            case EndBehavior.Repeat:
                                Action repeatAction = () =>
                                {
                                    VlcMediaPlayer.Stop();
                                    VlcMediaPlayer.Play();
                                };
                                repeatAction.EasyInvoke();
                                break;
                        }
                        break;
                }
            }));
        }

        private void VlcMediaPlayerLengthChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                OnPropertyChanged(() => Length);
                if (LengthChanged != null)
                {
                    LengthChanged(this, new EventArgs());
                }
            }));
        }

        #endregion VlcMediaPlayer event handlers

        #region Callbacks

        #region Video callbacks

        private IntPtr VideoLockCallback(IntPtr opaque, ref IntPtr planes)
        {
            if (VlcMediaPlayer.Volume != Volume)
            {
                VlcMediaPlayer.Volume = Volume;
            }

            if (!_context.IsAspectRatioChecked)
            {
                var tracks = VlcMediaPlayer.Media.GetTracks();
                MediaTrack videoMediaTrack = tracks.FirstOrDefault(t => t.Type == TrackType.Video);

                if (videoMediaTrack != null && videoMediaTrack.Type == TrackType.Video)
                {
                    if (videoMediaTrack.VideoTrack != null)
                    {
                        _context.CheckDisplaySize(videoMediaTrack.VideoTrack.Value);
                        var scale = GetScaleTransform();

                        if (Math.Abs(scale.Width - 1.0) + Math.Abs(scale.Height - 1.0) > 0.0000001)
                        {
                            _context.IsAspectRatioChecked = true;
                            Debug.WriteLine(String.Format("Scale:{0}x{1}", scale.Width, scale.Height));
                            Debug.WriteLine(String.Format("Resize Image to {0}x{1}", _context.DisplayWidth,
                                _context.DisplayHeight));
                        }
                        else
                        {
                            _checkCount++;
                            if (_checkCount > 5)
                            {
                                _context.IsAspectRatioChecked = true;
                            }
                        }

                        Dispatcher.Invoke(new Action(() =>
                        {
                            ScaleTransform = new ScaleTransform(scale.Width, scale.Height);
                        }));
                    }
                }
            }
            return planes = _context.MapView;
        }

        private void VideoUnlockCallback(IntPtr opaque, IntPtr picture, ref IntPtr planes)
        {
            return;
        }

        private void VideoDisplayCallback(IntPtr opaque, IntPtr picture)
        {
            _context.Display();
            if (_snapshotContext == null) return;

            _snapshotContext.GetName(this);
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                switch (_snapshotContext.Format)
                {
                    case SnapshotFormat.BMP:
                        var bmpE = new BmpBitmapEncoder();
                        bmpE.Frames.Add(BitmapFrame.Create(VideoSource));
                        using (
                            Stream stream =
                                File.Create(String.Format("{0}\\{1}.bmp", _snapshotContext.Path, _snapshotContext.Name))
                            )
                        {
                            bmpE.Save(stream);
                        }
                        break;

                    case SnapshotFormat.JPG:
                        var jpgE = new JpegBitmapEncoder();
                        jpgE.Frames.Add(BitmapFrame.Create(VideoSource));
                        using (
                            Stream stream =
                                File.Create(String.Format("{0}\\{1}.jpg", _snapshotContext.Path, _snapshotContext.Name))
                            )
                        {
                            jpgE.QualityLevel = _snapshotContext.Quality;
                            jpgE.Save(stream);
                        }
                        break;

                    case SnapshotFormat.PNG:
                        var pngE = new PngBitmapEncoder();
                        pngE.Frames.Add(BitmapFrame.Create(VideoSource));
                        using (
                            Stream stream =
                                File.Create(String.Format("{0}\\{1}.png", _snapshotContext.Path, _snapshotContext.Name))
                            )
                        {
                            pngE.Save(stream);
                        }
                        break;
                }
                _snapshotContext = null;
            }));
        }

        private uint VideoFormatCallback(ref IntPtr opaque, ref uint chroma, ref uint width, ref uint height,
            ref uint pitches, ref uint lines)
        {
            Debug.WriteLine(String.Format("Initialize Video Content : {0}x{1}", width, height));
            if (_context == null)
            {
                _context = new VideoDisplayContext(width, height, ChromaType.RV32);
            }
            chroma = (uint)_context.ChromaType;
            width = (uint) _context.Width;
            height = (uint) _context.Height;
            pitches = (uint) _context.Stride;
            lines = (uint) _context.Height;
            Dispatcher.Invoke(new Action(() =>
            {
                VideoSource = _context.Image;
            }));
            return (uint) _context.Size;
        }

        private void VideoCleanupCallback(IntPtr opaque)
        {
        }

        #endregion

        #region Audio callbacks

        //private int AudioFormatSetupCallback(ref IntPtr opaque, IntPtr format, ref uint rate, ref uint channels)
        //{
        //    string formatStr = InteropHelper.PtrToString(format, 4);
        //    Debug.WriteLine(String.Format("Sound format: {0}",formatStr));
        //    Debug.WriteLine(String.Format("Sound rate: {0}", rate));
        //    Debug.WriteLine(String.Format("Sound channels: {0}", channels));

        //    return 0;
        //}

        //private void AudioFormatCleanupCallback(IntPtr opaque)
        //{

        //}

        //private void AudioPlayCallback(IntPtr opaque, IntPtr sample, uint count, Int64 pts)
        //{
        //    VlcMediaPlayer.Volume = Volume;
        //}

        //private void AudioPauseCallback(IntPtr opaque,  Int64 pts)
        //{

        //}

        //private void AudioResumeCallback(IntPtr opaque, Int64 pts)
        //{

        //}

        //private void AudioFlushCallback(IntPtr opaque, Int64 pts)
        //{

        //}

        //private void AudioDrainCallback(IntPtr opaque, Int64 pts)
        //{

        //}

        //private void AudioSetVolumeCallback(IntPtr opaque, float volume, bool mute)
        //{
            
        //}

        #endregion

        #endregion Callbacks

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if ((VlcMediaPlayer != null) && State == MediaState.Playing && (Vlc.LibDev == "xZune") && _isDVD) 
                VlcMediaPlayer.SetMouseCursor(0, GetVideoPositionX(e.GetPosition(this).X), GetVideoPositionY(e.GetPosition(this).Y));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if ((VlcMediaPlayer != null) && State == MediaState.Playing && (Vlc.LibDev == "xZune") && _isDVD)
                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        VlcMediaPlayer.SetMouseUp(0, Interop.MediaPlayer.MouseButton.Left);
                        break;

                    case MouseButton.Right:
                        VlcMediaPlayer.SetMouseUp(0, Interop.MediaPlayer.MouseButton.Right);
                        break;

                    case MouseButton.Middle:
                    case MouseButton.XButton1:
                    case MouseButton.XButton2:
                    default:
                        VlcMediaPlayer.SetMouseUp(0, Interop.MediaPlayer.MouseButton.Other);
                        break;
                }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if ((VlcMediaPlayer != null) && State == MediaState.Playing && (Vlc.LibDev == "xZune") && _isDVD)
                switch (e.ChangedButton)
                {
                    case MouseButton.Left:
                        VlcMediaPlayer.SetMouseDown(0, Interop.MediaPlayer.MouseButton.Left);
                        break;

                    case MouseButton.Right:
                        VlcMediaPlayer.SetMouseDown(0, Interop.MediaPlayer.MouseButton.Right);
                        break;

                    case MouseButton.Middle:
                    case MouseButton.XButton1:
                    case MouseButton.XButton2:
                    default:
                        VlcMediaPlayer.SetMouseDown(0, Interop.MediaPlayer.MouseButton.Other);
                        break;
                }
        }
    }
}