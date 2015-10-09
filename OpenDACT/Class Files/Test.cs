using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class advanced
    {
        private void testAdvanced(ref EEPROM eeprom, ref UserVariables userVariables, ref Heights heights)
        {
            if (advancedCalibration == 1)
            {
                //find base heights
                //find heights with each value increased by 1 - HRad, tower offset 1-3, diagonal rod

                if (advancedCalCount == 0)
                {//start
                    if (_serialPort.IsOpen)
                    {
                        //set diagonal rod +1
                        _serialPort.WriteLine("M206 T3 P881 X" + (diagonalRod + 1).ToString());
                        LogConsole("Setting diagonal rod to: " + (diagonalRod + 1).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;
                }
                else if (advancedCalCount == 1)
                {//get diagonal rod percentages

                    deltaTower = ((tempX - X) + (tempY - Y) + (tempZ - Z)) / 3;
                    deltaOpp = ((tempXOpp - XOpp) + (tempYOpp - YOpp) + (tempZOpp - ZOpp)) / 3;

                    if (_serialPort.IsOpen)
                    {
                        //reset diagonal rod
                        _serialPort.WriteLine("M206 T3 P881 X" + (diagonalRod).ToString());
                        LogConsole("Setting diagonal rod to: " + (diagonalRod).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                        //set Hrad +1
                        _serialPort.WriteLine("M206 T3 P885 X" + (HRad + 1).ToString());
                        LogConsole("Setting Horizontal Radius to: " + (HRad + 1).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;
                }
                else if (advancedCalCount == 2)
                {//get HRad percentages
                    HRadRatio = -(Math.Abs((X - tempX) + (Y - tempY) + (Z - tempZ) + (XOpp - tempXOpp) + (YOpp - tempYOpp) + (ZOpp - tempZOpp))) / 6;

                    if (_serialPort.IsOpen)
                    {
                        //reset horizontal radius
                        _serialPort.WriteLine("M206 T3 P885 X" + (HRad).ToString());
                        LogConsole("Setting Horizontal Radius to: " + (HRad).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                        //set X offset
                        _serialPort.WriteLine("M206 T1 P893 S" + (offsetX + 80).ToString());
                        LogConsole("Setting offset X to: " + (offsetX + 80).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;
                }
                else if (advancedCalCount == 3)
                {//get X offset percentages

                    offsetXCorrection = Math.Abs(1 / (X - tempX));
                    xxOppPerc = Math.Abs((XOpp - tempXOpp) / (X - tempX));
                    xyPerc = Math.Abs((Y - tempY) / (X - tempX));
                    xyOppPerc = Math.Abs((YOpp - tempYOpp) / (X - tempX));
                    xzPerc = Math.Abs((Z - tempZ) / (X - tempX));
                    xzOppPerc = Math.Abs((ZOpp - tempZOpp) / (X - tempX));

                    if (_serialPort.IsOpen)
                    {
                        //reset X offset
                        _serialPort.WriteLine("M206 T1 P893 S" + (offsetX).ToString());
                        LogConsole("Setting offset X to: " + (offsetX).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                        //set Y offset
                        _serialPort.WriteLine("M206 T1 P895 S" + (offsetY + 80).ToString());
                        LogConsole("Setting offset Y to: " + (offsetY + 80).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;
                }
                else if (advancedCalCount == 4)
                {//get Y offset percentages

                    offsetYCorrection = Math.Abs(1 / (Y - tempY));
                    yyOppPerc = Math.Abs((YOpp - tempYOpp) / (Y - tempY));
                    yxPerc = Math.Abs((X - tempX) / (Y - tempY));
                    yxOppPerc = Math.Abs((XOpp - tempXOpp) / (Y - tempY));
                    yzPerc = Math.Abs((Z - tempZ) / (Y - tempY));
                    yzOppPerc = Math.Abs((ZOpp - tempZOpp) / (Y - tempY));

                    if (_serialPort.IsOpen)
                    {
                        //reset Y offset
                        _serialPort.WriteLine("M206 T1 P895 S" + (offsetY).ToString());
                        LogConsole("Setting offset Y to: " + (offsetY).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                        //set Z offset
                        _serialPort.WriteLine("M206 T1 P897 S" + (offsetZ + 80).ToString());
                        LogConsole("Setting offset Z to: " + (offsetZ + 80).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;
                }
                else if (advancedCalCount == 5)
                {//get Z offset percentages

                    offsetZCorrection = Math.Abs(1 / (Z - tempZ));
                    zzOppPerc = Math.Abs((ZOpp - tempZOpp) / (Z - tempZ));
                    zxPerc = Math.Abs((X - tempX) / (Z - tempZ));
                    zxOppPerc = Math.Abs((XOpp - tempXOpp) / (Z - tempZ));
                    zyPerc = Math.Abs((Y - tempY) / (Z - tempZ));
                    zyOppPerc = Math.Abs((YOpp - tempYOpp) / (Z - tempZ));

                    if (_serialPort.IsOpen)
                    {
                        //set Z offset
                        _serialPort.WriteLine("M206 T1 P897 S" + (offsetZ).ToString());
                        LogConsole("Setting offset Z to: " + (offsetZ).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                        //set alpha rotation offset perc X
                        _serialPort.WriteLine("M206 T3 P901 X" + (A + 1).ToString());
                        LogConsole("Setting Alpha A to: " + (A + 1).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;

                }
                else if (advancedCalCount == 6)//6
                {//get A alpha rotation

                    alphaRotationPercentageX = (2 / Math.Abs((YOpp - ZOpp) - (tempYOpp - tempZOpp)));

                    if (_serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc X
                        _serialPort.WriteLine("M206 T3 P901 X" + (A).ToString());
                        LogConsole("Setting Alpha A to: " + (A).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                        //set alpha rotation offset perc Y
                        _serialPort.WriteLine("M206 T3 P905 X" + (B + 1).ToString());
                        LogConsole("Setting Alpha B to: " + (B + 1).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;
                }
                else if (advancedCalCount == 7)//7
                {//get B alpha rotation

                    alphaRotationPercentageY = (2 / Math.Abs((ZOpp - XOpp) - (tempZOpp - tempXOpp)));

                    if (_serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc Y
                        _serialPort.WriteLine("M206 T3 P905 X" + (B).ToString());
                        LogConsole("Setting Alpha B to: " + (B).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                        //set alpha rotation offset perc Z
                        _serialPort.WriteLine("M206 T3 P909 X" + (C + 1).ToString());
                        LogConsole("Setting Alpha C to: " + (C + 1).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);
                    }

                    initiateCal();

                    advancedCalCount++;
                }
                else if (advancedCalCount == 8)//8
                {//get C alpha rotation

                    alphaRotationPercentageZ = (2 / Math.Abs((XOpp - YOpp) - (tempXOpp - tempYOpp)));

                    if (_serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc Z
                        _serialPort.WriteLine("M206 T3 P909 X" + (C).ToString());
                        LogConsole("Setting Alpha C to: " + (C).ToString() + "\n");
                        Thread.Sleep(pauseTimeSet);

                    }

                    LogConsole("Alpha offset percentages: " + alphaRotationPercentageX + ", " + alphaRotationPercentageY + ", and" + alphaRotationPercentageZ + "\n");

                    advancedCalibration = 0;
                    advancedCalCount = 0;

                    initiateCal();

                    setAdvancedCalVars();
                }
            }
        }

        class Heightsbackup
        {
            //store every set of heights
            public static float center;
            public static float X;
            public static float XOpp;
            public static float Y;
            public static float YOpp;
            public static float Z;
            public static float ZOpp;

            //set
            public static void setCenter(float value)
            {
                value = Validation.checkZero(value);
                center = value;
            }
            public static void setX(float value)
            {
                value = Validation.checkZero(value);
                X = value;
            }
            public static void setXOpp(float value)
            {
                value = Validation.checkZero(value);
                XOpp = value;
            }
            public static void setY(float value)
            {
                value = Validation.checkZero(value);
                Y = value;
            }
            public static void setYOpp(float value)
            {
                value = Validation.checkZero(value);
                YOpp = value;
            }
            public static void setZ(float value)
            {
                value = Validation.checkZero(value);
                Z = value;
            }
            public static void setZOpp(float value)
            {
                value = Validation.checkZero(value);
                ZOpp = value;
            }

            //return
            public static float returnCenter()
            {
                return center;
            }
            public static float returnX()
            {
                return X;
            }
            public static float returnXOpp()
            {
                return XOpp;
            }
            public static float returnY()
            {
                return Y;
            }
            public static float returnYOpp()
            {
                return YOpp;
            }
            public static float returnZ()
            {
                return Z;
            }
            public static float returnZOpp()
            {
                return ZOpp;
            }
        }


        class EEPROMbackup
        {
            public static float stepsPerMM;
            public static float tempSPM;
            public static float zMaxLength;
            public static float zProbe;
            public static float HRadius;
            public static float offsetX;
            public static float offsetY;
            public static float offsetZ;
            public static float A;
            public static float B;
            public static float C;
            public static float DA;
            public static float DB;
            public static float DC;

            //set
            public static void setSPM(float value)
            {
                value = Validation.checkZero(value);
                stepsPerMM = value;
            }
            public static void setTempSPM(float value)
            {
                value = Validation.checkZero(value);
                tempSPM = value;
            }
            public static void setZMaxLength(float value)
            {
                value = Validation.checkZero(value);
                zMaxLength = value;
            }
            public static void setZProbe(float value)
            {
                value = Validation.checkZero(value);
                zProbe = value;
            }
            public static void setHRadius(float value)
            {
                value = Validation.checkZero(value);
                HRadius = value;
            }
            public static void setOffsetX(float value)
            {
                value = Validation.checkZero(value);
                offsetX = value;
            }
            public static void setOffsetY(float value)
            {
                value = Validation.checkZero(value);
                offsetY = value;
            }
            public static void setOffsetZ(float value)
            {
                value = Validation.checkZero(value);
                offsetZ = value;
            }
            public static void setA(float value)
            {
                value = Validation.checkZero(value);
                A = value;
            }
            public static void setB(float value)
            {
                value = Validation.checkZero(value);
                B = value;
            }
            public static void setC(float value)
            {
                value = Validation.checkZero(value);
                C = value;
            }
            public static void setDA(float value)
            {
                value = Validation.checkZero(value);
                DA = value;
            }
            public static void setDB(float value)
            {
                value = Validation.checkZero(value);
                DB = value;
            }
            public static void setDC(float value)
            {
                value = Validation.checkZero(value);
                DC = value;
            }

            //return
            public static float returnSPM()
            {
                return stepsPerMM;
            }
            public static float returnTempSPM()
            {
                return tempSPM;
            }
            public static float returnZMaxLength()
            {
                return zMaxLength;
            }
            public static float returnZProbe()
            {
                return zProbe;
            }
            public static float returnHRadius()
            {
                return HRadius;
            }
            public static float returnOffsetX()
            {
                return offsetX;
            }
            public static float returnOffsetY()
            {
                return offsetY;
            }
            public static float returnOffsetZ()
            {
                return offsetZ;
            }
            public static float returnA()
            {
                return A;
            }
            public static float returnB()
            {
                return B;
            }
            public static float returnC()
            {
                return C;
            }
            public static float returnDA()
            {
                return DA;
            }
            public static float returnDB()
            {
                return DB;
            }
            public static float returnDC()
            {
                return DC;
            }
        }


        class UserVariablesbackup
        {
            //misc vars, alpha offsets, tower offsets, spm offsets, hrad offsets, drad offsets
            public static float HRadRatio;
            public static float DRadRatio;
            public static float accuracy = 0.025F;
            public static float probingHeight = 75;
            public static float offsetXCorrection = 1.5F;
            public static float offsetYCorrection = 1.5F;
            public static float offsetZCorrection = 1.5F;
            public static float xxOppPerc = 0.5F;
            public static float xyPerc = 0.25F;
            public static float xyOppPerc = 0.25F;
            public static float xzPerc = 0.25F;
            public static float xzOppPerc = 0.25F;
            public static float yyOppPerc = 0.5F;
            public static float yxPerc = 0.25F;
            public static float yxOppPerc = 0.25F;
            public static float yzPerc = 0.25F;
            public static float yzOppPerc = 0.25F;
            public static float zzOppPerc = 0.5F;
            public static float zxPerc = 0.25F;
            public static float zxOppPerc = 0.25F;
            public static float zyPerc = 0.25F;
            public static float zyOppPerc = 0.25F;
            public static float alphaRotationPercentageX = 1.725F;
            public static float alphaRotationPercentageY = 1.725F;
            public static float alphaRotationPercentageZ = 1.725F;

            public static int pauseTimeSet = 500;

            public static void setHRadRatio(float value)
            {
                value = Validation.checkZero(value);
                HRadRatio = value;
            }
            public static void setDRadRatio(float value)
            {
                value = Validation.checkZero(value);
                DRadRatio = value;
            }
            public static void setAccuracy(float value)
            {
                value = Validation.checkZero(value);
                accuracy = value;
            }
            public static void setOffsetXCorrection(float ioffsetXCorrection, float ixxOppPerc, float ixyPerc, float ixyOppPerc, float ixzPerc, float ixzOppPerc)
            {
                offsetXCorrection = ioffsetXCorrection;
                xxOppPerc = ixxOppPerc;
                xyPerc = ixyPerc;
                xyOppPerc = ixyOppPerc;
                xzPerc = ixzPerc;
                xzOppPerc = ixzOppPerc;

            }
            public static void setOffsetYCorrection(float ioffsetYCorrection, float iyyOppPerc, float iyxPerc, float iyxOppPerc, float iyzPerc, float iyzOppPerc)
            {
                offsetYCorrection = ioffsetYCorrection;
                yyOppPerc = iyyOppPerc;
                yxPerc = iyxPerc;
                yxOppPerc = iyxOppPerc;
                yzPerc = iyzPerc;
                yzOppPerc = iyzOppPerc;
            }
            public static void setOffsetZCorrection(float ioffsetZCorrection, float izzOppPerc, float izxPerc, float izxOppPerc, float izyPerc, float izyOppPerc)
            {
                offsetZCorrection = ioffsetZCorrection;
                zzOppPerc = izzOppPerc;
                zxPerc = izxPerc;
                zxOppPerc = izxOppPerc;
                zyPerc = izyPerc;
                zyOppPerc = izyOppPerc;
            }
            public static void setAlphaRotationPercentageX(float value)
            {
                value = Validation.checkZero(value);
                alphaRotationPercentageX = value;
            }
            public static void setAlphaRotationPercentageY(float value)
            {
                value = Validation.checkZero(value);
                alphaRotationPercentageY = value;
            }
            public static void setAlphaRotationPercentageZ(float value)
            {
                value = Validation.checkZero(value);
                alphaRotationPercentageZ = value;
            }
            public static void setPauseTimeSet(int value)
            {
                pauseTimeSet = value;
            }


            public static float returnHRadRatio()
            {
                return HRadRatio;
            }
            public static float returnDRadRatio()
            {
                return DRadRatio;
            }
            public static float returnUserAccuracy()
            {
                return accuracy;
            }
            public static void returnOffsetXCorrection(out float ioffsetXCorrection, out float ixxOppPerc, out float ixyPerc, out float ixyOppPerc, out float ixzPerc, out float ixzOppPerc)
            {
                ioffsetXCorrection = offsetXCorrection;
                ixxOppPerc = xxOppPerc;
                ixyPerc = xyPerc;
                ixyOppPerc = xyOppPerc;
                ixzPerc = xzPerc;
                ixzOppPerc = xzOppPerc;

            }
            public static void returnOffsetYCorrection(out float ioffsetYCorrection, out float iyyOppPerc, out float iyxPerc, out float iyxOppPerc, out float iyzPerc, out float iyzOppPerc)
            {
                ioffsetYCorrection = offsetYCorrection;
                iyyOppPerc = yyOppPerc;
                iyxPerc = yxPerc;
                iyxOppPerc = yxOppPerc;
                iyzPerc = yzPerc;
                iyzOppPerc = yzOppPerc;
            }
            public static void returnOffsetZCorrection(out float ioffsetZCorrection, out float izzOppPerc, out float izxPerc, out float izxOppPerc, out float izyPerc, out float izyOppPerc)
            {
                ioffsetZCorrection = offsetZCorrection;
                izzOppPerc = zzOppPerc;
                izxPerc = zxPerc;
                izxOppPerc = zxOppPerc;
                izyPerc = zyPerc;
                izyOppPerc = zyOppPerc;
            }
            public static void returnAlphaRotationPercentageX(out float ialphaRotPercX)
            {
                ialphaRotPercX = alphaRotationPercentageX;
            }
            public static void returnAlphaRotationPercentageY(out float ialphaRotPercY)
            {
                ialphaRotPercY = alphaRotationPercentageY;
            }
            public static void returnAlphaRotationPercentageZ(out float ialphaRotPercZ)
            {
                ialphaRotPercZ = alphaRotationPercentageZ;
            }
            public static int returnPauseTimeSet()
            {
                return pauseTimeSet;
            }
        }
    }
