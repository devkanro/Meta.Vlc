// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: ApiManager.cs
// Version: 20181231

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Meta.Vlc.Wpf
{
    /// <summary>
    ///     The manager of LibVlc api.
    /// </summary>
    public static class ApiManager
    {
        #region --- Fields ---

        public static Vlc _defaultVlc;

        #endregion --- Fields ---

        #region --- Cleanup ---

        /// <summary>
        ///     Release VLC instance.
        /// </summary>
        public static void ReleaseAll()
        {
            if (Vlcs == null) return;
            foreach (var vlc in Vlcs) vlc.Dispose();
            Vlcs.Clear();
        }

        #endregion --- Cleanup ---

        #region --- Properties ---

        /// <summary>
        ///     The path of LibVlc dlls.
        /// </summary>
        public static string LibVlcPath { get; private set; }

        /// <summary>
        ///     The options when initialize LibVlc.
        /// </summary>
        public static IList<string> VlcOption { get; private set; }

        /// <summary>
        ///     The list of VLC.
        /// </summary>
        public static List<Vlc> Vlcs { get; private set; }

        /// <summary>
        ///     Default VLC instance.
        /// </summary>
        public static Vlc DefaultVlc
        {
            get
            {
                if (_defaultVlc == null) Vlcs.Add(_defaultVlc = new Vlc(VlcOption.ToArray()));

                return _defaultVlc;
            }
        }

        /// <summary>
        ///     The state of VLC initialization.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        #endregion --- Properties ---

        #region --- Initialization ---

        static ApiManager()
        {
            IsInitialized = false;
            LibVlcPath =
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) +
                @"\LibVlc\";
        }

        private static void Initialize()
        {
            if (IsInitialized) return;
            Vlcs = new List<Vlc>();
            LibVlcManager.LoadLibVlc(LibVlcPath);
            IsInitialized = true;
        }

        /// <summary>
        ///     Initialize the VLC with path of LibVlc.
        /// </summary>
        /// <param name="libVlcPath"></param>
        public static void Initialize(string libVlcPath)
        {
            LibVlcPath = libVlcPath;
            Initialize();
        }

        /// <summary>
        ///     Initialize the VLC with path of LibVlc and options.
        /// </summary>
        /// <param name="libVlcPath"></param>
        /// <param name="vlcOption"></param>
        public static void Initialize(string libVlcPath, params string[] vlcOption)
        {
            LibVlcPath = libVlcPath;
            VlcOption = vlcOption;
            Initialize();
        }

        #endregion --- Initialization ---
    }
}