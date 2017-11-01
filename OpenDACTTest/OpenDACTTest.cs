using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDACTTest
{
    public partial class OpenDACTTest : Form
    {
        public OpenDACTTest()
        {
            InitializeComponent();

            TestGCodeDisplay();
        }

        public void TestGCodeDisplay()
        {
            GCodeCommandsTest gCodeCommandsTest = new GCodeCommandsTest();

            bmpPictureTest.Image = gCodeCommandsTest.DisplayGCodePath();
        }
    }
}
