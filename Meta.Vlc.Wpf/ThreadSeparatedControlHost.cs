// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: ThreadSeparatedControlHost.cs
// Version: 20160327

using System;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Meta.Vlc.Wpf
{
    public abstract class ThreadSeparatedControlHost : FrameworkElement
    {
        public FrameworkElement TargetElement { get; protected set; }
        public HostVisual HostVisual { get; protected set; }
        public VisualTargetPresentationSource VisualTarget { get; protected set; }

        public Dispatcher SeparateThreadDispatcher
        {
            get { return TargetElement == null ? null : TargetElement.Dispatcher; }
        }

        protected override int VisualChildrenCount
        {
            get { return HostVisual != null ? 1 : 0; }
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (HostVisual != null)
                {
                    yield return HostVisual;
                }
            }
        }

        protected abstract FrameworkElement CreateThreadSeparatedControl();

        protected virtual void LoadThreadSeparatedControl()
        {
            if (SeparateThreadDispatcher != null) return;

            AutoResetEvent sync = new AutoResetEvent(false);
            HostVisual = new HostVisual();

            AddLogicalChild(HostVisual);
            AddVisualChild(HostVisual);
            
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            var thread = new Thread(() =>
            {
                TargetElement = CreateThreadSeparatedControl();

                if (TargetElement == null) return;

                VisualTarget = new VisualTargetPresentationSource(HostVisual);
                VisualTarget.RootVisual = TargetElement;

                Dispatcher.BeginInvoke(new Action(() => { InvalidateMeasure(); }));

                sync.Set();

                Dispatcher.Run();

                VisualTarget.Dispose();
            })
            {
                IsBackground = true
            };

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            sync.WaitOne();
        }

        protected virtual void UnloadThreadSeparatedControl()
        {
            if (SeparateThreadDispatcher == null) return;

            SeparateThreadDispatcher.InvokeShutdown();

            RemoveLogicalChild(HostVisual);
            RemoveVisualChild(HostVisual);

            HostVisual = null;
            TargetElement = null;
        }

        protected override void OnInitialized(EventArgs e)
        {
            LoadThreadSeparatedControl();

            Unloaded += (sender, args) => { UnloadThreadSeparatedControl(); };

            base.OnInitialized(e);
        }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint)
        {
            var uiSize = new System.Windows.Size();

            if (TargetElement != null)
            {
                TargetElement.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => TargetElement.Measure(constraint)));
                uiSize = TargetElement.DesiredSize;
            }

            return uiSize;
        }

        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize)
        {
            if (TargetElement != null)
            {
                TargetElement.Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() => TargetElement.Arrange(new Rect(finalSize))));
            }

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return HostVisual;
            }

            throw new IndexOutOfRangeException("index");
        }
    }
}