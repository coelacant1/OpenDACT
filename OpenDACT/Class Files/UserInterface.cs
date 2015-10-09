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
        public float HRadRatio = -0.5F;
        public float DRadRatio = -0.5F;
        public float accuracy = 0.025F;
        public float calculationAccuracy = 0.001F;
        public float probingHeight = 10;
        public float offsetXCorrection = 1.5F;
        public float offsetYCorrection = 1.5F;
        public float offsetZCorrection = 1.5F;
        public float xxOppPerc = 0.5F;
        public float xyPerc = 0.25F;
        public float xyOppPerc = 0.25F;
        public float xzPerc = 0.25F;
        public float xzOppPerc = 0.25F;
        public float yyOppPerc = 0.5F;
        public float yxPerc = 0.25F;
        public float yxOppPerc = 0.25F;
        public float yzPerc = 0.25F;
        public float yzOppPerc = 0.25F;
        public float zzOppPerc = 0.5F;
        public float zxPerc = 0.25F;
        public float zxOppPerc = 0.25F;
        public float zyPerc = 0.25F;
        public float zyOppPerc = 0.25F;
        public float alphaRotationPercentageX = 1.725F;
        public float alphaRotationPercentageY = 1.725F;
        public float alphaRotationPercentageZ = 1.725F;
        public float deltaTower = 0.3F;
        public float deltaOpp = 0.2F;
        public float plateDiameter = 200F;

        public int pauseTimeSet = 500;

        public UserVariables() { }

        public void setHRadRatio(float value)
        {
            HRadRatio = value;
        }

        public void setDRadRatio(float value)
        {
            DRadRatio = value;
        }
        public void setAccuracy(float value)
        {
            accuracy = value;
        }
        public void setAlphaRotationPercentageX(float value)
        {
            value = Validation.checkZero(value);
            alphaRotationPercentageX = value;
        }
        public void setAlphaRotationPercentageY(float value)
        {
            value = Validation.checkZero(value);
            alphaRotationPercentageY = value;
        }
        public void setAlphaRotationPercentageZ(float value)
        {
            value = Validation.checkZero(value);
            alphaRotationPercentageZ = value;
        }
        public void setPauseTimeSet(int value)
        {
            pauseTimeSet = value;
        }
        public void setPlateDiameter(int value)
        {
            plateDiameter = value;
        }

        public void setOffsetXCorrection(float ioffsetXCorrection, float ixxOppPerc, float ixyPerc, float ixyOppPerc, float ixzPerc, float ixzOppPerc)
        {
            offsetXCorrection = ioffsetXCorrection;
            xxOppPerc = ixxOppPerc;
            xyPerc = ixyPerc;
            xyOppPerc = ixyOppPerc;
            xzPerc = ixzPerc;
            xzOppPerc = ixzOppPerc;

        }
        public void setOffsetYCorrection(float ioffsetYCorrection, float iyyOppPerc, float iyxPerc, float iyxOppPerc, float iyzPerc, float iyzOppPerc)
        {
            offsetYCorrection = ioffsetYCorrection;
            yyOppPerc = iyyOppPerc;
            yxPerc = iyxPerc;
            yxOppPerc = iyxOppPerc;
            yzPerc = iyzPerc;
            yzOppPerc = iyzOppPerc;
        }
        public void setOffsetZCorrection(float ioffsetZCorrection, float izzOppPerc, float izxPerc, float izxOppPerc, float izyPerc, float izyOppPerc)
        {
            offsetZCorrection = ioffsetZCorrection;
            zzOppPerc = izzOppPerc;
            zxPerc = izxPerc;
            zxOppPerc = izxOppPerc;
            zyPerc = izyPerc;
            zyOppPerc = izyOppPerc;
        }


        public float returnCalculationAccuracy()
        {
            return calculationAccuracy;
        }
    }


    static class UserInterface
    {
        /*
        mainForm mainForm;

        public UserInterface(mainForm _mainForm)
        {
            this.mainForm = _mainForm;
        }
        */

        static public UserVariables returnUserVariablesObject()
        {
            UserVariables userVariables = new UserVariables();
            return userVariables;
        }
        
        static public void logConsole(string value)
        {
            if (mainForm.consoleMain.InvokeRequired)
            {
                mainForm.consoleMain.Invoke(new Action(() =>
                {
                    mainForm.consoleMain.AppendText(value + "\n");
                }));
                return;
            }
            else
            {
                mainForm.consoleMain.AppendText(value + "\n");
            }
        }


        static public void logPrinter(string value)
        {
            if (mainForm.consolePrinter.InvokeRequired)
            {
                mainForm.consolePrinter.Invoke(new Action(() =>
                {
                    mainForm.consolePrinter.AppendText(value + "\n");
                }));
                return;
            }
            else
            {
                mainForm.consolePrinter.AppendText(value + "\n");
            }
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



        static public void GraphAccuracy()
        {
            //create graph of accuracy over iterations


        }
    }
}
