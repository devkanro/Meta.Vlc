//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: VlcPlayer.cs
//Version: 20151112

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using xZune.Vlc.Interop.Media;
using xZune.Vlc.Interop.MediaPlayer;
using MediaState = xZune.Vlc.Interop.Media.MediaState;
using MouseButton = System.Windows.Input.MouseButton;

namespace xZune.Vlc.Wpf
{
    public class VlcPlayer : Control, IDisposable
    {
        #region --- Fields ---

        //TODO: maybe make all fields private or protected (for descendant classes to access)?

        private VideoLockCallback _lockCallback;
        private VideoUnlockCallback _unlockCallback;
        private VideoDisplayCallback _displayCallback;
        private VideoFormatCallback _formatCallback;
        private VideoCleanupCallback _cleanupCallback;

        private GCHandle _lockCallbackHandle;
        private GCHandle _unlockCallbackHandle;
        private GCHandle _displayCallbackHandle;
        private GCHandle _formatCallbackHandle;
        private GCHandle _cleanupCallbackHandle;

        //Position//
        private bool _setVlcPosition = true;

        //Time//
        private bool _setVlcTime = true;

        //FPS//
        private System.Windows.Threading.DispatcherTimer _timer;

        private int _fpsCount;
        private bool _isRunFps;

        //TakeSnapshot//
        private SnapshotContext _snapshotContext;

        private VideoDisplayContext _context;
        private int _checkCount = 0;
        private Size _sar = new Size(1, 1);

        //Stop//
        private EventWaitHandle _stopWaitHandle;

        //Dispose//
        private bool _disposed = false;

        #endregion --- Fields ---

        #region --- Initialization ---

