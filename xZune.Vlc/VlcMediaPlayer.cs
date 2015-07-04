using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;

using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core.Events;
using xZune.Vlc.Interop.MediaPlayer;

namespace xZune.Vlc
{
    public class VlcMediaPlayer : IVlcObject
    {

        public IntPtr InstancePointer { get; private set; }

        public VlcEventManager EventManager { get; private set; }

        public static VlcMediaPlayer Create(Vlc vlc)
        {
            return new VlcMediaPlayer(CreateMediaPlayerFunction.Delegate(vlc.InstancePointer));
        }

        public static VlcMediaPlayer CreatFormMedia(VlcMedia media)
        {
            return new VlcMediaPlayer(CreateMediaPlayerFromMediaFunction.Delegate(media.InstancePointer));
        }

        /// <summary>
        /// 载入 LibVlc 的 MediaPlayer 模块,该方法会在 <see cref="Vlc.LoadLibVlc"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        public static void LoadLibVlc(IntPtr libHandle, Version libVersion, String devString)
        {
            if (!IsLibLoaded)
            {
                CreateMediaPlayerFunction = new LibVlcFunction<CreateMediaPlayer>(libHandle, libVersion, devString);
                CreateMediaPlayerFromMediaFunction = new LibVlcFunction<CreateMediaPlayerFromMedia>(libHandle, libVersion, devString);
                ReleaseMediaPlayerFunction = new LibVlcFunction<ReleaseMediaPlayer>(libHandle, libVersion, devString);
                RetainMediaPlayerFunction = new LibVlcFunction<RetainMediaPlayer>(libHandle, libVersion, devString);
                SetMediaFunction = new LibVlcFunction<SetMedia>(libHandle, libVersion, devString);
                GetMediaFunction = new LibVlcFunction<GetMedia>(libHandle, libVersion, devString);
                GetEventManagerFunction = new LibVlcFunction<GetEventManager>(libHandle, libVersion, devString);
                IsPlayingFunction = new LibVlcFunction<IsPlaying>(libHandle, libVersion, devString);
                PlayFunction = new LibVlcFunction<Play>(libHandle, libVersion, devString);
                SetPauseFunction = new LibVlcFunction<SetPause>(libHandle, libVersion, devString);
                SetPositionFunction = new LibVlcFunction<SetPosition>(libHandle, libVersion, devString);
                StopFunction = new LibVlcFunction<Stop>(libHandle, libVersion, devString);
                SetVideoCallbackFunction = new LibVlcFunction<SetVideoCallback>(libHandle, libVersion, devString);
                SetVideoFormatFunction = new LibVlcFunction<SetVideoFormat>(libHandle, libVersion, devString);
                SetVideoFormatCallbackFunction = new LibVlcFunction<SetVideoFormatCallback>(libHandle, libVersion, devString);
                SetHwndFunction = new LibVlcFunction<SetHwnd>(libHandle, libVersion, devString);
                GetHwndFunction = new LibVlcFunction<GetHwnd>(libHandle, libVersion, devString);
                SetAudioCallbackFunction = new LibVlcFunction<SetAudioCallback>(libHandle, libVersion, devString);
                SetAudioFormatFunction = new LibVlcFunction<SetAudioFormat>(libHandle, libVersion, devString);
                SetAudioFormatCallbackFunction = new LibVlcFunction<SetAudioFormatCallback>(libHandle, libVersion, devString);
                SetAudioVolumeCallbackFunction = new LibVlcFunction<SetAudioVolumeCallback>(libHandle, libVersion, devString);
                GetLengthFunction = new LibVlcFunction<GetLength>(libHandle, libVersion, devString);
                GetTimeFunction = new LibVlcFunction<GetTime>(libHandle, libVersion, devString);
                SetTimeFunction = new LibVlcFunction<SetTime>(libHandle, libVersion, devString);
                GetPositionFunction = new LibVlcFunction<GetPosition>(libHandle, libVersion, devString);
                SetChapterFunction = new LibVlcFunction<SetChapter>(libHandle, libVersion, devString);
                GetChapterFunction = new LibVlcFunction<GetChapter>(libHandle, libVersion, devString);
                GetChapterCountFunction = new LibVlcFunction<GetChapterCount>(libHandle, libVersion, devString);
                CanPlayFunction = new LibVlcFunction<CanPlay>(libHandle, libVersion, devString);
                GetTitleChapterCountFunction = new LibVlcFunction<GetTitleChapterCount>(libHandle, libVersion, devString);
                SetTitleFunction = new LibVlcFunction<SetTitle>(libHandle, libVersion, devString);
                GetTitleFunction = new LibVlcFunction<GetTitle>(libHandle, libVersion, devString);
                GetTitleCountFunction = new LibVlcFunction<GetTitleCount>(libHandle, libVersion, devString);
                PreviousChapterFunction = new LibVlcFunction<PreviousChapter>(libHandle, libVersion, devString);
                NextChapterFunction = new LibVlcFunction<NextChapter>(libHandle, libVersion, devString);
                GetRateFunction = new LibVlcFunction<GetRate>(libHandle, libVersion, devString);
                SetRateFunction = new LibVlcFunction<SetRate>(libHandle, libVersion, devString);
                GetStateFunction = new LibVlcFunction<GetState>(libHandle, libVersion, devString);
                GetFPSFunction = new LibVlcFunction<GetFPS>(libHandle, libVersion, devString);
                HasVoutFunction = new LibVlcFunction<HasVout>(libHandle, libVersion, devString);
                IsSeekableFunction = new LibVlcFunction<IsSeekable>(libHandle, libVersion, devString);
                CanPauseFunction = new LibVlcFunction<CanPause>(libHandle, libVersion, devString);
                NextFrameFunction = new LibVlcFunction<NextFrame>(libHandle, libVersion, devString);
                NavigateFunction = new LibVlcFunction<Navigate>(libHandle, libVersion, devString);
                SetVideoTitleDisplayFunction = new LibVlcFunction<SetVideoTitleDisplay>(libHandle, libVersion, devString);
                ReleaseTrackDescriptionFunction = new LibVlcFunction<ReleaseTrackDescription>(libHandle, libVersion, devString);
                ToggleMuteFunction = new LibVlcFunction<ToggleMute>(libHandle, libVersion, devString);
                GetMuteFunction = new LibVlcFunction<GetMute>(libHandle, libVersion, devString);
                SetMuteFunction = new LibVlcFunction<SetMute>(libHandle, libVersion, devString);
                GetVolumeFunction = new LibVlcFunction<GetVolume>(libHandle, libVersion, devString);
                SetVolumeFunction = new LibVlcFunction<SetVolume>(libHandle, libVersion, devString);
                GetCursorFunction = new LibVlcFunction<GetCursor>(libHandle, libVersion, devString);
                SetCursorFunction = new LibVlcFunction<SetCursor>(libHandle, libVersion, devString);
                SetMouseDownFunction = new LibVlcFunction<SetMouseDown>(libHandle, libVersion, devString);
                SetMouseUpFunction = new LibVlcFunction<SetMouseUp>(libHandle, libVersion, devString);
                IsLibLoaded = true;
            }
        }

