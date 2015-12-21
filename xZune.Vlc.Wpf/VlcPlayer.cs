//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: VlcPlayer.cs
//Version: 20151112

using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using xZune.Vlc.Interop.MediaPlayer;
using xZune.Vlc.Wpf.Annotations;
using MediaState = xZune.Vlc.Interop.Media.MediaState;

namespace xZune.Vlc.Wpf
{
    /// <summary>
    /// VLC media player.
    /// </summary>
    public partial class VlcPlayer : Control, IDisposable, INotifyPropertyChanged
    {
        #region --- Fields ---
        
        //TODO: maybe make all fields private or protected (for descendant classes to access)?

        private VideoLockCallback _lockCallback;
        private VideoUnlockCallback _unlockCallback;
        private VideoDisplayCallback _displayCallback;
        private VideoFormatCallback _formatCallback;
        private VideoCleanupCallback _cleanupCallback;

        private GCHandle _lockCallbackHandle;
        private GCHandle _unlockCallbackHandle;
        private GCHandle _displayCallbackHandle;
        private GCHandle _formatCallbackHandle;
        private GCHandle _cleanupCallbackHandle;

        //TakeSnapshot//
        private SnapshotContext _snapshotContext;

        private VideoDisplayContext _context;
        private int _checkCount = 0;
        private Size _sar = new Size(1, 1);

        //Stop//
        private StopRequest _stopRequest = null;

        //Dispose//
        private bool _disposed = false;

        #endregion --- Fields ---

        #region --- Initialization ---

