using System;
using System.Runtime.InteropServices;
using System.Text;
using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core.Events;
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
        /// 载入 LibVlc 的 Media 模块,该方法会在 <see cref="Vlc.LoadLibVlc()"/> 中自动被调用
        /// </summary>
        /// <param name="libHandle"></param>
        /// <param name="libVersion"></param>
        /// <param name="devString"></param>
        public static void LoadLibVlc(IntPtr libHandle,Version libVersion, String devString)
        {
            if(!IsLibLoaded)
            {
                _addOptionFunction = new LibVlcFunction<MediaAddOption>(libHandle, libVersion, devString);
                _addOptionFlagFunction = new LibVlcFunction<MediaAddOptionFlag>(libHandle, libVersion, devString);
                _duplicateFunction = new LibVlcFunction<MediaDuplicate>(libHandle, libVersion, devString);
                _getEventManagerFunction = new LibVlcFunction<GetEventManager>(libHandle, libVersion, devString);
                _getCodecDescriptionFunction = new LibVlcFunction<GetCodecDescription>(libHandle, libVersion, devString);
                _getDurationFunction = new LibVlcFunction<GetDuration>(libHandle, libVersion, devString);
                _getMetaFunction = new LibVlcFunction<GetMeta>(libHandle, libVersion, devString);
                _getMrlFunction = new LibVlcFunction<GetMrl>(libHandle, libVersion, devString);
                _getStateFunction = new LibVlcFunction<GetState>(libHandle, libVersion, devString);
                _getStatsFunction = new LibVlcFunction<GetStats>(libHandle, libVersion, devString);
                _getTracksInfoFunction = new LibVlcFunction<GetTracksInfo>(libHandle, libVersion, devString);
                _getUserDataFunction = new LibVlcFunction<GetUserData>(libHandle, libVersion, devString);
                _isParsedFunction = new LibVlcFunction<IsParsed>(libHandle, libVersion, devString);
                _createMediaAsNewNodeFunction = new LibVlcFunction<CreateMediaAsNewNode>(libHandle, libVersion, devString);
                _createMediaFormFileDescriptorFunction = new LibVlcFunction<CreateMediaFormFileDescriptor>(libHandle, libVersion, devString);
                _createMediaFormLocationFunction = new LibVlcFunction<CreateMediaFormLocation>(libHandle, libVersion, devString);
                _createMediaFormPathFunction = new LibVlcFunction<CreateMediaFormPath>(libHandle, libVersion, devString);
                _parseMediaFunction = new LibVlcFunction<ParseMedia>(libHandle, libVersion, devString);
                _parseMediaAsyncFunction = new LibVlcFunction<ParseMediaAsync>(libHandle, libVersion, devString);
                _parseMediaWithOptionAsyncFunction = new LibVlcFunction<ParseMediaWithOptionAsync>(libHandle, libVersion, devString);
                _releaseMediaFunction = new LibVlcFunction<ReleaseMedia>(libHandle, libVersion, devString);
                _retainMediaFunction = new LibVlcFunction<RetainMedia>(libHandle, libVersion, devString);
                _saveMetaFunction = new LibVlcFunction<SaveMeta>(libHandle, libVersion, devString);
                _setMetaFunction = new LibVlcFunction<SetMeta>(libHandle, libVersion, devString);
                _setUserDataFunction = new LibVlcFunction<SetUserData>(libHandle, libVersion, devString);
                _getSubitemsFunction = new LibVlcFunction<GetSubitems>(libHandle, libVersion, devString);
                _getTracksFunction = new LibVlcFunction<GetTracks>(libHandle, libVersion, devString);
                _releaseTracksFunction = new LibVlcFunction<ReleaseTracks>(libHandle, libVersion, devString);
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

        private static LibVlcFunction<MediaAddOption> _addOptionFunction;
        private static LibVlcFunction<MediaAddOptionFlag> _addOptionFlagFunction;
        private static LibVlcFunction<MediaDuplicate> _duplicateFunction;
        private static LibVlcFunction<GetEventManager> _getEventManagerFunction;
        private static LibVlcFunction<GetCodecDescription> _getCodecDescriptionFunction;
        private static LibVlcFunction<GetDuration> _getDurationFunction;
        private static LibVlcFunction<GetMeta> _getMetaFunction;
        private static LibVlcFunction<GetMrl> _getMrlFunction;
        private static LibVlcFunction<GetState> _getStateFunction;
        private static LibVlcFunction<GetStats> _getStatsFunction;
        private static LibVlcFunction<GetTracksInfo> _getTracksInfoFunction;
        private static LibVlcFunction<GetUserData> _getUserDataFunction;
        private static LibVlcFunction<IsParsed> _isParsedFunction;
        private static LibVlcFunction<CreateMediaAsNewNode> _createMediaAsNewNodeFunction;
        private static LibVlcFunction<CreateMediaFormFileDescriptor> _createMediaFormFileDescriptorFunction;
        private static LibVlcFunction<CreateMediaFormLocation> _createMediaFormLocationFunction;
        private static LibVlcFunction<CreateMediaFormPath> _createMediaFormPathFunction;
        private static LibVlcFunction<ParseMedia> _parseMediaFunction;
        private static LibVlcFunction<ParseMediaAsync> _parseMediaAsyncFunction;
        private static LibVlcFunction<ParseMediaWithOptionAsync> _parseMediaWithOptionAsyncFunction;
        private static LibVlcFunction<ReleaseMedia> _releaseMediaFunction;
        private static LibVlcFunction<RetainMedia> _retainMediaFunction;
        private static LibVlcFunction<SaveMeta> _saveMetaFunction;
        private static LibVlcFunction<SetMeta> _setMetaFunction;
        private static LibVlcFunction<SetUserData> _setUserDataFunction;
        private static LibVlcFunction<GetSubitems> _getSubitemsFunction;
        private static LibVlcFunction<GetTracks> _getTracksFunction;
        private static LibVlcFunction<ReleaseTracks> _releaseTracksFunction;

        readonly LibVlcEventCallBack _onMetaChanged;
        readonly LibVlcEventCallBack _onSubItemAdded;
        readonly LibVlcEventCallBack _onDurationChanged;
        readonly LibVlcEventCallBack _onParsedChanged;
        readonly LibVlcEventCallBack _onFreed;
        readonly LibVlcEventCallBack _onStateChanged;

        GCHandle _onMetaChangedHandle;
        GCHandle _onSubItemAddedHandle;
        GCHandle _onDurationChangedHandle;
        GCHandle _onParsedChangedHandle;
        GCHandle _onFreedHandle;
        GCHandle _onStateChangedHandle;

        private VlcMedia(IntPtr pointer)
        {
            InstancePointer = pointer;
            EventManager = new VlcEventManager(_getEventManagerFunction.Delegate(InstancePointer));

            _onMetaChanged = OnMetaChanged;
            _onSubItemAdded = OnSubItemAdded;
            _onDurationChanged = OnDurationChanged;
            _onParsedChanged = OnParsedChanged;
            _onFreed = OnFreed;
            _onStateChanged = OnStateChanged;

            _onMetaChangedHandle = GCHandle.Alloc(_onMetaChanged);
            _onSubItemAddedHandle = GCHandle.Alloc(_onSubItemAdded);
            _onDurationChangedHandle = GCHandle.Alloc(_onDurationChanged);
            _onParsedChangedHandle = GCHandle.Alloc(_onParsedChanged);
            _onFreedHandle = GCHandle.Alloc(_onFreed);
            _onStateChangedHandle = GCHandle.Alloc(_onStateChanged);

            HandleManager.Add(this);

            EventManager.Attach(EventTypes.MediaMetaChanged, _onMetaChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaSubItemAdded, _onSubItemAdded, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaDurationChanged, _onDurationChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaParsedChanged, _onParsedChanged, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaFreed, _onFreed, IntPtr.Zero);
            EventManager.Attach(EventTypes.MediaStateChanged, _onStateChanged, IntPtr.Zero);
        }

        private void OnMetaChanged(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if(MetaChanged != null)
            {
                MetaChanged(this, new ObjectEventArgs<MediaMetaChangedArgs>(arg.MediaMetaChanged));
            }
        }
        public event EventHandler<ObjectEventArgs<MediaMetaChangedArgs>> MetaChanged;

        private void OnSubItemAdded(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if(SubItemAdded != null)
            {
                SubItemAdded(this, new ObjectEventArgs<MediaSubitemAddedArgs>(arg.MediaSubitemAdded));
            }
        }
        public event EventHandler<ObjectEventArgs<MediaSubitemAddedArgs>> SubItemAdded;

        private void OnDurationChanged(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (DurationChanged != null)
            {
                DurationChanged(this, new ObjectEventArgs<MediaDurationChangedArgs>(arg.MediaDurationChanged));
            }
        }

        public event EventHandler<ObjectEventArgs<MediaDurationChangedArgs>> DurationChanged;

        private void OnParsedChanged(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (ParsedChanged != null)
            {
                ParsedChanged(this, new ObjectEventArgs<MediaParsedChangedArgs>(arg.MediaParsedChanged));
            }
        }

        public event EventHandler<ObjectEventArgs<MediaParsedChangedArgs>> ParsedChanged;

        private void OnFreed(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (Freed != null)
            {
                Freed(this, new ObjectEventArgs<MediaFreedArgs>(arg.MediaFreed));
            }
        }

        public event EventHandler<ObjectEventArgs<MediaFreedArgs>> Freed;

        private void OnStateChanged(ref LibVlcEventArgs arg, IntPtr userData)
        {
            if (StateChanged != null)
            {
                StateChanged(this, new ObjectEventArgs<MediaStateChangedArgs>(arg.MediaStateChanged));
            }
        }

        public event EventHandler<ObjectEventArgs<MediaStateChangedArgs>> StateChanged;

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
            GCHandle handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(name), GCHandleType.Pinned);
            var madia = new VlcMedia(_createMediaAsNewNodeFunction.Delegate(vlc.InstancePointer, handle.AddrOfPinnedObject()));
            handle.Free();
            return madia;
        }

        /// <summary>
        /// 通过给定的文件描述符创建一个新的 VlcMedia
        /// </summary>
        /// <param name="vlc">Vlc 对象</param>
        /// <param name="fileDescriptor">文件描述符</param>
        public static VlcMedia CreateFormFileDescriptor(Vlc vlc,int fileDescriptor)
        {
            return new VlcMedia(_createMediaFormFileDescriptorFunction.Delegate(vlc.InstancePointer, fileDescriptor));
        }

        /// <summary>
        /// 通过给定的文件 Url 创建一个新的 VlcMedia,该 Url 的格式必须以 "file://" 开头,参见 "RFC3986".
        /// </summary>
        /// <param name="vlc">Vlc 对象</param>
        /// <param name="url">文件 Url</param>
        public static VlcMedia CreateFormLocation(Vlc vlc,String url)
        {
            GCHandle handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(url), GCHandleType.Pinned);
            var media = new VlcMedia(_createMediaFormLocationFunction.Delegate(vlc.InstancePointer, handle.AddrOfPinnedObject()));
            handle.Free();
            return media;
        }

        /// <summary>
        /// 通过给定的文件路径创建一个新的 VlcMedia
        /// </summary>
        /// <param name="vlc">Vlc 对象</param>
        /// <param name="path">文件路径</param>
        public static VlcMedia CreateFormPath(Vlc vlc,String path)
        {
            GCHandle handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(path), GCHandleType.Pinned);
            var media = new VlcMedia(_createMediaFormPathFunction.Delegate(vlc.InstancePointer, handle.AddrOfPinnedObject()));
            handle.Free();
            return media;
        }

        /// <summary>
        /// 向一个媒体添加一个选项,这个选项将会确定媒体播放器将如何读取介质,
        /// </summary>
        /// <param name="options"></param>
        public void AddOption(String options)
        {
            GCHandle handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(options), GCHandleType.Pinned);
            _addOptionFunction.Delegate(InstancePointer, handle.AddrOfPinnedObject());
            handle.Free();
        }

        /// <summary>
        /// 向一个媒体通过可配置的标志添加一个选项,这个选项将会确定媒体播放器将如何读取介质,
        /// </summary>
        /// <param name="options"></param>
        /// <param name="flag"></param>
        public void AddOptionFlag(String options,MediaOption flag)
        {
            GCHandle handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(options), GCHandleType.Pinned);
            _addOptionFlagFunction.Delegate(InstancePointer, handle.AddrOfPinnedObject(), flag);
        }

        /// <summary>
        /// 复制一个媒体对象
        /// </summary>
        /// <returns>复制的媒体对象</returns>
        public VlcMedia Duplicate()
        {
            return new VlcMedia(_duplicateFunction.Delegate(InstancePointer));
        }

        /// <summary>
        /// 获取媒体的基本编码器的说明
        /// </summary>
        /// <param name="type">由 <see cref="MediaTrack.Type"/> 得来</param>
        /// <param name="codec">由 <see cref="MediaTrack.Codec"/> 得来</param>
        /// <returns>返回媒体的基本编码器的说明</returns>
        public static String GetCodecDescription(TrackType type,int codec)
        {
            return InteropHelper.PtrToString(_getCodecDescriptionFunction.Delegate(type, codec));
        }

        /// <summary>
        /// 获取媒体的时间长度
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return new TimeSpan(_getDurationFunction.Delegate(InstancePointer) * 10000);
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
            return InteropHelper.PtrToString(_getMetaFunction.Delegate(InstancePointer, type));
        }

        /// <summary>
        /// 获取该媒体的媒体资源地址
        /// </summary>
        public String Mrl
        {
            get
            {
                return InteropHelper.PtrToString(_getMrlFunction.Delegate(InstancePointer));
            }
        }

        /// <summary>
        /// 获取媒体当前状态
        /// </summary>
        public MediaState State
        {
            get
            {
                return _getStateFunction.Delegate(InstancePointer);
            }
        }

        private MediaStats _stats;

        /// <summary>
        /// 获取媒体当前统计
        /// </summary>
        public MediaStats Stats
        {
            get
            {
                if (_getStatsFunction.Delegate(InstancePointer, ref _stats))
                {
                    return _stats;
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
        /// <returns>一个 <see cref="MediaTrackInfo"/> 数组</returns>
        public MediaTrackInfo[] GetTrackInfo()
        {
            IntPtr pointer;
            var count = _getTracksInfoFunction.Delegate(InstancePointer, out pointer);
            var result = new MediaTrackInfo[count];
            var temp = pointer;

            for (var i = 0; i < count; i++)
            {
                result[i] = (MediaTrackInfo)Marshal.PtrToStructure(temp, typeof(MediaTrackInfo));
                temp = (IntPtr)((int)temp + Marshal.SizeOf(typeof(MediaTrackInfo)));
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
                return _getUserDataFunction.Delegate(InstancePointer);
            }

            set
            {
                _setUserDataFunction.Delegate(InstancePointer, value);
            }
        }

        /// <summary>
        /// 获取一个值表示该媒体是否已经解析
        /// </summary>
        public bool IsParsed
        {
            get
            {
                return _isParsedFunction.Delegate(InstancePointer);
            }
        }

        public IntPtr Subitems
        {
            get { return _getSubitemsFunction.Delegate(InstancePointer); }
        }

        /// <summary>
        /// 解析一个媒体,获取媒体的元数据和轨道信息
        /// </summary>
        public void Parse()
        {
            _parseMediaFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 异步解析一个媒体,获取媒体的元数据和轨道信息,这是 <see cref="VlcMedia.Parse"/> 的异步版本,
        /// 解析完成会触发 <see cref="VlcMedia.ParsedChanged"/> 事件,您可以跟踪该事件
        /// </summary>
        public void ParseAsync()
        {
            _parseMediaAsyncFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 根据提供的标志异步解析一个媒体,获取媒体的元数据和轨道信息,这是 <see cref="VlcMedia.ParseAsync"/> 的高级版本,
        /// 默认情况下解析一个本地文件,解析完成会触发 <see cref="VlcMedia.ParsedChanged"/> 事件,您可以跟踪该事件
        /// </summary>
        public void ParseWithOptionAsync(MediaParseFlag option)
        {
            _parseMediaWithOptionAsyncFunction.Delegate(InstancePointer, option);
        }

        /// <summary>
        /// 递增媒体对象的引用计数
        /// </summary>
        public void RetainMedia()
        {
            _retainMediaFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 保存当前的元数据到媒体
        /// </summary>
        /// <returns>如果操作成功将会返回 True</returns>
        public bool SaveMeta()
        {
            return _saveMetaFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 设置媒体的元数据
        /// </summary>
        /// <param name="type">元数据类型</param>
        /// <param name="data">元数据值</param>
        public void SetMeta(MetaDataType type,String data)
        {
            GCHandle handle = GCHandle.Alloc(Encoding.UTF8.GetBytes(data), GCHandleType.Pinned);
            _setMetaFunction.Delegate(InstancePointer, type, handle.AddrOfPinnedObject());
            handle.Free();
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
            var count = _getTracksFunction.Delegate(InstancePointer, ref pointer);
            if(pointer == IntPtr.Zero)
            {
                var ex = VlcError.GetErrorMessage();
                throw new Exception(ex);
            }
            var result = new MediaTrack[count];
            var temp = pointer;
            for (var i = 0; i < count; i++)
            {
                var p = Marshal.ReadIntPtr(temp);
                result[i] = (MediaTrack)Marshal.PtrToStructure(p, typeof(MediaTrack));
                temp = (IntPtr)((int)temp + Marshal.SizeOf(typeof(IntPtr)));
            }

            _releaseTracksFunction.Delegate(pointer, count);
            return result;
        }

        public VlcMediaPlayer CreateMediaPlayer()
        {
            return VlcMediaPlayer.CreatFormMedia(this);
        }


        bool _disposed;

        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            HandleManager.Remove(this);
            EventManager.Dispose();
            _onMetaChangedHandle.Free();
            _onSubItemAddedHandle.Free();
            _onDurationChangedHandle.Free();
            _onParsedChangedHandle.Free();
            _onFreedHandle.Free();
            _onStateChangedHandle.Free();
            _releaseMediaFunction.Delegate(InstancePointer);
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
    }

    public class ObjectEventArgs<T> : EventArgs
    {
        public ObjectEventArgs()
        {
            
        }

        public ObjectEventArgs(T value)
        {
            Value = value;
        }
        public T Value { get; set; }
    }
}
