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
    static class UserInterface
    {
        
        public static void LogConsole(string value)
        {
            Program.MainForm.Invoke((MethodInvoker)delegate { Program.MainForm.consoleMain.AppendText(value + "\n"); });
            if (Settings.ScrollToBottomSoftwareLog)
            {
                Program.MainForm.Invoke((MethodInvoker)delegate { Program.MainForm.consoleMain.ScrollToCaret(); });
            }
        }


        public static void LogPrinter(string value)
        {
            Program.MainForm.Invoke((MethodInvoker)delegate { Program.MainForm.consolePrinter.AppendText(value + "\n"); });
            if (Settings.ScrollToBottomPrinterLog)
            {
                Program.MainForm.Invoke((MethodInvoker)delegate { Program.MainForm.consolePrinter.ScrollToCaret(); });
            }
        }

        public static void SetAccuracyPoint(float x, float y)
        {
            Program.MainForm.Invoke((MethodInvoker)delegate
            {
                Program.MainForm.accuracyTime.Refresh();
                Program.MainForm.accuracyTime.Series["Accuracy"].Points.AddXY(x, y);
            });
        }
        
        public static void SetHeightsInvoke(Printer printer)
        {
            double X = printer.bedHeightMap.XTower;
            double XOpp = printer.bedHeightMap.XOpposite;
            double Y = printer.bedHeightMap.YTower;
            double YOpp = printer.bedHeightMap.YOpposite;
            double Z = printer.bedHeightMap.ZTower;
            double ZOpp = printer.bedHeightMap.ZOpposite;

            //set base heights for advanced calibration comparison
            if (Calibration.iterationNum == 0)
            {
                Invoke((MethodInvoker)delegate { this.iXtext.Text = Math.Round(X, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iXOpptext.Text = Math.Round(XOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iYtext.Text = Math.Round(Y, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iYOpptext.Text = Math.Round(YOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iZtext.Text = Math.Round(Z, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iZOpptext.Text = Math.Round(ZOpp, 3).ToString(); });

                Calibration.iterationNum++;

                Invoke((MethodInvoker)delegate { this.XText.Text = Math.Round(X, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.XOppText.Text = Math.Round(XOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.YText.Text = Math.Round(Y, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.YOppText.Text = Math.Round(YOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.ZText.Text = Math.Round(Z, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.ZOppText.Text = Math.Round(ZOpp, 3).ToString(); });
            }
            else
            {
                Invoke((MethodInvoker)delegate { this.XText.Text = Math.Round(X, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.XOppText.Text = Math.Round(XOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.YText.Text = Math.Round(Y, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.YOppText.Text = Math.Round(YOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.ZText.Text = Math.Round(Z, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.ZOppText.Text = Math.Round(ZOpp, 3).ToString(); });
            }
        }

        public static void SetEEPROMGUIList()
        {
            Invoke((MethodInvoker)delegate
            {
                this.stepsPerMMText.Text = EEPROM.stepsPerMM.ToString();
                this.zMaxLengthText.Text = EEPROM.zMaxLength.ToString();
                this.zProbeText.Text = EEPROM.zProbeHeight.ToString();
                this.zProbeSpeedText.Text = EEPROM.zProbeSpeed.ToString();
                this.diagonalRod.Text = EEPROM.diagonalRod.ToString();
                this.HRadiusText.Text = EEPROM.HRadius.ToString();
                this.offsetXText.Text = EEPROM.offsetX.ToString();
                this.offsetYText.Text = EEPROM.offsetY.ToString();
                this.offsetZText.Text = EEPROM.offsetZ.ToString();
                this.AText.Text = EEPROM.A.ToString();
                this.BText.Text = EEPROM.B.ToString();
                this.CText.Text = EEPROM.C.ToString();
                this.DAText.Text = EEPROM.DA.ToString();
                this.DBText.Text = EEPROM.DB.ToString();
                this.DCText.Text = EEPROM.DC.ToString();
            });
        }
    }
}
