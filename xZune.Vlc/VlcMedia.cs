using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Media;

namespace xZune.Vlc
{
    public class VlcMedia : IVlcObject
    {
        static VlcMedia()
        {
            IsLibLoaded = false;
        }

        /// <summary>
        /// 载入 LibVlc 的 Media 模块,该方法会在 <see cref="Vlc.LoadLibVlc"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        public static void LoadLibVlc(IntPtr libHandle,Version libVersion, String devString)
        {
            if(!IsLibLoaded)
            {
                AddOptionFunction = new LibVlcFunction<MediaAddOption>(libHandle, libVersion, devString);
                AddOptionFlagFunction = new LibVlcFunction<MediaAddOptionFlag>(libHandle, libVersion, devString);
                DuplicateFunction = new LibVlcFunction<MediaDuplicate>(libHandle, libVersion, devString);
                GetEventManagerFunction = new LibVlcFunction<GetEventManager>(libHandle, libVersion, devString);
                GetCodecDescriptionFunction = new LibVlcFunction<GetCodecDescription>(libHandle, libVersion, devString);
                GetDurationFunction = new LibVlcFunction<GetDuration>(libHandle, libVersion, devString);
                GetMetaFunction = new LibVlcFunction<GetMeta>(libHandle, libVersion, devString);
                GetMrlFunction = new LibVlcFunction<GetMrl>(libHandle, libVersion, devString);
                GetStateFunction = new LibVlcFunction<GetState>(libHandle, libVersion, devString);
                GetStatsFunction = new LibVlcFunction<GetStats>(libHandle, libVersion, devString);
                GetTracksInfoFunction = new LibVlcFunction<GetTracksInfo>(libHandle, libVersion, devString);
                GetUserDataFunction = new LibVlcFunction<GetUserData>(libHandle, libVersion, devString);
                IsParsedFunction = new LibVlcFunction<IsParsed>(libHandle, libVersion, devString);
                CreateMediaAsNewNodeFunction = new LibVlcFunction<CreateMediaAsNewNode>(libHandle, libVersion, devString);
                CreateMediaFormFileDescriptorFunction = new LibVlcFunction<CreateMediaFormFileDescriptor>(libHandle, libVersion, devString);
                CreateMediaFormLocationFunction = new LibVlcFunction<CreateMediaFormLocation>(libHandle, libVersion, devString);
                CreateMediaFormPathFunction = new LibVlcFunction<CreateMediaFormPath>(libHandle, libVersion, devString);
                ParseMediaFunction = new LibVlcFunction<ParseMedia>(libHandle, libVersion, devString);
                ParseMediaAsyncFunction = new LibVlcFunction<ParseMediaAsync>(libHandle, libVersion, devString);
                ParseMediaWithOptionAsyncFunction = new LibVlcFunction<ParseMediaWithOptionAsync>(libHandle, libVersion, devString);
                ReleaseMediaFunction = new LibVlcFunction<ReleaseMedia>(libHandle, libVersion, devString);
                RetainMediaFunction = new LibVlcFunction<RetainMedia>(libHandle, libVersion, devString);
                SaveMetaFunction = new LibVlcFunction<SaveMeta>(libHandle, libVersion, devString);
                SetMetaFunction = new LibVlcFunction<SetMeta>(libHandle, libVersion, devString);
                SetUserDataFunction = new LibVlcFunction<SetUserData>(libHandle, libVersion, devString);
                GetSubitemsFunction = new LibVlcFunction<GetSubitems>(libHandle, libVersion, devString);
                GetTracksFunction = new LibVlcFunction<GetTracks>(libHandle, libVersion, devString);
                ReleaseTracksFunction = new LibVlcFunction<ReleaseTracks>(libHandle, libVersion, devString);
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

        private static LibVlcFunction<MediaAddOption> AddOptionFunction;
        private static LibVlcFunction<MediaAddOptionFlag> AddOptionFlagFunction;
        private static LibVlcFunction<MediaDuplicate> DuplicateFunction;
        private static LibVlcFunction<GetEventManager> GetEventManagerFunction;
        private static LibVlcFunction<GetCodecDescription> GetCodecDescriptionFunction;
        private static LibVlcFunction<GetDuration> GetDurationFunction;
        private static LibVlcFunction<GetMeta> GetMetaFunction;
        private static LibVlcFunction<GetMrl> GetMrlFunction;
        private static LibVlcFunction<GetState> GetStateFunction;
        private static LibVlcFunction<GetStats> GetStatsFunction;
        private static LibVlcFunction<GetTracksInfo> GetTracksInfoFunction;
        private static LibVlcFunction<GetUserData> GetUserDataFunction;
        private static LibVlcFunction<IsParsed> IsParsedFunction;
        private static LibVlcFunction<CreateMediaAsNewNode> CreateMediaAsNewNodeFunction;
        private static LibVlcFunction<CreateMediaFormFileDescriptor> CreateMediaFormFileDescriptorFunction;
        private static LibVlcFunction<CreateMediaFormLocation> CreateMediaFormLocationFunction;
        private static LibVlcFunction<CreateMediaFormPath> CreateMediaFormPathFunction;
        private static LibVlcFunction<ParseMedia> ParseMediaFunction;
        private static LibVlcFunction<ParseMediaAsync> ParseMediaAsyncFunction;
        private static LibVlcFunction<ParseMediaWithOptionAsync> ParseMediaWithOptionAsyncFunction;
        private static LibVlcFunction<ReleaseMedia> ReleaseMediaFunction;
        private static LibVlcFunction<RetainMedia> RetainMediaFunction;
        private static LibVlcFunction<SaveMeta> SaveMetaFunction;
        private static LibVlcFunction<SetMeta> SetMetaFunction;
        private static LibVlcFunction<SetUserData> SetUserDataFunction;
        private static LibVlcFunction<GetSubitems> GetSubitemsFunction;
        private static LibVlcFunction<GetTracks> GetTracksFunction;
        private static LibVlcFunction<ReleaseTracks> ReleaseTracksFunction;

        Interop.Core.Events.LibVlcEventCallBack onMetaChanged;
        Interop.Core.Events.LibVlcEventCallBack onSubItemAdded;
        Interop.Core.Events.LibVlcEventCallBack onDurationChanged;
        Interop.Core.Events.LibVlcEventCallBack onParsedChanged;
        Interop.Core.Events.LibVlcEventCallBack onFreed;
        Interop.Core.Events.LibVlcEventCallBack onStateChanged;

        GCHandle onMetaChangedHandle;
        GCHandle onSubItemAddedHandle;
        GCHandle onDurationChangedHandle;
        GCHandle onParsedChangedHandle;
        GCHandle onFreedHandle;
        GCHandle onStateChangedHandle;

        private VlcMedia(IntPtr pointer)
        {
            InstancePointer = pointer;
            EventManager = new VlcEventManager(GetEventManagerFunction.Delegate(InstancePointer));

            onMetaChanged = OnMetaChanged;
            onSubItemAdded = OnSubItemAdded;
            onDurationChanged = OnDurationChanged;
            onParsedChanged = OnParsedChanged;
            onFreed = OnFreed;
            onStateChanged = OnStateChanged;

            onMetaChangedHandle = GCHandle.Alloc(onMetaChanged);
            onSubItemAddedHandle = GCHandle.Alloc(onSubItemAdded);
            onDurationChangedHandle = GCHandle.Alloc(onDurationChanged);
            onParsedChangedHandle = GCHandle.Alloc(onParsedChanged);
            onFreedHandle = GCHandle.Alloc(onFreed);
            onStateChangedHandle = GCHandle.Alloc(onStateChanged);

            HandleManager.Add(this);

            EventManager.Attach(Interop.Core.Events.EventTypes.MediaMetaChanged, onMetaChanged, IntPtr.Zero);
            EventManager.Attach(Interop.Core.Events.EventTypes.MediaSubItemAdded, onSubItemAdded, IntPtr.Zero);
            EventManager.Attach(Interop.Core.Events.EventTypes.MediaDurationChanged, onDurationChanged, IntPtr.Zero);
            EventManager.Attach(Interop.Core.Events.EventTypes.MediaParsedChanged, onParsedChanged, IntPtr.Zero);
            EventManager.Attach(Interop.Core.Events.EventTypes.MediaFreed, onFreed, IntPtr.Zero);
            EventManager.Attach(Interop.Core.Events.EventTypes.MediaStateChanged, onStateChanged, IntPtr.Zero);
        }

        private void OnMetaChanged(ref Interop.Core.Events.LibVlcEventArgs arg, IntPtr userData)
        {
            if(MetaChanged != null)
            {
                MetaChanged(this, arg.MediaMetaChanged);
            }
        }

        public event EventHandler<Interop.Core.Events.MediaMetaChangedArgs> MetaChanged;

        private void OnSubItemAdded(ref Interop.Core.Events.LibVlcEventArgs arg, IntPtr userData)
        {
            if(SubItemAdded != null)
            {
                SubItemAdded(this, arg.MediaSubitemAdded);
            }
        }

        public event EventHandler<Interop.Core.Events.MediaSubitemAddedArgs> SubItemAdded;

        private void OnDurationChanged(ref Interop.Core.Events.LibVlcEventArgs arg, IntPtr userData)
        {
            if (DurationChanged != null)
            {
                DurationChanged(this, arg.MediaDurationChanged);
            }
        }

        public event EventHandler<Interop.Core.Events.MediaDurationChangedArgs> DurationChanged;

        private void OnParsedChanged(ref Interop.Core.Events.LibVlcEventArgs arg, IntPtr userData)
        {
            if (ParsedChanged != null)
            {
                ParsedChanged(this, arg.MediaParsedChanged);
            }
        }

        public event EventHandler<Interop.Core.Events.MediaParsedChangedArgs> ParsedChanged;

        private void OnFreed(ref Interop.Core.Events.LibVlcEventArgs arg, IntPtr userData)
        {
            if (Freed != null)
            {
                Freed(this, arg.MediaFreed);
            }
        }

        public event EventHandler<Interop.Core.Events.MediaFreedArgs> Freed;

        private void OnStateChanged(ref Interop.Core.Events.LibVlcEventArgs arg, IntPtr userData)
        {
            if (StateChanged != null)
            {
                StateChanged(this, arg.MediaStateChanged);
            }
        }

        public event EventHandler<Interop.Core.Events.MediaStateChangedArgs> StateChanged;

        /// <summary>
        /// 获取 Media 实例指针
        /// </summary>
        public IntPtr InstancePointer { get; private set; }

        public VlcEventManager EventManager { get; private set; }

        /// <summary>
        /// 通过名称创建一个新的 VlcMedia
        /// </summary>
        /// <param name="vlc">Vlc 对象</param>
        /// <param name="name">媒体名称</param>
        public static VlcMedia CreateAsNewNode(Vlc vlc,String name)
        {
            return new VlcMedia(CreateMediaAsNewNodeFunction.Delegate(vlc.InstancePointer, name));
        }

        /// <summary>
        /// 通过给定的文件描述符创建一个新的 VlcMedia
        /// </summary>
        /// <param name="vlc">Vlc 对象</param>
        /// <param name="fileDescriptor">文件描述符</param>
        public static VlcMedia CreateFormFileDescriptor(Vlc vlc,int fileDescriptor)
        {
            return new VlcMedia(CreateMediaFormFileDescriptorFunction.Delegate(vlc.InstancePointer, fileDescriptor));
        }

        /// <summary>
        /// 通过给定的文件 Url 创建一个新的 VlcMedia,该 Url 的格式必须以 "file://" 开头,参见 "RFC3986".
        /// </summary>
        /// <param name="vlc">Vlc 对象</param>
        /// <param name="url">文件 Url</param>
        public static VlcMedia CreateFormLocation(Vlc vlc,String url)
        {
            return new VlcMedia(CreateMediaFormLocationFunction.Delegate(vlc.InstancePointer, url));
        }

        /// <summary>
        /// 通过给定的文件路径创建一个新的 VlcMedia
        /// </summary>
        /// <param name="vlc">Vlc 对象</param>
        /// <param name="path">文件路径</param>
        public static VlcMedia CreateFormPath(Vlc vlc,String path)
        {
            return new VlcMedia(CreateMediaFormPathFunction.Delegate(vlc.InstancePointer, path));
        }

        /// <summary>
        /// 向一个媒体添加一个选项,这个选项将会确定媒体播放器将如何读取介质,
        /// </summary>
        /// <param name="options"></param>
        public void AddOption(String options)
        {
            AddOptionFunction.Delegate(InstancePointer, options);
        }

        /// <summary>
        /// 向一个媒体通过可配置的标志添加一个选项,这个选项将会确定媒体播放器将如何读取介质,
        /// </summary>
        /// <param name="options"></param>
        /// <param name="flags"></param>
        public void AddOptionFlag(String options,MediaOption flag)
        {
            AddOptionFlagFunction.Delegate(InstancePointer, options, flag);
        }

        /// <summary>
        /// 复制一个媒体对象
        /// </summary>
        /// <returns>复制的媒体对象</returns>
        public VlcMedia Duplicate()
        {
            return new VlcMedia(DuplicateFunction.Delegate(InstancePointer));
        }

        /// <summary>
        /// 获取媒体的基本编码器的说明
        /// </summary>
        /// <param name="type">由 <see cref="MediaTrack.Type"/> 得来</param>
        /// <param name="codec">由 <see cref="MediaTrack.Codec"/> 得来</param>
        /// <returns>返回媒体的基本编码器的说明</returns>
        public static String GetCodecDescription(TrackType type,int codec)
        {
            return InteropHelper.PtrToString(GetCodecDescriptionFunction.Delegate(type, codec));
        }

        /// <summary>
        /// 获取媒体的时间长度
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return new TimeSpan(GetDurationFunction.Delegate(InstancePointer) * 10000);
            }
        }

