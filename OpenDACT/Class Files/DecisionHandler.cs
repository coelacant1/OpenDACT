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
            EEPROM eeprom;
            UserVariables userVariables;

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
                eeprom = EEPROMFunctions.returnEEPROMObject();
                Program.mainFormTest.setEEPROMGUIList(eeprom);
                EEPROMFunctions.EEPROMReadCount++;
            }
            else if (GCode.checkHeights == true && EEPROMFunctions.tempEEPROMSet == true && Calibration.calibrateInProgress == false && EEPROMFunctions.EEPROMReadOnly == false)
            {
                UserInterface.logConsole("3");
                userVariables = UserInterface.returnUserVariablesObject();
                GCode.positionFlow(ref userVariables);
            }
            else if (Calibration.calibrateInProgress == false && GCode.checkHeights == false && EEPROMFunctions.tempEEPROMSet == true && EEPROMFunctions.EEPROMReadOnly == false)
            {
                UserInterface.logConsole("4");
                eeprom = EEPROMFunctions.returnEEPROMObject();
                userVariables = UserInterface.returnUserVariablesObject();


                if (HeightFunctions.parseZProbe(message) != 1000)
                {
                    HeightFunctions.setHeights(HeightFunctions.parseZProbe(message), ref eeprom, ref userVariables);
                    UserInterface.logConsole(HeightFunctions.parseZProbe(message) + "\n");
                }

                if(HeightFunctions.heightsSet == true)
                {
                    Heights heights = HeightFunctions.returnHeightObject();
                    Program.mainFormTest.setHeightsInvoke(heights);
                    
                    if (Calibration.calibrationState == true && HeightFunctions.checkHeightsOnly == false)
                    {
                        Calibration.calibrateInProgress = true;

                        if (EEPROMFunctions.EEPROMRequestSent == false)
                        {
                            EEPROMFunctions.readEEPROM();
                            EEPROMFunctions.EEPROMRequestSent = true;
                        }

                        Program.mainFormTest.setUserVariables(ref userVariables);

                        if (userVariables.advancedCalibration == false)
                        {
                            Calibration.calibrate(Calibration.calibrationSelection, ref eeprom, ref heights, ref userVariables);
                        }
                        else
                        {
                            GCode.heuristicLearning(ref eeprom, ref userVariables, ref heights);
                        }

                        Program.mainFormTest.setEEPROMGUIList(eeprom);
                        EEPROMFunctions.sendEEPROM(eeprom);
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
