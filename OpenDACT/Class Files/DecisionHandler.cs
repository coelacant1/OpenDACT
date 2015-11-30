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
                /*
                if (UserVariables.probeChoice == "Z-Probe" && GCode.wasZProbeHeightSet == false && GCode.wasSet == true)
                {
                    if (HeightFunctions.parseZProbe(message) != 1000)
                    {
                        EEPROM.zMaxLength = Convert.ToSingle(HeightFunctions.parseZProbe(message) + Math.Round((EEPROM.zMaxLength * 5) / 6));
                        
                        GCode.wasZProbeHeightSet = true;
                        Program.mainFormTest.setEEPROMGUIList();
                        EEPROMFunctions.sendEEPROM();
                    }

                }
                else
                {
                */
                    GCode.positionFlow();
                //}
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

                        /*
                        if (EEPROMFunctions.EEPROMRequestSent == false)
                        {
                            EEPROMFunctions.readEEPROM();
                            EEPROMFunctions.EEPROMRequestSent = true;
                        }
                        */

                        if (UserVariables.advancedCalibration == false)
                        {
                            UserInterface.logConsole("Calibration Iteration Number: " + Calibration.iterationNum);
                            Calibration.calibrate();

                            Program.mainFormTest.setEEPROMGUIList();
                            EEPROMFunctions.sendEEPROM();

                            if (Calibration.calibrationState == false)
                            {
                                GCode.homeAxes();
                                UserInterface.logConsole("Calibration Complete");
                                //end calibration
                            }
                        }
                        else
                        {
                            UserInterface.logConsole("Heuristic Step: " + UserVariables.advancedCalCount);
                            GCode.heuristicLearning();

                            Program.mainFormTest.setEEPROMGUIList();
                            EEPROMFunctions.sendEEPROM();
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
