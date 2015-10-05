using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class ConsoleRead
    {
        Connection Connection;
        UserInterface UserInterface;
        EEPROM EEPROM;

        public ConsoleRead(Connection _Connection, UserInterface _UserInterface, EEPROM _EEPROM)
        {
            this.Connection = _Connection;
            this.UserInterface = _UserInterface;
            this.EEPROM = _EEPROM;
        }



        public bool _continue;

        public void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = Connection._serialPort.ReadLine();
                    
                    UserInterface.logConsole(message + "\n");

                    if (EEPROM.EEPROMSet == true)
                    {
                        //initiate
                        //enable calibration
                    }
                    else
                    {
                        EEPROM.parseEEPROM(message);
                    }
                    

                    EEPROM.parseZProbe(message);
                }
                catch (TimeoutException) { }
            }//end while
        }//end void

    }
}
