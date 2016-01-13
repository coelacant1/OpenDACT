using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace OpenDACT.Class_Files
{
    static class Threading
    {
        public static bool _continue = true;
        public static bool isCalibrating = true;

        static List<string> readLineData = new List<string>();

        public static void Read()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            while (_continue)
            {
                try
                {
                    string message = Connection._serialPort.ReadLine();

                    UserInterface.logPrinter(message);

                    //DecisionHandler.handleInput(message);
                    readLineData.Add(message);
                }
                catch (TimeoutException) { }
            }//end while
        }//end void

        public static void HandleRead()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            while (_continue)
            {
                while (isCalibrating && !readLineData.Any())
                {
                    try
                    {
                        //wait for ok to perform calculation?
                        UserVariables.isInitiated = true;
                        bool canMove;

                        if (readLineData.First().Contains("ok"))
                        {
                            canMove = true;
                        }
                        else
                        {
                            canMove = false;
                        }

                        DecisionHandler.handleInput(readLineData.First(), canMove);
                        readLineData.Remove(readLineData.First());
                    }
                    catch (Exception) { }
                }//end while
            }//end while continue
        }

    }
}
