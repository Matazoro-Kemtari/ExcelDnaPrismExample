using ExcelDna.Integration;
using MvvmProject.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelProject
{
    public partial class ExcelAddin
    {
        [ExcelFunction(IsMacroType = true)]
        public static string TestOpenWindow(bool ShowModal)
        {
            Log.Debug("Called TestOpenWindow function (ShowModal = {0})", ShowModal);

            if (ShowModal)
            {
                _Starter.ShowModal<TestView>();
            }
            else
            {
                _Starter.Show<TestView>();
            }

            Log.Debug("Exiting TestOpenWindow function");
            return "done";
        }
    }
}
