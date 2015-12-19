using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace OpenDACT.Class_Files
{
    static class ConsoleRead
    {
        public static bool _continue = true;
        public static void Read()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");
            while (_continue)
            {
                try
                {
                    UserVariables.isInitiated = true;

                    string message = Connection._serialPort.ReadLine();

                    UserInterface.logPrinter(message);

                    DecisionHandler.handleInput(message);
                }
                catch (TimeoutException) { }
            }//end while
        }//end void

    }
}
