using MvvmProject.View.Internal;
using Prism.Modularity;
using Prism.Regions;

namespace MvvmProject.Modules.Internal
{
    public class PopupModule : IModule
    {
        private readonly IRegionManager regionManager;

        public PopupModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(RegionNames.ShellContentRegion, typeof(PopupView));
        }
    }
}
