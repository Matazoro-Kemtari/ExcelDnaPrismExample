using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows;
using System.Windows.Controls;

namespace ExcelProject.Extensions
{
    public class ShellRegionAdapter : RegionAdapterBase<UserControl>
    {
 
        public ShellRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory)
            : base(regionBehaviorFactory)
        {
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

            WindowRegionBehavior behavior = new WindowRegionBehavior(regionTarget, region, WindowStyle);
            behavior.Attach();
        }

        public Style WindowStyle { get; set; }

        private class WindowRegionBehavior
        {
            private readonly WeakReference _regionWeakReference;
            private readonly Style _windowStyle;
            protected readonly ConditionalWeakTable<object, Window> _Windows;

            internal WindowRegionBehavior(UserControl owner, IRegion region, Style windowStyle)
            {
                this._Windows = new ConditionalWeakTable<object, Window>();
                _regionWeakReference = new WeakReference(region);
                _windowStyle = windowStyle;
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
                            window.Closed += this.window_Closed;
                            //window.Owner = owner;
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

                        }
                    }
                }
            }

        }
    }
}
