using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;

namespace OpenDACT.Class_Files
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            UserVariables userVariables = UserInterface.returnUserVariablesObject();

            InitializeComponent();
            consoleMain.Text = "";
            consoleMain.ScrollBars = RichTextBoxScrollBars.Vertical;
            consolePrinter.Text = "";
            consolePrinter.ScrollBars = RichTextBoxScrollBars.Vertical;


            // Basic set of standard baud rates.
            baudRateCombo.Items.Add("250000");
            baudRateCombo.Items.Add("115200");
            baudRateCombo.Items.Add("57600");
            baudRateCombo.Items.Add("38400");
            baudRateCombo.Items.Add("19200");
            baudRateCombo.Items.Add("9600");
            baudRateCombo.Text = "250000";  // This is the default for most RAMBo controllers.

            advancedPanel.Visible = false;
            printerLogPanel.Visible = false;

            Connection.readThread = new Thread(ConsoleRead.Read);
            Connection._serialPort = new SerialPort();


            // Build the combobox of available ports.
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length >= 1)
            {
                Dictionary<string, string> comboSource = new Dictionary<string, string>();

                int count = 0;

                foreach (string element in ports)
                {
                    comboSource.Add(ports[count], ports[count]);
                    count++;
                }

                portsCombo.DataSource = new BindingSource(comboSource, null);
                portsCombo.DisplayMember = "Key";
                portsCombo.ValueMember = "Value";
            }
            else
            {
                UserInterface.logConsole("No ports available\n");
            }

            accuracyTime.Series["Accuracy"].Points.AddXY(0, 1);
            UserInterface.isInitiated = true;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Connection.connect();
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            Connection.disconnect();
        }

        private void calibrateButton_Click(object sender, EventArgs e)
        {
            if (Connection._serialPort.IsOpen)
            {
                EEPROMFunctions.readEEPROM();
                EEPROMFunctions.EEPROMReadOnly = false;
                Calibration.calibrationState = true;
                Calibration.calibrationSelection = 0;
                HeightFunctions.checkHeightsOnly = false;
            }
            else
            {
                UserInterface.logConsole("Not connected\n");
            }
        }

        private void resetPrinter_Click(object sender, EventArgs e)
        {
            if (Connection._serialPort.IsOpen)
            {
                GCode.emergencyReset();
            }
            else
            {
                UserInterface.logConsole("Not connected\n");
            }
        }
        public void appendMainConsole(string value)
        {
            Invoke((MethodInvoker)delegate { consoleMain.AppendText(value + "\n"); });
            Invoke((MethodInvoker)delegate { consoleMain.ScrollToCaret(); });
        }
        public void appendPrinterConsole(string value)
        {
            Invoke((MethodInvoker)delegate { consolePrinter.AppendText(value + "\n"); });
            Invoke((MethodInvoker)delegate { consolePrinter.ScrollToCaret(); });
        }

        private void openAdvanced_Click(object sender, EventArgs e)
        {
            if (advancedPanel.Visible == false)
            {
                advancedPanel.Visible = true;
                printerLogPanel.Visible = true;
            }
            else
            {
                advancedPanel.Visible = false;
                printerLogPanel.Visible = false;
            }
        }

        private void sendGCode_Click(object sender, EventArgs e)
        {
            if (Connection._serialPort.IsOpen)
            {
                Connection._serialPort.WriteLine(GCodeBox.Text.ToString().ToUpper());
                UserInterface.logConsole("Sent: " + GCodeBox.Text.ToString().ToUpper());
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }

        public void setAccuracyPoint(float x, float y)
        {
            Invoke((MethodInvoker)delegate
            {
                accuracyTime.Refresh();
                accuracyTime.Series["Accuracy"].Points.AddXY(x, y);
            });
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Version: 3.0.0\n\nCreated by Steven T. Rowland\n\nWith help from Gene Buckle and Michael Hackney\n");
        }
        private void contactButton_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:steventrowland@gmail.com";
            proc.Start();
        }

        private void donateButton_Click_1(object sender, EventArgs e)
        {
            string url = "";

            string business = "steventrowland@gmail.com";
            string description = "Donation";
            string country = "US";
            string currency = "USD";

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            System.Diagnostics.Process.Start(url);
        }

        public void setHeightsInvoke(Heights heights)
        {
            float X = heights.X;
            float XOpp = heights.XOpp;
            float Y = heights.Y;
            float YOpp = heights.YOpp;
            float Z = heights.Z;
            float ZOpp = heights.ZOpp;

            //set base heights for advanced calibration comparison
            if (Calibration.iterationNum == 0)
            {
                Invoke((MethodInvoker)delegate { this.iXtext.Text = Math.Round(X, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iXOpptext.Text = Math.Round(XOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iYtext.Text = Math.Round(Y, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iYOpptext.Text = Math.Round(YOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iZtext.Text = Math.Round(Z, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.iZOpptext.Text = Math.Round(ZOpp, 3).ToString(); });
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

        public void setEEPROMGUIList(EEPROM eeprom)
        {
            Invoke((MethodInvoker)delegate
            {
                this.stepsPerMMText.Text = eeprom.stepsPerMM.ToString();
                this.zMaxLengthText.Text = eeprom.zMaxLength.ToString();
                this.zProbeText.Text = eeprom.zProbe.ToString();
                this.zProbeSpeedText.Text = textProbingSpeed.Text;
                this.HRadiusText.Text = eeprom.HRadius.ToString();
                this.offsetXText.Text = eeprom.offsetX.ToString();
                this.offsetYText.Text = eeprom.offsetY.ToString();
                this.offsetZText.Text = eeprom.offsetZ.ToString();
                this.AText.Text = eeprom.A.ToString();
                this.BText.Text = eeprom.B.ToString();
                this.CText.Text = eeprom.C.ToString();
                this.DAText.Text = eeprom.DA.ToString();
                this.DBText.Text = eeprom.DB.ToString();
                this.DCText.Text = eeprom.DC.ToString();
            });
        }

        private void sendEEPROMButton_Click(object sender, EventArgs e)
        {
            UserInterface.logConsole("Setting EEPROM.");
            Thread.Sleep(1000);
            GCode.sendEEPROMVariable(3, 11, Convert.ToSingle(stepsPerMMText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 153, Convert.ToSingle(zMaxLengthText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 808, Convert.ToSingle(zProbeText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 812, Convert.ToSingle(zProbeSpeedText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 885, Convert.ToSingle(HRadiusText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(1, 893, Convert.ToSingle(offsetXText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(1, 895, Convert.ToSingle(offsetYText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(1, 897, Convert.ToSingle(offsetZText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 901, Convert.ToSingle(AText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 905, Convert.ToSingle(BText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 909, Convert.ToSingle(CText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 913, Convert.ToSingle(DAText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 917, Convert.ToSingle(DBText.Text));
            Thread.Sleep(750);
            GCode.sendEEPROMVariable(3, 921, Convert.ToSingle(DCText.Text));
            Thread.Sleep(750);
        }

        private void readEEPROM_Click(object sender, EventArgs e)
        {
            if (Connection._serialPort.IsOpen)
            {
                EEPROMFunctions.readEEPROM();
                EEPROMFunctions.EEPROMReadOnly = true;
                HeightFunctions.checkHeightsOnly = false;
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }

        public void setUserVariables(ref UserVariables userVariables)
        {
            userVariables.calculationAccuracy = Convert.ToSingle(textAccuracy.Text);
            userVariables.accuracy = Convert.ToSingle(textAccuracy2.Text);
            userVariables.HRadRatio = Convert.ToSingle(textHRadRatio.Text);

            userVariables.probeChoice = comboBoxZMinimumValue.Text;
            userVariables.advancedCalibration = Convert.ToBoolean(heuristicComboBox.Text);

            userVariables.pauseTimeSet = Convert.ToInt32(textPauseTimeSet.Text);
            userVariables.maxIterations = Convert.ToInt32(textMaxIterations.Text);
            userVariables.probingSpeed = Convert.ToSingle(textProbingSpeed.Text);
            userVariables.FSROffset = Convert.ToSingle(textFSRPO.Text);
            userVariables.deltaOpp = Convert.ToSingle(textDeltaOpp.Text);
            userVariables.deltaTower = Convert.ToSingle(textDeltaTower.Text);
            userVariables.diagonalRodLength = Convert.ToSingle(diagonalRodLengthText.Text);
            userVariables.alphaRotationPercentageX = Convert.ToSingle(AText.Text);
            userVariables.alphaRotationPercentageY = Convert.ToSingle(BText.Text);
            userVariables.alphaRotationPercentageZ = Convert.ToSingle(CText.Text);
            userVariables.plateDiameter = Convert.ToSingle(textPlateDiameter.Text);

            //XYZ Offset percs
            userVariables.offsetXCorrection = Convert.ToSingle(offsetXText.Text);
            userVariables.xxOppPerc = Convert.ToSingle(textxxOppPerc.Text);
            userVariables.xyPerc = Convert.ToSingle(textxyPerc.Text);
            userVariables.xyOppPerc = Convert.ToSingle(textxyOppPerc.Text);
            userVariables.xzPerc = Convert.ToSingle(textxzPerc.Text);
            userVariables.xzOppPerc = Convert.ToSingle(textxzOppPerc.Text);

            userVariables.offsetYCorrection = Convert.ToSingle(offsetYText.Text);
            userVariables.yyOppPerc = Convert.ToSingle(textyyOppPerc.Text);
            userVariables.yxPerc = Convert.ToSingle(textyxPerc.Text);
            userVariables.yxOppPerc = Convert.ToSingle(textyxOppPerc);
            userVariables.yzPerc = Convert.ToSingle(textyzPerc.Text);
            userVariables.yzOppPerc = Convert.ToSingle(textyzOppPerc.Text);

            userVariables.offsetZCorrection = Convert.ToSingle(offsetZText.Text);
            userVariables.zzOppPerc = Convert.ToSingle(textzzOppPerc.Text);
            userVariables.zxPerc = Convert.ToSingle(textzxPerc.Text);
            userVariables.zxOppPerc = Convert.ToSingle(textzxOppPerc.Text);
            userVariables.zyPerc = Convert.ToSingle(textzyPerc.Text);
            userVariables.zyOppPerc = Convert.ToSingle(textzyOppPerc.Text);
        }

        private void checkHeights_Click(object sender, EventArgs e)
        {
            EEPROMFunctions.readEEPROM();
            EEPROMFunctions.EEPROMReadOnly = false;
            Calibration.calibrationState = true;
            Calibration.calibrationSelection = 0;
            HeightFunctions.checkHeightsOnly = true;
        }
    }
}