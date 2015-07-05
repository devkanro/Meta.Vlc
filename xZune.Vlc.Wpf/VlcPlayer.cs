using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using xZune.Vlc;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace xZune.Vlc.Wpf
{
    public class VlcPlayer : Control, INotifyPropertyChanged, IDisposable
    {
        static VlcPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlcPlayer), new FrameworkPropertyMetadata(typeof(VlcPlayer)));
        }

        xZune.Vlc.Interop.MediaPlayer.VideoLockCallback lockCallback;
        xZune.Vlc.Interop.MediaPlayer.VideoUnlockCallback unlockCallback;
        xZune.Vlc.Interop.MediaPlayer.VideoDisplayCallback displayCallback;
        xZune.Vlc.Interop.MediaPlayer.VideoFormatCallback formatCallback;
        xZune.Vlc.Interop.MediaPlayer.VideoCleanupCallback cleanupCallback;
        GCHandle lockCallbackHandle;
        GCHandle unlockCallbackHandle;
        GCHandle displayCallbackHandle;
        GCHandle formatCallbackHandle;
        GCHandle cleanupCallbackHandle;

        public VlcPlayer()
        {

        }

        static String CombinePath(String path1, String path2)
        {
            string result = string.Empty;

            if (!Path.IsPathRooted(path2))
            {
                Regex regex = new Regex(@"^\\|([..]+)");
                int backUp = regex.Matches(path2).Count;
                List<string> pathes = path1.Split('\\').ToList();
                pathes.RemoveRange(pathes.Count - backUp, backUp);
                regex = new Regex(@"^\\|([a-zA-Z0-9]+)");
                MatchCollection matches = regex.Matches(path2);
                foreach (Match match in matches)
                {
                    pathes.Add(match.Value);
                }
                pathes[0] = Path.GetPathRoot(path1);
                foreach (string p in pathes)
                {
                    result = Path.Combine(result, p);
                }
            }
            return result;
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (!ApiManager.IsInited)
                {
                    if (LibVlcPath != null)
                    {
                        if (Path.IsPathRooted(LibVlcPath))
                        {
                            ApiManager.LibVlcPath = LibVlcPath;
                        }
                        else
                        {
                            ApiManager.LibVlcPath = CombinePath(ApiManager.LibVlcPath, LibVlcPath);
                        }
                        if (ApiManager.LibVlcPath[ApiManager.LibVlcPath.Length - 1] != '\\' && ApiManager.LibVlcPath[ApiManager.LibVlcPath.Length - 1] != '/')
                        {
                            ApiManager.LibVlcPath += "\\";
                        }
                    }
                    ApiManager.Init();
                }
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

                lockCallback = VideoLockCallback;
                unlockCallback = VideoUnlockCallback;
                displayCallback = VideoDisplayCallback;
                formatCallback = VideoFormatCallback;
                cleanupCallback = VideoCleanupCallback;

                lockCallbackHandle = GCHandle.Alloc(lockCallback);
                unlockCallbackHandle = GCHandle.Alloc(unlockCallback);
                displayCallbackHandle = GCHandle.Alloc(displayCallback);
                formatCallbackHandle = GCHandle.Alloc(formatCallback);
                cleanupCallbackHandle = GCHandle.Alloc(cleanupCallback);
            }
            base.OnInitialized(e);
        }


        #region 依赖属性 LibVlcPath

        public String LibVlcPath
        {
            get { return (String)GetValue(LibVlcPathProperty); }
            set { SetValue(LibVlcPathProperty, value); }
        }

        public static readonly DependencyProperty LibVlcPathProperty =
            DependencyProperty.Register(nameof(LibVlcPath), typeof(String), typeof(VlcPlayer), new PropertyMetadata(null, OnLibVlcPathChangedStatic));

        private static void OnLibVlcPathChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as VlcPlayer).OnLibVlcPathChanged(new EventArgs());
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


        #region 视频呈现
        VideoDisplayContext context;
        
        IntPtr VideoLockCallback(IntPtr opaque, ref IntPtr planes)
        {
            return planes = context.MapView;
        }

        void VideoUnlockCallback(IntPtr opaque, IntPtr picture, ref IntPtr planes)
        {
            return;
        }

        async void VideoDisplayCallback(IntPtr opaque, IntPtr picture)
        {
            if (isRunFPS)
            {
                fpsCount++;
            }
            context.Display();
            if(snapshotContext != null)
            {
                snapshotContext.GetName(this);
                await this.Dispatcher.InvokeAsync(() =>
                {
                    switch (snapshotContext.Format)
                    {
                        case SnapshotFormat.BMP:
                            BmpBitmapEncoder bmpE = new BmpBitmapEncoder();
                            bmpE.Frames.Add(BitmapFrame.Create(VideoSource));
                            using (Stream stream = File.Create($"{snapshotContext.Path}\\{snapshotContext.Name}.bmp"))
                            {
                                bmpE.Save(stream);
                            }
                            break;
                        case SnapshotFormat.JPG:
                            JpegBitmapEncoder jpgE = new JpegBitmapEncoder();
                            jpgE.Frames.Add(BitmapFrame.Create(VideoSource));
                            using (Stream stream = File.Create($"{snapshotContext.Path}\\{snapshotContext.Name}.jpg"))
                            {
                                jpgE.QualityLevel = snapshotContext.Quality;
                                jpgE.Save(stream);
                            }
                            break;
                        case SnapshotFormat.PNG:
                            PngBitmapEncoder pngE = new PngBitmapEncoder();
                            pngE.Frames.Add(BitmapFrame.Create(VideoSource));
                            using (Stream stream = File.Create($"{snapshotContext.Path}\\{snapshotContext.Name}.png"))
                            {
                                pngE.Save(stream);
                            }
                            break;
                        default:
                            break;
                    }
                });
                //System.Windows.Media.Imaging.WriteableBitmap bm = new System.Windows.Media.Imaging.WriteableBitmap(VideoSource);
                snapshotContext = null;
            }
        }

        uint VideoFormatCallback(ref IntPtr opaque, ref uint chroma, ref uint width, ref uint height, ref uint pitches, ref uint lines)
        {
            context = new VideoDisplayContext(width, height, PixelFormats.Bgr32);

            chroma = BitConverter.ToUInt32(new[] { (byte)'R', (byte)'V', (byte)'3', (byte)'2' }, 0);
            width = (uint)context.Width;
            height = (uint)context.Height;
            pitches = (uint)context.Stride;
            lines = (uint)context.Height;

            Dispatcher.Invoke(() =>
            {
                VideoSource = context.Image;
            });

            return (uint)context.Size;
        }

        void VideoCleanupCallback(IntPtr opaque)
        {
            context.Dispose();
        }
        #endregion

        #region 属性变更通知
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChange([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void SetProperty(ref object field, object newValue, [CallerMemberName]string propertyName = "")
        {
            if (field != newValue)
            {
                field = newValue;
                NotifyPropertyChange(propertyName);
            }
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (!object.Equals(field, newValue))
            {
                field = newValue;
                NotifyPropertyChange(propertyName);
            }
        }
        #endregion

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
            DependencyProperty.Register(nameof(VideoSource), typeof(InteropBitmap), typeof(VlcPlayer), new PropertyMetadata(null, OnVideoSourceChangedStatic));

        private static void OnVideoSourceChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as VlcPlayer).OnVideoSourceChanged(new EventArgs());
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

        bool setVlcPosition = true;

        private void VlcMediaPlayerPositionChanged(object sender, EventArgs e)
        {
            SetPosition(VlcMediaPlayer.Position, false);
        }

        private void SetPosition(float pos, bool setVlcPosition)
        {
            this.setVlcPosition = setVlcPosition;
            Dispatcher.Invoke(() => SetValue(PositionProperty, pos));
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
            DependencyProperty.Register(nameof(Position), typeof(float), typeof(VlcPlayer), new PropertyMetadata(0.0f, OnPositionChangedStatic));

        private static void OnPositionChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as VlcPlayer).OnPositionChanged(e);
        }

        /// <summary>
        /// 引发 <see cref="VlcPlayer.PositionChanged"/> 事件
        /// </summary>
        protected void OnPositionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (setVlcPosition)
            {
                VlcMediaPlayer.Position = (float)e.NewValue;
            }
            setVlcPosition = true;
            if (PositionChanged != null)
            {
                PositionChanged(this, e);
            }
        }

        /// <summary>
        /// 在 VlcPlayer 的 <see cref="VlcPlayer.Position"/> 属性改变时调用
        /// </summary>
        public event EventHandler<DependencyPropertyChangedEventArgs> PositionChanged;
        #endregion

        #region 依赖属性 Time

        bool setVlcTime = true;

        private void VlcMediaPlayerTimeChanged(object sender, EventArgs e)
        {
            SetTime(VlcMediaPlayer.Time, false);
        }

        private void SetTime(TimeSpan time, bool setVlcTime)
        {
            this.setVlcTime = setVlcTime;
            Dispatcher.Invoke(() => SetValue(TimeProperty, time));
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
            DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(VlcPlayer), new PropertyMetadata(TimeSpan.Zero, OnTimeChangedStatic));

        private static void OnTimeChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as VlcPlayer).OnTimeChanged(e);
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
            if (setVlcTime)
            {
                VlcMediaPlayer.Time = (TimeSpan)e.NewValue;
            }
            setVlcTime = true;
            if (TimeChanged != null)
            {
                TimeChanged(this, e);
            }
        }

        /// <summary>
        /// 在 MediaPlayer 的 <see cref="MediaPlayer.Time"/> 属性改变时调用
        /// </summary>
        public event EventHandler<DependencyPropertyChangedEventArgs> TimeChanged;
        #endregion

        #region 依赖属性 FPS
        private System.Windows.Threading.DispatcherTimer timer;
        private int fpsCount = 0;
        private bool isRunFPS = false;

        /// <summary>
        /// 开始FPS计数
        /// </summary>
        public void StartFPS()
        {
            if (!isRunFPS)
            {
                timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Tick += FPSTick;
                isRunFPS = true;
                fpsCount = 0;
                timer.Start();
            }
        }

        /// <summary>
        /// 停止FPS计数
        /// </summary>
        public void StopFPS()
        {
            timer?.Stop();
            timer = null;
            isRunFPS = false;
        }

        private void FPSTick(object sender, EventArgs e)
        {
            FPS = fpsCount;
            fpsCount = 0;
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
            DependencyProperty.Register(nameof(FPS), typeof(int), typeof(VlcPlayer), new PropertyMetadata(0, OnFPSChangedStatic));

        private static void OnFPSChangedStatic(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as VlcPlayer).OnFPSChanged(new EventArgs());
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
                if (value != VlcMediaPlayer.IsMute)
                {
                    VlcMediaPlayer.IsMute = value;
                    NotifyPropertyChange();
                    IsMuteChanged?.Invoke(this, new EventArgs());
                }
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
                if (value != VlcMediaPlayer.Volume)
                {
                    VlcMediaPlayer.Volume = value;
                    NotifyPropertyChange();
                    VolumeChanged?.Invoke(this, new EventArgs());
                }
            }
        }
        public event EventHandler VolumeChanged;
        #endregion

        #region 只读属性 IsSeekable
        private void VlcMediaPlayerSeekableChanged(object sender, EventArgs e)
        {
            IsSeekable = VlcMediaPlayer.IsSeekable;
        }

        private bool isSeekable;

        /// <summary>
        /// 获取一个值,该值表示是否允许调整进度条
        /// </summary>
        public bool IsSeekable
        {
            get { return isSeekable; }
            private set { SetProperty<bool>(ref isSeekable, value); }
        }
        #endregion

        #region 只读属性 State
        private void VlcMediaPlayerStateChanged(object sender, EventArgs e)
        {
            State = VlcMediaPlayer.State;
        }

        private xZune.Vlc.Interop.Media.MediaState state;

        /// <summary>
        /// 获取一个值,该值表示当前的媒体状态
        /// </summary>
        public xZune.Vlc.Interop.Media.MediaState State
        {
            get { return state; }
            private set { SetProperty<xZune.Vlc.Interop.Media.MediaState>(ref state, value); }
        }
        #endregion

        #region 只读属性 Length

        private void VlcMediaPlayerLengthChanged(object sender, EventArgs e)
        {
            Length = VlcMediaPlayer.Length;
        }

        private TimeSpan length;

        /// <summary>
        /// 获取一个值,该值表示当前的媒体的长度
        /// </summary>
        public TimeSpan Length
        {
            get { return length; }
            set { SetProperty<TimeSpan>(ref length, value); }
        }
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
            VlcMediaPlayer.Media?.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormPath(path);
            VlcMediaPlayer.Media.ParseAsync();
        }

        internal static bool IsRootPath(string path)
        {
            path = path.Replace('\\', '/');
            if(path[path.Length - 1] != '/')
            {
                path += '/';
            }
            int index;
            if((index = path.IndexOf(":/")) != -1 && index + 2 == path.Length)
            {
                return true;
            }
            return false;

        }

        public void LoadMedia(Uri uri)
        {
            VlcMediaPlayer.Media?.Dispose();
            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFormLocation(uri.ToString());
            VlcMediaPlayer.Media.ParseAsync();
        }

        public void Play()
        {
            VlcMediaPlayer.SetVideoDecodeCallback(lockCallback, unlockCallback, displayCallback, IntPtr.Zero);
            VlcMediaPlayer.SetVideoFormatCallback(formatCallback, cleanupCallback);

            VlcMediaPlayer.Play();
        }

        public void PauseOrResume()
        {
            VlcMediaPlayer.PauseOrResume();
        }

        public void AddOption(String option)
        {
            VlcMediaPlayer.Media?.AddOption(option);
        }

        public void NextFrame()
        {
            VlcMediaPlayer.NextFrame();
        }

        public void ToggleMute()
        {
            VlcMediaPlayer.ToggleMute();
        }

        public async Task Stop()
        {

            if (VlcMediaPlayer.Media != null )
            {
                VlcMediaPlayer.SetVideoDecodeCallback(null, null, null, IntPtr.Zero);
                VlcMediaPlayer.SetVideoFormatCallback(null, null);

                VlcMediaPlayer.Pause();

                await Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(50);
                    VlcMediaPlayer.Stop();

                    context?.Dispose();
                    context = null;
                });

                VideoSource = null;
            }
        }

        SnapshotContext snapshotContext;

        public void TakeSnapshot(String path, SnapshotFormat format, int quality)
        {
            switch (VlcMediaPlayer.State)
            {
                case Interop.Media.MediaState.NothingSpecial:
                case Interop.Media.MediaState.Opening:
                case Interop.Media.MediaState.Buffering:
                case Interop.Media.MediaState.Stopped:
                case Interop.Media.MediaState.Ended:
                case Interop.Media.MediaState.Error:
                    break;
                case Interop.Media.MediaState.Playing:
                case Interop.Media.MediaState.Paused:
                    snapshotContext = new SnapshotContext(path, format, quality);
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            bool test = VlcMediaPlayer.SetMouseCursor(0, GetVideoPositionX(e.GetPosition(this).X), GetVideoPositionY(e.GetPosition(this).Y));
        }

        int GetVideoPositionX(double x)
        {
            int px = 0;
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
                            px = (int)(x - ((this.ActualWidth - context.Width) / 2));
                            break;
                        case HorizontalAlignment.Right:
                            px = (int)(x - (this.ActualWidth - context.Width));
                            break;
                        case HorizontalAlignment.Stretch:
                            if(this.ActualWidth > context.Width)
                            {
                                px = (int)(x - ((this.ActualWidth - context.Width) / 2));
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
                            if (this.ActualWidth > context.Width)
                            {
                                px = (int)(x / this.ActualWidth * context.Width);
                            }
                            break;
                        case StretchDirection.DownOnly:
                            if(this.ActualWidth < context.Width)
                            {
                                px = (int)(x / this.ActualWidth * context.Width);
                            }
                            break;
                        case StretchDirection.Both:
                            px = (int)(x / this.ActualWidth * context.Width);
                            break;
                    }
                    break;
                case Stretch.Uniform:
                    scaleX = this.ActualWidth / context.Width;
                    scaleY = this.ActualHeight / context.Height;
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
                                            px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
                                            break;
                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - context.Width * scale)) / scale);
                                            break;
                                        case HorizontalAlignment.Stretch:
                                            px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
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
                                        px = (int)(x - ((this.ActualWidth - context.Width) / 2));
                                        break;
                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - context.Width));
                                        break;
                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > context.Width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - context.Width) / 2));
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
                            if(scale < 1)
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
                                            px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
                                            break;
                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - context.Width * scale)) / scale);
                                            break;
                                        case HorizontalAlignment.Stretch:
                                            px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
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
                                        px = (int)(x - ((this.ActualWidth - context.Width) / 2));
                                        break;
                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - context.Width));
                                        break;
                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > context.Width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - context.Width) / 2));
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
                                        px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
                                        break;
                                    case HorizontalAlignment.Right:
                                        px = (int)((x - (this.ActualWidth - context.Width * scale)) / scale);
                                        break;
                                    case HorizontalAlignment.Stretch:
                                        px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
                case Stretch.UniformToFill:
                    scaleX = this.ActualWidth / context.Width;
                    scaleY = this.ActualHeight / context.Height;
                    scale = Math.Max(scaleX, scaleY);

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
                                            px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
                                            break;
                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - context.Width * scale)) / scale);
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
                                        px = (int)(x - ((this.ActualWidth - context.Width) / 2));
                                        break;
                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - context.Width));
                                        break;
                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > context.Width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - context.Width) / 2));
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
                            if(scale < 1)
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
                                            px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
                                            break;
                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - context.Width * scale)) / scale);
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
                                        px = (int)(x - ((this.ActualWidth - context.Width) / 2));
                                        break;
                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - context.Width));
                                        break;
                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > context.Width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - context.Width) / 2));
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
                                        px = (int)((x - ((this.ActualWidth - context.Width * scale) / 2)) / scale);
                                        break;
                                    case HorizontalAlignment.Right:
                                        px = (int)((x - (this.ActualWidth - context.Width * scale)) / scale);
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
                            py = (int)(y - ((this.ActualHeight - context.Height) / 2));
                            break;
                        case VerticalAlignment.Bottom:
                            py = (int)(y - (this.ActualHeight - context.Height));
                            break;
                        case VerticalAlignment.Stretch:
                            if (this.ActualHeight > context.Height)
                            {
                                py = (int)(y - ((this.ActualHeight - context.Height) / 2));
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
                            if (this.ActualHeight > context.Height)
                            {
                                py = (int)(y / this.ActualHeight * context.Height);
                            }
                            break;
                        case StretchDirection.DownOnly:
                            if (this.ActualHeight < context.Height)
                            {
                                py = (int)(y / this.ActualHeight * context.Height);
                            }
                            break;
                        case StretchDirection.Both:
                            py = (int)(y / this.ActualHeight * context.Height);
                            break;
                    }
                    break;
                case Stretch.Uniform:
                    scaleX = this.ActualWidth / context.Width;
                    scaleY = this.ActualHeight / context.Height;
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
                                            py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
                                            break;
                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - context.Height * scale)) / scale);
                                            break;
                                        case VerticalAlignment.Stretch:
                                            py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
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
                                        py = (int)(y - ((this.ActualHeight - context.Height) / 2));
                                        break;
                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - context.Height));
                                        break;
                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > context.Height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - context.Height) / 2));
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
                                            py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
                                            break;
                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - context.Height * scale)) / scale);
                                            break;
                                        case VerticalAlignment.Stretch:
                                            py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
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
                                        py = (int)(y - ((this.ActualHeight - context.Height) / 2));
                                        break;
                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - context.Height));
                                        break;
                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > context.Height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - context.Height) / 2));
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
                                        py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
                                        break;
                                    case VerticalAlignment.Bottom:
                                        py = (int)((y - (this.ActualHeight - context.Height * scale)) / scale);
                                        break;
                                    case VerticalAlignment.Stretch:
                                        py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
                case Stretch.UniformToFill:
                    scaleX = this.ActualWidth / context.Width;
                    scaleY = this.ActualHeight / context.Height;
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
                                            py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
                                            break;
                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - context.Height * scale)) / scale);
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
                                        py = (int)(y - ((this.ActualHeight - context.Height) / 2));
                                        break;
                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - context.Height));
                                        break;
                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > context.Height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - context.Height) / 2));
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
                                            py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
                                            break;
                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - context.Height * scale)) / scale);
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
                                        py = (int)(y - ((this.ActualHeight - context.Height) / 2));
                                        break;
                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - context.Height));
                                        break;
                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > context.Height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - context.Height) / 2));
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
                                        py = (int)((y - ((this.ActualHeight - context.Height * scale) / 2)) / scale);
                                        break;
                                    case VerticalAlignment.Bottom:
                                        py = (int)((y - (this.ActualHeight - context.Height * scale)) / scale);
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

        bool disposed = false;

        protected async void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            await Stop();
            VlcMediaPlayer?.Media?.Dispose();
            VlcMediaPlayer?.Dispose();

            disposed = true;
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
        public int Stride { get; private set; }
        public PixelFormat PixelFormat { get; private set; }
        public IntPtr FileMapping { get; private set; }
        public IntPtr MapView { get; private set; }
        public InteropBitmap Image { get; private set; }

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
            Size = width * height * format.BitsPerPixel / 8;
            Width = width;
            Height = height;
            PixelFormat = format;
            Stride = width * format.BitsPerPixel / 8;
            FileMapping = Win32API.CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PageAccess.ReadWrite, 0, Size, null);
            MapView = Win32API.MapViewOfFile(FileMapping, FileMapAccess.AllAccess, 0, 0, (uint)Size);
            Application.Current.Dispatcher.Invoke(() =>
            {
                Image = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(FileMapping, Width, Height, PixelFormat, Stride, 0);
            });
        }

        public void Display()
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                Image?.Invalidate();
            });
        }

        bool disposed = false;

        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                Size = 0;
                Width = 0;
                Height = 0;
                PixelFormat = PixelFormats.Default;
                Stride = 0;
                Image = null;
                Win32API.UnmapViewOfFile(MapView);
                Win32API.CloseHandle(FileMapping);
                FileMapping = MapView = IntPtr.Zero;
            }
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
        public SnapshotContext(String path, SnapshotFormat format,int quality)
        {
            Path = path.Replace('/', '\\');
            if(Path[Path.Length - 1] == '\\')
            {
                Path = Path.Substring(0, Path.Length - 1);
            }
            Format = format;
            Quality = quality;
        }

        static int count = 0;
        public String Path { get; private set; }
        public String Name { get; private set; }
        public SnapshotFormat Format { get; private set; }
        public int Quality { get; private set; }

        public String GetName (VlcPlayer player)
        {
            player.Dispatcher.Invoke(() =>
            {
                Name = $"{GetMediaName(player.VlcMediaPlayer.Media.Mrl.Replace("file:///", ""))}-{(int)(player.Time.TotalMilliseconds)}-{count++}";
            });
            return Name;
        }

        internal static String GetMediaName(String path)
        {
            if(VlcPlayer.IsRootPath(path))
            {
                path = path.Replace('/', '\\').ToUpper();
                foreach (var item in DriveInfo.GetDrives())
                {
                    if(item.Name.ToUpper() == path)
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
