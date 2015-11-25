using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    public static class EEPROM
    {
        public static float stepsPerMM;
        public static float tempSPM;
        public static float zMaxLength;
        public static float zProbeHeight;
        public static float zProbeSpeed;
        public static float HRadius;
        public static float diagonalRod;
        public static float offsetX;
        public static float offsetY;
        public static float offsetZ;
        public static float A;
        public static float B;
        public static float C;
        public static float DA;
        public static float DB;
        public static float DC;
    }

    static class EEPROMFunctions
    {
        public static bool tempEEPROMSet = false;
        public static bool EEPROMRequestSent = false;
        public static bool EEPROMReadOnly = false;
        public static int EEPROMReadCount = 0;

        public static void readEEPROM()
        {
            GCode.sendReadEEPROMCommand();
        }

        public static void parseEEPROM(string value, out int intParse, out float floatParse2)
        {
            //parse EEProm
            if (value.Contains("EPR"))
            {
                string[] parseEPR = value.Split(new string[] { "EPR", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] parseEPRSpace;

                if (parseEPR.Length > 1)
                {
                    parseEPRSpace = parseEPR[1].Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    parseEPRSpace = parseEPR[0].Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                }
                
                //check if there is a space between
                if (parseEPRSpace[0] == ":")
                {
                    //Space
                    intParse = int.Parse(parseEPRSpace[2]);
                    floatParse2 = float.Parse(parseEPRSpace[3]);
                }
                else if (value.Contains("EEPROM") || value.Contains("updated"))
                {
                    intParse = 1000;
                    floatParse2 = 0F;
                }
                else
                {
                    //No space
                    intParse = int.Parse(parseEPRSpace[1]);
                    floatParse2 = float.Parse(parseEPRSpace[2]);
                }
            }//end EEProm capture
            else
            {
                //No space
                intParse = 0;
                floatParse2 = 0;
            }
        }

        public static void setEEPROM(int intParse, float floatParse2)
        {
            switch (intParse)
            {
                case 11:
                    UserInterface.logConsole("EEPROM capture initiated");

                    EEPROM.stepsPerMM = floatParse2;
                    EEPROM.tempSPM = floatParse2;
                    break;
                case 153:
                    EEPROM.zMaxLength = floatParse2;
                    break;
                case 808:
                    EEPROM.zProbeHeight = floatParse2;
                    break;
                case 812:
                    EEPROM.zProbeSpeed = floatParse2;
                    tempEEPROMSet = true;
                    GCode.checkHeights = true;
                    EEPROMReadCount++;
                    Program.mainFormTest.setEEPROMGUIList();

                    break;
                case 881:
                    EEPROM.diagonalRod = floatParse2;
                    break;
                case 885:
                    EEPROM.HRadius = floatParse2;
                    break;
                case 893:
                    EEPROM.offsetX = floatParse2;
                    break;
                case 895:
                    EEPROM.offsetY = floatParse2;
                    break;
                case 897:
                    EEPROM.offsetZ = floatParse2;
                    break;
                case 901:
                    EEPROM.A = floatParse2;
                    break;
                case 905:
                    EEPROM.B = floatParse2;
                    break;
                case 909:
                    EEPROM.C = floatParse2;
                    break;
                case 913:
                    EEPROM.DA = floatParse2;
                    break;
                case 917:
                    EEPROM.DB = floatParse2;
                    break;
                case 921:
                    EEPROM.DC = floatParse2;
                    break;
            }
        }

        public static void sendEEPROM()
        {
            //manually set all eeprom values
            UserInterface.logConsole("Setting EEPROM.");
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 11, EEPROM.stepsPerMM);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 153, EEPROM.zMaxLength);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 808, EEPROM.zProbeHeight);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 812, EEPROM.zProbeSpeed);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 881, EEPROM.diagonalRod);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 885, EEPROM.HRadius);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(1, 893, EEPROM.offsetX);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(1, 895, EEPROM.offsetY);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(1, 897, EEPROM.offsetZ);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 901, EEPROM.A);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 905, EEPROM.B);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 909, EEPROM.C);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 913, EEPROM.DA);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 917, EEPROM.DB);
            Thread.Sleep(50);
            GCode.sendEEPROMVariable(3, 921, EEPROM.DC);
            Thread.Sleep(50);
        }
    }
}
