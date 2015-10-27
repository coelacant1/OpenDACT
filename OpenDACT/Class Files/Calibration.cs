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

        public static void calibrate(int value)
        {
            if (value == 0)
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

            //FIX CHECK ACCURACY

            tempAccuracy = (Math.Abs(Heights.X) + Math.Abs(Heights.XOpp) + Math.Abs(Heights.Y) + Math.Abs(Heights.YOpp) + Math.Abs(Heights.Z) + Math.Abs(Heights.ZOpp)) / 6;
            Program.mainFormTest.setAccuracyPoint(iterationNum, tempAccuracy);
            checkAccuracy(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);

            if (calibrationState == true)
            {
                HRad(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                DRad(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                towerOffsets(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                alphaRotation(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
                diagonalRodSPM(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
            }
            else
            {
                //analyzeGeometry(ref Heights.X, ref Heights.XOpp, ref Heights.Y, ref Heights.YOpp, ref Heights.Z, ref Heights.ZOpp);
            }
            //change all instances of a new variable which calls a class object to modify the class object directly as opposed to just pulling its value
        }
        public static void iterativeCalibration()
        {

        }

        public static void checkAccuracy(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
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

        public static void HRad(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);
            float HRadRatio = UserVariables.HRadRatio;

            EEPROM.HRadius = EEPROM.HRadius + (HRadSA / HRadRatio);

            X -= HRadSA;
            Y -= HRadSA;
            Z -= HRadSA;
            XOpp -= HRadSA;
            YOpp -= HRadSA;
            ZOpp -= HRadSA;

            UserInterface.logConsole("HRad:" + EEPROM.HRadius.ToString());
        }

        public static void DRad(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float DASA = ((X + XOpp) / 2);
            float DBSA = ((Y + YOpp) / 2);
            float DCSA = ((Z + ZOpp) / 2);
            float DRadRatio = UserVariables.DRadRatio;

            EEPROM.DA += ((DASA) / DRadRatio);
            EEPROM.DB += ((DBSA) / DRadRatio);
            EEPROM.DC += ((DCSA) / DRadRatio);

            X = X + ((DASA) / DRadRatio) * 0.5F;
            XOpp = XOpp + ((DASA) / DRadRatio) * 0.225F;
            Y = Y + ((DASA) / DRadRatio) * 0.1375F;
            YOpp = YOpp + ((DASA) / DRadRatio) * 0.1375F;
            Z = Z + ((DASA) / DRadRatio) * 0.1375F;
            ZOpp = ZOpp + ((DASA) / DRadRatio) * 0.1375F;

            X = X + ((DBSA) / DRadRatio) * 0.1375F;
            XOpp = XOpp + ((DBSA) / DRadRatio) * 0.1375F;
            Y = Y + ((DBSA) / DRadRatio) * 0.5F;
            YOpp = YOpp + ((DBSA) / DRadRatio) * 0.225F;
            Z = Z + ((DBSA) / DRadRatio) * 0.1375F;
            ZOpp = ZOpp + ((DBSA) / DRadRatio) * 0.1375F;

            X = X + ((DCSA) / DRadRatio) * 0.1375F;
            XOpp = XOpp + ((DCSA) / DRadRatio) * 0.1375F;
            Y = Y + ((DCSA) / DRadRatio) * 0.1375F;
            YOpp = YOpp + ((DCSA) / DRadRatio) * 0.1375F;
            Z = Z + ((DCSA) / DRadRatio) * 0.5F;
            ZOpp = ZOpp + ((DCSA) / DRadRatio) * 0.225F;

            UserInterface.logConsole("DRad: " + EEPROM.DA.ToString() + ", " + EEPROM.DB.ToString() + ", " + EEPROM.DC.ToString());
        }
        /*
        public void analyzeGeometry(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            int analyzeCount = 0;


            UserInterface.logConsole("Expect a slight inaccuracy in the geometry analysis; basic calibration.");
        }
        */
        public static void towerOffsets(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
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

            //
            float offsetXCorrection = UserVariables.offsetXCorrection;
            float offsetYCorrection = UserVariables.offsetYCorrection;
            float offsetZCorrection = UserVariables.offsetZCorrection;
            float xxOppPerc = UserVariables.xxOppPerc;
            float xyPerc = UserVariables.xyPerc;
            float xyOppPerc = UserVariables.xyOppPerc;
            float xzPerc = UserVariables.xzPerc;
            float xzOppPerc = UserVariables.xzOppPerc;
            float yyOppPerc = UserVariables.yyOppPerc;
            float yxPerc = UserVariables.yxPerc;
            float yxOppPerc = UserVariables.yxOppPerc;
            float yzPerc = UserVariables.yzPerc;
            float yzOppPerc = UserVariables.yzOppPerc;
            float zzOppPerc = UserVariables.zzOppPerc;
            float zxPerc = UserVariables.zxPerc;
            float zxOppPerc = UserVariables.zxOppPerc;
            float zyPerc = UserVariables.zyPerc;
            float zyOppPerc = UserVariables.zyOppPerc;

            while (j < 100)
            {
                float theoryX = offsetX + tempX2 * stepsPerMM * offsetXCorrection;

                //correction of one tower allows for XY dimensional accuracy
                if (tempX2 > 0)
                {
                    //if x is positive
                    offsetX = offsetX + tempX2 * stepsPerMM * offsetXCorrection;

                    tempXOpp2 = tempXOpp2 + (tempX2 * xxOppPerc);//0.5
                    tempZ2 = tempZ2 + (tempX2 * xzPerc);//0.25
                    tempY2 = tempY2 + (tempX2 * xyPerc);//0.25
                    tempZOpp2 = tempZOpp2 - (tempX2 * xzOppPerc);//0.25
                    tempYOpp2 = tempYOpp2 - (tempX2 * xyOppPerc);//0.25
                    tempX2 = tempX2 - tempX2;
                }
                else if (theoryX > 0 && tempX2 < 0)
                {
                    //if x is negative and can be decreased
                    offsetX = offsetX + tempX2 * stepsPerMM * offsetXCorrection;

                    tempXOpp2 = tempXOpp2 + (tempX2 * xxOppPerc);//0.5
                    tempZ2 = tempZ2 + (tempX2 * xzPerc);//0.25
                    tempY2 = tempY2 + (tempX2 * xyPerc);//0.25
                    tempZOpp2 = tempZOpp2 - (tempX2 * xzOppPerc);//0.25
                    tempYOpp2 = tempYOpp2 - (tempX2 * xyOppPerc);//0.25
                    tempX2 = tempX2 - tempX2;
                }
                else
                {
                    //if tempX2 is negative
                    offsetY = offsetY - tempX2 * stepsPerMM * offsetYCorrection * 2;
                    offsetZ = offsetZ - tempX2 * stepsPerMM * offsetZCorrection * 2;

                    tempYOpp2 = tempYOpp2 - (tempX2 * 2 * yyOppPerc);
                    tempX2 = tempX2 - (tempX2 * 2 * yxPerc);
                    tempZ2 = tempZ2 - (tempX2 * 2 * yzPerc);
                    tempXOpp2 = tempXOpp2 + (tempX2 * 2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 + (tempX2 * 2 * yzOppPerc);
                    tempY2 = tempY2 + tempX2 * 2;

                    tempZOpp2 = tempZOpp2 - (tempX2 * 2 * zzOppPerc);
                    tempX2 = tempX2 - (tempX2 * 2 * zxPerc);
                    tempY2 = tempY2 - (tempX2 * 2 * zyPerc);
                    tempXOpp2 = tempXOpp2 + (tempX2 * 2 * zxOppPerc);
                    tempYOpp2 = tempYOpp2 + (tempX2 * 2 * zyOppPerc);
                    tempZ2 = tempZ2 + tempX2 * 2;
                }

                float theoryY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                //Y
                if (tempY2 > 0)
                {
                    offsetY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                    tempYOpp2 = tempYOpp2 + (tempY2 * yyOppPerc);
                    tempX2 = tempX2 + (tempY2 * yxPerc);
                    tempZ2 = tempZ2 + (tempY2 * yzPerc);
                    tempXOpp2 = tempXOpp2 - (tempY2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 - (tempY2 * yzOppPerc);
                    tempY2 = tempY2 - tempY2;
                }
                else if (theoryY > 0 && tempY2 < 0)
                {
                    offsetY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                    tempYOpp2 = tempYOpp2 + (tempY2 * yyOppPerc);
                    tempX2 = tempX2 + (tempY2 * yxPerc);
                    tempZ2 = tempZ2 + (tempY2 * yzPerc);
                    tempXOpp2 = tempXOpp2 - (tempY2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 - (tempY2 * yzOppPerc);
                    tempY2 = tempY2 - tempY2;
                }
                else
                {
                    offsetX = offsetX - tempY2 * stepsPerMM * offsetXCorrection * 2;
                    offsetZ = offsetZ - tempY2 * stepsPerMM * offsetZCorrection * 2;

                    tempXOpp2 = tempXOpp2 - (tempY2 * 2 * xxOppPerc);//0.5
                    tempZ2 = tempZ2 - (tempY2 * 2 * xzPerc);//0.25
                    tempY2 = tempY2 - (tempY2 * 2 * xyPerc);//0.25
                    tempZOpp2 = tempZOpp2 + (tempY2 * 2 * xzOppPerc);//0.25
                    tempYOpp2 = tempYOpp2 + (tempY2 * 2 * xyOppPerc);//0.25
                    tempX2 = tempX2 + tempY2 * 2;

                    tempZOpp2 = tempZOpp2 - (tempY2 * 2 * zzOppPerc);
                    tempX2 = tempX2 - (tempY2 * 2 * zxPerc);
                    tempY2 = tempY2 - (tempY2 * 2 * zyPerc);
                    tempXOpp2 = tempXOpp2 + (tempY2 * 2 * zxOppPerc);
                    tempYOpp2 = tempYOpp2 + (tempY2 * 2 * zyOppPerc);
                    tempZ2 = tempZ2 + tempY2 * 2;
                }

                float theoryZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                //Z
                if (tempZ2 > 0)
                {
                    offsetZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                    tempZOpp2 = tempZOpp2 + (tempZ2 * zzOppPerc);
                    tempX2 = tempX2 + (tempZ2 * zxPerc);
                    tempY2 = tempY2 + (tempZ2 * zyPerc);
                    tempXOpp2 = tempXOpp2 - (tempZ2 * zxOppPerc);
                    tempYOpp2 = tempYOpp2 - (tempZ2 * zyOppPerc);
                    tempZ2 = tempZ2 - tempZ2;
                }
                else if (theoryZ > 0 && tempZ2 < 0)
                {
                    offsetZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                    tempZOpp2 = tempZOpp2 + (tempZ2 * zzOppPerc);
                    tempX2 = tempX2 + (tempZ2 * zxPerc);
                    tempY2 = tempY2 + (tempZ2 * zyPerc);
                    tempXOpp2 = tempXOpp2 - (tempZ2 * zxOppPerc);
                    tempYOpp2 = tempYOpp2 - (tempZ2 * zyOppPerc);
                    tempZ2 = tempZ2 - tempZ2;
                }
                else
                {
                    offsetY = offsetY - tempZ2 * stepsPerMM * offsetYCorrection * 2;
                    offsetX = offsetX - tempZ2 * stepsPerMM * offsetXCorrection * 2;

                    tempXOpp2 = tempXOpp2 - (tempZ2 * 2 * xxOppPerc);//0.5
                    tempZ2 = tempZ2 - (tempZ2 * 2 * xzPerc);//0.25
                    tempY2 = tempY2 - (tempZ2 * 2 * xyPerc);//0.25
                    tempZOpp2 = tempZOpp2 + (tempZ2 * 2 * xzOppPerc);//0.25
                    tempYOpp2 = tempYOpp2 + (tempZ2 * 2 * xyOppPerc);//0.25
                    tempX2 = tempX2 + tempZ2 * 2;

                    tempYOpp2 = tempYOpp2 - (tempZ2 * 2 * yyOppPerc);
                    tempX2 = tempX2 - (tempZ2 * 2 * yxPerc);
                    tempZ2 = tempZ2 - (tempZ2 * 2 * yzPerc);
                    tempXOpp2 = tempXOpp2 + (tempZ2 * 2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 + (tempZ2 * 2 * yzOppPerc);
                    tempY2 = tempY2 + tempZ2 * 2;
                }

                tempX2 = Validation.checkZero(tempX2);
                tempY2 = Validation.checkZero(tempY2);
                tempZ2 = Validation.checkZero(tempZ2);
                tempXOpp2 = Validation.checkZero(tempXOpp2);
                tempYOpp2 = Validation.checkZero(tempYOpp2);
                tempZOpp2 = Validation.checkZero(tempZOpp2);


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
                X = tempX2;
                XOpp = tempXOpp2;
                Y = tempY2;
                YOpp = tempYOpp2;
                Z = tempZ2;
                ZOpp = tempZOpp2;
            }

            //round to the nearest whole number
            EEPROM.offsetX = Convert.ToInt32(offsetX);
            EEPROM.offsetY = Convert.ToInt32(offsetY);
            EEPROM.offsetZ = Convert.ToInt32(offsetZ);

            UserInterface.logConsole("XYZ:" + offsetX + " " + offsetY + " " + offsetZ);
        }

        public static void alphaRotation(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float offsetX = EEPROM.offsetX;
            float offsetY = EEPROM.offsetY;
            float offsetZ = EEPROM.offsetZ;
            float accuracy = UserVariables.accuracy;

            //change to object
            float alphaRotationPercentageX = UserVariables.alphaRotationPercentageX;
            float alphaRotationPercentageY = UserVariables.alphaRotationPercentageY;
            float alphaRotationPercentageZ = UserVariables.alphaRotationPercentageZ;

            int k = 0;
            while (k < 100)
            {
                //X Alpha Rotation
                if (YOpp > ZOpp)
                {
                    float ZYOppAvg = (YOpp - ZOpp) / 2;
                    EEPROM.A = EEPROM.A + (ZYOppAvg * alphaRotationPercentageX); // (0.5/((diff y0 and z0 at X + 0.5)-(diff y0 and z0 at X = 0))) * 2 = 1.75
                    YOpp = YOpp - ZYOppAvg;
                    ZOpp = ZOpp + ZYOppAvg;
                }
                else if (YOpp < ZOpp)
                {
                    float ZYOppAvg = (ZOpp - YOpp) / 2;

                    EEPROM.A = EEPROM.A - (ZYOppAvg * alphaRotationPercentageX);
                    YOpp = YOpp + ZYOppAvg;
                    ZOpp = ZOpp - ZYOppAvg;
                }

                //Y Alpha Rotation
                if (ZOpp > XOpp)
                {
                    float XZOppAvg = (ZOpp - XOpp) / 2;
                    EEPROM.B = EEPROM.B + (XZOppAvg * alphaRotationPercentageY);
                    ZOpp = ZOpp - XZOppAvg;
                    XOpp = XOpp + XZOppAvg;
                }
                else if (ZOpp < XOpp)
                {
                    float XZOppAvg = (XOpp - ZOpp) / 2;

                    EEPROM.B = EEPROM.B - (XZOppAvg * alphaRotationPercentageY);
                    ZOpp = ZOpp + XZOppAvg;
                    XOpp = XOpp - XZOppAvg;
                }
                //Z Alpha Rotation
                if (XOpp > YOpp)
                {
                    float YXOppAvg = (XOpp - YOpp) / 2;
                    EEPROM.C = EEPROM.C + (YXOppAvg * alphaRotationPercentageZ);
                    XOpp = XOpp - YXOppAvg;
                    YOpp = YOpp + YXOppAvg;
                }
                else if (XOpp < YOpp)
                {
                    float YXOppAvg = (YOpp - XOpp) / 2;

                    EEPROM.C = EEPROM.C - (YXOppAvg * alphaRotationPercentageZ);
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

        public static void diagonalRodSPM(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float diagChange = 1 / UserVariables.deltaOpp;
            float towOppDiff = UserVariables.deltaTower / UserVariables.deltaOpp; //0.5
            float XYZ = (X + Y + Z) / 3;
            float XYZOpp = (XOpp + YOpp + ZOpp) / 3;

            int i = 0;
            while (i < 100)
            {
                EEPROM.stepsPerMM += (Math.Abs(XYZ - XYZOpp) * diagChange);

                X = X - towOppDiff * XYZOpp;
                Y = Y - towOppDiff * XYZOpp;
                Z = Z - towOppDiff * XYZOpp;
                XOpp = XOpp - XYZOpp;
                YOpp = YOpp - XYZOpp;
                ZOpp = ZOpp - XYZOpp;
                XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                XYZOpp = Validation.checkZero(XYZOpp);

                //hrad
                EEPROM.HRadius += (XYZ / UserVariables.HRadRatio);

                if (XYZOpp >= 0)
                {
                    X -= XYZ;
                    Y -= XYZ;
                    Z -= XYZ;
                    XOpp -= XYZ;
                    YOpp -= XYZ;
                    ZOpp -= XYZ;
                }
                else
                {
                    X += XYZ;
                    Y += XYZ;
                    Z += XYZ;
                    XOpp += XYZ;
                    YOpp += XYZ;
                    ZOpp += XYZ;
                }

                X = Validation.checkZero(X);
                Y = Validation.checkZero(Y);
                Z = Validation.checkZero(Z);
                XOpp = Validation.checkZero(XOpp);
                YOpp = Validation.checkZero(YOpp);
                ZOpp = Validation.checkZero(ZOpp);

                UserInterface.logConsole(EEPROM.stepsPerMM.ToString());

                //XYZ is zero
                if (Math.Abs(XYZ - XYZOpp) < UserVariables.calculationAccuracy)
                {
                    i = 100;
                }
                else
                {
                    i++;
                }
            }
        }
        public static void LinearRegression(float[] xVals, float[] yVals, int inclusiveStart, int exclusiveEnd, out float rsquared, out float yintercept, out float slope)
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
    }
}
