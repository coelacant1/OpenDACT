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

            heuristicModeCombo.Items.Add("True");
            heuristicModeCombo.Items.Add("False");
            heuristicModeCombo.Text = "False";

            comboBoxZMinimumValue.Items.Add("FSR");
            comboBoxZMinimumValue.Items.Add("Z-Probe");
            comboBoxZMinimumValue.Text = "FSR";

            iXtext.Text = "0.00";
            iXOpptext.Text = "0.00";
            iYtext.Text = "0.00";
            iYOpptext.Text = "0.00";
            iZtext.Text = "0.00";
            iZOpptext.Text = "0.00";

            XText.Text = "0.00";
            XOppText.Text = "0.00";
            YText.Text = "0.00";
            YOppText.Text = "0.00";
            ZText.Text = "0.00";
            ZOppText.Text = "0.00";
            
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

            /*
            String[] zMinArray = { "FSR", "Z-Probe" };
            comboZMin.DataSource = zMinArray;

            // clear the result labels.
            lblXAngleTower.Text = "";
            lblXPlate.Text = "";
            lblXAngleTop.Text = "";
            lblXPlateTop.Text = "";
            lblYAngleTower.Text = "";
            lblYPlate.Text = "";
            lblYAngleTop.Text = "";
            lblYPlateTop.Text = "";
            lblZAngleTower.Text = "";
            lblZPlate.Text = "";
            lblZAngleTop.Text = "";
            lblZPlateTop.Text = "";
            lblScaleOffset.Text = "";
            */

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
                Calibration.calibrationState = true;
                Calibration.calibrationSelection = 0;
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
            if (advancedPanel.Visible == false) {
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
            Invoke((MethodInvoker)delegate {
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

        public void setHeightsInvoke(ref Heights heights)
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
            Invoke((MethodInvoker)delegate { this.stepsPerMMText.Text = eeprom.stepsPerMM.ToString(); });
            Invoke((MethodInvoker)delegate { this.zMaxLengthText.Text = eeprom.zMaxLength.ToString(); });
            Invoke((MethodInvoker)delegate { this.zProbeText.Text = eeprom.zProbe.ToString(); });
            Invoke((MethodInvoker)delegate { this.zProbeSpeedText.Text = textProbingSpeed.Text; });
            Invoke((MethodInvoker)delegate { this.HRadiusText.Text = eeprom.HRadius.ToString(); });
            Invoke((MethodInvoker)delegate { this.offsetXText.Text = eeprom.offsetX.ToString(); });
            Invoke((MethodInvoker)delegate { this.offsetYText.Text = eeprom.offsetY.ToString(); });
            Invoke((MethodInvoker)delegate { this.offsetZText.Text = eeprom.offsetZ.ToString(); });
            Invoke((MethodInvoker)delegate { this.AText.Text = eeprom.A.ToString(); });
            Invoke((MethodInvoker)delegate { this.BText.Text = eeprom.B.ToString(); });
            Invoke((MethodInvoker)delegate { this.CText.Text = eeprom.C.ToString(); });
            Invoke((MethodInvoker)delegate { this.DAText.Text = eeprom.DA.ToString(); });
            Invoke((MethodInvoker)delegate { this.DBText.Text = eeprom.DB.ToString(); });
            Invoke((MethodInvoker)delegate { this.DCText.Text = eeprom.DC.ToString(); });

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
    }
}
