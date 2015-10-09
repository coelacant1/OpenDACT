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
                    else if (Calibration.calibrateInProgress == false && GCode.checkHeights == false)
                    {
                        if (EEPROMFunctions.EEPROMSet == true)
                        {
                            EEPROM eeprom = EEPROMFunctions.returnEEPROMObject();
                            UserVariables userVariables = UserInterface.returnUserVariablesObject();
                            

                            if (HeightFunctions.parseZProbe(message) != 200)
                            {
                                HeightFunctions.setHeights(HeightFunctions.parseZProbe(message), ref eeprom, ref userVariables);
                                UserInterface.logConsole(HeightFunctions.parseZProbe(message) + "\n");
                            }

                            if (HeightFunctions.heightsSet == true)
                            {
                                GCode.checkHeights = false;
                                Heights heights = HeightFunctions.returnHeightObject();
                                Calibration.calibrate(Calibration.calibrationSelection, ref eeprom, ref heights, ref userVariables);
                            }
                        }
                    }
                    else if (GCode.checkHeights == true && Calibration.calibrateInProgress == false)
                    {
                        UserVariables userVariables = UserInterface.returnUserVariablesObject();
                        GCode.positionFlow(ref userVariables);
                    }


                }
                catch (TimeoutException) { }
            }//end while
        }//end void

    }
}
