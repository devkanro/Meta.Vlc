// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: ThreadSeparatedImage.cs
// Version: 20160327

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Meta.Vlc.Wpf
{
    public sealed class ThreadSeparatedImage : ThreadSeparatedControlHost
    {
        private static Dispatcher _commonDispatcher;
        public static Dispatcher CommonDispatcher
        {
            get
            {
                if (_commonDispatcher == null)
                {
                    Thread separateThread = new Thread(() =>
                    {
                        Dispatcher.Run();
                    })
                    {
                        IsBackground = true
                    };
                    separateThread.SetApartmentState(ApartmentState.STA);
                    separateThread.Priority = ThreadPriority.Highest;

                    separateThread.Start();

                    while (Dispatcher.FromThread(separateThread) == null)
                    {
                        Thread.Sleep(50);
                    }
                    _commonDispatcher = Dispatcher.FromThread(separateThread);
                }

                return _commonDispatcher;
            }
        }

        private HorizontalAlignment _horizontalContentAlignment = default(HorizontalAlignment);

        private ScaleTransform _scaleTransform;

        private ImageSource _source;

        private Stretch _stretch = Stretch.Uniform;

        private StretchDirection _stretchDirection = StretchDirection.Both;

        private VerticalAlignment _verticalContentAlignment = default(VerticalAlignment);

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

        protected override void LoadThreadSeparatedControl()
        {
            HostVisual = new HostVisual();

            AddLogicalChild(HostVisual);
            AddVisualChild(HostVisual);

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            CommonDispatcher.BeginInvoke(new Action(() =>
            {
                   TargetElement = CreateThreadSeparatedControl();

                if (TargetElement == null) return;

                VisualTarget = new VisualTargetPresentationSource(HostVisual);
                VisualTarget.RootVisual = TargetElement;

                Dispatcher.BeginInvoke(new Action(() => { InvalidateMeasure(); }));
            }));
        }

        protected override void UnloadThreadSeparatedControl()
        {
            RemoveLogicalChild(HostVisual);
            RemoveVisualChild(HostVisual);

            HostVisual = null;
            TargetElement = null;
        }
    }
}