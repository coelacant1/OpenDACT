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
                    string message = Connection._serialPort.ReadLine();

                    UserInterface.logPrinter(message + "\n");

                    if (EEPROMFunctions.EEPROMSet == false)
                    {
                        int intParse;
                        float floatParse2;

                        EEPROMFunctions.parseEEPROM(message, out intParse, out floatParse2);
                        EEPROMFunctions.setEEPROM(intParse, floatParse2);
                    }
                    else if (Calibration.calibrateInProgress == false)
                    {

                        EEPROM eeprom = EEPROMFunctions.returnEEPROMObject();
                        Heights heights = new Heights(HeightFunctions.tempCenter, HeightFunctions.tempX, HeightFunctions.tempXOpp, HeightFunctions.tempY, HeightFunctions.tempYOpp, HeightFunctions.tempZ, HeightFunctions.tempZOpp);
                        UserVariables userVariables = UserInterface.returnUserVariablesObject();


                        if (HeightFunctions.parseZProbe(message) != 200)
                        {
                            HeightFunctions.setHeights(HeightFunctions.parseZProbe(message), ref eeprom, ref userVariables);
                        }

                        UserInterface.logConsole(HeightFunctions.parseZProbe(message) + "\n");

                        Calibration.calibrate(Calibration.calibrationSelection, ref eeprom, ref heights, ref userVariables);
                    }

                }
                catch (TimeoutException) { }
            }//end while
        }//end void

    }
}