        /// <summary>
        /// 获取一个值,该值指示当前模块是否被载入
        /// </summary>
        public static bool IsLibLoaded
        {
            get;
            private set;
        }

        static LibVlcFunction<CreateMediaPlayer> CreateMediaPlayerFunction;
        static LibVlcFunction<CreateMediaPlayerFromMedia> CreateMediaPlayerFromMediaFunction;
        static LibVlcFunction<ReleaseMediaPlayer> ReleaseMediaPlayerFunction;
        static LibVlcFunction<RetainMediaPlayer> RetainMediaPlayerFunction;
        static LibVlcFunction<SetMedia> SetMediaFunction;
        static LibVlcFunction<GetMedia> GetMediaFunction;
        static LibVlcFunction<GetEventManager> GetEventManagerFunction;
        static LibVlcFunction<IsPlaying> IsPlayingFunction;
        static LibVlcFunction<Play> PlayFunction;
        static LibVlcFunction<SetPause> SetPauseFunction;
        static LibVlcFunction<SetPosition> SetPositionFunction;
        static LibVlcFunction<Stop> StopFunction;
        static LibVlcFunction<SetVideoCallback> SetVideoCallbackFunction;
        static LibVlcFunction<SetVideoFormat> SetVideoFormatFunction;
        static LibVlcFunction<SetVideoFormatCallback> SetVideoFormatCallbackFunction;
        static LibVlcFunction<SetHwnd> SetHwndFunction;
        static LibVlcFunction<GetHwnd> GetHwndFunction;
        static LibVlcFunction<SetAudioCallback> SetAudioCallbackFunction;
        static LibVlcFunction<SetAudioFormat> SetAudioFormatFunction;
        static LibVlcFunction<SetAudioFormatCallback> SetAudioFormatCallbackFunction;
        static LibVlcFunction<SetAudioVolumeCallback> SetAudioVolumeCallbackFunction;
        static LibVlcFunction<GetLength> GetLengthFunction;
        static LibVlcFunction<GetTime> GetTimeFunction;
        static LibVlcFunction<SetTime> SetTimeFunction;
        static LibVlcFunction<GetPosition> GetPositionFunction;
        static LibVlcFunction<SetChapter> SetChapterFunction;
        static LibVlcFunction<GetChapter> GetChapterFunction;
        static LibVlcFunction<GetChapterCount> GetChapterCountFunction;
        static LibVlcFunction<CanPlay> CanPlayFunction;
        static LibVlcFunction<GetTitleChapterCount> GetTitleChapterCountFunction;
        static LibVlcFunction<SetTitle> SetTitleFunction;
        static LibVlcFunction<GetTitle> GetTitleFunction;
        static LibVlcFunction<GetTitleCount> GetTitleCountFunction;
        static LibVlcFunction<PreviousChapter> PreviousChapterFunction;
        static LibVlcFunction<NextChapter> NextChapterFunction;
        static LibVlcFunction<GetRate> GetRateFunction;
        static LibVlcFunction<SetRate> SetRateFunction;
        static LibVlcFunction<GetState> GetStateFunction;
        static LibVlcFunction<GetFPS> GetFPSFunction;
        static LibVlcFunction<HasVout> HasVoutFunction;
        static LibVlcFunction<IsSeekable> IsSeekableFunction;
        static LibVlcFunction<CanPause> CanPauseFunction;
        static LibVlcFunction<NextFrame> NextFrameFunction;
        static LibVlcFunction<Navigate> NavigateFunction;
        static LibVlcFunction<SetVideoTitleDisplay> SetVideoTitleDisplayFunction;
        static LibVlcFunction<ReleaseTrackDescription> ReleaseTrackDescriptionFunction;
        static LibVlcFunction<ToggleMute> ToggleMuteFunction;
        static LibVlcFunction<GetMute> GetMuteFunction;
        static LibVlcFunction<SetMute> SetMuteFunction;
        static LibVlcFunction<GetVolume> GetVolumeFunction;
        static LibVlcFunction<SetVolume> SetVolumeFunction;
        static LibVlcFunction<GetCursor> GetCursorFunction;
        static LibVlcFunction<SetCursor> SetCursorFunction;
        static LibVlcFunction<SetMouseDown> SetMouseDownFunction;
        static LibVlcFunction<SetMouseUp> SetMouseUpFunction;

