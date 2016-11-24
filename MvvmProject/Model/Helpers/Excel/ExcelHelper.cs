using ExcelDna.Integration;
using System;
using System.Threading.Tasks;

namespace MvvmProject.Model.Helpers.Excel
{
    public static class ExcelHelper
    {
        /// <summary>
        /// Runs code on Excel's calculation thread and returns and awaitable task
        /// </summary>
        public static Task<TResult> RunAsMacro<TResult>(Func<TResult> func)
        {
            var tcs = new TaskCompletionSource<TResult>();

            ExcelAsyncUtil.QueueAsMacro(() =>
            {
                try
                {
                    var r = func();

                    tcs.SetResult(r);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Runs code on Excel's calculation thread and returns and awaitable task
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task RunAsMacro(Action action)
        {
            return RunAsMacro(() => { action(); return true; });
        }
    }
}
