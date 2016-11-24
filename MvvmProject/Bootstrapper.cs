using MvvmProject.PrismExtensions.Adapters;
using MvvmProject.View.Internal;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Modularity;
using Prism.Ninject;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Ninject;
using System.Reflection;
using MvvmProject.View;
using MvvmProject.ViewModel;
using Ninject.Modules;
using Ninject.Extensions.Logging.Log4net;

namespace MvvmProject
{
    internal class Bootstrapper : NinjectBootstrapper
    {
        private readonly IntPtr OwnerWindow;

        public Bootstrapper(IntPtr OwnerWindow)
        {
            this.OwnerWindow = OwnerWindow;
        }

        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override Ninject.IKernel CreateKernel()
        {
            ConfigureLog();
            var settings = new NinjectSettings { LoadExtensions = false };
            return new StandardKernel(settings, new INinjectModule[] { new Log4NetModule() });
        }

        protected void ConfigureLog()
        {
            if (!log4net.LogManager.GetRepository().Configured)
            {
                log4net.Config.XmlConfigurator.Configure();
            }
        }

        // Key: Type of the view
        // Value: Type of the View Model
        private static readonly IDictionary<Type, Type> ViewToViewModelMap = CreateViewToViewModelMap();

        private static IDictionary<Type, Type> CreateViewToViewModelMap()
        {
            var result = new Dictionary<Type,Type>();
            result.Add(typeof(PopupView), null);
            result.Add(typeof(TestView), typeof(TestViewModel));
            return result;
        }

        protected Type GetViewModelForView(Type viewType)
        {
            if(!ViewToViewModelMap.ContainsKey(viewType))
                throw new NotImplementedException(String.Format("No ViewModel type registered for View of type '{0}'", viewType));

            return ViewToViewModelMap[viewType];
        }

        protected object CreateViewModelOfType(Type vmType)
        {
            return Kernel.Get(vmType);
        }

        protected override void ConfigureKernel()
        {
            base.ConfigureKernel();

            Kernel.Bind<PopupRegionAdapter>().ToSelf().WithConstructorArgument<IntPtr>(this.OwnerWindow);
        }

        protected override void InitializeShell()
        {
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(GetViewModelForView);
            ViewModelLocationProvider.SetDefaultViewModelFactory(CreateViewModelOfType);

            base.InitializeShell();
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var regionAdapterMappings = Kernel.Get<RegionAdapterMappings>();

            if (regionAdapterMappings != null)
            {
                regionAdapterMappings.RegisterMapping(typeof(PopupView), Kernel.Get<PopupRegionAdapter>());
            }

            return base.ConfigureRegionAdapterMappings();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var catalogue = (ModuleCatalog)this.ModuleCatalog;

            var moduleInfos = typeof(Bootstrapper).Assembly.GetTypes()
                .Where(typeof(IModule).IsAssignableFrom)
                .Where(t => t != typeof(IModule))
                .Where(t => !t.IsAbstract)
                .Select(t => CreateModuleInfo(t));

            foreach(var mi in moduleInfos)
            {
                catalogue.AddModule(mi);
            }
        }

        
        private static ModuleInfo CreateModuleInfo(Type type)
        {
            string moduleName = type.Name;

            var moduleAttribute = CustomAttributeData.GetCustomAttributes(type).FirstOrDefault(cad => cad.Constructor.DeclaringType.FullName == typeof(ModuleAttribute).FullName);

            if (moduleAttribute != null)
            {
                foreach (CustomAttributeNamedArgument argument in moduleAttribute.NamedArguments)
                {
                    string argumentName = argument.MemberInfo.Name;
                    if (argumentName == "ModuleName")
                    {
                        moduleName = (string)argument.TypedValue.Value;
                        break;
                    }
                }
            }

            ModuleInfo moduleInfo = new ModuleInfo(moduleName, type.AssemblyQualifiedName)
            {
                InitializationMode = InitializationMode.WhenAvailable,
                Ref = type.Assembly.CodeBase,
            };

            return moduleInfo;
        }

    }
}
