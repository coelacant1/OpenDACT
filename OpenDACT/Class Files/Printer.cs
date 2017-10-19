using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class Printer : IParameters
    {
        public BedHeightMap bedHeightMap;
        public Kinematics kinematics;
        public GCodeCommands gCodeCommands;

        public Printer(double bedDiameter)
        {
            bedHeightMap = new BedHeightMap();
            kinematics = new Kinematics();
            gCodeCommands = new GCodeCommands(bedDiameter);
        }

        public class BedHeightMap
        {
            public double XTower { get; set; }
            public double XOpposite { get; set; }
            public double YTower { get; set; }
            public double YOpposite { get; set; }
            public double ZTower { get; set; }
            public double ZOpposite { get; set; }
        };

        public class Kinematics
        {
            public CarriageOffset CarriageOffset { get; set; }
            public DeltaRadius DeltaRadius { get; set; }
            public AlphaRotation AlphaRotation { get; set; }
            public double HorizontalRadius { get; set; }
            public double DiagonalRodLength { get; set; }
            public double StepsPerMM { get; set; }
            public double MaxZHeight { get; set; }
            public double ZProbeOffset { get; set; }

            public Kinematics()
            {
                CarriageOffset = new CarriageOffset();
                DeltaRadius = new DeltaRadius();
                AlphaRotation = new AlphaRotation();
            }
        };

        public class CarriageOffset
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        };

        public class DeltaRadius
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        };

        public class AlphaRotation
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        };
        
        public void SaveParameters(string location)
        {
            throw new NotImplementedException();
        }

        public void LoadParameters(string location)
        {
            Tuple<List<string>, List<string>> parameters = ReadCSVToString(location);

            gCodeCommands.LoadGCodeMappings(test);

            throw new NotImplementedException();
        }

        public class GCodeCommands
        {
            public List<string> checkHeights;
            public List<string> CoordinatesYOppX { get; set; }
            public List<string> CoordinatesXZOpp { get; set; }
            public List<string> CoordinatesZOppY { get; set; }
            public List<string> CoordinatesYXOpp { get; set; }
            public List<string> CoordinatesXOppZ { get; set; }

            public GCodeCommands(double bedDiameter)
            {
                checkHeights = new List<string>(new string[]{
                    "",
                    "",
                    ""
                });

                string test = GCODE.HomeAllAxes;
                CalculateBedCoordinates(bedDiameter);
            }

            private void CalculateBedCoordinates(double bedDiameter)
            {
                CoordinatesYOppX = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(300, 240, 0.5, bedDiameter/2));
                CoordinatesXZOpp = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(240, 180, 0.5, bedDiameter/2));
                CoordinatesZOppY = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(180, 120, 0.5, bedDiameter/2));
                CoordinatesYXOpp = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(120, 60,  0.5, bedDiameter/2));
                CoordinatesXOppZ = ConvertBedCoordinatesToGCode(CalculateCoordinateRange(60,  0,   0.5, bedDiameter/2));
            }

            private List<Tuple<double, double>> CalculateCoordinateRange(double startDegree, double stopDegree, double degreeAccuracy, double radius)
            {
                List<Tuple<double, double>> coordinates = new List<Tuple<double, double>>();

                for (double i = 0; i < 300; i += degreeAccuracy)
                {
                    coordinates.Add(Tuple.Create(radius * Math.Cos(i) * 180 / Math.PI, radius * Math.Sin(i) * 180 / Math.PI));
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

            public void LoadGCodeMappings()
            {
                GCODE.HomeAllAxes;// G28
                GCODE.SingleProbe;// G30
                GCODE.EmergencyReset;// M112
                GCODE.SendToCenter;// G0 X0 Y0

                throw new NotImplementedException();

                //read in from csv set parameters of homing, setting

            }

            //check heights
            //read parameters
            //write parameters

            public static class GCODE
            {
                public static string HomeAllAxes { get; set; }
                public static string SingleProbe { get; set; }
                public static string EmergencyReset { get; set; }
                public static string SendToCenter { get; set; }
            };
        }
    }
}
