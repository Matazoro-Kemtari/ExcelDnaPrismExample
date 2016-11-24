using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExcelUtil
{
    /// <summary>
    /// This class MUST be initialized on Excel's main thread.
    /// </summary>
    public class ApplicationState
    {
        private static ApplicationState _Instance;
        private static object _BlockingFormLock = new object();
        private static BlockingForm _BlockingForm;
        private static ManualResetEventSlim _Blocker = new ManualResetEventSlim();

        /// <summary>
        /// MUST be called from an Excel thread
        /// </summary>
        public static void Initialize()
        {
            if (_Instance != null)
                throw new InvalidOperationException("ExcelUtil has already been initialized");

            _Instance = new ApplicationState();
        }

        
        public static bool IsBlocked
        {
            get
            {
                return _Blocker.IsSet;
            }
        }

        public static void Block()
        {
            if (IsBlocked)
                throw new InvalidOperationException("Excel is already blocked");

            try
            {
                lock (_BlockingFormLock)
                {
                    _BlockingForm = new BlockingForm();
                }

                _Blocker.Set();
                _BlockingForm.ShowDialog(new WindowWrapper(ExcelDnaUtil.WindowHandle));
                
            } catch
            {
                _Blocker.Reset();
                throw;
            }
        }

        public static void Unblock()
        {
            if(!IsBlocked)
                throw new InvalidOperationException("Excel is not currently blocked");

            if (_BlockingForm == null)
                throw new InvalidProgramException("BlockingForm was null");

            try
            {
                _BlockingForm.Invoke(new MethodInvoker(() => _BlockingForm.Close()));

                lock (_BlockingFormLock)
                {
                    _BlockingForm = null;
                }

                _Blocker.Reset();
            } catch
            {
                _Blocker.Set();
            }
        }
    }
}
