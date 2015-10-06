using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class UserVariables
    {
        //misc vars, alpha offsets, tower offsets, spm offsets, hrad offsets, drad offsets
        public static float HRadRatio;
        public static float DRadRatio;
        public static float accuracy = 0.025F;
        public static float probingHeight = 75;
        public static float offsetXCorrection = 1.5F;
        public static float offsetYCorrection = 1.5F;
        public static float offsetZCorrection = 1.5F;
        public static float xxOppPerc = 0.5F;
        public static float xyPerc = 0.25F;
        public static float xyOppPerc = 0.25F;
        public static float xzPerc = 0.25F;
        public static float xzOppPerc = 0.25F;
        public static float yyOppPerc = 0.5F;
        public static float yxPerc = 0.25F;
        public static float yxOppPerc = 0.25F;
        public static float yzPerc = 0.25F;
        public static float yzOppPerc = 0.25F;
        public static float zzOppPerc = 0.5F;
        public static float zxPerc = 0.25F;
        public static float zxOppPerc = 0.25F;
        public static float zyPerc = 0.25F;
        public static float zyOppPerc = 0.25F;

        public static int pauseTimeSet = 500;

        public static void setHRadRatio(float value)
        {
            value = Validation.checkZero(value);
            HRadRatio = value;
        }
        public static void setDRadRatio(float value)
        {
            value = Validation.checkZero(value);
            DRadRatio = value;
        }
        public static void setAccuracy(float value)
        {
            value = Validation.checkZero(value);
            accuracy = value;
        }
        public static void setPauseTimeSet(int value)
        {
            pauseTimeSet = value;
        }


        public static float returnHRadRatio()
        {
            return HRadRatio;
        }
        public static float returnDRadRatio()
        {
            return DRadRatio;
        }
        public static float returnUserAccuracy()
        {
            return accuracy;
        }
        public static int returnPauseTimeSet()
        {
            return pauseTimeSet;
        }
    }


    class UserInterface
    {
        mainForm mainForm;

        public UserInterface(mainForm _mainForm)
        {
            this.mainForm = _mainForm;
        }
        


        public void logConsole(string value)
        {
            mainForm.consoleMain.AppendText(value + "\n");
        }


        public void logPrinter(string value)
        {
            mainForm.consolePrinter.AppendText(value + "\n");
        }
        
        /*
            BUTTONS:
            connect
            disconnect
            calibrate - readeeprom, checkheights, calibrate, checkheights, calibrate etc - while loop
            checkHeights - set gcode bool to true
            
            UI:
            visible: console log, 
            not: printer log, tabs: settings, advanced, calibration graph
        */
    }
}
