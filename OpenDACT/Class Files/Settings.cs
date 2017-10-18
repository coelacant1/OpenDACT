using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class Settings
    {
        public double CalculationAccuracy { get; set; }
        public double HeightmapAccuracy { get; set; }
        public double HorizontalRadiusChange { get; set; }
        public double ProbingSpeed { get; set; }
        public double ProbingHeight { get; set; }
        public double StepsPerMMChange { get; set; }
        public double TowerSPMChange { get; set; }
        public double OppositeSPMChange { get; set; }
        public double AlphaRotationChange { get; set; }
        public double TowerOffsetCorrectionMain { get; set; }
        public double TowerOffsetCorrectionMainOpposite { get; set; }
        public double TowerOffsetCorrectionSecondary { get; set; }
        public double TowerOffsetCorrectionSecondaryOpposite { get; set; }
        public double XYTravelSpeed { get; set; }
        public double PlateDiameter { get; set; }
        public string Firmware { get; set; }
        public string COMPort { get; set; }
        public int MaximumIterations { get; set; }


        public enum Baudrate
        {
            [Description("250000 Bits Per Second")]
            B250000 = 250000,
            [Description("115200 Bits Per Second")]
            B115200 = 115200,
            [Description("57600 Bits Per Second")]
            B57600 = 57600,
            [Description("38400 Bits Per Second")]
            B38400 = 38400,
            [Description("19200 Bits Per Second")]
            B19200 = 19200,
            [Description("9600 Bits Per Second")]
            B9600 = 9600
        };
    }
}
