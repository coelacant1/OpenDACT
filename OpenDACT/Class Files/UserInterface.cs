using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

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
        public bool advancedCalibration = false;
        public int advancedCalCount = 0;

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
        public static bool isInitiated = false;

        static public UserVariables returnUserVariablesObject()
        {
            UserVariables userVariables = new UserVariables();
            return userVariables;
        }


        public static void logConsole(string value)
        {
            if (isInitiated == true)
            {
                Program.mainFormTest.appendMainConsole(value);
            }
        }


        public static void logPrinter(string value)
        {
            if (isInitiated == true)
            {
                Program.mainFormTest.appendPrinterConsole(value);
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

        //
        public static void setAdvancedCalVars()
        {

            /*
            Invoke((MethodInvoker)delegate { mainForm.textDeltaTower.Text = Math.Round(deltaTower, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textDeltaOpp.Text = Math.Round(deltaOpp, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textHRadRatio.Text = Math.Round(HRadRatio, 3).ToString(); });

            Invoke((MethodInvoker)delegate { this.textxxPerc.Text = Math.Round(offsetXCorrection, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textxxOppPerc.Text = Math.Round(xxOppPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textxyPerc.Text = Math.Round(xyPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textxyOppPerc.Text = Math.Round(xyOppPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textxzPerc.Text = Math.Round(xzPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textxzOppPerc.Text = Math.Round(xzOppPerc, 3).ToString(); });

            Invoke((MethodInvoker)delegate { this.textyyPerc.Text = Math.Round(offsetYCorrection, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textyyOppPerc.Text = Math.Round(yyOppPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textyxPerc.Text = Math.Round(yxPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textyxOppPerc.Text = Math.Round(yxOppPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textyzPerc.Text = Math.Round(yzPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textyzOppPerc.Text = Math.Round(yzOppPerc, 3).ToString(); });

            Invoke((MethodInvoker)delegate { this.textzzPerc.Text = Math.Round(offsetZCorrection, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textzzOppPerc.Text = Math.Round(zzOppPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textzxPerc.Text = Math.Round(zxPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textzxOppPerc.Text = Math.Round(zxOppPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textzyPerc.Text = Math.Round(zyPerc, 3).ToString(); });
            Invoke((MethodInvoker)delegate { this.textzyOppPerc.Text = Math.Round(zyOppPerc, 3).ToString(); });
            */
        }

        public static void GraphAccuracy()
        {
            //create graph of accuracy over iterations


        }
    }
}
