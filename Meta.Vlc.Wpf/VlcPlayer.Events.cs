// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcPlayer.Events.cs
// Version: 20160708

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

        public event EventHandler ThreadSeparatedImageLoaded;

        /// <summary>
        ///     <see cref="VlcPlayer.State" />
        /// </summary>
        public event EventHandler<ObjectEventArgs<MediaState>> StateChanged;

        public event EventHandler<VideoFormatChangingEventArgs> VideoFormatChanging;

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

        private void CheckAspectRatio()
        {
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
        }

        private void TakeSnapshot()
        {
            DisplayThreadDispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                if (_snapshotContext != null)
                {
                    _snapshotContext.Save(this, this.VideoSource);
                    _snapshotContext = null;
                }
            }));
        }
        
        private IntPtr VideoLockCallback(IntPtr opaque, ref IntPtr planes)
        {
            if (_context == null)
            {
                throw new NullReferenceException("Video context is null");
            }

            if (VlcMediaPlayer.Volume != Volume)
            {
                VlcMediaPlayer.Volume = Volume;
            }

            if (VideoSource == null)
            {
                VideoSource = _context.Image;
            }

            try
            {
                CheckAspectRatio();
            }
            catch
            {
                // ignored
            }

            return planes = _context.MapView;
        }

        private void VideoUnlockCallback(IntPtr opaque, IntPtr picture, ref IntPtr planes)
        {
        }

        private void VideoDisplayCallback(IntPtr opaque, IntPtr picture)
        {
            if (_context == null || DisplayThreadDispatcher == null)
            {
                return;
            }

            _context.Display();

            try
            {
                TakeSnapshot();
            }
            catch
            {
                // ignored
            }
        }

        private uint VideoFormatCallback(ref IntPtr opaque, ref uint chroma, ref uint width, ref uint height,
            ref uint pitches, ref uint lines)
        {
            Debug.WriteLine(String.Format("Initialize Video Content : {0}x{1}", width, height));

            var videoFormatChangingArgs = new VideoFormatChangingEventArgs(width, height, ChromaType.RV32);

            if (VideoFormatChanging != null)
            {
                VideoFormatChanging(this, videoFormatChangingArgs);
            }

            if (_context == null || videoFormatChangingArgs.Width != _context.Width || videoFormatChangingArgs.Height != _context.Height)
            {
                if (DisplayThreadDispatcher == null)
                {
                    throw new NullReferenceException(String.Format("Image = {0}, Image.SeparateThreadDispatcher = {1}, ThreadSeparatedImage.CommonDispatcher = {2}", Image, Image.SeparateThreadDispatcher, ThreadSeparatedImage.CommonDispatcher));
                }
                DisplayThreadDispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        if (_context != null)
                        {
                            _context.Dispose();
                        }
                        _context = new VideoDisplayContext(videoFormatChangingArgs.Width, videoFormatChangingArgs.Height, videoFormatChangingArgs.ChromaType);
						VideoSource = null;
                    }));
            }
            
            _context.IsAspectRatioChecked = false;
            chroma = (uint) _context.ChromaType;
            width = (uint) _context.Width;
            height = (uint) _context.Height;
            pitches = (uint) _context.Stride;
            lines = (uint) _context.Height;
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

    public class VideoFormatChangingEventArgs : EventArgs
    {
        internal VideoFormatChangingEventArgs(uint width, uint height, ChromaType chromaType)
        {
            Width = width;
            Height = height;
            ChromaType = chromaType;
        }

        /// <summary>
        /// Width of video.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// Height of video.
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Video chroma type.
        /// </summary>
        public ChromaType ChromaType { get; set; }
    }
}