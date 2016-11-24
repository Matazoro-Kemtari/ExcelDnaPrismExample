using MvvmProject.View.AttachedProperties;
using Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace MvvmProject.PrismExtensions.Adapters
{
    /// <summary>
    /// This is heavily based on the solution on
    /// http://southworks.com/blog/2008/09/26/windowregionadapter-for-compositewpf-prism/
    /// with only a few small modifications for positioning/sizing etc.
    /// </summary>
    public class PopupRegionAdapter : RegionAdapterBase<UserControl>
    {
        private readonly IntPtr OwnerWindow;

        public PopupRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory, IntPtr OwnerWindow)
            : base(regionBehaviorFactory)
        {
            this.OwnerWindow = OwnerWindow;
        }

        protected override void Adapt(IRegion region, UserControl regionTarget)
        {
        }

        protected override IRegion CreateRegion()
        {
            return new Region();
        }

        protected override void AttachBehaviors(IRegion region, UserControl regionTarget)
        {
            base.AttachBehaviors(region, regionTarget);

            WindowRegionBehavior behavior = new WindowRegionBehavior(regionTarget, region, WindowStyle, OwnerWindow);
            behavior.Attach();
        }

        public Style WindowStyle { get; set; }

        private class WindowRegionBehavior
        {
            private readonly WeakReference _regionWeakReference;
            private readonly Style _windowStyle;
            private readonly IntPtr OwnerWindow;
            protected readonly ConditionalWeakTable<object, Window> _Windows;

            internal WindowRegionBehavior(UserControl owner, IRegion region, Style windowStyle, IntPtr OwnerWindow)
            {
                this._Windows = new ConditionalWeakTable<object, Window>();
                _regionWeakReference = new WeakReference(region);
                _windowStyle = windowStyle;
                this.OwnerWindow = OwnerWindow;
            }

            internal void Attach()
            {
                IRegion region = _regionWeakReference.Target as IRegion;

                if (region != null)
                {
 
                    region.Views.CollectionChanged += new NotifyCollectionChangedEventHandler(Views_CollectionChanged);
                    region.ActiveViews.CollectionChanged += new NotifyCollectionChangedEventHandler(ActiveViews_CollectionChanged);
                }
            }

            internal void Detach()
            {
                IRegion region = _regionWeakReference.Target as IRegion;

                if (region != null)
                {
                    region.Views.CollectionChanged -= Views_CollectionChanged;
                    region.ActiveViews.CollectionChanged -= ActiveViews_CollectionChanged;
                }
            }

            private void window_Activated(object sender, EventArgs e)
            {
                IRegion region = _regionWeakReference.Target as IRegion;
                Window window = sender as Window;

                if (window != null && !region.ActiveViews.Contains(window.Content))
                    region.Activate(window.Content);
            }

            private void window_Deactivated(object sender, EventArgs e)
            {
                IRegion region = _regionWeakReference.Target as IRegion;
                Window window = sender as Window;

                if (window != null)
                    region.Deactivate(window.Content);
            }

            private void window_Closed(object sender, EventArgs e)
            {
                Window window = sender as Window;
                IRegion region = _regionWeakReference.Target as IRegion;

                if (window != null && region != null)
                    if (region.Views.Contains(window.Content))
                        region.Remove(window.Content);
            }


            private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (object view in e.NewItems)
                    {
                        Window window;
                        if (_Windows.TryGetValue(view, out window))
                        {
                            if (window != null && !window.IsFocused)
                            {
                                window.WindowState = WindowState.Normal;
                                window.Activate();
                            }
                        }
                    }
                }
            }

            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (object view in e.NewItems)
                    {
                            Window window = new Window();
                            window.Activated += this.window_Activated;
                            window.Deactivated += this.window_Deactivated;
                            window.Style = _windowStyle;
                            window.Content = view;

                            var dView = view as DependencyObject;
                            if(dView != null)
                            {
                                window.Width = DesiredSize.GetDesiredWidth(dView);
                                window.Height = DesiredSize.GetDesiredHeight(dView);
                            }
     
                            window.Closed += this.window_Closed;

                            if (this.OwnerWindow != null && this.OwnerWindow != IntPtr.Zero)
                            {
                                RECT rct = new RECT();
                                GetWindowRect(this.OwnerWindow, ref rct);

                                window.Left = rct.Left;
                                window.Top = rct.Top;

                                // Prefer not to set the owner, otherwise found to run into
                                // Airspace issues with WPF controls...
                                //new WindowInteropHelper(window).Owner = this.OwnerWindow;
                            }

 
                            this._Windows.Add(view, window);
                            window.Show();
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (object view in e.OldItems)
                    {
                        Window window;
                        if (_Windows.TryGetValue(view, out window))
                        {
                            if (window != null)
                                window.Close();

                            _Windows.Remove(view);

                        }
                    }
                }
            }

        }
    }
}
