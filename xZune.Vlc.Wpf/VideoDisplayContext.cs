//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: VideoDisplayContext.cs
//Version: 20151112

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

using xZune.Vlc.Interop.Media;

namespace xZune.Vlc.Wpf
{
    /// <summary>
    /// Context used to render video data
    /// </summary>
    public class VideoDisplayContext : IDisposable
    {
        #region --- Fields ---

        private bool _disposed = false;

        #endregion --- Fields ---

        #region --- Initialization ---

        public VideoDisplayContext(uint width, uint height, PixelFormat format)
            : this((int)width, (int)height, format)
        {
        }

        public VideoDisplayContext(double width, double height, PixelFormat format)
            : this((int)width, (int)height, format)
        {
        }

        public VideoDisplayContext(int width, int height, PixelFormat format)
        {
            IsAspectRatioChecked = false;
            Size = width * height * format.BitsPerPixel / 8;
            DisplayWidth = Width = width;
            DisplayHeight = Height = height;
            PixelFormat = format;
            Stride = width * format.BitsPerPixel / 8;
            FileMapping = Win32Api.CreateFileMapping(new IntPtr(-1), IntPtr.Zero, PageAccess.ReadWrite, 0, Size, null);
            MapView = Win32Api.MapViewOfFile(FileMapping, FileMapAccess.AllAccess, 0, 0, (uint)Size);
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Image = (InteropBitmap)Imaging.CreateBitmapSourceFromMemorySection(FileMapping, Width, Height, PixelFormat, Stride, 0);
            }));
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        public void Dispose(bool disposing)
        {
            if (_disposed) return;
            Size = 0;
            PixelFormat = PixelFormats.Default;
            Stride = 0;
            Image = null;
            Win32Api.UnmapViewOfFile(MapView);
            Win32Api.CloseHandle(FileMapping);
            FileMapping = MapView = IntPtr.Zero;
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion --- Cleanup ---

        #region --- Properties ---

        public int Size { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public double DisplayWidth { get; private set; }
        public double DisplayHeight { get; private set; }
        public int Stride { get; private set; }
        public PixelFormat PixelFormat { get; private set; }
        public IntPtr FileMapping { get; private set; }
        public IntPtr MapView { get; private set; }
        public InteropBitmap Image { get; private set; }
        public bool IsAspectRatioChecked { get; set; }

        #endregion --- Properties ---

        #region --- Methods ---

        public void Display()
        {
            if (Application.Current != null)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    Image.Invalidate();
                }));
            }
        }

        public void CheckDisplaySize(VideoTrack track)
        {
            if (!IsAspectRatioChecked)
            {
                var sar = 1.0 * track.SarNum / track.SarDen;

                if (track.SarNum == 0 || track.SarDen == 0) return;

                Debug.WriteLine(String.Format("Video Size:{0}x{1}\r\nSAR:{2}/{3}", track.Width, track.Height, track.SarNum, track.SarDen));

                if (sar > 1)
                {
                    DisplayWidth = sar * track.Width;
                    DisplayHeight = track.Height;
                }
                else
                {
                    DisplayWidth = track.Width;
                    DisplayHeight = track.Height / sar;
                }
            }
        }

        #endregion --- Methods ---
    }
}