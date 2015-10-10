using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    static class ConsoleRead
    {
        public static bool _continue = true;
        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    UserInterface.isInitiated = true;

                    string message = Connection._serialPort.ReadLine();

                    UserInterface.logPrinter(message);

                    DecisionHandler.handleInput(message);
                }
                catch (TimeoutException) { }
            }//end while
        }//end void

    }
}