        /// <summary>
        /// 获取媒体的某个元属性,如果尚未解析元属性,将会返回 NULL.
        /// 这个方法会自动调用 <see cref="ParseMediaAsync"/> 方法,所以你在之后应该会收到一个 MediaMetaChanged 事件.
        /// 如果你喜欢同步版本,可以在 GetMeta 之前调用 <see cref="ParseMedia"/> 方法
        /// </summary>
        /// <param name="type">元属性类型</param>
        /// <returns>返回媒体的某个元属性</returns>
        public String GetMeta(MetaDataType type)
        {
            return InteropHelper.PtrToString(GetMetaFunction.Delegate(InstancePointer, type));
        }

        /// <summary>
        /// 获取该媒体的媒体资源地址
        /// </summary>
        public String Mrl
        {
            get
            {
                return InteropHelper.PtrToString(GetMrlFunction.Delegate(InstancePointer));
            }
        }

        /// <summary>
        /// 获取媒体当前状态
        /// </summary>
        public MediaState State
        {
            get
            {
                return GetStateFunction.Delegate(InstancePointer);
            }
        }

        private MediaStats stats = new MediaStats();
        /// <summary>
        /// 获取媒体当前统计
        /// </summary>
        public MediaStats Stats
        {
            get
            {
                if (GetStatsFunction.Delegate(InstancePointer, ref stats))
                {
                    return stats;
                }
                else
                {
                    throw new Exception("无法获取媒体统计信息");
                }
            }
        }

