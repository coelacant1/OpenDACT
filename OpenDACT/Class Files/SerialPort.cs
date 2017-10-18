using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RJCP.IO.Ports;
using System.Diagnostics;

namespace OpenDACT.Class_Files
{
    class SerialPort
    {
        private SerialPortStream serialStream;
        private delegate SerialPinChange Test();


        SerialPort()
        {
            foreach (string c in SerialPortStream.GetPortNames())
            {
                Trace.WriteLine("GetPortNames: " + c);
            }

            foreach (var desc in SerialPortStream.GetPortDescriptions())
            {
                Trace.WriteLine("GetPortDescriptions: " + desc.Port + "; Description: " + desc.Description);
            }

            //Use for listener
            SerialPinChangedEventArgs test = serialStream.PinChanged;

            Printer printer = new Printer();
            printer.kinematics.carriageOffset.X = 0.0;
           

            printer.bedHeightMap.xOpposite = 0.0;
            printer.kinematics.carriageOffset.x = 0.0;
            printer.kinematics.stepsPerMM;

        }

        public void WriteLine(string text)
        {
            if (serialStream.IsOpen && serialStream.CanRead)
            {
                serialStream.WriteLine(text);
            }
            else
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
        }

        public string ReadLine()
        {
            if (serialStream.IsOpen && serialStream.CanRead)
            {
                return serialStream.ReadLine();
            }
            else
            {
                throw new System.ArgumentException("Parameter cannot be null", "original");
            }
        }
        
        public void Open(string port, int baudrate)
        {
            serialStream = new SerialPortStream(port, baudrate);
            serialStream.Open();
            serialStream.GetPortSettings();
        }

        public void Close()
        {
            serialStream.Close();
        }

        public void Dispose()
        {
            serialStream.Dispose();//cannot reopen once called
        }
    }
}
