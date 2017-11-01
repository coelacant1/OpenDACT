using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDACTTest
{
    static class Program
    {
        public static OpenDACTTest openDACTTest;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            openDACTTest = new OpenDACTTest();
            Application.Run(openDACTTest);
        }
    }
}
