using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDACT
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            consoleMain.Text = "";
            consoleMain.ScrollBars = RichTextBoxScrollBars.Vertical;
            consolePrinter.Text = "";
            consolePrinter.ScrollBars = RichTextBoxScrollBars.Vertical;



            /*
            printerConsoleTextBox.Text = "";
            printerConsoleTextBox.ScrollBars = ScrollBars.Vertical;

            consoleTextBox.Text = "";
            consoleTextBox.ScrollBars = ScrollBars.Vertical;

            String[] zMinArray = { "FSR", "Z-Probe" };
            comboZMin.DataSource = zMinArray;

            // Build the combobox of available ports.
            string[] ports = _SerialPort.GetPortNames();

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
            */
        }
    }


    public class GraphAccuracy
    {
        //create graph of accuracy over iterations


    }

}