        LibVlcEventCallBack onPlaying;
        LibVlcEventCallBack onPaused;
        LibVlcEventCallBack onOpening;
        LibVlcEventCallBack onBuffering;
        LibVlcEventCallBack onStoped;
        LibVlcEventCallBack onForward;
        LibVlcEventCallBack onBackward;
        LibVlcEventCallBack onEndReached;
        LibVlcEventCallBack onMediaChanged;
        LibVlcEventCallBack onNothingSpecial;
        LibVlcEventCallBack onPausableChanged;
        LibVlcEventCallBack onPositionChanged;
        LibVlcEventCallBack onSeekableChanged;
        LibVlcEventCallBack onSnapshotTaken;
        LibVlcEventCallBack onTimeChanged;
        LibVlcEventCallBack onTitleChanged;
        LibVlcEventCallBack onVideoOutChanged;
        LibVlcEventCallBack onLengthChanged;
        LibVlcEventCallBack onEncounteredError;

        GCHandle onPlayingHandle;
        GCHandle onPausedHandle;
        GCHandle onOpeningHandle;
        GCHandle onBufferingHandle;
        GCHandle onStopedHandle;
        GCHandle onForwardHandle;
        GCHandle onBackwardHandle;
        GCHandle onEndReachedHandle;
        GCHandle onMediaChangedHandle;
        GCHandle onNothingSpecialHandle;
        GCHandle onPausableChangedHandle;
        GCHandle onPositionChangedHandle;
        GCHandle onSeekableChangedHandle;
        GCHandle onSnapshotTakenHandle;
        GCHandle onTimeChangedHandle;
        GCHandle onTitleChangedHandle;
        GCHandle onVideoOutChangedHandle;
        GCHandle onLengthChangedHandle;
        GCHandle onEncounteredErrorHandle;


