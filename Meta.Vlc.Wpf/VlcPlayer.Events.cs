// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcPlayer.Events.cs
// Version: 20160404

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Meta.Vlc.Interop.Media;

namespace Meta.Vlc.Wpf
{
    public partial class VlcPlayer
    {
        /// <summary>
        ///     <see cref="VlcPlayer.Position" />
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        ///     <see cref="VlcPlayer.Time" />
        /// </summary>
        public event EventHandler TimeChanged;

        /// <summary>
        ///     <see cref="VlcPlayer.IsMute" />
        /// </summary>
        public event EventHandler IsMuteChanged;

        /// <summary>
        ///     <see cref="VlcPlayer.IsSeekableChanged" />
        /// </summary>
        public event EventHandler IsSeekableChanged;

        /// <summary>
        ///     <see cref="VlcPlayer.Volume" />
        /// </summary>
        public event EventHandler VolumeChanged;

        /// <summary>
        ///     <see cref="VlcPlayer.LengthChanged" />
        /// </summary>
        public event EventHandler LengthChanged;

        /// <summary>
        ///     <see cref="VlcPlayer.State" />
        /// </summary>
        public event EventHandler<ObjectEventArgs<MediaState>> StateChanged;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isDVD && (VlcMediaPlayer != null) && State == MediaState.Playing &&
                (LibVlcManager.LibVlcVersion.DevString == "Meta"))
                VlcMediaPlayer.SetMouseCursor(0, GetVideoPositionX(e.GetPosition(this).X),
                    GetVideoPositionY(e.GetPosition(this).Y));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isDVD && (VlcMediaPlayer != null) && State == MediaState.Playing &&
                (LibVlcManager.LibVlcVersion.DevString == "Meta"))
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

            if (_isDVD && (VlcMediaPlayer != null) && State == MediaState.Playing &&
                (LibVlcManager.LibVlcVersion.DevString == "Meta"))
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

        #region VlcMediaPlayer event handlers

        private void VlcMediaPlayerPositionChanged(object sender, EventArgs e)
        {
            if (_disposing || _isStopping) return;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
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
            if (_disposing || _isStopping) return;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
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
            if (_disposing || _isStopping) return;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                OnPropertyChanged(() => IsSeekable);
                if (IsSeekableChanged != null)
                {
                    IsSeekableChanged(this, new EventArgs());
                }
            }));
        }

        private void VlcMediaPlayerEndReached(object sender, ObjectEventArgs<MediaState> e)
        {
            if (_disposing || _isStopping) return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                switch (EndBehavior)
                {
                    case EndBehavior.Nothing:

                        break;

                    case EndBehavior.Stop:
                        Stop();
                        break;

                    case EndBehavior.Repeat:
                        StopInternal();
                        VlcMediaPlayer.Play();
                        break;
                }
            }));
        }

        private void VlcMediaPlayerLengthChanged(object sender, EventArgs e)
        {
            if (_disposing || _isStopping) return;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                OnPropertyChanged(() => Length);
                if (LengthChanged != null)
                {
                    LengthChanged(this, new EventArgs());
                }
            }));
        }

        private void VlcMediaPlayerMediaChanged(object sender, MediaPlayerMediaChangedEventArgs e)
        {
            if (_oldMedia != null)
            {
                _oldMedia.StateChanged -= MediaStateChanged;
            }

            if (e.NewMedia != null)
            {
                e.NewMedia.StateChanged += MediaStateChanged;
                _oldMedia = e.NewMedia;
            }
        }

        private void MediaStateChanged(object sender, ObjectEventArgs<Interop.Core.Events.MediaStateChangedArgs> e)
        {
            if (_disposing || _isStopping) return;

            Debug.WriteLine(String.Format("StateChanged : {0}", e.Value.NewState));

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
             {
                 if (StateChanged != null)
                     StateChanged(this, new ObjectEventArgs<MediaState>(e.Value.NewState));
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
                var videoMediaTracks = tracks.OfType<VideoTrack>().ToList();
                var videoTrack = videoMediaTracks.FirstOrDefault();

                if (videoTrack != null)
                {
                    _context.CheckDisplaySize(videoTrack);
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

                    if (DisplayThreadDispatcher != null)
                    {
                        DisplayThreadDispatcher.BeginInvoke(
                            new Action(() => { ScaleTransform = new ScaleTransform(scale.Width, scale.Height); }));
                    }
                }
            }
            return planes = _context.MapView;
        }

        private void VideoUnlockCallback(IntPtr opaque, IntPtr picture, ref IntPtr planes)
        {
        }

        private void VideoDisplayCallback(IntPtr opaque, IntPtr picture)
        {
            _context.Display();
            if (_snapshotContext == null) return;

            _snapshotContext.GetName(this);

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
        }

        private uint VideoFormatCallback(ref IntPtr opaque, ref uint chroma, ref uint width, ref uint height,
            ref uint pitches, ref uint lines)
        {
            Debug.WriteLine(String.Format("Initialize Video Content : {0}x{1}", width, height));
            if (_context == null)
            {
                uint tmpWidth = width;
                uint tmpHeight = height;

                if (DisplayThreadDispatcher == null)
                {
                    throw new InvalidOperationException("VlcPlayer not be ready, if you want to use VlcPlay no in XAML, please read this Wiki: \"https://github.com/higankanshi/Meta.Vlc/wiki/Use-VlcPlayer-with-other-controls\".");
                }

                DisplayThreadDispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => { _context = new VideoDisplayContext(tmpWidth, tmpHeight, ChromaType.RV32); }));
            }
            chroma = (uint) _context.ChromaType;
            width = (uint) _context.Width;
            height = (uint) _context.Height;
            pitches = (uint) _context.Stride;
            lines = (uint) _context.Height;
            VideoSource = _context.Image;
            return (uint) _context.Size;
        }

        private void VideoCleanupCallback(IntPtr opaque)
        {
        }

        #endregion Video callbacks

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

        #endregion Audio callbacks

        #endregion Callbacks
    }
}