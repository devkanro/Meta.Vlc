// Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
// Filename: VlcPlayer.DependencyProperties.cs
// Version: 20160214

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace xZune.Vlc.Wpf
{
    public partial class VlcPlayer
    {
        #region LibVlcPath

        /// <summary>
        ///     The path of LibVlc, it is a DependencyProperty.
        /// </summary>
        public String LibVlcPath
        {
            get { return (String) GetValue(LibVlcPathProperty); }
            set { SetValue(LibVlcPathProperty, value); }
        }

        public static readonly DependencyProperty LibVlcPathProperty =
            DependencyProperty.Register("LibVlcPath", typeof (String), typeof (VlcPlayer), null);

        #endregion LibVlcPath

        #region VlcOption

        /// <summary>
        ///     The options of LibVlc, it is a DependencyProperty.
        /// </summary>
        public String[] VlcOption
        {
            get { return (String[]) GetValue(VlcOptionProperty); }
            set { SetValue(VlcOptionProperty, value); }
        }

        public static readonly DependencyProperty VlcOptionProperty =
            DependencyProperty.Register("VlcOption", typeof (String[]), typeof (VlcPlayer), null);

        #endregion VlcOption

        #region ScaleTransform

        internal ScaleTransform _scaleTransform = null;
        internal ScaleTransform ScaleTransform
        {
            get { return _scaleTransform; }
            set {
                if (Image != null)
                {
                    Image.ScaleTransform = value;
                }

                if (_scaleTransform != value)
                {
                    _scaleTransform = value;
                    OnPropertyChanged(() => ScaleTransform);
                }
            }
        }

        #endregion ScaleTransform

        #region AspectRatio

        public static readonly DependencyProperty PropertyTypeProperty =
            DependencyProperty.Register("PropertyType", typeof (AspectRatio), typeof (VlcPlayer),
                new PropertyMetadata(AspectRatio.Default, OnAspectRatioChanged));

        /// <summary>
        ///     The aspect ratio of video, it is a DependencyProperty.
        /// </summary>
        public AspectRatio AspectRatio
        {
            get { return (AspectRatio) GetValue(PropertyTypeProperty); }
            set { SetValue(PropertyTypeProperty, value); }
        }

        private static void OnAspectRatioChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;

            var scale = vlcPlayer.GetScaleTransform();

            if (vlcPlayer.ImageDispatcher != null)
            {
                vlcPlayer.ImageDispatcher.BeginInvoke(new Action(() =>
                {
                    vlcPlayer.ScaleTransform = new ScaleTransform(scale.Width, scale.Height);
                }));
            }
        }

        #endregion AspectRatio

        #region VideoSource

        public BitmapSource _videoSource = null;
        /// <summary>
        ///     The image data of video, it is a DependencyProperty.
        /// </summary>
        public BitmapSource VideoSource
        {
            get { return _videoSource; }
            private set
            {
                if (Image != null)
                {
                    Image.Source = value;
                }

                if (_videoSource != value)
                {
                    _videoSource = value;
                    OnPropertyChanged(() => VideoSource);
                    if (VideoSourceChanged != null)
                    {
                        VideoSourceChanged(this, new VideoSourceChangedEventArgs(value));
                    }
                }
            }
        }

        public event EventHandler<VideoSourceChangedEventArgs> VideoSourceChanged;

        #endregion VideoSource

        #region Stretch

        /// <summary>
        ///     The stretch mode of video.
        /// </summary>
        public Stretch Stretch
        {
            get { return (Stretch) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof (Stretch), typeof (VlcPlayer),
                new PropertyMetadata(Stretch.Uniform));

        /// <summary>
        ///     The stretch direction of video.
        /// </summary>
        public StretchDirection StretchDirection
        {
            get { return (StretchDirection) GetValue(StretchDirectionProperty); }
            set { SetValue(StretchDirectionProperty, value); }
        }

        public static readonly DependencyProperty StretchDirectionProperty =
            DependencyProperty.Register("StretchDirection", typeof (StretchDirection), typeof (VlcPlayer),
                new PropertyMetadata(StretchDirection.Both));

        #endregion Stretch

        #region EndBehavior

        public static readonly DependencyProperty EndBehaviorProperty = DependencyProperty.Register(
            "EndBehavior", typeof (EndBehavior), typeof (VlcPlayer), new PropertyMetadata(EndBehavior.Default));

        public EndBehavior EndBehavior
        {
            get { return (EndBehavior) GetValue(EndBehaviorProperty); }
            set { SetValue(EndBehaviorProperty, value); }
        }

        #endregion EndBehavior
        
    }

    public class VideoSourceChangedEventArgs : EventArgs
    {
        public VideoSourceChangedEventArgs(ImageSource _newVideoSource)
        {
            NewVideoSource = _newVideoSource;
        }

        public ImageSource NewVideoSource { get; private set; }
    }
}