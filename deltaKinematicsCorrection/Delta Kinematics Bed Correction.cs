using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

namespace deltaKinematics
{
    public partial class Form1 : Form
    {
        //majority of the variables used and modified by the program
        #region Variables
        string versionState = "2.0.5A";
        string wait = "wait";
        string message;
        string _eepromString;
        string comboBoxZMinimumValue;

        int zProbeSet = 0;
        int advancedCalibration = 0;
        int advancedCalCount = 0;
        int maxIterations = 20;
        int calculationCount = 0;
        int calculationCheckCount = 0;
        int pauseTimeSet = 1000;
        int iterationNum = 0;
        int j = 0;
        int t = 0;
        int analyzeCount = 0;
        int calibrationState;
        int stepsCalcNumber = 0;
        int l = 0;

        double tempSPM;
        double centerHeight;
        double X;
        double XOpp;
        double Y;
        double YOpp;
        double Z;
        double ZOpp;
        double tempX;
        double tempXOpp;
        double tempY;
        double tempYOpp;
        double tempZ;
        double tempZOpp;
        double tempX2;
        double tempXOpp2;
        double tempY2;
        double tempYOpp2;
        double tempZ2;
        double tempZOpp2;
        double calculationX;
        double calculationXOpp;
        double calculationY;
        double calculationYOpp;
        double calculationZ;
        double calculationZOpp;
        double calculationTemp1;
        double HRadCorrection;
        double plateDiameter;
        double valueZ;
        double valueXYLarge;
        double valueXYSmall;
        double stepsPerMM;
        double xMaxLength;
        double yMaxLength;
        double zMaxLength;
        double diagonalRod;
        double HRad;
        double HRadSA;
        double offsetX;
        double offsetY;
        double offsetZ;
        double xxPerc;
        double yyPerc;
        double zzPerc;
        double A;
        double B;
        double C;
        double DA;
        double DB;
        double DC;
        double zProbe;
        double XYZAvg;
        double calculationXYZAvg;
        double offsetXYZ;
        double towerXRotation;
        double towerYRotation;
        double towerZRotation;
        double diagonalRodLength;
        double zProbeSpeed;
        double DASA;
        double DBSA;
        double DCSA;
        double centerIterations = 0;
        double xIterations = 0;
        double xOppIterations = 0;
        double yIterations = 0;
        double yOppIterations = 0;
        double zIterations = 0;
        double zOppIterations = 0;
        double probingHeight = 100;
        double HRadRatio = -0.5;
        double accuracy = 0.001;
        double accuracy2 = 0.025;
        double offsetXCorrection = 1.5;
        double offsetYCorrection = 1.5;
        double offsetZCorrection = 1.5;
        double xxOppPerc = 0.5;
        double xyPerc = 0.25;
        double xyOppPerc = 0.25;
        double xzPerc = 0.25;
        double xzOppPerc = 0.25;
        double yyOppPerc = 0.5;
        double yxPerc = 0.25;
        double yxOppPerc = 0.25;
        double yzPerc = 0.25;
        double yzOppPerc = 0.25;
        double zzOppPerc = 0.5;
        double zxPerc = 0.25;
        double zxOppPerc = 0.25;
        double zyPerc = 0.25;
        double zyOppPerc = 0.25;
        double deltaTower = 0.13083;
        double deltaOpp = 0.21083;
        double alphaRotationPercentageX = 1.725;
        double alphaRotationPercentageY = 1.725;
        double alphaRotationPercentageZ = 1.725;

        List<double> known_yDR = new List<double>();
        List<double> known_xDR = new List<double>();

        bool checkHeightsOnly = false;

        static SerialPort _serialPort;
        static bool _continue;
        Thread readThread;
        Boolean _initiatingCalibration = false;
        ErrorProvider errorProvider = new ErrorProvider();
        #endregion

        //calls the initializing function
        #region Misc initializing components
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        //
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //
        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
        #endregion

        // Initialize the application.
        private void Init()
        {
            readThread = new Thread(Read);
            _serialPort = new SerialPort();

            printerConsoleTextBox.Text = "";
            printerConsoleTextBox.ScrollBars = ScrollBars.Vertical;

            consoleTextBox.Text = "";
            consoleTextBox.ScrollBars = ScrollBars.Vertical;

            String[] zMinArray = { "FSR", "Z-Probe" };
            comboZMin.DataSource = zMinArray;

            // Build the combobox of available ports.
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length >= 1)
            {
                Dictionary<string, string> comboSource =
                    new Dictionary<string, string>();

                int count = 0;

                foreach (string element in ports)
                {
                    comboSource.Add(ports[count], ports[count]);
                    count++;
                }

                portComboBox.DataSource = new BindingSource(comboSource, null);
                portComboBox.DisplayMember = "Key";
                portComboBox.ValueMember = "Value";
            }
            else
            {
                LogConsole("No ports available\n");
            }

            // Basic set of standard baud rates.
            cboBaudRate.Items.Add("250000");
            cboBaudRate.Items.Add("115200");
            cboBaudRate.Items.Add("57600");
            cboBaudRate.Items.Add("38400");
            cboBaudRate.Items.Add("19200");
            cboBaudRate.Items.Add("9600");
            cboBaudRate.Text = "250000";  // This is the default for most RAMBo controllers.

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

            //set tower analysis label
            deltaAnalysisDesc.Text = "This analysis may not give\naccurate results. This is due to\nthe error in the steps per\nmilimeter having the same\nresult as the error in tower\nleaning.";
        }

