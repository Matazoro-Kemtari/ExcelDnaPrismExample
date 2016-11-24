using Microsoft.Practices.Prism.Mvvm;
using Prism.Modularity;
using Prism.Ninject;
using ExcelProject.Modules;
using ExcelProject.View;
using ExcelProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Ninject;
using Prism.Regions;
using ExcelProject.Extensions;

namespace ExcelProject
{
    internal class Bootstrapper : NinjectBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new PopupView();
            //return new Shell();
            //return new MainWindow();
        }

        protected override void InitializeShell()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                if (viewType == typeof(HelloWorldView))
                    return typeof(HelloWorldViewModel);

                if (viewType == typeof(SecondHelloWorldView))
                    return typeof(SecondHelloWorldViewModel);

                throw new NotImplementedException();
            });


            ViewModelLocationProvider.SetDefaultViewModelFactory((type) =>
                {
                    return this.Kernel.Get(type);
                });


            base.InitializeShell();

            //App.Current.MainWindow = (Window)this.Shell;
            //App.Current.MainWindow.Show();
        }

        protected override Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            RegionAdapterMappings regionAdapterMappings = Kernel.Get<RegionAdapterMappings>();

            if (regionAdapterMappings != null)
            {
                //regionAdapterMappings.RegisterMapping(typeof(Window), new WindowRegionAdapter(Kernel.Get < IRegionBehaviorFactory>()));
                //regionAdapterMappings.RegisterMapping(typeof(Shell), new ShellRegionAdapter(Kernel.Get<IRegionBehaviorFactory>()));
                regionAdapterMappings.RegisterMapping(typeof(PopupView), new ShellRegionAdapter(Kernel.Get<IRegionBehaviorFactory>()));
            }


            return base.ConfigureRegionAdapterMappings();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var behaviours = base.ConfigureDefaultRegionBehaviors();
            behaviours.AddIfMissing(RegionAwareBehaviour.RegionAwareBehaviourKey, typeof(RegionAwareBehaviour));
            return behaviours;
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var catalogue = (ModuleCatalog)this.ModuleCatalog;
            catalogue.AddModule(typeof(HelloWorldModule));
            catalogue.AddModule(typeof(SecondHelloWorldModule));
        }


     
    }
}
