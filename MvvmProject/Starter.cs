using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Threading;
using Ninject;
using System.Windows;
using MvvmProject.View.AttachedProperties;
using System.Collections.Specialized;
using System.Reactive.Linq;
using Ninject.Extensions.Logging;

namespace MvvmProject
{
    public class Starter
    {
        private readonly TimeSpan InitializationTimeout = TimeSpan.FromSeconds(15);
        private ManualResetEventSlim _Initialized = new ManualResetEventSlim(false);  // Is triggered when the internal STA thread has been created and the bootstrapper has been initialized
        private readonly ILogger log;
        private Dispatcher GuiDispatcher;
        
        internal Bootstrapper _Bootstrapper;

        public Starter(ILogger log, IntPtr OwnerWindow)
        {
            log.Debug("Creating Starter-object");
            this.log = log;

            StaTaskScheduler staTaskScheduler = new StaTaskScheduler(1);

            // We'll create a new STA Thread, and run the Bootstrapper in it
            Task.Factory.StartNew(
               () =>
               {
                   log.Info("Starting STA Thread");

                   try
                   {
                       var bootstrapper = new Bootstrapper(OwnerWindow);
                       this._Bootstrapper = bootstrapper;
                       bootstrapper.Run();
                   } catch (Exception ex)
                   {
                       log.Error("Error creating/running bootstrapper; {0}", ex);
                       throw;
                   }
                   
                   this.GuiDispatcher = Dispatcher.CurrentDispatcher;

                   _Initialized.Set();
                   log.Info("STA Thread initialized");

                   System.Windows.Threading.Dispatcher.Run();
               },  CancellationToken.None, TaskCreationOptions.None, staTaskScheduler);
        }


        /// <summary>
        /// Shows a View in Modal mode:
        /// Blocks the calling thread until the view is closed
        /// </summary>
        public void ShowModal<T>(int? width = null, int? height = null)
            where T : DependencyObject, IView
        {
            // We'll need to call into the STA thread; however this function is (probably)
            // being called from the Excel thread. So first of all, we need to make sure the STA
            // thread has been properly created and initialized (or wait on the calling thread until this has happened)
            if (!_Initialized.Wait(InitializationTimeout))
                throw new TimeoutException("Initialization failed");

            log.Debug("Showing view '{0}' in modal mode", typeof(T).FullName);

            if (GuiDispatcher.CheckAccess())
            {
                throw new InvalidOperationException("ShowModal cannot be called from the GUI/STA Thread");
            }
            
            Action OnViewClosed = () =>
            {
                log.Debug("View '{0}' closed; unblocking Excel", typeof(T).FullName);
                ExcelUtil.ApplicationState.Unblock();
                log.Debug("Unblocked Excel");
            };

            GuiDispatcher.Invoke(() => ShowOnGui<T>(width, height, OnViewClosed));
            ExcelUtil.ApplicationState.Block();
        }

        /// <summary>
        /// Shows the given view in non-modal mode
        /// </summary>
        public void Show<T>(int? width = null, int? height = null)
            where T : DependencyObject, IView
        {
            if (!_Initialized.Wait(InitializationTimeout))
                throw new TimeoutException("Initialization failed");

            log.Debug("Showing view '{0}' in non-modal mode", typeof(T).FullName);

            if (GuiDispatcher.CheckAccess())
            {
                // we are on the dispatcher's thread, so no need to call invoke
                ShowOnGui<T>(width, height);
            }
            else
            {
                GuiDispatcher.Invoke(() => ShowOnGui<T>(width, height));
            }
        }

        // This is being run on the GUI's Dispatcher (STA Thread) by contract
        protected void ShowOnGui<T>(int? width, int? height, Action OnViewClosed = null)
            where T : DependencyObject, IView
        {
            log.Debug("Creating view '{0}' on GUI Thread", typeof(T).FullName);

            var regionManager = _Bootstrapper.Kernel.Get<IRegionManager>();
            IRegion popupRegion = regionManager.Regions[RegionNames.PopupRegion];

            T view = default(T);

            try
            {
                view = _Bootstrapper.Kernel.Get<T>();
            } catch (Exception ex)
            {
                log.Error("Error instantiating view '{0}': {1}", typeof(T).FullName, ex);
                throw;
            }

            if (width != null)
                DesiredSize.SetDesiredWidth(view, (double)width);

            if (height != null)
                DesiredSize.SetDesiredHeight(view, (double)height);

            popupRegion.Add(view, null, true);

            if (OnViewClosed != null)
            {
                log.Debug("Subscribing to closed-event of view '{0}'", view);

                // We'll use PRISM to find out when the view has been closed, namely
                // by monitoring the IRegion.Views collection for any changes, and then checking if the newly created view has been removed from that collection
                Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(popupRegion.Views, "CollectionChanged")
                    .Where(p => p.EventArgs.Action == NotifyCollectionChangedAction.Remove && !((ViewsCollection)p.Sender).Contains(view))
                    .FirstAsync()
                    .Do(_ => log.Debug("Received Closed-notification for view '{0}'", view))
                    .Subscribe(_ => OnViewClosed());
            }
        }
    }
}