        //Functions that deal with user interaction
        #region User interaction: Console log, setting textboxes
        // Connect to printer.
        private void connectButton_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                LogConsole("Already Connected\n");
            }
            else
            {
                try
                {
                    // Opens a new thread if there has been a previous thread that has closed.
                    if (readThread.IsAlive == false)
                    {
                        readThread = new Thread(Read);
                        _serialPort = new SerialPort();
                    }

                    _serialPort.PortName = portComboBox.Text;
                    _serialPort.BaudRate = int.Parse(cboBaudRate.Text);

                    // Set the read/write timeouts.
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;

                    // Open the serial port and start reading on a reader thread.
                    // _continue is a flag used to terminate the app.

                    if (_serialPort.BaudRate != 0 && _serialPort.PortName != "")
                    {
                        _serialPort.Open();
                        _continue = true;

                        readThread.Start();
                        LogConsole("Connected\n");
                    }
                    else
                    {
                        LogConsole("Please fill all text boxes above\n");
                    }
                }
                catch (Exception e1)
                {
                    LogConsole(e1.Message + "\n");
                    _continue = false;

                    //check if connection is open
                    if (readThread.IsAlive)
                    {
                        readThread.Join();
                    }

                    _serialPort.Close();
                }
            }
        }

        // Disconnect from printer.
        private void disconnectButton_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen && readThread.IsAlive)
            {
                try
                {
                    _continue = false;
                    readThread.Join();
                    _serialPort.Close();
                    LogConsole("Disconnected\n");
                }
                catch (Exception e1)
                {
                    LogConsole(e1.Message + "\n");
                }
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }

        // Send gcode to printer.
        private void sendGCodeButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                string text = textGCode.Text.ToString().ToUpper();
                _serialPort.WriteLine(text + "\n");
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }

        // Clear logs.
        private void clearLogsButton_Click(object sender, EventArgs e)
        {
            printerConsoleTextBox.Text = "";
            consoleTextBox.Text = "";
        }

        // Calibrate.
        private void calibrateButton_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                setVariablesAll();

                calibrationState = 0;
                advancedCalibration = 0;

                //fetches EEProm
                fetchEEProm();
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }

        // Reset printer.
        private void resetPrinterButton_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.WriteLine("M112");
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }

        // Donate.
        private void donateButton_Click(object sender, EventArgs e)
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

        // Contact.
        private void contactButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = "mailto:steventrowland@gmail.com";
            proc.Start();
        }

        // Version information.
        private void versionButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Version: " + versionState + "\n\nCreated by Steven T. Rowland\n\nWith help from Gene Buckle and Michael Hackney\n");
        }

        // Open advanced panel.
        private void openAdvancedPanelButton_Click(object sender, EventArgs e)
        {
            if (advancedPanel.Visible == false)
            {
                advancedPanel.Visible = true;
                tabControl1.Visible = true;
            }
            else
            {
                advancedPanel.Visible = false;
                tabControl1.Visible = false;
            }
        }

        //starts basic offset learning calibration
        private void basicCalibration_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                setVariablesAll();

                calibrationState = 0;
                advancedCalibration = 1;
                //fetches EEProm
                fetchEEProm();
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }

        // Start heuristic calibration.
        private void advancedCalibrationButton_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                setVariablesAll();

                calibrationState = 1;
                advancedCalibration = 1;
                //fetches EEProm
                fetchEEProm();
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }

        //set variables 
        private void setVariablesAll()
        {
            if (_serialPort.IsOpen)
            {
                accuracy = Convert.ToDouble(textAccuracy.Text);
                accuracy2 = Convert.ToDouble(textAccuracy2.Text);
                maxIterations = int.Parse(textMaxIterations.Text);
                pauseTimeSet = int.Parse(textPauseTimeSet.Text);
                probingHeight = double.Parse(textProbingHeight.Text);
                HRadRatio = Convert.ToDouble(textHRadRatio.Text);
                zProbeSpeed = double.Parse(textProbingSpeed.Text);

                offsetXCorrection = 1.55;
                offsetYCorrection = 1.55;
                offsetZCorrection = 1.55;

                //XYZ offset
                //X
                xxOppPerc = -0.352;
                xyPerc = -0.232;
                xyOppPerc = 0.163;
                xzPerc = -0.232;
                xzOppPerc = 0.163;

                //Y
                yyOppPerc = -0.352;
                yxPerc = -0.232;
                yxOppPerc = 0.163;
                yzPerc = -0.232;
                yzOppPerc = 0.163;

                //Z
                zzOppPerc = -0.352;
                zxPerc = -0.232;
                zxOppPerc = 0.163;
                zyPerc = -0.232;
                zyOppPerc = 0.163;

                /*
                //XYZ offset
                //X
                xxOppPerc = Convert.ToDouble(textxxOppPerc.Text);
                xyPerc = Convert.ToDouble(textxyPerc.Text);
                xyOppPerc = Convert.ToDouble(textxyOppPerc.Text);
                xzPerc = Convert.ToDouble(textxzPerc.Text);
                xzOppPerc = Convert.ToDouble(textxzOppPerc.Text);

                //Y
                yyOppPerc = Convert.ToDouble(textyyOppPerc.Text);
                yxPerc = Convert.ToDouble(textyxPerc.Text);
                yxOppPerc = Convert.ToDouble(textyxOppPerc.Text);
                yzPerc = Convert.ToDouble(textyzPerc.Text);
                yzOppPerc = Convert.ToDouble(textyzOppPerc.Text);

                //Z
                zzOppPerc = Convert.ToDouble(textzzOppPerc.Text);
                zxPerc = Convert.ToDouble(textzxPerc.Text);
                zxOppPerc = Convert.ToDouble(textzxOppPerc.Text);
                zyPerc = Convert.ToDouble(textzyPerc.Text);
                zyOppPerc = Convert.ToDouble(textzyOppPerc.Text);
                */

                //diagonal rod
                deltaTower = Convert.ToDouble(textDeltaTower.Text);
                deltaOpp = Convert.ToDouble(textDeltaOpp.Text);
                diagonalRodLength = Convert.ToDouble(textDiagonalRod.Text);

                _serialPort.WriteLine("M206 T3 P812 X" + textProbingSpeed.Text.ToString());
                _serialPort.WriteLine("M206 T3 808 X" + textZProbeHeight.Text.ToString());

                LogConsole("Setting Z-Probe Speed\n");
                LogConsole("Setting Z-Probe Height\n");
                Thread.Sleep(pauseTimeSet);

                LogConsole("Variables set\n");
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }

        //prints to printer console
        public void LogMessage(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(LogMessage), new object[] { value });
                return;
            }
            printerConsoleTextBox.AppendText(value + "\n");
        }

        //prints to console
        public void LogConsole(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(LogConsole), new object[] { value });
                return;
            }
            consoleTextBox.AppendText(value + "\n");
        }

        //set height-map values
        public void setHeights()
        {
            //set base heights for advanced calibration comparison
            if (iterationNum == 0)
            {
                Invoke((MethodInvoker)delegate { this.textXTemp.Text = Math.Round(X, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textXOppTemp.Text = Math.Round(XOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textYTemp.Text = Math.Round(Y, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textYOppTemp.Text = Math.Round(YOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textZTemp.Text = Math.Round(Z, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textZOppTemp.Text = Math.Round(ZOpp, 3).ToString(); });

                //calculate parameters
                tempX = X;
                tempXOpp = XOpp;
                tempY = Y;
                tempYOpp = YOpp;
                tempZ = Z;
                tempZOpp = ZOpp;
            }
            else
            {
                Invoke((MethodInvoker)delegate { this.textX.Text = Math.Round(X, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textXOpp.Text = Math.Round(XOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textY.Text = Math.Round(Y, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textYOpp.Text = Math.Round(YOpp, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textZ.Text = Math.Round(Z, 3).ToString(); });
                Invoke((MethodInvoker)delegate { this.textZOpp.Text = Math.Round(ZOpp, 3).ToString(); });
            }
        }

        //
        public void setAdvancedCalVars()
        {
            Invoke((MethodInvoker)delegate { this.textDeltaTower.Text = Math.Round(deltaTower, 3).ToString(); });
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
        }
        #endregion

        // The reader thread. Continue reading as long as _continue is true.
        void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();

                    if (!_initiatingCalibration)
                    {
                        LogMessage(message + "\n");
                    }
                    else
                    {
                        LogMessage(message + "\n");

                        if (message.Contains("Z-probe:"))
                        {
                            //Z-probe: 10.66 zCorr: 0

                            string[] parseInData = message.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            string[] parseFirstLine = parseInData[0].Split(new char[] { ':', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            //: 10.66 zCorr: 0
                            string[] parseZProbe = message.Split(new string[] { "Z-probe", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] parseZProbeSpace = parseZProbe[0].Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            double zProbeParse;

                            //check if there is a space between
                            if (parseZProbeSpace[0] == ":")
                            {
                                //Space
                                zProbeParse = double.Parse(parseZProbeSpace[1]);
                            }
                            else
                            {
                                //No space
                                zProbeParse = double.Parse(parseZProbeSpace[0].Substring(1));
                            }

                            //use returned probe height to calculate the actual z-Probe height
                            if (zProbeSet == 1)
                            {
                                LogConsole("Z-Probe length set to: " + (zMaxLength - Convert.ToDouble(parseFirstLine[1])) + "\n");
                                zProbe = zMaxLength - Convert.ToDouble(parseFirstLine[1]);
                                zProbeSet = 0;
                            }
                            else if (centerIterations == iterationNum)
                            {
                                //LogConsole("Z-Probe Center Height: " + parseFirstLine[1] + "\n");
                                centerHeight = Convert.ToDouble(parseFirstLine[1]);

                                centerIterations++;

                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");

                                Thread.Sleep(pauseTimeSet);
                                //X axis
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G30");
                            }
                            else if (xIterations == iterationNum)
                            {
                                //LogMessage("Z-Probe X Height: " + parseFirstLine[1] + "\n");
                                X = Convert.ToDouble(parseFirstLine[1]);

                                xIterations++;

                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G30");
                            }
                            else if (xOppIterations == iterationNum)
                            {
                                //LogMessage("Z-Probe X Opposite Height: " + parseFirstLine[1] + "\n");
                                XOpp = Convert.ToDouble(parseFirstLine[1]);

                                xOppIterations++;

                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                                Thread.Sleep(pauseTimeSet);

                                //Y axis
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G30");
                            }
                            else if (yIterations == iterationNum)
                            {
                                //LogMessage("Z-Probe Y Height: " + parseFirstLine[1] + "\n");
                                Y = Convert.ToDouble(parseFirstLine[1]);

                                yIterations++;

                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X" + valueXYLarge.ToString() + " Y-" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G30");
                            }
                            else if (yOppIterations == iterationNum)
                            {
                                //LogMessage("Z-Probe Y Opposite Height: " + parseFirstLine[1] + "\n");
                                YOpp = Convert.ToDouble(parseFirstLine[1]);

                                yOppIterations++;

                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X-" + valueXYLarge.ToString() + " Y" + valueXYSmall.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                                Thread.Sleep(pauseTimeSet);

                                //Z axis
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y" + valueZ.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G30");
                            }
                            else if (zIterations == iterationNum)
                            {
                                //LogMessage("Z-Probe Z Height: " + parseFirstLine[1] + "\n");
                                Z = Convert.ToDouble(parseFirstLine[1]);

                                zIterations++;

                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y" + valueZ.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y-" + valueZ.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G30");
                            }
                            else if (zOppIterations == iterationNum)
                            {
                                //LogMessage("Z-Probe Z Opposite Height: " + parseFirstLine[1] + "\n");
                                ZOpp = Convert.ToDouble(parseFirstLine[1]);

                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " Y-" + valueZ.ToString());
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " X0 Y0");
                                Thread.Sleep(pauseTimeSet);

                                centerHeight = zMaxLength - probingHeight + centerHeight;
                                X = centerHeight - (zMaxLength - probingHeight + X);
                                XOpp = centerHeight - (zMaxLength - probingHeight + XOpp);
                                Y = centerHeight - (zMaxLength - probingHeight + Y);
                                YOpp = centerHeight - (zMaxLength - probingHeight + YOpp);
                                Z = centerHeight - (zMaxLength - probingHeight + Z);
                                ZOpp = centerHeight - (zMaxLength - probingHeight + ZOpp);

                                //invert values
                                X = -X;
                                XOpp = -XOpp;
                                Y = -Y;
                                YOpp = -YOpp;
                                Z = -Z;
                                ZOpp = -ZOpp;

                                // Sets height-maps in separate function
                                setHeights();

                                _serialPort.WriteLine("M206 T3 P153 X" + centerHeight);
                                LogConsole("Setting Z Max Length\n");
                                Thread.Sleep(pauseTimeSet);

                                zMaxLength = centerHeight;

                                zOppIterations++;

                                if (advancedCalibration == 1)
                                {
                                    //find base heights
                                    //find heights with each value increased by 1 - HRad, tower offset 1-3, diagonal rod

                                    if (advancedCalCount == 0)
                                    {//start
                                        if (_serialPort.IsOpen)
                                        {
                                            //set diagonal rod +1
                                            _serialPort.WriteLine("M206 T3 P881 X" + (diagonalRod + 1).ToString());
                                            LogConsole("Setting diagonal rod to: " + (diagonalRod + 1).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;
                                    }
                                    else if (advancedCalCount == 1)
                                    {//get diagonal rod percentages

                                        deltaTower = ((tempX - X) + (tempY - Y) + (tempZ - Z)) / 3;
                                        deltaOpp = ((tempXOpp - XOpp) + (tempYOpp - YOpp) + (tempZOpp - ZOpp)) / 3;

                                        if (_serialPort.IsOpen)
                                        {
                                            //reset diagonal rod
                                            _serialPort.WriteLine("M206 T3 P881 X" + (diagonalRod).ToString());
                                            LogConsole("Setting diagonal rod to: " + (diagonalRod).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                            //set Hrad +1
                                            _serialPort.WriteLine("M206 T3 P885 X" + (HRad + 1).ToString());
                                            LogConsole("Setting Horizontal Radius to: " + (HRad + 1).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;
                                    }
                                    else if (advancedCalCount == 2)
                                    {//get HRad percentages
                                        HRadRatio = -(Math.Abs((X - tempX) + (Y - tempY) + (Z - tempZ) + (XOpp - tempXOpp) + (YOpp - tempYOpp) + (ZOpp - tempZOpp))) / 6;

                                        if (_serialPort.IsOpen)
                                        {
                                            //reset horizontal radius
                                            _serialPort.WriteLine("M206 T3 P885 X" + (HRad).ToString());
                                            LogConsole("Setting Horizontal Radius to: " + (HRad).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                            //set X offset
                                            _serialPort.WriteLine("M206 T1 P893 S" + (offsetX + 80).ToString());
                                            LogConsole("Setting offset X to: " + (offsetX + 80).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;
                                    }
                                    else if (advancedCalCount == 3)
                                    {//get X offset percentages

                                        offsetXCorrection = Math.Abs(1 / (X - tempX));
                                        xxOppPerc = Math.Abs((XOpp - tempXOpp) / (X - tempX));
                                        xyPerc = Math.Abs((Y - tempY) / (X - tempX));
                                        xyOppPerc = Math.Abs((YOpp - tempYOpp) / (X - tempX));
                                        xzPerc = Math.Abs((Z - tempZ) / (X - tempX));
                                        xzOppPerc = Math.Abs((ZOpp - tempZOpp) / (X - tempX));

                                        if (_serialPort.IsOpen)
                                        {
                                            //reset X offset
                                            _serialPort.WriteLine("M206 T1 P893 S" + (offsetX).ToString());
                                            LogConsole("Setting offset X to: " + (offsetX).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                            //set Y offset
                                            _serialPort.WriteLine("M206 T1 P895 S" + (offsetY + 80).ToString());
                                            LogConsole("Setting offset Y to: " + (offsetY + 80).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;
                                    }
                                    else if (advancedCalCount == 4)
                                    {//get Y offset percentages

                                        offsetYCorrection = Math.Abs(1 / (Y - tempY));
                                        yyOppPerc = Math.Abs((YOpp - tempYOpp) / (Y - tempY));
                                        yxPerc = Math.Abs((X - tempX) / (Y - tempY));
                                        yxOppPerc = Math.Abs((XOpp - tempXOpp) / (Y - tempY));
                                        yzPerc = Math.Abs((Z - tempZ) / (Y - tempY));
                                        yzOppPerc = Math.Abs((ZOpp - tempZOpp) / (Y - tempY));

                                        if (_serialPort.IsOpen)
                                        {
                                            //reset Y offset
                                            _serialPort.WriteLine("M206 T1 P895 S" + (offsetY).ToString());
                                            LogConsole("Setting offset Y to: " + (offsetY).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                            //set Z offset
                                            _serialPort.WriteLine("M206 T1 P897 S" + (offsetZ + 80).ToString());
                                            LogConsole("Setting offset Z to: " + (offsetZ + 80).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;
                                    }
                                    else if (advancedCalCount == 5)
                                    {//get Z offset percentages

                                        offsetZCorrection = Math.Abs(1 / (Z - tempZ));
                                        zzOppPerc = Math.Abs((ZOpp - tempZOpp) / (Z - tempZ));
                                        zxPerc = Math.Abs((X - tempX) / (Z - tempZ));
                                        zxOppPerc = Math.Abs((XOpp - tempXOpp) / (Z - tempZ));
                                        zyPerc = Math.Abs((Y - tempY) / (Z - tempZ));
                                        zyOppPerc = Math.Abs((YOpp - tempYOpp) / (Z - tempZ));

                                        if (_serialPort.IsOpen)
                                        {
                                            //set Z offset
                                            _serialPort.WriteLine("M206 T1 P897 S" + (offsetZ).ToString());
                                            LogConsole("Setting offset Z to: " + (offsetZ).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                            //set alpha rotation offset perc X
                                            _serialPort.WriteLine("M206 T3 P901 X" + (A + 1).ToString());
                                            LogConsole("Setting Alpha A to: " + (A + 1).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;

                                    }
                                    else if (advancedCalCount == 6)//6
                                    {//get A alpha rotation

                                        alphaRotationPercentageX = (2 / Math.Abs((YOpp - ZOpp) - (tempYOpp - tempZOpp)));

                                        if (_serialPort.IsOpen)
                                        {
                                            //set alpha rotation offset perc X
                                            _serialPort.WriteLine("M206 T3 P901 X" + (A).ToString());
                                            LogConsole("Setting Alpha A to: " + (A).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                            //set alpha rotation offset perc Y
                                            _serialPort.WriteLine("M206 T3 P905 X" + (B + 1).ToString());
                                            LogConsole("Setting Alpha B to: " + (B + 1).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;
                                    }
                                    else if (advancedCalCount == 7)//7
                                    {//get B alpha rotation

                                        alphaRotationPercentageY = (2 / Math.Abs((ZOpp - XOpp) - (tempZOpp - tempXOpp)));

                                        if (_serialPort.IsOpen)
                                        {
                                            //set alpha rotation offset perc Y
                                            _serialPort.WriteLine("M206 T3 P905 X" + (B).ToString());
                                            LogConsole("Setting Alpha B to: " + (B).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                            //set alpha rotation offset perc Z
                                            _serialPort.WriteLine("M206 T3 P909 X" + (C + 1).ToString());
                                            LogConsole("Setting Alpha C to: " + (C + 1).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        initiateCal();

                                        advancedCalCount++;
                                    }
                                    else if (advancedCalCount == 8)//8
                                    {//get C alpha rotation

                                        alphaRotationPercentageZ = (2 / Math.Abs((XOpp - YOpp) - (tempXOpp - tempYOpp)));

                                        if (_serialPort.IsOpen)
                                        {
                                            //set alpha rotation offset perc Z
                                            _serialPort.WriteLine("M206 T3 P909 X" + (C).ToString());
                                            LogConsole("Setting Alpha C to: " + (C).ToString() + "\n");
                                            Thread.Sleep(pauseTimeSet);

                                        }

                                        LogConsole("Alpha offset percentages: " + alphaRotationPercentageX + ", " + alphaRotationPercentageY + ", and" + alphaRotationPercentageZ + "\n");

                                        advancedCalibration = 0;
                                        advancedCalCount = 0;

                                        initiateCal();

                                        setAdvancedCalVars();
                                    }

                                    iterationNum++;
                                }
                                else
                                {
                                    iterationNum++;

                                    if (checkHeightsOnly == false)
                                    {
                                        calibratePrinter();
                                    }
                                    else
                                    {
                                        Thread.Sleep(pauseTimeSet);
                                        _serialPort.WriteLine("G28");
                                        checkHeightsOnly = false;
                                    }
                                }
                            }
                        }

                        //parse EEProm
                        if (message.Contains("EPR"))
                        {
                            string[] parseEPR = message.Split(new string[] { "EPR", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string[] parseEPRSpace;

                            if (parseEPR.Length > 1)
                            {
                                parseEPRSpace = parseEPR[1].Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            else
                            {
                                parseEPRSpace = parseEPR[0].Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            }

                            int intParse;
                            double doubleParse2;

                            //check if there is a space between
                            if (parseEPRSpace[0] == ":")
                            {
                                //Space
                                intParse = int.Parse(parseEPRSpace[2]);
                                doubleParse2 = double.Parse(parseEPRSpace[3]);
                            }
                            else
                            {
                                //No space
                                intParse = int.Parse(parseEPRSpace[1]);
                                doubleParse2 = double.Parse(parseEPRSpace[2]);
                            }


                            if (intParse == 11)
                            {
                                LogConsole("EEProm capture initiated\n");
                                stepsPerMM = doubleParse2;
                                tempSPM = stepsPerMM;
                            }
                            else if (intParse == 145)
                            {
                                xMaxLength = doubleParse2;
                            }
                            else if (intParse == 149)
                            {
                                yMaxLength = doubleParse2;
                            }
                            else if (intParse == 153)
                            {
                                zMaxLength = doubleParse2;
                            }
                            else if (intParse == 808)
                            {
                                zProbe = doubleParse2;
                            }
                            else if (intParse == 881)
                            {
                                diagonalRod = doubleParse2;

                                //check if diag rod textbox was used, else use the eeprom value
                                if (diagonalRodLength == 0)
                                {
                                    diagonalRodLength = doubleParse2;
                                }
                            }
                            else if (intParse == 885)
                            {
                                HRad = doubleParse2;
                            }
                            else if (intParse == 893)
                            {
                                offsetX = doubleParse2;
                            }
                            else if (intParse == 895)
                            {
                                offsetY = doubleParse2;
                            }
                            else if (intParse == 897)
                            {
                                offsetZ = doubleParse2;
                            }
                            else if (intParse == 901)
                            {
                                A = doubleParse2;
                            }
                            else if (intParse == 905)
                            {
                                B = doubleParse2;
                            }
                            else if (intParse == 909)
                            {
                                C = doubleParse2;
                            }
                            else if (intParse == 913)
                            {
                                DA = doubleParse2;
                            }
                            else if (intParse == 917)
                            {
                                DB = doubleParse2;
                            }
                            else if (intParse == 921)
                            {
                                DC = doubleParse2;

                                LogConsole("EEProm:  Steps:" + stepsPerMM + ", X Max:" + xMaxLength + ", Y Max:" + yMaxLength + ", Z Max:" + zMaxLength + ", Z-Probe Offset:" + zProbe + ", Diagonal Rod:" + diagonalRod + ", Horizontal Radius:" + HRad + ", X Offset:" + offsetX + ", Y Offset:" + offsetY + ", Z Offset:" + offsetZ + ", Alpha A:" + A + ", Alpha B:" + B + ",  Alpha C:" + C + ", Delta A:" + DA + ", Delta B:" + DB + ", and Delta C:" + DC + "\n");
                                LogConsole("EEProm captured, beginning calibration.");

                                // Once the program has the EEProm stored, calibration initiates
                                initiateCal();
                            }
                        }//end EEProm capture
                    }
                }

                catch (TimeoutException) { }
            }
        }

        //Starts the calibration through sending gcode, once received by the reader then calibratePrinter will be called
        private void initiateCal()
        {
            //set gcode specifications
            valueZ = 0.482 * plateDiameter;
            valueXYLarge = 0.417 * plateDiameter;
            valueXYSmall = 0.241 * plateDiameter;

            //set zprobe height to zero

            if (comboBoxZMinimumValue == "Z-Probe" && iterationNum == 0)
            {
                zProbeSet = 1;
                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("G28");
                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("G30");

                double heightTime = (zMaxLength / zProbeSpeed) * 1000;
                Thread.Sleep(Convert.ToInt32(heightTime));
            }

            //zero bed
            Thread.Sleep(pauseTimeSet);
            _serialPort.WriteLine("G28");
            Thread.Sleep(pauseTimeSet);
            _serialPort.WriteLine("G1 Z" + probingHeight.ToString() + " F5000");
            Thread.Sleep(pauseTimeSet);
            _serialPort.WriteLine("G30");
        }

        //Main calculation for calibration
        private void calibratePrinter()
        {
            //check accuracy of current height-map and determine to end or procede
            if (X <= accuracy2 && X >= -accuracy2 && XOpp <= accuracy2 && XOpp >= -accuracy2 && Y <= accuracy2 && Y >= -accuracy2 && YOpp <= accuracy2 && YOpp >= -accuracy2 && Z <= accuracy2 && Z >= -accuracy2 && ZOpp <= accuracy2 && ZOpp >= -accuracy2 && (diagonalRod - diagonalRodLength) <= 0.25 && (diagonalRod - diagonalRodLength) >= -0.25)
            {
                //fsr plate offset
                if (comboBoxZMinimumValue == "FSR")
                {
                    _serialPort.WriteLine("M206 T3 P153 X" + (centerHeight - Convert.ToDouble(textFSRPlateOffset.Text)));
                    LogConsole("Setting Z Max Length with adjustment for FSR\n");
                    Thread.Sleep(pauseTimeSet);
                }

                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("G28");
                LogConsole("Calibration Complete\n");
                //end code
            }
            else if (iterationNum > maxIterations)
            {
                //max iterations hit
                LogConsole("Calibration Failed\n");
                LogConsole("Maximum number of iterations hit. Please restart application and run the advanced calibration.\n");
            }
            else
            {
                //logs current iteration number
                LogConsole("Current iteration: " + iterationNum + "\n");

                //basic calibration
                if (calibrationState == 0)
                {
                    //////////////////////////////////////////////////////////////////////////////
                    //HRad is calibrated by increasing the outside edge of the glass by the average differences, this should balance the values with a central point of around zero
                    double HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);

                    HRad = HRad + (HRadSA / HRadRatio);

                    X = X - HRadSA;
                    Y = Y - HRadSA;
                    Z = Z - HRadSA;
                    XOpp = XOpp - HRadSA;
                    YOpp = YOpp - HRadSA;
                    ZOpp = ZOpp - HRadSA;

                    X = checkZero(X);
                    Y = checkZero(Y);
                    Z = checkZero(Z);
                    XOpp = checkZero(XOpp);
                    YOpp = checkZero(YOpp);
                    ZOpp = checkZero(ZOpp);

                    LogConsole("HRad:" + HRad + "\n");

                    ////////////////////////////////////////////////////////////////////////////////
                    //Delta Radius Calibration******************************************************
                    DASA = ((X + XOpp) / 2);
                    DBSA = ((Y + YOpp) / 2);
                    DCSA = ((Z + ZOpp) / 2);

                    DA = DA + ((DASA) / HRadRatio);
                    DB = DB + ((DBSA) / HRadRatio);
                    DC = DC + ((DCSA) / HRadRatio);

                    X = X + ((DASA) / HRadRatio) * 0.5;
                    XOpp = XOpp + ((DASA) / HRadRatio) * 0.225;
                    Y = Y + ((DASA) / HRadRatio) * 0.1375;
                    YOpp = YOpp + ((DASA) / HRadRatio) * 0.1375;
                    Z = Z + ((DASA) / HRadRatio) * 0.1375;
                    ZOpp = ZOpp + ((DASA) / HRadRatio) * 0.1375;

                    X = X + ((DBSA) / HRadRatio) * 0.1375;
                    XOpp = XOpp + ((DBSA) / HRadRatio) * 0.1375;
                    Y = Y + ((DBSA) / HRadRatio) * 0.5;
                    YOpp = YOpp + ((DBSA) / HRadRatio) * 0.225;
                    Z = Z + ((DBSA) / HRadRatio) * 0.1375;
                    ZOpp = ZOpp + ((DBSA) / HRadRatio) * 0.1375;

                    X = X + ((DCSA) / HRadRatio) * 0.1375;
                    XOpp = XOpp + ((DCSA) / HRadRatio) * 0.1375;
                    Y = Y + ((DCSA) / HRadRatio) * 0.1375;
                    YOpp = YOpp + ((DCSA) / HRadRatio) * 0.1375;
                    Z = Z + ((DCSA) / HRadRatio) * 0.5;
                    ZOpp = ZOpp + ((DCSA) / HRadRatio) * 0.225;

                    DA = checkZero(DA);
                    DB = checkZero(DB);
                    DC = checkZero(DC);

                    LogConsole("Delta Radii Offsets: " + DA.ToString() + ", " + DB.ToString() + ", " + DC.ToString());

                    _serialPort.WriteLine("M206 T3 P913 X" + ToLongString(DA));
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T3 P917 X" + ToLongString(DB));
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T3 P921 X" + ToLongString(DC));
                    Thread.Sleep(pauseTimeSet);

                    //analyzes the printer geometry
                    if (analyzeCount == 0)
                    {
                        analyzeCount++;
                        analyzeGeometry();

                        LogConsole("Expect a slight inaccuracy in the geometry analysis; basic calibration.");
                    }

                    ////////////////////////////////////////////////////////////////////////////////
                    //Tower Offset Calibration******************************************************
                    int j = 0;
                    tempX2 = X;
                    tempXOpp2 = XOpp;
                    tempY2 = Y;
                    tempYOpp2 = YOpp;
                    tempZ2 = Z;
                    tempZOpp2 = ZOpp;

                    while (j < 100)
                    {
                        double theoryX = offsetX + tempX2 * stepsPerMM * offsetXCorrection;

                        //correction of one tower allows for XY dimensional accuracy
                        if (tempX2 > 0)
                        {
                            //if x is positive
                            offsetX = offsetX + tempX2 * stepsPerMM * offsetXCorrection;

                            tempXOpp2 = tempXOpp2 + (tempX2 * xxOppPerc);//0.5
                            tempZ2 = tempZ2 + (tempX2 * xzPerc);//0.25
                            tempY2 = tempY2 + (tempX2 * xyPerc);//0.25
                            tempZOpp2 = tempZOpp2 - (tempX2 * xzOppPerc);//0.25
                            tempYOpp2 = tempYOpp2 - (tempX2 * xyOppPerc);//0.25
                            tempX2 = tempX2 - tempX2;
                        }
                        else if (theoryX > 0 && tempX2 < 0)
                        {
                            //if x is negative and can be decreased
                            offsetX = offsetX + tempX2 * stepsPerMM * offsetXCorrection;

                            tempXOpp2 = tempXOpp2 + (tempX2 * xxOppPerc);//0.5
                            tempZ2 = tempZ2 + (tempX2 * xzPerc);//0.25
                            tempY2 = tempY2 + (tempX2 * xyPerc);//0.25
                            tempZOpp2 = tempZOpp2 - (tempX2 * xzOppPerc);//0.25
                            tempYOpp2 = tempYOpp2 - (tempX2 * xyOppPerc);//0.25
                            tempX2 = tempX2 - tempX2;
                        }
                        else
                        {
                            //if tempX2 is negative
                            offsetY = offsetY - tempX2 * stepsPerMM * offsetYCorrection * 2;
                            offsetZ = offsetZ - tempX2 * stepsPerMM * offsetZCorrection * 2;

                            tempYOpp2 = tempYOpp2 - (tempX2 * 2 * yyOppPerc);
                            tempX2 = tempX2 - (tempX2 * 2 * yxPerc);
                            tempZ2 = tempZ2 - (tempX2 * 2 * yxPerc);
                            tempXOpp2 = tempXOpp2 + (tempX2 * 2 * yxOppPerc);
                            tempZOpp2 = tempZOpp2 + (tempX2 * 2 * yxOppPerc);
                            tempY2 = tempY2 + tempX2 * 2;

                            tempZOpp2 = tempZOpp2 - (tempX2 * 2 * zzOppPerc);
                            tempX2 = tempX2 - (tempX2 * 2 * zxPerc);
                            tempY2 = tempY2 - (tempX2 * 2 * zyPerc);
                            tempXOpp2 = tempXOpp2 + (tempX2 * 2 * yxOppPerc);
                            tempYOpp2 = tempYOpp2 + (tempX2 * 2 * zyOppPerc);
                            tempZ2 = tempZ2 + tempX2 * 2;
                        }

                        double theoryY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                        //Y
                        if (tempY2 > 0)
                        {
                            offsetY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                            tempYOpp2 = tempYOpp2 + (tempY2 * yyOppPerc);
                            tempX2 = tempX2 + (tempY2 * yxPerc);
                            tempZ2 = tempZ2 + (tempY2 * yxPerc);
                            tempXOpp2 = tempXOpp2 - (tempY2 * yxOppPerc);
                            tempZOpp2 = tempZOpp2 - (tempY2 * yxOppPerc);
                            tempY2 = tempY2 - tempY2;
                        }
                        else if (theoryY > 0 && tempY2 < 0)
                        {
                            offsetY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                            tempYOpp2 = tempYOpp2 + (tempY2 * yyOppPerc);
                            tempX2 = tempX2 + (tempY2 * yxPerc);
                            tempZ2 = tempZ2 + (tempY2 * yxPerc);
                            tempXOpp2 = tempXOpp2 - (tempY2 * yxOppPerc);
                            tempZOpp2 = tempZOpp2 - (tempY2 * yxOppPerc);
                            tempY2 = tempY2 - tempY2;
                        }
                        else
                        {
                            offsetX = offsetX - tempY2 * stepsPerMM * offsetXCorrection * 2;
                            offsetZ = offsetZ - tempY2 * stepsPerMM * offsetZCorrection * 2;

                            tempXOpp2 = tempXOpp2 - (tempY2 * 2 * xxOppPerc);//0.5
                            tempZ2 = tempZ2 - (tempY2 * 2 * xzPerc);//0.25
                            tempY2 = tempY2 - (tempY2 * 2 * xyPerc);//0.25
                            tempZOpp2 = tempZOpp2 + (tempY2 * 2 * xzOppPerc);//0.25
                            tempYOpp2 = tempYOpp2 + (tempY2 * 2 * xyOppPerc);//0.25
                            tempX2 = tempX2 + tempY2 * 2;

                            tempZOpp2 = tempZOpp2 - (tempY2 * 2 * zzOppPerc);
                            tempX2 = tempX2 - (tempY2 * 2 * zxPerc);
                            tempY2 = tempY2 - (tempY2 * 2 * zyPerc);
                            tempXOpp2 = tempXOpp2 + (tempY2 * 2 * yxOppPerc);
                            tempYOpp2 = tempYOpp2 + (tempY2 * 2 * zyOppPerc);
                            tempZ2 = tempZ2 + tempY2 * 2;
                        }

                        double theoryZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                        //Z
                        if (tempZ2 > 0)
                        {
                            offsetZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                            tempZOpp2 = tempZOpp2 + (tempZ2 * zzOppPerc);
                            tempX2 = tempX2 + (tempZ2 * zxPerc);
                            tempY2 = tempY2 + (tempZ2 * zyPerc);
                            tempXOpp2 = tempXOpp2 - (tempZ2 * yxOppPerc);
                            tempYOpp2 = tempYOpp2 - (tempZ2 * zyOppPerc);
                            tempZ2 = tempZ2 - tempZ2;
                        }
                        else if (theoryZ > 0 && tempZ2 < 0)
                        {
                            offsetZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                            tempZOpp2 = tempZOpp2 + (tempZ2 * zzOppPerc);
                            tempX2 = tempX2 + (tempZ2 * zxPerc);
                            tempY2 = tempY2 + (tempZ2 * zyPerc);
                            tempXOpp2 = tempXOpp2 - (tempZ2 * yxOppPerc);
                            tempYOpp2 = tempYOpp2 - (tempZ2 * zyOppPerc);
                            tempZ2 = tempZ2 - tempZ2;
                        }
                        else
                        {
                            offsetY = offsetY - tempZ2 * stepsPerMM * offsetYCorrection * 2;
                            offsetX = offsetX - tempZ2 * stepsPerMM * offsetXCorrection * 2;

                            tempXOpp2 = tempXOpp2 - (tempZ2 * 2 * xxOppPerc);//0.5
                            tempZ2 = tempZ2 - (tempZ2 * 2 * xzPerc);//0.25
                            tempY2 = tempY2 - (tempZ2 * 2 * xyPerc);//0.25
                            tempZOpp2 = tempZOpp2 + (tempZ2 * 2 * xzOppPerc);//0.25
                            tempYOpp2 = tempYOpp2 + (tempZ2 * 2 * xyOppPerc);//0.25
                            tempX2 = tempX2 + tempZ2 * 2;

                            tempYOpp2 = tempYOpp2 - (tempZ2 * 2 * yyOppPerc);
                            tempX2 = tempX2 - (tempZ2 * 2 * yxPerc);
                            tempZ2 = tempZ2 - (tempZ2 * 2 * yxPerc);
                            tempXOpp2 = tempXOpp2 + (tempZ2 * 2 * yxOppPerc);
                            tempZOpp2 = tempZOpp2 + (tempZ2 * 2 * yxOppPerc);
                            tempY2 = tempY2 + tempZ2 * 2;
                        }

                        tempX2 = checkZero(tempX2);
                        tempY2 = checkZero(tempY2);
                        tempZ2 = checkZero(tempZ2);
                        tempXOpp2 = checkZero(tempXOpp2);
                        tempYOpp2 = checkZero(tempYOpp2);
                        tempZOpp2 = checkZero(tempZOpp2);

                        if (tempX2 < accuracy && tempX2 > -accuracy && tempY2 < accuracy && tempY2 > -accuracy && tempZ2 < accuracy && tempZ2 > -accuracy && offsetX < 1000 && offsetY < 1000 && offsetZ < 1000)
                        {
                            j = 100;
                        }
                        else if (j == 50)
                        {
                            //error protection
                            tempX2 = X;
                            tempXOpp2 = XOpp;
                            tempY2 = Y;
                            tempYOpp2 = YOpp;
                            tempZ2 = Z;
                            tempZOpp2 = ZOpp;

                            //X
                            offsetXCorrection = 1.5;
                            xxOppPerc = 0.5;
                            xyPerc = 0.25;
                            xyOppPerc = 0.25;
                            xzPerc = 0.25;
                            xzOppPerc = 0.25;

                            //Y
                            offsetYCorrection = 1.5;
                            yyOppPerc = 0.5;
                            yxPerc = 0.25;
                            yxOppPerc = 0.25;
                            yzPerc = 0.25;
                            yzOppPerc = 0.25;

                            //Z
                            offsetZCorrection = 1.5;
                            zzOppPerc = 0.5;
                            zxPerc = 0.25;
                            zxOppPerc = 0.25;
                            zyPerc = 0.25;
                            zyOppPerc = 0.25;

                            offsetX = 0;
                            offsetY = 0;
                            offsetZ = 0;

                            j++;
                        }
                        else
                        {
                            j++;
                        }
                    }

                    if (offsetX > 1000 || offsetY > 1000 || offsetZ > 1000)
                    {
                        LogConsole("XYZ offset calibration error, setting default values.");
                        LogConsole("XYZ offsets before damage prevention: X" + offsetX + " Y" + offsetY + " Z" + offsetZ + "\n");
                        offsetX = 0;
                        offsetY = 0;
                        offsetZ = 0;
                    }
                    else
                    {
                        X = tempX2;
                        XOpp = tempXOpp2;
                        Y = tempY2;
                        YOpp = tempYOpp2;
                        Z = tempZ2;
                        ZOpp = tempZOpp2;
                    }

                    //round to the nearest whole number
                    offsetX = Math.Round(offsetX);
                    offsetY = Math.Round(offsetY);
                    offsetZ = Math.Round(offsetZ);

                    LogConsole("XYZ:" + offsetX + " " + offsetY + " " + offsetZ + "\n");

                    //send data back to printer
                    _serialPort.WriteLine("M206 T1 P893 S" + offsetX.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T1 P895 S" + offsetY.ToString());
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T1 P897 S" + offsetZ.ToString());
                    Thread.Sleep(pauseTimeSet);

                    ////////////////////////////////////////////////////////////////////////////////
                    //Alpha Rotation Calibration****************************************************

                    if (offsetX != 0 && offsetY != 0 && offsetZ != 0)
                    {
                        int k = 0;
                        while (k < 100)
                        {
                            //X Alpha Rotation
                            if (YOpp > ZOpp)
                            {
                                double ZYOppAvg = (YOpp - ZOpp) / 2;
                                A = A + (ZYOppAvg * alphaRotationPercentageX); // (0.5/((diff y0 and z0 at X + 0.5)-(diff y0 and z0 at X = 0))) * 2 = 1.75
                                YOpp = YOpp - ZYOppAvg;
                                ZOpp = ZOpp + ZYOppAvg;
                            }
                            else if (YOpp < ZOpp)
                            {
                                double ZYOppAvg = (ZOpp - YOpp) / 2;

                                A = A - (ZYOppAvg * alphaRotationPercentageX);
                                YOpp = YOpp + ZYOppAvg;
                                ZOpp = ZOpp - ZYOppAvg;
                            }

                            //Y Alpha Rotation
                            if (ZOpp > XOpp)
                            {
                                double XZOppAvg = (ZOpp - XOpp) / 2;
                                B = B + (XZOppAvg * alphaRotationPercentageY);
                                ZOpp = ZOpp - XZOppAvg;
                                XOpp = XOpp + XZOppAvg;
                            }
                            else if (ZOpp < XOpp)
                            {
                                double XZOppAvg = (XOpp - ZOpp) / 2;

                                B = B - (XZOppAvg * alphaRotationPercentageY);
                                ZOpp = ZOpp + XZOppAvg;
                                XOpp = XOpp - XZOppAvg;
                            }
                            //Z Alpha Rotation
                            if (XOpp > YOpp)
                            {
                                double YXOppAvg = (XOpp - YOpp) / 2;
                                C = C + (YXOppAvg * alphaRotationPercentageZ);
                                XOpp = XOpp - YXOppAvg;
                                YOpp = YOpp + YXOppAvg;
                            }
                            else if (XOpp < YOpp)
                            {
                                double YXOppAvg = (YOpp - XOpp) / 2;

                                C = C - (YXOppAvg * alphaRotationPercentageZ);
                                XOpp = XOpp + YXOppAvg;
                                YOpp = YOpp - YXOppAvg;
                            }

                            //determine if value is close enough
                            double hTow = Math.Max(Math.Max(XOpp, YOpp), ZOpp);
                            double lTow = Math.Min(Math.Min(XOpp, YOpp), ZOpp);
                            double towDiff = hTow - lTow;

                            if (towDiff < accuracy && towDiff > -accuracy)
                            {
                                k = 100;
                            }
                            else
                            {
                                k++;
                            }
                        }

                        //log
                        LogConsole("ABC:" + A + " " + B + " " + C + "\n");
                    }

                    ////////////////////////////////////////////////////////////////////////////////
                    //Steps per Millimeter Calibration******************************************************

                    //opp = 0.21; //4/5
                    //tower = 0.27; //9/32

                    double diagChange = 1 / deltaOpp;
                    double towOppDiff = deltaTower / deltaOpp; //0.5
                    double XYZ = (X + Y + Z) / 3;
                    double XYZOpp = (XOpp + YOpp + ZOpp) / 3;

                    LogConsole(X.ToString() + " " + XOpp.ToString() + " " + Y.ToString() + " " + YOpp.ToString() + " " + Z.ToString() + " " + ZOpp.ToString());

                    if (Math.Abs(XYZOpp - XYZ) > accuracy*2)
                    {
                        int i = 0;
                        while (i < 100)
                        {
                            XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                            stepsPerMM = stepsPerMM + (XYZOpp * diagChange);

                            X = X - towOppDiff * XYZOpp;
                            Y = Y - towOppDiff * XYZOpp;
                            Z = Z - towOppDiff * XYZOpp;
                            XOpp = XOpp - XYZOpp;
                            YOpp = YOpp - XYZOpp;
                            ZOpp = ZOpp - XYZOpp;

                            XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                            XYZOpp = checkZero(XYZOpp);

                            //HRAD recalibration
                            XYZ = (X + Y + Z) / 3;
                            HRad = HRad + ((XYZOpp * diagChange) / HRadRatio);

                            if (XYZOpp >= 0)
                            {
                                X = X - XYZ;
                                Y = Y - XYZ;
                                Z = Z - XYZ;
                                XOpp = XOpp - XYZ;
                                YOpp = YOpp - XYZ;
                                ZOpp = ZOpp - XYZ;
                            }
                            else
                            {
                                X = X + XYZ;
                                Y = Y + XYZ;
                                Z = Z + XYZ;
                                XOpp = XOpp + XYZ;
                                YOpp = YOpp + XYZ;
                                ZOpp = ZOpp + XYZ;
                            }

                            X = checkZero(X);
                            Y = checkZero(Y);
                            Z = checkZero(Z);
                            XOpp = checkZero(XOpp);
                            YOpp = checkZero(YOpp);
                            ZOpp = checkZero(ZOpp);

                            //XYZ is zero
                            if (XYZOpp < accuracy && XYZOpp > -accuracy && XYZ < accuracy && XYZ > -accuracy)
                            {
                                //end calculation
                                stepsPerMM = checkZero(stepsPerMM);
                                
                                double changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / tempSPM;

                                LogConsole("zMaxLength changed by: " + changeInMM);
                                LogConsole("zMaxLength before: " + centerHeight);
                                LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                                double tempChange = centerHeight - changeInMM;

                                _serialPort.WriteLine("M206 T3 P153 X" + tempChange.ToString());
                                LogConsole("Resetting Z Max Length\n");
                                Thread.Sleep(pauseTimeSet);

                                _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                                LogConsole("Steps Per Millimeter Changed: " + stepsPerMM.ToString());
                                Thread.Sleep(pauseTimeSet);

                                i = 100;
                            }
                            else
                            {
                                i++;
                            }
                        }
                        
                        if (HRad < 250 && HRad > 50)
                        {
                            //send obtained values back to the printer*************************************
                            LogConsole("HRad Recalibration:" + HRad + "\n");
                            _serialPort.WriteLine("M206 T3 P885 X" + checkZero(HRad).ToString());
                            LogConsole("Setting Horizontal Radius\n");
                            Thread.Sleep(pauseTimeSet);
                        }
                        else
                        {
                            LogConsole("Horizontal radius not set\n");
                        }
                    }

                    //send obtained values back to the printer*************************************
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T3 P901 X" + checkZero(A).ToString());
                    LogConsole("Setting A Rotation\n");
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T3 P905 X" + checkZero(B).ToString());
                    LogConsole("Setting B Rotation\n");
                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T3 P909 X" + checkZero(C).ToString());
                    LogConsole("Setting C Rotation\n");
                    Thread.Sleep(pauseTimeSet);

                    //rechecks calibration to either restart or finish
                    LogConsole("Checking height-map\n");
                    initiateCal();
                }
                else
                {
                    //advanced calibration
                    if (calculationCount == 0)
                    {
                        //////////////////////////////////////////////////////////////////////////////
                        //HRad is calibrated by increasing the outside edge of the glass by the average differences, this should balance the values with an average of zero
                        if (calculationCheckCount == 0)
                        {
                            calculationX = X;
                            calculationXOpp = XOpp;
                            calculationY = Y;
                            calculationYOpp = YOpp;
                            calculationZ = Z;
                            calculationZOpp = ZOpp;

                            calculationTemp1 = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);
                            HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);
                            calculationCheckCount++;
                        }
                        else
                        {
                            HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);

                            if (HRadCorrection != 0)
                            {
                                HRadCorrection = (HRadCorrection + (calculationTemp1 / (calculationTemp1 - HRadSA))) / 2;
                            }
                            else
                            {
                                HRadCorrection = calculationTemp1 / (calculationTemp1 - HRadSA);
                            }
                        }//end else

                        //check accuracy, if good, move to next step
                        if (HRadSA < accuracy2 && HRadSA > -accuracy2)
                        {
                            LogConsole("HRad:" + HRad + "\n");
                            LogConsole("HRad Correction:" + HRadCorrection + "\n");
                            LogConsole("HRad Average before calibration:" + calculationTemp1 + "\n");
                            LogConsole("HRad Average:" + HRadSA + "\n");
                            LogConsole("Horizontal Radius Calibration Success; Checking height-map.\n");
                            calculationCount++;
                            initiateCal();
                            calculationCheckCount = 0;
                            calculationTemp1 = 0;
                        }
                        else
                        {
                            if (HRadCorrection == 0)
                            {
                                HRad = HRad + ((HRadSA) / HRadRatio);
                            }
                            else
                            {
                                HRad = HRad + ((HRadSA * HRadCorrection) / HRadRatio);
                            }

                            LogConsole("HRad Average before calibration:" + calculationTemp1 + "\n");

                            //remembers previous offset
                            calculationTemp1 = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);

                            X = X - HRadSA;
                            Y = Y - HRadSA;
                            Z = Z - HRadSA;
                            XOpp = XOpp - HRadSA;
                            YOpp = YOpp - HRadSA;
                            ZOpp = ZOpp - HRadSA;

                            X = checkZero(X);
                            Y = checkZero(Y);
                            Z = checkZero(Z);
                            XOpp = checkZero(XOpp);
                            YOpp = checkZero(YOpp);
                            ZOpp = checkZero(ZOpp);

                            LogConsole("HRad:" + HRad + "\n");
                            LogConsole("HRad Correction:" + HRadCorrection + "\n");
                            LogConsole("HRad Average:" + HRadSA + "\n");

                            _serialPort.WriteLine("M206 T3 P885 X" + checkZero(HRad).ToString());
                            LogConsole("Setting Horizontal Radius\n");
                            Thread.Sleep(pauseTimeSet);

                            LogConsole("Checking height-map\n");
                            initiateCal();
                        }// end else
                    }// end horizontal radius calibration
                    else if (calculationCount == 1)
                    {
                        ////////////////////////////////////////////////////////////////////////////////
                        //Delta Radius Calibration******************************************************
                        double DASA = ((X + XOpp) / 2);
                        double DBSA = ((Y + YOpp) / 2);
                        double DCSA = ((Z + ZOpp) / 2);


                        if (DASA < accuracy2 && DASA > -accuracy2 && DBSA < accuracy2 && DBSA > -accuracy2 && DCSA < accuracy2 && DCSA > -accuracy2)
                        {
                            LogConsole("Delta Radius Calibration Success; Checking Height-Map");
                            calculationCount++;
                            initiateCal();
                        }
                        else
                        {
                            DA = DA + ((DASA) / HRadRatio);
                            DB = DB + ((DBSA) / HRadRatio);
                            DC = DC + ((DCSA) / HRadRatio);

                            LogConsole("Delta Radii Offsets: " + DA.ToString() + ", " + DB.ToString() + ", " + DC.ToString());

                            _serialPort.WriteLine("M206 T3 P913 X" + ToLongString(DA));
                            Thread.Sleep(pauseTimeSet);
                            _serialPort.WriteLine("M206 T3 P917 X" + ToLongString(DB));
                            Thread.Sleep(pauseTimeSet);
                            _serialPort.WriteLine("M206 T3 P921 X" + ToLongString(DC));
                            Thread.Sleep(pauseTimeSet);

                            LogConsole("Checking height-map");
                            initiateCal();
                        }
                    }
                    else if (calculationCount == 2)
                    {
                        if (analyzeCount == 0)
                        {
                            analyzeCount++;
                            analyzeGeometry();

                            LogConsole("Tower Rotation calculated, check XY Panel\n");
                        }

                        ////////////////////////////////////////////////////////////////////////////////
                        //Tower Offset Calibration******************************************************
                        double hTow2 = Math.Max(Math.Max(X, Y), Z);
                        double lTow2 = Math.Min(Math.Min(X, Y), Z);
                        double towDiff2 = hTow2 - lTow2;

                        XYZAvg = (X + Y + Z) / 3;

                        if (towDiff2 < 0.1 && towDiff2 > -0.1)
                        {
                            LogConsole("XYZ Offset Correction: " + calculationTemp1);
                            LogConsole("XYZ Offset Average Before Calibration: " + calculationXYZAvg);
                            LogConsole("XYZ Offset Average Afer Calibration: " + XYZAvg);
                            LogConsole("XYZ Offset Calibration Success; checking height-map\n");

                            calculationCount++;
                            calculationCheckCount = 0;
                            initiateCal();
                        }
                        else
                        {
                            //balance axes - retrieve data
                            if (calculationCheckCount == 0)
                            {
                                calculationXYZAvg = (X + Y + Z) / 3;
                                offsetXYZ = 1 / 0.7;

                                LogConsole("XYZ Offset Correction: " + calculationTemp1);
                                LogConsole("XYZ Offset Average Before Calibration: " + calculationXYZAvg);
                                LogConsole("XYZ Offset Average Afer Calibration: " + XYZAvg);
                                calculationCheckCount++;
                            }
                            else
                            {
                                calculationTemp1 = calculationXYZAvg / (calculationXYZAvg - XYZAvg);
                                offsetXYZ = (1 / 0.7) * calculationTemp1;

                                LogConsole("XYZ Offset Correction: " + calculationTemp1);
                                LogConsole("XYZ Offset: " + offsetXYZ);
                                LogConsole("XYZ Offset Average Before Calibration: " + calculationXYZAvg);
                                LogConsole("XYZ Offset Average Afer Calibration: " + XYZAvg);
                            }

                            double theoryX = offsetX + X * stepsPerMM * offsetXCorrection;
                            double theoryY = offsetY + Y * stepsPerMM * offsetYCorrection;
                            double theoryZ = offsetZ + Z * stepsPerMM * offsetZCorrection;

                            //correction of one tower allows for XY dimensional accuracy
                            if (X > 0)
                            {
                                //if x is positive
                                offsetX = offsetX + X * stepsPerMM * offsetXCorrection;
                            }
                            else if (theoryX > 0 && X < 0)
                            {
                                //if x is negative and can be decreased
                                offsetX = offsetX + X * stepsPerMM * offsetXCorrection;
                            }
                            else
                            {
                                //if X is negative
                                offsetY = offsetY - X * stepsPerMM * offsetYCorrection * 2;
                                offsetZ = offsetZ - X * stepsPerMM * offsetZCorrection * 2;
                            }

                            //Y
                            if (Y > 0)
                            {
                                offsetY = offsetY + Y * stepsPerMM * offsetYCorrection;
                            }
                            else if (theoryY > 0 && Y < 0)
                            {
                                offsetY = offsetY + Y * stepsPerMM * offsetYCorrection;
                            }
                            else
                            {
                                offsetX = offsetX - Y * stepsPerMM * offsetXCorrection * 2;
                                offsetZ = offsetZ - Y * stepsPerMM * offsetZCorrection * 2;
                            }

                            //Z
                            if (Z > 0)
                            {
                                offsetZ = offsetZ + Z * stepsPerMM * offsetZCorrection;
                            }
                            else if (theoryZ > 0 && Z < 0)
                            {
                                offsetZ = offsetZ + Z * stepsPerMM * offsetZCorrection;
                            }
                            else
                            {
                                offsetY = offsetY - Z * stepsPerMM * offsetYCorrection * 2;
                                offsetX = offsetX - Z * stepsPerMM * offsetXCorrection * 2;
                            }

                            //send data back to printer

                            offsetX = Math.Round(offsetX);
                            offsetY = Math.Round(offsetY);
                            offsetZ = Math.Round(offsetZ);

                            LogConsole("XYZ:" + ToLongString(offsetX) + " " + ToLongString(offsetY) + " " + ToLongString(offsetZ) + "\n");

                            _serialPort.WriteLine("M206 T1 P893 S" + ToLongString(offsetX));
                            Thread.Sleep(pauseTimeSet);
                            _serialPort.WriteLine("M206 T1 P895 S" + ToLongString(offsetY));
                            Thread.Sleep(pauseTimeSet);
                            _serialPort.WriteLine("M206 T1 P897 S" + ToLongString(offsetZ));
                            Thread.Sleep(pauseTimeSet);

                            LogConsole("Checking height-map\n");
                            initiateCal();
                        }//end else 
                    }//end else if calculation count 1
                    else if (calculationCount == 3)//2
                    {
                        ////////////////////////////////////////////////////////////////////////////////
                        //Alpha Rotation Calibration****************************************************
                        double hTow1 = Math.Max(Math.Max(XOpp, YOpp), ZOpp);
                        double lTow1 = Math.Min(Math.Min(XOpp, YOpp), ZOpp);
                        double towDiff1 = hTow1 - lTow1;

                        if (towDiff1 < accuracy2 && towDiff1 > -accuracy2)
                        {
                            LogConsole("Alpha Rotation Calibration Success; checking height-map\n");

                            calculationCount++;
                            initiateCal();
                        }
                        else
                        {
                            int k = 0;

                            while (k < 100)
                            {
                                //X Alpha Rotation
                                if (YOpp > ZOpp)
                                {
                                    double ZYOppAvg = (YOpp - ZOpp) / 2;
                                    A = A + (ZYOppAvg * alphaRotationPercentageX);
                                    YOpp = YOpp - ZYOppAvg;
                                    ZOpp = ZOpp + ZYOppAvg;
                                }
                                else if (YOpp < ZOpp)
                                {
                                    double ZYOppAvg = (ZOpp - YOpp) / 2;

                                    A = A - (ZYOppAvg * alphaRotationPercentageX);
                                    YOpp = YOpp + ZYOppAvg;
                                    ZOpp = ZOpp - ZYOppAvg;
                                }

                                //Y Alpha Rotation
                                if (ZOpp > XOpp)
                                {
                                    double XZOppAvg = (ZOpp - XOpp) / 2;
                                    B = B + (XZOppAvg * alphaRotationPercentageY);
                                    ZOpp = ZOpp - XZOppAvg;
                                    XOpp = XOpp + XZOppAvg;
                                }
                                else if (ZOpp < XOpp)
                                {
                                    double XZOppAvg = (XOpp - ZOpp) / 2;

                                    B = B - (XZOppAvg * alphaRotationPercentageY);
                                    ZOpp = ZOpp + XZOppAvg;
                                    XOpp = XOpp - XZOppAvg;
                                }

                                //Z Alpha Rotation
                                if (XOpp > YOpp)
                                {
                                    double YXOppAvg = (XOpp - YOpp) / 2;
                                    C = C + (YXOppAvg * alphaRotationPercentageZ);
                                    XOpp = XOpp - YXOppAvg;
                                    YOpp = YOpp + YXOppAvg;
                                }
                                else if (XOpp < YOpp)
                                {
                                    double YXOppAvg = (YOpp - XOpp) / 2;

                                    C = C - (YXOppAvg * alphaRotationPercentageZ);
                                    XOpp = XOpp + YXOppAvg;
                                    YOpp = YOpp - YXOppAvg;
                                }

                                //determine if value is close enough
                                double hTow = Math.Max(Math.Max(XOpp, YOpp), ZOpp);
                                double lTow = Math.Min(Math.Min(XOpp, YOpp), ZOpp);
                                double towDiff = hTow - lTow;

                                if (towDiff < accuracy && towDiff > -accuracy)
                                {
                                    k = 100;
                                }
                                else
                                {
                                    k++;
                                }
                            }

                            LogConsole("ABC:" + A + " " + B + " " + C + "\n");
                            LogConsole("Heights: X:" + X + ", XOpp:" + XOpp + ", Y:" + Y + ", YOpp:" + YOpp + ", Z:" + Z + ", and ZOpp:" + ZOpp + "\n");

                            _serialPort.WriteLine("M206 T3 P901 X" + checkZero(A).ToString());
                            LogConsole("Setting A Rotation\n");
                            Thread.Sleep(pauseTimeSet);
                            _serialPort.WriteLine("M206 T3 P905 X" + checkZero(B).ToString());
                            LogConsole("Setting B Rotation\n");
                            Thread.Sleep(pauseTimeSet);
                            _serialPort.WriteLine("M206 T3 P909 X" + checkZero(C).ToString());
                            LogConsole("Setting C Rotation\n");
                            Thread.Sleep(pauseTimeSet);

                            LogConsole("Checking height-map\n");
                            initiateCal();
                        }//end else
                    }//end alpha rotation calibration
                    else if (calculationCount == 4)//3
                    {
                        ////////////////////////////////////////////////////////////////////////////////
                        //Diagonal Rod Calibration******************************************************
                        double XYZ2 = (X + Y + Z) / 3;
                        double XYZOpp2 = (XOpp + YOpp + ZOpp) / 3;
                        double hTow2 = Math.Max(Math.Max(X, Y), Z);
                        double lTow2 = Math.Min(Math.Min(X, Y), Z);
                        double towDiff2 = hTow2 - lTow2;

                        XYZ2 = checkZero(XYZ2);
                        XYZOpp2 = checkZero(XYZOpp2);
                        XYZAvg = (X + Y + Z) / 3;

                        if (towDiff2 < 0.1 && towDiff2 > -0.1)
                        {
                            if (XYZOpp2 < accuracy && XYZOpp2 > -accuracy && XYZ2 < accuracy && XYZ2 > -accuracy || t >= 2)
                            {
                                LogConsole("Diagonal Rod Calibration Success; checking height-map\n");
                                LogConsole("Calibration Complete; Homing Printer");
                                _serialPort.WriteLine("G28");

                                calculationCount = 0;
                                advancedCalibration = 0;
                                advancedCalCount = 0;
                            }
                            else
                            {
                                double diagChange = 1 / deltaOpp;
                                double towOppDiff = deltaTower / deltaOpp; //0.5

                                int i = 0;
                                while (i < 100)
                                {
                                    double XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                                    diagonalRod = diagonalRod + (XYZOpp * diagChange);
                                    X = X - towOppDiff * XYZOpp;
                                    Y = Y - towOppDiff * XYZOpp;
                                    Z = Z - towOppDiff * XYZOpp;
                                    XOpp = XOpp - XYZOpp;
                                    YOpp = YOpp - XYZOpp;
                                    ZOpp = ZOpp - XYZOpp;
                                    XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                                    XYZOpp = checkZero(XYZOpp);

                                    double XYZ = (X + Y + Z) / 3;
                                    //hrad
                                    HRad = HRad + (XYZ / HRadRatio);

                                    if (XYZOpp >= 0)
                                    {
                                        X = X - XYZ;
                                        Y = Y - XYZ;
                                        Z = Z - XYZ;
                                        XOpp = XOpp - XYZ;
                                        YOpp = YOpp - XYZ;
                                        ZOpp = ZOpp - XYZ;
                                    }
                                    else
                                    {
                                        X = X + XYZ;
                                        Y = Y + XYZ;
                                        Z = Z + XYZ;
                                        XOpp = XOpp + XYZ;
                                        YOpp = YOpp + XYZ;
                                        ZOpp = ZOpp + XYZ;
                                    }

                                    X = checkZero(X);
                                    Y = checkZero(Y);
                                    Z = checkZero(Z);
                                    XOpp = checkZero(XOpp);
                                    YOpp = checkZero(YOpp);
                                    ZOpp = checkZero(ZOpp);

                                    //XYZ is zero
                                    if (XYZOpp < accuracy && XYZOpp > -accuracy && XYZ < accuracy && XYZ > -accuracy)
                                    {
                                        #region replace
                                        //end calculation
                                        diagonalRod = checkZero(diagonalRod);

                                        //add diagonal rod and steps per millimeter to list to use later for linear regression
                                        known_xDR.Add(diagonalRod);
                                        known_yDR.Add(stepsPerMM);

                                        //prevent using linear regression if there are not two values store
                                        if (stepsCalcNumber >= 2)
                                        {
                                            if (stepsCalcNumber >= 3)
                                            {
                                                known_xDR.RemoveAt(l);
                                                known_yDR.RemoveAt(l);
                                                l++;
                                            }

                                            double rsquared = 0;
                                            double yintercept = 0;
                                            double slope = 0;

                                            LinearRegression(known_xDR.ToArray(), known_yDR.ToArray(), 0, known_yDR.ToArray().Length, out rsquared, out yintercept, out slope);
                                            double stepsPerMM = slope * diagonalRodLength + yintercept;

                                            Thread.Sleep(pauseTimeSet);
                                            _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                                            LogConsole("Steps Per Millimeter Changed: " + stepsPerMM.ToString());

                                            LogConsole("SPM yintercept: " + yintercept);
                                            LogConsole("SPM slope: " + slope);

                                            double changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / stepsPerMM;

                                            LogConsole("zMaxLength changed by: " + changeInMM);
                                            LogConsole("zMaxLength before: " + centerHeight);
                                            LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                                            _serialPort.WriteLine("M206 T3 P153 X" + (centerHeight - changeInMM));
                                            LogConsole("Resetting Z Max Length\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }
                                        else if (stepsCalcNumber == 0)
                                        {
                                            //add one to steps calnumber
                                            stepsCalcNumber++;

                                            //adds a point to the array below the stepsPerMM
                                            stepsPerMM = tempSPM - (1 / tempSPM) * 160;
                                            LogConsole("Steps Per Millimeter Decreased: " + stepsPerMM.ToString());

                                            Thread.Sleep(pauseTimeSet);
                                            _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                                            LogConsole("Setting steps per millimeter\n");

                                            double changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / stepsPerMM;

                                            LogConsole("zMaxLength changed by: " + changeInMM);
                                            LogConsole("zMaxLength before: " + centerHeight);
                                            LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                                            _serialPort.WriteLine("M206 T3 P153 X" + (centerHeight - changeInMM));
                                            LogConsole("Resetting Z Max Length\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }
                                        else if (stepsCalcNumber == 1)
                                        {
                                            //add one to steps calnumber
                                            stepsCalcNumber++;

                                            //adds a point to the array above the stepsPerMM
                                            stepsPerMM = tempSPM + (1 / tempSPM) * 160;//*2 to compensate for the subtraction
                                            LogConsole("Steps Per Millimeter Increased: " + stepsPerMM.ToString());

                                            Thread.Sleep(pauseTimeSet);
                                            _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                                            LogConsole("Setting steps per millimeter\n");

                                            double changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / stepsPerMM;

                                            LogConsole("zMaxLength changed by: " + changeInMM);
                                            LogConsole("zMaxLength before: " + centerHeight);
                                            LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                                            _serialPort.WriteLine("M206 T3 P153 X" + (centerHeight - changeInMM));
                                            LogConsole("Resetting Z Max Length\n");
                                            Thread.Sleep(pauseTimeSet);
                                        }

                                        #endregion
                                        i = 100;
                                    }
                                    else
                                    {
                                        i++;
                                    }
                                }

                                LogConsole("Diagonal Rod:" + diagonalRod + "\n");
                                LogConsole("Heights: X:" + X + ", XOpp:" + XOpp + ", Y:" + Y + ", YOpp:" + YOpp + ", Z:" + Z + ", and ZOpp:" + ZOpp + "\n");

                                //send obtained values back to the printer*************************************
                                //Thread.Sleep(5000);
                                _serialPort.WriteLine("M206 T3 P881 X" + diagonalRod.ToString());
                                LogConsole("Setting diagonal rod\n");
                                Thread.Sleep(pauseTimeSet);
                                _serialPort.WriteLine("M206 T3 P885 X" + checkZero(HRad).ToString());
                                LogConsole("Setting Horizontal Radius\n");
                                Thread.Sleep(pauseTimeSet);

                                //rechecks calibration to either restart or finish
                                LogConsole("Checking height-map\n");
                                t++;
                                initiateCal();
                            }//end accuracy check
                        }//end XYZ offset check
                        else
                        {
                            LogConsole("Recalculating previous steps due to change in steps per millimeter.");//due to steps per millimeter fix
                            calculationCount = 0;
                            initiateCal();
                        }
                    }// end diagonal rod calibration
                }// end else
            }// end advanced calibration
        }


        public void LinearRegression(double[] xVals, double[] yVals, int inclusiveStart, int exclusiveEnd, out double rsquared, out double yintercept, out double slope)
        {
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double ssX = 0;
            double ssY = 0;
            double sumCodeviates = 0;
            double sCo = 0;
            double count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                double x = xVals[ctr];
                double y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            double RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            double RDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            double meanX = sumOfX / count;
            double meanY = sumOfY / count;
            double dblR = RNumerator / Math.Sqrt(RDenom);
            rsquared = dblR * dblR;
            yintercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }

        //used in previous delta radii calibration
        private double[] linearRegression(double[] y, double[] x)
        {
            double[] lr = new double[4];
            int n = y.Length;
            double sum_x = 0;
            double sum_y = 0;
            double sum_xy = 0;
            double sum_xx = 0;
            double sum_yy = 0;

            for (var i = 0; i < y.Length; i++)
            {
                sum_x += x[i];
                sum_y += y[i];
                sum_xy += (x[i] * y[i]);
                sum_xx += (x[i] * x[i]);
                sum_yy += (y[i] * y[i]);
            }

            lr[1] = (n * sum_xy - sum_x * sum_y) / (n * sum_xx - sum_x * sum_x);//slope
            lr[2] = (sum_y - lr[1] * sum_x) / n;//intercept
            lr[3] = Math.Pow((n * sum_xy - sum_x * sum_y) / Math.Sqrt((n * sum_xx - sum_x * sum_x) * (n * sum_yy - sum_y * sum_y)), 2);//r2

            return lr;
        }

        //check if values are close to zero, then set them to zero - avoids errors
        private double checkZero(double value)
        {
            if (value > 0 && value < accuracy)
            {
                return 0;
            }
            else if (value < 0 && value > -accuracy)
            {
                return 0;
            }
            else
            {
                return value;
            }
        }

        //uses long string instead of scientific notation
        private static string ToLongString(double input)
        {
            string str = input.ToString().ToUpper();

            // if string representation was collapsed from scientific notation, just return it:
            if (!str.Contains("E")) return str;

            bool negativeNumber = false;

            if (str[0] == '-')
            {
                str = str.Remove(0, 1);
                negativeNumber = true;
            }

            string sep = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            char decSeparator = sep.ToCharArray()[0];

            string[] exponentParts = str.Split('E');
            string[] decimalParts = exponentParts[0].Split(decSeparator);

            // fix missing decimal point:
            if (decimalParts.Length == 1) decimalParts = new string[] { exponentParts[0], "0" };

            int exponentValue = int.Parse(exponentParts[1]);

            string newNumber = decimalParts[0] + decimalParts[1];

            string result;

            if (exponentValue > 0)
            {
                result =
                    newNumber +
                    GetZeros(exponentValue - decimalParts[1].Length);
            }
            else // negative exponent
            {
                result =
                    "0" +
                    decSeparator +
                    GetZeros(exponentValue + decimalParts[0].Length) +
                    newNumber;

                result = result.TrimEnd('0');
            }

            if (negativeNumber)
                result = "-" + result;

            return result;
        }

        //keeps input from being in scientific notation, and if the value is near zero then it will be set to zero
        private static string GetZeros(int zeroCount)
        {
            if (zeroCount < 0)
            {
                zeroCount = Math.Abs(zeroCount);
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < zeroCount; i++)
            {
                sb.Append("0");
            }

            return sb.ToString();
        }

        //analyzes the geometry/accuracies of the printers frame
        private void analyzeGeometry()
        {
            //calculates the tower angle at the top and bottom
            towerXRotation = Math.Acos((plateDiameter * 0.963) / Math.Sqrt(Math.Pow(Math.Abs(X - XOpp), 2) + Math.Pow((plateDiameter * 0.963), 2))) * 57.296 * 5;
            towerYRotation = Math.Acos((plateDiameter * 0.963) / Math.Sqrt(Math.Pow(Math.Abs(Y - YOpp), 2) + Math.Pow((plateDiameter * 0.963), 2))) * 57.296 * 5;
            towerZRotation = Math.Acos((plateDiameter * 0.963) / Math.Sqrt(Math.Pow(Math.Abs(Z - ZOpp), 2) + Math.Pow((plateDiameter * 0.963), 2))) * 57.296 * 5;

            /*
            if (X < XOpp)
            {
                towerXRotation = 90 - towerXRotation;
            }
            else
            {
                towerXRotation = 90 + towerXRotation;
            }

            if (Y < YOpp)
            {
                towerYRotation = 90 - towerYRotation;
            }
            else
            {
                towerYRotation = 90 + towerYRotation;
            }

            if (Z < ZOpp)
            {
                towerZRotation = 90 - towerZRotation;
            }
            else
            {
                towerZRotation = 90 + towerZRotation;
            }

            //bottom
            lblXAngleTower.Text = towerXRotation.ToString();
            lblYAngleTower.Text = towerYRotation.ToString();
            lblZAngleTower.Text = towerZRotation.ToString();

            //top
            lblXAngleTop.Text = (180 - towerXRotation).ToString();
            lblYAngleTop.Text = (180 - towerYRotation).ToString();
            lblZAngleTop.Text = (180 - towerZRotation).ToString();

            //Calculates the radii for each tower at the top and bottom of the towers
            //X
            double hypotenuseX = (Math.Sin(90) / Math.Sin(Math.PI - towerXRotation - (180 - towerXRotation))) * centerHeight;
            double radiusSideX = Math.Sqrt(Math.Pow(hypotenuseX, 2) - Math.Pow(centerHeight, 2));
            double bottomX = HRad;
            double topX = HRad - radiusSideX;

            //Top
            lblXPlate.Text = bottomX.ToString();
            lblXPlateTop.Text = topX.ToString();

            //Y
            double hypotenuseY = (Math.Sin(90) / Math.Sin(Math.PI - towerYRotation - (180 - towerYRotation))) * centerHeight;
            double radiusSideY = Math.Sqrt(Math.Pow(hypotenuseX, 2) - Math.Pow(centerHeight, 2));
            double bottomY = HRad;
            double topY = HRad - radiusSideY;

            //Top
            lblYPlate.Text = bottomY.ToString();
            lblYPlateTop.Text = topY.ToString();

            //Z
            double hypotenuseZ = (Math.Sin(90) / Math.Sin(Math.PI - towerZRotation - (180 - towerZRotation))) * centerHeight;
            double radiusSideZ = Math.Sqrt(Math.Pow(hypotenuseX, 2) - Math.Pow(centerHeight, 2));
            double bottomZ = HRad;
            double topZ = HRad - radiusSideZ;

            //Top
            lblZPlate.Text = bottomZ.ToString();
            lblZPlateTop.Text = topZ.ToString();

            //find max offset in Xy scaling with current tower offsets
            double AScaling = Math.Max(Math.Max(Math.Abs(90 - hypotenuseX), Math.Abs(90 - hypotenuseY)), Math.Abs(90 - hypotenuseZ));
            double offsetScalingMax = (Math.Sin(90) / Math.Sin(Math.PI - 90 - AScaling)) * centerHeight;

            //set scaling offset
            lblScaleOffset.Text = offsetScalingMax.ToString();
            */
            }

        //field validators to check user input
            #region Field Validation checks.
            //
        private void fetchEEProm()
        {
            // If a .Parse() call fails, it will throw an exception.  If you use .TryParse(),
            // it will return false on a failure as well as populate the plateDiameter value with 
            // zero.  If it succeeds, it will return true and populate plateDiameter with the 
            // converted value.
            if (double.TryParse(textBuildDiameter.Text, out plateDiameter))
            {
                if (plateDiameter > 50)
                {
                    // Replace later
                    comboBoxZMinimumValue = comboZMin.SelectedItem.ToString();

                    // Read EEPROM
                    _serialPort.WriteLine("M205");
                    LogConsole("Request to read EEPROM sent\n");
                    _initiatingCalibration = true;
                }
                else
                {
                    LogConsole("The minimum plate diameter is 50mm.  Please re-enter.\n");
                }
            }
            else
            {
                LogConsole("Please enter your build plate diameter and try again\n");
            }
        }

        private bool ValidateDoubleField(string inValue, string fieldName)
        {
            double tempDbl = 0.0;
            if (!double.TryParse(inValue, out tempDbl))
            {
                LogConsole(String.Format("Please enter a valid value for {0}.\n", fieldName));
                return false;
            }
            else
                return true;
        }

        private bool ValidateIntField(string inValue, string fieldName)
        {
            int tempInt = 0;
            if (!int.TryParse(inValue, out tempInt))
            {
                LogConsole(String.Format("Please enter a valid value for {0}.\n", fieldName));
                return false;
            }
            else
                return true;
        }

        private bool ValidateDoubleField(TextBox textField, string fieldName)
        {
            double tempDbl = 0.0;
            string inValue = textField.Text;

            if (!double.TryParse(inValue, out tempDbl))
            {
                errorProvider.SetError(textField, String.Format("Please enter a valid value for {0}.\n", fieldName));
                //errorProvider.SetIconAlignment(textField, ErrorIconAlignment.TopRight);
                LogConsole(String.Format("Please enter a valid value for {0}.\n", fieldName));
                return false;
            }
            else
                errorProvider.Clear();
            return true;
        }

        private bool ValidateIntField(TextBox textField, string fieldName)
        {
            int tempInt = 0;
            string inValue = textField.Text;
            if (!int.TryParse(inValue, out tempInt))
            {
                errorProvider.SetError(textField, String.Format("Please enter a valid value for {0}.\n", fieldName));
                LogConsole(String.Format("Please enter a valid value for {0}.\n", fieldName));
                return false;
            }
            else
                errorProvider.Clear();
            return true;
        }

        private void cboBaudRate_Validating(object sender, CancelEventArgs e)
        {
            if (!cboBaudRate.Items.Contains(cboBaudRate.Text))
            {
                LogConsole("Invalid baud rate selected!\n");
                e.Cancel = false; // if this is true, the user can't leave the control.
            }
        }

        private void textAccuracy_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Calculation Accuracy");
            e.Cancel = false;
        }

        private void textAccuracy2_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Heightmap Accuracy");
            e.Cancel = false;
        }

        private void textHRadRatio_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Horizontal Radius Change");
            e.Cancel = false;
        }

        private void textFSRPlateOffset_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "FSR Plate Offset");
            e.Cancel = false;
        }

        private void textPauseTimeSet_Validating(object sender, CancelEventArgs e)
        {
            ValidateIntField((TextBox)sender, "Pause-Time COM");
            e.Cancel = false;
        }

        private void textProbingHeight_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z-Probe Start Height");
            e.Cancel = false;
        }

        private void textDeltaTower_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Tower Diag Rod");
            e.Cancel = false;
        }

        private void textDeltaOpp_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Opp Diag Rod");
            e.Cancel = false;
        }

        private void textMaxIterations_Validating(object sender, CancelEventArgs e)
        {
            ValidateIntField((TextBox)sender, "Max Iterations");
            e.Cancel = false;
        }

        private void textProbingSpeed_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Probing Speed");
            e.Cancel = false;
        }

        private void textZProbeHeight_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z-Probe Height");
            e.Cancel = false;
        }

        private void textxxPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "X(Main)");
            e.Cancel = false;
        }

        private void textxxOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "X Opposite");
            e.Cancel = false;
        }

        private void textxyPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Y");
            e.Cancel = false;
        }

        private void textxyOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Y Opposite");
            e.Cancel = false;
        }

        private void textxzPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z");
            e.Cancel = false;
        }

        private void textxzOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z Opposite");
            e.Cancel = false;
        }

        private void textyyPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Y(Main)");
            e.Cancel = false;
        }

        private void textyyOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Y Opposite");
            e.Cancel = false;
        }


        private void textyxOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "X Opposite");
            e.Cancel = false;
        }

        private void textyzPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z");
            e.Cancel = false;
        }

        private void textyzOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z Opposite");
            e.Cancel = false;
        }

        private void textzzPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z(Main)");
            e.Cancel = false;
        }

        private void textzzOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Z Opposite");
            e.Cancel = false;
        }

        private void textzxPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "X");
            e.Cancel = false;
        }

        private void textzyPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Y");
            e.Cancel = false;
        }

        private void textzyOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Y Opposite");
            e.Cancel = false;
        }

        private void textzxOppPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "X Opposite");
            e.Cancel = false;
        }

        private void textyxPerc_Validating(object sender, CancelEventArgs e)
        {
            ValidateDoubleField((TextBox)sender, "Y");
            e.Cancel = false;

        }

        private void comboZMin_Validating(object sender, CancelEventArgs e)
        {
            if (!comboZMin.Items.Contains(comboZMin.Text))
            {
                errorProvider.SetError(comboZMin, "Invalid Z-Minimum Type!");
                LogConsole("Invalid Z-Minimum Type!");
            }
            else
            {
                errorProvider.Clear();
            }
        }
        #endregion

        private void checkHeightsButton_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {

                checkHeightsOnly = true;
                setVariablesAll();
                _initiatingCalibration = true;

                fetchEEProm();
            }
            else
            {
                LogConsole("Not Connected\n");
            }
        }
    }
}

