using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class Settings : Parameters
    {
        public double CalculationAccuracy { get; private set; }
        public double HeightmapAccuracy { get; private set; }
        public double HorizontalRadiusChange { get; private set; }
        public double ProbingSpeed { get; private set; }
        public double ProbingHeight { get; private set; }
        public double StepsPerMMChange { get; private set; }
        public double TowerSPMChange { get; private set; }
        public double OppositeSPMChange { get; private set; }
        public double AlphaRotationChange { get; private set; }
        public double TowerOffsetCorrectionMain { get; private set; }
        public double TowerOffsetCorrectionMainOpposite { get; private set; }
        public double TowerOffsetCorrectionSecondary { get; private set; }
        public double TowerOffsetCorrectionSecondaryOpposite { get; private set; }
        public double XYTravelSpeed { get; private set; }
        public double PlateDiameter { get; private set; }
        public string Firmware { get; private set; }
        public string COMPort { get; private set; }
        public bool ScrollToBottomPrinterLog { get; private set; }
        public bool ScrollToBottomSoftwareLog { get; private set; }
        public int MaximumIterations { get; private set; }
        public double GCodeCalculationAccuracy { get; private set; }

        public override void SaveParameters(string location)
        {
            throw new NotImplementedException();
        }
        
        public override void LoadParameters(string location)
        {
            throw new NotImplementedException();
        }

        public override void ValidateParameters()
        {
            throw new NotImplementedException();
        }

        public class Builder : Builder<Settings> // welcome to scaffoldfest, where the code makes you cry
        {
            Settings settings = new Settings();

            public double CalculationAccuracy { set { settings.CalculationAccuracy = value; } }
            public double HeightmapAccuracy { set { settings.HeightmapAccuracy = value; } }
            public double HorizontalRadiusChange { set { settings.HorizontalRadiusChange = value; } }
            public double ProbingSpeed { set { settings.ProbingSpeed = value; } }
            public double ProbingHeight { set { settings.ProbingHeight = value; } }
            public double StepsPerMMChange { set { settings.StepsPerMMChange = value; } }
            public double TowerSPMChange { set { settings.TowerSPMChange = value; } }
            public double OppositeSPMChange { set { settings.OppositeSPMChange = value; } }
            public double AlphaRotationChange { set { settings.AlphaRotationChange = value; } }
            public double TowerOffsetCorrectionMain { set { settings.TowerOffsetCorrectionMain = value; } }
            public double TowerOffsetCorrectionMainOpposite { set { settings.TowerOffsetCorrectionMainOpposite = value; } }
            public double TowerOffsetCorrectionSecondary { set { settings.TowerOffsetCorrectionSecondary = value; } }
            public double TowerOffsetCorrectionSecondaryOpposite { set { settings.TowerOffsetCorrectionSecondaryOpposite = value; } }
            public double XYTravelSpeed { set { settings.XYTravelSpeed = value; } }
            public double PlateDiameter { set { settings.PlateDiameter = value; } }
            public string Firmware { set { settings.Firmware = value; } }
            public string COMPort { set { settings.COMPort = value; } }
            public bool ScrollToBottomPrinterLog { set { settings.ScrollToBottomPrinterLog = value; } }
            public bool ScrollToBottomSoftwareLog { set { settings.ScrollToBottomSoftwareLog = value; } }
            public int MaximumIterations { set { settings.MaximumIterations = value; } }
            public double GCodeCalculationAccuracy { set { settings.GCodeCalculationAccuracy = value; } }

            public override Settings Build()
            {
                return settings;
            }
        }
    }
}
