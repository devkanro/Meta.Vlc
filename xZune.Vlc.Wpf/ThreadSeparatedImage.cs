// Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
// Filename: ThreadSeparatedImage.cs
// Version: 20160324

using System;
using System.Collections;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace xZune.Vlc.Wpf
{
	public class ThreadSeparatedImage : FrameworkElement
	{
		#region fields
		private readonly AutoResetEvent _resentEvent;
		private HostVisual _hostVisual;
		#endregion

		#region properties

        public Dispatcher ThreadSeparatedDispatcher { get; protected set; }
        public Image InternalImageControl { get; protected set; }

	    private ImageSource _source = null;

        public ImageSource Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    ThreadSeparatedDispatcher.Invoke(new Action(() =>
                    {
                        InternalImageControl.Source = value;
                    }));
                }
            }
	    }

	    public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
	        "Stretch", typeof (Stretch), typeof (ThreadSeparatedImage), new PropertyMetadata(default(Stretch), OnStretchChanged));

        private static void OnStretchChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ThreadSeparatedImage @this = (ThreadSeparatedImage)dependencyObject;

            if (@this.InternalImageControl != null)
            {
                @this.ThreadSeparatedDispatcher.Invoke(new Action(() =>
                {
                    @this.InternalImageControl.Stretch = (Stretch)dependencyPropertyChangedEventArgs.NewValue;
                }));
            }
        }

        public Stretch Stretch
	    {
	        get { return (Stretch) GetValue(StretchProperty); }
	        set { SetValue(StretchProperty, value); }
	    }

	    public static readonly DependencyProperty StretchDirectionProperty = DependencyProperty.Register(
	        "StretchDirection", typeof (StretchDirection), typeof (ThreadSeparatedImage), new PropertyMetadata(default(StretchDirection), OnStretchDirectionChanged));

        private static void OnStretchDirectionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ThreadSeparatedImage @this = (ThreadSeparatedImage)dependencyObject;

            if (@this.InternalImageControl != null)
            {
                @this.ThreadSeparatedDispatcher.Invoke(new Action(() =>
                {
                    @this.InternalImageControl.StretchDirection = (StretchDirection)dependencyPropertyChangedEventArgs.NewValue;
                }));
            }
        }

        public StretchDirection StretchDirection
	    {
	        get { return (StretchDirection) GetValue(StretchDirectionProperty); }
	        set { SetValue(StretchDirectionProperty, value); }
	    }


        private ScaleTransform _scaleTransform = null;
        public ScaleTransform ScaleTransform
        {
            get { return _scaleTransform; }
            set
            {
                if (_scaleTransform != value)
                {
                    _scaleTransform = value;
                    ThreadSeparatedDispatcher.Invoke(new Action(() =>
                    {
                        InternalImageControl.LayoutTransform = value;
                    }));
                }
            }
        }

        public static readonly DependencyProperty HorizontalAlignmentProperty = DependencyProperty.Register(
	        "HorizontalAlignment", typeof (HorizontalAlignment), typeof (ThreadSeparatedImage), new PropertyMetadata(default(HorizontalAlignment), OnHorizontalAlignmentChanged));

        private static void OnHorizontalAlignmentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ThreadSeparatedImage @this = (ThreadSeparatedImage)dependencyObject;

            if (@this.InternalImageControl != null)
            {
                @this.ThreadSeparatedDispatcher.Invoke(new Action(() =>
                {
                    @this.InternalImageControl.HorizontalAlignment = (HorizontalAlignment)dependencyPropertyChangedEventArgs.NewValue;
                }));
            }
        }

        public HorizontalAlignment HorizontalAlignment
        {
	        get { return (HorizontalAlignment) GetValue(HorizontalAlignmentProperty); }
	        set { SetValue(HorizontalAlignmentProperty, value); }
	    }

	    public static readonly DependencyProperty VerticalAlignmentProperty = DependencyProperty.Register(
	        "VerticalAlignment", typeof (VerticalAlignment), typeof (ThreadSeparatedImage), new PropertyMetadata(default(VerticalAlignment), OnVerticalAlignmentChanged));

        private static void OnVerticalAlignmentChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ThreadSeparatedImage @this = (ThreadSeparatedImage)dependencyObject;

            if (@this.InternalImageControl != null)
            {
                @this.ThreadSeparatedDispatcher.Invoke(new Action(() =>
                {
                    @this.InternalImageControl.VerticalAlignment = (VerticalAlignment)dependencyPropertyChangedEventArgs.NewValue;
                }));
            }
        }

        public VerticalAlignment VerticalAlignment
        {
	        get { return (VerticalAlignment) GetValue(VerticalAlignmentProperty); }
	        set { SetValue(VerticalAlignmentProperty, value); }
	    }

		#endregion

		#region constructor

		public ThreadSeparatedImage()
		{
			_resentEvent = new AutoResetEvent( false );

			Initialized += UiThreadSeparatedBase_Initialized;
		}

        #endregion

        #region protected

        ImageSource imageSource = null;
        Stretch stretch = Stretch.None;
        StretchDirection stretchDirection = StretchDirection.Both;
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;
        VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        ScaleTransform scaleTransform = null;

        private void CreateImage()
		{
            InternalImageControl = new Image();
		    InternalImageControl.Source = imageSource;
		    InternalImageControl.Stretch = stretch;
		    InternalImageControl.StretchDirection = stretchDirection;
		    InternalImageControl.HorizontalAlignment = horizontalAlignment;
		    InternalImageControl.VerticalAlignment = verticalAlignment;
		    InternalImageControl.LayoutTransform = scaleTransform;
		}

		protected virtual void CreateThreadSeparatedElement()
		{
			_hostVisual = new HostVisual();

			AddLogicalChild( _hostVisual );
			AddVisualChild( _hostVisual );

			// Spin up a worker thread, and pass it the HostVisual that it
			// should be part of.
		    var thread = new Thread(CreateContentOnSeparateThread);
		    thread.Name = "VlcDisplayThread";

            thread.SetApartmentState( ApartmentState.STA );
            thread.Priority = ThreadPriority.Highest;

            imageSource = Source;
            stretch = Stretch;
            stretchDirection = StretchDirection;
            StretchDirection = StretchDirection;
            horizontalAlignment = HorizontalAlignment;
            verticalAlignment = VerticalAlignment;
            scaleTransform = ScaleTransform;

            thread.Start();

			// Wait for the worker thread to spin up and create the VisualTarget.
			_resentEvent.WaitOne();

			InvalidateMeasure();
		}

		protected virtual void DestroyThreadSeparatedElement()
		{
			if ( ThreadSeparatedDispatcher != null )
			{
                ThreadSeparatedDispatcher.InvokeShutdown();

				RemoveLogicalChild( _hostVisual );
				RemoveVisualChild( _hostVisual );

				_hostVisual = null;
				InternalImageControl = null;
                ThreadSeparatedDispatcher = null;
			}
		}

		#endregion

		#region private

		private void CreateContentOnSeparateThread()
		{
			if ( _hostVisual != null )
			{
				// Create the VisualTargetPresentationSource and then signal the
				// calling thread, so that it can continue without waiting for us.
				var visualTarget = new VisualTargetPresentationSource( _hostVisual );

                CreateImage();

                ThreadSeparatedDispatcher = InternalImageControl.Dispatcher;

				_resentEvent.Set();

                visualTarget.RootVisual = InternalImageControl;
                
                // Run a dispatcher for this worker thread.  This is the central
                // processing loop for WPF.
                Dispatcher.Run();

                visualTarget.Dispose();
			}
		}

		private void UiThreadSeparatedBase_Initialized( object sender, EventArgs e )
        {
            CreateThreadSeparatedElement();
        }

		#endregion

		#region overrides

		protected override int VisualChildrenCount
		{
			get
			{
				return _hostVisual != null ? 1 : 0;
			}
		}

		protected override IEnumerator LogicalChildren
		{
			get
			{
				if ( _hostVisual != null )
				{
					yield return _hostVisual;
				}
			}
		}

		protected override Visual GetVisualChild( int index )
		{
			if ( index == 0 )
			{
                return _hostVisual;
            }

			throw new IndexOutOfRangeException( "index" );
		}

		protected override System.Windows.Size MeasureOverride( System.Windows.Size constraint )
		{
			var childSize = new System.Windows.Size();
			var uiSize = new System.Windows.Size();

			if ( InternalImageControl != null )
			{
                InternalImageControl.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => InternalImageControl.Measure( constraint ) ) );
				uiSize.Width = InternalImageControl.ActualWidth;
				uiSize.Height = InternalImageControl.ActualHeight;
			}

			var size = new System.Windows.Size( Math.Max( childSize.Width, uiSize.Width ), Math.Max( childSize.Height, uiSize.Height ) ); ;
			return size;
		}

		protected override System.Windows.Size ArrangeOverride( System.Windows.Size finalSize )
		{
            if (InternalImageControl != null )
			{
                InternalImageControl.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => InternalImageControl.Arrange( new Rect( finalSize ) ) ) );
			}

			return finalSize;
		}

		#endregion
	}

    public class VisualTargetPresentationSource : PresentationSource, IDisposable
    {
        private readonly VisualTarget _visualTarget;

        public VisualTargetPresentationSource(HostVisual hostVisual)
        {
            _visualTarget = new VisualTarget(hostVisual);
            AddSource();
        }

        public override Visual RootVisual
        {
            get
            {
                try
                {
                    return _visualTarget.RootVisual;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            set
            {
                Visual oldRoot = _visualTarget.RootVisual;

                // Set the root visual of the VisualTarget.  This visual will
                // now be used to visually compose the scene.
                _visualTarget.RootVisual = value;

                // Tell the PresentationSource that the root visual has
                // changed.  This kicks off a bunch of stuff like the
                // Loaded event.
                RootChanged(oldRoot, value);

                // Kickoff layout...
                UIElement rootElement = value as UIElement;
                if (rootElement != null)
                {
                    rootElement.Measure(new System.Windows.Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    rootElement.Arrange(new Rect(rootElement.DesiredSize));
                }
            }
        }

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return _visualTarget;
        }


        private bool _isDisposed;
        public override bool IsDisposed
        {
            get { return _isDisposed; }
        }

        public void Dispose()
        {
            RemoveSource();
            _isDisposed = true;
        }
    }
}