        private VlcMediaPlayer(IntPtr pointer)
        {
            InstancePointer = pointer;
            EventManager = new VlcEventManager(GetEventManagerFunction.Delegate(InstancePointer));

            HandleManager.Add(this);

            onPlaying = OnPlaying;
            onPaused = OnPaused;
            onOpening = OnOpening;
            onBuffering = OnBuffering;
            onStoped = OnStoped;  //This Event has something wrong.
            onForward = OnForward;
            onBackward = OnBackward;
            onEndReached = OnEndReached;
            onMediaChanged = OnMediaChanged;
            onNothingSpecial = OnNothingSpecial;
            onPausableChanged = OnPausableChanged;
            onPositionChanged = OnPositionChanged;
            onSeekableChanged = OnSeekableChanged;
            onSnapshotTaken = OnSnapshotTaken;
            onTimeChanged = OnTimeChanged;
            onTitleChanged = OnTitleChanged;
            onVideoOutChanged = OnVideoOutChanged;
            onLengthChanged = OnLengthChanged;
            onEncounteredError = OnEncounteredError;

            onPlayingHandle = GCHandle.Alloc(onPlaying);
            onPausedHandle = GCHandle.Alloc(onPaused);
            onOpeningHandle = GCHandle.Alloc(onOpening);
            onBufferingHandle = GCHandle.Alloc(onBuffering);
            onStopedHandle = GCHandle.Alloc(onStoped);  //This Event has something wrong.
            onForwardHandle = GCHandle.Alloc(onForward);
            onBackwardHandle = GCHandle.Alloc(onBackward);
            onEndReachedHandle = GCHandle.Alloc(onEndReached);
            onMediaChangedHandle = GCHandle.Alloc(onMediaChanged);
            onNothingSpecialHandle = GCHandle.Alloc(onNothingSpecial);
            onPausableChangedHandle = GCHandle.Alloc(onPausableChanged);
            onPositionChangedHandle = GCHandle.Alloc(onPositionChanged);
            onSeekableChangedHandle = GCHandle.Alloc(onSeekableChanged);
            onSnapshotTakenHandle = GCHandle.Alloc(onSnapshotTaken);
            onTimeChangedHandle = GCHandle.Alloc(onTimeChanged);
            onTitleChangedHandle = GCHandle.Alloc(onTitleChanged);
            onVideoOutChangedHandle = GCHandle.Alloc(onVideoOutChanged);
            onLengthChangedHandle = GCHandle.Alloc(onLengthChanged);
            onEncounteredErrorHandle = GCHandle.Alloc(onEncounteredError);

            EventManager.Attach(EventTypes.MediaPlayerPlaying, onPlaying, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerPaused, onPaused, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerOpening, onOpening, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerBuffering, onBuffering, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerStopped, onStoped, IntPtr.Zero);  //This Event has something wrong.
            EventManager.Attach(EventTypes.MediaPlayerForward, onForward, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerBackward, onBackward, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerEndReached, onEndReached, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerMediaChanged, onMediaChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerNothingSpecial, onNothingSpecial, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerPausableChanged, onPausableChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerPositionChanged, onPositionChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerSeekableChanged, onSeekableChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerSnapshotTaken, onSnapshotTaken, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerTimeChanged, onTimeChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerTitleChanged, onTitleChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerVideoOutChanged, onVideoOutChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerLengthChanged, onLengthChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerEncounteredError, onEncounteredError, IntPtr.Zero);
        }

        #region 一般事件
        void OnPlaying(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            Playing?.Invoke(this, new EventArgs());
        }

        public event EventHandler Playing;

        void OnPaused(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (Paused != null)
            {
                Paused(this, new EventArgs());
            }
        }
        public event EventHandler Paused;

