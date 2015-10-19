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

        public static void calibrate(int value, ref EEPROM eeprom, ref Heights heights, ref UserVariables userVariables)
        {
            if (value == 0)
            {
                basicCalibration(ref eeprom, ref heights, ref userVariables);
            }
            else
            {
                iterativeCalibration();
            }

            iterationNum++;
        }

        public static void learnPrinter(ref EEPROM eeprom, ref Heights heights, ref UserVariables userVariables)
        {
            GCode.heuristicLearning(ref eeprom, ref userVariables, ref heights);
        }

        public static void basicCalibration(ref EEPROM eeprom, ref Heights heights, ref UserVariables userVariables)
        {
            //check if eeprom object remains after this function is called for the second time

            if (iterationNum == 0)
            {
                if (userVariables.diagonalRodLength == Convert.ToSingle(""))
                {
                    userVariables.diagonalRodLength = eeprom.diagonalRod;
                    UserInterface.logConsole("Using default diagonal rod length from EEPROM");
                }
            }

            tempAccuracy = (Math.Abs(heights.X) + Math.Abs(heights.XOpp) + Math.Abs(heights.Y) + Math.Abs(heights.YOpp) + Math.Abs(heights.Z) + Math.Abs(heights.ZOpp)) / 6;
            Program.mainFormTest.setAccuracyPoint(iterationNum, tempAccuracy);
            checkAccuracy(ref eeprom, ref userVariables, ref heights.X, ref heights.XOpp, ref heights.Y, ref heights.YOpp, ref heights.Z, ref heights.ZOpp);
            HRad(ref eeprom, ref userVariables, ref heights.X, ref heights.XOpp, ref heights.Y, ref heights.YOpp, ref heights.Z, ref heights.ZOpp);
            DRad(ref eeprom, ref userVariables, ref heights.X, ref heights.XOpp, ref heights.Y, ref heights.YOpp, ref heights.Z, ref heights.ZOpp);
            //analyzeGeometry(ref eeprom, ref heights.X, ref heights.XOpp, ref heights.Y, ref heights.YOpp, ref heights.Z, ref heights.ZOpp);
            towerOffsets(ref eeprom, ref userVariables, ref heights.X, ref heights.XOpp, ref heights.Y, ref heights.YOpp, ref heights.Z, ref heights.ZOpp);
            alphaRotation(ref eeprom, ref userVariables, ref heights.X, ref heights.XOpp, ref heights.Y, ref heights.YOpp, ref heights.Z, ref heights.ZOpp);
            diagonalRodSPM(ref eeprom, ref userVariables, ref heights.X, ref heights.XOpp, ref heights.Y, ref heights.YOpp, ref heights.Z, ref heights.ZOpp);

            //change all instances of a new variable which calls a class object to modify the class object directly as opposed to just pulling its value
        }
        public static void iterativeCalibration()
        {

        }

        public static void checkAccuracy(ref EEPROM eeprom, ref UserVariables userVariables, ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float accuracy = userVariables.accuracy;

            if (X <= accuracy && X >= -accuracy && XOpp <= accuracy && XOpp >= -accuracy && Y <= accuracy && Y >= -accuracy && YOpp <= accuracy && YOpp >= -accuracy && Z <= accuracy && Z >= -accuracy && ZOpp <= accuracy && ZOpp >= -accuracy)
            {
                //fsr plate offset
                string zMinTemp = userVariables.probeChoice.ToString();
                string textFSRPO = userVariables.FSROffset.ToString();

                if (userVariables.probeChoice == "FSR")
                {
                    GCode.sendEEPROMVariable(3, 153, eeprom.zMaxLength - Convert.ToSingle(textFSRPO));
                    UserInterface.logConsole("Setting Z Max Length with adjustment for FSR");
                    Thread.Sleep(userVariables.pauseTimeSet);
                }

                calibrationState = false;
                Thread.Sleep(userVariables.pauseTimeSet);
                GCode.homeAxes();
                UserInterface.logConsole("Calibration Complete");
                //end code
            }
            else
            {
                GCode.checkHeights = true;
            }
        }

        public static void HRad(ref EEPROM eeprom, ref UserVariables userVariables, ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);
            float HRadRatio = userVariables.HRadRatio;

            eeprom.HRadius = eeprom.HRadius + (HRadSA / HRadRatio);

            X -= HRadSA;
            Y -= HRadSA;
            Z -= HRadSA;
            XOpp -= HRadSA;
            YOpp -= HRadSA;
            ZOpp -= HRadSA;

            UserInterface.logConsole("HRad:" + eeprom.HRadius.ToString());
        }

        public static void DRad(ref EEPROM eeprom, ref UserVariables userVariables, ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float DASA = ((X + XOpp) / 2);
            float DBSA = ((Y + YOpp) / 2);
            float DCSA = ((Z + ZOpp) / 2);
            float DRadRatio = userVariables.DRadRatio;

            eeprom.DA += ((DASA) / DRadRatio);
            eeprom.DB += ((DBSA) / DRadRatio);
            eeprom.DC += ((DCSA) / DRadRatio);

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

            UserInterface.logConsole("DRad: " + eeprom.DA.ToString() + ", " + eeprom.DB.ToString() + ", " + eeprom.DC.ToString());
        }
        /*
        public void analyzeGeometry(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            int analyzeCount = 0;


            UserInterface.logConsole("Expect a slight inaccuracy in the geometry analysis; basic calibration.");
        }
        */
        public static void towerOffsets(ref EEPROM eeprom, ref UserVariables userVariables, ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            int j = 0;
            float accuracy = 0.001F;
            float tempX2 = X;
            float tempXOpp2 = XOpp;
            float tempY2 = Y;
            float tempYOpp2 = YOpp;
            float tempZ2 = Z;
            float tempZOpp2 = ZOpp;
            float offsetX = eeprom.offsetX;
            float offsetY = eeprom.offsetY;
            float offsetZ = eeprom.offsetZ;
            float stepsPerMM = eeprom.stepsPerMM;

            //
            float offsetXCorrection = userVariables.offsetXCorrection;
            float offsetYCorrection = userVariables.offsetYCorrection;
            float offsetZCorrection = userVariables.offsetZCorrection;
            float xxOppPerc = userVariables.xxOppPerc;
            float xyPerc = userVariables.xyPerc;
            float xyOppPerc = userVariables.xyOppPerc;
            float xzPerc = userVariables.xzPerc;
            float xzOppPerc = userVariables.xzOppPerc;
            float yyOppPerc = userVariables.yyOppPerc;
            float yxPerc = userVariables.yxPerc;
            float yxOppPerc = userVariables.yxOppPerc;
            float yzPerc = userVariables.yzPerc;
            float yzOppPerc = userVariables.yzOppPerc;
            float zzOppPerc = userVariables.zzOppPerc;
            float zxPerc = userVariables.zxPerc;
            float zxOppPerc = userVariables.zxOppPerc;
            float zyPerc = userVariables.zyPerc;
            float zyOppPerc = userVariables.zyOppPerc;

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
            eeprom.offsetX = Convert.ToInt32(offsetX);
            eeprom.offsetY = Convert.ToInt32(offsetY);
            eeprom.offsetZ = Convert.ToInt32(offsetZ);

            UserInterface.logConsole("XYZ:" + offsetX + " " + offsetY + " " + offsetZ);
        }

        public static void alphaRotation(ref EEPROM eeprom, ref UserVariables userVariables, ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float offsetX = eeprom.offsetX;
            float offsetY = eeprom.offsetY;
            float offsetZ = eeprom.offsetZ;
            float accuracy = userVariables.accuracy;

            //change to object
            float alphaRotationPercentageX = userVariables.alphaRotationPercentageX;
            float alphaRotationPercentageY = userVariables.alphaRotationPercentageY;
            float alphaRotationPercentageZ = userVariables.alphaRotationPercentageZ;

            int k = 0;
            while (k < 100)
            {
                //X Alpha Rotation
                if (YOpp > ZOpp)
                {
                    float ZYOppAvg = (YOpp - ZOpp) / 2;
                    eeprom.A = eeprom.A + (ZYOppAvg * alphaRotationPercentageX); // (0.5/((diff y0 and z0 at X + 0.5)-(diff y0 and z0 at X = 0))) * 2 = 1.75
                    YOpp = YOpp - ZYOppAvg;
                    ZOpp = ZOpp + ZYOppAvg;
                }
                else if (YOpp < ZOpp)
                {
                    float ZYOppAvg = (ZOpp - YOpp) / 2;

                    eeprom.A = eeprom.A - (ZYOppAvg * alphaRotationPercentageX);
                    YOpp = YOpp + ZYOppAvg;
                    ZOpp = ZOpp - ZYOppAvg;
                }

                //Y Alpha Rotation
                if (ZOpp > XOpp)
                {
                    float XZOppAvg = (ZOpp - XOpp) / 2;
                    eeprom.B = eeprom.B + (XZOppAvg * alphaRotationPercentageY);
                    ZOpp = ZOpp - XZOppAvg;
                    XOpp = XOpp + XZOppAvg;
                }
                else if (ZOpp < XOpp)
                {
                    float XZOppAvg = (XOpp - ZOpp) / 2;

                    eeprom.B = eeprom.B - (XZOppAvg * alphaRotationPercentageY);
                    ZOpp = ZOpp + XZOppAvg;
                    XOpp = XOpp - XZOppAvg;
                }
                //Z Alpha Rotation
                if (XOpp > YOpp)
                {
                    float YXOppAvg = (XOpp - YOpp) / 2;
                    eeprom.C = eeprom.C + (YXOppAvg * alphaRotationPercentageZ);
                    XOpp = XOpp - YXOppAvg;
                    YOpp = YOpp + YXOppAvg;
                }
                else if (XOpp < YOpp)
                {
                    float YXOppAvg = (YOpp - XOpp) / 2;

                    eeprom.C = eeprom.C - (YXOppAvg * alphaRotationPercentageZ);
                    XOpp = XOpp + YXOppAvg;
                    YOpp = YOpp - YXOppAvg;
                }

                //determine if value is close enough
                float hTow = Math.Max(Math.Max(XOpp, YOpp), ZOpp);
                float lTow = Math.Min(Math.Min(XOpp, YOpp), ZOpp);
                float towDiff = hTow - lTow;

                if (towDiff < accuracy && towDiff > -accuracy)
                {
                    k = 100;
                }
                else
                {
                    k++;
                }

                //log
                UserInterface.logConsole("ABC:" + eeprom.A + " " + eeprom.B + " " + eeprom.C);
            }
        }

        public static void diagonalRodSPM(ref EEPROM eeprom, ref UserVariables userVariables, ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {

            //opp = 0.21; //4/5
            //tower = 0.27; //9/32

            float diagChange = 1 / userVariables.deltaOpp;
            float towOppDiff = userVariables.deltaTower / userVariables.deltaOpp; //0.5
            float XYZ = (X + Y + Z) / 3;
            float XYZOpp = (XOpp + YOpp + ZOpp) / 3;
            float accuracy = userVariables.accuracy;

            //UserInterface.logConsole(X.ToString() + " " + XOpp.ToString() + " " + Y.ToString() + " " + YOpp.ToString() + " " + Z.ToString() + " " + ZOpp.ToString());

            if (Math.Abs(XYZOpp - XYZ) > accuracy * 2)
            {
                int i = 0;
                eeprom.diagonalRod += (XYZOpp * diagChange);
                X = X - towOppDiff * XYZOpp;
                Y = Y - towOppDiff * XYZOpp;
                Z = Z - towOppDiff * XYZOpp;
                XOpp = XOpp - XYZOpp;
                YOpp = YOpp - XYZOpp;
                ZOpp = ZOpp - XYZOpp;
                XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                XYZOpp = Validation.checkZero(XYZOpp);

                //hrad
                eeprom.HRadius += (XYZ / userVariables.HRadRatio);

                if (XYZOpp >= 0)
                {
                    X = X - XYZ;
                    Y = Y - XYZ;
                    Z = Z - XYZ;
                    XOpp = XOpp - XYZ;
                    YOpp = YOpp - XYZ;
                    ZOpp = ZOpp - XYZ;
                }
                else
                {
                    X = X + XYZ;
                    Y = Y + XYZ;
                    Z = Z + XYZ;
                    XOpp = XOpp + XYZ;
                    YOpp = YOpp + XYZ;
                    ZOpp = ZOpp + XYZ;
                }

                X = Validation.checkZero(X);
                Y = Validation.checkZero(Y);
                Z = Validation.checkZero(Z);
                XOpp = Validation.checkZero(XOpp);
                YOpp = Validation.checkZero(YOpp);
                ZOpp = Validation.checkZero(ZOpp);

                //XYZ is zero
                if (XYZOpp < accuracy && XYZOpp > -accuracy && XYZ < accuracy && XYZ > -accuracy)
                {
                    //end calculation
                    eeprom.diagonalRod = Validation.checkZero(eeprom.diagonalRod);

                    //add diagonal rod and steps per millimeter to list to use later for linear regression
                    userVariables.known_xDR.Add(eeprom.diagonalRod);
                    userVariables.known_yDR.Add(eeprom.stepsPerMM);

                    //prevent using linear regression if there are not two values store
                    if (userVariables.stepsCalcNumber >= 2)
                    {
                        if (userVariables.stepsCalcNumber >= 3)
                        {
                            userVariables.known_xDR.RemoveAt(userVariables.l);
                            userVariables.known_yDR.RemoveAt(userVariables.l);
                            userVariables.l++;
                        }

                        float rsquared = 0;
                        float yintercept = 0;
                        float slope = 0;

                        LinearRegression(userVariables.known_xDR.ToArray(), userVariables.known_yDR.ToArray(), 0, userVariables.known_yDR.ToArray().Length, out rsquared, out yintercept, out slope);
                        eeprom.stepsPerMM = slope * userVariables.diagonalRodLength + yintercept;

                        Thread.Sleep(userVariables.pauseTimeSet);
                        GCode.sendEEPROMVariable(3, 11, eeprom.stepsPerMM);
                        UserInterface.logConsole("Steps Per Millimeter Changed: " + eeprom.stepsPerMM.ToString());

                        UserInterface.logConsole("SPM yintercept: " + yintercept);
                        UserInterface.logConsole("SPM slope: " + slope);

                        float changeInMM = ((eeprom.stepsPerMM * eeprom.zMaxLength) - (eeprom.tempSPM * eeprom.zMaxLength)) / eeprom.stepsPerMM;

                        UserInterface.logConsole("zMaxLength changed by: " + changeInMM);
                        UserInterface.logConsole("zMaxLength before: " + eeprom.zMaxLength);
                        UserInterface.logConsole("zMaxLength after: " + (eeprom.zMaxLength - changeInMM));

                        GCode.sendEEPROMVariable(3, 153, (eeprom.zMaxLength - changeInMM));
                        UserInterface.logConsole("Resetting Z Max Length\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }
                    else if (userVariables.stepsCalcNumber == 0)
                    {
                        //add one to steps calnumber
                        userVariables.stepsCalcNumber++;

                        //adds a point to the array below the stepsPerMM
                        eeprom.stepsPerMM = eeprom.tempSPM - (1 / eeprom.tempSPM) * 160;
                        UserInterface.logConsole("Steps Per Millimeter Decreased: " + eeprom.stepsPerMM.ToString());

                        Thread.Sleep(userVariables.pauseTimeSet);
                        GCode.sendEEPROMVariable(3, 11, eeprom.stepsPerMM);
                        UserInterface.logConsole("Setting steps per millimeter\n");

                        float changeInMM = ((eeprom.stepsPerMM * eeprom.zMaxLength) - (eeprom.tempSPM * eeprom.zMaxLength)) / eeprom.stepsPerMM;

                        UserInterface.logConsole("zMaxLength changed by: " + changeInMM);
                        UserInterface.logConsole("zMaxLength before: " + eeprom.zMaxLength);
                        UserInterface.logConsole("zMaxLength after: " + (eeprom.zMaxLength - changeInMM));

                        GCode.sendEEPROMVariable(3, 153, (eeprom.zMaxLength - changeInMM));
                        UserInterface.logConsole("Resetting Z Max Length\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }
                    else if (userVariables.stepsCalcNumber == 1)
                    {
                        //add one to steps calnumber
                        userVariables.stepsCalcNumber++;

                        //adds a point to the array above the stepsPerMM
                        eeprom.stepsPerMM = eeprom.tempSPM + (1 / eeprom.tempSPM) * 160;//*2 to compensate for the subtraction
                        UserInterface.logConsole("Steps Per Millimeter Increased: " + eeprom.stepsPerMM.ToString());

                        Thread.Sleep(userVariables.pauseTimeSet);
                        GCode.sendEEPROMVariable(3, 11, eeprom.stepsPerMM);
                        UserInterface.logConsole("Setting steps per millimeter\n");

                        float changeInMM = ((eeprom.stepsPerMM * eeprom.zMaxLength) - (eeprom.tempSPM * eeprom.zMaxLength)) / eeprom.stepsPerMM;

                        UserInterface.logConsole("zMaxLength changed by: " + changeInMM);
                        UserInterface.logConsole("zMaxLength before: " + eeprom.zMaxLength);
                        UserInterface.logConsole("zMaxLength after: " + (eeprom.zMaxLength - changeInMM));

                        GCode.sendEEPROMVariable(3, 153, (eeprom.zMaxLength - changeInMM));
                        UserInterface.logConsole("Resetting Z Max Length\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

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
