using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using xZune.Vlc;

namespace xZune.Vlc.Wpf
{
    public static class ApiManager
    {
        static ApiManager()
        {
            IsInited = false;
            LibVlcPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\LibVlc\";
        }

        #region 静态属性 LibVlcPath
        public static String LibVlcPath { get; set; }
        #endregion

        #region 只读静态属性 Vlc
        public static xZune.Vlc.Vlc Vlc { get; private set; }
        #endregion

        #region 只读静态属性 IsInited
        public static bool IsInited { get; private set; }
        #endregion

        public static void Init()
        {
            if(!IsInited)
            {
                xZune.Vlc.Vlc.LoadLibVlc(LibVlcPath);
                Vlc = new xZune.Vlc.Vlc();
            }
        }

        public static void Init(String libVlcPath)
        {
            LibVlcPath = libVlcPath;
            Init();
        }

        public static void ReleaseAll()
        {
            Vlc?.Dispose();
        }
    }
}
