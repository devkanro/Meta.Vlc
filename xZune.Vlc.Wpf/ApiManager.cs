//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: ApiManager.cs
//Version: 20151220

using System;

namespace xZune.Vlc.Wpf
{
    /// <summary>
    /// The manager of LibVlc api.
    /// </summary>
    public static class ApiManager
    {
        #region --- Properties ---

        /// <summary>
        /// The path of LibVlc dlls.
        /// </summary>
        public static String LibVlcPath { get; private set; }

        /// <summary>
        /// The options when initialize LibVlc.
        /// </summary>
        public static String[] VlcOption { get; private set; }

        /// <summary>
        /// The instance of VLC.
        /// </summary>
        public static xZune.Vlc.Vlc Vlc { get; private set; }

        /// <summary>
        /// The state of VLC initialization.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        #endregion --- Properties ---

        #region --- Initialization ---

        static ApiManager()
        {
            IsInitialized = false;
            LibVlcPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\LibVlc\";
        }

        private static void Initialize()
        {
            if (IsInitialized) return;
            Vlc.LoadLibVlc(LibVlcPath);
            Vlc = new Vlc(VlcOption);
            IsInitialized = true;
        }

        /// <summary>
        /// Initialize the VLC with path of LibVlc.
        /// </summary>
        /// <param name="libVlcPath"></param>
        public static void Initialize(String libVlcPath)
        {
            LibVlcPath = libVlcPath;
            Initialize();
        }

        /// <summary>
        /// Initialize the VLC with path of LibVlc and options.
        /// </summary>
        /// <param name="libVlcPath"></param>
        /// <param name="vlcOption"></param>
        public static void Initialize(String libVlcPath, params String[] vlcOption)
        {
            LibVlcPath = libVlcPath;
            VlcOption = vlcOption;
            Initialize();
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        /// <summary>
        /// Release VLC instance.
        /// </summary>
        public static void ReleaseAll()
        {
            if (Vlc != null)
                Vlc.Dispose();
        }

        #endregion --- Cleanup ---
    }
}