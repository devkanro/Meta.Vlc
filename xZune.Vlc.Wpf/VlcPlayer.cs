using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using xZune.Vlc.Interop.MediaPlayer;
using MediaState = xZune.Vlc.Interop.Media.MediaState;
using MouseButton = System.Windows.Input.MouseButton;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using xZune.Vlc.Interop.Media;

namespace xZune.Vlc.Wpf
{
    public class VlcPlayer : Control, IDisposable
    {
        static VlcPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlcPlayer), new FrameworkPropertyMetadata(typeof(VlcPlayer)));
        }

        VideoLockCallback _lockCallback;
        VideoUnlockCallback _unlockCallback;
        VideoDisplayCallback _displayCallback;
        VideoFormatCallback _formatCallback;
        VideoCleanupCallback _cleanupCallback;

        GCHandle _lockCallbackHandle;
        GCHandle _unlockCallbackHandle;
        GCHandle _displayCallbackHandle;
        GCHandle _formatCallbackHandle;
        GCHandle _cleanupCallbackHandle;

        static String CombinePath(String path1, String path2)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(path1, path2));
            return dir.FullName;
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
                        {
                            libVlcOption = vlcSettingsAttribute.VlcOption;
                        }
                    }

                    if (LibVlcPath != null)
                    {
                        libVlcPath = Path.IsPathRooted(LibVlcPath) ? LibVlcPath : CombinePath(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), LibVlcPath);
                    }
                    if (VlcOption != null)
                    {
                        libVlcOption = VlcOption;
                    }

                    if (libVlcPath != null)
                    {
                        Initialize(libVlcPath, libVlcOption);
                    }
                }
            }
            base.OnInitialized(e);
        }

        public void Initialize(String libVlcPath)
        {
#if DEBUG
            Initialize(libVlcPath, new[]
            {
                "-I", "dummy", "--ignore-config", "--no-video-title","--file-logging","--logfile=log.txt","--verbose=2","--no-sub-autodetect-file"
            });
#else
            Initialize(libVlcPath, new[]
            {
                "-I", "dummy", "--dummy-quiet", "--ignore-config", "--no-video-title", "--no-sub-autodetect-file"
            });
#endif
        }

        public void Initialize(String libVlcPath, String[] libVlcOption)
        {
            if (ApiManager.IsInitialized)
                return;

            ApiManager.Initialize(libVlcPath, libVlcOption);
            VlcMediaPlayer = ApiManager.Vlc.CreateMediaPlayer();
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

        #region 依赖属性 LibVlcPath

        public String LibVlcPath
        {
            get { return (String)GetValue(LibVlcPathProperty); }
            set { SetValue(LibVlcPathProperty, value); }
        }

        public static readonly DependencyProperty LibVlcPathProperty =
            DependencyProperty.Register("LibVlcPath", typeof(String), typeof(VlcPlayer), new PropertyMetadata(null, OnLibVlcPathChangedStatic));

        private static void OnLibVlcPathChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnLibVlcPathChanged(new EventArgs());
        }

        /// <summary>
        /// 引发 <see cref="VlcPlayer.LibVlcPathChanged"/> 事件
        /// </summary>
        protected void OnLibVlcPathChanged(EventArgs e)
        {
            if (LibVlcPathChanged != null)
            {
                LibVlcPathChanged(this, e);
            }
        }

        /// <summary>
        /// 在 VlcPlayer 的 <see cref="VlcPlayer.LibVlcPath"/> 属性改变时调用
        /// </summary>
        public event EventHandler LibVlcPathChanged;
        #endregion

        #region 依赖属性 VlcOption

        public String[] VlcOption
        {
            get { return (String[])GetValue(VlcOptionProperty); }
            set { SetValue(VlcOptionProperty, value); }
        }

        public static readonly DependencyProperty VlcOptionProperty =
            DependencyProperty.Register("VlcOption", typeof(String[]), typeof(VlcPlayer), new PropertyMetadata(null, OnVlcOptionChangedStatic));

        private static void OnVlcOptionChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnVlcOptionChanged(new EventArgs());
        }

        /// <summary>
        /// 引发 <see cref="VlcPlayer.VlcOptionChanged"/> 事件
        /// </summary>
        protected void OnVlcOptionChanged(EventArgs e)
        {
            if (VlcOptionChanged != null)
            {
                VlcOptionChanged(this, e);
            }
        }

        /// <summary>
        /// 在 VlcPlayer 的 <see cref="VlcPlayer.VlcOption"/> 属性改变时调用
        /// </summary>
        public event EventHandler VlcOptionChanged;
        #endregion

        #region 视频呈现
        VideoDisplayContext _context;
        int _checkCount = 0;
        Size _sar = new Size(1, 1);

        IntPtr VideoLockCallback(IntPtr opaque, ref IntPtr planes)
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

        void VideoUnlockCallback(IntPtr opaque, IntPtr picture, ref IntPtr planes)
        {
            return;
        }

        void VideoDisplayCallback(IntPtr opaque, IntPtr picture)
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

        uint VideoFormatCallback(ref IntPtr opaque, ref uint chroma, ref uint width, ref uint height, ref uint pitches, ref uint lines)
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

        void VideoCleanupCallback(IntPtr opaque)
        {
            _context.Dispose();
            _context = null;
        }
        #endregion
        
        internal static readonly DependencyProperty ScaleTransformProperty = DependencyProperty.Register(
            "ScaleTransform", typeof(ScaleTransform), typeof(VlcPlayer), new PropertyMetadata(default(ScaleTransform)));

        internal ScaleTransform ScaleTransform
        {
            get { return (ScaleTransform)GetValue(ScaleTransformProperty); }
            set { SetValue(ScaleTransformProperty, value); }
        }

        private Size GetScaleTransform()
        {
            if (_context == null) return new Size(1.0,1.0);

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

        public static readonly DependencyProperty PropertyTypeProperty = DependencyProperty.Register(
            "PropertyType", typeof (AspectRatio), typeof (VlcPlayer), new PropertyMetadata(AspectRatio.Default, OnAspectRatioChangedStatic));

        public AspectRatio AspectRatio
        {
            get { return (AspectRatio) GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        private static void OnAspectRatioChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;

            var scale = vlcPlayer.GetScaleTransform();
            vlcPlayer.ScaleTransform = new ScaleTransform(scale.Width, scale.Height);

            if (vlcPlayer != null) vlcPlayer.OnAspectRatioChanged(new EventArgs());
        }

        /// <summary>
        /// 引发 <see cref="VlcPlayer.AspectRatioChanged"/> 事件
        /// </summary>
        protected void OnAspectRatioChanged(EventArgs e)
        {
            if (AspectRatioChanged != null)
            {
                AspectRatioChanged(this, e);
            }
        }

        /// <summary>
        /// 在 VlcPlayer 的 <see cref="VlcPlayer.AspectRatio"/> 属性改变时调用
        /// </summary>
        public event EventHandler AspectRatioChanged;

        #region 依赖属性 VideoSource
        /// <summary>
        /// 获取 VlcPlayer 的视频源
        /// </summary>
        public InteropBitmap VideoSource
        {
            get { return (InteropBitmap)GetValue(VideoSourceProperty); }
            private set { SetValue(VideoSourceProperty, value); }
        }

        public static readonly DependencyProperty VideoSourceProperty =
            DependencyProperty.Register("VideoSource", typeof(InteropBitmap), typeof(VlcPlayer), new PropertyMetadata(null, OnVideoSourceChangedStatic));

        private static void OnVideoSourceChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnVideoSourceChanged(new EventArgs());
        }

        /// <summary>
        /// 引发 <see cref="VlcPlayer.VideoSourceChanged"/> 事件
        /// </summary>
        protected void OnVideoSourceChanged(EventArgs e)
        {
            if (VideoSourceChanged != null)
            {
                VideoSourceChanged(this, e);
            }
        }

        /// <summary>
        /// 在 VlcPlayer 的 <see cref="VlcPlayer.VideoSource"/> 属性改变时调用
        /// </summary>
        public event EventHandler VideoSourceChanged;
        #endregion

        #region 依赖属性 Position

        bool _setVlcPosition = true;

        private void VlcMediaPlayerPositionChanged(object sender, EventArgs e)
        {
            SetPosition(VlcMediaPlayer.Position, false);
        }

        private void SetPosition(float pos, bool setVlcPosition)
        {
            _setVlcPosition = setVlcPosition;
            Dispatcher.Invoke(new Action(() => SetValue(PositionProperty, pos)));
        }

        /// <summary>
        /// 获取或设置一个值,该值表示播放进度
        /// </summary>
        public float Position
        {
            get { return (float)GetValue(PositionProperty); }
            set { SetPosition(value, true); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(float), typeof(VlcPlayer), new PropertyMetadata(0.0f, OnPositionChangedStatic));

        private static void OnPositionChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnPositionChanged(e);
        }

        /// <summary>
        /// 引发 <see cref="VlcPlayer.PositionChanged"/> 事件
        /// </summary>
        protected void OnPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_setVlcPosition)
            {
                VlcMediaPlayer.Position = (float)e.NewValue;
            }
            _setVlcPosition = true;
            if (PositionChanged != null)
            {
                PositionChanged(this, e);
            }
        }

        /// <summary>
        /// 在 VlcPlayer 的 <see cref="VlcPlayer.Position"/> 属性改变时调用
        /// </summary>
        public event DependencyPropertyChangedEventHandler PositionChanged;
        #endregion

        #region 依赖属性 Time

        bool _setVlcTime = true;

        private void VlcMediaPlayerTimeChanged(object sender, EventArgs e)
        {
            SetTime(VlcMediaPlayer.Time, false);
        }

        private void SetTime(TimeSpan time, bool setVlcTime)
        {
            _setVlcTime = setVlcTime;
            Dispatcher.Invoke(new Action(() => SetValue(TimeProperty, time)));
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前的时间进度
        /// </summary>
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetTime(value, true); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register("Time", typeof(TimeSpan), typeof(VlcPlayer), new PropertyMetadata(TimeSpan.Zero, OnTimeChangedStatic));

        private static void OnTimeChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnTimeChanged(e);
        }

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
            DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(VlcPlayer), new PropertyMetadata(StretchDirection.Both));


        /// <summary>
        /// 引发 <see cref="VlcPlayer.TimeChanged"/> 事件
        /// </summary>
        protected void OnTimeChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_setVlcTime)
            {
                VlcMediaPlayer.Time = (TimeSpan)e.NewValue;
            }
            _setVlcTime = true;
            if (TimeChanged != null)
            {
                TimeChanged(this, e);
            }
        }

        /// <summary>
        /// 在 MediaPlayer 的 <see cref="MediaPlayer.Time"/> 属性改变时调用
        /// </summary>
        public event DependencyPropertyChangedEventHandler TimeChanged;
        #endregion

        #region 依赖属性 FPS
        private System.Windows.Threading.DispatcherTimer _timer;
        private int _fpsCount;
        private bool _isRunFps;

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

        /// <summary>
        /// 获取一个整数,该值表示一秒内呈现的图片数量,此属性反映了当前的视频刷新率,更新间隔为1秒,要想开始FPS计数,请使用 <see cref="VlcPlayer.StartFPS"/> 方法
        /// </summary>
        public int FPS
        {
            get { return (int)GetValue(FPSProperty); }
            private set { SetValue(FPSProperty, value); }
        }

        public static readonly DependencyProperty FPSProperty =
            DependencyProperty.Register("FPS", typeof(int), typeof(VlcPlayer), new PropertyMetadata(0, OnFPSChangedStatic));

        private static void OnFPSChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;
            if (vlcPlayer != null) vlcPlayer.OnFPSChanged(new EventArgs());
        }

        /// <summary>
        /// 引发 <see cref="VlcPlayer.FPSChanged"/> 事件
        /// </summary>
        protected void OnFPSChanged(EventArgs e)
        {
            if (FPSChanged != null)
            {
                FPSChanged(this, e);
            }
        }

        /// <summary>
        /// 在 VlcPlayer 的 <see cref="VlcPlayer.FPS"/> 属性改变时调用
        /// </summary>
        public event EventHandler FPSChanged;
        #endregion

        #region 属性 IsMute
        public bool IsMute
        {
            get
            {
                return VlcMediaPlayer.IsMute;
            }
            set
            {
                if (value == VlcMediaPlayer.IsMute) return;
                VlcMediaPlayer.IsMute = value;
                if (IsMuteChanged != null) IsMuteChanged.Invoke(this, new EventArgs());
            }
        }
        public event EventHandler IsMuteChanged;
        #endregion

        #region 属性 Volume
        public int Volume
        {
            get
            {
                return VlcMediaPlayer.Volume;
            }
            set
            {
                if (value == VlcMediaPlayer.Volume) return;
                VlcMediaPlayer.Volume = value; ;
                if (VolumeChanged != null) VolumeChanged.Invoke(this, new EventArgs());
            }
        }
        public event EventHandler VolumeChanged;
        #endregion

        #region 属性 AudioOutputChannel
        public AudioOutputChannel AudioOutputChannel
        {
            get
            {
                return VlcMediaPlayer.AudioOutputChannel;
            }

            set
            {
                VlcMediaPlayer.AudioOutputChannel = value;
            }
        }
        #endregion

        #region 属性 AudioTrackCount
        public int AudioTrackCount
        {
            get
            {
                return VlcMediaPlayer.AudioTrackCount;
            }
        }
        #endregion

        #region 属性 AudioTrack
        public int AudioTrack
        {
            get
            {
                return VlcMediaPlayer.AudioTrack;
            }
            set
            {
                VlcMediaPlayer.AudioTrack = value;
            }
        }
        #endregion

        #region 属性 AudioTrackDescription
        public TrackDescription AudioTrackDescription
        {
            get
            {
                return VlcMediaPlayer.AudioTrackDescription;
            }
        }
        #endregion

        #region 属性 Rate
        public float Rate
        {
            get
            {
                return VlcMediaPlayer.Rate;
            }
            set
            {
                VlcMediaPlayer.Rate = value;
            }
        }
        #endregion

        #region 属性 Title
        public int Title
        {
            get
            {
                return VlcMediaPlayer.Title;
            }
            set
            {
                VlcMediaPlayer.Title = value;
            }
        }
        #endregion

        #region 属性 TitleCount
        public int TitleCount
        {
            get
            {
                return VlcMediaPlayer.TitleCount;
            }
        }
        #endregion

        #region 属性 Chapter
        public int Chapter
        {
            get
            {
                return VlcMediaPlayer.Chapter;
            }
            set
            {
                VlcMediaPlayer.Chapter = value;
            }
        }
        #endregion

        #region 属性 ChapterCount
        public int ChapterCount
        {
            get
            {
                return VlcMediaPlayer.ChapterCount;
            }
        }
        #endregion

        #region 只读属性 IsSeekable
        private void VlcMediaPlayerSeekableChanged(object sender, EventArgs e)
        {
            IsSeekable = VlcMediaPlayer.IsSeekable;
        }

        /// <summary>
        /// 获取一个值,该值表示是否允许调整进度条
        /// </summary>
        public bool IsSeekable { get; private set; }

        #endregion

        #region 只读属性 State
        private void VlcMediaPlayerStateChanged(object sender, EventArgs e)
        {
            var oldState = State;
            State = VlcMediaPlayer.State;

            if (State == MediaState.Paused)
            {
                _stopWaitHandle.Set();
                return;
            }

            if (oldState != State)
            {
                Debug.WriteLine(String.Format("StateChanged : {0} to {1}", oldState, State));

                if (StateChanged != null)
                {
                    StateChanged(this, new ObjectEventArgs<MediaState>(State));
                }
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前的媒体状态
        /// </summary>
        public MediaState State { get; private set; }

        public event EventHandler<ObjectEventArgs<MediaState>> StateChanged;
        #endregion

        #region 只读属性 Length

        private void VlcMediaPlayerLengthChanged(object sender, EventArgs e)
        {
            Length = VlcMediaPlayer.Length;
        }

        /// <summary>
        /// 获取一个值,该值表示当前的媒体的长度
        /// </summary>
        public TimeSpan Length { get; private set; }
        #endregion

        #region 只读属性 VlcMediaPlayer
        public VlcMediaPlayer VlcMediaPlayer { get; private set; }
        #endregion

        #region 方法

        public void LoadMedia(String path)
        {
            if (!(File.Exists(path) || IsRootPath(Path.GetFullPath(path))))
            {
                throw new FileNotFoundException(String.Format("找不到媒体文件:{0}", path), path);
            }
            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormPath(path);
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void LoadMedia(Uri uri)
        {
            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormLocation(uri.ToString());
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void LoadMediaWithOptions(String path, String options)
        {
            if (!(File.Exists(path) || IsRootPath(Path.GetFullPath(path))))
            {
                throw new FileNotFoundException(String.Format("找不到媒体文件:{0}", path), path);
            }
            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormPath(path);
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void LoadMediaWithOptions(Uri uri, String options)
        {
            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormLocation(uri.ToString());
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
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

        public void Play()
        {
            VlcMediaPlayer.SetVideoDecodeCallback(_lockCallback, _unlockCallback, _displayCallback, IntPtr.Zero);
            VlcMediaPlayer.SetVideoFormatCallback(_formatCallback, _cleanupCallback);

            VlcMediaPlayer.Play();
        }

        public void PauseOrResume()
        {
            VlcMediaPlayer.PauseOrResume();
        }

        public void AddOption(String option)
        {
            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.AddOption(option);
        }

        public void NextFrame()
        {
            VlcMediaPlayer.NextFrame();
        }

        public void ToggleMute()
        {
            VlcMediaPlayer.ToggleMute();
        }

        public void Navigate(NavigateMode mode)
        {
            VlcMediaPlayer.Navigate(mode);
        }

        private EventWaitHandle _stopWaitHandle;

        public void BeginStop(AsyncCallback callback)
        {
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
            if (VlcMediaPlayer.Media == null) return null;

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

        SnapshotContext _snapshotContext;

        public void TakeSnapshot(String path, SnapshotFormat format, int quality)
        {
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

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Vlc.LibDev == "xZune")
            {
                VlcMediaPlayer.SetMouseCursor(0, GetVideoPositionX(e.GetPosition(this).X), GetVideoPositionY(e.GetPosition(this).Y));
            }
        }

        int GetVideoPositionX(double x)
        {
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

        int GetVideoPositionY(double y)
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

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (Vlc.LibDev == "xZune")
            {
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
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (Vlc.LibDev == "xZune")
            {
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

        bool _disposed = false;

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

        /// <summary>
        /// 释放 VlcMedia 资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    /// <summary>
    /// 用于呈现视频的上下文数据
    /// </summary>
    public class VideoDisplayContext : IDisposable
    {
        public int Size { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public double DisplayWidth { get; private set; }
        public double DisplayHeight { get; private set; }
        public int Stride { get; private set; }
        public PixelFormat PixelFormat { get; private set; }
        public IntPtr FileMapping { get; private set; }
        public IntPtr MapView { get; private set; }
        public InteropBitmap Image { get; private set; }
        public bool IsAspectRatioChecked { get; set; }

        public VideoDisplayContext(uint width, uint height, PixelFormat format)
            : this((int)width, (int)height, format)
        {
        }

        public VideoDisplayContext(double width, double height, PixelFormat format)
            : this((int)width, (int)height, format)
        {
        }

        public VideoDisplayContext(int width, int height, PixelFormat format)
        {
            IsAspectRatioChecked = false;
            Size = width * height * format.BitsPerPixel / 8;
            DisplayWidth = Width = width;
            DisplayHeight = Height = height;
            PixelFormat = format;
            Stride = width * format.BitsPerPixel / 8;
            FileMapping = Win32Api.CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PageAccess.ReadWrite, 0, Size, null);
            MapView = Win32Api.MapViewOfFile(FileMapping, FileMapAccess.AllAccess, 0, 0, (uint)Size);
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Image = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(FileMapping, Width, Height, PixelFormat, Stride, 0);
            }));
        }

        public void Display()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Image.Invalidate();
                }));
            }
        }

        public void CheckDisplaySize(VideoTrack track)
        {
            if (!IsAspectRatioChecked)
            {
                var sar = 1.0 * track.SarNum / track.SarDen;

                if (track.SarNum == 0 || track.SarDen == 0)
                {
                    return;
                }

                Debug.WriteLine(String.Format("Video Size:{0}x{1}\r\nSAR:{2}/{3}", track.Width, track.Height, track.SarNum, track.SarDen));

                if (sar > 1)
                {
                    DisplayWidth = sar * track.Width;
                    DisplayHeight = track.Height;
                }
                else
                {
                    DisplayWidth = track.Width;
                    DisplayHeight = track.Height / sar;
                }
            }
        }

        bool _disposed = false;

        public void Dispose(bool disposing)
        {
            if (_disposed) return;
            Size = 0;
            PixelFormat = PixelFormats.Default;
            Stride = 0;
            Image = null;
            Win32Api.UnmapViewOfFile(MapView);
            Win32Api.CloseHandle(FileMapping);
            FileMapping = MapView = IntPtr.Zero;
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    public enum SnapshotFormat
    {
        BMP,
        JPG,
        PNG
    }

    public class SnapshotContext
    {
        private static int _count;
        public SnapshotContext(String path, SnapshotFormat format, int quality)
        {
            Path = path.Replace('/', '\\');
            if (Path[Path.Length - 1] == '\\')
            {
                Path = Path.Substring(0, Path.Length - 1);
            }
            Format = format;
            Quality = quality;
        }

        public String Path { get; private set; }
        public String Name { get; private set; }
        public SnapshotFormat Format { get; private set; }
        public int Quality { get; private set; }

        public String GetName(VlcPlayer player)
        {
            player.Dispatcher.Invoke(new Action(() =>
            {
                Name = String.Format("{0}-{1}-{2}",
                    GetMediaName(player.VlcMediaPlayer.Media.Mrl.Replace("file:///", "")),
                    (int)(player.Time.TotalMilliseconds), _count++);
            }));
            return Name;
        }

        internal static String GetMediaName(String path)
        {
            if (VlcPlayer.IsRootPath(path))
            {
                path = path.Replace('/', '\\').ToUpper();
                foreach (var item in DriveInfo.GetDrives())
                {
                    if (item.Name.ToUpper() == path)
                    {
                        return item.VolumeLabel;
                    }
                }
            }
            else
            {
                return System.IO.Path.GetFileNameWithoutExtension(path);
            }
            return "Unkown";
        }
    }
}
