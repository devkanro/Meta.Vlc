using System;

namespace xZune.Vlc.Wpf
{
    public static class ApiManager
    {
        static ApiManager()
        {
            IsInitialized = false;
            LibVlcPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\LibVlc\";
        }

        #region 静态属性 LibVlcPath
        public static String LibVlcPath { get; set; }
        #endregion

        #region 静态属性 VlcOption
        public static String[] VlcOption { get; set; }
        #endregion

        #region 只读静态属性 Vlc
        public static Vlc Vlc { get; private set; }
        #endregion

        #region 只读静态属性 IsInited
        public static bool IsInitialized { get; private set; }
        #endregion

        public static void Initialize()
        {
            if (IsInitialized) return;
            Vlc.LoadLibVlc(LibVlcPath);
            Vlc = new Vlc(VlcOption);
        }

        public static void Initialize(String libVlcPath)
        {
            LibVlcPath = libVlcPath;
            Initialize();
        }

        public static void Initialize(String libVlcPath, String[] vlcOption)
        {
            LibVlcPath = libVlcPath;
            VlcOption = vlcOption;
            Initialize();
        }

        public static void ReleaseAll()
        {
            if(Vlc != null)
            Vlc.Dispose();
        }
    }
}