        void OnOpening(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (Opening != null)
            {
                Opening(this, new EventArgs());
            }
        }
        public event EventHandler Opening;

        void OnBuffering(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (Buffering != null)
            {
                Buffering(this, new MediaPlayerBufferingEventArgs(eventArgs.MediaPlayerBuffering.NewCache));
            }
        }
        public event EventHandler<MediaPlayerBufferingEventArgs> Buffering;


        void OnStoped(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (Stoped != null)
            {
                Stoped(this, new EventArgs());
            }
        }
        public event EventHandler Stoped;

        void OnForward(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (Forward != null)
            {
                Forward(this, new EventArgs());
            }
        }
        public event EventHandler Forward;

        void OnBackward(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (Backward != null)
            {
                Backward(this, new EventArgs());
            }
        }
        public event EventHandler Backward;

        void OnEndReached(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (EndReached != null)
            {
                EndReached(this, new EventArgs());
            }
        }
        public event EventHandler EndReached;

        void OnMediaChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (MediaChanged != null)
            {
                MediaChanged(this, new MediaPlayerMediaChangedEventArgs(HandleManager.GetVlcObject(eventArgs.MediaPlayerMediaChanged.NewMediaHandle) as VlcMedia));
            }
        }
        public event EventHandler<MediaPlayerMediaChangedEventArgs> MediaChanged;

        void OnNothingSpecial(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (NothingSpecial != null)
            {
                NothingSpecial(this, new EventArgs());
            }
        }
        public event EventHandler NothingSpecial;


