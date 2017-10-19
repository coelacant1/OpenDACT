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
        
        public SerialPort()
        {

        }

        public Boolean IsOpen()
        {
            return serialStream.IsOpen;
        }

        public string[] GetPortNames()
        {
            return SerialPortStream.GetPortNames();
        }

        public string[] GetPortDescriptions()
        {
            List<string> descriptions = new List<string>();

            foreach (var desc in SerialPortStream.GetPortDescriptions())
            {
                Trace.WriteLine("GetPortDescriptions: " + desc.Port + "; Description: " + desc.Description);
                descriptions.Add(desc.Port + ": " + desc.Description);
            }

            return descriptions.ToArray();
        }

        public void WriteLine(string text)
        {
            if (serialStream.IsOpen && serialStream.CanRead)
            {
                serialStream.WriteLine(text);
            }
            else
            {
                throw new System.ArgumentException("Cannot write unless connected", "SerialPort WriteLine");
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
                throw new System.ArgumentException("Cannot read unless connected", "SerialPort ReadLine");
            }
        }
        
        public void Open(string port, string baudrate)
        {
            Int32.TryParse(baudrate, out int baud);

            serialStream = new SerialPortStream(port, baud);
            serialStream.Open();
            serialStream.GetPortSettings();
        }

        public void Close()
        {
            if (serialStream.IsOpen)
            {
                serialStream.Close();
            }
        }

        public void Dispose()
        {
            serialStream.Dispose();//cannot reopen once called
        }

        void OnPinChanged(SerialPinChangedEventArgs eventArgs)
        {

        }
    }
}
