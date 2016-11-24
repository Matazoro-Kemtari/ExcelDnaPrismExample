using MvvmProject;
using MvvmProject.View;
using Ninject;
using Ninject.Extensions.Logging.Log4net;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (IKernel kernel = CreateKernel())
            {
                var starter = kernel.Get<Starter>();

                Console.WriteLine("Showing TestView");
                starter.Show<TestView>(300, 100);
            }

            Console.WriteLine("done");
            Console.ReadKey();
        }

        protected static IKernel CreateKernel()
        {
            ConfigureLog();
            var settings = new NinjectSettings { LoadExtensions = false };

            var kernel = new StandardKernel(settings, new INinjectModule[] { new Log4NetModule() });
            kernel.Bind<Starter>().ToSelf().WithConstructorArgument<IntPtr>(IntPtr.Zero);

            return kernel;
        }

        protected static void ConfigureLog()
        {
            if(!log4net.LogManager.GetRepository().Configured)
            {
                log4net.Config.XmlConfigurator.Configure();
            }
        }
           
    }
}
