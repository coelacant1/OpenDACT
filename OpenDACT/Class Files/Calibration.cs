using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    static class Calibration
    {
        public static bool calibrateInProgress = false;
        public static bool calibrationState = false;
        public static int calibrationSelection = 0;
        public static int iterationNum = 0;
        private static float tempAccuracy;
        private static int iterativeStep = 0;

        private static bool HRadRequired = false;
        private static bool DRadRequired = false;
        private static bool towerOffsetsRequired = false;
        private static bool alphaRotationRequired = false;
        private static bool stepsPMMRequired = false;

        public static void calibrate()
        {
            if (calibrationSelection == 0)
            {
                basicCalibration();
            }
            else
            {
                iterativeCalibration();
            }

            iterationNum++;
        }

        public static void learnPrinter()
        {
            GCode.heuristicLearning();
        }

        public static void basicCalibration()
        {
            //check if eeprom object remains after this function is called for the second time

            if (iterationNum == 0)
            {
                if (UserVariables.diagonalRodLength == Convert.ToSingle(""))
                {
                    UserVariables.diagonalRodLength = EEPROM.diagonalRod;
                    UserInterface.logConsole("Using default diagonal rod length from EEPROM");
                }
            }

            tempAccuracy = (Math.Abs(Heights.X) + Math.Abs(Heights.XOpp) + Math.Abs(Heights.Y) + Math.Abs(Heights.YOpp) + Math.Abs(Heights.Z) + Math.Abs(Heights.ZOpp)) / 6;
            Program.mainFormTest.setAccuracyPoint(iterationNum, tempAccuracy);
            checkAccuracy(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);

            if (calibrationState == true)
            {
                towerOffsets(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                /*
                HRad(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                //DRad(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                alphaRotation(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                stepsPMM(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                */
            }
            else
            {
                //analyzeGeometry(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
            }
        }

        public static void iterativeCalibration()
        {
            /*
            if (iterationNum == 0)
            {
                if (UserVariables.diagonalRodLength == Convert.ToSingle(""))
                {
                    UserVariables.diagonalRodLength = EEPROM.diagonalRod;
                    UserInterface.logConsole("Using default diagonal rod length from EEPROM");
                }
            }

            tempAccuracy = (Math.Abs(Heights.X) + Math.Abs(Heights.XOpp) + Math.Abs(Heights.Y) + Math.Abs(Heights.YOpp) + Math.Abs(Heights.Z) + Math.Abs(Heights.ZOpp)) / 6;
            Program.mainFormTest.setAccuracyPoint(iterationNum, tempAccuracy);

            //check accuracy of hrad, then drad, then tOffs, then aRot, then SPM

            if (calibrationState == true)
            {
                switch (iterativeStep)
                {
                    case 0:
                        checkHRad();

                        if (HRadRequired) { HRad(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp); }
                        else { iterativeStep++; goto case 1; }
                        break;
                    case 1:
                        checkDRad();

                        if (DRadRequired) { DRad(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp); }
                        else { iterativeStep++; goto case 2; }
                        break;
                    case 2:
                        checkTOffsets();

                        if (towerOffsetsRequired) { towerOffsets(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp); }
                        else { iterativeStep++; goto case 3; }
                        break;
                    case 3:
                        checkARot();

                        if (alphaRotationRequired) { alphaRotation(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp); }
                        else { iterativeStep++; goto case 4; }
                        break;
                    case 4:
                        checkStepsPMM();

                        if (stepsPMMRequired) { stepsPMM(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp); }
                        else { iterativeStep++; }
                        break;
                }
            }
            else
            {
                //analyzeGeometry(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
            }
            */
        }

        private static void checkHRad()
        { }
        private static void checkDRad()
        { }
        private static void checkTOffsets()
        { }
        private static void checkARot()
        { }
        private static void checkStepsPMM()
        { }

        private static void checkAccuracy(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float accuracy = UserVariables.accuracy;

            if (X <= accuracy && X >= -accuracy && XOpp <= accuracy && XOpp >= -accuracy && Y <= accuracy && Y >= -accuracy && YOpp <= accuracy && YOpp >= -accuracy && Z <= accuracy && Z >= -accuracy && ZOpp <= accuracy && ZOpp >= -accuracy)
            {
                if (UserVariables.probeChoice == "FSR")
                {
                    EEPROM.zMaxLength -= UserVariables.FSROffset;
                    UserInterface.logConsole("Setting Z Max Length with adjustment for FSR");
                }

                calibrationState = false;
            }
            else
            {
                GCode.checkHeights = true;
                UserInterface.logConsole("Continuing Calibration");
            }
        }

        private static void HRad(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);
            float HRadRatio = UserVariables.HRadRatio;

            EEPROM.HRadius += (HRadSA / HRadRatio);

            X -= HRadSA;
            Y -= HRadSA;
            Z -= HRadSA;
            XOpp -= HRadSA;
            YOpp -= HRadSA;
            ZOpp -= HRadSA;

            UserInterface.logConsole("HRad:" + EEPROM.HRadius.ToString());
        }

        private static void DRad(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float DASA = ((X + XOpp) / 2);
            float DBSA = ((Y + YOpp) / 2);
            float DCSA = ((Z + ZOpp) / 2);

            float DRadRatio = UserVariables.DRadRatio;

            EEPROM.DA -= X / 0.5F;
            EEPROM.DB -= Y / 0.5F;
            EEPROM.DC -= Z / 0.5F;

            XOpp += X * (0.225F / 0.5F);
            YOpp += X * (0.1375F / 0.5F);
            ZOpp += X * (0.1375F / 0.5F);
            X += X / 0.5F;

            XOpp += Y * (0.1375F / 0.5F);
            YOpp += Y * (0.225F / 0.5F);
            ZOpp += Y * (0.1375F / 0.5F);
            Y += Y / 0.5F;

            XOpp += Z * (0.1375F / 0.5F);
            YOpp += Z * (0.1375F / 0.5F);
            ZOpp += Z * (0.225F / 0.5F);
            Z += Z / 0.5F;

            UserInterface.logConsole("DRad: " + EEPROM.DA.ToString() + ", " + EEPROM.DB.ToString() + ", " + EEPROM.DC.ToString());
        }

        /*
        public void analyzeGeometry(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            int analyzeCount = 0;


            UserInterface.logConsole("Expect a slight inaccuracy in the geometry analysis; basic calibration.");
        }
        */

        private static void towerOffsets(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            int j = 0;
            float accuracy = UserVariables.calculationAccuracy;
            float tempX2 = X;
            float tempXOpp2 = XOpp;
            float tempY2 = Y;
            float tempYOpp2 = YOpp;
            float tempZ2 = Z;
            float tempZOpp2 = ZOpp;
            float offsetX = EEPROM.offsetX;
            float offsetY = EEPROM.offsetY;
            float offsetZ = EEPROM.offsetZ;
            float stepsPerMM = EEPROM.stepsPerMM;



            UserInterface.logConsole("VHeights :" + tempX2 + " " + tempXOpp2 + " " + tempY2 + " " + tempYOpp2 + " " + tempZ2 + " " + tempZOpp2);

            while (j < 100)
            {
                if (offsetX >= 50 && offsetY >= 50 && offsetZ >= 50)
                {
                    offsetX += tempX2 * stepsPerMM * (1 / 0.60F);

                    tempXOpp2 += tempX2 * (0.5F / 0.60F);
                    tempY2 += tempX2 * (0.3F / 0.60F);
                    tempYOpp2 += tempX2 * (-0.25F / 0.60F);
                    tempZ2 += tempX2 * (0.3F / 0.60F);
                    tempZOpp2 += tempX2 * (-0.25F / 0.60F);
                    tempX2 += tempX2 / -1;

                    offsetY += tempY2 * stepsPerMM * (1 / 0.60F);

                    tempYOpp2 += tempY2 * (0.5F / 0.60F);
                    tempX2 += tempY2 * (0.3F / 0.60F);
                    tempXOpp2 += tempY2 * (-0.25F / 0.60F);
                    tempZ2 += tempY2 * (0.3F / 0.60F);
                    tempZOpp2 += tempY2 * (-0.25F / 0.60F);
                    tempY2 += tempY2 / -1;

                    offsetZ += tempZ2 * stepsPerMM * (1 / 0.60F);

                    tempZOpp2 += tempZ2 * (0.5F / 0.60F);
                    tempX2 += tempZ2 * (0.3F / 0.60F);
                    tempXOpp2 += tempZ2 * (-0.25F / 0.60F);
                    tempY2 += tempZ2 * (0.3F / 0.60F);
                    tempYOpp2 += tempZ2 * (-0.25F / 0.60F);
                    tempZ2 += tempZ2 / -1;

                    tempX2 = Validation.checkZero(tempX2);
                    tempY2 = Validation.checkZero(tempY2);
                    tempZ2 = Validation.checkZero(tempZ2);
                    tempXOpp2 = Validation.checkZero(tempXOpp2);
                    tempYOpp2 = Validation.checkZero(tempYOpp2);
                    tempZOpp2 = Validation.checkZero(tempZOpp2);

                    UserInterface.logConsole("Offs :" + offsetX + " " + offsetY + " " + offsetZ);
                    UserInterface.logConsole("VHeights :" + tempX2 + " " + tempXOpp2 + " " + tempY2 + " " + tempYOpp2 + " " + tempZ2 + " " + tempZOpp2);
                }

                if (offsetX < 50 || offsetY < 50 || offsetZ < 50)
                {
                    //set xyz to 500
                    //changed all three delta radii - same value
                    //average the final x, y, z height values
                    
                    //HERE

                    j = 100;
                }
                else
                {
                    j++;
                }

                /*
                if (tempX2 < accuracy && tempX2 > -accuracy && tempY2 < accuracy && tempY2 > -accuracy && tempZ2 < accuracy && tempZ2 > -accuracy && offsetX < 1000 && offsetY < 1000 && offsetZ < 1000)
                {
                    j = 100;
                }
                else if (j == 50)
                {
                    //error protection
                    tempX2 = X;
                    tempXOpp2 = XOpp;
                    tempY2 = Y;
                    tempYOpp2 = YOpp;
                    tempZ2 = Z;
                    tempZOpp2 = ZOpp;

                    //X
                    offsetXCorrection = 1.5F;
                    xxOppPerc = 0.5F;
                    xyPerc = 0.25F;
                    xyOppPerc = 0.25F;
                    xzPerc = 0.25F;
                    xzOppPerc = 0.25F;

                    //Y
                    offsetYCorrection = 1.5F;
                    yyOppPerc = 0.5F;
                    yxPerc = 0.25F;
                    yxOppPerc = 0.25F;
                    yzPerc = 0.25F;
                    yzOppPerc = 0.25F;

                    //Z
                    offsetZCorrection = 1.5F;
                    zzOppPerc = 0.5F;
                    zxPerc = 0.25F;
                    zxOppPerc = 0.25F;
                    zyPerc = 0.25F;
                    zyOppPerc = 0.25F;

                    offsetX = 0;
                    offsetY = 0;
                    offsetZ = 0;

                    j++;
                }
                else
                {
                    j++;
                }
                */
            }

            if (offsetX > 1000 || offsetY > 1000 || offsetZ > 1000)
            {
                UserInterface.logConsole("XYZ offset calibration error, setting default values.");
                UserInterface.logConsole("XYZ offsets before damage prevention: X" + offsetX + " Y" + offsetY + " Z" + offsetZ);
                offsetX = 0;
                offsetY = 0;
                offsetZ = 0;
            }
            else
            {
                /*
                float tempMin = Math.Min(offsetX, Math.Min(offsetY, offsetZ));

                offsetX -= tempMin;
                offsetY -= tempMin;
                offsetZ -= tempMin;
                
                //offsetX = tempX2 * stepsPerMM * offsetXCorrection;

                tempX2 = -tempMin / (stepsPerMM * (1 / 0.60F));
                tempY2 = -tempMin / (stepsPerMM * (1 / 0.60F));
                tempZ2 = -tempMin / (stepsPerMM * (1 / 0.60F));
                */

                X = tempX2;
                XOpp = tempXOpp2;
                Y = tempY2;
                YOpp = tempYOpp2;
                Z = tempZ2;
                ZOpp = tempZOpp2;
                
                //UserInterface.logConsole("heights :" + X + " " + XOpp + " " + Y + " " + YOpp + " " + Z + " " + ZOpp);
                //UserInterface.logConsole("XYZ:" + offsetX + " " + offsetY + " " + offsetZ);

                //round to the nearest whole number
                EEPROM.offsetX = Convert.ToInt32(offsetX);
                EEPROM.offsetY = Convert.ToInt32(offsetY);
                EEPROM.offsetZ = Convert.ToInt32(offsetZ);
            }
        }

        private static void alphaRotation(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float offsetX = EEPROM.offsetX;
            float offsetY = EEPROM.offsetY;
            float offsetZ = EEPROM.offsetZ;
            float accuracy = UserVariables.accuracy;

            //change to object
            float alphaRotationPercentage = UserVariables.alphaRotationPercentage;

            int k = 0;
            while (k < 100)
            {
                //X Alpha Rotation
                if (YOpp > ZOpp)
                {
                    float ZYOppAvg = (YOpp - ZOpp) / 2;
                    EEPROM.A = EEPROM.A + (ZYOppAvg * alphaRotationPercentage); // (0.5/((diff y0 and z0 at X + 0.5)-(diff y0 and z0 at X = 0))) * 2 = 1.75
                    YOpp = YOpp - ZYOppAvg;
                    ZOpp = ZOpp + ZYOppAvg;
                }
                else if (YOpp < ZOpp)
                {
                    float ZYOppAvg = (ZOpp - YOpp) / 2;

                    EEPROM.A = EEPROM.A - (ZYOppAvg * alphaRotationPercentage);
                    YOpp = YOpp + ZYOppAvg;
                    ZOpp = ZOpp - ZYOppAvg;
                }

                //Y Alpha Rotation
                if (ZOpp > XOpp)
                {
                    float XZOppAvg = (ZOpp - XOpp) / 2;
                    EEPROM.B = EEPROM.B + (XZOppAvg * alphaRotationPercentage);
                    ZOpp = ZOpp - XZOppAvg;
                    XOpp = XOpp + XZOppAvg;
                }
                else if (ZOpp < XOpp)
                {
                    float XZOppAvg = (XOpp - ZOpp) / 2;

                    EEPROM.B = EEPROM.B - (XZOppAvg * alphaRotationPercentage);
                    ZOpp = ZOpp + XZOppAvg;
                    XOpp = XOpp - XZOppAvg;
                }
                //Z Alpha Rotation
                if (XOpp > YOpp)
                {
                    float YXOppAvg = (XOpp - YOpp) / 2;
                    EEPROM.C = EEPROM.C + (YXOppAvg * alphaRotationPercentage);
                    XOpp = XOpp - YXOppAvg;
                    YOpp = YOpp + YXOppAvg;
                }
                else if (XOpp < YOpp)
                {
                    float YXOppAvg = (YOpp - XOpp) / 2;

                    EEPROM.C = EEPROM.C - (YXOppAvg * alphaRotationPercentage);
                    XOpp = XOpp + YXOppAvg;
                    YOpp = YOpp - YXOppAvg;
                }

                //determine if value is close enough
                float hTow = Math.Max(Math.Max(XOpp, YOpp), ZOpp);
                float lTow = Math.Min(Math.Min(XOpp, YOpp), ZOpp);
                float towDiff = hTow - lTow;

                if (towDiff < UserVariables.calculationAccuracy && towDiff > -UserVariables.calculationAccuracy)
                {
                    k = 100;

                    //log
                    UserInterface.logConsole("ABC:" + EEPROM.A + " " + EEPROM.B + " " + EEPROM.C);
                }
                else
                {
                    k++;
                }
            }
        }

        private static void stepsPMM(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float diagChange = 1 / UserVariables.deltaOpp;
            float towChange = 1 / UserVariables.deltaTower;

            float XYZ = (X + Y + Z) / 3;
            float XYZOpp = (XOpp + YOpp + ZOpp) / 3;

            EEPROM.stepsPerMM -= (XYZ - XYZOpp) * ((diagChange + towChange) / 2);

            X += (XYZ - XYZOpp) * towChange;
            Y += (XYZ - XYZOpp) * towChange;
            Z += (XYZ - XYZOpp) * towChange;
            XOpp += (XYZ - XYZOpp) * diagChange;
            YOpp += (XYZ - XYZOpp) * diagChange;
            ZOpp += (XYZ - XYZOpp) * diagChange;

            UserInterface.logConsole("Steps per Millimeter: " + EEPROM.stepsPerMM.ToString());
        }

        /*
        private static void LinearRegression(float[] xVals, float[] yVals, int inclusiveStart, int exclusiveEnd, out float rsquared, out float yintercept, out float slope)
        {
            float sumOfX = 0;
            float sumOfY = 0;
            float sumOfXSq = 0;
            float sumOfYSq = 0;
            float ssX = 0;
            float ssY = 0;
            float sumCodeviates = 0;
            float sCo = 0;
            float count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                float x = xVals[ctr];
                float y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }

            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            float RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            float RDenom = (count * sumOfXSq - (sumOfX * sumOfX))
             * (count * sumOfYSq - (sumOfY * sumOfY));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            float meanX = sumOfX / count;
            float meanY = sumOfY / count;
            float dblR = RNumerator / Convert.ToSingle(Math.Sqrt(RDenom));
            rsquared = dblR * dblR;
            yintercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }
        */
    }
}
