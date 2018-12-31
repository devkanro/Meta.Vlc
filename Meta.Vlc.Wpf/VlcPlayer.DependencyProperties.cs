// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcPlayer.DependencyProperties.cs
// Version: 20181231

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Meta.Vlc.Wpf
{
    public partial class VlcPlayer
    {
        #region LibVlcPath

        /// <summary>
        ///     The path of LibVlc, it is a DependencyProperty.
        /// </summary>
        public string LibVlcPath
        {
            get => (string) GetValue(LibVlcPathProperty);
            set => SetValue(LibVlcPathProperty, value);
        }

        public static readonly DependencyProperty LibVlcPathProperty =
            DependencyProperty.Register("LibVlcPath", typeof(string), typeof(VlcPlayer), null);

        #endregion LibVlcPath

        #region VlcOption

        /// <summary>
        ///     The options of LibVlc, it is a DependencyProperty.
        /// </summary>
        public IList<string> VlcOption
        {
            get => (IList<string>) GetValue(VlcOptionProperty);
            set => SetValue(VlcOptionProperty, value);
        }

        public static readonly DependencyProperty VlcOptionProperty =
            DependencyProperty.Register("VlcOption", typeof(IList<string>), typeof(VlcPlayer),
                new PropertyMetadata(new List<string>()));

        #endregion VlcOption

        #region AspectRatio

        public static readonly DependencyProperty PropertyTypeProperty =
            DependencyProperty.Register("PropertyType", typeof(AspectRatio), typeof(VlcPlayer),
                new PropertyMetadata(AspectRatio.Default, OnAspectRatioChanged));

        /// <summary>
        ///     The aspect ratio of video, it is a DependencyProperty.
        /// </summary>
        public AspectRatio AspectRatio
        {
            get => (AspectRatio) GetValue(PropertyTypeProperty);
            set => SetValue(PropertyTypeProperty, value);
        }

        private static void OnAspectRatioChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var vlcPlayer = sender as VlcPlayer;

            var scale = vlcPlayer.GetScaleTransform();

            if (vlcPlayer.DisplayThreadDispatcher != null)
                vlcPlayer.DisplayThreadDispatcher.BeginInvoke(
                    new Action(() => { vlcPlayer.ScaleTransform = new ScaleTransform(scale.Width, scale.Height); }));
        }

        #endregion AspectRatio

        #region Stretch

        /// <summary>
        ///     The stretch mode of video.
        /// </summary>
        public Stretch Stretch
        {
            get => (Stretch) GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(VlcPlayer),
                new PropertyMetadata(Stretch.Uniform, (o, args) =>
                {
                    var @this = o as VlcPlayer;
                    if (@this == null)
                        return;
                    if (@this.Image != null) @this.Image.Stretch = (Stretch) args.NewValue;
                }));

        /// <summary>
        ///     The stretch direction of video.
        /// </summary>
        public StretchDirection StretchDirection
        {
            get => (StretchDirection) GetValue(StretchDirectionProperty);
            set => SetValue(StretchDirectionProperty, value);
        }

        public static readonly DependencyProperty StretchDirectionProperty =
            DependencyProperty.Register("StretchDirection", typeof(StretchDirection), typeof(VlcPlayer),
                new PropertyMetadata(StretchDirection.Both, (o, args) =>
                {
                    var @this = o as VlcPlayer;
                    if (@this == null)
                        return;

                    if (@this.Image != null) @this.Image.StretchDirection = (StretchDirection) args.NewValue;
                }));

        #endregion Stretch

        #region EndBehavior

        public static readonly DependencyProperty EndBehaviorProperty = DependencyProperty.Register(
            "EndBehavior", typeof(EndBehavior), typeof(VlcPlayer), new PropertyMetadata(EndBehavior.Default));

        public EndBehavior EndBehavior
        {
            get => (EndBehavior) GetValue(EndBehaviorProperty);
            set => SetValue(EndBehaviorProperty, value);
        }

        #endregion EndBehavior
    }

    public class VideoSourceChangedEventArgs : EventArgs
    {
        public VideoSourceChangedEventArgs(ImageSource _newVideoSource)
        {
            NewVideoSource = _newVideoSource;
        }

        public ImageSource NewVideoSource { get; }
    }
}