        void OnPausableChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (PausableChanged != null)
            {
                PausableChanged(this, new EventArgs());
            }
        }
        public event EventHandler PausableChanged;

        void OnPositionChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (PositionChanged != null)
            {
                PositionChanged(this, new EventArgs());
            }
        }
        public event EventHandler PositionChanged;

        void OnSeekableChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (SeekableChanged != null)
            {
                SeekableChanged(this, new EventArgs());
            }
        }
        public event EventHandler SeekableChanged;

        void OnSnapshotTaken(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (SnapshotTaken != null)
            {
                SnapshotTaken(this, new EventArgs());
            }
        }
        public event EventHandler SnapshotTaken;

        void OnTimeChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (TimeChanged != null)
            {
                TimeChanged(this, new EventArgs());
            }
        }
        public event EventHandler TimeChanged;

        void OnTitleChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (TitleChanged != null)
            {
                TitleChanged(this, new EventArgs());
            }
        }
        public event EventHandler TitleChanged;

        void OnVideoOutChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (VideoOutChanged != null)
            {
                VideoOutChanged(this, new EventArgs());
            }
        }
        public event EventHandler VideoOutChanged;

        void OnLengthChanged(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (LengthChanged != null)
            {
                LengthChanged(this, new EventArgs());
            }
        }
        public event EventHandler LengthChanged;

        void OnEncounteredError(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (EncounteredError != null)
            {
                EncounteredError(this, new EventArgs());
            }
        }
        public event EventHandler EncounteredError;
        #endregion

        #region 属性 Media
        public VlcMedia Media
        {
            get
            {
                return InstancePointer == IntPtr.Zero ? null : HandleManager.GetVlcObject(GetMediaFunction.Delegate(InstancePointer)) as VlcMedia;
            }
            set
            {
                if(value == null)
                {
                    SetMediaFunction.Delegate(InstancePointer, IntPtr.Zero);
                }
                else
                {
                    SetMediaFunction.Delegate(InstancePointer, value.InstancePointer);
                }
            }
        }

        /// <summary>
        /// 获取一个值,该值表示 <see cref="VlcMediaPlayer"/> 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return IsPlayingFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示 <see cref="VlcMediaPlayer"/> 的播放进度,范围为0.0~1.0
        /// </summary>
        public float Position
        {
            get
            {
                return GetPositionFunction.Delegate(InstancePointer);
            }

            set
            {
                SetPositionFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示 <see cref="VlcMediaPlayer"/> 通过GDI的方式,将视频渲染到指定的窗口句柄
        /// </summary>
        public IntPtr Hwnd
        {
            get
            {
                return GetHwndFunction.Delegate(InstancePointer);
            }
            set
            {
                SetHwndFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示 <see cref="VlcMediaPlayer"/> 目前媒体的长度
        /// </summary>
        public TimeSpan Length
        {
            get
            {
                Int64 ms = GetLengthFunction.Delegate(InstancePointer);
                if(ms != -1)
                {
                    return new TimeSpan(ms * 10000);
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体播放进度
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                Int64 ms = GetTimeFunction.Delegate(InstancePointer);
                if (ms != -1)
                {
                    return new TimeSpan(ms * 10000);
                }
                else
                {
                    return TimeSpan.Zero;
                }
            }
            set
            {
                SetTimeFunction.Delegate(InstancePointer, (Int64)value.TotalMilliseconds);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前 <see cref="VlcMediaPlayer"/> 播放的章节
        /// </summary>
        public int Chapter
        {
            get
            {
                return GetChapterFunction.Delegate(InstancePointer);
            }
            set
            {
                SetChapterFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示媒体共有多少个章节
        /// </summary>
        public int ChapterCount
        {
            get
            {
                return GetChapterCountFunction.Delegate(InstancePointer);
            }
        }


        /// <summary>
        /// 获取一个值,该值表示现在媒体是否可以进行播放
        /// </summary>
        public bool CanPlay
        {
            get
            {
                return CanPlayFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示 <see cref="VlcMediaPlayer"/> 当前播放的标题
        /// </summary>
        public int Title
        {
            get
            {
                return GetTitleFunction.Delegate(InstancePointer);
            }

            set
            {
                SetTitleFunction.Delegate(InstancePointer, value);
            }
        }

        public int TitleCount
        {
            get
            {
                return GetTitleCountFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体的播放速率
        /// </summary>
        public float Rate
        {
            get
            {
                return GetRateFunction.Delegate(InstancePointer);
            }
            set
            {
                SetRateFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值,该值示当前媒体状态
        /// </summary>
        public Interop.Media.MediaState State
        {
            get
            {
                return GetStateFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前媒体的FPS
        /// </summary>
        public float FPS
        {
            get
            {
                return GetFPSFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前拥有的视频输出数量
        /// </summary>
        public uint HasVideoOutCount
        {
            get
            {
                return HasVoutFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前媒体是否允许跳进度
        /// </summary>
        public bool IsSeekable
        {
            get
            {
                return IsSeekableFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前媒体是否允许暂停
        /// </summary>
        public bool CanPause
        {
            get
            {
                return CanPauseFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体音频的音量
        /// </summary>
        public int Volume
        {
            get
            {
                return GetVolumeFunction.Delegate(InstancePointer);
            }

            set
            {
                SetVolumeFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体是否静音
        /// </summary>
        public bool IsMute
        {
            get
            {
                return GetMuteFunction.Delegate(InstancePointer) == 1;
            }

            set
            {
                SetMuteFunction.Delegate(InstancePointer, value ? 1 : 0);
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 使 <see cref="VlcMediaPlayer"/> 开始播放
        /// </summary>
        public void Play()
        {
            PlayFunction.Delegate(InstancePointer);
        }

        public void SetVideoDecodeCallback(VideoLockCallback lockCallback, VideoUnlockCallback unlockCallback, VideoDisplayCallback displayCallback, IntPtr userData)
        {
            SetVideoCallbackFunction.Delegate(InstancePointer, lockCallback, unlockCallback, displayCallback, userData);
        }

        public void SetVideoFormatCallback(VideoFormatCallback formatCallback, VideoCleanupCallback cleanupCallback)
        {
            SetVideoFormatCallbackFunction.Delegate(InstancePointer, formatCallback, cleanupCallback);
        }

        public void SetAudioCallback(AudioPlayCallback playCallback, AudioPauseCallback pauseCallback, AudioResumeCallback resumeCallback, AudioFlushCallback flushCallback, AudioDrainCallback drainCallback)
        {
            SetAudioCallbackFunction.Delegate(InstancePointer, playCallback, pauseCallback, resumeCallback, flushCallback, drainCallback);
        }

        public void SetAudioFormatCallback(AudioSetupCallback setupCallback, AudioCheanupCallback cheanupCallback)
        {
            SetAudioFormatCallbackFunction.Delegate(InstancePointer, setupCallback, cheanupCallback);
        }

        public void SetAudioVolumeCallback(AudioSetVolumeCallback volumeCallback)
        {
            SetAudioVolumeCallbackFunction.Delegate(InstancePointer, volumeCallback);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 播放或者暂停
        /// </summary>
        /// <param name="pause">true 代表暂停,false 代表播放或继续</param>
        public void SetPause(bool pause)
        {
            SetPauseFunction.Delegate(InstancePointer, pause);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 为暂停
        /// </summary>
        public void Pause()
        {
            SetPauseFunction.Delegate(InstancePointer, true);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 为播放
        /// </summary>
        public void Resume()
        {
            SetPauseFunction.Delegate(InstancePointer, false);
        }

        /// <summary>
        /// 当播放时设置 <see cref="VlcMediaPlayer"/> 为暂停,反之为播放
        /// </summary>
        public void PauseOrResume()
        {
            SetPauseFunction.Delegate(InstancePointer, IsPlaying);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 为停止
        /// </summary>
        public void Stop()
        {
            StopFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 设置 Audio 的格式
        /// </summary>
        /// <param name="format">格式字符串,一个四字符的字符串</param>
        /// <param name="rate">采样率</param>
        /// <param name="channels">通道数</param>
        public void SetAudioFormat(String format,uint rate,uint channels)
        {
            uint fmt = BitConverter.ToUInt32(new byte[] { (byte)format[0], (byte)format[1], (byte)format[2], (byte)format[3] }, 0);
            SetAudioFormatFunction.Delegate(InstancePointer, fmt, rate, channels);
        }

        public int GetTitleChapterCount(int title)
        {
            return GetTitleChapterCountFunction.Delegate(InstancePointer, title);
        }

        /// <summary>
        /// 播放上一个章节
        /// </summary>
        public void PreviousChapter()
        {
            PreviousChapterFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 播放下一个章节
        /// </summary>
        public void NextChapter()
        {
            NextChapterFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 播放下一帧
        /// </summary>
        public void NextFrame()
        {
            NextFrameFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 设置标题显示位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="timeout">显示时间</param>
        public void SetVideoTitleDisplay(Position pos, uint timeout)
        {
            SetVideoTitleDisplayFunction.Delegate(InstancePointer, pos, timeout);
        }

        /// <summary>
        /// 切换静音状态
        /// </summary>
        public void ToggleMute()
        {
            ToggleMuteFunction.Delegate(InstancePointer);
        }
        
        public bool SetMouseCursor(uint num, int x,int y)
        {
            return SetCursorFunction.Delegate(InstancePointer, num, x, y) == 0;
        }

        public bool SetMouseDown(uint num, MouseButton button)
        {
            return SetMouseDownFunction.Delegate(InstancePointer, num, button) == 0;
        }

        public bool SetMouseUp(uint num, MouseButton button)
        {
            return SetMouseDownFunction.Delegate(InstancePointer, num, button) == 0;
        }

        bool disposed = false;

        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            
            HandleManager.Remove(this);
            //EventManager.Dispose();
            onPlayingHandle.Free();
            onPausedHandle.Free();
            onOpeningHandle.Free();
            onBufferingHandle.Free();
            onStopedHandle.Free();
            onForwardHandle.Free();
            onBackwardHandle.Free();
            onEndReachedHandle.Free();
            onMediaChangedHandle.Free();
            onNothingSpecialHandle.Free();
            onPausableChangedHandle.Free();
            onPositionChangedHandle.Free();
            onSeekableChangedHandle.Free();
            onSnapshotTakenHandle.Free();
            onTimeChangedHandle.Free();
            onTitleChangedHandle.Free();
            onVideoOutChangedHandle.Free();
            onLengthChangedHandle.Free();
            onEncounteredErrorHandle.Free();
            ReleaseMediaPlayerFunction.Delegate(InstancePointer);
            InstancePointer = IntPtr.Zero;

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


    public class MediaPlayerBufferingEventArgs : EventArgs
    {
        public MediaPlayerBufferingEventArgs(float newCache)
        {
            NewCache = newCache;
        }
        public float NewCache { get; private set; }
    }

    public class MediaPlayerMediaChangedEventArgs : EventArgs
    {
        public MediaPlayerMediaChangedEventArgs(VlcMedia newMedia)
        {
            NewMedia = newMedia;
        }
        public VlcMedia NewMedia { get; private set; }
    }
}
