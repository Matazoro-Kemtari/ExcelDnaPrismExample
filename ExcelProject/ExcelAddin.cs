using ExcelDna.Integration;
using MvvmProject;
using MvvmProject.View;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.Log4net;
using Ninject.Modules;
using System;

namespace ExcelProject
{
    public partial class ExcelAddin : IExcelAddIn
    {
        internal static MvvmProject.Starter _Starter;
        protected static ILogger Log;

        public void AutoClose()
        {

        }

        public void AutoOpen()
        {
            ExcelUtil.ApplicationState.Initialize();
            
            using(var kernel = CreateKernel())
            {
                _Starter = kernel.Get<Starter>();
                Log = kernel.Get<ILoggerFactory>().GetCurrentClassLogger();
            }
        }

        protected IKernel CreateKernel()
        {
            ConfigureLog();
            var settings = new NinjectSettings { LoadExtensions = false };

            var kernel = new StandardKernel(settings, new INinjectModule[] { new Log4NetModule() });
            kernel.Bind<Starter>().ToSelf().InSingletonScope().WithConstructorArgument<IntPtr>(ExcelDnaUtil.WindowHandle);
            return kernel;
        }

        protected void ConfigureLog()
        {
            if(!log4net.LogManager.GetRepository().Configured)
            {
                log4net.Config.XmlConfigurator.Configure();
            }
        }
    }
}
