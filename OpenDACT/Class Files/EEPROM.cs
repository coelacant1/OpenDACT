using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class EEPROMClass
    {
        public float stepsPerMM;
        public float tempSPM;
        public float zMaxLength;
        public float zProbe;
        public float HRad;
        public float offsetX;
        public float offsetY;
        public float offsetZ;
        public float A;
        public float B;
        public float C;
        public float DA;
        public float DB;
        public float DC;
    }

    class EEPROM
    {
        UserInterface UserInterface;
        GCode Gcode;

        public EEPROM(UserInterface _UserInterface, GCode _GCode)
        {
            this.UserInterface = _UserInterface;
            this.Gcode = _GCode;
        }



        //public object EEPROMVariables = new object();
        public bool EEPROMSet = false;


        public object createEEPROM()
        {
            object EEPROMObject = new EEPROMClass();
            EEPROMClass EEPROMVars = (EEPROMClass)EEPROMObject;

            //example
            EEPROMVars.stepsPerMM++;

            return EEPROMVars;
        }

        public void readEEPROM(string value)
        {
            GCode.sendReadEEPROMCommand();
        }

        public object returnEEPROM()
        {
            return EEPROMVariables;
        }

        public void parseEEPROM(string value)
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

                int intParse;
                double doubleParse2;

                //check if there is a space between
                if (parseEPRSpace[0] == ":")
                {
                    //Space
                    intParse = int.Parse(parseEPRSpace[2]);
                    doubleParse2 = double.Parse(parseEPRSpace[3]);
                }
                else
                {
                    //No space
                    intParse = int.Parse(parseEPRSpace[1]);
                    doubleParse2 = double.Parse(parseEPRSpace[2]);
                }

                switch (intParse)
                {
                    case 11:
                        UserInterface.logConsole("EEPROM capture initiated\n");

                        object EEPROMVariables = createEEPROM();

                        EEPROMVariables.stepsPerMM = doubleParse2;
                        tempSPM = stepsPerMM;
                        break;
                    case 153:
                        zMaxLength = doubleParse2;
                        break;
                    case 808:
                        zProbe = doubleParse2;
                        break;
                    case 885:
                        HRad = doubleParse2;
                        break;
                    case 893:
                        offsetX = doubleParse2;
                        break;
                    case 895:
                        offsetY = doubleParse2;
                        break;
                    case 897:
                        offsetZ = doubleParse2;
                        break;
                    case 901:
                        A = doubleParse2;
                        break;
                    case 905:
                        B = doubleParse2;
                        break;
                    case 909:
                        C = doubleParse2;
                        break;
                    case 913:
                        DA = doubleParse2;
                        break;
                    case 917:
                        DB = doubleParse2;
                        break;
                    case 921:
                        DC = doubleParse2;
                        EEPROMSet = true;
                        break;
                }
            }//end EEProm capture

        }//end class
        public void parseZProbe(string value)
        {

            if (value.Contains("Z-probe:"))
            {
                //Z-probe: 10.66 zCorr: 0

                string[] parseInData = value.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string[] parseFirstLine = parseInData[0].Split(new char[] { ':', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                //: 10.66 zCorr: 0
                string[] parseZProbe = value.Split(new string[] { "Z-probe", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] parseZProbeSpace = parseZProbe[0].Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                double zProbeParse;

                //check if there is a space between
                if (parseZProbeSpace[0] == ":")
                {
                    //Space
                    zProbeParse = double.Parse(parseZProbeSpace[1]);
                }
                else
                {
                    //No space
                    zProbeParse = double.Parse(parseZProbeSpace[0].Substring(1));
                }

                /*
                //use returned probe height to calculate the actual z-Probe height
                if (zProbeSet == 1)
                {
                    LogConsole("Z-Probe length set to: " + (zMaxLength - Convert.ToDouble(parseFirstLine[1])) + "\n");
                    zProbe = zMaxLength - Convert.ToDouble(parseFirstLine[1]);
                    zProbeSet = 0;
                }
                else if (centerIterations == iterationNum)
                {
                    //LogConsole("Z-Probe Center Height: " + parseFirstLine[1] + "\n");
                    centerHeight = Convert.ToDouble(parseFirstLine[1]);

                    centerIterations++;

                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");

                    Thread.Sleep(pauseTimeSet);
                    //X axis
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G30");
                }
                else if (xIterations == iterationNum)
                {
                    //LogMessage("Z-Probe X Height: " + parseFirstLine[1] + "\n");
                    X = Convert.ToDouble(parseFirstLine[1]);

                    xIterations++;

                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G30");
                }
                else if (xOppIterations == iterationNum)
                {
                    //LogMessage("Z-Probe X Opposite Height: " + parseFirstLine[1] + "\n");
                    XOpp = Convert.ToDouble(parseFirstLine[1]);

                    xOppIterations++;

                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);

                    //Y axis
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G30");
                }
                else if (yIterations == iterationNum)
                {
                    //LogMessage("Z-Probe Y Height: " + parseFirstLine[1] + "\n");
                    Y = Convert.ToDouble(parseFirstLine[1]);

                    yIterations++;

                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G30");
                }
                else if (yOppIterations == iterationNum)
                {
                    //LogMessage("Z-Probe Y Opposite Height: " + parseFirstLine[1] + "\n");
                    YOpp = Convert.ToDouble(parseFirstLine[1]);

                    yOppIterations++;

                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);

                    //Z axis
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G30");
                }
                else if (zIterations == iterationNum)
                {
                    //LogMessage("Z-Probe Z Height: " + parseFirstLine[1] + "\n");
                    Z = Convert.ToDouble(parseFirstLine[1]);

                    zIterations++;

                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y-" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G30");
                }
                else if (zOppIterations == iterationNum)
                {
                    //LogMessage("Z-Probe Z Opposite Height: " + parseFirstLine[1] + "\n");
                    ZOpp = Convert.ToDouble(parseFirstLine[1]);

                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y-" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);

                    centerHeight = zMaxLength - probingHeight + centerHeight;
                    X = centerHeight - (zMaxLength - probingHeight + X);
                    XOpp = centerHeight - (zMaxLength - probingHeight + XOpp);
                    Y = centerHeight - (zMaxLength - probingHeight + Y);
                    YOpp = centerHeight - (zMaxLength - probingHeight + YOpp);
                    Z = centerHeight - (zMaxLength - probingHeight + Z);
                    ZOpp = centerHeight - (zMaxLength - probingHeight + ZOpp);

                    //invert values
                    X = -X;
                    XOpp = -XOpp;
                    Y = -Y;
                    YOpp = -YOpp;
                    Z = -Z;
                    ZOpp = -ZOpp;

                    // Sets height-maps in separate function
                    setHeights();

                    _serialPort.WriteLine("M206 T3 P153 X" + centerHeight);
                    LogConsole("Setting Z Max Length\n");
                    Thread.Sleep(pauseTimeSet);

                    zMaxLength = centerHeight;

                    zOppIterations++;

                }
                */

            }
        }
    }
}
