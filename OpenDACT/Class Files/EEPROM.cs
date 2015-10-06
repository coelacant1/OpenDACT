using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class EEPROM
    {
        public static float stepsPerMM;
        public static float tempSPM;
        public static float zMaxLength;
        public static float zProbe;
        public static float HRadius;
        public static float offsetX;
        public static float offsetY;
        public static float offsetZ;
        public static float A;
        public static float B;
        public static float C;
        public static float DA;
        public static float DB;
        public static float DC;

        //set
        public static void setSPM(float value)
        {
            value = Validation.checkZero(value);
            stepsPerMM = value;
        }
        public static void setTempSPM(float value)
        {
            value = Validation.checkZero(value);
            tempSPM = value;
        }
        public static void setZMaxLength(float value)
        {
            value = Validation.checkZero(value);
            zMaxLength = value;
        }
        public static void setZProbe(float value)
        {
            value = Validation.checkZero(value);
            zProbe = value;
        }
        public static void setHRadius(float value)
        {
            value = Validation.checkZero(value);
            HRadius = value;
        }
        public static void setOffsetX(float value)
        {
            value = Validation.checkZero(value);
            offsetX = value;
        }
        public static void setOffsetY(float value)
        {
            value = Validation.checkZero(value);
            offsetY = value;
        }
        public static void setOffsetZ(float value)
        {
            value = Validation.checkZero(value);
            offsetZ = value;
        }
        public static void setA(float value)
        {
            value = Validation.checkZero(value);
            A = value;
        }
        public static void setB(float value)
        {
            value = Validation.checkZero(value);
            B = value;
        }
        public static void setC(float value)
        {
            value = Validation.checkZero(value);
            C = value;
        }
        public static void setDA(float value)
        {
            value = Validation.checkZero(value);
            DA = value;
        }
        public static void setDB(float value)
        {
            value = Validation.checkZero(value);
            DB = value;
        }
        public static void setDC(float value)
        {
            value = Validation.checkZero(value);
            DC = value;
        }

        //return
        public static float returnSPM()
        {
            return stepsPerMM;
        }
        public static float returnTempSPM()
        {
            return tempSPM;
        }
        public static float returnZMaxLength()
        {
            return zMaxLength;
        }
        public static float returnZProbe()
        {
            return zProbe;
        }
        public static float returnHRadius()
        {
            return HRadius;
        }
        public static float returnOffsetX()
        {
            return offsetX;
        }
        public static float returnOffsetY()
        {
            return offsetY;
        }
        public static float returnOffsetZ()
        {
            return offsetZ;
        }
        public static float returnA()
        {
            return A;
        }
        public static float returnB()
        {
            return B;
        }
        public static float returnC()
        {
            return C;
        }
        public static float returnDA()
        {
            return DA;
        }
        public static float returnDB()
        {
            return DB;
        }
        public static float returnDC()
        {
            return DC;
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

        public void readEEPROM()
        {
            GCode.sendReadEEPROMCommand();
        }

        public object returnEEPROM()
        {
            return EEPROMVariables;
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
    }
}
