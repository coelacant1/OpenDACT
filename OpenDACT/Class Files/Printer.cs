using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    class Heights
    {
        //store every set of heights
        public static float center;
        public static float X;
        public static float XOpp;
        public static float Y;
        public static float YOpp;
        public static float Z;
        public static float ZOpp;

        //set
        public static void setCenter(float value)
        {
            value = Validation.checkZero(value);
            center = value;
        }
        public static void setX(float value)
        {
            value = Validation.checkZero(value);
            X = value;
        }
        public static void setXOpp(float value)
        {
            value = Validation.checkZero(value);
            XOpp = value;
        }
        public static void setY(float value)
        {
            value = Validation.checkZero(value);
            Y = value;
        }
        public static void setYOpp(float value)
        {
            value = Validation.checkZero(value);
            YOpp = value;
        }
        public static void setZ(float value)
        {
            value = Validation.checkZero(value);
            Z = value;
        }
        public static void setZOpp(float value)
        {
            value = Validation.checkZero(value);
            ZOpp = value;
        }

        //return
        public static float returnCenter()
        {
            return center;
        }
        public static float returnX()
        {
            return X;
        }
        public static float returnXOpp()
        {
            return XOpp;
        }
        public static float returnY()
        {
            return Y;
        }
        public static float returnYOpp()
        {
            return YOpp;
        }
        public static float returnZ()
        {
            return Z;
        }
        public static float returnZOpp()
        {
            return ZOpp;
        }
    }

    class HeightFunctions
    {
        UserInterface UserInterface;
        GCode GCode;
        UserVariables UserVariables;

        public HeightFunctions(UserInterface _UserInterface, GCode _GCode, UserVariables _UserVariables)
        {
            this.UserInterface = _UserInterface;
            this.GCode = _GCode;
            this.UserVariables = _UserVariables;
        }



        public int probePosition = 0;

        public void setHeights(float value)
        {
            float zMaxLength = EEPROM.returnZMaxLength();
            float probingHeight = UserVariables.returnProbingHeight();

            if (value != 200)
            {
                switch (probePosition)
                {
                    case 0:
                        probePosition = zMaxLength - probingHeight + probePosition;
                        Heights.setCenter(probePosition);
                        probePosition++;
                        break;
                    case 1:
                        probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        Heights.setX(probePosition);
                        probePosition++;
                        break;
                    case 2:
                        probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        Heights.setXOpp(probePosition);
                        probePosition++;
                        break;
                    case 3:
                        probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        Heights.setY(probePosition);
                        probePosition++;
                        break;
                    case 4:
                        probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        Heights.setYOpp(probePosition);
                        probePosition++;
                        break;
                    case 5:
                        probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        Heights.setZ(probePosition);
                        probePosition++;
                        break;
                    case 6:
                        probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        Heights.setZOpp(probePosition);
                        probePosition = 0;

                        setHeights();

                        GCode.sendEEPROMVariable(3, 153, Heights.center);
                        UserInterface.logConsole("Setting Z Max Length\n");
                        Thread.Sleep(pauseTimeSet);

                        EEPROMClass.zMaxLength = Heights.center;
                        break;
                }
            }
        }

        public float parseZProbe(string value)
        {
            if (value.Contains("Z-probe:"))
            {
                //Z-probe: 10.66 zCorr: 0

                string[] parseInData = value.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string[] parseFirstLine = parseInData[0].Split(new char[] { ':', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                //: 10.66 zCorr: 0
                string[] parseZProbe = value.Split(new string[] { "Z-probe", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] parseZProbeSpace = parseZProbe[0].Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                float zProbeParse;

                //check if there is a space between
                if (parseZProbeSpace[0] == ":")
                {
                    //Space
                    zProbeParse = float.Parse(parseZProbeSpace[1]);
                }
                else
                {
                    //No space
                    zProbeParse = float.Parse(parseZProbeSpace[0].Substring(1));
                }

                return float.Parse(parseFirstLine[1]);
            }
            else
            {
                return 200;
            }
        }
    }
}
