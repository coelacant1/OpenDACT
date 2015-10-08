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
        public float center;
        public float X;
        public float XOpp;
        public float Y;
        public float YOpp;
        public float Z;
        public float ZOpp;
        
        public Heights(float _center, float _X, float _XOpp, float _Y, float _YOpp, float _Z, float _ZOpp)
        {
            center = _center;
            X = _X;
            XOpp = _XOpp;
            Y = _Y;
            YOpp = _YOpp;
            Z = _Z;
            ZOpp = _ZOpp;
        }
    }
    
    class HeightFunctions
    {
        UserInterface UserInterface;
        GCode GCode;
        UserVariables UserVariables;
        Calibration Calibration;

        public HeightFunctions(UserInterface _UserInterface, GCode _GCode, UserVariables _UserVariables, Calibration _Calibration)
        {
            this.UserInterface = _UserInterface;
            this.GCode = _GCode;
            this.UserVariables = _UserVariables;
            this.Calibration = _Calibration;
        }


        private static float tempCenter;
        private static float tempX;
        private static float tempXOpp;
        private static float tempY;
        private static float tempYOpp;
        private static float tempZ;
        private static float tempZOpp;
        private static int position = 0;

        public Heights returnHeightObject()
        {
            Heights heightObject = new Heights(tempCenter, tempX, tempXOpp, tempY, tempYOpp, tempZ, tempZOpp);
            return heightObject;
        }
        public void setHeights(float probePosition)
        {
            float zMaxLength = EEPROM.returnZMaxLength();
            float probingHeight = UserVariables.returnProbingHeight();

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

                        Calibration.calibrationState = true;
                        setHeights();

                        GCode.sendEEPROMVariable(3, 153, tempCenter);
                        UserInterface.logConsole("Setting Z Max Length\n");
                        Thread.Sleep(UserVariables.pauseTimeSet);

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