/*
if (offsetX != 0 && offsetY != 0 && offsetZ != 0)
{
    int i = 0;
    while (i < 100)
    {
        double XYZOpp = (XOpp + YOpp + ZOpp) / 3;
        diagonalRod = diagonalRod + (XYZOpp * diagChange);
        X = X - towOppDiff * XYZOpp;
        Y = Y - towOppDiff * XYZOpp;
        Z = Z - towOppDiff * XYZOpp;
        XOpp = XOpp - XYZOpp;
        YOpp = YOpp - XYZOpp;
        ZOpp = ZOpp - XYZOpp;
        XYZOpp = (XOpp + YOpp + ZOpp) / 3;
        XYZOpp = checkZero(XYZOpp);

        double XYZ = (X + Y + Z) / 3;
        //hrad
        HRad = HRad + (XYZ / HRadRatio);

        if (XYZOpp >= 0)
        {
            X = X - XYZ;
            Y = Y - XYZ;
            Z = Z - XYZ;
            XOpp = XOpp - XYZ;
            YOpp = YOpp - XYZ;
            ZOpp = ZOpp - XYZ;
        }
        else
        {
            X = X + XYZ;
            Y = Y + XYZ;
            Z = Z + XYZ;
            XOpp = XOpp + XYZ;
            YOpp = YOpp + XYZ;
            ZOpp = ZOpp + XYZ;
        }

        X = checkZero(X);
        Y = checkZero(Y);
        Z = checkZero(Z);
        XOpp = checkZero(XOpp);
        YOpp = checkZero(YOpp);
        ZOpp = checkZero(ZOpp);

        //XYZ is zero
        if (XYZOpp < accuracy && XYZOpp > -accuracy && XYZ < accuracy && XYZ > -accuracy)
        {
            //end calculation
            diagonalRod = checkZero(diagonalRod);

            //add diagonal rod and steps per millimeter to list to use later for linear regression
            known_xDR.Add(diagonalRod);
            known_yDR.Add(stepsPerMM);

            //add one to steps calnumber
            stepsCalcNumber++;

            //prevent using linear regression if there are not two values store
            if (stepsCalcNumber >= 3)
            {
                if (stepsCalcNumber >= 4)
                {
                    known_xDR.RemoveAt(l);
                    known_yDR.RemoveAt(l);
                    l++;
                }

                //CORRECT
                if (((known_xDR[l] - diagonalRodLength) - (diagonalRod - diagonalRodLength)) <= 0.5 && ((known_xDR[l] - diagonalRodLength) - (diagonalRod - diagonalRodLength)) >= -0.5) {
                    if (diagonalRod > diagonalRodLength)
                    {
                        stepsPerMM += 1;
                    }
                    else if (diagonalRod < diagonalRodLength) {
                        stepsPerMM -= 1;
                    }
                }
                else
                {
                    double rsquared = 0;
                    double yintercept = 0;
                    double slope = 0;

                    LinearRegression(known_xDR.ToArray(), known_yDR.ToArray(), 0, known_yDR.ToArray().Length, out rsquared, out yintercept, out slope);
                    double stepsPerMM = slope * diagonalRodLength + yintercept;

                    Thread.Sleep(pauseTimeSet);
                    _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                    LogConsole("Steps Per Millimeter Changed: " + stepsPerMM.ToString());

                    LogConsole("SPM yintercept: " + yintercept);
                    LogConsole("SPM slope: " + slope);
                }

                double changeInMM = (((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / stepsPerMM) - 5;

                LogConsole("zMaxLength changed by: " + changeInMM);
                LogConsole("zMaxLength before: " + centerHeight);
                LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                _serialPort.WriteLine("M206 T3 P153 X" + (centerHeight - changeInMM));
                LogConsole("Resetting Z Max Length\n");
                Thread.Sleep(pauseTimeSet);
            }
            else if (stepsCalcNumber == 1)
            {
                //adds a point to the array below the stepsPerMM
                stepsPerMM = tempSPM - (1 / tempSPM) * 160;
                LogConsole("Steps Per Millimeter Decreased: " + stepsPerMM.ToString());

                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                LogConsole("Setting steps per millimeter\n");

                double changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / stepsPerMM;

                LogConsole("zMaxLength changed by: " + changeInMM);
                LogConsole("zMaxLength before: " + centerHeight);
                LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                _serialPort.WriteLine("M206 T3 P153 X" + (centerHeight - changeInMM));
                LogConsole("Resetting Z Max Length\n");
                Thread.Sleep(pauseTimeSet);
            }
            else if (stepsCalcNumber == 2)
            {
                //adds a point to the array above the stepsPerMM
                stepsPerMM = tempSPM + (1 / tempSPM) * 160;//*2 to compensate for the subtraction
                LogConsole("Steps Per Millimeter Increased: " + stepsPerMM.ToString());

                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                LogConsole("Setting steps per millimeter\n");

                double changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / stepsPerMM;

                LogConsole("zMaxLength changed by: " + changeInMM);
                LogConsole("zMaxLength before: " + centerHeight);
                LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                _serialPort.WriteLine("M206 T3 P153 X" + (centerHeight - changeInMM));
                LogConsole("Resetting Z Max Length\n");
                Thread.Sleep(pauseTimeSet);
            }

            i = 100;
        }
        else
        {
            i++;
        }
    }

    if (diagonalRod < 1000 && diagonalRod > 1)
    {
        //send obtained values back to the printer*************************************
        LogConsole("Diagonal Rod:" + diagonalRod + "\n");
        Thread.Sleep(pauseTimeSet);
        _serialPort.WriteLine("M206 T3 P881 X" + diagonalRod.ToString());
        LogConsole("Setting diagonal rod\n");
    }
    else
    {
        LogConsole("Diagonal rod not set\n");
    }

    if (HRad < 250 && HRad > 50)
    {
        //send obtained values back to the printer*************************************
        LogConsole("HRad Recalibration:" + HRad + "\n");
        _serialPort.WriteLine("M206 T3 P885 X" + checkZero(HRad).ToString());
        LogConsole("Setting Horizontal Radius\n");
        Thread.Sleep(pauseTimeSet);
    }
    else
    {
        LogConsole("Horizontal radius not set\n");
    }
}
*/
