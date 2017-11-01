using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenDACT;
using OpenDACT.Class_Files;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace OpenDACTTest
{
    [TestClass]
    public class GCodeCommandsTest
    {
        [TestMethod]
        public Bitmap DisplayGCodePath()
        {
            Bitmap bmp = new Bitmap(500, 500);

            TestGCodeCoordinateGenerationSixPoint(ref bmp, 480);
            TestGCodeCoordinateGenerationThreePoint(ref bmp, 460);

            TestGCodeCoordinateGenerationSixPoint(ref bmp, 380);
            TestGCodeCoordinateGenerationThreePoint(ref bmp, 360);
            
            TestGCodeCoordinateGenerationSixPoint(ref bmp, 280);
            TestGCodeCoordinateGenerationThreePoint(ref bmp, 260);
            
            TestGCodeCoordinateGenerationSixPoint(ref bmp, 180);
            TestGCodeCoordinateGenerationThreePoint(ref bmp, 160);
            
            TestGCodeCoordinateGenerationSixPoint(ref bmp, 80);
            TestGCodeCoordinateGenerationThreePoint(ref bmp, 60);

            return bmp;
        }

        private void TestGCodeCoordinateGenerationSixPoint(ref Bitmap bmp, double bedDiameter)
        {
            GCodeCommands gCodeCommands = new GCodeCommands(bedDiameter, 0.25);
            
            PrintGCode(ref bmp, gCodeCommands.CoordinatesYOppX, Color.FromArgb(0, 0, 255));
            PrintGCode(ref bmp, gCodeCommands.CoordinatesXZOpp, Color.FromArgb(0, 255, 0));
            PrintGCode(ref bmp, gCodeCommands.CoordinatesZOppY, Color.FromArgb(255, 0, 0));
            PrintGCode(ref bmp, gCodeCommands.CoordinatesYXOpp, Color.FromArgb(255, 0, 255));
            PrintGCode(ref bmp, gCodeCommands.CoordinatesXOppZ, Color.FromArgb(255, 255, 0));
        }

        private void TestGCodeCoordinateGenerationThreePoint(ref Bitmap bmp, double bedDiameter)
        {
            GCodeCommands gCodeCommands = new GCodeCommands(bedDiameter, 0.25);

            PrintGCode(ref bmp, gCodeCommands.CoordinatesXY, Color.FromArgb(0, 0, 255));
            PrintGCode(ref bmp, gCodeCommands.CoordinatesYZ, Color.FromArgb(0, 255, 0));
        }

        private void PrintGCode(ref Bitmap bmp, List<string> coordinates, Color color)
        {
            foreach (string coordinate in coordinates)
            {
                Tuple<double, double> value = ParseGCode(coordinate);

                bmp.SetPixel(Convert.ToInt32(value.Item1) + 250, Convert.ToInt32(value.Item2) + 250, color);
            }
        }

        private Tuple<double, double> ParseGCode(string command)
        {
            string[] splitCommand = command.Split(' ');

            Double.TryParse(splitCommand[1].Split('X')[1], out double xPosition);
            Double.TryParse(splitCommand[2].Split('Y')[1], out double yPosition);

            return new Tuple<double, double>(xPosition, yPosition);
        }

    }
    
}
