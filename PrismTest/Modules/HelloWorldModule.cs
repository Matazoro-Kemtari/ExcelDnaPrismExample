using Prism.Modularity;
using Prism.Regions;
using ExcelProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelProject.Modules
{
    public class HelloWorldModule : IModule
    {
        private readonly IRegionManager regionManager;

        public HelloWorldModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion("MyWindowRegion", typeof(HelloWorldView));
            regionManager.RegisterViewWithRegion("AddinViewRegion", typeof(HelloWorldView));
        }
    }
}
