using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    public static class Heights
    {
        //store every set of heights
        public static float center;
        public static float X;
        public static float XOpp;
        public static float Y;
        public static float YOpp;
        public static float Z;
        public static float ZOpp;
        public static float teX;
        public static float teXOpp;
        public static float teY;
        public static float teYOpp;
        public static float teZ;
        public static float teZOpp;
        public static bool firstHeights = true;

        /*
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
        */
    }

    public static class HeightFunctions
    {
        private static int position = 0;
        public static bool heightsSet = false;
        public static bool checkHeightsOnly = false;

        public static void setHeights(float probePosition)
        {
            float zMaxLength = EEPROM.zMaxLength;
            float probingHeight = UserVariables.probingHeight;

            switch (position)
            {
                case 0:
                    probePosition = zMaxLength - probingHeight + probePosition;
                    Heights.center = probePosition;
                    position++;
                    break;
                case 1:
                    probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                    probePosition = -probePosition;
                    Heights.X = probePosition;
                    position++;
                    break;
                case 2:
                    probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                    probePosition = -probePosition;
                    Heights.XOpp = probePosition;
                    position++;
                    break;
                case 3:
                    probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                    probePosition = -probePosition;
                    Heights.Y = probePosition;
                    position++;
                    break;
                case 4:
                    probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                    probePosition = -probePosition;
                    Heights.YOpp = probePosition;
                    position++;
                    break;
                case 5:
                    probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                    probePosition = -probePosition;
                    Heights.Z = probePosition;
                    position++;
                    break;
                case 6:
                    probePosition = Heights.center - (zMaxLength - probingHeight + probePosition);
                    probePosition = -probePosition;
                    Heights.ZOpp = probePosition;
                    position = 0;

                    EEPROM.zMaxLength = Heights.center;

                    heightsSet = true;
                    break;
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
