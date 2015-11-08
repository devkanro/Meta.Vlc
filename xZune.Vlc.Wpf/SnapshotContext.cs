//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: SnapshotContext.cs
//Version: 20151108

using System;
using System.IO;

namespace xZune.Vlc.Wpf
{
  public class SnapshotContext
  {

    #region --- Fields ---

    private static int _count;

    #endregion

    #region --- Initialization ---

    public SnapshotContext(String path, SnapshotFormat format, int quality)
    {
      Path = path.Replace('/', '\\');
      if (Path[Path.Length - 1] == '\\')
        Path = Path.Substring(0, Path.Length - 1);
      Format = format;
      Quality = quality;
    }

    #endregion

    #region --- Properties ---

    public String Path { get; private set; }
    public String Name { get; private set; }
    public SnapshotFormat Format { get; private set; }
    public int Quality { get; private set; }

    #endregion

    #region --- Methods ---

    public String GetName(VlcPlayer player)
    {
      player.Dispatcher.Invoke(new Action(() =>
      {
        Name = String.Format("{0}-{1}-{2}",
                  GetMediaName(player.VlcMediaPlayer.Media.Mrl.Replace("file:///", "")),
                  (int)(player.Time.TotalMilliseconds), _count++);
      }));
      return Name;
    }

    internal static String GetMediaName(String path)
    {
      if (VlcPlayer.IsRootPath(path))
      {
        path = path.Replace('/', '\\').ToUpper();
        foreach (var item in DriveInfo.GetDrives())
          if (item.Name.ToUpper() == path)
            return item.VolumeLabel;
      }
      else
        return System.IO.Path.GetFileNameWithoutExtension(path);

      return "Unkown";
    }

    #endregion

  }

}
