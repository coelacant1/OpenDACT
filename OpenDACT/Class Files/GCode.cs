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
                UserInterface.logConsole("Not Connected\n");
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
                UserInterface.logConsole("Not Connected\n");
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
                UserInterface.logConsole("Not Connected\n");
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
                UserInterface.logConsole("Not Connected\n");
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
                UserInterface.logConsole("Not Connected\n");
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
                    UserInterface.logConsole("Invalid EEPROM Variable.\n");
                }
            }
            else
            {
                UserInterface.logConsole("Not Connected\n");
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
    }
}
