// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: Vlc.cs
// Version: 20181231

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Meta.Vlc.Interop.Core;

namespace Meta.Vlc
{
    public unsafe class Vlc : IVlcObject
    {
        private bool _disposed;

        private GCHandle _onExitHandle;

        /// <summary>
        ///     Create a Vlc instance by default options.
        /// </summary>
        /// <exception cref="VlcCreateFailException">Can't create a Vlc instance, check your Vlc options.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public Vlc() :
            this(new[]
            {
                "-I", "dummy", "--ignore-config", "--no-video-title", "--file-logging", "--logfile=log.txt",
                "--verbose=2", "--no-sub-autodetect-file"
            })
        {
        }

        /// <summary>
        ///     Create a Vlc instance by options.
        /// </summary>
        /// <param name="argv"></param>
        /// <exception cref="VlcCreateFailException">Can't create a Vlc instence, check your Vlc options.</exception>
        /// <exception cref="Exception">A delegate callback throws an exception.</exception>
        public Vlc(string[] argv)
        {
            InstancePointer = argv == null
                ? LibVlcManager.GetFunctionDelegate<libvlc_new>().Invoke(0, IntPtr.Zero)
                : LibVlcManager.GetFunctionDelegate<libvlc_new>()
                    .Invoke(argv.Length, InteropHelper.StringArrayToPtr(argv));

            if (InstancePointer == null) throw new VlcCreateFailException(VlcError.GetErrorMessage());

            ExitHandler onExit = OnExit;
            _onExitHandle = GCHandle.Alloc(onExit);
            LibVlcManager.GetFunctionDelegate<libvlc_set_exit_handler>().Invoke(InstancePointer, onExit, null);

            VlcObjectManager.Add(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void* InstancePointer { get; private set; }

        public Vlc VlcInstance => this;

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _onExitHandle.Free();
            VlcObjectManager.Remove(this);
            LibVlcManager.GetFunctionDelegate<libvlc_release>().Invoke(InstancePointer);
            InstancePointer = null;

            _disposed = true;
        }

        /// <summary>
        ///     Try to start a user interface for the libvlc instance.
        /// </summary>
        /// <param name="name">interface name, or NULL for default</param>
        /// <returns>true on success, false on error.</returns>
        public bool AddInterface(string name)
        {
            using (var handle = new StringHandle(name))
            {
                return LibVlcManager.GetFunctionDelegate<libvlc_add_intf>()
                           .Invoke(InstancePointer, handle.UnsafePointer) == 0;
            }
        }

        /// <summary>
        ///     Sets the application name. LibVLC passes this as the user agent string when a protocol requires it.
        /// </summary>
        /// <param name="name">human-readable application name, e.g. "FooBar player 1.2.3"</param>
        /// <param name="http">HTTP User Agent, e.g. "FooBar/1.2.3 Python/2.6.0"</param>
        public void SetUserAgent(string name, string http)
        {
            using (var nameHandle = new StringHandle(name))
            {
                using (var httpHandle = new StringHandle(http))
                {
                    LibVlcManager.GetFunctionDelegate<libvlc_set_user_agent>().Invoke(InstancePointer,
                        nameHandle.UnsafePointer, httpHandle.UnsafePointer);
                }
            }
        }

        /// <summary>
        ///     Sets some meta-information about the application.
        /// </summary>
        /// <param name="id">Java-style application identifier, e.g. "com.acme.foobar"</param>
        /// <param name="version">application version numbers, e.g. "1.2.3"</param>
        /// <param name="icon">application icon name, e.g. "foobar"</param>
        public void SetAppId(string id, string version, string icon)
        {
            using (var idHandle = new StringHandle(id))
            {
                using (var versionHandle = new StringHandle(version))
                {
                    using (var iconHandle = new StringHandle(icon))
                    {
                        LibVlcManager.GetFunctionDelegate<libvlc_set_app_id>().Invoke(InstancePointer,
                            idHandle.UnsafePointer, versionHandle.UnsafePointer, iconHandle.UnsafePointer);
                    }
                }
            }
        }

        /// <summary>
        ///     Returns a list of audio filters that are available.
        /// </summary>
        public List<ModuleDescription> GetAudioFilterList()
        {
            using (var list = new ModuleDescriptionList(LibVlcManager
                .GetFunctionDelegate<libvlc_audio_filter_list_get>().Invoke(InstancePointer)))
            {
                return new List<ModuleDescription>(list);
            }
        }

        /// <summary>
        ///     Returns a list of video filters that are available.
        /// </summary>
        public List<ModuleDescription> GetVideoFilterList()
        {
            using (var list = new ModuleDescriptionList(LibVlcManager
                .GetFunctionDelegate<libvlc_video_filter_list_get>().Invoke(InstancePointer)))
            {
                return new List<ModuleDescription>(list);
            }
        }

        /// <summary>
        ///     通过名称创建一个新的 VlcMedia
        /// </summary>
        /// <param name="name">媒体名称</param>
        public VlcMedia CreateMediaAsNewNode(string name)
        {
            return VlcMedia.CreateAsNewNode(this, name);
        }

        /// <summary>
        ///     通过给定的文件 Url 创建一个新的 VlcMedia,该 Url 的格式必须以 "file://" 开头,参见 "RFC3986".
        /// </summary>
        /// <param name="url">文件 Url</param>
        public VlcMedia CreateMediaFromLocation(string url)
        {
            return VlcMedia.CreateFormLocation(this, url);
        }

        /// <summary>
        ///     通过给定的文件路径创建一个新的 VlcMedia
        /// </summary>
        /// <param name="path">文件路径</param>
        public VlcMedia CreateMediaFromPath(string path)
        {
            return VlcMedia.CreateFormPath(this, path);
        }

        public VlcMediaPlayer CreateMediaPlayer()
        {
            return VlcMediaPlayer.Create(this);
        }

        public event EventHandler<EventArgs> Exit;

        private void OnExit(void* data)
        {
            Exit?.Invoke(this, new EventArgs());
        }
    }
}