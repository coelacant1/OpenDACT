using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class Communication
    {
        Connection Connection;

        public Communication(Connection _Connection)
        {
            this.Connection = _Connection;
        }

        public void sendToPosition(float x, float y, float z)
        {
            Connection._serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
        }

        public void homePrinter()
        {
            Connection._serialPort.WriteLine("G28");
        }
        
        public void probe()
        {
            Connection._serialPort.WriteLine("G31");
        }
    }
}
