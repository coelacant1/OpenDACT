using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class EEPROM
    {
        public float stepsPerMM;
        public float tempSPM;
        public float zMaxLength;
        public float zProbe;
        public float HRadius;
        public float offsetX;
        public float offsetY;
        public float offsetZ;
        public float A;
        public float B;
        public float C;
        public float DA;
        public float DB;
        public float DC;
        
        public EEPROM(float _stepsPerMM, float _tempSPM, float _zMaxLength, float _zProbe, float _HRadius, float _offsetX, float _offsetY, float _offsetZ, float _A, float _B, float _C, float _DA, float _DB, float _DC)
        {
            stepsPerMM = _stepsPerMM;
            tempSPM = _tempSPM;
            zMaxLength = _zMaxLength;
            zProbe = _zProbe;
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

    class EEPROMFunctions
    {
        UserInterface UserInterface;
        GCode GCode;
        EEPROM EEPROM;

        public EEPROMFunctions(UserInterface _UserInterface, GCode _GCode, EEPROM _EEPROM)
        {
            this.UserInterface = _UserInterface;
            this.GCode = _GCode;
            this.EEPROM = _EEPROM;
        }



        public object EEPROMVariables;
        public bool EEPROMSet = false;
        public int iterationNum;
        public int centerIterations;
        private static float tempStepsPerMM;
        private static float tempTempSPM;
        private static float tempZMaxLength;
        private static float tempZProbe;
        private static float tempHRadius;
        private static float tempOffsetX;
        private static float tempOffsetY;
        private static float tempOffsetZ;
        private static float tempA;
        private static float tempB;
        private static float tempC;
        private static float tempDA;
        private static float tempDB;
        private static float tempDC;

        public void readEEPROM()
        {
            GCode.sendReadEEPROMCommand();
        }

        public object returnEEPROM()
        {
            return EEPROMVariables;
        }

        public EEPROM returnEEPROMObject()
        {
            EEPROM eepromObject = new EEPROM(tempStepsPerMM, tempTempSPM, tempZMaxLength, tempZProbe, tempHRadius, tempOffsetX, tempOffsetY, tempOffsetZ, tempA, tempB, tempC, tempDA, tempDB, tempDC);
            return eepromObject;
        }

        public void parseEEPROM(string value, out int intParse, out float floatParse2)
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

        public void setEEPROM(int intParse, float floatParse2)
        {
            switch (intParse)
            {
                case 11:
                    UserInterface.logConsole("EEPROM capture initiated\n");

                    EEPROM.setSPM(floatParse2);
                    EEPROM.setTempSPM(floatParse2);
                    break;
                case 153:
                    EEPROM.setZMaxLength(floatParse2);
                    break;
                case 808:
                    EEPROM.setZProbe(floatParse2);
                    break;
                case 885:
                    EEPROM.setHRadius(floatParse2);
                    break;
                case 893:
                    EEPROM.setOffsetX(floatParse2);
                    break;
                case 895:
                    EEPROM.setOffsetY(floatParse2);
                    break;
                case 897:
                    EEPROM.setOffsetZ(floatParse2);
                    break;
                case 901:
                    EEPROM.setA(floatParse2);
                    break;
                case 905:
                    EEPROM.setB(floatParse2);
                    break;
                case 909:
                    EEPROM.setC(floatParse2);
                    break;
                case 913:
                    EEPROM.setDA(floatParse2);
                    break;
                case 917:
                    EEPROM.setDB(floatParse2);
                    break;
                case 921:
                    EEPROM.setDC(floatParse2);
                    EEPROMSet = true;
                    break;
            }
        }

        public void sendEEPROM(EEPROM eeprom)
        {

            GCode.sendEEPROMVariable();
        }
    }
}
