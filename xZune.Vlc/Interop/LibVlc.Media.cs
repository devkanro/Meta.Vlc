using System;
using System.Runtime.InteropServices;

namespace xZune.Vlc.Interop.Media
{
    /// <summary>
    /// 向一个媒体添加一个选项,这个选项将会确定媒体播放器将如何读取介质,
    /// </summary>
    /// <param name="media">一个媒体指针</param>
    /// <param name="options"></param>
    [LibVlcFunction("libvlc_media_add_option")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl,CharSet = CharSet.Ansi)]
    public delegate void MediaAddOption(IntPtr media,IntPtr options);

    /// <summary>
    /// 向一个媒体通过可配置的标志添加一个选项,这个选项将会确定媒体播放器将如何读取介质,
    /// </summary>
    /// <param name="media">一个媒体指针</param>
    /// <param name="options"></param>
    /// <param name="flags"></param>
    [LibVlcFunction("libvlc_media_add_option_flag")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void MediaAddOptionFlag(IntPtr media, IntPtr options, MediaOption flags);

    /// <summary>
    /// 复制一个媒体对象
    /// </summary>
    /// <param name="media">要被复制的媒体对象</param>
    /// <returns>复制的媒体对象</returns>
    [LibVlcFunction("libvlc_media_duplicate")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr MediaDuplicate(IntPtr media);

    /// <summary>
    /// 获取媒体对象的事件管理器,该函数不会增加引用计数
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>返回媒体对象的事件管理器</returns>
    [LibVlcFunction("libvlc_media_event_manager")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetEventManager(IntPtr media);

    /// <summary>
    /// 获取媒体的基本编码器的说明
    /// </summary>
    /// <param name="type">由 <see cref="MediaTrack.Type"/> 得来</param>
    /// <param name="codec">由 <see cref="MediaTrack.Codec"/> 得来</param>
    /// <returns>返回媒体的基本编码器的说明</returns>
    [LibVlcFunction("libvlc_media_get_codec_description","3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr GetCodecDescription(TrackType type, int codec);

    /// <summary>
    /// 获取媒体的时间长度
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>返回媒体的时间长度</returns>
    [LibVlcFunction("libvlc_media_get_duration")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Int64 GetDuration(IntPtr media);

    /// <summary>
    /// 获取媒体的某个元属性,如果尚未解析元属性,将会返回 NULL.
    /// 这个方法会自动调用 <see cref="ParseMediaAsync"/> 方法,所以你在之后应该会收到一个 MediaMetaChanged 事件.
    /// 如果你喜欢同步版本,可以在 GetMeta 之前调用 <see cref="ParseMedia"/> 方法
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <param name="type">元属性类型</param>
    /// <returns>返回媒体的某个元属性</returns>
    [LibVlcFunction("libvlc_media_get_meta")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr GetMeta(IntPtr media, MetaDataType type);

    /// <summary>
    /// 获取该媒体的媒体资源地址
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>返回该媒体的媒体资源地址</returns>
    [LibVlcFunction("libvlc_media_get_mrl")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr GetMrl(IntPtr media);

    /// <summary>
    /// 获取媒体当前状态
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>返回媒体当前状态</returns>
    [LibVlcFunction("libvlc_media_get_state")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate MediaState GetState(IntPtr media);

    /// <summary>
    /// 获取媒体当前统计
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <param name="stats">统计结构体指针,指向 <seealso cref="MediaStats"/></param>
    /// <returns>如果成功会返回 true ,否则会返回 false</returns>
    [LibVlcFunction("libvlc_media_get_stats")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool GetStats(IntPtr media, ref MediaStats stats);

    /// <summary>
    /// 获取媒体的基本流的描述,注意,在调用该方法之前你需要首先调用 <see cref="ParseMedia"/> 方法,或者至少播放一次.
    /// 否则,你将的得到一个空数组
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <param name="tracks">一个 <see cref="MediaTrackInfo"/> 数组</param>
    /// <returns>数组的元素个数</returns>
    [LibVlcFunction("libvlc_media_get_tracks_info")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int GetTracksInfo(IntPtr media, out IntPtr tracks);

    /// <summary>
    /// 获取由用户定义的媒体数据
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>返回由用户定义的媒体数据指针</returns>
    [LibVlcFunction("libvlc_media_get_user_data")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetUserData(IntPtr media);

    /// <summary>
    /// 获取一个值表示该媒体是否已经解析
    /// </summary>
    /// <param name="media">LibVlc 实例指针</param>
    /// <returns>True 表示已经解析,False 表示尚未被解析</returns>
    [LibVlcFunction("libvlc_media_is_parsed")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool IsParsed(IntPtr media);

    /// <summary>
    /// 创建一个具有名字的媒体作为一个空节点
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="name">名字</param>
    /// <returns>创建的媒体对象指针</returns>
    [LibVlcFunction("libvlc_media_new_as_node")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr CreateMediaAsNewNode(IntPtr instance, IntPtr name);

    /// <summary>
    /// 通过给定的文件描述符创建一个媒体,该文件描述符必须具有 Read 访问权限.
    /// LibVlc 不会关闭任何文件描述符,尽管如此,一般一个媒体描述符只能在媒体播放器中使用一次,如果你想复用,需要使用 lseek 函数将文件描述符的文件指针倒回开头
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="fileDescriptor">文件描述符</param>
    /// <returns>创建的媒体对象指针,发送错误时会返回 NULL</returns>
    [LibVlcFunction("libvlc_media_new_fd","1.1.5")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr CreateMediaFormFileDescriptor(IntPtr instance, int fileDescriptor);

    /// <summary>
    /// 通过给定的文件 Url 创建一个媒体,该 Url 的格式必须以 "file://" 开头,参见 "RFC3986".
    /// 对于打开本地媒体,其实我们更推荐使用 <see cref="CreateMediaFormPath"/>
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="url">媒体的文件 Url</param>
    /// <returns>创建的媒体对象指针,发送错误时会返回 NULL</returns>
    [LibVlcFunction("libvlc_media_new_location")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate IntPtr CreateMediaFormLocation(IntPtr instance, IntPtr url);

    /// <summary>
    /// 通过给定的文件路径创建一个媒体
    /// </summary>
    /// <param name="instance">LibVlc 实例指针</param>
    /// <param name="path">媒体文件路径</param>
    /// <returns>创建的媒体对象指针,发送错误时会返回 NULL</returns>
    [LibVlcFunction("libvlc_media_new_path")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Auto)]
    public delegate IntPtr CreateMediaFormPath(IntPtr instance, IntPtr path);

    /// <summary>
    /// 解析一个媒体,获取媒体的元数据和轨道信息
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    [LibVlcFunction("libvlc_media_parse")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ParseMedia(IntPtr media);

    /// <summary>
    /// 异步解析一个媒体,获取媒体的元数据和轨道信息,这是 <see cref="ParseMedia"/> 的异步版本,
    /// 解析完成会触发 MediaParsedChanged 事件,您可以跟踪该事件
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    [LibVlcFunction("libvlc_media_parse_async")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ParseMediaAsync(IntPtr media);

    /// <summary>
    /// 根据提供的标志异步解析一个媒体,获取媒体的元数据和轨道信息,这是 <see cref="ParseMediaAsync"/> 的高级版本,
    /// 默认情况下解析一个本地文件,解析完成会触发 MediaParsedChanged 事件,您可以跟踪该事件
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <param name="flag">提供的解析标志</param>
    /// <returns>成功解析会返回 0,否则会返回 -1</returns>
    [LibVlcFunction("libvlc_media_parse_with_options", "3.0.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int ParseMediaWithOptionAsync(IntPtr media, MediaParseFlag flag);

    /// <summary>
    /// 递减媒体对象的引用计数,如果它达到零,将会释放这个实例
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    [LibVlcFunction("libvlc_media_release")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseMedia(IntPtr media);

    /// <summary>
    /// 递增媒体对象的引用计数
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    [LibVlcFunction("libvlc_media_retain")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void RetainMedia(IntPtr media);

    /// <summary>
    /// 保存当前的元数据到媒体
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>如果操作成功将会返回 True</returns>
    [LibVlcFunction("libvlc_media_save_meta")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate bool SaveMeta(IntPtr media);

    /// <summary>
    /// 设置媒体的元数据
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <param name="type">元数据类型</param>
    /// <param name="data">元数据值</param>
    [LibVlcFunction("libvlc_media_set_meta")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public delegate void SetMeta(IntPtr media, MetaDataType type, IntPtr data);

    /// <summary>
    /// 设置媒体的由用户定义的数据
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <param name="userData">用户定义的数据</param>
    [LibVlcFunction("libvlc_media_set_user_data")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void SetUserData(IntPtr media, IntPtr userData);

    /// <summary>
    /// 获取媒体对象的子对象列表,这将增加引用计数,使用 <see cref="Interop.MediaList.ReleaseMediaList"/> 来减少引用计数
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <returns>返回媒体对象的子对象列表</returns>
    [LibVlcFunction("libvlc_media_subitems")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetSubitems(IntPtr media);

    /// <summary>    
    /// 获取媒体的基本流的描述,注意,在调用该方法之前你需要首先调用 <see cref="ParseMedia"/> 方法,或者至少播放一次.
    /// 否则,你将的得到一个空数组
    /// </summary>
    /// <param name="media">媒体对象指针</param>
    /// <param name="tracks">一个 <see cref="MediaTrack"/> 数组的数组</param>
    /// <returns>返回媒体的基本流的描述</returns>
    [LibVlcFunction("libvlc_media_tracks_get", "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate uint GetTracks(IntPtr media, ref IntPtr tracks);

    /// <summary>
    /// 释放一个媒体的基本流的描述的数组
    /// </summary>
    /// <param name="tracks">基本流的描述的数组</param>
    [LibVlcFunction("libvlc_media_tracks_release", "2.1.0")]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void ReleaseTracks(IntPtr tracks, uint count);


    [StructLayout(LayoutKind.Sequential)]
    public struct MediaStats
    {
        public int ReadBytes;
        public float InputBitrate;
        public int DemuxReadBytes;
        public float DemuxBitrate;
        public int DemuxCorrupted;
        public int DemuxDiscontinuity;
        public int DecodedVideo;
        public int DecodedAudio;
        public int DisplayedPictures;
        public int LostPictures;
        public int PlayedBbuffers;
        public int LostAbuffers;
        public int SentPackets;
        public int SentBytes;
        public float SendBitrate;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct MediaTrackInfo
    {
        public UInt32 Codec;
        public int Id;
        public TrackType Type;
        public int Profile;
        public int Level;
        /// <summary>
        /// 表示音频的通道数或者视频的帧高
        /// </summary>
        public uint ChannelsOrHeight;
        /// <summary>
        /// 表示音频的速率或者视频的帧宽
        /// </summary>
        public uint RateOrWidth;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct AudioTrack
    {
        /// <summary>
        /// 表示音频的通道数
        /// </summary>
        public uint Channels;
        /// <summary>
        /// 表示音频的速率
        /// </summary>
        public uint Rate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VideoTrack
    {
        /// <summary>
        /// 表示视频的帧高
        /// </summary>
        public uint Height;
        /// <summary>
        /// 表示视频的帧宽
        /// </summary>
        public uint Width;
        public uint SarNum;
        public uint SarDen;
        public uint FrameRateNum;
        public uint FrameRateDen;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct SubtitleTrack
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public String Encoding;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MediaTrack
    {
        public uint Codec;
        public uint OriginalFourcc;
        public int Id;
        public TrackType Type;
        public int Profile;
        public int Level;
        /// <summary>
        /// 表示一个 Track 的具体指针,该指针可能指向 <see cref="VideoTrack"/>,<see cref="AudioTrack"/> 或者 <see cref="SubtitleTrack"/>,根据 Type 的值不同,Track 的指向数据也可能不同
        /// </summary>
        public IntPtr Track;
        public uint Bitrate;
        [MarshalAs(UnmanagedType.LPStr)]
        public String Language;
        [MarshalAs(UnmanagedType.LPStr)]
        public String Description;
    }

    public enum TrackType
    {
        Unkown = -1,
        Audio = 0,
        Video = 1,
        Text = 2
    }

    public enum MediaState
    {
        NothingSpecial = 0,
        Opening,
        Buffering,
        Playing,
        Paused,
        Stopped,
        Ended,
        Error
    }

    public enum MediaOption
    {
        Trusted = 0x2,
        Unique = 0x100
    }

    [Flags]
    public enum MediaParseFlag
    {
        ParseLocal = 0x00,
        ParseNetwork = 0x01,
        FetchLocal = 0x02,
        FetchNetwork = 0x04
    }

    public enum MetaDataType
    {
        Title,
        Artist,
        Genre,
        Copyright,
        Album,
        TrackNumber,
        Description,
        Rating,
        Date,
        Setting,
        Url,
        Language,
        NowPlaying,
        Publisher,
        EncodedBy,
        ArtworkUrl,
        TrackID,
        TrackTotal,
        Director,
        Season,
        Episode,
        ShowName,
        Actors,
        AlbumArtist,
        DiscNumber,
    }
}
