using System;

namespace xZune.Vlc.Wpf
{
    public static class ApiManager
    {
        static ApiManager()
        {
            IsInited = false;
            LibVlcPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\LibVlc\";
            VlcOption = new []
            {
                "-I", "dummy", "--ignore-config", "--no-video-title","--file-logging","--logfile=log.txt","--verbose=2","--no-sub-autodetect-file"
            };
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
        public static bool IsInited { get; private set; }
        #endregion

        public static void Init()
        {
            if (IsInited) return;
            Vlc.LoadLibVlc(LibVlcPath);
            Vlc = new Vlc(VlcOption);
        }

        public static void Init(String libVlcPath)
        {
            LibVlcPath = libVlcPath;
            Init();
        }

        public static void Init(String libVlcPath, String[] vlcOption)
        {
            LibVlcPath = libVlcPath;
            VlcOption = vlcOption;
            Init();
        }

        public static void ReleaseAll()
        {
            if(Vlc != null)
            Vlc.Dispose();
        }
    }
}
