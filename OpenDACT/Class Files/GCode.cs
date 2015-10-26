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
        public static bool checkHeights = false;

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

        public static void positionFlow()
        {
            float probingHeight = UserVariables.probingHeight;
            float plateDiameter = UserVariables.plateDiameter;
            int pauseTimeSet = UserVariables.pauseTimeSet;
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



        public static void heuristicLearning(ref EEPROM eeprom)
        {
            //find base heights
            //find heights with each value increased by 1 - HRad, tower offset 1-3, diagonal rod

            if (UserVariables.advancedCalCount == 0)
            {//start
                if (Connection._serialPort.IsOpen)
                {
                    //set diagonal rod +1
                    GCode.sendEEPROMVariable(3, 881, eeprom.stepsPerMM + 1);
                    UserInterface.logConsole("Setting diagonal rod to: " + (eeprom.stepsPerMM + 1).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;
            }
            else if (UserVariables.advancedCalCount == 1)
            {//get diagonal rod percentages

                UserVariables.deltaTower = ((Heights.teX - Heights.X) + (Heights.teY - Heights.Y) + (Heights.teZ - Heights.Z)) / 3;
                UserVariables.deltaOpp = ((Heights.teXOpp - Heights.XOpp) + (Heights.teYOpp - Heights.YOpp) + (Heights.teZOpp - Heights.ZOpp)) / 3;

                if (Connection._serialPort.IsOpen)
                {
                    //reset diagonal rod
                    GCode.sendEEPROMVariable(3, 881, eeprom.stepsPerMM);
                    UserInterface.logConsole("Setting diagonal rod to: " + (eeprom.stepsPerMM).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                    //set Hrad +1
                    GCode.sendEEPROMVariable(3, 885, eeprom.HRadius + 1);
                    UserInterface.logConsole("Setting Horizontal Radius to: " + (eeprom.HRadius + 1).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;
            }
            else if (UserVariables.advancedCalCount == 2)
            {//get HRad percentages
                UserVariables.HRadRatio = -(Math.Abs((Heights.X - Heights.teX) + (Heights.Y - Heights.teY) + (Heights.Z - Heights.teZ) + (Heights.XOpp - Heights.teXOpp) + (Heights.YOpp - Heights.teYOpp) + (Heights.ZOpp - Heights.teZOpp))) / 6;

                if (Connection._serialPort.IsOpen)
                {
                    //reset horizontal radius
                    GCode.sendEEPROMVariable(3, 885, eeprom.HRadius);
                    UserInterface.logConsole("Setting Horizontal Radius to: " + (eeprom.HRadius).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                    //set X offset
                    GCode.sendEEPROMVariable(1, 893, eeprom.offsetX + 80);
                    UserInterface.logConsole("Setting offset X to: " + (eeprom.offsetX + 80).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;
            }
            else if (UserVariables.advancedCalCount == 3)
            {//get X offset percentages

                UserVariables.offsetXCorrection = Math.Abs(1 / (Heights.X - Heights.teX));
                UserVariables.xxOppPerc = Math.Abs((Heights.XOpp - Heights.teXOpp) / (Heights.X - Heights.teX));
                UserVariables.xyPerc = Math.Abs((Heights.Y - Heights.teY) / (Heights.X - Heights.teX));
                UserVariables.xyOppPerc = Math.Abs((Heights.YOpp - Heights.teYOpp) / (Heights.X - Heights.teX));
                UserVariables.xzPerc = Math.Abs((Heights.Z - Heights.teZ) / (Heights.X - Heights.teX));
                UserVariables.xzOppPerc = Math.Abs((Heights.ZOpp - Heights.teZOpp) / (Heights.X - Heights.teX));

                if (Connection._serialPort.IsOpen)
                {
                    //reset X offset
                    GCode.sendEEPROMVariable(1, 893, eeprom.offsetX);
                    UserInterface.logConsole("Setting offset X to: " + (eeprom.offsetX).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                    //set Y offset
                    GCode.sendEEPROMVariable(1, 895, eeprom.offsetY + 80);
                    UserInterface.logConsole("Setting offset Y to: " + (eeprom.offsetY + 80).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;
            }
            else if (UserVariables.advancedCalCount == 4)
            {//get Y offset percentages

                UserVariables.offsetYCorrection = Math.Abs(1 / (Heights.Y - Heights.teY));
                UserVariables.yyOppPerc = Math.Abs((Heights.YOpp - Heights.teYOpp) / (Heights.Y - Heights.teY));
                UserVariables.yxPerc = Math.Abs((Heights.X - Heights.teX) / (Heights.Y - Heights.teY));
                UserVariables.yxOppPerc = Math.Abs((Heights.XOpp - Heights.teXOpp) / (Heights.Y - Heights.teY));
                UserVariables.yzPerc = Math.Abs((Heights.Z - Heights.teZ) / (Heights.Y - Heights.teY));
                UserVariables.yzOppPerc = Math.Abs((Heights.ZOpp - Heights.teZOpp) / (Heights.Y - Heights.teY));

                if (Connection._serialPort.IsOpen)
                {
                    //reset Y offset
                    GCode.sendEEPROMVariable(1, 895, eeprom.offsetY);
                    UserInterface.logConsole("Setting offset Y to: " + (eeprom.offsetY).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                    //set Z offset
                    GCode.sendEEPROMVariable(1, 897, eeprom.offsetZ + 80);
                    UserInterface.logConsole("Setting offset Z to: " + (eeprom.offsetZ + 80).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;
            }
            else if (UserVariables.advancedCalCount == 5)
            {//get Z offset percentages

                UserVariables.offsetZCorrection = Math.Abs(1 / (Heights.Z - Heights.teZ));
                UserVariables.zzOppPerc = Math.Abs((Heights.ZOpp - Heights.teZOpp) / (Heights.Z - Heights.teZ));
                UserVariables.zxPerc = Math.Abs((Heights.X - Heights.teX) / (Heights.Z - Heights.teZ));
                UserVariables.zxOppPerc = Math.Abs((Heights.XOpp - Heights.teXOpp) / (Heights.Z - Heights.teZ));
                UserVariables.zyPerc = Math.Abs((Heights.Y - Heights.teY) / (Heights.Z - Heights.teZ));
                UserVariables.zyOppPerc = Math.Abs((Heights.YOpp - Heights.teYOpp) / (Heights.Z - Heights.teZ));

                if (Connection._serialPort.IsOpen)
                {
                    //set Z offset
                    GCode.sendEEPROMVariable(1, 897, eeprom.offsetZ);
                    UserInterface.logConsole("Setting offset Z to: " + (eeprom.offsetZ).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                    //set alpha rotation offset perc X
                    GCode.sendEEPROMVariable(3, 901, eeprom.A + 1);
                    UserInterface.logConsole("Setting Alpha A to: " + (eeprom.A + 1).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;

            }
            else if (UserVariables.advancedCalCount == 6)//6
            {//get A alpha rotation

                UserVariables.alphaRotationPercentageX = (2 / Math.Abs((Heights.YOpp - Heights.ZOpp) - (Heights.teYOpp - Heights.teZOpp)));

                if (Connection._serialPort.IsOpen)
                {
                    //set alpha rotation offset perc X
                    GCode.sendEEPROMVariable(3, 901, eeprom.A);
                    UserInterface.logConsole("Setting Alpha A to: " + (eeprom.A).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                    //set alpha rotation offset perc Y
                    GCode.sendEEPROMVariable(3, 905, eeprom.B + 1);
                    UserInterface.logConsole("Setting Alpha B to: " + (eeprom.B + 1).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;
            }
            else if (UserVariables.advancedCalCount == 7)//7
            {//get B alpha rotation

                UserVariables.alphaRotationPercentageY = (2 / Math.Abs((Heights.ZOpp - Heights.XOpp) - (Heights.teXOpp - Heights.teXOpp)));

                if (Connection._serialPort.IsOpen)
                {
                    //set alpha rotation offset perc Y
                    GCode.sendEEPROMVariable(3, 905, eeprom.B);
                    UserInterface.logConsole("Setting Alpha B to: " + (eeprom.B).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                    //set alpha rotation offset perc Z
                    GCode.sendEEPROMVariable(3, 909, eeprom.C + 1);
                    UserInterface.logConsole("Setting Alpha C to: " + (eeprom.C + 1).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);
                }

                //check heights

                UserVariables.advancedCalCount++;
            }
            else if (UserVariables.advancedCalCount == 8)//8
            {//get C alpha rotation

                UserVariables.alphaRotationPercentageZ = (2 / Math.Abs((Heights.XOpp - Heights.YOpp) - (Heights.teXOpp - Heights.teYOpp)));

                if (Connection._serialPort.IsOpen)
                {
                    //set alpha rotation offset perc Z
                    GCode.sendEEPROMVariable(3, 909, eeprom.C);
                    UserInterface.logConsole("Setting Alpha C to: " + (eeprom.C).ToString());
                    Thread.Sleep(UserVariables.pauseTimeSet);

                }

                UserInterface.logConsole("Alpha offset percentages: " + UserVariables.alphaRotationPercentageX + ", " + UserVariables.alphaRotationPercentageY + ", and" + UserVariables.alphaRotationPercentageZ);

                UserVariables.advancedCalibration = false;
                UserVariables.advancedCalCount = 0;

                //check heights

                UserInterface.setAdvancedCalVars();
            }
        }
    }
}
