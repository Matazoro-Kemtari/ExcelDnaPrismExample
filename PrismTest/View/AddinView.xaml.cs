using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using ExcelProject.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelProject.View
{
    /// <summary>
    /// Interaction logic for AddinView.xaml
    /// </summary>
    public partial class AddinView : UserControl, IView/*, IRegionManagerAware*/
    {
        //public IRegionManager RegionManager { set; get; }

        public AddinView()
        {
            Console.WriteLine("Creating AddinView");     

            InitializeComponent();
        }
    }
}
