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
            Program.mainFormTest.setUserVariables();

            if (EEPROMFunctions.tempEEPROMSet == false)
            {
                int intParse;
                float floatParse2;

                EEPROMFunctions.parseEEPROM(message, out intParse, out floatParse2);
                EEPROMFunctions.setEEPROM(intParse, floatParse2);
            }
            else if (EEPROMFunctions.tempEEPROMSet == true && EEPROMFunctions.EEPROMReadOnly == true && EEPROMFunctions.EEPROMReadCount < 1)
            {
                //rm
            }
            else if (GCode.checkHeights == true && EEPROMFunctions.tempEEPROMSet == true && Calibration.calibrateInProgress == false && EEPROMFunctions.EEPROMReadOnly == false)
            {
                GCode.positionFlow();
            }
            else if (Calibration.calibrationState == true && Calibration.calibrateInProgress == false && GCode.checkHeights == false && EEPROMFunctions.tempEEPROMSet == true && EEPROMFunctions.EEPROMReadOnly == false)
            {
                if (HeightFunctions.parseZProbe(message) != 1000 && HeightFunctions.heightsSet == false)
                {
                    HeightFunctions.setHeights(HeightFunctions.parseZProbe(message));
                }
                else if (HeightFunctions.heightsSet == true)
                {
                    Program.mainFormTest.setHeightsInvoke();

                    if (Calibration.calibrationState == true && HeightFunctions.checkHeightsOnly == false)
                    {
                        Calibration.calibrateInProgress = true;

                        if (EEPROMFunctions.EEPROMRequestSent == false)
                        {
                            EEPROMFunctions.readEEPROM();
                            EEPROMFunctions.EEPROMRequestSent = true;
                        }

                        if (UserVariables.advancedCalibration == false)
                        {
                            Calibration.calibrate();
                        }
                        else
                        {
                            GCode.heuristicLearning();
                        }

                        Program.mainFormTest.setEEPROMGUIList();
                        EEPROMFunctions.sendEEPROM();

                        if (Calibration.calibrationState == false)
                        {
                            GCode.homeAxes();
                            UserInterface.logConsole("Calibration Complete");
                            //end calibration
                        }

                        Calibration.calibrateInProgress = false;
                    }

                    HeightFunctions.heightsSet = false;
                }
            }
            /*
            else
            {
                UserInterface.logConsole("0: " + Calibration.calibrateInProgress + GCode.checkHeights + EEPROMFunctions.tempEEPROMSet + EEPROMFunctions.EEPROMReadOnly);
            }
            */

        }
    }
}
