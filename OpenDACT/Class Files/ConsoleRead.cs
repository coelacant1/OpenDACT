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
        int i = 0;
        public void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = Connection._serialPort.ReadLine();
                    
                    UserInterface.logConsole(message + "\n");

                    if (EEPROM.EEPROMSet == false)
                    {
                        int intParse;
                        float floatParse2;

                        EEPROM.parseEEPROM(message, out intParse, out floatParse2);
                        EEPROM.setEEPROM(intParse, floatParse2);
                    }
                    
                    EEPROM.parseZProbe(message);
                }
                catch (TimeoutException) { }
            }//end while
        }//end void

    }
}
