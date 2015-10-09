using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    static class GCode
    {
        public static int currentPosition = 0;
        public static bool checkHeights = true;

        public static void sendToPosition(float X, float Y, float Z)
        {
            if (Connection._serialPort.IsOpen)
            {
                Connection._serialPort.WriteLine("G1 Z" + Z.ToString() + " X" + X.ToString() + " Y" + Y.ToString());
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }

        public static void homeAxes()
        {
            if (Connection._serialPort.IsOpen)
            {
                Connection._serialPort.WriteLine("G28");
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }
        private static void probe()
        {
            if (Connection._serialPort.IsOpen)
            {
                Connection._serialPort.WriteLine("G30");
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }
        public static void emergencyReset()
        {
            if (Connection._serialPort.IsOpen)
            {
                Connection._serialPort.WriteLine("M112");
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }
        public static void sendReadEEPROMCommand()
        {
            if (Connection._serialPort.IsOpen)
            {
                Connection._serialPort.WriteLine("M205");
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }

        public static void sendEEPROMVariable(int type, int position, float value)
        {
            if (Connection._serialPort.IsOpen)
            {
                if (type == 1)
                {
                    Connection._serialPort.WriteLine("M206 M206 T1 P" + position + " S" + value);
                }
                else if (type == 3)
                {
                    Connection._serialPort.WriteLine("M206 M206 T3 P" + position + " X" + value);
                }
                else
                {
                    UserInterface.logConsole("Invalid EEPROM Variable.");
                }
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }

        public static void pauseTime()
        {
            Thread.Sleep(2500);
        }

        public static void positionFlow(ref UserVariables userVariables)
        {
            float probingHeight = userVariables.probingHeight;
            float plateDiameter = userVariables.plateDiameter;
            int pauseTimeSet = userVariables.pauseTimeSet;
            float valueZ = 0.482F * plateDiameter;
            float valueXYLarge = 0.417F * plateDiameter;
            float valueXYSmall = 0.241F * plateDiameter;

            switch (currentPosition)
            {
                case 0:
                    homeAxes();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    pauseTime();
                    probe();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    currentPosition++;
                    break;
                case 1:
                    probe();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    pauseTime();
                    currentPosition++;
                    break;
                case 2:
                    probe();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    pauseTime();
                    currentPosition++;
                    break;
                case 3:
                    probe();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    pauseTime();
                    currentPosition++;
                    break;
                case 4:
                    probe();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y" + valueZ.ToString());
                    pauseTime();
                    currentPosition++;
                    break;
                case 5:
                    probe();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y" + valueZ.ToString());
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y-" + valueZ.ToString());
                    pauseTime();
                    currentPosition++;
                    break;
                case 6:
                    probe();
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y-" + valueZ.ToString());
                    pauseTime();
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    pauseTime();
                    currentPosition = 0;
                    checkHeights = false;
                    break;
            }
        }



        public static void heuristicLearning(ref EEPROM eeprom, ref UserVariables userVariables, ref Heights heights)
        {
            if (userVariables.advancedCalibration == true)
            {
                //find base heights
                //find heights with each value increased by 1 - HRad, tower offset 1-3, diagonal rod

                if (userVariables.advancedCalCount == 0)
                {//start
                    if (Connection._serialPort.IsOpen)
                    {
                        //set diagonal rod +1
                        GCode.sendEEPROMVariable(3, 881, eeprom.stepsPerMM + 1);
                        UserInterface.logConsole("Setting diagonal rod to: " + (eeprom.stepsPerMM + 1).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 1)
                {//get diagonal rod percentages

                    userVariables.deltaTower = ((heights.teX - heights.X) + (heights.teY - heights.Y) + (heights.teZ - heights.Z)) / 3;
                    userVariables.deltaOpp = ((heights.teXOpp - heights.XOpp) + (heights.teYOpp - heights.YOpp) + (heights.teZOpp - heights.ZOpp)) / 3;

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset diagonal rod
                        GCode.sendEEPROMVariable(3, 881, eeprom.stepsPerMM);
                        UserInterface.logConsole("Setting diagonal rod to: " + (eeprom.stepsPerMM).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set Hrad +1
                        GCode.sendEEPROMVariable(3, 885, eeprom.HRadius + 1);
                        UserInterface.logConsole("Setting Horizontal Radius to: " + (eeprom.HRadius + 1).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 2)
                {//get HRad percentages
                    userVariables.HRadRatio = -(Math.Abs((heights.X - heights.teX) + (heights.Y - heights.teY) + (heights.Z - heights.teZ) + (heights.XOpp - heights.teXOpp) + (heights.YOpp - heights.teYOpp) + (heights.ZOpp - heights.teZOpp))) / 6;

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset horizontal radius
                        GCode.sendEEPROMVariable(3, 885, eeprom.HRadius);
                        UserInterface.logConsole("Setting Horizontal Radius to: " + (eeprom.HRadius).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set X offset
                        GCode.sendEEPROMVariable(1, 893, eeprom.offsetX + 80);
                        UserInterface.logConsole("Setting offset X to: " + (eeprom.offsetX + 80).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 3)
                {//get X offset percentages

                    userVariables.offsetXCorrection = Math.Abs(1 / (heights.X - heights.teX));
                    userVariables.xxOppPerc = Math.Abs((heights.XOpp - heights.teXOpp) / (heights.X - heights.teX));
                    userVariables.xyPerc = Math.Abs((heights.Y - heights.teY) / (heights.X - heights.teX));
                    userVariables.xyOppPerc = Math.Abs((heights.YOpp - heights.teYOpp) / (heights.X - heights.teX));
                    userVariables.xzPerc = Math.Abs((heights.Z - heights.teZ) / (heights.X - heights.teX));
                    userVariables.xzOppPerc = Math.Abs((heights.ZOpp - heights.teZOpp) / (heights.X - heights.teX));

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset X offset
                        GCode.sendEEPROMVariable(1, 893, eeprom.offsetX);
                        UserInterface.logConsole("Setting offset X to: " + (eeprom.offsetX).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set Y offset
                        GCode.sendEEPROMVariable(1, 895, eeprom.offsetY + 80);
                        UserInterface.logConsole("Setting offset Y to: " + (eeprom.offsetY + 80).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 4)
                {//get Y offset percentages

                    userVariables.offsetYCorrection = Math.Abs(1 / (heights.Y - heights.teY));
                    userVariables.yyOppPerc = Math.Abs((heights.YOpp - heights.teYOpp) / (heights.Y - heights.teY));
                    userVariables.yxPerc = Math.Abs((heights.X - heights.teX) / (heights.Y - heights.teY));
                    userVariables.yxOppPerc = Math.Abs((heights.XOpp - heights.teXOpp) / (heights.Y - heights.teY));
                    userVariables.yzPerc = Math.Abs((heights.Z - heights.teZ) / (heights.Y - heights.teY));
                    userVariables.yzOppPerc = Math.Abs((heights.ZOpp - heights.teZOpp) / (heights.Y - heights.teY));

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset Y offset
                        GCode.sendEEPROMVariable(1, 895, eeprom.offsetY);
                        UserInterface.logConsole("Setting offset Y to: " + (eeprom.offsetY).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set Z offset
                        GCode.sendEEPROMVariable(1, 897, eeprom.offsetZ + 80);
                        UserInterface.logConsole("Setting offset Z to: " + (eeprom.offsetZ + 80).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 5)
                {//get Z offset percentages

                    userVariables.offsetZCorrection = Math.Abs(1 / (heights.Z - heights.teZ));
                    userVariables.zzOppPerc = Math.Abs((heights.ZOpp - heights.teZOpp) / (heights.Z - heights.teZ));
                    userVariables.zxPerc = Math.Abs((heights.X - heights.teX) / (heights.Z - heights.teZ));
                    userVariables.zxOppPerc = Math.Abs((heights.XOpp - heights.teXOpp) / (heights.Z - heights.teZ));
                    userVariables.zyPerc = Math.Abs((heights.Y - heights.teY) / (heights.Z - heights.teZ));
                    userVariables.zyOppPerc = Math.Abs((heights.YOpp - heights.teYOpp) / (heights.Z - heights.teZ));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set Z offset
                        GCode.sendEEPROMVariable(1, 897, eeprom.offsetZ);
                        UserInterface.logConsole("Setting offset Z to: " + (eeprom.offsetZ).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set alpha rotation offset perc X
                        GCode.sendEEPROMVariable(3, 901, eeprom.A + 1);
                        UserInterface.logConsole("Setting Alpha A to: " + (eeprom.A + 1).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;

                }
                else if (userVariables.advancedCalCount == 6)//6
                {//get A alpha rotation

                    userVariables.alphaRotationPercentageX = (2 / Math.Abs((heights.YOpp - heights.ZOpp) - (heights.teYOpp - heights.teZOpp)));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc X
                        GCode.sendEEPROMVariable(3, 901, eeprom.A);
                        UserInterface.logConsole("Setting Alpha A to: " + (eeprom.A).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set alpha rotation offset perc Y
                        GCode.sendEEPROMVariable(3, 905, eeprom.B + 1);
                        UserInterface.logConsole("Setting Alpha B to: " + (eeprom.B + 1).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 7)//7
                {//get B alpha rotation

                    userVariables.alphaRotationPercentageY = (2 / Math.Abs((heights.ZOpp - heights.XOpp) - (heights.teXOpp - heights.teXOpp)));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc Y
                        GCode.sendEEPROMVariable(3, 905, eeprom.B);
                        UserInterface.logConsole("Setting Alpha B to: " + (eeprom.B).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set alpha rotation offset perc Z
                        GCode.sendEEPROMVariable(3, 909, eeprom.C + 1);
                        UserInterface.logConsole("Setting Alpha C to: " + (eeprom.C + 1).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 8)//8
                {//get C alpha rotation

                    userVariables.alphaRotationPercentageZ = (2 / Math.Abs((heights.XOpp - heights.YOpp) - (heights.teXOpp - heights.teYOpp)));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc Z
                        GCode.sendEEPROMVariable(3, 909, eeprom.C);
                        UserInterface.logConsole("Setting Alpha C to: " + (eeprom.C).ToString());
                        Thread.Sleep(userVariables.pauseTimeSet);

                    }

                    UserInterface.logConsole("Alpha offset percentages: " + userVariables.alphaRotationPercentageX + ", " + userVariables.alphaRotationPercentageY + ", and" + userVariables.alphaRotationPercentageZ);

                    userVariables.advancedCalibration = false;
                    userVariables.advancedCalCount = 0;

                    //check heights

                    UserInterface.setAdvancedCalVars();
                }
            }
        }
    }
}
