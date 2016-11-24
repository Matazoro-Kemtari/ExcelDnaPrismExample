using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows.Threading;
using Ninject;
using Prism.Regions;

namespace ExcelProject
{
    public class AddinStarter
    {
        protected Dispatcher GuiDispatcher;
        internal Bootstrapper _Bootstrapper;
        protected ManualResetEventSlim _Initialized = new ManualResetEventSlim(false);

        public AddinStarter()
        {

            StaTaskScheduler staTaskScheduler = new StaTaskScheduler(1);

            Task.Factory.StartNew(
               () =>
               {
                   var bootstrapper = new Bootstrapper();
                   this._Bootstrapper = bootstrapper;
                   bootstrapper.Run();
                   
                   this.GuiDispatcher = Dispatcher.CurrentDispatcher;

                   _Initialized.Set();

                   System.Windows.Threading.Dispatcher.Run();
               },  CancellationToken.None, TaskCreationOptions.None, staTaskScheduler);
        }

        public void Show<T>()
            where T : IView
        {
            _Initialized.Wait();

            if (GuiDispatcher.CheckAccess())
            {
                // we are on the dispatcher's thread, so no need to call invoke
                ShowOnGui<T>();

            }
            else
            {
                GuiDispatcher.Invoke(ShowOnGui<T>);
            }
        }

        // This is being run on the GUI's Dispatcher
        protected void ShowOnGui<T>()
            where T : IView
        {
            var regionManager = _Bootstrapper.Kernel.Get<IRegionManager>();
            IRegion popupRegion = regionManager.Regions["PopupRegion"];
            IView view = _Bootstrapper.Kernel.Get<T>();
            popupRegion.Add(view, null, true);

            Console.WriteLine("success!");
        }
    }
}
