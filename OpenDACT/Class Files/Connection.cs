using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

namespace OpenDACT.Class_Files
{
    static class Connection
    {
        public static SerialPort _serialPort;
        public static Thread readThread;
        public static Thread calcThread;

        public static void connect()
        {
            if (_serialPort.IsOpen)
            {
                UserInterface.logConsole("Already Connected");
            }
            else
            {
                try
                {
                    // Opens a new thread if there has been a previous thread that has closed.
                    if (readThread.IsAlive == false)
                    {
                        readThread = new Thread(Threading.Read);
                        calcThread = new Thread(Threading.HandleRead);
                        _serialPort = new SerialPort();
                    }
                    
                    _serialPort.PortName = Program.mainFormTest.portsCombo.Text;
                    _serialPort.BaudRate = int.Parse(Program.mainFormTest.baudRateCombo.Text, , CultureInfo.InvariantCulture);

                    // Set the read/write timeouts.
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;

                    // Open the serial port and start reading on a reader thread.
                    // _continue is a flag used to terminate the app.

                    UserInterface.logConsole(Program.mainFormTest.portsCombo.Text);

                    if (Program.mainFormTest.portsCombo.Text != "" && Program.mainFormTest.baudRateCombo.Text != "")
                    {
                        _serialPort.Open();
                        Threading._continue = true;

                        readThread.Start();
                        calcThread.Start();
                        UserInterface.logConsole("Connected");
                    }
                    else
                    {
                        UserInterface.logConsole("Please fill all text boxes above");
                    }
                }
                catch (Exception e1)
                {
                    UserInterface.logConsole(e1.Message);
                    Threading._continue = false;

                    //check if connection is open
                    if (readThread.IsAlive)
                    {
                        readThread.Join();
                    }

                    if (calcThread.IsAlive)
                    {
                        calcThread.Join();
                    }

                    _serialPort.Close();
                }
            }
        }

        public static void disconnect()
        {
            if (_serialPort.IsOpen && readThread.IsAlive)
            {
                try
                {
                    Threading._continue = false;
                    readThread.Join();
                    calcThread.Join();
                    _serialPort.Close();
                    UserInterface.logConsole("Disconnected");
                }
                catch (Exception e1)
                {
                    UserInterface.logConsole(e1.Message);
                }
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }
    }
}
