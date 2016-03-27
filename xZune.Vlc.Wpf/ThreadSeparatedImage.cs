// Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
// Filename: ThreadSeparatedImage.cs
// Version: 20160327

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace xZune.Vlc.Wpf
{
    public sealed class ThreadSeparatedImage : ThreadSeparatedControlHost
    {
        private HorizontalAlignment _horizontalContentAlignment = HorizontalAlignment.Stretch;

        private ScaleTransform _scaleTransform;

        private ImageSource _source;

        private Stretch _stretch = Stretch.Uniform;

        private StretchDirection _stretchDirection = StretchDirection.Both;

        private VerticalAlignment _verticalContentAlignment = VerticalAlignment.Stretch;

        public Image InternalImageControl { get; private set; }

        public ImageSource Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;

                    if (InternalImageControl == null) return;

                    SeparateThreadDispatcher.Invoke(new Action(() => { InternalImageControl.Source = value; }));
                }
            }
        }

        public Stretch Stretch
        {
            get { return _stretch; }
            set
            {
                if (_stretch != value)
                {
                    _stretch = value;

                    if (InternalImageControl == null) return;

                    SeparateThreadDispatcher.Invoke(new Action(() => { InternalImageControl.Stretch = value; }));
                }
            }
        }

        public StretchDirection StretchDirection
        {
            get { return _stretchDirection; }
            set
            {
                if (_stretchDirection != value)
                {
                    _stretchDirection = value;

                    if (InternalImageControl == null) return;

                    SeparateThreadDispatcher.Invoke(new Action(() => { InternalImageControl.StretchDirection = value; }));
                }
            }
        }

        public ScaleTransform ScaleTransform
        {
            get { return _scaleTransform; }
            set
            {
                if (_scaleTransform != value)
                {
                    _scaleTransform = value;

                    if (InternalImageControl == null) return;

                    SeparateThreadDispatcher.Invoke(new Action(() => { InternalImageControl.LayoutTransform = value; }));
                }
            }
        }

        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return _horizontalContentAlignment; }
            set
            {
                if (_horizontalContentAlignment != value)
                {
                    _horizontalContentAlignment = value;

                    if (InternalImageControl == null) return;

                    SeparateThreadDispatcher.Invoke(
                        new Action(() => { InternalImageControl.HorizontalAlignment = value; }));
                }
            }
        }

        public VerticalAlignment VerticalContentAlignment
        {
            get { return _verticalContentAlignment; }
            set
            {
                if (_verticalContentAlignment != value)
                {
                    _verticalContentAlignment = value;

                    if (InternalImageControl == null) return;

                    SeparateThreadDispatcher.Invoke(new Action(() => { InternalImageControl.VerticalAlignment = value; }));
                }
            }
        }

        protected override FrameworkElement CreateThreadSeparatedControl()
        {
            InternalImageControl = new Image();
            InternalImageControl.Source = Source;
            InternalImageControl.Stretch = Stretch;
            InternalImageControl.StretchDirection = StretchDirection;
            InternalImageControl.HorizontalAlignment = HorizontalContentAlignment;
            InternalImageControl.VerticalAlignment = VerticalContentAlignment;
            InternalImageControl.LayoutTransform = ScaleTransform;

            return InternalImageControl;
        }
    }
}