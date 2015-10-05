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
            object UserVariables = new UserVariables();
            UserVariables userVars = (UserVariables)UserVariables;

            //EEPROMVariables.stepsPerMM++;

            return userVars;
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
    }
}
