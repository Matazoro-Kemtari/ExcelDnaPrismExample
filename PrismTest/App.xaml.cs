using Microsoft.Practices.Prism.Mvvm;
using ExcelProject.View;
using ExcelProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Windows;

namespace ExcelProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

           
            //ViewModelLocationProvider.Register("HelloWorldView", createVM);

            StaTaskScheduler staTaskScheduler = new StaTaskScheduler(1);

             Task.Factory.StartNew(
                () =>
                    {
                        var bootstrapper = new Bootstrapper();
                        bootstrapper.Run();
                        System.Windows.Threading.Dispatcher.Run();
                        },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        staTaskScheduler);
        }

        /*
        protected object createVM()
        {
            return new HelloWorldViewModel();
        }*/
    }
}
