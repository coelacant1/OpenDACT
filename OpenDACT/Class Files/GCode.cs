using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    class GCode
    {
        Connection Connection;
        UserInterface UserInterface;

        public GCode(Connection _Connection, UserInterface _UserInterface)
        {
            this.Connection = _Connection;
            this.UserInterface = _UserInterface;
        }


        public int currentPosition = 0;

        public void sendToPosition(float X, float Y, float Z)
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

        public void homeAxes()
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
        public void probe()
        {
            if (Connection._serialPort.IsOpen)
            {
                Connection._serialPort.WriteLine("G31");
            }
            else
            {
                UserInterface.logConsole("Not Connected\n");
            }
        }
        public void sendReadEEPROMCommand()
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

        public void sendEEPROMVariable(int type, int position, float value)
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

        public void positionFlow()
        {
            switch (currentPosition)
            {
                case 0:
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    probe();
                    currentPosition++;
                    break;
                case 1:
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G30");
                    currentPosition++;
                    break;
                case 2:
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G30");
                    currentPosition++;
                    break;
                case 3:
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G30");
                    currentPosition++;
                    break;
                case 4:
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G30");
                    currentPosition++;
                    break;
                case 5:
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y-" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G30");
                    currentPosition++;
                    break;
                case 6:
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y-" + valueZ.ToString());
                    Thread.Sleep(pauseTimeSet);
                    Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                    Thread.Sleep(pauseTimeSet);
                    currentPosition = 0;
                    break;
            }
        }
    }
}
}
