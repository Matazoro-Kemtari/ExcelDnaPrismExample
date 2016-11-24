using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelUtil
{
    internal class WindowWrapper : IWin32Window
    {
        private readonly IntPtr _Handle;

        public WindowWrapper(IntPtr Handle)
        {
            this._Handle = Handle;
        }

        public IntPtr Handle
        {
            get
            {
                return this._Handle;
            }
        }
    }
}
