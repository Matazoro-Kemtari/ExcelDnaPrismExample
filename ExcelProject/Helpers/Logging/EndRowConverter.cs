using System.IO;
using System.Text;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace FExcel.Helpers.Logging
{
    public class EndRowConverter : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            CsvTextWriter ctw = writer as CsvTextWriter;
            // write the ending quote for the last field
            if (ctw != null)
                ctw.WriteQuote();
            writer.WriteLine();
        }
    }

}
