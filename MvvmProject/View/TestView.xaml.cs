using ExcelDna.Integration;
using MvvmProject.Model.Helpers.Excel;
using Microsoft.Practices.Prism.Mvvm;
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


namespace MvvmProject.View
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl, IView
    {
        public TestView()
        {
            InitializeComponent();
        }
        /*
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var excel = new Excel.Application(null, ExcelDnaUtil.Application))
            {
                var activeCell = excel.Cells[1, 1].Value;
                MessageBox.Show("active cell: " + activeCell);
            }
        }*/
    }
}
