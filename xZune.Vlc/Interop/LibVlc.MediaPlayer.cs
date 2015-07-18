using System;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop.MediaPlayer
{
    /// <summary>
    /// 表示一个视频,音频,或者文本的描述
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TrackDescription
    {
        public int Id;
        [MarshalAs(UnmanagedType.LPStr)]
        public String Name;
        /// <summary>
        /// 这是一个 <see cref="TrackDescription"/> 类型的指针,指向下一个描述
        /// </summary>
        public IntPtr Next;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct AudioOutput
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public String Name;
        [MarshalAs(UnmanagedType.LPStr)]
        public String Description;
        /// <summary>
        /// 这是一个 <see cref="AudioOutput"/> 类型的指针,指向下一个音频输出
        /// </summary>
        public IntPtr Next;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct AudioOutputDevice
    {
        /// <summary>
        /// 这是一个 <see cref="AudioOutputDevice"/> 类型的指针,指向下一个音频输出设备
        /// </summary>
        public IntPtr Next;
        /// <summary>
        /// 设备标识符
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public String Device;
        /// <summary>
        /// 设备描述
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public String Description;
    }

    public struct Rectangle
    {
        public int Top;
        public int Left;
        public int Bottom;
        public int Right;
    }

    /// <summary>
    /// 视频字幕设定项
    /// </summary>
    public enum VideoMarqueeOption
    {
        Enable = 0,
        Text,
        Color,
        Opacity,
        Position,
        Refresh,
        Size,
        Timeout,
        X,
        Y
    }

    public enum NavigateMode
    {
        Activate = 0,
        Up,
        Down,
        Left,
        Right
    }

    public enum Position
    {
        Disable = -1,
        Center,
        Left,
        Right,
        Top,
        TopLeft,
        TopRight,
        Bottom,
        BottomLeft,
        BottomRight
    }

    /// <summary>
    /// 当锁定图像缓存时,调用此回调.
    /// 每当一个新帧需要被解码,都会调用此回调,一个或者三个像素平面会被通过第二个参数返回.这些像素屏幕需要 32 字节对齐
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="planes">像素平面</param>
    /// <returns>一个私有指针用来显示或解锁回调用来识别图像缓存</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr VideoLockCallback(IntPtr opaque, ref IntPtr planes);

    /// <summary>
    /// 当解锁图像缓存时,调用此回调.
    /// 每当一个帧被解码完成,都会调用此回调,该回调并不是必须的,但是它是读取像素值的唯一的途径.
    /// 该回调会发生在图片解码之后,显示之前
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="picture">由 <see cref="VideoLockCallback"/> 返回的指针</param>
    /// <param name="planes">像素平面</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VideoUnlockCallback(IntPtr opaque, IntPtr picture, ref IntPtr planes);

    /// <summary>
    /// 当显示图像时,调用此回调.
    /// 每当一个帧需要被显示时,都会调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="picture">由 <see cref="VideoLockCallback"/> 返回的指针</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VideoDisplayCallback(IntPtr opaque, IntPtr picture);

    /// <summary>
    /// 当配置图像缓存格式时,调用此回调.
    /// 此回调会获取由解码器和过滤器(如果有)输出的视频的格式,
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="chroma">视频格式识别码</param>
    /// <param name="width">像素宽</param>
    /// <param name="height">像素高</param>
    /// <param name="pitches">每个像素平面字节的扫描线间距</param>
    /// <param name="lines">每个像素平面的扫描线的个数</param>
    /// <returns>分配的图片缓存大小,0代表失败</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint VideoFormatCallback(ref IntPtr opaque, ref uint chroma, ref uint width, ref uint height, ref uint pitches, ref uint lines);

    /// <summary>
    /// 配置图片缓存格式时,调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VideoCleanupCallback(IntPtr opaque);

    /// <summary>
    /// 音频播放时,调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="sample">采样数据</param>
    /// <param name="count">采样数</param>
    /// <param name="pts">预计播放时间戳</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioPlayCallback(IntPtr opaque, IntPtr sample, uint count, Int64 pts);

    /// <summary>
    /// 音频暂停时,调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="pts">请求暂停的时间戳</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioPauseCallback(IntPtr opaque, Int64 pts);

    /// <summary>
    /// 音频继续播放时,调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="pts">请求继续的时间戳</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioResumeCallback(IntPtr opaque, Int64 pts);

    /// <summary>
    /// 音频缓冲刷新时,调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="pts"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioFlushCallback(IntPtr opaque, Int64 pts);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="opaque"></param>
    /// <param name="pts"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioDrainCallback(IntPtr opaque, Int64 pts);

    /// <summary>
    /// 音频格式完成配置时调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="format">格式字符串,一个四字符的字符串</param>
    /// <param name="rate">采样率</param>
    /// <param name="channels">通道数</param>
    /// <returns>0代表成功</returns>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int AudioSetupCallback(ref IntPtr opaque, IntPtr format, ref uint rate, ref uint channels);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="opaque"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioCheanupCallback(IntPtr opaque);

    /// <summary>
    /// 音频设置音量时,调用此回调
    /// </summary>
    /// <param name="opaque">一个私有指针</param>
    /// <param name="volume">音量</param>
    /// <param name="mute">是否为静音</param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void AudioSetVolumeCallback(IntPtr opaque, float volume, bool mute);

    /// <summary>
    /// 创建一个空的媒体播放器对象
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <returns>创建好的媒体播放器对象指针</returns>
    [LibVlcFunction("libvlc_media_player_new")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr CreateMediaPlayer(IntPtr instance);

    /// <summary>
    /// 通过一个媒体对象创建一个媒体播放器对象
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>创建好的媒体播放器对象指针</returns>
    [LibVlcFunction("libvlc_media_player_new_from_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr CreateMediaPlayerFromMedia(IntPtr media);

    /// <summary>
    /// 递减媒体播放器对象的引用计数,如果它达到零,将会释放这个实例
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseMediaPlayer(IntPtr mediaPlayer);

    /// <summary>
    /// 递增媒体播放器对象的引用计数
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_retain")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RetainMediaPlayer(IntPtr mediaPlayer);

    /// <summary>
    /// 为媒体播放器设置媒体
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="media">媒体对象</param>
    [LibVlcFunction("libvlc_media_player_set_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetMedia(IntPtr mediaPlayer, IntPtr media);

    /// <summary>
    /// 获取媒体播放器的媒体
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>媒体对象</returns>
    [LibVlcFunction("libvlc_media_player_get_media")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetMedia(IntPtr mediaPlayer);

    /// <summary>
    /// 获取媒体播放器对象的事件管理器,该函数不会增加引用计数
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>返回媒体播放器对象的事件管理器</returns>
    [LibVlcFunction("libvlc_media_player_event_manager")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetEventManager(IntPtr mediaPlayer);

    /// <summary>
    /// 获取媒体播放器对象是否正在播放
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>如果播放器对象正在播放则返回 True ,否则返回 Flase</returns>
    [LibVlcFunction("libvlc_media_player_is_playing")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool IsPlaying(IntPtr mediaPlayer);

    /// <summary>
    /// 使媒体播放器开始播放
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>0代表成功,-1代表失败</returns>
    [LibVlcFunction("libvlc_media_player_play")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int Play(IntPtr mediaPlayer);

    /// <summary>
    /// 设置媒体播放器播放或者暂停,如果没有设置媒体对象,将会没有作用
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="pause">true 代表暂停,false 代表播放或继续</param>
    [LibVlcFunction("libvlc_media_player_set_pause")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetPause(IntPtr mediaPlayer, bool pause);

    /// <summary>
    /// 设置媒体播放器的进度,如果后台播放未启用将会没有作用,根据底层的输入格式和协议,可能导致无法正常播放
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="pos">播放进度,取值为0.0~1.0</param>
    [LibVlcFunction("libvlc_media_player_set_position")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetPosition(IntPtr mediaPlayer, float pos);

    /// <summary>
    /// 停止媒体播放器的播放,如果没有设置媒体将会没有作用
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_stop")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Stop(IntPtr mediaPlayer);

    /// <summary>
    /// 设置 Video 的事件回调
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="lockCallback">Lock 事件回调,必须</param>
    /// <param name="unlockCallback">Unlock 事件回调</param>
    /// <param name="displayCallback">Display 事件回调</param>
    /// <param name="userData">回调用用户数据</param>
    [LibVlcFunction("libvlc_video_set_callbacks","1.1.1")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetVideoCallback(IntPtr mediaPlayer, VideoLockCallback lockCallback, VideoUnlockCallback unlockCallback, VideoDisplayCallback displayCallback,IntPtr userData);

    /// <summary>
    /// 设置 Video 解码格式
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="chroma">视频格式识别码,一个四个字符的识别码</param>
    /// <param name="width">像素宽</param>
    /// <param name="height">像素高</param>
    /// <param name="pitch">扫描线</param>
    [LibVlcFunction("libvlc_video_set_format", "1.1.1")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet = CharSet.Ansi)]
    public delegate void SetVideoFormat(IntPtr mediaPlayer, String chroma, uint width, uint height, uint pitch);

    /// <summary>
    /// 设置 Video 解码格式回调
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="formatCallback"></param>
    /// <param name="cleanupCallback"></param>
    [LibVlcFunction("libvlc_video_set_format_callbacks", "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetVideoFormatCallback(IntPtr mediaPlayer, VideoFormatCallback formatCallback, VideoCleanupCallback cleanupCallback);

    /// <summary>
    /// 为媒体播放器设置一个视频输出句柄,将会在该句柄上绘图
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="hwnd">句柄</param>
    [LibVlcFunction("libvlc_media_player_set_hwnd")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetHwnd(IntPtr mediaPlayer, IntPtr hwnd);

    /// <summary>
    /// 获取为媒体播放器设置的视频输出句柄
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>句柄</returns>
    [LibVlcFunction("libvlc_media_player_set_hwnd")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetHwnd(IntPtr mediaPlayer);

    /// <summary>
    /// 设置 Audio 的事件回调
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="playCallback"></param>
    /// <param name="pauseCallback"></param>
    /// <param name="resumeCallback"></param>
    /// <param name="flushCallback"></param>
    /// <param name="drainCallback"></param>
    [LibVlcFunction("libvlc_audio_set_callbacks","2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetAudioCallback(IntPtr mediaPlayer, AudioPlayCallback playCallback, AudioPauseCallback pauseCallback, AudioResumeCallback resumeCallback, AudioFlushCallback flushCallback, AudioDrainCallback drainCallback);

    /// <summary>
    /// 设置 Audio 的格式
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="format">格式字符串,一个四字符的字符串</param>
    /// <param name="rate">采样率</param>
    /// <param name="channels">通道数</param>
    [LibVlcFunction("libvlc_audio_set_format", "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetAudioFormat(IntPtr mediaPlayer, uint format, uint rate, uint channels);

    /// <summary>
    /// 设置 Audio 的格式回调
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="setupCallback"></param>
    /// <param name="cheanupCallback"></param>
    [LibVlcFunction("libvlc_audio_set_format_callbacks", "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetAudioFormatCallback(IntPtr mediaPlayer, AudioSetupCallback setupCallback, AudioCheanupCallback cheanupCallback);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="volumeCallback"></param>
    [LibVlcFunction("libvlc_audio_set_volume_callback", "2.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetAudioVolumeCallback(IntPtr mediaPlayer, AudioSetVolumeCallback volumeCallback);

    /// <summary>
    /// 获取媒体的长度,以毫秒为单位
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>-1为未设置媒体</returns>
    [LibVlcFunction("libvlc_media_player_get_length")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Int64 GetLength(IntPtr mediaPlayer);

    /// <summary>
    /// 获取目前的媒体进度,以毫秒为单位
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>-1为未设置媒体</returns>
    [LibVlcFunction("libvlc_media_player_get_time")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Int64 GetTime(IntPtr mediaPlayer);

    /// <summary>
    /// 设置目前的媒体进度,以毫秒为单位
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="time">播放进度</param>
    [LibVlcFunction("libvlc_media_player_set_time")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetTime(IntPtr mediaPlayer, Int64 time);

    /// <summary>
    /// 获取当前媒体进度,0~1范围
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_get_position")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetPosition(IntPtr mediaPlayer);

    /// <summary>
    /// 设置当前媒体播放器的章节
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="chapter">章节</param>
    [LibVlcFunction("libvlc_media_player_set_chapter")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetChapter(IntPtr mediaPlayer, int chapter);

    /// <summary>
    /// 获取当前媒体播放器的章节
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>-1代表没有设置媒体</returns>
    [LibVlcFunction("libvlc_media_player_get_chapter")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetChapter(IntPtr mediaPlayer);

    /// <summary>
    /// 获取当前媒体播放器的章节数
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns>-1代表没有设置媒体</returns>
    [LibVlcFunction("libvlc_media_player_get_chapter_count")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetChapterCount(IntPtr mediaPlayer);

    /// <summary>
    /// 获取当前媒体播放器是否处于可播放
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_will_play")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool CanPlay(IntPtr mediaPlayer);

    /// <summary>
    /// 获取标题的章节数
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="title">标题</param>
    /// <returns>-1代表没有设置媒体</returns>
    [LibVlcFunction("libvlc_media_player_get_chapter_count_for_title")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetTitleChapterCount(IntPtr mediaPlayer, int title);

    /// <summary>
    /// 设置媒体播放器的标题
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="title"></param>
    [LibVlcFunction("libvlc_media_player_set_title")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetTitle(IntPtr mediaPlayer, int title);

    /// <summary>
    /// 获取媒体播放器的标题
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_get_title")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetTitle(IntPtr mediaPlayer);

    /// <summary>
    /// 获取媒体播放器的标题数
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_get_title_count")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetTitleCount(IntPtr mediaPlayer);

    /// <summary>
    /// 上一个章节
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_previous_chapter")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void PreviousChapter(IntPtr mediaPlayer);

    /// <summary>
    /// 下一个章节
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_next_chapter")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NextChapter(IntPtr mediaPlayer);

    /// <summary>
    /// 获取媒体速率
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_get_rate")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetRate(IntPtr mediaPlayer);

    /// <summary>
    /// 设置媒体是速率
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="rate"></param>
    [LibVlcFunction("libvlc_media_player_set_rate")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetRate(IntPtr mediaPlayer, float rate);

    /// <summary>
    /// 获取媒体的状态
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_get_state")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Media.MediaState GetState(IntPtr mediaPlayer);

    /// <summary>
    /// 获取媒体的FPS
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_get_fps")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate float GetFps(IntPtr mediaPlayer);

    /// <summary>
    /// 获取该媒体播放器视频输出的个数
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_has_vout")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint HasVout(IntPtr mediaPlayer);

    /// <summary>
    /// 获取该媒体是否能够跳进度
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_is_seekable")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool IsSeekable(IntPtr mediaPlayer);

    /// <summary>
    /// 获取该媒体是否能够暂停
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <returns></returns>
    [LibVlcFunction("libvlc_media_player_can_pause")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool CanPause(IntPtr mediaPlayer);

    /// <summary>
    /// 播放下一帧
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    [LibVlcFunction("libvlc_media_player_next_frame")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void NextFrame(IntPtr mediaPlayer);

    /// <summary>
    /// 导航DVD菜单
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="navigate"></param>
    [LibVlcFunction("libvlc_media_player_navigate","2.2.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Navigate(IntPtr mediaPlayer, NavigateMode navigate);


    /// <summary>
    /// 设置播放器播放视频时显示视频标题
    /// </summary>
    /// <param name="mediaPlayer">媒体播放器对象</param>
    /// <param name="pos"></param>
    /// <param name="timeout"></param>
    [LibVlcFunction("libvlc_media_player_navigate", "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetVideoTitleDisplay(IntPtr mediaPlayer, Position pos, uint timeout);

    /// <summary>
    /// 释放 TrackDescriptionList 资源
    /// </summary>
    /// <param name="track"></param>
    [LibVlcFunction("libvlc_track_description_list_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseTrackDescription(IntPtr track);
}
