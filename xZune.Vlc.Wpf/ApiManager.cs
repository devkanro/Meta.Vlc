//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: ApiManager.cs
//Version: 20151112

using System;

namespace xZune.Vlc.Wpf
{
  public static class ApiManager
  {

    #region --- Properties ---

    public static String LibVlcPath { get; set; }

    public static String[] VlcOption { get; set; }

    public static xZune.Vlc.Vlc Vlc { get; private set; }

    public static bool IsInitialized { get; private set; }

    #endregion

    #region --- Initialization ---

    static ApiManager()
    {
      IsInitialized = false;
      LibVlcPath = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\LibVlc\";
    }

    public static void Initialize()
    {
      if (IsInitialized) return;
      xZune.Vlc.Vlc.LoadLibVlc(LibVlcPath);
      Vlc = new xZune.Vlc.Vlc(VlcOption);
    }

    public static void Initialize(String libVlcPath)
    {
      LibVlcPath = libVlcPath;
      Initialize();
    }

    public static void Initialize(String libVlcPath, params String[] vlcOption)
    {
      LibVlcPath = libVlcPath;
      VlcOption = vlcOption;
      Initialize();
    }

    #endregion

    #region --- Cleanup ---

    public static void ReleaseAll()
    {
      if (Vlc != null)
        Vlc.Dispose();
    }

    #endregion

  }
}
