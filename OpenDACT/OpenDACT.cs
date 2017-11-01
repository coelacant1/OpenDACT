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
    public partial class MainForm : Form
    {
        SerialPort serialPort;
        Printer printer;
        Settings settings;
        GCodeCommands gCodeCommands;

        public MainForm()
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            InitializeComponent();

            consoleMain.Text = "";
            consoleMain.ScrollBars = RichTextBoxScrollBars.Vertical;
            consolePrinter.Text = "";
            consolePrinter.ScrollBars = RichTextBoxScrollBars.Vertical;
            
            baudRateCombo.Text = "250000";

            advancedPanel.Visible = false;
            printerLogPanel.Visible = false;
        }

        private void InitializeCOMPorts()
        {
            // Build the combobox of available ports.
            string[] ports = serialPort.GetPortNames();

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
                UserInterface.LogConsole("No ports available", settings);
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            serialPort = new SerialPort();
            serialPort.Open(portsCombo.Text, baudRateCombo.Text);
        }

        private void DisconnectButton_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen())
            {
                serialPort.Close();
                serialPort.Dispose();
            }
            else
            {
                UserInterface.LogConsole("Not connected", settings);
            }
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen())
            {
                InitializePrinter();
                InitializeSettings();
                InitializeGCodeCommandsSmoothieware();
                //process software settings
                //calibration routine

            }
            else
            {
                UserInterface.LogConsole("Not connected", settings);
            }
        }
        
        private void ResetPrinter_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen())
            {
                //emergency reset
            }
            else
            {
                UserInterface.LogConsole("Not connected", settings);
            }
        }

        private void OpenAdvanced_Click(object sender, EventArgs e)
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

        private void SendGCode_Click(object sender, EventArgs e)
        {
            SendGCodeText();
        }

        private void GCodeBox_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                SendGCodeText();
        }

        private void SendGCodeText() 
            {
            if (serialPort.IsOpen()) {
                serialPort.WriteLine(GCodeBox.Text.ToString().ToUpper());
                UserInterface.LogConsole("Sent: " + GCodeBox.Text.ToString().ToUpper(), settings);
            }
            else {
                UserInterface.LogConsole("Not Connected", settings);
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Version: 4.0.0\n\nCreated by Steven T. Rowland\n\nWith help from Gene Buckle and Michael Hackney\n");
        }

        private void ContactButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:steventrowland@gmail.com";
            proc.Start();
        }

        private void DonateButton_Click(object sender, EventArgs e)
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


        private void SendEEPROMButton_Click(object sender, EventArgs e)
        {/*
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
            */
        }

        private void ReadEEPROM_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen())
            {

            }
            else
            {
                UserInterface.LogConsole("Not Connected", settings);
            }
        }

        public void SetButtonValues()
        {
            /*
            Invoke((MethodInvoker)delegate
            {
                this.textAccuracy.Text = UserVariables.calculationAccuracy.ToString();
                this.textAccuracy2.Text = UserVariables.accuracy.ToString();
                this.textHRadRatio.Text = UserVariables.HRadRatio.ToString();
                this.textDRadRatio.Text = UserVariables.DRadRatio.ToString();

                this.heuristicComboBox.Text = UserVariables.advancedCalibration.ToString();

                this.textPauseTimeSet.Text = UserVariables.pauseTimeSet.ToString();
                this.textMaxIterations.Text = UserVariables.maxIterations.ToString();
                this.textProbingSpeed.Text = UserVariables.probingSpeed.ToString();
                this.textFSRPO.Text = UserVariables.FSROffset.ToString();
                this.textDeltaOpp.Text = UserVariables.deltaOpp.ToString();
                this.textDeltaTower.Text = UserVariables.deltaTower.ToString();
                this.diagonalRodLengthText.Text = UserVariables.diagonalRodLength.ToString();
                this.alphaText.Text = UserVariables.alphaRotationPercentage.ToString();
                this.bedDiameter.Text = UserVariables.plateDiameter.ToString();
                this.textProbingHeight.Text = UserVariables.probingHeight.ToString();

                //XYZ Offset percs
                this.textOffsetPerc.Text = UserVariables.offsetCorrection.ToString();
                this.textMainOppPerc.Text = UserVariables.mainOppPerc.ToString();
                this.textTowPerc.Text = UserVariables.towPerc.ToString();
                this.textOppPerc.Text = UserVariables.oppPerc.ToString();
            });
            */
        }
        private string GetZMin()
        {
            if (comboBoxZMin.InvokeRequired)
            {
                return (string)comboBoxZMin.Invoke(new Func<string>(GetZMin));
            }
            else
            {
                return comboBoxZMin.Text;
            }
        }

        private string GetHeuristic()
        {
            if (heuristicComboBox.InvokeRequired)
            {
                return (string)heuristicComboBox.Invoke(new Func<string>(GetHeuristic));
            }
            else
            {
                return heuristicComboBox.Text;
            }
        }

        public void SetUserVariables()
        {/*
            UserVariables.calculationAccuracy = Convert.ToSingle(this.textAccuracy.Text);
            UserVariables.accuracy = Convert.ToSingle(this.textAccuracy2.Text);
            UserVariables.HRadRatio = Convert.ToSingle(this.textHRadRatio.Text);
            UserVariables.DRadRatio = Convert.ToSingle(this.textDRadRatio.Text);

            UserVariables.probeChoice = GetZMin();
            UserVariables.advancedCalibration = Convert.ToBoolean(GetHeuristic());

            UserVariables.pauseTimeSet = Convert.ToInt32(this.textPauseTimeSet.Text);
            UserVariables.maxIterations = Convert.ToInt32(this.textMaxIterations.Text);
            UserVariables.probingSpeed = Convert.ToSingle(this.textProbingSpeed.Text);
            UserVariables.FSROffset = Convert.ToSingle(this.textFSRPO.Text);
            UserVariables.deltaOpp = Convert.ToSingle(this.textDeltaOpp.Text);
            UserVariables.deltaTower = Convert.ToSingle(this.textDeltaTower.Text);
            UserVariables.diagonalRodLength = Convert.ToSingle(this.diagonalRodLengthText.Text);
            UserVariables.alphaRotationPercentage = Convert.ToSingle(this.alphaText.Text);
            UserVariables.plateDiameter = Convert.ToSingle(this.bedDiameter.Text);
            UserVariables.probingHeight = Convert.ToSingle(this.textProbingHeight.Text);

            //XYZ Offset percs
            UserVariables.offsetCorrection = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textOffsetPerc.Text, out value); return value; }));
            UserVariables.mainOppPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textMainOppPerc.Text, out value); return value; }));
            UserVariables.towPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textTowPerc.Text, out value); return value; }));
            UserVariables.oppPerc = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(textOppPerc.Text, out value); return value; }));

            UserVariables.xySpeed = Convert.ToSingle(this.Invoke((Func<double>)delegate { double value; Double.TryParse(xySpeedTxt.Text, out value); return value; }));
            */
        }

        private void CheckHeights_Click(object sender, EventArgs e)
        {

        }

        private void StopBut_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }

        }

        private void ManualCalibrateBut_Click(object sender, EventArgs e)
        {/*
            try
            {
                Calibration.calibrationState = true;

                Program.MainFormTest.SetUserVariables();

                Heights.X = Convert.ToSingle(xManual.Text);
                Heights.XOpp = Convert.ToSingle(xOppManual.Text);
                Heights.Y = Convert.ToSingle(yManual.Text);
                Heights.YOpp = Convert.ToSingle(yOppManual.Text);
                Heights.Z = Convert.ToSingle(zManual.Text);
                Heights.ZOpp = Convert.ToSingle(zOppManual.Text);

                EEPROM.stepsPerMM = Convert.ToSingle(spmMan.Text);
                EEPROM.tempSPM = Convert.ToSingle(spmMan.Text);
                EEPROM.zMaxLength = Convert.ToSingle(zMaxMan.Text);
                EEPROM.zProbeHeight = Convert.ToSingle(zProHeiMan.Text);
                EEPROM.zProbeSpeed = Convert.ToSingle(zProSpeMan.Text);
                EEPROM.HRadius = Convert.ToSingle(horRadMan.Text);
                EEPROM.diagonalRod = Convert.ToSingle(diaRodMan.Text);
                EEPROM.offsetX = Convert.ToSingle(towOffXMan.Text);
                EEPROM.offsetY = Convert.ToSingle(towOffYMan.Text);
                EEPROM.offsetZ = Convert.ToSingle(towOffZMan.Text);
                EEPROM.A = Convert.ToSingle(alpRotAMan.Text);
                EEPROM.B = Convert.ToSingle(alpRotBMan.Text);
                EEPROM.C = Convert.ToSingle(alpRotCMan.Text);
                EEPROM.DA = Convert.ToSingle(delRadAMan.Text);
                EEPROM.DB = Convert.ToSingle(delRadBMan.Text);
                EEPROM.DC = Convert.ToSingle(delRadCMan.Text);

                Calibration.BasicCalibration();

                //set eeprom vals in manual calibration
                this.spmMan.Text = EEPROM.stepsPerMM.ToString();
                this.zMaxMan.Text = EEPROM.zMaxLength.ToString();
                this.zProHeiMan.Text = EEPROM.zProbeHeight.ToString();
                this.zProSpeMan.Text = EEPROM.zProbeSpeed.ToString();
                this.diaRodMan.Text = EEPROM.diagonalRod.ToString();
                this.horRadMan.Text = EEPROM.HRadius.ToString();
                this.towOffXMan.Text = EEPROM.offsetX.ToString();
                this.towOffYMan.Text = EEPROM.offsetY.ToString();
                this.towOffZMan.Text = EEPROM.offsetZ.ToString();
                this.alpRotAMan.Text = EEPROM.A.ToString();
                this.alpRotBMan.Text = EEPROM.B.ToString();
                this.alpRotCMan.Text = EEPROM.C.ToString();
                this.delRadAMan.Text = EEPROM.DA.ToString();
                this.delRadBMan.Text = EEPROM.DB.ToString();
                this.delRadCMan.Text = EEPROM.DC.ToString();

                //set expected height map
                this.xExp.Text = Heights.X.ToString();
                this.xOppExp.Text = Heights.XOpp.ToString();
                this.yExp.Text = Heights.Y.ToString();
                this.yOppExp.Text = Heights.YOpp.ToString();
                this.zExp.Text = Heights.Z.ToString();
                this.zOppExp.Text = Heights.ZOpp.ToString();


                Calibration.calibrationState = false;
            }
            catch (Exception ex)
            {
                UserInterface.LogConsole(ex.ToString());
            }
            */
        }

        public void InitializePrinter()
        {
            printer = new Printer();
        }

        public void InitializeSettings()
        {
            settings = new Settings.Builder() {
                CalculationAccuracy = 0,
                HeightmapAccuracy = 0,
                HorizontalRadiusChange = 0,
                ProbingSpeed = 0,
                ProbingHeight = 0,
                StepsPerMMChange = 0,
                TowerSPMChange = 0,
                OppositeSPMChange = 0,
                AlphaRotationChange = 0,
                TowerOffsetCorrectionMain = 0,
                TowerOffsetCorrectionMainOpposite = 0,
                TowerOffsetCorrectionSecondary = 0,
                TowerOffsetCorrectionSecondaryOpposite = 0,
                XYTravelSpeed = 0,
                PlateDiameter = 0,
                Firmware = "",
                COMPort = "",
                ScrollToBottomPrinterLog = true,
                ScrollToBottomSoftwareLog = true,
                MaximumIterations = 0,
                GCodeCalculationAccuracy = 0
            }.Build();
        }

        public void InitializeGCodeCommandsSmoothieware()
        {
            gCodeCommands = new GCodeCommands.Builder() {
                HomeAllAxes = "G28",
                SingleProbe = "G30",
                EmergencyReset = "M112"
            }.Build();
        }
        
        public void InitializeGCodeCommandsRepetier()
        {
            gCodeCommands = new GCodeCommands.Builder()
            {
                HomeAllAxes = "G28",
                SingleProbe = "G30",
                EmergencyReset = "M112"
            }.Build();
        }
        
        public void InitializeGCodeCommandsMarlin()
        {
            gCodeCommands = new GCodeCommands.Builder()
            {
                HomeAllAxes = "G28",
                SingleProbe = "G30",
                EmergencyReset = "M112"
            }.Build();
        }
    }
}
