using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    public class Heights
    {
        //store every set of heights
        public float center;
        public float X;
        public float XOpp;
        public float Y;
        public float YOpp;
        public float Z;
        public float ZOpp;
        public float teX;
        public float teXOpp;
        public float teY;
        public float teYOpp;
        public float teZ;
        public float teZOpp;
        public bool firstHeights = true;

        public Heights(float _center, float _X, float _XOpp, float _Y, float _YOpp, float _Z, float _ZOpp)
        {
            center = _center;
            X = _X;
            XOpp = _XOpp;
            Y = _Y;
            YOpp = _YOpp;
            Z = _Z;
            ZOpp = _ZOpp;

            if (firstHeights == true)
            {
                teX = _X;
                teXOpp = _XOpp;
                teY = _Y;
                teYOpp = _YOpp;
                teZ = _Z;
                teZOpp = _ZOpp;
                firstHeights = false;
            }
        }
    }
    
    public static class HeightFunctions
    {
        public static float tempCenter;
        public static float tempX;
        public static float tempXOpp;
        public static float tempY;
        public static float tempYOpp;
        public static float tempZ;
        public static float tempZOpp;
        private static int position = 0;
        public static bool heightsSet = false;
        public static bool checkHeightsOnly = false;

        public static Heights returnHeightObject()
        {
            Heights heights = new Heights(tempCenter, tempX, tempXOpp, tempY, tempYOpp, tempZ, tempZOpp);
            return heights;
        }

        public static void setHeights(float probePosition, ref EEPROM eeprom, ref UserVariables userVariables)
        {
            float zMaxLength = eeprom.zMaxLength;
            float probingHeight = userVariables.probingHeight;

            if (probePosition != 200)
            {
                switch (position)
                {
                    case 0:
                        probePosition = zMaxLength - probingHeight + probePosition;
                        tempCenter = probePosition;
                        position++;
                        break;
                    case 1:
                        probePosition = tempCenter - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        tempX = probePosition;
                        position++;
                        break;
                    case 2:
                        probePosition = tempCenter - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        tempXOpp = probePosition;
                        position++;
                        break;
                    case 3:
                        probePosition = tempCenter - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        tempY = probePosition;
                        position++;
                        break;
                    case 4:
                        probePosition = tempCenter - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        tempYOpp = probePosition;
                        position++;
                        break;
                    case 5:
                        probePosition = tempCenter - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        tempZ = probePosition;
                        position++;
                        break;
                    case 6:
                        probePosition = tempCenter - (zMaxLength - probingHeight + probePosition);
                        probePosition = -probePosition;
                        tempZOpp = probePosition;
                        position = 0;
                        
                        //setHeightMap();

                        GCode.sendEEPROMVariable(3, 153, tempCenter);
                        UserInterface.logConsole("Setting Z Max Length");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        eeprom.zMaxLength = tempCenter;

                        heightsSet = true;
                        break;
                }
            }
        }

        public static float parseZProbe(string value)
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
                return 1000;
            }
        }
    }
}