        static VlcPlayer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VlcPlayer), new FrameworkPropertyMetadata(typeof(VlcPlayer)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            ScaleTransform = new ScaleTransform(1, 1);
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (!ApiManager.IsInitialized)
                {
                    String libVlcPath = null;
                    String[] libVlcOption = null;

                    var vlcSettings =
                        System.Reflection.Assembly.GetEntryAssembly()
                            .GetCustomAttributes(typeof(VlcSettingsAttribute), false);

                    if (vlcSettings.Length > 0)
                    {
                        var vlcSettingsAttribute = vlcSettings[0] as VlcSettingsAttribute;

                        if (vlcSettingsAttribute != null && vlcSettingsAttribute.LibVlcPath != null)
                        {
                            libVlcPath = Path.IsPathRooted(vlcSettingsAttribute.LibVlcPath)
                                ? vlcSettingsAttribute.LibVlcPath
                                : CombinePath(
                                    Path.GetDirectoryName(
                                        System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName),
                                    vlcSettingsAttribute.LibVlcPath);
                        }

                        if (vlcSettingsAttribute != null && vlcSettingsAttribute.VlcOption != null)
                            libVlcOption = vlcSettingsAttribute.VlcOption;
                    }

                    if (LibVlcPath != null)
                        libVlcPath = Path.IsPathRooted(LibVlcPath) ? LibVlcPath : CombinePath(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), LibVlcPath);

                    if (VlcOption != null)
                        libVlcOption = VlcOption;

                    if (libVlcPath != null)
                        Initialize(libVlcPath, libVlcOption);
                }
            }
            base.OnInitialized(e);
        }

        /// <summary>
        /// Initialize VLC player with path of LibVlc.
        /// </summary>
        /// <param name="libVlcPath"></param>
        public void Initialize(String libVlcPath)
        {
            Initialize(libVlcPath, new[]
            {
      #if DEBUG
                "-I", "dummy", "--ignore-config", "--no-video-title","--file-logging","--logfile=log.txt","--verbose=2","--no-sub-autodetect-file"
      #else
                "-I", "dummy", "--dummy-quiet", "--ignore-config", "--no-video-title", "--no-sub-autodetect-file"
      #endif
      });
        }

        /// <summary>
        /// Initialize VLC player with path of LibVlc and options.
        /// </summary>
        /// <param name="libVlcPath"></param>
        /// <param name="libVlcOption"></param>
        public void Initialize(String libVlcPath, params String[] libVlcOption)
        {
            if (ApiManager.IsInitialized) return;

            ApiManager.Initialize(libVlcPath, libVlcOption);

            VlcMediaPlayer = ApiManager.Vlc.CreateMediaPlayer();
            if (VlcMediaPlayer != null)
            {
                VlcMediaPlayer.PositionChanged += VlcMediaPlayerPositionChanged;
                VlcMediaPlayer.TimeChanged += VlcMediaPlayerTimeChanged;
                VlcMediaPlayer.Playing += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Paused += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Stoped += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Opening += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.Buffering += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.EndReached += VlcMediaPlayerStateChanged;
                VlcMediaPlayer.SeekableChanged += VlcMediaPlayerSeekableChanged;
                VlcMediaPlayer.LengthChanged += VlcMediaPlayerLengthChanged;

                _lockCallback = VideoLockCallback;
                _unlockCallback = VideoUnlockCallback;
                _displayCallback = VideoDisplayCallback;
                _formatCallback = VideoFormatCallback;
                _cleanupCallback = VideoCleanupCallback;

                _lockCallbackHandle = GCHandle.Alloc(_lockCallback);
                _unlockCallbackHandle = GCHandle.Alloc(_unlockCallback);
                _displayCallbackHandle = GCHandle.Alloc(_displayCallback);
                _formatCallbackHandle = GCHandle.Alloc(_formatCallback);
                _cleanupCallbackHandle = GCHandle.Alloc(_cleanupCallback);

                VlcMediaPlayer.SetVideoDecodeCallback(_lockCallback, _unlockCallback, _displayCallback, IntPtr.Zero);
                VlcMediaPlayer.SetVideoFormatCallback(_formatCallback, _cleanupCallback);
            }
        }

        #endregion --- Initialization ---

        #region --- Cleanup ---

        /// <summary>
        /// Cleanup the player used resource.
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            BeginStop(() =>
            {
                if (VlcMediaPlayer != null)
                {
                    if (VlcMediaPlayer.Media != null)
                    {
                        VlcMediaPlayer.Media.Dispose();
                    }
                    VlcMediaPlayer.Dispose();
                }

                _lockCallbackHandle.Free();
                _unlockCallbackHandle.Free();
                _displayCallbackHandle.Free();
                _formatCallbackHandle.Free();
                _cleanupCallbackHandle.Free();

                _disposed = true;
            });
        }

        /// <summary>
        /// Cleanup the player used resource.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion --- Cleanup ---

        #region --- Methods ---

        #region Path Helpers

        internal static String CombinePath(String path1, String path2)
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(path1, path2));
            return dir.FullName;
        }

        internal static bool IsRootPath(string path)
        {
            path = path.Replace('\\', '/');
            if (path[path.Length - 1] != '/')
            {
                path += '/';
            }
            int index;
            return (index = path.IndexOf(":/", StringComparison.Ordinal)) != -1 && index + 2 == path.Length;
        }

        internal static bool IsNetwork(Uri uri)
        {
            return uri.Scheme.ToLower() != "file";
        }

        #endregion Path Helpers

        #region Property Helpers

        private Size GetScaleTransform()
        {
            if (_context == null) return new Size(1.0, 1.0);

            AspectRatio aspectRatio = AspectRatio.Default;

            Dispatcher.Invoke(new Action(() =>
            {
                aspectRatio = AspectRatio;
            }));

            Size scale = new Size(_context.DisplayWidth / _context.Width, _context.DisplayHeight / _context.Height);

            switch (aspectRatio)
            {
                case AspectRatio.Default:
                    return scale;

                case AspectRatio._16_9:
                    return new Size(1.0 * _context.DisplayHeight / 9 * 16 / _context.Width, 1.0 * _context.DisplayHeight / _context.Height);

                case AspectRatio._4_3:
                    return new Size(1.0 * _context.DisplayHeight / 3 * 4 / _context.Width, 1.0 * _context.DisplayHeight / _context.Height);
            }
            return new Size(1.0, 1.0);
        }

        #endregion Property Helpers

        #region Coordinate Helpers

        private int GetVideoPositionX(double x)
        {
            if (_context == null)
            {
                return (int)x;
            }
            double width = _context.Width * ScaleTransform.ScaleX,
                height = _context.Height * ScaleTransform.ScaleY;
            var px = 0;
            double scale, scaleX, scaleY;
            switch (this.Stretch)
            {
                case Stretch.None:
                    switch (this.HorizontalContentAlignment)
                    {
                        case HorizontalAlignment.Left:
                            px = (int)x;
                            break;

                        case HorizontalAlignment.Center:
                            px = (int)(x - ((this.ActualWidth - width) / 2));
                            break;

                        case HorizontalAlignment.Right:
                            px = (int)(x - (this.ActualWidth - width));
                            break;

                        case HorizontalAlignment.Stretch:
                            if (this.ActualWidth > width)
                            {
                                px = (int)(x - ((this.ActualWidth - width) / 2));
                            }
                            else
                            {
                                px = (int)x;
                            }
                            break;
                    }
                    break;

                case Stretch.Fill:
                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (this.ActualWidth > width)
                            {
                                px = (int)(x / this.ActualWidth * width);
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (this.ActualWidth < width)
                            {
                                px = (int)(x / this.ActualWidth * width);
                            }
                            break;

                        case StretchDirection.Both:
                            px = (int)(x / this.ActualWidth * width);
                            break;
                    }
                    break;

                case Stretch.Uniform:
                    scaleX = this.ActualWidth / width;
                    scaleY = this.ActualHeight / height;
                    scale = Math.Min(scaleX, scaleY);

                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (this.HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((this.ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (this.HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((this.ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleX == scale)
                            {
                                px = (int)(x / scale);
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)(x / scale);
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                        break;
                                }
                            }
                            break;
                    }
                    break;

                case Stretch.UniformToFill:
                    scaleX = ActualWidth / width;
                    scaleY = ActualHeight / height;
                    scale = Math.Max(scaleX, scaleY);

                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (this.HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)(x / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((this.ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (this.ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (this.ActualWidth > width)
                                        {
                                            px = (int)(x - ((this.ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int)(x / scale);
                                }
                                else
                                {
                                    switch (HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int)(x / scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int)((x - ((ActualWidth - width * scale) / 2)) / scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int)((x - (ActualWidth - width * scale)) / scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int)(x / scale);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)(x - ((ActualWidth - width) / 2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)(x - (ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (ActualWidth > width)
                                        {
                                            px = (int)(x - ((ActualWidth - width) / 2));
                                        }
                                        else
                                        {
                                            px = (int)x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleX == scale)
                            {
                                px = (int)(x / scale);
                            }
                            else
                            {
                                switch (this.HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int)(x / scale);
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int)((x - ((this.ActualWidth - width * scale) / 2)) / scale);
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int)((x - (this.ActualWidth - width * scale)) / scale);
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        px = (int)(x / scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return px;
        }

        private int GetVideoPositionY(double y)
        {
            double width = _context.Width * ScaleTransform.ScaleX,
                height = _context.Height * ScaleTransform.ScaleY;
            int py = 0;
            double scale, scaleX, scaleY;
            switch (this.Stretch)
            {
                case Stretch.None:
                    switch (this.VerticalContentAlignment)
                    {
                        case VerticalAlignment.Top:
                            py = (int)y;
                            break;

                        case VerticalAlignment.Center:
                            py = (int)(y - ((this.ActualHeight - height) / 2));
                            break;

                        case VerticalAlignment.Bottom:
                            py = (int)(y - (this.ActualHeight - height));
                            break;

                        case VerticalAlignment.Stretch:
                            if (this.ActualHeight > height)
                            {
                                py = (int)(y - ((this.ActualHeight - height) / 2));
                            }
                            else
                            {
                                py = (int)y;
                            }
                            break;
                    }
                    break;

                case Stretch.Fill:
                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (this.ActualHeight > height)
                            {
                                py = (int)(y / this.ActualHeight * height);
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (this.ActualHeight < height)
                            {
                                py = (int)(y / this.ActualHeight * height);
                            }
                            break;

                        case StretchDirection.Both:
                            py = (int)(y / this.ActualHeight * height);
                            break;
                    }
                    break;

                case Stretch.Uniform:
                    scaleX = this.ActualWidth / width;
                    scaleY = this.ActualHeight / height;
                    scale = Math.Min(scaleX, scaleY);

                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleY == scale)
                            {
                                py = (int)(y / scale);
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)(y / scale);
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                        break;

                                    case VerticalAlignment.Stretch:
                                        py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;

                case Stretch.UniformToFill:
                    scaleX = this.ActualWidth / width;
                    scaleY = this.ActualHeight / height;
                    scale = Math.Max(scaleX, scaleY);

                    switch (this.StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)(y / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int)(y / scale);
                                }
                                else
                                {
                                    switch (this.VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int)(y / scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int)(y / scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)(y - ((this.ActualHeight - height) / 2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)(y - (this.ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (this.ActualHeight > height)
                                        {
                                            py = (int)(y - ((this.ActualHeight - height) / 2));
                                        }
                                        else
                                        {
                                            py = (int)y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleY == scale)
                            {
                                py = (int)(y / scale);
                            }
                            else
                            {
                                switch (this.VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int)(y / scale);
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int)((y - ((this.ActualHeight - height * scale) / 2)) / scale);
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int)((y - (this.ActualHeight - height * scale)) / scale);
                                        break;

                                    case VerticalAlignment.Stretch:
                                        py = (int)(y / scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return py;
        }

        #endregion Coordinate Helpers

        #region LoadMedia

        //note: if you pass a string instead of a Uri, LoadMedia will see if it is an absolute Uri, else will treat it as a file path
        /// <summary>
        /// Load a media by file path.
        /// </summary>
        /// <param name="path"></param>
        public void LoadMedia(String path)
        {
            Uri uri;
            if (Uri.TryCreate(path, UriKind.Absolute, out uri))
            {
                LoadMedia(uri);
                return;
            }

            if (!(File.Exists(path) || IsRootPath(Path.GetFullPath(path))))
                throw new FileNotFoundException(String.Format("Not found: {0}", path), path);

            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFromPath(path);
            VlcMediaPlayer.Media.ParseAsync();
        }

        /// <summary>
        /// Load a media by uri.
        /// </summary>
        /// <param name="uri"></param>
        public void LoadMedia(Uri uri)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null) VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFromLocation(uri.ToString());
            VlcMediaPlayer.Media.ParseAsync();
        }

        /// <summary>
        /// Load a media by file path and options.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="options"></param>
        public void LoadMediaWithOptions(String path, params String[] options)
        {
            if (!(File.Exists(path) || IsRootPath(Path.GetFullPath(path))))
                throw new FileNotFoundException(String.Format("Not found: {0}", path), path);

            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFromPath(path);
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
        }

        /// <summary>
        /// Load a media by uri and options.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="options"></param>
        public void LoadMediaWithOptions(Uri uri, params String[] options)
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.Dispose();

            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }

            VlcMediaPlayer.Media = ApiManager.Vlc.CreateMediaFromLocation(uri.ToString());
            VlcMediaPlayer.Media.AddOption(options);
            VlcMediaPlayer.Media.ParseAsync();
        }

        #endregion LoadMedia

        #region Play/Pause

        /// <summary>
        /// Play media.
        /// </summary>
        public void Play()
        {
            if (VlcMediaPlayer == null) return;

            if (_context != null)
            {
                VideoSource = _context.Image;
            }

            VlcMediaPlayer.Play();
        }

        /// <summary>
        /// Pause or resume media.
        /// </summary>
        public void PauseOrResume()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.PauseOrResume();
        }

        #endregion Play/Pause

        #region Stop

        /// <summary>
        /// Async stop a media by callback, callback will be invoked after media is stoped.
        /// </summary>
        /// <param name="callback"></param>
        public void BeginStop(Action callback)
        {
            if (VlcMediaPlayer == null || VlcMediaPlayer.Media == null)
            {
                callback();
                return;
            }

            _stopRequest = new StopRequest(this, () =>
            {
                _stopRequest = null;

                callback();

                VideoSource = null;
            });
            _stopRequest.Send();
        }

        /// <summary>
        /// Stop a media, please don't call any media operation after this method, if you want to do it, please use <see cref="BeginStop"/> or <see cref="StopAsync"/>.
        /// </summary>
        public void Stop()
        {
            if ((VlcMediaPlayer == null) || (VlcMediaPlayer.Media == null)) return;

            _stopRequest = new StopRequest(this, () =>
            {
                _stopRequest = null;

                VideoSource = null;
            });
            _stopRequest.Send();
        }

#if DotNet45
        /// <summary>
        /// Async stop a media by async/await keyword.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task StopAsync()
        {
            if (VlcMediaPlayer == null) return;

            if (VlcMediaPlayer.Media == null) return;

            EventWaitHandle stopWaitHandle = new ManualResetEvent(false);
            stopWaitHandle.Reset();

            BeginStop(() =>
            {
                stopWaitHandle.Set();
            });

            await System.Threading.Tasks.Task.Run(() =>
            {
                stopWaitHandle.WaitOne();
            });

            VideoSource = null;
        }

#endif

        #endregion Stop

        /// <summary>
        /// Add options to media.
        /// </summary>
        /// <param name="option"></param>
        public void AddOption(params String[] option)
        {
            if ((VlcMediaPlayer != null) && VlcMediaPlayer.Media != null)
                VlcMediaPlayer.Media.AddOption(option);
        }

        /// <summary>
        /// Show next frame.
        /// </summary>
        public void NextFrame()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.NextFrame();
        }

        /// <summary>
        /// Inactive with DVD menu.
        /// </summary>
        /// <param name="mode"></param>
        public void Navigate(NavigateMode mode)
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.Navigate(mode);
        }

        /// <summary>
        /// Toggle mute mode.
        /// </summary>
        public void ToggleMute()
        {
            if (VlcMediaPlayer != null)
                VlcMediaPlayer.ToggleMute();
        }

        /// <summary>
        /// Take a snapshot.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <param name="quality"></param>
        public void TakeSnapshot(String path, SnapshotFormat format, int quality)
        {
            if (VlcMediaPlayer != null)
                switch (VlcMediaPlayer.State)
                {
                    case MediaState.NothingSpecial:
                    case MediaState.Opening:
                    case MediaState.Buffering:
                    case MediaState.Stopped:
                    case MediaState.Ended:
                    case MediaState.Error:
                        break;

                    case MediaState.Playing:
                    case MediaState.Paused:
                        _snapshotContext = new SnapshotContext(path, format, quality);
                        break;
                }
        }

        #endregion --- Methods ---

        #region --- NotifyPropertyChanged ---

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            if (PropertyChanged != null)
            {
                var bodyExpr = expr.Body as MemberExpression;
                var propInfo = bodyExpr.Member as PropertyInfo;
                var propName = propInfo.Name;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        #endregion --- NotifyPropertyChanged ---
    }
}