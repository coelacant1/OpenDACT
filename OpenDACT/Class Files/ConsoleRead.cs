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
        EEPROMFunctions EEPROMFunctions;
        GCode GCode;
        HeightFunctions HeightFunctions;
        Calibration Calibration;

        public ConsoleRead(Connection _Connection, UserInterface _UserInterface, EEPROMFunctions _EEPROMFunctions, GCode _GCode, HeightFunctions _HeightFunctions, Calibration _Calibration)
        {
            this.Connection = _Connection;
            this.UserInterface = _UserInterface;
            this.EEPROMFunctions = _EEPROMFunctions;
            this.GCode = _GCode;
            this.HeightFunctions = _HeightFunctions;
            this.Calibration = _Calibration;
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

                    if (EEPROMFunctions.EEPROMSet == false)
                    {
                        int intParse;
                        float floatParse2;

                        EEPROMFunctions.parseEEPROM(message, out intParse, out floatParse2);
                        EEPROMFunctions.setEEPROM(intParse, floatParse2);
                    }
                    else
                    {
                        HeightFunctions.setHeights(HeightFunctions.parseZProbe(message));

                        if (GCode.checkHeights == true)
                        {
                            GCode.positionFlow();
                        }
                        else
                        {
                            if (Calibration.calibrationState == true)
                            {
                                Calibration.calibrate(Calibration.calibrationSelection);
                            }
                        }
                    }

                }
                catch (TimeoutException) { }
            }//end while
        }//end void

    }
}
