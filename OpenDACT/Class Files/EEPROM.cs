using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    public class EEPROM
    {
        public float stepsPerMM;
        public float tempSPM;
        public float zMaxLength;
        public float zProbe;
        public float HRadius;
        public float diagonalRod;
        public float offsetX;
        public float offsetY;
        public float offsetZ;
        public float A;
        public float B;
        public float C;
        public float DA;
        public float DB;
        public float DC;

        public EEPROM(float _stepsPerMM, float _tempSPM, float _zMaxLength, float _zProbe, float _diagonalRod, float _HRadius, float _offsetX, float _offsetY, float _offsetZ, float _A, float _B, float _C, float _DA, float _DB, float _DC)
        {
            stepsPerMM = _stepsPerMM;
            tempSPM = _tempSPM;
            zMaxLength = _zMaxLength;
            zProbe = _zProbe;
            diagonalRod = _diagonalRod;
            HRadius = _HRadius;
            offsetX = _offsetX;
            offsetY = _offsetY;
            offsetZ = _offsetZ;
            A = _A;
            B = _B;
            C = _C;
            DA = _DA;
            DB = _DB;
            DC = _DC;
        }
    }

    static class EEPROMFunctions
    {
        public static bool EEPROMSet = false;
        public static bool EEPROMRequestSent = false;
        private static float tempStepsPerMM;
        private static float tempTempSPM;
        private static float tempZMaxLength;
        private static float tempZProbe;
        private static float tempHRadius;
        private static float tempDiagonalRod;
        private static float tempOffsetX;
        private static float tempOffsetY;
        private static float tempOffsetZ;
        private static float tempA;
        private static float tempB;
        private static float tempC;
        private static float tempDA;
        private static float tempDB;
        private static float tempDC;

        public static void readEEPROM()
        {
            GCode.sendReadEEPROMCommand();
        }

        public static EEPROM returnEEPROMObject()
        {
            EEPROM eepromObject = new EEPROM(tempStepsPerMM, tempTempSPM, tempZMaxLength, tempZProbe, tempDiagonalRod, tempHRadius, tempOffsetX, tempOffsetY, tempOffsetZ, tempA, tempB, tempC, tempDA, tempDB, tempDC);
            return eepromObject;
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

                //int intParse;
                //float floatParse2;

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

                    tempStepsPerMM = floatParse2;
                    tempTempSPM = floatParse2;
                    break;
                case 153:
                    tempZMaxLength = floatParse2;
                    break;
                case 808:
                    tempZProbe = floatParse2;
                    break;
                case 881:
                    tempDiagonalRod = floatParse2;
                    break;
                case 885:
                    tempHRadius = floatParse2;
                    break;
                case 893:
                    tempOffsetX = floatParse2;
                    break;
                case 895:
                    tempOffsetY = floatParse2;
                    break;
                case 897:
                    tempOffsetZ = floatParse2;
                    break;
                case 901:
                    tempA = floatParse2;
                    break;
                case 905:
                    tempB = floatParse2;
                    break;
                case 909:
                    tempC = floatParse2;
                    break;
                case 913:
                    tempDA = floatParse2;
                    break;
                case 917:
                    tempDB = floatParse2;
                    break;
                case 921:
                    tempDC = floatParse2;
                    EEPROMFunctions.EEPROMSet = true;
                    GCode.checkHeights = true;
                    break;
            }
        }

        public static void sendEEPROM(EEPROM eeprom)
        {
            //manually set all eeprom values
            UserInterface.logConsole("Setting EEPROM.");
            Thread.Sleep(750);
            GCode.sendToPosition(0, 0, 100);
            Thread.Sleep(1000);
            GCode.sendEEPROMVariable(3, 11, eeprom.stepsPerMM);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 153, eeprom.zMaxLength);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 808, eeprom.zProbe);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 881, eeprom.diagonalRod);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 885, eeprom.HRadius);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(1, 893, eeprom.offsetX);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(1, 895, eeprom.offsetY);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(1, 897, eeprom.offsetZ);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 901, eeprom.A);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 905, eeprom.B);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 909, eeprom.C);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 913, eeprom.DA);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 917, eeprom.DB);
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 921, eeprom.DC);
            Thread.Sleep(750);
        }
    }
}
