using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    static class DecisionHandler
    {

        public static void handleInput(string message)
        {
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
                        if (userVariables.advancedCalibration == false)
                        {
                            GCode.checkHeights = false;
                            Heights heights = HeightFunctions.returnHeightObject();
                            Calibration.calibrate(Calibration.calibrationSelection, ref eeprom, ref heights, ref userVariables);
                        }
                        else
                        {
                            GCode.checkHeights = false;
                            Heights heights = HeightFunctions.returnHeightObject();
                            GCode.heuristicLearning(ref eeprom, ref userVariables, ref heights);
                        }
                    }
                }
            }
            else if (GCode.checkHeights == true && Calibration.calibrateInProgress == false)
            {
                UserVariables userVariables = UserInterface.returnUserVariablesObject();
                GCode.positionFlow(ref userVariables);
            }


        }
    }
}
