using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class Printer : Parameters
    {
        public BedHeightMap bedHeightMap;
        public Kinematics kinematics;
        public GCodeCommands gCodeCommands;

        public Printer()
        {
            bedHeightMap = new BedHeightMap();
            kinematics = new Kinematics();
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
        
        public override void SaveParameters(string location)
        {
            throw new NotImplementedException();
        }

        public override void LoadParameters(string location)
        {
            Dictionary<string, string> parameters = ReadCSVToString(location);

            gCodeCommands.LoadParameters(location);

            throw new NotImplementedException();
        }

        public void LoadParametersPort(SerialPort serialPort)
        {
            //send command, read command

            throw new NotImplementedException();
        }

        public void SaveParametersPort(SerialPort serialPort)
        {
            //write each parameter and send save command
            throw new NotImplementedException();
        }

        public override void ValidateParameters()
        {
            throw new NotImplementedException();
        }
    }
}
