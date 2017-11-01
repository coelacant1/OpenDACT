using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public abstract class Parameters
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

        public abstract void SaveParameters(string location);
        public abstract void LoadParameters(string location);
        public abstract void ValidateParameters();//ensures that the parameters are properly initialized, if not it lets users know

        public abstract class Builder<T> where T : Parameters
        {
            public abstract T Build();
        };
    }
}
