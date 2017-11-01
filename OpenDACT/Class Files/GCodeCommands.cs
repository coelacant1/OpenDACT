using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class GCodeCommands : Parameters
    {
        public List<string> CoordinatesYOppX { get; private set; }
        public List<string> CoordinatesXZOpp { get; private set; }
        public List<string> CoordinatesZOppY { get; private set; }
        public List<string> CoordinatesYXOpp { get; private set; }
        public List<string> CoordinatesXOppZ { get; private set; }
        public List<string> CoordinatesXY { get; private set; }
        public List<string> CoordinatesYZ { get; private set; }

        public string HomeAllAxes { get; private set; }
        public string SingleProbe { get; private set; }
        public string EmergencyReset { get; private set; }

        public GCodeCommands()
        {

        }

        public void CalculateCoordinates(Printer printer, Settings settings)
        {
            CalculateSixPointBedCoordinateTransitions(settings.PlateDiameter, settings.GCodeCalculationAccuracy);
            CalculateThreePointBedCoordinateTransitions(settings.PlateDiameter, settings.GCodeCalculationAccuracy);
        }

        private void CalculateSixPointBedCoordinateTransitions(double bedDiameter, double gCodeCalculationAccuracy)
        {
            CoordinatesYOppX = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(300, 240, gCodeCalculationAccuracy, bedDiameter / 2));
            CoordinatesXZOpp = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(240, 180, gCodeCalculationAccuracy, bedDiameter / 2));
            CoordinatesZOppY = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(180, 120, gCodeCalculationAccuracy, bedDiameter / 2));
            CoordinatesYXOpp = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(120, 60, gCodeCalculationAccuracy, bedDiameter / 2));
            CoordinatesXOppZ = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(60, 0, gCodeCalculationAccuracy, bedDiameter / 2));
        }
        private void CalculateThreePointBedCoordinateTransitions(double bedDiameter, double gCodeCalculationAccuracy)
        {
            CoordinatesXY = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(240, 120, gCodeCalculationAccuracy, bedDiameter / 2));
            CoordinatesYZ = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(120, 0, gCodeCalculationAccuracy, bedDiameter / 2));
        }

        private List<Tuple<double, double>> CalculateCoordinateRange(double startDegree, double stopDegree, double degreeAccuracy, double radius)
        {
            List<Tuple<double, double>> coordinates = new List<Tuple<double, double>>();
            
            for (double i = startDegree; i > stopDegree; i -= degreeAccuracy)
            {
                double xPosition = radius * Math.Sin(i * Math.PI / 180);
                double yPosition = radius * Math.Cos(i * Math.PI / 180) * -1;
                coordinates.Add(Tuple.Create(xPosition, yPosition));
            }

            return coordinates;
        }

        private List<string> ConvertBedCoordinatesToGCode(List<Tuple<double, double>> coordinates)
        {
            List<string> gCodeCoordinates = new List<string>();

            foreach (Tuple<double, double> coordinate in coordinates)
            {
                gCodeCoordinates.Add("G0 X" + coordinate.Item1 + " Y" + coordinate.Item2);
            }

            return gCodeCoordinates;
        }

        public override void SaveParameters(string location)
        {
            throw new NotImplementedException();
        }

        public override void LoadParameters(string location)
        {
            Dictionary<string, string> parameters = ReadCSVToString(location);

            parameters.TryGetValue("HomeAllAxes", out string HomeAllAxes);

            HomeAllAxes = ParseGCodeParameter(HomeAllAxes);// G28

            string test = "123456";
            string result = $"Param: {test}";

            throw new NotImplementedException();
        }

        public string ParseGCodeParameter(string parameter)
        {
            throw new NotImplementedException();
        }
        
        public string GenerateGCodeSaveCommands()
        {
            throw new NotImplementedException();
        }

        public string GenerateGCodeLoadCommands()
        {
            throw new NotImplementedException();
        }

        public override void ValidateParameters()
        {
            throw new NotImplementedException();
        }

        public class Builder : Builder<GCodeCommands> // welcome to scaffoldfest, where the code makes you cry
        {
            GCodeCommands gCodeCommands = new GCodeCommands();
            
            public string HomeAllAxes { set { gCodeCommands.HomeAllAxes = value; } }
            public string SingleProbe { set { gCodeCommands.SingleProbe = value; } }
            public string EmergencyReset { set { gCodeCommands.EmergencyReset = value; } }
            
            public override GCodeCommands Build()
            {
                return gCodeCommands;
            }
        }
    }
}
