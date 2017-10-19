using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class Settings : IParameters
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
        public bool ScrollToBottomPrinterLog { get; set; }
        public bool ScrollToBottomSoftwareLog { get; set; }
        public int MaximumIterations { get; set; }

        public void SaveParameters(string location)
        {
            throw new NotImplementedException();
        }
        
        public void LoadParameters(string location)
        {
            throw new NotImplementedException();
        }
    }
}
