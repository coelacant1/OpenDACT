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
    public class UserVariables
    {
        //misc vars, alpha offsets, tower offsets, spm offsets, hrad offsets, drad offsets
        public float HRadRatio = -0.5F;
        public float DRadRatio = -0.5F;
        public float accuracy = 0.025F;
        public float calculationAccuracy = 0.001F;
        public float probingHeight = 10;

        public float offsetXCorrection = 1.55F;
        public float xxOppPerc = -0.35F;
        public float xyPerc = -0.23F;
        public float xyOppPerc = 0.16F;
        public float xzPerc = -0.23F;
        public float xzOppPerc = 0.16F;

        public float offsetYCorrection = 1.55F;
        public float yyOppPerc = -0.35F;
        public float yxPerc = -0.23F;
        public float yxOppPerc = 0.16F;
        public float yzPerc = -0.23F;
        public float yzOppPerc = 0.16F;

        public float offsetZCorrection = 1.55F;
        public float zzOppPerc = -0.35F;
        public float zxPerc = -0.23F;
        public float zxOppPerc = 0.16F;
        public float zyPerc = -0.23F;
        public float zyOppPerc = 0.16F;

        public float alphaRotationPercentageX = 1.725F;
        public float alphaRotationPercentageY = 1.725F;
        public float alphaRotationPercentageZ = 1.725F;
        public float deltaTower = 0.293F;
        public float deltaOpp = 0.214F;
        public float plateDiameter = 230F;
        public float diagonalRodLength = 269;
        public float FSROffset = 0.6F;
        public float probingSpeed = 5F;

        public string probeChoice = "FSR";

        public bool advancedCalibration = false;


        public int pauseTimeSet = 500;
        public int advancedCalCount = 0;
        public int stepsCalcNumber = 0;
        public int maxIterations = 50;
        public int l = 0;
        
        public List<float> known_yDR = new List<float>();
        public List<float> known_xDR = new List<float>();

        public UserVariables() { }
    }
    

    static class UserInterface
    {
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
