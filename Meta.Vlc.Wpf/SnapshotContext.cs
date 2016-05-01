// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: SnapshotContext.cs
// Version: 20160214

using System;
using System.IO;

namespace Meta.Vlc.Wpf
{
    internal class SnapshotContext
    {
        #region --- Fields ---

        private static int _count;

        #endregion --- Fields ---

        #region --- Initialization ---

        public SnapshotContext(String path, SnapshotFormat format, int quality)
        {
            Path = path.FormatPath();
            Format = format;
            Quality = quality;
        }

        #endregion --- Initialization ---

        #region --- Properties ---

        public String Path { get; private set; }
        public String Name { get; private set; }
        public SnapshotFormat Format { get; private set; }
        public int Quality { get; private set; }

        #endregion --- Properties ---

        #region --- Methods ---

        public String GetName(VlcPlayer player)
        {
            Name = String.Format("{0}-{1}-{2}",
                GetMediaName(player.VlcMediaPlayer.Media.Mrl.Replace("file:///", "")),
                (int)(player.Time.TotalMilliseconds), _count++);
            return Name;
        }

        private static String GetMediaName(String path)
        {
            if (path.IsDriveRootDirectory())
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

        #endregion --- Methods ---
    }
}