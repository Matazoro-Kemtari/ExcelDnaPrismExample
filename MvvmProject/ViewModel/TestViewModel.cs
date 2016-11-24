using ExcelDna.Integration;
using MvvmProject.Model.Helpers.Excel;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;
using Excel = NetOffice.ExcelApi;

namespace MvvmProject.ViewModel
{
    public class TestViewModel : BindableBase
    {
        public string TestProperty { get; set; }

        public ICommand AccessExcelQueued { get; private set; }
        public ICommand AccessExcelDirectly { get; private set; }

        public TestViewModel()
        {
            this.TestProperty = "Test Property";

            this.AccessExcelQueued = new DelegateCommand(AccessExcelQueuedExecute);
            this.AccessExcelDirectly = new DelegateCommand(AccessExcelDirectlyExecute);
        }

        private void AccessExcelQueuedExecute()
        {
            ExcelHelper.RunAsMacro(AccessExcel);
        }

        private void AccessExcelDirectlyExecute()
        {
            if(ExcelUtil.ApplicationState.IsBlocked)
            {
                // we are in a modal window context. Warn user to clarify the following exception is to be expected.
                MessageBox.Show("You are trying to access Excel directly from a modal window context. It is expected you will now get an exception, as you are trying the (busy) Excel object.");
            }

            AccessExcel();
        }

        private void AccessExcel()
        {
            try
            {
                using (var excel = new Excel.Application(null, ExcelDnaUtil.Application))
                {
                    var targetCell = excel.Cells[1, 1].Value;
                    excel.Worksheets.Add();
                    MessageBox.Show("Target Cell (1,1) Value: " + targetCell);
                    ((Excel.Worksheet)excel.Worksheets[2]).Activate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occured: " + ex.Message);
                MessageBox.Show(ex.ToString());
            }
            catch
            {
                MessageBox.Show("Unknown Exception occured");
            }
        }
    }
}
