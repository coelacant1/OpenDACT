using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public abstract class IParameters
    {
        public Dictionary<string, string> ReadCSVToString(string location)
        {
            Dictionary<string, string> CSVContent = new Dictionary<string, string>();

            using (var reader = new StreamReader(location))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    CSVContent.Add(values[0],values[1]);
                }
            }

            return CSVContent;
        }
    }
}
