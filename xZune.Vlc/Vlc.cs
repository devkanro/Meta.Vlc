using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using xZune.Vlc.Interop;
using xZune.Vlc.Interop.Core;

namespace xZune.Vlc
{
    public class Vlc : IVlcObject
    {
        #region 静态
        static Vlc()
        {
            IsLibLoaded = false;
            LibDirectory = Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// 提供指定的路径,载入 LibVlc
        /// </summary>
        /// <param name="libPath">LibVlc 库路径</param>
        /// <param name="pluginPath">LibVlc 插件路径</param>
        public static void LoadLibVlc(String libDirectory)
        {
            LibDirectory = libDirectory;
            LoadLibVlc();
        }

        /// <summary>
        /// 使用已经设定好的路径,载入 LibVlc
        /// </summary>
        public static void LoadLibVlc()
        {
            if (!IsLibLoaded)
            {
                try
                {
                    LibCoreHandle = Win32API.LoadLibrary(String.Concat(LibDirectory, "libvlccore.dll"));
                    LibHandle = Win32API.LoadLibrary(Path.Combine(LibDirectory, "libvlc.dll"));
                }
                catch (Win32Exception e)
                {
                    throw new Exception("无法载入 LibVlc 库", e);
                }

                GetVersionFunction = new LibVlcFunction<GetVersion>(LibHandle);
                var versionString = InteropHelper.PtrToString(GetVersionFunction.Delegate());
                var match = Regex.Match(versionString, "^[0-9.]*");
                if (match.Success)
                {
                    LibVersion = new Version(match.Groups[0].Value);
                }
                var devString = versionString.Split(' ', '-')[1];
                NewInstanceFunction = new LibVlcFunction<NewInstance>(LibHandle, LibVersion, devString);
                ReleaseInstanceFunction = new LibVlcFunction<ReleaseInstance>(LibHandle, LibVersion, devString);
                RetainInstanceFunction = new LibVlcFunction<RetainInstance>(LibHandle, LibVersion, devString);
                AddInterfaceFunction = new LibVlcFunction<AddInterface>(LibHandle, LibVersion, devString);
                SetExitHandlerFunction = new LibVlcFunction<SetExitHandler>(LibHandle, LibVersion, devString);
                WaitFunction = new LibVlcFunction<Wait>(LibHandle, LibVersion, devString);
                SetUserAgentFunction = new LibVlcFunction<SetUserAgent>(LibHandle, LibVersion, devString);
                SetAppIdFunction = new LibVlcFunction<SetAppId>(LibHandle, LibVersion, devString);
                GetCompilerFunction = new LibVlcFunction<GetCompiler>(LibHandle, LibVersion, devString);
                GetChangesetFunction = new LibVlcFunction<GetChangeset>(LibHandle, LibVersion, devString);
                FreeFunction = new LibVlcFunction<Free>(LibHandle, LibVersion, devString);
                ReleaseLibVlcModuleDescriptionFunction = new LibVlcFunction<ReleaseLibVlcModuleDescription>(LibHandle, LibVersion, devString);
                GetAudioFilterListFunction = new LibVlcFunction<GetAudioFilterList>(LibHandle, LibVersion, devString);
                GetVideoFilterListFunction = new LibVlcFunction<GetVideoFilterList>(LibHandle, LibVersion, devString);
                VlcError.LoadLibVlc(LibHandle, LibVersion, devString);
                VlcEventManager.LoadLibVlc(LibHandle, LibVersion, devString);
                VlcMedia.LoadLibVlc(LibHandle, LibVersion, devString);
                VlcMediaPlayer.LoadLibVlc(LibHandle, LibVersion, devString);
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

        /// <summary>
        /// 获取载入的 LibVlc 库句柄
        /// </summary>
        public static IntPtr LibHandle { get; private set; }
        /// <summary>
        /// 获取载入的 LibVlcCore 库句柄
        /// </summary>
        public static IntPtr LibCoreHandle { get; private set; }
        /// <summary>
        /// 获取或设置载入 LibVlc 库时,提供的库的所在文件夹
        /// </summary>
        public static String LibDirectory { get; set; }

        public static Version LibVersion { get; set; }

        public static String LibDev { get; set; }
        #endregion

        #region LibVlcFunctions
        /// <summary>
        /// 创建并初始化一个 LibVlc 实例,并提供相应的参数,这些参数和命令行提供的参数类似,会影响到 LibVlc 实例的默认配置.
        /// 有效参数的列表取决于 LibVlc 版本,操作系统,可用 LibVlc 插件和平台.无效或不支持的参数会导致实例创建失败
        /// </summary>
        private static LibVlcFunction<NewInstance> NewInstanceFunction;
        /// <summary>
        /// 递减 LibVlc 实例的引用计数,如果它达到零,将会释放这个实例
        /// </summary>
        private static LibVlcFunction<ReleaseInstance> ReleaseInstanceFunction;
        /// <summary>
        /// 递增 LibVlc 实例的引用计数,当调用 NewInstance 初始化成功时,引用计数将初始化为1
        /// </summary>
        private static LibVlcFunction<RetainInstance> RetainInstanceFunction;
        /// <summary>
        /// 尝试启动一个用户接口,用于 LibVlc 实例
        /// </summary>
        private static LibVlcFunction<AddInterface> AddInterfaceFunction;
        /// <summary>
        /// 为 LibVlc 设置一个回调,该回调将会在 LibVlc 退出时被调用,不能与 <see cref="Wait"/> 一起使用.
        /// 而且,这个函数应该在播放一个列表或者开始一个用户接口前被调用,否则可能导致 LibVlc 在注册该回调前退出
        /// </summary>
        private static LibVlcFunction<SetExitHandler> SetExitHandlerFunction;
        /// <summary>
        /// 等待,直到一个接口导致 LibVlc 实例退出为止,在使用之前,应该使用 <see cref="AddInterface"/> 添加至少一个用户接口.
        /// 实际上这个方法只会导致一个线程阻塞,建议使用 <see cref="SetExitHandler"/>
        /// </summary>
        private static LibVlcFunction<Wait> WaitFunction;
        /// <summary>
        /// 设置一个用户代理字符串,当一个协议需要它的时候,LibVlc 将会提供该字符串
        /// </summary>
        private static LibVlcFunction<SetUserAgent> SetUserAgentFunction;
        /// <summary>
        /// 设置一些元信息关于该应用程序
        /// </summary>
        private static LibVlcFunction<SetAppId> SetAppIdFunction;
        /// <summary>
        /// 获取 LibVlc 的版本信息
        /// </summary>
        private static LibVlcFunction<GetVersion> GetVersionFunction;
        /// <summary>
        /// 获取 LibVlc 的编译器信息
        /// </summary>
        private static LibVlcFunction<GetCompiler> GetCompilerFunction;
        /// <summary>
        /// 获取 LibVlc 的变更集(?)
        /// </summary>
        private static LibVlcFunction<GetChangeset> GetChangesetFunction;
        /// <summary>
        /// 释放由 LibVlc 函数返回的指针资源,其作用类似于 C语言 中的 free() 函数
        /// </summary>
        private static LibVlcFunction<Free> FreeFunction;
        /// <summary>
        /// 释放 <see cref="ModuleDescription"/> 的资源
        /// </summary>
        private static LibVlcFunction<ReleaseLibVlcModuleDescription> ReleaseLibVlcModuleDescriptionFunction;
        /// <summary>
        /// 获取可用的音频过滤器
        /// </summary>
        private static LibVlcFunction<GetAudioFilterList> GetAudioFilterListFunction;
        /// <summary>
        /// 获取可用的视频过滤器
        /// </summary>
        private static LibVlcFunction<GetVideoFilterList> GetVideoFilterListFunction;
        #endregion


        /// <summary>
        /// 使用默认的参数初始化一个 Vlc 实例
        /// </summary>
        public Vlc() :
            this(new String[]
            {
                "-I", "dummy", "--ignore-config", "--no-video-title","--file-logging","--logfile=log.txt","--verbose=2","--no-sub-autodetect-file"
            })
        {

        }

        /// <summary>
        /// 提供指定的参数初始化一个 Vlc 实例
        /// </summary>
        /// <param name="argv"></param>
        public Vlc(String[] argv)
        {
            if (!IsLibLoaded)
            {
                LoadLibVlc();
            }
            InstancePointer = NewInstanceFunction.Delegate(argv.Length, InteropHelper.StringArrayToPtr(argv));
            if (InstancePointer == IntPtr.Zero)
            {
                String ex = VlcError.GetErrorMessage();
                throw new Exception(ex);
            }

            HandleManager.Add(this);
        }

        /// <summary>
        /// 获取 Vlc 实例的指针
        /// </summary>
        public IntPtr InstancePointer { get; private set; }

        /// <summary>
        /// 递增引用计数,在使用 xZune.Vlc 时,一般是不需要调用此方法,引用计数是由 Vlc 类托管的
        /// </summary>
        public void Retain()
        {
            RetainInstanceFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 尝试添加一个用户接口
        /// </summary>
        /// <param name="name">接口名,为 NULL 则为默认</param>
        /// <returns>是否成功添加接口</returns>
        public bool AddInterface(String name)
        {
            return AddInterfaceFunction.Delegate(InstancePointer, name) == 0;
        }

        /// <summary>
        /// 等待,直到一个接口导致实例退出为止,在使用之前,应该使用 <see cref="AddInterface"/> 添加至少一个用户接口.
        /// 实际上这个方法只会导致一个线程阻塞,建议使用 <see cref="Exit"/> 事件
        /// </summary>
        public void Wait()
        {
            WaitFunction.Delegate(InstancePointer);
        }

        /// <summary>
        /// 设置一个用户代理字符串,当一个协议需要它的时候,将会提供该字符串
        /// </summary>
        /// <param name="name">应用程序名称,类似于 "FooBar player 1.2.3",实际上只要能标识应用程序,任何字符串都是可以的</param>
        /// <param name="http">HTTP 用户代理,类似于 "FooBar/1.2.3 Python/2.6.0"</param>
        public void SetUserAgent(String name,String http)
        {
            SetUserAgentFunction.Delegate(InstancePointer, name, http);
        }

        /// <summary>
        /// 设置一些元信息关于该应用程序
        /// </summary>
        /// <param name="id">Java 风格的应用标识符,类似于 "com.acme.foobar"</param>
        /// <param name="version">应用程序版本,类似于 "1.2.3"</param>
        /// <param name="icon">应用程序图标,类似于 "foobar"</param>
        public void SetAppId(String id, String version, String icon)
        {
            SetAppIdFunction.Delegate(InstancePointer, id, version, icon);
        }

        /// <summary>
        /// 获取 LibVlc 的版本信息
        /// </summary>
        /// <returns></returns>
        public static String GetVersion()
        {
            return InteropHelper.PtrToString(GetVersionFunction.Delegate());
        }

        /// <summary>
        /// 获取 LibVlc 的编译器信息
        /// </summary>
        public static String GetCompiler()
        {
            return InteropHelper.PtrToString(GetCompilerFunction.Delegate());
        }

        /// <summary>
        /// 获取 LibVlc 的变更集(?)
        /// </summary>
        public static String GetChangeset()
        {
            return InteropHelper.PtrToString(GetChangesetFunction.Delegate());
        }

        /// <summary>
        /// 释放由 LibVlc 函数返回的指针资源,其作用类似于 C语言 中的 free() 函数,在使用 xZune.Vlc 时,一般是不需要调用此方法,所有指针资源都是由 Vlc 类托管的
        /// </summary>
        public static void Free(IntPtr pointer)
        {
            FreeFunction.Delegate(pointer);
        }

        /// <summary>
        /// 释放 <see cref="ModuleDescription"/> 的资源,实际上我们更推荐使用 <see cref="ModuleDescription.Dispose"/> 方法释放
        /// </summary>
        public static void ReleaseModuleDescription(ModuleDescription moduleDescription)
        {
            ReleaseLibVlcModuleDescriptionFunction.Delegate(moduleDescription.Pointer);
        }

        /// <summary>
        /// 获取可用的音频过滤器
        /// </summary>
        public ModuleDescription GetAudioFilterList()
        {
            return new ModuleDescription(GetAudioFilterListFunction.Delegate(InstancePointer));
        }

        /// <summary>
        /// 获取可用的视频过滤器
        /// </summary>
        public ModuleDescription GetVideoFilterList()
        {
            return new ModuleDescription(GetVideoFilterListFunction.Delegate(InstancePointer));
        }

        /// <summary>
        /// 通过名称创建一个新的 VlcMedia
        /// </summary>
        /// <param name="name">媒体名称</param>
        public VlcMedia CreateMediaAsNewNode(String name)
        {
            return VlcMedia.CreateAsNewNode(this, name);
        }

        /// <summary>
        /// 通过给定的文件描述符创建一个新的 VlcMedia
        /// </summary>
        /// <param name="fileDescriptor">文件描述符</param>
        public VlcMedia CreateMediaFormFileDescriptor(int fileDescriptor)
        {
            return VlcMedia.CreateFormFileDescriptor(this, fileDescriptor);
        }

        /// <summary>
        /// 通过给定的文件 Url 创建一个新的 VlcMedia,该 Url 的格式必须以 "file://" 开头,参见 "RFC3986".
        /// </summary>
        /// <param name="url">文件 Url</param>
        public VlcMedia CreateMediaFormLocation(String url)
        {
            return VlcMedia.CreateFormLocation(this, url);
        }

        /// <summary>
        /// 通过给定的文件路径创建一个新的 VlcMedia
        /// </summary>
        /// <param name="path">文件路径</param>
        public VlcMedia CreateMediaFormPath(String path)
        {
            return VlcMedia.CreateFormPath(this, path);
        }

        public VlcMediaPlayer CreateMediaPlayer()
        {
            return VlcMediaPlayer.Create(this);
        }

        bool disposed = false;

        protected void Dispose(bool disposing)
        {
            if(disposed)
            {
                return;
            }

            HandleManager.Remove(this);
            ReleaseInstanceFunction.Delegate(InstancePointer);
            InstancePointer = IntPtr.Zero;

            disposed = true;
        }

        /// <summary>
        /// 释放当前 Vlc 资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
    }
}