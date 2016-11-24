using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExcelProject.Extensions
{
    /// <summary>
    /// A behaviour class which attaches the current scoped <see cref="IRegionManager"/> to views and their data contexts.
    /// </summary>
    public class RegionAwareBehaviour : RegionBehavior
    {
        /// <summary>
        /// The key to identify this behaviour.
        /// </summary>
        public const string RegionAwareBehaviourKey = "RegionAwareBehaviour";

        /// <summary>
        /// Override this method to perform the logic after the behaviour has been attached.
        /// </summary>
        protected override void OnAttach()
        {
            Region.Views.CollectionChanged += RegionViewsChanged;
        }

        private void RegionViewsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    MakeViewAware(item);
        }

        private void MakeViewAware(object view)
        {
            var frameworkElement = view as FrameworkElement;
            if (frameworkElement != null)
                MakeDataContextAware(frameworkElement);

            MakeAware(view);
        }

        private void MakeDataContextAware(FrameworkElement frameworkElement)
        {
            if (frameworkElement.DataContext != null)
                MakeAware(frameworkElement.DataContext);
        }

        private void MakeAware(object target)
        {
            var scope = target as IRegionManagerAware;
            if (scope != null)
                scope.RegionManager = Region.RegionManager;
        }
    }
}
