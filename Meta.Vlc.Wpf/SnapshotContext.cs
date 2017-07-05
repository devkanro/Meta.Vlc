// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: SnapshotContext.cs
// Version: 20160214

using System;
using System.IO;
using System.Windows.Media.Imaging;

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

        public SnapshotContext(String path, int quality)
        {
            Path = System.IO.Path.GetDirectoryName(path).FormatPath();
            Name = System.IO.Path.GetFileName(path);
            switch (System.IO.Path.GetExtension(path).ToLower())
            {
                case ".bmp":
                    Format = SnapshotFormat.BMP;
                    break;
                case ".png":
                    Format = SnapshotFormat.PNG;
                    break;
                case ".jpg":
                case ".jpeg":
                    Format = SnapshotFormat.JPG;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("path", @"Screenshot format must be 'bmp', 'jpg' or 'png'.");
            }
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
        
        public void Save(VlcPlayer player, BitmapSource source)
        {
            if (this.Name == null)
            {
                this.GetName(player);
            }

            BitmapEncoder encoder = null;
            switch (this.Format)
            {
                case SnapshotFormat.BMP:
                    encoder = new BmpBitmapEncoder();
                    break;

                case SnapshotFormat.JPG:
                    encoder = new JpegBitmapEncoder() { QualityLevel = this.Quality };
                    break;

                case SnapshotFormat.PNG:
                    encoder = new PngBitmapEncoder();
                    break;
            }

            encoder.Frames.Add(BitmapFrame.Create(source));
            using (Stream stream = File.Create(String.Format("{0}\\{1}", this.Path, this.Name)))
            {
                encoder.Save(stream);
            }
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

        private String GetName(VlcPlayer player)
        {
            Name = String.Format("{0}-{1}-{2}.{3}",
                GetMediaName(player.VlcMediaPlayer.Media.Mrl.Replace("file:///", "")),
                (int)(player.Time.TotalMilliseconds), _count++, Format.ToString().ToLower());
            return Name;
        }
        #endregion --- Methods ---
    }
}
