using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class EEPROMClass
    {
        public static float stepsPerMM;
        public static float tempSPM;
        public static float zMaxLength;
        public static float zProbe;
        public static float HRad;
        public static float offsetX;
        public static float offsetY;
        public static float offsetZ;
        public static float A;
        public static float B;
        public static float C;
        public static float DA;
        public static float DB;
        public static float DC;

        public static void setSPM(float value)
        {
            stepsPerMM = value;
        }
        public static void setTempSPM(float value)
        {
            tempSPM = value;
        }
        public static void setZMaxLength(float value)
        {
            zMaxLength = value;
        }
        public static void setZProbe(float value)
        {
            zProbe = value;
        }
        public static void setHRad(float value)
        {
            HRad = value;
        }
        public static void setOffsetX(float value)
        {
            offsetX = value;
        }
        public static void setOffsetY(float value)
        {
            offsetY = value;
        }
        public static void setOffsetZ(float value)
        {
            offsetZ = value;
        }
        public static void setA(float value)
        {
            A = value;
        }
        public static void setB(float value)
        {
            B = value;
        }
        public static void setC(float value)
        {
            C = value;
        }
        public static void setDA(float value)
        {
            DA = value;
        }
        public static void setDB(float value)
        {
            DB = value;
        }
        public static void setDC(float value)
        {
            DC = value;
        }
    }

    class EEPROM
    {
        UserInterface UserInterface;
        GCode GCode;
        Heights Heights;

        public EEPROM(UserInterface _UserInterface, GCode _GCode, Heights _Heights)
        {
            this.UserInterface = _UserInterface;
            this.GCode = _GCode;
            this.Heights = _Heights;
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
                    
                    EEPROMClass.setSPM(floatParse2);
                    EEPROMClass.setTempSPM(floatParse2);
                    break;
                case 153:
                    EEPROMClass.setZMaxLength(floatParse2);
                    break;
                case 808:
                    EEPROMClass.setZProbe(floatParse2);
                    break;
                case 885:
                    EEPROMClass.setHRad(floatParse2);
                    break;
                case 893:
                    EEPROMClass.setOffsetX(floatParse2);
                    break;
                case 895:
                    EEPROMClass.setOffsetY(floatParse2);
                    break;
                case 897:
                    EEPROMClass.setOffsetZ(floatParse2);
                    break;
                case 901:
                    EEPROMClass.setA(floatParse2);
                    break;
                case 905:
                    EEPROMClass.setB(floatParse2);
                    break;
                case 909:
                    EEPROMClass.setC(floatParse2);
                    break;
                case 913:
                    EEPROMClass.setDA(floatParse2);
                    break;
                case 917:
                    EEPROMClass.setDB(floatParse2);
                    break;
                case 921:
                    EEPROMClass.setDC(floatParse2);
                    EEPROMSet = true;
                    break;
            }
        }
    }
}
