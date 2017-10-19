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
        public Tuple<List<string>, List<string>> ReadCSVToString(string location)
        {
            Tuple<List<string>, List<string>> CSVContent;

            using (var reader = new StreamReader(location))
            {
                List<string> ParameterName = new List<string>();
                List<string> ParameterValue = new List<string>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    ParameterName.Add(values[0]);
                    ParameterValue.Add(values[1]);
                }

                CSVContent = Tuple.Create(ParameterName, ParameterValue);
            }

            return CSVContent;
        }
    }
}