        /// <summary>
        /// 获取媒体的基本流的描述,注意,在调用该方法之前你需要首先调用 <see cref="ParseMedia"/> 方法,或者至少播放一次.
        /// 否则,你将的得到一个空数组
        /// </summary>
        /// <param name="tracks">一个 <see cref="MediaTrackInfo"/> 数组</param>
        /// <returns>数组的元素个数</returns>
        public MediaTrackInfo[] GetTrackInfo()
        {
            var pointer = IntPtr.Zero;
            int count = GetTracksInfoFunction.Delegate(InstancePointer, out pointer);
            var result = new MediaTrackInfo[count];
            var temp = pointer;

            for (int i = 0; i < count; i++)
            {
                result[i] = (MediaTrackInfo)Marshal.PtrToStructure(temp, typeof(MediaTrackInfo));
                temp += Marshal.SizeOf(typeof(MediaTrackInfo));
            }

            Vlc.Free(pointer);
            return result;
        }

        /// <summary>
        /// 获取或设置由用户定义的媒体数据
        /// </summary>
        public IntPtr UserData
        {
            get
            {
                return GetUserDataFunction.Delegate(InstancePointer);
            }

            set
            {
                SetUserDataFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值表示该媒体是否已经解析
        /// </summary>
        public bool IsParsed
        {
            get
            {
                return IsParsedFunction.Delegate(InstancePointer);
            }
        }

        /// <summary>
        /// 解析一个媒体,获取媒体的元数据和轨道信息
        /// </summary>
        public void Parse()
        {
            ParseMediaFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 异步解析一个媒体,获取媒体的元数据和轨道信息,这是 <see cref="VlcMedia.Parse"/> 的异步版本,
        /// 解析完成会触发 <see cref="VlcMedia.ParsedChanged"/> 事件,您可以跟踪该事件
        /// </summary>
        public void ParseAsync()
        {
            ParseMediaAsyncFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 根据提供的标志异步解析一个媒体,获取媒体的元数据和轨道信息,这是 <see cref="VlcMedia.ParseAsync"/> 的高级版本,
        /// 默认情况下解析一个本地文件,解析完成会触发 <see cref="VlcMedia.ParsedChanged"/> 事件,您可以跟踪该事件
        /// </summary>
        public void ParseWithOptionAsync(MediaParseFlag option)
        {
            ParseMediaWithOptionAsyncFunction.Delegate(InstancePointer, option);
        }

        /// <summary>
        /// 递增媒体对象的引用计数
        /// </summary>
        public void RetainMedia()
        {
            RetainMediaFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 保存当前的元数据到媒体
        /// </summary>
        /// <returns>如果操作成功将会返回 True</returns>
        public bool SaveMeta()
        {
            return SaveMetaFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 设置媒体的元数据
        /// </summary>
        /// <param name="type">元数据类型</param>
        /// <param name="data">元数据值</param>
        public void SetMeta(MetaDataType type,String data)
        {
            SetMetaFunction.Delegate(InstancePointer, type, data);
        }

        /*
        public void GetSubitems()
        {
            return GetSubitemsFunction.Delegate(InstancePointer);
        }
        */

        /// <summary>    
        /// 获取媒体的基本流的描述,注意,在调用该方法之前你需要首先调用 <see cref="VlcMedia.Parse"/> 方法,或者至少播放一次.
        /// 否则,你将的得到一个空数组
        /// </summary>
        public MediaTrack[] GetTracks()
        {
            var pointer = IntPtr.Zero;
            uint count = GetTracksFunction.Delegate(InstancePointer, ref pointer);
            if(pointer == IntPtr.Zero)
            {
                String ex = VlcError.GetErrorMessage();
                throw new Exception(ex);
            }
            var result = new MediaTrack[count];
            var temp = pointer;
            for (int i = 0; i < count; i++)
            {
                var p = Marshal.ReadIntPtr(temp);
                result[i] = (MediaTrack)Marshal.PtrToStructure(p, typeof(MediaTrack));
                temp += Marshal.SizeOf(typeof(IntPtr));
            }

            ReleaseTracksFunction.Delegate(pointer, count);
            return result;
        }

        public VlcMediaPlayer CreateMediaPlayer()
        {
            return VlcMediaPlayer.CreatFormMedia(this);
        }


        bool disposed = false;

        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            HandleManager.Remove(this);
            EventManager.Dispose();
            onMetaChangedHandle.Free();
            onSubItemAddedHandle.Free();
            onDurationChangedHandle.Free();
            onParsedChangedHandle.Free();
            onFreedHandle.Free();
            onStateChangedHandle.Free();
            ReleaseMediaFunction.Delegate(InstancePointer);
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
    }
}
