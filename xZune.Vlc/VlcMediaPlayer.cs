using System;
using System.Runtime.InteropServices;

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
            return new VlcMediaPlayer(_createMediaPlayerFunction.Delegate(vlc.InstancePointer));
        }

        public static VlcMediaPlayer CreatFormMedia(VlcMedia media)
        {
            return new VlcMediaPlayer(_createMediaPlayerFromMediaFunction.Delegate(media.InstancePointer));
        }

        /// <summary>
        /// 载入 LibVlc 的 MediaPlayer 模块,该方法会在 <see cref="Vlc.LoadLibVlc()"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        /// <param name="devString"></param>
        public static void LoadLibVlc(IntPtr libHandle, Version libVersion, String devString)
        {
            if (!IsLibLoaded)
            {
                _createMediaPlayerFunction = new LibVlcFunction<CreateMediaPlayer>(libHandle, libVersion, devString);
                _createMediaPlayerFromMediaFunction = new LibVlcFunction<CreateMediaPlayerFromMedia>(libHandle, libVersion, devString);
                _releaseMediaPlayerFunction = new LibVlcFunction<ReleaseMediaPlayer>(libHandle, libVersion, devString);
                _retainMediaPlayerFunction = new LibVlcFunction<RetainMediaPlayer>(libHandle, libVersion, devString);
                _setMediaFunction = new LibVlcFunction<SetMedia>(libHandle, libVersion, devString);
                _getMediaFunction = new LibVlcFunction<GetMedia>(libHandle, libVersion, devString);
                _getEventManagerFunction = new LibVlcFunction<GetEventManager>(libHandle, libVersion, devString);
                _isPlayingFunction = new LibVlcFunction<IsPlaying>(libHandle, libVersion, devString);
                _playFunction = new LibVlcFunction<Play>(libHandle, libVersion, devString);
                _setPauseFunction = new LibVlcFunction<SetPause>(libHandle, libVersion, devString);
                _setPositionFunction = new LibVlcFunction<SetPosition>(libHandle, libVersion, devString);
                _stopFunction = new LibVlcFunction<Stop>(libHandle, libVersion, devString);
                _setVideoCallbackFunction = new LibVlcFunction<SetVideoCallback>(libHandle, libVersion, devString);
                _setVideoFormatFunction = new LibVlcFunction<SetVideoFormat>(libHandle, libVersion, devString);
                _setVideoFormatCallbackFunction = new LibVlcFunction<SetVideoFormatCallback>(libHandle, libVersion, devString);
                _setHwndFunction = new LibVlcFunction<SetHwnd>(libHandle, libVersion, devString);
                _getHwndFunction = new LibVlcFunction<GetHwnd>(libHandle, libVersion, devString);
                _setAudioCallbackFunction = new LibVlcFunction<SetAudioCallback>(libHandle, libVersion, devString);
                _setAudioFormatFunction = new LibVlcFunction<SetAudioFormat>(libHandle, libVersion, devString);
                _setAudioFormatCallbackFunction = new LibVlcFunction<SetAudioFormatCallback>(libHandle, libVersion, devString);
                _setAudioVolumeCallbackFunction = new LibVlcFunction<SetAudioVolumeCallback>(libHandle, libVersion, devString);
                _getLengthFunction = new LibVlcFunction<GetLength>(libHandle, libVersion, devString);
                _getTimeFunction = new LibVlcFunction<GetTime>(libHandle, libVersion, devString);
                _setTimeFunction = new LibVlcFunction<SetTime>(libHandle, libVersion, devString);
                _getPositionFunction = new LibVlcFunction<GetPosition>(libHandle, libVersion, devString);
                _setChapterFunction = new LibVlcFunction<SetChapter>(libHandle, libVersion, devString);
                _getChapterFunction = new LibVlcFunction<GetChapter>(libHandle, libVersion, devString);
                _getChapterCountFunction = new LibVlcFunction<GetChapterCount>(libHandle, libVersion, devString);
                _canPlayFunction = new LibVlcFunction<CanPlay>(libHandle, libVersion, devString);
                _getTitleChapterCountFunction = new LibVlcFunction<GetTitleChapterCount>(libHandle, libVersion, devString);
                _setTitleFunction = new LibVlcFunction<SetTitle>(libHandle, libVersion, devString);
                _getTitleFunction = new LibVlcFunction<GetTitle>(libHandle, libVersion, devString);
                _getTitleCountFunction = new LibVlcFunction<GetTitleCount>(libHandle, libVersion, devString);
                _previousChapterFunction = new LibVlcFunction<PreviousChapter>(libHandle, libVersion, devString);
                _nextChapterFunction = new LibVlcFunction<NextChapter>(libHandle, libVersion, devString);
                _getRateFunction = new LibVlcFunction<GetRate>(libHandle, libVersion, devString);
                _setRateFunction = new LibVlcFunction<SetRate>(libHandle, libVersion, devString);
                _getStateFunction = new LibVlcFunction<GetState>(libHandle, libVersion, devString);
                _getFpsFunction = new LibVlcFunction<GetFps>(libHandle, libVersion, devString);
                _hasVoutFunction = new LibVlcFunction<HasVout>(libHandle, libVersion, devString);
                _isSeekableFunction = new LibVlcFunction<IsSeekable>(libHandle, libVersion, devString);
                _canPauseFunction = new LibVlcFunction<CanPause>(libHandle, libVersion, devString);
                _nextFrameFunction = new LibVlcFunction<NextFrame>(libHandle, libVersion, devString);
                _navigateFunction = new LibVlcFunction<Navigate>(libHandle, libVersion, devString);
                _setVideoTitleDisplayFunction = new LibVlcFunction<SetVideoTitleDisplay>(libHandle, libVersion, devString);
                _releaseTrackDescriptionFunction = new LibVlcFunction<ReleaseTrackDescription>(libHandle, libVersion, devString);
                _toggleMuteFunction = new LibVlcFunction<ToggleMute>(libHandle, libVersion, devString);
                _getMuteFunction = new LibVlcFunction<GetMute>(libHandle, libVersion, devString);
                _setMuteFunction = new LibVlcFunction<SetMute>(libHandle, libVersion, devString);
                _getVolumeFunction = new LibVlcFunction<GetVolume>(libHandle, libVersion, devString);
                _setVolumeFunction = new LibVlcFunction<SetVolume>(libHandle, libVersion, devString);
                _getCursorFunction = new LibVlcFunction<GetCursor>(libHandle, libVersion, devString);
                _setCursorFunction = new LibVlcFunction<SetCursor>(libHandle, libVersion, devString);
                _setMouseDownFunction = new LibVlcFunction<SetMouseDown>(libHandle, libVersion, devString);
                _setMouseUpFunction = new LibVlcFunction<SetMouseUp>(libHandle, libVersion, devString);
                _getOutputChannelFunction = new LibVlcFunction<GetOutputChannel>(libHandle, libVersion, devString);
                _setOutputChannelFunction = new LibVlcFunction<SetOutputChannel>(libHandle, libVersion, devString);
                _getAudioTrackFunction = new LibVlcFunction<GetAudioTrack>(libHandle, libVersion, devString);
                _setAudioTrackFunction = new LibVlcFunction<SetAudioTrack>(libHandle, libVersion, devString);
                _getAudioTrackCountFunction = new LibVlcFunction<GetAudioTrackCount>(libHandle, libVersion, devString);
                _getAudioTrackDescriptionFunction = new LibVlcFunction<GetAudioTrackDescription>(libHandle, libVersion, devString);
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

        static LibVlcFunction<CreateMediaPlayer> _createMediaPlayerFunction;
        static LibVlcFunction<CreateMediaPlayerFromMedia> _createMediaPlayerFromMediaFunction;
        static LibVlcFunction<ReleaseMediaPlayer> _releaseMediaPlayerFunction;
        static LibVlcFunction<RetainMediaPlayer> _retainMediaPlayerFunction;
        static LibVlcFunction<SetMedia> _setMediaFunction;
        static LibVlcFunction<GetMedia> _getMediaFunction;
        static LibVlcFunction<GetEventManager> _getEventManagerFunction;
        static LibVlcFunction<IsPlaying> _isPlayingFunction;
        static LibVlcFunction<Play> _playFunction;
        static LibVlcFunction<SetPause> _setPauseFunction;
        static LibVlcFunction<SetPosition> _setPositionFunction;
        static LibVlcFunction<Stop> _stopFunction;
        static LibVlcFunction<SetVideoCallback> _setVideoCallbackFunction;
        static LibVlcFunction<SetVideoFormat> _setVideoFormatFunction;
        static LibVlcFunction<SetVideoFormatCallback> _setVideoFormatCallbackFunction;
        static LibVlcFunction<SetHwnd> _setHwndFunction;
        static LibVlcFunction<GetHwnd> _getHwndFunction;
        static LibVlcFunction<SetAudioCallback> _setAudioCallbackFunction;
        static LibVlcFunction<SetAudioFormat> _setAudioFormatFunction;
        static LibVlcFunction<SetAudioFormatCallback> _setAudioFormatCallbackFunction;
        static LibVlcFunction<SetAudioVolumeCallback> _setAudioVolumeCallbackFunction;
        static LibVlcFunction<GetLength> _getLengthFunction;
        static LibVlcFunction<GetTime> _getTimeFunction;
        static LibVlcFunction<SetTime> _setTimeFunction;
        static LibVlcFunction<GetPosition> _getPositionFunction;
        static LibVlcFunction<SetChapter> _setChapterFunction;
        static LibVlcFunction<GetChapter> _getChapterFunction;
        static LibVlcFunction<GetChapterCount> _getChapterCountFunction;
        static LibVlcFunction<CanPlay> _canPlayFunction;
        static LibVlcFunction<GetTitleChapterCount> _getTitleChapterCountFunction;
        static LibVlcFunction<SetTitle> _setTitleFunction;
        static LibVlcFunction<GetTitle> _getTitleFunction;
        static LibVlcFunction<GetTitleCount> _getTitleCountFunction;
        static LibVlcFunction<PreviousChapter> _previousChapterFunction;
        static LibVlcFunction<NextChapter> _nextChapterFunction;
        static LibVlcFunction<GetRate> _getRateFunction;
        static LibVlcFunction<SetRate> _setRateFunction;
        static LibVlcFunction<GetState> _getStateFunction;
        static LibVlcFunction<GetFps> _getFpsFunction;
        static LibVlcFunction<HasVout> _hasVoutFunction;
        static LibVlcFunction<IsSeekable> _isSeekableFunction;
        static LibVlcFunction<CanPause> _canPauseFunction;
        static LibVlcFunction<NextFrame> _nextFrameFunction;
        static LibVlcFunction<Navigate> _navigateFunction;
        static LibVlcFunction<SetVideoTitleDisplay> _setVideoTitleDisplayFunction;
        static LibVlcFunction<ReleaseTrackDescription> _releaseTrackDescriptionFunction;
        static LibVlcFunction<ToggleMute> _toggleMuteFunction;
        static LibVlcFunction<GetMute> _getMuteFunction;
        static LibVlcFunction<SetMute> _setMuteFunction;
        static LibVlcFunction<GetVolume> _getVolumeFunction;
        static LibVlcFunction<SetVolume> _setVolumeFunction;
        static LibVlcFunction<GetCursor> _getCursorFunction;
        static LibVlcFunction<SetCursor> _setCursorFunction;
        static LibVlcFunction<SetMouseDown> _setMouseDownFunction;
        static LibVlcFunction<SetMouseUp> _setMouseUpFunction;
        static LibVlcFunction<GetOutputChannel> _getOutputChannelFunction;
        static LibVlcFunction<SetOutputChannel> _setOutputChannelFunction;
        static LibVlcFunction<GetAudioTrack> _getAudioTrackFunction;
        static LibVlcFunction<SetAudioTrack> _setAudioTrackFunction;
        static LibVlcFunction<GetAudioTrackCount> _getAudioTrackCountFunction;
        static LibVlcFunction<GetAudioTrackDescription> _getAudioTrackDescriptionFunction;


        readonly LibVlcEventCallBack _onPlaying;
        readonly LibVlcEventCallBack _onPaused;
        readonly LibVlcEventCallBack _onOpening;
        readonly LibVlcEventCallBack _onBuffering;
        readonly LibVlcEventCallBack _onStoped;
        readonly LibVlcEventCallBack _onForward;
        readonly LibVlcEventCallBack _onBackward;
        readonly LibVlcEventCallBack _onEndReached;
        readonly LibVlcEventCallBack _onMediaChanged;
        readonly LibVlcEventCallBack _onNothingSpecial;
        readonly LibVlcEventCallBack _onPausableChanged;
        readonly LibVlcEventCallBack _onPositionChanged;
        readonly LibVlcEventCallBack _onSeekableChanged;
        readonly LibVlcEventCallBack _onSnapshotTaken;
        readonly LibVlcEventCallBack _onTimeChanged;
        readonly LibVlcEventCallBack _onTitleChanged;
        readonly LibVlcEventCallBack _onVideoOutChanged;
        readonly LibVlcEventCallBack _onLengthChanged;
        readonly LibVlcEventCallBack _onEncounteredError;

        GCHandle _onPlayingHandle;
        GCHandle _onPausedHandle;
        GCHandle _onOpeningHandle;
        GCHandle _onBufferingHandle;
        GCHandle _onStopedHandle;
        GCHandle _onForwardHandle;
        GCHandle _onBackwardHandle;
        GCHandle _onEndReachedHandle;
        GCHandle _onMediaChangedHandle;
        GCHandle _onNothingSpecialHandle;
        GCHandle _onPausableChangedHandle;
        GCHandle _onPositionChangedHandle;
        GCHandle _onSeekableChangedHandle;
        GCHandle _onSnapshotTakenHandle;
        GCHandle _onTimeChangedHandle;
        GCHandle _onTitleChangedHandle;
        GCHandle _onVideoOutChangedHandle;
        GCHandle _onLengthChangedHandle;
        GCHandle _onEncounteredErrorHandle;


        private VlcMediaPlayer(IntPtr pointer)
        {
            InstancePointer = pointer;
            EventManager = new VlcEventManager(_getEventManagerFunction.Delegate(InstancePointer));

            HandleManager.Add(this);

            _onPlaying = OnPlaying;
            _onPaused = OnPaused;
            _onOpening = OnOpening;
            _onBuffering = OnBuffering;
            _onStoped = OnStoped;  //This Event has something wrong.
            _onForward = OnForward;
            _onBackward = OnBackward;
            _onEndReached = OnEndReached;
            _onMediaChanged = OnMediaChanged;
            _onNothingSpecial = OnNothingSpecial;
            _onPausableChanged = OnPausableChanged;
            _onPositionChanged = OnPositionChanged;
            _onSeekableChanged = OnSeekableChanged;
            _onSnapshotTaken = OnSnapshotTaken;
            _onTimeChanged = OnTimeChanged;
            _onTitleChanged = OnTitleChanged;
            _onVideoOutChanged = OnVideoOutChanged;
            _onLengthChanged = OnLengthChanged;
            _onEncounteredError = OnEncounteredError;

            _onPlayingHandle = GCHandle.Alloc(_onPlaying);
            _onPausedHandle = GCHandle.Alloc(_onPaused);
            _onOpeningHandle = GCHandle.Alloc(_onOpening);
            _onBufferingHandle = GCHandle.Alloc(_onBuffering);
            _onStopedHandle = GCHandle.Alloc(_onStoped);  //This Event has something wrong.
            _onForwardHandle = GCHandle.Alloc(_onForward);
            _onBackwardHandle = GCHandle.Alloc(_onBackward);
            _onEndReachedHandle = GCHandle.Alloc(_onEndReached);
            _onMediaChangedHandle = GCHandle.Alloc(_onMediaChanged);
            _onNothingSpecialHandle = GCHandle.Alloc(_onNothingSpecial);
            _onPausableChangedHandle = GCHandle.Alloc(_onPausableChanged);
            _onPositionChangedHandle = GCHandle.Alloc(_onPositionChanged);
            _onSeekableChangedHandle = GCHandle.Alloc(_onSeekableChanged);
            _onSnapshotTakenHandle = GCHandle.Alloc(_onSnapshotTaken);
            _onTimeChangedHandle = GCHandle.Alloc(_onTimeChanged);
            _onTitleChangedHandle = GCHandle.Alloc(_onTitleChanged);
            _onVideoOutChangedHandle = GCHandle.Alloc(_onVideoOutChanged);
            _onLengthChangedHandle = GCHandle.Alloc(_onLengthChanged);
            _onEncounteredErrorHandle = GCHandle.Alloc(_onEncounteredError);

            EventManager.Attach(EventTypes.MediaPlayerPlaying, _onPlaying, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerPaused, _onPaused, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerOpening, _onOpening, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerBuffering, _onBuffering, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerStopped, _onStoped, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerForward, _onForward, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerBackward, _onBackward, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerEndReached, _onEndReached, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerMediaChanged, _onMediaChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerNothingSpecial, _onNothingSpecial, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerPausableChanged, _onPausableChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerPositionChanged, _onPositionChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerSeekableChanged, _onSeekableChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerSnapshotTaken, _onSnapshotTaken, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerTimeChanged, _onTimeChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerTitleChanged, _onTitleChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerVideoOutChanged, _onVideoOutChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerLengthChanged, _onLengthChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaPlayerEncounteredError, _onEncounteredError, IntPtr.Zero);
        }

        #region 一般事件
        void OnPlaying(ref LibVlcEventArgs eventArgs, IntPtr userData)
        {
            if (Playing != null)
            {
                Playing(this, new EventArgs());
            }
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
                return InstancePointer == IntPtr.Zero ? null : HandleManager.GetVlcObject(_getMediaFunction.Delegate(InstancePointer)) as VlcMedia;
            }
            set
            {
                _setMediaFunction.Delegate(InstancePointer, value == null ? IntPtr.Zero : value.InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示 <see cref="VlcMediaPlayer"/> 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return _isPlayingFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示 <see cref="VlcMediaPlayer"/> 的播放进度,范围为0.0~1.0
        /// </summary>
        public float Position
        {
            get
            {
                return _getPositionFunction.Delegate(InstancePointer);
            }

            set
            {
                _setPositionFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示 <see cref="VlcMediaPlayer"/> 通过GDI的方式,将视频渲染到指定的窗口句柄
        /// </summary>
        public IntPtr Hwnd
        {
            get
            {
                return _getHwndFunction.Delegate(InstancePointer);
            }
            set
            {
                _setHwndFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示 <see cref="VlcMediaPlayer"/> 目前媒体的长度
        /// </summary>
        public TimeSpan Length
        {
            get
            {
                var ms = _getLengthFunction.Delegate(InstancePointer);
                return ms != -1 ? new TimeSpan(ms * 10000) : TimeSpan.Zero;
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体播放进度
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                var ms = _getTimeFunction.Delegate(InstancePointer);
                return ms != -1 ? new TimeSpan(ms * 10000) : TimeSpan.Zero;
            }
            set
            {
                _setTimeFunction.Delegate(InstancePointer, (Int64)value.TotalMilliseconds);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前 <see cref="VlcMediaPlayer"/> 播放的章节
        /// </summary>
        public int Chapter
        {
            get
            {
                return _getChapterFunction.Delegate(InstancePointer);
            }
            set
            {
                _setChapterFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示媒体共有多少个章节
        /// </summary>
        public int ChapterCount
        {
            get
            {
                return _getChapterCountFunction.Delegate(InstancePointer);
            }
        }


        /// <summary>
        /// 获取一个值,该值表示现在媒体是否可以进行播放
        /// </summary>
        public bool CanPlay
        {
            get
            {
                return _canPlayFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示 <see cref="VlcMediaPlayer"/> 当前播放的标题
        /// </summary>
        public int Title
        {
            get
            {
                return _getTitleFunction.Delegate(InstancePointer);
            }

            set
            {
                _setTitleFunction.Delegate(InstancePointer, value);
            }
        }

        public int TitleCount
        {
            get
            {
                return _getTitleCountFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体的播放速率
        /// </summary>
        public float Rate
        {
            get
            {
                return _getRateFunction.Delegate(InstancePointer);
            }
            set
            {
                _setRateFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值,该值示当前媒体状态
        /// </summary>
        public Interop.Media.MediaState State
        {
            get
            {
                return _getStateFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前媒体的FPS
        /// </summary>
        public float Fps
        {
            get
            {
                return _getFpsFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前拥有的视频输出数量
        /// </summary>
        public uint HasVideoOutCount
        {
            get
            {
                return _hasVoutFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前媒体是否允许跳进度
        /// </summary>
        public bool IsSeekable
        {
            get
            {
                return _isSeekableFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取一个值,该值表示当前媒体是否允许暂停
        /// </summary>
        public bool CanPause
        {
            get
            {
                return _canPauseFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体音频的音量
        /// </summary>
        public int Volume
        {
            get
            {
                return _getVolumeFunction.Delegate(InstancePointer);
            }

            set
            {
                _setVolumeFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取或设置一个值,该值表示当前媒体是否静音
        /// </summary>
        public bool IsMute
        {
            get
            {
                return _getMuteFunction.Delegate(InstancePointer) == 1;
            }

            set
            {
                _setMuteFunction.Delegate(InstancePointer, value ? 1 : 0);
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值表示音频输出通道
        /// </summary>
        public AudioOutputChannel AudioOutputChannel
        {
            get
            {
                return _getOutputChannelFunction.Delegate(InstancePointer);
            }

            set
            {
                _setOutputChannelFunction.Delegate(InstancePointer, value);
            }
        }

        public int AudioTrackCount
        {
            get
            {
                return _getAudioTrackCountFunction.Delegate(InstancePointer);
            }
        }

        public int AudioTrack
        {
            get
            {
                return _getAudioTrackFunction.Delegate(InstancePointer);
            }
            set
            {
                _setAudioTrackFunction.Delegate(InstancePointer, value);
            }
        }

        public TrackDescription AudioTrackDescription
        {
            get
            {
                return new TrackDescription(_getAudioTrackDescriptionFunction.Delegate(InstancePointer));
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 使 <see cref="VlcMediaPlayer"/> 开始播放
        /// </summary>
        public void Play()
        {
            _playFunction.Delegate(InstancePointer);
        }

        public void SetVideoDecodeCallback(VideoLockCallback lockCallback, VideoUnlockCallback unlockCallback, VideoDisplayCallback displayCallback, IntPtr userData)
        {
            _setVideoCallbackFunction.Delegate(InstancePointer, lockCallback, unlockCallback, displayCallback, userData);
        }

        public void SetVideoFormatCallback(VideoFormatCallback formatCallback, VideoCleanupCallback cleanupCallback)
        {
            _setVideoFormatCallbackFunction.Delegate(InstancePointer, formatCallback, cleanupCallback);
        }

        public void SetAudioCallback(AudioPlayCallback playCallback, AudioPauseCallback pauseCallback, AudioResumeCallback resumeCallback, AudioFlushCallback flushCallback, AudioDrainCallback drainCallback)
        {
            _setAudioCallbackFunction.Delegate(InstancePointer, playCallback, pauseCallback, resumeCallback, flushCallback, drainCallback);
        }

        public void SetAudioFormatCallback(AudioSetupCallback setupCallback, AudioCheanupCallback cheanupCallback)
        {
            _setAudioFormatCallbackFunction.Delegate(InstancePointer, setupCallback, cheanupCallback);
        }

        public void SetAudioVolumeCallback(AudioSetVolumeCallback volumeCallback)
        {
            _setAudioVolumeCallbackFunction.Delegate(InstancePointer, volumeCallback);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 播放或者暂停
        /// </summary>
        /// <param name="pause">true 代表暂停,false 代表播放或继续</param>
        public void SetPause(bool pause)
        {
            _setPauseFunction.Delegate(InstancePointer, pause);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 为暂停
        /// </summary>
        public void Pause()
        {
            _setPauseFunction.Delegate(InstancePointer, true);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 为播放
        /// </summary>
        public void Resume()
        {
            _setPauseFunction.Delegate(InstancePointer, false);
        }

        /// <summary>
        /// 当播放时设置 <see cref="VlcMediaPlayer"/> 为暂停,反之为播放
        /// </summary>
        public void PauseOrResume()
        {
            _setPauseFunction.Delegate(InstancePointer, IsPlaying);
        }

        /// <summary>
        /// 设置 <see cref="VlcMediaPlayer"/> 为停止
        /// </summary>
        public void Stop()
        {
            _stopFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 设置 Audio 的格式
        /// </summary>
        /// <param name="format">格式字符串,一个四字符的字符串</param>
        /// <param name="rate">采样率</param>
        /// <param name="channels">通道数</param>
        public void SetAudioFormat(String format,uint rate,uint channels)
        {
            var fmt = BitConverter.ToUInt32(new [] { (byte)format[0], (byte)format[1], (byte)format[2], (byte)format[3] }, 0);
            _setAudioFormatFunction.Delegate(InstancePointer, fmt, rate, channels);
        }

        public int GetTitleChapterCount(int title)
        {
            return _getTitleChapterCountFunction.Delegate(InstancePointer, title);
        }

        /// <summary>
        /// 播放上一个章节
        /// </summary>
        public void PreviousChapter()
        {
            _previousChapterFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 播放下一个章节
        /// </summary>
        public void NextChapter()
        {
            _nextChapterFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 播放下一帧
        /// </summary>
        public void NextFrame()
        {
            _nextFrameFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 设置标题显示位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="timeout">显示时间</param>
        public void SetVideoTitleDisplay(Position pos, uint timeout)
        {
            _setVideoTitleDisplayFunction.Delegate(InstancePointer, pos, timeout);
        }

        /// <summary>
        /// 切换静音状态
        /// </summary>
        public void ToggleMute()
        {
            _toggleMuteFunction.Delegate(InstancePointer);
        }

        public void SetVideoFormat(String chroma, uint width, uint height, uint pitch)
        {
            _setVideoFormatFunction.Delegate(InstancePointer, chroma, width, height, pitch);
        }

        /// <summary>
        /// 获取鼠标坐标
        /// </summary>
        /// <param name="num">视频输出号</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void GetMouseCursor(uint num, ref int x,ref int y)
        {
            _getCursorFunction.Delegate(InstancePointer, num, ref x, ref y);
        }
        
        public bool SetMouseCursor(uint num, int x,int y)
        {
            return _setCursorFunction.Delegate(InstancePointer, num, x, y) == 0;
        }

        public bool SetMouseDown(uint num, MouseButton button)
        {
            return _setMouseDownFunction.Delegate(InstancePointer, num, button) == 0;
        }

        public bool SetMouseUp(uint num, MouseButton button)
        {
            return _setMouseUpFunction.Delegate(InstancePointer, num, button) == 0;
        }

        public static void ReleaseTrackDescription(TrackDescription trackDescription)
        {
            _releaseTrackDescriptionFunction.Delegate(trackDescription.Pointer);
        }

        public void Navigate(NavigateMode mode)
        {
            _navigateFunction.Delegate(InstancePointer, mode);
        }

        bool _disposed;

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            //EventManager.Dispose();
            HandleManager.Remove(this);

            EventManager.Detach(EventTypes.MediaPlayerPlaying, _onPlaying, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerPaused, _onPaused, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerOpening, _onOpening, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerBuffering, _onBuffering, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerStopped, _onStoped, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerForward, _onForward, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerBackward, _onBackward, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerEndReached, _onEndReached, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerMediaChanged, _onMediaChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerNothingSpecial, _onNothingSpecial, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerPausableChanged, _onPausableChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerPositionChanged, _onPositionChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerSeekableChanged, _onSeekableChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerSnapshotTaken, _onSnapshotTaken, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerTimeChanged, _onTimeChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerTitleChanged, _onTitleChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerVideoOutChanged, _onVideoOutChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerLengthChanged, _onLengthChanged, IntPtr.Zero);
            EventManager.Detach(EventTypes.MediaPlayerEncounteredError, _onEncounteredError, IntPtr.Zero);

            _onPlayingHandle.Free();
            _onPausedHandle.Free();
            _onOpeningHandle.Free();
            _onBufferingHandle.Free();
            _onStopedHandle.Free();
            _onForwardHandle.Free();
            _onBackwardHandle.Free();
            _onEndReachedHandle.Free();
            _onMediaChangedHandle.Free();
            _onNothingSpecialHandle.Free();
            _onPausableChangedHandle.Free();
            _onPositionChangedHandle.Free();
            _onSeekableChangedHandle.Free();
            _onSnapshotTakenHandle.Free();
            _onTimeChangedHandle.Free();
            _onTitleChangedHandle.Free();
            _onVideoOutChangedHandle.Free();
            _onLengthChangedHandle.Free();
            _onEncounteredErrorHandle.Free();
            _releaseMediaPlayerFunction.Delegate(InstancePointer);
            InstancePointer = IntPtr.Zero;

            _disposed = true;
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
