using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    class HeightVariables
    {
        //store every set of heights
        public static float center;
        public static float X;
        public static float XOpp;
        public static float Y;
        public static float YOpp;
        public static float Z;
        public static float ZOpp;

        public static void setCenter(float value)
        {
            center = value;
        }
        public static void setX(float value)
        {
            X = value;
        }
        public static void setXOpp(float value)
        {
            XOpp = value;
        }
        public static void setY(float value)
        {
            Y = value;
        }
        public static void setYOpp(float value)
        {
            YOpp = value;
        }
        public static void setZ(float value)
        {
            Z = value;
        }
        public static void setZOpp(float value)
        {
            ZOpp = value;
        }
    }

    class Heights
    {
        UserInterface UserInterface;
        GCode GCode;

        public Heights(UserInterface _UserInterface, GCode _GCode)
        {
            this.UserInterface = _UserInterface;
            this.GCode = _GCode;
        }



        public int probePosition = 0;

        public void setHeights(float value)
        {
            if (value != 200)
            {
                switch (probePosition)
                {
                    case 0:
                        probePosition = zMaxLength - probingHeight + probePosition;
                        HeightVariables.setCenter(probePosition);
                        probePosition++;
                        break;
                    case 1:
                        probePosition = HeightVariables.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        HeightVariables.setX(probePosition);
                        probePosition++;
                        break;
                    case 2:
                        probePosition = HeightVariables.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        HeightVariables.setXOpp(probePosition);
                        probePosition++;
                        break;
                    case 3:
                        probePosition = HeightVariables.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        HeightVariables.setY(probePosition);
                        probePosition++;
                        break;
                    case 4:
                        probePosition = HeightVariables.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        HeightVariables.setYOpp(probePosition);
                        probePosition++;
                        break;
                    case 5:
                        probePosition = HeightVariables.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        HeightVariables.setZ(probePosition);
                        probePosition++;
                        break;
                    case 6:
                        probePosition = HeightVariables.center - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        HeightVariables.setZOpp(probePosition);
                        probePosition = 0;

                        setHeights();

                        GCode.sendEEPROMVariable(3, 153, HeightVariables.center);
                        UserInterface.logConsole("Setting Z Max Length\n");
                        Thread.Sleep(pauseTimeSet);

                        EEPROMClass.zMaxLength = HeightVariables.center;
                        break;
                }
            }
        }

        public void parseZProbe(string value, out float height)
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

                height = float.Parse(parseFirstLine[1]);
            }
            else
            {
                height = 200;
            }
        }
    }
}
