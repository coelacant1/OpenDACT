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
                UserInterface.logConsole("1");
            }
            else if (EEPROMFunctions.tempEEPROMSet == true && EEPROMFunctions.EEPROMReadOnly == true && EEPROMFunctions.EEPROMReadCount < 1)
            {
                UserInterface.logConsole("2");
                Program.mainFormTest.setEEPROMGUIList();
            }
            else if (GCode.checkHeights == true && EEPROMFunctions.tempEEPROMSet == true && Calibration.calibrateInProgress == false && EEPROMFunctions.EEPROMReadOnly == false)
            {
                UserInterface.logConsole("3");
                GCode.positionFlow();
            }
            else if (Calibration.calibrateInProgress == false && GCode.checkHeights == false && EEPROMFunctions.tempEEPROMSet == true && EEPROMFunctions.EEPROMReadOnly == false)
            {
                UserInterface.logConsole("4");


                if (HeightFunctions.parseZProbe(message) != 1000)
                {
                    HeightFunctions.setHeights(HeightFunctions.parseZProbe(message));
                    UserInterface.logConsole(HeightFunctions.parseZProbe(message) + "\n");
                }

                if(HeightFunctions.heightsSet == true)
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
                            Calibration.calibrate(Calibration.calibrationSelection);
                        }
                        else
                        {
                            GCode.heuristicLearning();
                        }

                        Program.mainFormTest.setEEPROMGUIList();
                        EEPROMFunctions.sendEEPROM();
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
