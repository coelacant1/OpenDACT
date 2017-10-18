using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class Printer
    {
        public BedHeightMap bedHeightMap = new BedHeightMap();
        public Kinematics kinematics = new Kinematics();

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
            public CarriageOffset carriageOffset = new CarriageOffset();
            public DeltaRadius deltaRadius = new DeltaRadius();
            public AlphaRotation alphaRotation = new AlphaRotation();
            public double HorizontalRadius { get; set; }
            public double DiagonalRodLength { get; set; }
            public double StepsPerMM { get; set; }
            public double MaxZHeight { get; set; }
            public double ZProbeOffset { get; set; }
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
    }
}
