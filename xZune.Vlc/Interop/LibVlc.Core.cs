using System;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop.Core
{
    /// <summary>
    /// 尝试启动一个用户接口,用于 LibVlc 实例
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="name">接口名,为 NULL 则为默认</param>
    /// <returns>如果成功会返回 0 ,否则会返回 -1</returns>
    [LibVlcFunction("libvlc_new")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate int AddInterface(IntPtr instance, IntPtr name);

    /// <summary>
    /// 获取可用的音频过滤器
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <returns>可用音频过滤器列表指针,这是一个 <see cref="ModuleDescription"/> 类型的指针</returns>
    [LibVlcFunction("libvlc_audio_filter_list_get")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetAudioFilterList(IntPtr instance);

    /// <summary>
    /// 释放由 LibVlc 函数返回的指针资源,其作用类似于 C语言 中的 free() 函数
    /// </summary>
    /// <param name="pointer">指针</param>
    [LibVlcFunction("libvlc_free")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Free(IntPtr pointer);

    /// <summary>
    /// 获取 LibVlc 的变更集(?)
    /// </summary>
    /// <returns>返回 LibVlc 的变更集,类似于 "aa9bce0bc4"</returns>
    [LibVlcFunction("libvlc_get_changeset")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr GetChangeset();

    /// <summary>
    /// 获取 LibVlc 的编译器信息
    /// </summary>
    /// <returns>返回 LibVlc 的编译器信息</returns>
    [LibVlcFunction("libvlc_get_compiler")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr GetCompiler();

    /// <summary>
    /// 获取 LibVlc 的版本信息
    /// </summary>
    /// <returns>返回 LibVlc 的版本信息,类似于 "1.1.0-git The Luggage"</returns>
    [LibVlcFunction("libvlc_get_version")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr GetVersion();

    /// <summary>
    /// 释放 <see cref="ModuleDescription"/> 的资源
    /// </summary>
    /// <param name="moduleDescription">资源指针</param>
    [LibVlcFunction("libvlc_module_description_list_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseLibVlcModuleDescription(IntPtr moduleDescription);

    /// <summary>
    /// 创建并初始化一个 LibVlc 实例,并提供相应的参数,这些参数和命令行提供的参数类似,会影响到 LibVlc 实例的默认配置.
    /// 有效参数的列表取决于 LibVlc 版本,操作系统,可用 LibVlc 插件和平台.无效或不支持的参数会导致实例创建失败
    /// </summary>
    /// <param name="argsCount">参数个数</param>
    /// <param name="argv">参数列表</param>
    /// <returns>返回 LibVlc 实例指针,如果出错将返回 NULL</returns>
    [LibVlcFunction("libvlc_new")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr NewInstance(int argsCount, IntPtr argv);

    /// <summary>
    /// 递减 LibVlc 实例的引用计数,如果它达到零,将会释放这个实例
    /// </summary>
    /// <param name="instance">需要释放的 LibVlc 实例指针</param>
    [LibVlcFunction("libvlc_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseInstance(IntPtr instance);

    /// <summary>
    /// 递增 LibVlc 实例的引用计数,当调用 NewInstance 初始化成功时,引用计数将初始化为1
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    [LibVlcFunction("libvlc_retain")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RetainInstance(IntPtr instance);

    /// <summary>
    /// 设置一些元信息关于该应用程序
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="id">Java 风格的应用标识符,类似于 "com.acme.foobar"</param>
    /// <param name="version">应用程序版本,类似于 "1.2.3"</param>
    /// <param name="icon">应用程序图标,类似于 "foobar"</param>
    [LibVlcFunction("libvlc_set_app_id","2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void SetAppId(IntPtr instance, IntPtr id, IntPtr version, IntPtr icon);

    /// <summary>
    /// 为 LibVlc 设置一个回调,该回调将会在 LibVlc 退出时被调用,不能与 <see cref="Wait"/> 一起使用.
    /// 而且,这个函数应该在播放一个列表或者开始一个用户接口前被调用,否则可能导致 LibVlc 在注册该回调前退出
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="handler">函数指针,这是一个参数为 void*,无返回值的函数指针</param>
    /// <param name="arg">数据指针,将做为参数传递给回调函数</param>
    [LibVlcFunction("libvlc_set_exit_handler")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetExitHandler(IntPtr instance, ExitHandler handler, IntPtr arg);

    public delegate void ExitHandler(IntPtr data);

    /// <summary>
    /// 设置一个用户代理字符串,当一个协议需要它的时候,LibVlc 将会提供该字符串
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="name">应用程序名称,类似于 "FooBar player 1.2.3",实际上只要能标识应用程序,任何字符串都是可以的</param>
    /// <param name="http">HTTP 用户代理,类似于 "FooBar/1.2.3 Python/2.6.0"</param>
    [LibVlcFunction("libvlc_set_user_agent","2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void SetUserAgent(IntPtr instance, IntPtr name, IntPtr http);

    /// <summary>
    /// 获取可用的视频过滤器
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <returns>可用视频过滤器列表指针,这是一个 <see cref="ModuleDescription"/> 类型的指针</returns>
    [LibVlcFunction("libvlc_video_filter_list_get")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetVideoFilterList(IntPtr instance);

    /// <summary>
    /// 等待,直到一个接口导致 LibVlc 实例退出为止,在使用之前,应该使用 <see cref="AddInterface"/> 添加至少一个用户接口.
    /// 实际上这个方法只会导致一个线程阻塞,建议使用 <see cref="SetExitHandler"/>
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    [LibVlcFunction("libvlc_wait")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Wait(IntPtr instance);


    /// <summary>
    /// 对一个 LibVlc 的模块的说明
    /// </summary>
    [StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi)]
    public struct ModuleDescription
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public String Name;
        /// <summary>
        /// 短名称
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public String ShortName;
        /// <summary>
        /// 长名称
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public String LongName;
        /// <summary>
        /// 说明
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public String Help;
        /// <summary>
        /// 下一个模块,这是一个 <see cref="ModuleDescription"/> 指针
        /// </summary>
        public IntPtr Next;
    }

    namespace Error
    {
        /// <summary>
        /// 获取一个可读的 LibVlc 错误信息
        /// </summary>
        /// <returns>返回一个可读的 LibVlc 错误信息,如果没有错误信息将返回 NULL</returns>
        [LibVlcFunction("libvlc_errmsg")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate IntPtr ErrorMessage();

        /// <summary>
        /// 清除当前线程的 LibVlc 的错误信息
        /// </summary>
        [LibVlcFunction("libvlc_clearerr")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CleanError();
    }

    namespace Events
    {
        /// <summary>
        /// 为一个事件通知注册一个回调
        /// </summary>
        /// <param name="manager">事件管理器</param>
        /// <param name="type">事件类型</param>
        /// <param name="callback">回调</param>
        /// <param name="userData">由用户定义的数据</param>
        /// <returns>0代表成功,12代表出错</returns>
        [LibVlcFunction("libvlc_event_attach")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int EventAttach(IntPtr manager, EventTypes type, LibVlcEventCallBack callback, IntPtr userData);

        /// <summary>
        /// 为一个事件通知取消注册一个回调
        /// </summary>
        /// <param name="manager">事件管理器</param>
        /// <param name="type">事件类型</param>
        /// <param name="callback">回调</param>
        /// <param name="userData">由用户定义的数据</param>
        [LibVlcFunction("libvlc_event_detach")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EventDetach(IntPtr manager, EventTypes type, LibVlcEventCallBack callback, IntPtr userData);

        /// <summary>
        /// 获取事件类型名称
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <returns>返回事件类型名称</returns>
        [LibVlcFunction("libvlc_event_type_name")]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet = CharSet.Ansi)]
        public delegate IntPtr GetTypeName(EventTypes type);

        /// <summary>
        /// 表示一个 LibVlc 的事件回调代理
        /// </summary>
        /// <param name="eventArgs">事件参数</param>
        /// <param name="userData">用户数据指针</param>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LibVlcEventCallBack(ref LibVlcEventArgs eventArgs, IntPtr userData);

        /// <summary>
        /// 事件类型
        /// </summary>
        public enum EventTypes : uint
        {
            /// <summary>
            /// 媒体元数据改变
            /// </summary>
            MediaMetaChanged = 0,
            /// <summary>
            /// 媒体的子项被添加
            /// </summary>
            MediaSubItemAdded,
            /// <summary>
            /// 媒体时长改变
            /// </summary>
            MediaDurationChanged,
            /// <summary>
            /// 媒体解析状态被改变
            /// </summary>
            MediaParsedChanged,
            /// <summary>
            /// 媒体被释放
            /// </summary>
            MediaFreed,
            /// <summary>
            /// 媒体状态改变
            /// </summary>
            MediaStateChanged,

            /// <summary>
            /// 媒体播放器的媒体被改变
            /// </summary>
            MediaPlayerMediaChanged = 0x100,
            MediaPlayerNothingSpecial,
            /// <summary>
            /// 媒体播放器正在打开媒体
            /// </summary>
            MediaPlayerOpening,
            /// <summary>
            /// 媒体播放器正在缓冲媒体
            /// </summary>
            MediaPlayerBuffering,
            /// <summary>
            /// 媒体播放器正在播放
            /// </summary>
            MediaPlayerPlaying,
            /// <summary>
            /// 媒体播放器被暂停
            /// </summary>
            MediaPlayerPaused,
            /// <summary>
            /// 媒体播放器被停止播放
            /// </summary>
            MediaPlayerStopped,
            /// <summary>
            /// 媒体播放器前进
            /// </summary>
            MediaPlayerForward,
            /// <summary>
            /// 媒体播放器后退
            /// </summary>
            MediaPlayerBackward,
            /// <summary>
            /// 媒体播放器结束播放
            /// </summary>
            MediaPlayerEndReached,
            /// <summary>
            /// 媒体播放器遇到错误
            /// </summary>
            MediaPlayerEncounteredError,
            /// <summary>
            /// 媒体播放器时间改变
            /// </summary>
            MediaPlayerTimeChanged,
            /// <summary>
            /// 媒体播放器进度改变
            /// </summary>
            MediaPlayerPositionChanged,
            /// <summary>
            /// 媒体播放器是否允许寻址被改变
            /// </summary>
            MediaPlayerSeekableChanged,
            /// <summary>
            /// 媒体播放器是否允许被暂停被改变
            /// </summary>
            MediaPlayerPausableChanged,
            /// <summary>
            /// 媒体播放器标题被改变
            /// </summary>
            MediaPlayerTitleChanged,
            /// <summary>
            /// 媒体播放器捕获一个快照
            /// </summary>
            MediaPlayerSnapshotTaken,
            /// <summary>
            /// 媒体播放器长度改变
            /// </summary>
            MediaPlayerLengthChanged,
            /// <summary>
            /// 媒体播放器视频输出改变
            /// </summary>
            MediaPlayerVideoOutChanged,

            /// <summary>
            /// 一个项被添加到媒体列表
            /// </summary>
            MediaListItemAdded = 0x200,
            /// <summary>
            /// 一个项将被添加到媒体列表
            /// </summary>
            MediaListWillAddItem,
            /// <summary>
            /// 一个项从媒体列表移除
            /// </summary>
            MediaListItemDeleted,
            /// <summary>
            /// 一个项将从媒体列表移除
            /// </summary>
            MediaListWillDeleteItem,

            /// <summary>
            /// 一个项被添加到媒体列表视图
            /// </summary>
            MediaListViewItemAdded = 0x300,
            /// <summary>
            /// 一个项将被添加到媒体列表视图
            /// </summary>
            MediaListViewWillAddItem,
            /// <summary>
            /// 一个项从媒体列表视图移除
            /// </summary>
            MediaListViewItemDeleted,
            /// <summary>
            /// 一个项将从媒体列表视图移除
            /// </summary>
            MediaListViewWillDeleteItem,

            /// <summary>
            /// 媒体列表播放器开始播放
            /// </summary>
            MediaListPlayerPlayed = 0x400,
            /// <summary>
            /// 媒体列表播放器跳到下个项
            /// </summary>
            MediaListPlayerNextItemSet,
            /// <summary>
            /// 媒体列表播放器停止
            /// </summary>
            MediaListPlayerStopped,

            /// <summary>
            /// 媒体搜寻器开始搜寻
            /// </summary>
            MediaDiscovererStarted = 0x500,
            /// <summary>
            /// 媒体搜寻器搜寻结束
            /// </summary>
            MediaDiscovererEnded,

            /// <summary>
            /// 一个 VLM 媒体被添加
            /// </summary>
            VlmMediaAdded = 0x600,
            /// <summary>
            /// 一个 VLM 媒体被移除
            /// </summary>
            VlmMediaRemoved,
            /// <summary>
            /// 一个 VLM 媒体被改变
            /// </summary>
            VlmMediaChanged,
            /// <summary>
            /// 一个 VLM 媒体实例开始
            /// </summary>
            VlmMediaInstanceStarted,
            /// <summary>
            /// 一个 VLM 媒体实例停止
            /// </summary>
            VlmMediaInstanceStopped,
            /// <summary>
            /// 一个 VLM 媒体实例被初始化
            /// </summary>
            VlmMediaInstanceStatusInit,
            /// <summary>
            /// 一个 VLM 媒体实例正在打开
            /// </summary>
            VlmMediaInstanceStatusOpening,
            /// <summary>
            /// 一个 VLM 媒体实例正在播放
            /// </summary>
            VlmMediaInstanceStatusPlaying,
            /// <summary>
            /// 一个 VLM 媒体实例被暂停
            /// </summary>
            VlmMediaInstanceStatusPause,
            /// <summary>
            /// 一个 VLM 媒体实例结束播放
            /// </summary>
            VlmMediaInstanceStatusEnd,
            /// <summary>
            /// 一个 VLM 媒体实例出现错误
            /// </summary>
            VlmMediaInstanceStatusError
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct LibVlcEventArgs
        {
            [FieldOffset(0)]
            public EventTypes Type;

            [FieldOffset(4)]
            public IntPtr ObjectHandle;

            #region media descriptor

            [FieldOffset(8)]
            public MediaMetaChangedArgs MediaMetaChanged;

            [FieldOffset(8)]
            public MediaSubitemAddedArgs MediaSubitemAdded;

            [FieldOffset(8)]
            public MediaDurationChangedArgs MediaDurationChanged;

            [FieldOffset(8)]
            public MediaParsedChangedArgs MediaParsedChanged;

            [FieldOffset(8)]
            public MediaFreedArgs MediaFreed;

            [FieldOffset(8)]
            public MediaStateChangedArgs MediaStateChanged;

            #endregion

            #region media instance

            [FieldOffset(8)]
            public MediaPlayerBufferingArgs MediaPlayerBuffering;

            [FieldOffset(8)]
            public MediaPlayerPositionChangedArgs MediaPlayerPositionChanged;

            [FieldOffset(8)]
            public MediaPlayerTimeChangedArgs MediaPlayerTimeChanged;

            [FieldOffset(8)]
            public MediaPlayerTitleChangedArgs MediaPlayerTitleChanged;

            [FieldOffset(8)]
            public MediaPlayerSeekableChangedArgs MediaPlayerSeekableChanged;

            [FieldOffset(8)]
            public MediaPlayerPausableChangedArgs MediaPlayerPausableChanged;

            [FieldOffset(8)]
            public MediaPlayerVideoOutChangedArgs MediaPlayerVideoOutChanged;

            #endregion

            #region media list

            [FieldOffset(8)]
            public MediaListItemAddedArgs MediaListItemAdded;

            [FieldOffset(8)]
            public MediaListWillAddItemArgs MediaListWillAddItem;

            [FieldOffset(8)]
            public MediaListItemDeletedArgs MediaListItemDeleted;

            [FieldOffset(8)]
            public MediaListWillDeleteItemArgs MediaListWillDeleteItem;

            #endregion

            #region media list player

            [FieldOffset(8)]
            public MediaListPlayerNextItemSetArgs MediaListPlayerNextItemSet;

            #endregion

            #region snapshot taken

            [FieldOffset(8)]
            public MediaPlayerSnapshotTakenArgs MediaPlayerSnapshotTaken;

            #endregion

            #region Length changed

            [FieldOffset(8)]
            public MediaPlayerLengthChangedArgs MediaPlayerLengthChanged;

            #endregion

            #region VLM media

            [FieldOffset(8)]
            public VlmMediaEventArgs VlmMediaEvent;

            #endregion

            #region Extra MediaPlayer

            [FieldOffset(8)]
            public MediaPlayerMediaChangedArgs MediaPlayerMediaChanged;

            #endregion

            
        }

        #region media descriptor

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaMetaChangedArgs
        {
            public Media.MetaDataType MetaType;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaSubitemAddedArgs
        {
            public IntPtr NewChild;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaDurationChangedArgs
        {
            public long NewDuration;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaParsedChangedArgs
        {
            public int NewStatus;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaFreedArgs
        {
            public IntPtr MediaHandler;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaStateChangedArgs
        {
            public Media.MediaState NewState;
        }

        #endregion

        #region media instance

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerBufferingArgs
        {
            public float NewCache;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerPositionChangedArgs
        {
            public float NewPosition;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerTimeChangedArgs
        {
            public long NewTime;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerTitleChangedArgs
        {
            public int NewTitle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerSeekableChangedArgs
        {
            public int NewSeekable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerPausableChangedArgs
        {
            public int NewPausable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerVideoOutChangedArgs
        {
            public int NewCount;
        }

        #endregion

        #region media list

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListItemAddedArgs
        {
            public IntPtr ItemHandle;
            public int Index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListWillAddItemArgs
        {
            public IntPtr ItemHandle;
            public int Index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListItemDeletedArgs
        {
            public IntPtr ItemHandle;
            public int Index;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListWillDeleteItemArgs
        {
            public IntPtr ItemHandle;
            public int Index;
        }

        #endregion

        #region  media list player

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaListPlayerNextItemSetArgs
        {
            public IntPtr ItemHandle;
        }

        #endregion

        #region snapshot taken

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerSnapshotTakenArgs
        {
            public IntPtr pszFilename;
        }

        #endregion

        #region Length changed

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerLengthChangedArgs
        {
            public long NewLength;
        }

        #endregion

        #region  VLM media

        [StructLayout(LayoutKind.Sequential)]
        public struct VlmMediaEventArgs
        {
            public IntPtr pszMediaName;
            public IntPtr pszInstanceName;
        }

        #endregion

        #region  Extra MediaPlayer

        [StructLayout(LayoutKind.Sequential)]
        public struct MediaPlayerMediaChangedArgs
        {
            public IntPtr NewMediaHandle;
        }

        #endregion
    }
}
