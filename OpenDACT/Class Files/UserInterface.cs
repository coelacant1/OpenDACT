using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class UserInterface
    {
        mainForm mainForm;

        public UserInterface(mainForm _mainForm)
        {
            this.mainForm = _mainForm;
        }



        class UserVariables
        {
            //misc vars, alpha offsets, tower offsets, spm offsets, hrad offsets, drad offsets
            public float stepsPerMM;
            public float tempSPM;
            public float zMaxLength;
        }

        public static object createUserVarObject()
        {
            UserVariables UserVariables = new UserVariables();

            //EEPROMVariables.stepsPerMM++;

            return UserVariables;
        }

        public void logConsole(string value)
        {
            mainForm.consoleMain.AppendText(value + "\n");
        }


        public void logPrinter(string value)
        {
            mainForm.consolePrinter.AppendText(value + "\n");
        }

        public object fetchUserVariables()
        {
            object userVariables = createUserVarObject();

            //set variables according to user input

            return userVariables;
        }
        
        /*
            BUTTONS:
            connect
            disconnect
            calibrate - readeeprom, checkheights, calibrate, checkheights, calibrate etc - while loop
            checkHeights
            
            UI:
            visible: console log, 
            not: printer log, tabs: settings, advanced, calibration graph
        */
    }
}