        static VlcPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlcPlayer), new FrameworkPropertyMetadata(typeof(VlcPlayer)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            ScaleTransform = new ScaleTransform(1, 1);
            _stopWaitHandle = new AutoResetEvent(false);
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (!ApiManager.IsInitialized)
                {
                    String libVlcPath = null;
                    String[] libVlcOption = null;

                    var vlcSettings =
                        System.Reflection.Assembly.GetEntryAssembly()
                            .GetCustomAttributes(typeof(VlcSettingsAttribute), false);

                    if (vlcSettings.Length > 0)
                    {
                        var vlcSettingsAttribute = vlcSettings[0] as VlcSettingsAttribute;

                        if (vlcSettingsAttribute != null && vlcSettingsAttribute.LibVlcPath != null)
                        {
                            libVlcPath = Path.IsPathRooted(vlcSettingsAttribute.LibVlcPath)
                                ? vlcSettingsAttribute.LibVlcPath
                                : CombinePath(
                                    Path.GetDirectoryName(
                                        System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName),
                                    vlcSettingsAttribute.LibVlcPath);
                        }

                        if (vlcSettingsAttribute != null && vlcSettingsAttribute.VlcOption != null)
                            libVlcOption = vlcSettingsAttribute.VlcOption;
                    }

                    if (LibVlcPath != null)
                        libVlcPath = Path.IsPathRooted(LibVlcPath) ? LibVlcPath : CombinePath(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), LibVlcPath);

                    if (VlcOption != null)
                        libVlcOption = VlcOption;

                    if (libVlcPath != null)
                        Initialize(libVlcPath, libVlcOption);
                }
            }
            base.OnInitialized(e);
        }

        public void Initialize(String libVlcPath)
        {
            Initialize(libVlcPath, new[]
            {
      #if DEBUG
                "-I", "dummy", "--ignore-config", "--no-video-title","--file-logging","--logfile=log.txt","--verbose=2","--no-sub-autodetect-file"
      #else
                "-I", "dummy", "--dummy-quiet", "--ignore-config", "--no-video-title", "--no-sub-autodetect-file"
      #endif
      });
        }

        public void Initialize(String libVlcPath, params String[] libVlcOption)
        {
            if (ApiManager.IsInitialized) return;

            ApiManager.Initialize(libVlcPath, libVlcOption);

            VlcMediaPlayer = ApiManager.Vlc.CreateMediaPlayer();
            if (VlcMediaPlayer != null)
            {
                VlcMediaPlayer.PositionChanged += VlcMediaPlayerPositionChanged;
                VlcMediaPlayer.TimeChanged += VlcMediaPlayerTimeChanged;
                VlcMediaPlayer.Playing += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Paused += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Stoped += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Opening += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Buffering += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.EndReached += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.SeekableChanged += VlcMediaPlayerSeekableChanged;
                VlcMediaPlayer.LengthChanged += VlcMediaPlayerLengthChanged;

                _lockCallback = VideoLockCallback;
                _unlockCallback = VideoUnlockCallback;
                _displayCallback = VideoDisplayCallback;
                _formatCallback = VideoFormatCallback;
                _cleanupCallback = VideoCleanupCallback;

                _lockCallbackHandle = GCHandle.Alloc(_lockCallback);
                _unlockCallbackHandle = GCHandle.Alloc(_unlockCallback);
                _displayCallbackHandle = GCHandle.Alloc(_displayCallback);
                _formatCallbackHandle = GCHandle.Alloc(_formatCallback);
                _cleanupCallbackHandle = GCHandle.Alloc(_cleanupCallback);
            }
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            BeginStop(ar =>
            {
                if (VlcMediaPlayer != null)
                {
                    if (VlcMediaPlayer.Media != null)
                    {
                        VlcMediaPlayer.Media.Dispose();
                    }
                    VlcMediaPlayer.Dispose();
                }

                _lockCallbackHandle.Free();
                _unlockCallbackHandle.Free();
                _displayCallbackHandle.Free();
                _formatCallbackHandle.Free();
                _cleanupCallbackHandle.Free();

                _disposed = true;
            });
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion --- Cleanup ---

        #region --- Properties ---

        #region LibVlcPath

        public String LibVlcPath
        {
            get { return (String)GetValue(LibVlcPathProperty); }
            set { SetValue(LibVlcPathProperty, value); }
        }

        public static readonly DependencyProperty LibVlcPathProperty =
          DependencyProperty.Register("LibVlcPath", typeof(String), typeof(VlcPlayer),
            new PropertyMetadata(OnLibVlcPathChanged));

        private static void OnLibVlcPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnLibVlcPathChanged(new EventArgs());
        }

        /// <summary>
        /// <see cref="VlcPlayer.LibVlcPathChanged"/>
        /// </summary>
        protected void OnLibVlcPathChanged(EventArgs e)
        {
            if (LibVlcPathChanged != null)
                LibVlcPathChanged(this, e);
        }

        #endregion LibVlcPath

        #region VlcOption

        public String[] VlcOption
        {
            get { return (String[])GetValue(VlcOptionProperty); }
            set { SetValue(VlcOptionProperty, value); }
        }

        public static readonly DependencyProperty VlcOptionProperty =
          DependencyProperty.Register("VlcOption", typeof(String[]), typeof(VlcPlayer),
            new PropertyMetadata(OnVlcOptionChanged));

        private static void OnVlcOptionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnVlcOptionChanged(new EventArgs());
        }

        /// <summary>
        /// <see cref="VlcPlayer.VlcOptionChanged"/>
        /// </summary>
        protected void OnVlcOptionChanged(EventArgs e)
        {
            if (VlcOptionChanged != null)
                VlcOptionChanged(this, e);
        }

        #endregion VlcOption

        #region ScaleTransform

        internal static readonly DependencyProperty ScaleTransformProperty =
          DependencyProperty.Register("ScaleTransform", typeof(ScaleTransform), typeof(VlcPlayer),
            new PropertyMetadata(default(ScaleTransform)));

        internal ScaleTransform ScaleTransform
        {
            get { return (ScaleTransform)GetValue(ScaleTransformProperty); }
            set { SetValue(ScaleTransformProperty, value); }
        }

        #endregion ScaleTransform

        #region AspectRatio

        public static readonly DependencyProperty PropertyTypeProperty =
          DependencyProperty.Register("PropertyType", typeof(AspectRatio), typeof(VlcPlayer),
            new PropertyMetadata(AspectRatio.Default, OnAspectRatioChanged));

        public AspectRatio AspectRatio
        {
            get { return (AspectRatio)GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        private static void OnAspectRatioChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;

            var scale = vlcPlayer.GetScaleTransform();
            vlcPlayer.ScaleTransform = new ScaleTransform(scale.Width, scale.Height);

            if (vlcPlayer != null) vlcPlayer.OnAspectRatioChanged(new EventArgs());
        }

        /// <summary>
        /// <see cref="VlcPlayer.AspectRatioChanged"/>
        /// </summary>
        protected void OnAspectRatioChanged(EventArgs e)
        {
            if (AspectRatioChanged != null)
                AspectRatioChanged(this, e);
        }

        #endregion AspectRatio

        #region VideoSource

        public InteropBitmap VideoSource
        {
            get { return (InteropBitmap)GetValue(VideoSourceProperty); }
            private set { SetValue(VideoSourceProperty, value); }
        }

        public static readonly DependencyProperty VideoSourceProperty =
            DependencyProperty.Register("VideoSource", typeof(InteropBitmap), typeof(VlcPlayer),
              new PropertyMetadata(OnVideoSourceChanged));

        private static void OnVideoSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null)
                vlcPlayer.OnVideoSourceChanged(new EventArgs());
        }

        /// <summary>
        /// <see cref="VlcPlayer.VideoSourceChanged"/>
        /// </summary>
        protected void OnVideoSourceChanged(EventArgs e)
        {
            if (VideoSourceChanged != null)
                VideoSourceChanged(this, e);
        }

        #endregion VideoSource

        #region Position

        public float Position
        {
            get { return (float)GetValue(PositionProperty); }
            set { SetPosition(value, true); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(float), typeof(VlcPlayer),
              new PropertyMetadata(0.0f, OnPositionChanged));

        private static void OnPositionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnPositionChanged(e);
        }

        /// <summary>
        /// <see cref="VlcPlayer.PositionChanged"/>
        /// </summary>
        protected void OnPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_setVlcPosition && (VlcMediaPlayer != null))
                VlcMediaPlayer.Position = (float)e.NewValue;

            _setVlcPosition = true;

            if (PositionChanged != null)
                PositionChanged(this, e);
        }

        #endregion Position

        #region Time

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetTime(value, true); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(VlcPlayer),
              new PropertyMetadata(TimeSpan.Zero, OnTimeChanged));

        private static void OnTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnTimeChanged(e);
        }

        /// <summary>
        /// <see cref="VlcPlayer.TimeChanged"/>
        /// </summary>
        protected void OnTimeChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_setVlcTime && (VlcMediaPlayer != null))
                VlcMediaPlayer.Time = (TimeSpan)e.NewValue;

            _setVlcTime = true;

            if (TimeChanged != null)
                TimeChanged(this, e);
        }

        #endregion Time

        #region Stretch

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(VlcPlayer), new PropertyMetadata(Stretch.Uniform));

        public StretchDirection StretchDirection
        {
            get { return (StretchDirection)GetValue(StretchDirectionProperty); }
            set { SetValue(StretchDirectionProperty, value); }
        }

        public static readonly DependencyProperty StretchDirectionProperty =
            DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(VlcPlayer),
              new PropertyMetadata(StretchDirection.Both));

        #endregion Stretch

        #region FPS

        /// <summary>
        /// 获取一个整数,该值表示一秒内呈现的图片数量,此属性反映了当前的视频刷新率,更新间隔为1秒,要想开始FPS计数,请使用
        /// <see cref="VlcPlayer.StartFPS"/> 方法
        /// </summary>
        public int FPS
        {
            get { return (int)GetValue(FPSProperty); }
            private set { SetValue(FPSProperty, value); }
        }

        public static readonly DependencyProperty FPSProperty =
            DependencyProperty.Register("FPS", typeof(int), typeof(VlcPlayer), new PropertyMetadata(0, OnFPSChanged));

        private static void OnFPSChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnFPSChanged(new EventArgs());
        }

        /// <summary>
        /// <see cref="VlcPlayer.FPSChanged"/>
        /// </summary>
        protected void OnFPSChanged(EventArgs e)
        {
            if (FPSChanged != null)
                FPSChanged(this, e);
        }

        #endregion FPS

        #region IsMute

        public bool IsMute
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.IsMute : false; }
            set
            {
                if ((VlcMediaPlayer == null) || (value == VlcMediaPlayer.IsMute)) return;
                VlcMediaPlayer.IsMute = value;
                if (IsMuteChanged != null) IsMuteChanged.Invoke(this, new EventArgs());
            }
        }

        #endregion IsMute

        #region Volume

        public int Volume
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.Volume : 0; }
            set
            {
                if ((VlcMediaPlayer == null) || (value == VlcMediaPlayer.Volume)) return;
                VlcMediaPlayer.Volume = value;
                if (VolumeChanged != null) VolumeChanged.Invoke(this, new EventArgs());
            }
        }

        #endregion Volume

        #region AudioOutputChannel

        public AudioOutputChannel AudioOutputChannel
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.AudioOutputChannel : AudioOutputChannel.Error; }
            set { if (VlcMediaPlayer != null) VlcMediaPlayer.AudioOutputChannel = value; }
        }

        #endregion AudioOutputChannel

        #region AudioTrackCount

        public int AudioTrackCount
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.AudioTrackCount : 0; }
        }

        #endregion AudioTrackCount

        #region AudioTrack

        public int AudioTrack
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.AudioTrack : -1; } //note: assuming a 0-based index
            set { if (VlcMediaPlayer != null) VlcMediaPlayer.AudioTrack = value; }
        }

        #endregion AudioTrack

        #region AudioTrackDescription

        public TrackDescription AudioTrackDescription
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.AudioTrackDescription : null; }
        }

        #endregion AudioTrackDescription

        #region Rate

        public float Rate
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.Rate : 0; }
            set { if (VlcMediaPlayer != null) VlcMediaPlayer.Rate = value; }
        }

        #endregion Rate

        #region Title

        public int Title
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.Title : -1; } //note: assuming a 0-based index
            set { if (VlcMediaPlayer != null) VlcMediaPlayer.Title = value; }
        }

        #endregion Title

        #region TitleCount

        public int TitleCount
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.TitleCount : 0; }
        }

        #endregion TitleCount

        #region Chapter

        public int Chapter
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.Chapter : -1; } //note: assuming a 0-based index
            set { if (VlcMediaPlayer != null) VlcMediaPlayer.Chapter = value; }
        }

        #endregion Chapter

        #region ChapterCount

        public int ChapterCount
        {
            get { return (VlcMediaPlayer != null) ? VlcMediaPlayer.ChapterCount : 0; }
        }

        #endregion ChapterCount

        #region IsSeekable

        /// <summary>
        /// Checks if media is seekable
        /// </summary>
        public bool IsSeekable { get; private set; }

        #endregion IsSeekable

        #region State

        /// <summary>
        /// Current media state
        /// </summary>
        public MediaState State { get; private set; }

        #endregion State

        #region Length

        /// <summary>
        /// Length of current media
        /// </summary>
        public TimeSpan Length { get; private set; }

        #endregion Length

        #region VlcMediaPlayer

        public VlcMediaPlayer VlcMediaPlayer { get; private set; }

        #endregion VlcMediaPlayer

        #endregion --- Properties ---

        #region --- Methods ---

        #region Path Helpers

        private static String CombinePath(String path1, String path2)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(path1, path2));
            return dir.FullName;
        }

        internal static bool IsRootPath(string path)
        {
            path = path.Replace('\\', '/');
            if (path[path.Length - 1] != '/')
            {
                path += '/';
            }
            int index;
            return (index = path.IndexOf(":/", StringComparison.Ordinal)) != -1 && index + 2 == path.Length;
        }

        internal static bool IsNetwork(Uri uri)
        {
            return uri.Scheme.ToLower() != "file";
        }

        #endregion Path Helpers

        #region Property Helpers

        private Size GetScaleTransform()
        {
            if (_context == null) return new Size(1.0, 1.0);

            AspectRatio aspectRatio = AspectRatio.Default;

            Dispatcher.Invoke(new Action(() =>
            {
                aspectRatio = AspectRatio;
            }));

            Size scale = new Size(_context.DisplayWidth / _context.Width, _context.DisplayHeight / _context.Height);

            switch (aspectRatio)
            {
                case AspectRatio.Default:
                    return scale;

                case AspectRatio._16_9:
                    return new Size(1.0 * _context.DisplayHeight / 9 * 16 / _context.Width, 1.0 * _context.DisplayHeight / _context.Height);

                case AspectRatio._4_3:
                    return new Size(1.0 * _context.DisplayHeight / 3 * 4 / _context.Width, 1.0 * _context.DisplayHeight / _context.Height);
            }
            return new Size(1.0, 1.0);
        }

        private void SetPosition(float pos, bool setVlcPosition)
        {
            _setVlcPosition = setVlcPosition;
            Dispatcher.Invoke(new Action(() => SetValue(PositionProperty, pos)));
        }

        private void SetTime(TimeSpan time, bool setVlcTime)
        {
            _setVlcTime = setVlcTime;
            Dispatcher.Invoke(new Action(() => SetValue(TimeProperty, time)));
        }

        #endregion Property Helpers

        #region Coordinate Helpers

        private int GetVideoPositionX(double x)
        {
            if (_context == null)
            {
                return (int)x;
            }
            double width = _context.Width * ScaleTransform.ScaleX,
                height = _context.Height * ScaleTransform.ScaleY;
            var px = 0;
            double scale, scaleX, scaleY;
            switch (this.Stretch)
            {
                case Stretch.None:
                    switch (this.HorizontalContentAlignment)
                    {
                        case HorizontalAlignment.Left:
                            px = (int)x;
                            break;

                        case HorizontalAlignment.Center:
                            px = (int)(x - ((this.ActualWidth - width) / 2));
                            break;

                        case HorizontalAlignment.Right:
                            px = (int)(x - (this.ActualWidth - width));
                            break;

                        case HorizontalAlignment.Stretch:
                            if (this.ActualWidth > width)
                            {
                                px = (int)(x - ((this.ActualWidth - width) / 2));
                            }
                            else
                            {
                                px = (int)x;
                            }
                            break;
                    }
                    break;

                case Stretch.Fill:
                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (this.ActualWidth > width)
                            {
                                px = (int)(x / this.ActualWidth * width);
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (this.ActualWidth < width)
                            {
                                px = (int)(x / this.ActualWidth * width);
                            }
                            break;

                        case StretchDirection.Both:
                            px = (int)(x / this.ActualWidth * width);
                            break;
                    }
                    break;

                case Stretch.Uniform:
                    scaleX = this.ActualWidth / width;
                    scaleY = this.ActualHeight / height;
                    scale = Math.Min(scaleX, scaleY);

                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (this.HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((this.ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (this.HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((this.ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleX == scale)
                            {
                                px = (int)(x / scale);
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)(x / scale);
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                        break;
                                }
                            }
                            break;
                    }
                    break;

                case Stretch.UniformToFill:
                    scaleX = ActualWidth / width;
                    scaleY = ActualHeight / height;
                    scale = Math.Max(scaleX, scaleY);

                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (this.HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)(x / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((this.ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)(x / scale);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (ActualWidth > width)
                                        {
                                            px = (int)(x - ((ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleX == scale)
                            {
                                px = (int)(x / scale);
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)(x / scale);
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        px = (int)(x / scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return px;
        }

        private int GetVideoPositionY(double y)
        {
            double width = _context.Width * ScaleTransform.ScaleX,
                height = _context.Height * ScaleTransform.ScaleY;
            int py = 0;
            double scale, scaleX, scaleY;
            switch (this.Stretch)
            {
                case Stretch.None:
                    switch (this.VerticalContentAlignment)
                    {
                        case VerticalAlignment.Top:
                            py = (int)y;
                            break;

                        case VerticalAlignment.Center:
                            py = (int)(y - ((this.ActualHeight - height) / 2));
                            break;

                        case VerticalAlignment.Bottom:
                            py = (int)(y - (this.ActualHeight - height));
                            break;

                        case VerticalAlignment.Stretch:
                            if (this.ActualHeight > height)
                            {
                                py = (int)(y - ((this.ActualHeight - height) / 2));
                            }
                            else
                            {
                                py = (int)y;
                            }
                            break;
                    }
                    break;

                case Stretch.Fill:
                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (this.ActualHeight > height)
                            {
                                py = (int)(y / this.ActualHeight * height);
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (this.ActualHeight < height)
                            {
                                py = (int)(y / this.ActualHeight * height);
                            }
                            break;

                        case StretchDirection.Both:
                            py = (int)(y / this.ActualHeight * height);
                            break;
                    }
                    break;

                case Stretch.Uniform:
                    scaleX = this.ActualWidth / width;
                    scaleY = this.ActualHeight / height;
                    scale = Math.Min(scaleX, scaleY);

                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleY == scale)
                            {
                                py = (int)(y / scale);
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)(y / scale);
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                        break;

                                    case VerticalAlignment.Stretch:
                                        py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;

                case Stretch.UniformToFill:
                    scaleX = this.ActualWidth / width;
                    scaleY = this.ActualHeight / height;
                    scale = Math.Max(scaleX, scaleY);

                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)(y / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)(y / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleY == scale)
                            {
                                py = (int)(y / scale);
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)(y / scale);
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                        break;

                                    case VerticalAlignment.Stretch:
                                        py = (int)(y / scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return py;
        }

        #endregion Coordinate Helpers

        #region FPS

        /// <summary>
        /// 开始FPS计数
        /// </summary>
        public void StartFPS()
        {
            if (_isRunFps) return;
            _timer = new System.Windows.Threading.DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            _timer.Tick += FPSTick;
            _isRunFps = true;
            _fpsCount = 0;
            _timer.Start();
        }

        /// <summary>
        /// 停止FPS计数
        /// </summary>
        public void StopFPS()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer = null;
            _isRunFps = false;
        }

        private void FPSTick(object sender, EventArgs e)
        {
            FPS = _fpsCount;
            _fpsCount = 0;
        }

        #endregion FPS

        #region LoadMedia

        //note: if you pass a string instead of a Uri, LoadMedia will see if it is an absolute Uri, else will treat it as a file path
        public void LoadMedia(String path)
        {
            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                LoadMedia(uri);
                return;
            }

            if (!(File.Exists(path) || IsRootPath(Path.GetFullPath(path))))
                throw new FileNotFoundException(String.Format("Not found: {0}", path), path);

            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormPath(path);
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void LoadMedia(Uri uri)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormLocation(uri.ToString());
            VlcMediaPlayer.Media.ParseAsync();
        }

        [Obsolete]
        public void LoadMediaWithOptions(String path, String options)
        {
            if (!(File.Exists(path) || IsRootPath(Path.GetFullPath(path))))
                throw new FileNotFoundException(String.Format("Not found: {0}", path), path);

            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormPath(path);
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
        }

        [Obsolete]
        public void LoadMediaWithOptions(Uri uri, String options)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormLocation(uri.ToString());
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void LoadMediaWithOptions(String path, params String[] options)
        {
            if (!(File.Exists(path) || IsRootPath(Path.GetFullPath(path))))
                throw new FileNotFoundException(String.Format("Not found: {0}", path), path);

            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormPath(path);
            VlcMediaPlayer.Media.AddOption(String.Join(" ", options));
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void LoadMediaWithOptions(Uri uri, params String[] options)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormLocation(uri.ToString());
            VlcMediaPlayer.Media.AddOption(String.Join(" ", options));
            VlcMediaPlayer.Media.ParseAsync();
        }

        #endregion LoadMedia

        #region Play/Pause

        public void Play()
        {
            if (VlcMediaPlayer == null) return;

            VlcMediaPlayer.SetVideoDecodeCallback(_lockCallback, _unlockCallback, _displayCallback, IntPtr.Zero);
            VlcMediaPlayer.SetVideoFormatCallback(_formatCallback, _cleanupCallback);

            VlcMediaPlayer.Play();
        }

        public void PauseOrResume()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.PauseOrResume();
        }

        #endregion Play/Pause

        #region Stop

        public void BeginStop(AsyncCallback callback)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media == null)
            {
                callback(null);
                return;
            }

            Action action = new Action(() =>
            {
                VlcMediaPlayer.SetVideoDecodeCallback(null, null, null, IntPtr.Zero);
                VlcMediaPlayer.SetVideoFormatCallback(null, null);

                if (VlcMediaPlayer.State == MediaState.Playing)
                {
                    _stopWaitHandle.Reset();
                    VlcMediaPlayer.Pause();
                    _stopWaitHandle.WaitOne();
                }
            });

            var asyncResult = action.BeginInvoke((aresult) =>
            {
                VlcMediaPlayer.Stop();
                if (_context != null) _context.Dispose();
                _context = null;

                Dispatcher.BeginInvoke(new Action(() => VideoSource = null));

                callback(aresult);
                action.EndInvoke(aresult);
            }, null);
        }

        public IAsyncResult Stop()
        {
            if ((VlcMediaPlayer == null) || (VlcMediaPlayer.Media == null)) return null;

            Action action = new Action(() =>
            {
                VlcMediaPlayer.SetVideoDecodeCallback(null, null, null, IntPtr.Zero);
                VlcMediaPlayer.SetVideoFormatCallback(null, null);

                if (VlcMediaPlayer.State == MediaState.Playing)
                {
                    _stopWaitHandle.Reset();
                    VlcMediaPlayer.Pause();
                    _stopWaitHandle.WaitOne();
                }
            });

            return action.BeginInvoke((aresult) =>
            {
                VlcMediaPlayer.Stop();
                if (_context != null) _context.Dispose();
                _context = null;

                Dispatcher.BeginInvoke(new Action(() => VideoSource = null));
                action.EndInvoke(aresult);
            }, null);
        }

#if DotNet45

        public async System.Threading.Tasks.Task StopAsync()
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media == null) return;

            VlcMediaPlayer.SetVideoDecodeCallback(null, null, null, IntPtr.Zero);
            VlcMediaPlayer.SetVideoFormatCallback(null, null);

            if (VlcMediaPlayer.State == MediaState.Playing)
            {
                _stopWaitHandle.Reset();
                VlcMediaPlayer.Pause();
                await System.Threading.Tasks.Task.Run(() =>
                {
                    _stopWaitHandle.WaitOne();
                });
            }

            VlcMediaPlayer.Stop();

            if (_context != null) _context.Dispose();
            _context = null;

            VideoSource = null;
        }

#endif

        #endregion Stop

        public void AddOption(String option)
        {
            if ((VlcMediaPlayer != null) && VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.AddOption(option);
        }

        public void NextFrame()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.NextFrame();
        }

        public void Navigate(NavigateMode mode)
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.Navigate(mode);
        }

        public void ToggleMute()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.ToggleMute();
        }

        public void TakeSnapshot(String path, SnapshotFormat format, int quality)
        {
            if (VlcMediaPlayer != null)
                switch (VlcMediaPlayer.State)
                {
                    case MediaState.NothingSpecial:
                    case MediaState.Opening:
                    case MediaState.Buffering:
                    case MediaState.Stopped:
                    case MediaState.Ended:
                    case MediaState.Error:
                        break;

                    case MediaState.Playing:
                    case MediaState.Paused:
                        _snapshotContext = new SnapshotContext(path, format, quality);
                        break;
                }
        }

        #endregion --- Methods ---

        #region --- Events ---

        /// <summary>
        /// <see cref="VlcPlayer.LibVlcPath"/>
        /// </summary>
        public event EventHandler LibVlcPathChanged;

        /// <summary>
        /// <see cref="VlcPlayer.VlcOption"/>
        /// </summary>
        public event EventHandler VlcOptionChanged;

        /// <summary>
        /// <see cref="VlcPlayer.AspectRatio"/>
        /// </summary>
        public event EventHandler AspectRatioChanged;

        /// <summary>
        /// <see cref="VlcPlayer.VideoSource"/>
        /// </summary>
        public event EventHandler VideoSourceChanged;

        /// <summary>
        /// <see cref="VlcPlayer.Position"/>
        /// </summary>
        public event DependencyPropertyChangedEventHandler PositionChanged;

        /// <summary>
        /// <see cref="VlcPlayer.Time"/>
        /// </summary>
        public event DependencyPropertyChangedEventHandler TimeChanged;

        /// <summary>
        /// <see cref="VlcPlayer.FPS"/>
        /// </summary>
        public event EventHandler FPSChanged;

        /// <summary>
        /// <see cref="VlcPlayer.IsMute"/>
        /// </summary>
        public event EventHandler IsMuteChanged;

        /// <summary>
        /// <see cref="VlcPlayer.Volume"/>
        /// </summary>
        public event EventHandler VolumeChanged;

        /// <summary>
        /// <see cref="VlcPlayer.State"/>
        /// </summary>
        public event EventHandler<ObjectEventArgs<MediaState>> StateChanged;

        #region VlcMediaPlayer event handlers

        private void VlcMediaPlayerPositionChanged(object sender, EventArgs e)
        {
            SetPosition(VlcMediaPlayer.Position, false);
        }

        private void VlcMediaPlayerTimeChanged(object sender, EventArgs e)
        {
            SetTime(VlcMediaPlayer.Time, false);
        }

        private void VlcMediaPlayerSeekableChanged(object sender, EventArgs e)
        {
            IsSeekable = VlcMediaPlayer.IsSeekable;
        }

        private void VlcMediaPlayerStateChanged(object sender, EventArgs e)
        {
            var oldState = State;
            State = VlcMediaPlayer.State;
            if (State == MediaState.Paused || State == MediaState.Ended)
            {
                _stopWaitHandle.Set();
                return;
            }

            if (oldState != State)
            {
                Debug.WriteLine(String.Format("StateChanged : {0} to {1}", oldState, State));

                if (StateChanged != null)
                    StateChanged(this, new ObjectEventArgs<MediaState>(State));
            }
        }

        private void VlcMediaPlayerLengthChanged(object sender, EventArgs e)
        {
            Length = VlcMediaPlayer.Length;
        }

        #endregion VlcMediaPlayer event handlers

        #region Callbacks

        private IntPtr VideoLockCallback(IntPtr opaque, ref IntPtr planes)
        {
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
                            Debug.WriteLine(String.Format("Resize Image to {0}x{1}", _context.DisplayWidth, _context.DisplayHeight));
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
            if (_isRunFps)
            {
                _fpsCount++;
            }
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
                        using (Stream stream = File.Create(String.Format("{0}\\{1}.bmp", _snapshotContext.Path, _snapshotContext.Name)))
                        {
                            bmpE.Save(stream);
                        }
                        break;

                    case SnapshotFormat.JPG:
                        var jpgE = new JpegBitmapEncoder();
                        jpgE.Frames.Add(BitmapFrame.Create(VideoSource));
                        using (Stream stream = File.Create(String.Format("{0}\\{1}.jpg", _snapshotContext.Path, _snapshotContext.Name)))
                        {
                            jpgE.QualityLevel = _snapshotContext.Quality;
                            jpgE.Save(stream);
                        }
                        break;

                    case SnapshotFormat.PNG:
                        var pngE = new PngBitmapEncoder();
                        pngE.Frames.Add(BitmapFrame.Create(VideoSource));
                        using (Stream stream = File.Create(String.Format("{0}\\{1}.png", _snapshotContext.Path, _snapshotContext.Name)))
                        {
                            pngE.Save(stream);
                        }
                        break;
                }
                _snapshotContext = null;
            }));
        }

        private uint VideoFormatCallback(ref IntPtr opaque, ref uint chroma, ref uint width, ref uint height, ref uint pitches, ref uint lines)
        {
            Debug.WriteLine(String.Format("Initialize Video Content : {0}x{1}", width, height));
            _context = new VideoDisplayContext(width, height, PixelFormats.Bgr32);
            chroma = BitConverter.ToUInt32(new[] { (byte)'R', (byte)'V', (byte)'3', (byte)'2' }, 0);
            width = (uint)_context.Width;
            height = (uint)_context.Height;
            pitches = (uint)_context.Stride;
            lines = (uint)_context.Height;
            Dispatcher.Invoke(new Action(() =>
            {
                VideoSource = _context.Image;
            }));
            return (uint)_context.Size;
        }

        private void VideoCleanupCallback(IntPtr opaque)
        {
            _context.Dispose();
            _context = null;
        }

        #endregion Callbacks

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if ((VlcMediaPlayer != null) && (Vlc.LibDev == "xZune"))
                VlcMediaPlayer.SetMouseCursor(0, GetVideoPositionX(e.GetPosition(this).X), GetVideoPositionY(e.GetPosition(this).Y));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if ((VlcMediaPlayer != null) && (Vlc.LibDev == "xZune"))
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

            if ((VlcMediaPlayer != null) && (Vlc.LibDev == "xZune"))
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

        #endregion --- Events ---
    }
}