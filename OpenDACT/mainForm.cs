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
using System.Globalization;

namespace OpenDACT.Class_Files
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

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
                UserInterface.logConsole("No ports available");
            }

            //accuracyTime.Series["Accuracy"].Points.AddXY(0, 0);
            UserVariables.isInitiated = true;
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
                GCode.checkHeights = true;
                EEPROMFunctions.readEEPROM();
                EEPROMFunctions.EEPROMReadOnly = false;
                Calibration.calibrationState = true;
                Calibration.calibrationSelection = 0;
                HeightFunctions.checkHeightsOnly = false;
            }
            else
            {
                UserInterface.logConsole("Not connected");
            }
        }
        
        private void iterativeCalibrate_Click(object sender, EventArgs e)
        {
            /*
            if (Connection._serialPort.IsOpen)
            {
                GCode.checkHeights = true;
                EEPROMFunctions.readEEPROM();
                EEPROMFunctions.EEPROMReadOnly = false;
                Calibration.calibrationState = true;
                Calibration.calibrationSelection = 1;
                HeightFunctions.checkHeightsOnly = false;
            }
            else
            {
                UserInterface.logConsole("Not connected");
            }
            */
        }

        private void resetPrinter_Click(object sender, EventArgs e)
        {
            if (Connection._serialPort.IsOpen)
            {
                GCode.emergencyReset();
            }
            else
            {
                UserInterface.logConsole("Not connected");
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
            System.Windows.Forms.MessageBox.Show("Version: 3.0.0\n\nCreated by Coela Can't\n\nWith help from Gene Buckle and Michael Hackney\n");
        }
        private void contactButton_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:coelacannot@gmail.com";
            proc.Start();
        }

        private void donateButton_Click_1(object sender, EventArgs e)
        {
            string url = "";

            string business = coelacannot@gmail.com"";
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

        public void setHeightsInvoke()
        {
            float X = Heights.X;
            float XOpp = Heights.XOpp;
            float Y = Heights.Y;
            float YOpp = Heights.YOpp;
            float Z = Heights.Z;
            float ZOpp = Heights.ZOpp;

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

        public void setEEPROMGUIList()
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

        private void sendEEPROMButton_Click(object sender, EventArgs e)
        {
            EEPROM.stepsPerMM = Convert.ToInt32(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.stepsPerMMText.Text, out value); return value; }));
            EEPROM.zMaxLength = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.zMaxLengthText.Text, out value); return value; }));
            EEPROM.zProbeHeight = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.zProbeText.Text, out value); return value; }));
            EEPROM.zProbeSpeed = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.zProbeSpeedText.Text, out value); return value; }));
            EEPROM.diagonalRod = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.diagonalRod.Text, out value); return value; }));
            EEPROM.HRadius = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.HRadiusText.Text, out value); return value; }));
            EEPROM.offsetX = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.offsetXText.Text, out value); return value; }));
            EEPROM.offsetY = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.offsetYText.Text, out value); return value; }));
            EEPROM.offsetZ = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.offsetZText.Text, out value); return value; }));
            EEPROM.A = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.AText.Text, out value); return value; }));
            EEPROM.B = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.BText.Text, out value); return value; }));
            EEPROM.C = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.CText.Text, out value); return value; }));
            EEPROM.DA = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.DAText.Text, out value); return value; }));
            EEPROM.DB = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.DBText.Text, out value); return value; }));
            EEPROM.DC = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(this.DCText.Text, out value); return value; }));

            EEPROMFunctions.sendEEPROM();
        }

        private void readEEPROM_Click(object sender, EventArgs e)
        {
            if (Connection._serialPort.IsOpen)
            {
                EEPROMFunctions.tempEEPROMSet = false;
                EEPROMFunctions.readEEPROM();
                EEPROMFunctions.EEPROMReadOnly = true;
                HeightFunctions.checkHeightsOnly = false;
                EEPROMFunctions.EEPROMReadCount = 0;
            }
            else
            {
                UserInterface.logConsole("Not Connected");
            }
        }

        public void setButtonValues()
        {
            Invoke((MethodInvoker)delegate
            {
                this.textAccuracy.Text = UserVariables.calculationAccuracy.ToString();
                UserVariables.accuracy = Convert.ToSingle(this.textAccuracy2.Text);
                UserVariables.HRadRatio = Convert.ToSingle(this.textHRadRatio.Text);
                UserVariables.DRadRatio = Convert.ToSingle(this.textDRadRatio.Text);

                UserVariables.probeChoice = getZMin();
                UserVariables.advancedCalibration = Convert.ToBoolean(getHeuristic());

                UserVariables.pauseTimeSet = Convert.ToInt32(this.textPauseTimeSet.Text);
                UserVariables.maxIterations = Convert.ToInt32(this.textMaxIterations.Text);
                UserVariables.probingSpeed = Convert.ToSingle(this.textProbingSpeed.Text);
                UserVariables.FSROffset = Convert.ToSingle(this.textFSRPO.Text);
                UserVariables.deltaOpp = Convert.ToSingle(this.textDeltaOpp.Text);
                UserVariables.deltaTower = Convert.ToSingle(this.textDeltaTower.Text);
                UserVariables.diagonalRodLength = Convert.ToSingle(this.diagonalRodLengthText.Text);
                UserVariables.alphaRotationPercentage = Convert.ToSingle(this.alphaText.Text);
                UserVariables.plateDiameter = Convert.ToSingle(this.textPlateDiameter.Text);
                UserVariables.probingHeight = Convert.ToSingle(this.textProbingHeight.Text);

                //XYZ Offset percs
                UserVariables.offsetCorrection = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textOffsetPerc.Text, out value); return value; }));
                UserVariables.mainOppPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textMainOppPerc.Text, out value); return value; }));
                UserVariables.towPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textTowPerc.Text, out value); return value; }));
                UserVariables.oppPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textOppPerc.Text, out value); return value; }));
            });
        }
        private string getZMin()
        {
            if (comboBoxZMin.InvokeRequired)
            {
            return (string)comboBoxZMin.Invoke(new Func<string>(getZMin));
            }
            else
            {
                return comboBoxZMin.Text;
            }
        }

        private string getHeuristic()
        {
            if (heuristicComboBox.InvokeRequired)
            {
                return (string)heuristicComboBox.Invoke(new Func<string>(getHeuristic));
            }
            else
            {
                return heuristicComboBox.Text;
            }
        }
        
        public void setUserVariables()
        {
            UserVariables.calculationAccuracy = Convert.ToSingle(this.textAccuracy.Text);
            UserVariables.accuracy = Convert.ToSingle(this.textAccuracy2.Text);
            UserVariables.HRadRatio = Convert.ToSingle(this.textHRadRatio.Text);
            UserVariables.DRadRatio = Convert.ToSingle(this.textDRadRatio.Text);

            UserVariables.probeChoice = getZMin();
            UserVariables.advancedCalibration = Convert.ToBoolean(getHeuristic());

            UserVariables.pauseTimeSet = Convert.ToInt32(this.textPauseTimeSet.Text);
            UserVariables.maxIterations = Convert.ToInt32(this.textMaxIterations.Text);
            UserVariables.probingSpeed = Convert.ToSingle(this.textProbingSpeed.Text);
            UserVariables.FSROffset = Convert.ToSingle(this.textFSRPO.Text);
            UserVariables.deltaOpp = Convert.ToSingle(this.textDeltaOpp.Text);
            UserVariables.deltaTower = Convert.ToSingle(this.textDeltaTower.Text);
            UserVariables.diagonalRodLength = Convert.ToSingle(this.diagonalRodLengthText.Text);
            UserVariables.alphaRotationPercentage = Convert.ToSingle(this.alphaText.Text);
            UserVariables.plateDiameter = Convert.ToSingle(this.textPlateDiameter.Text);
            UserVariables.probingHeight = Convert.ToSingle(this.textProbingHeight.Text);

            //XYZ Offset percs
            UserVariables.offsetCorrection = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textOffsetPerc.Text, out value); return value; }));
            UserVariables.mainOppPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textMainOppPerc.Text, out value); return value; }));
            UserVariables.towPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textTowPerc.Text, out value); return value; }));
            UserVariables.oppPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textOppPerc.Text, out value); return value; }));

            UserVariables.xySpeed = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(xySpeedTxt.Text, out value); return value; }));
        }

        private void checkHeights_Click(object sender, EventArgs e)
        {
            if (EEPROMFunctions.tempEEPROMSet == false)
            {
                EEPROMFunctions.readEEPROM();
            }

            GCode.checkHeights = true;
            EEPROMFunctions.EEPROMReadOnly = false;
            Calibration.calibrationState = true;
            Calibration.calibrationSelection = 0;
            HeightFunctions.checkHeightsOnly = true;
            HeightFunctions.heightsSet = false;
        }
    }
}