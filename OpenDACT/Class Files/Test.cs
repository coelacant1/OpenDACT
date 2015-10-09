using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    class learnPrinter
    {
        private void testAdvanced(ref EEPROM eeprom, ref UserVariables userVariables, ref Heights heights)
        {
            if (userVariables.advancedCalibration == true)
            {
                //find base heights
                //find heights with each value increased by 1 - HRad, tower offset 1-3, diagonal rod

                if (userVariables.advancedCalCount == 0)
                {//start
                    if (Connection._serialPort.IsOpen)
                    {
                        //set diagonal rod +1
                        _serialPort.WriteLine("M206 T3 P881 X" + (eeprom.stepsPerMM + 1).ToString());
                        UserInterface.logConsole("Setting diagonal rod to: " + (eeprom.stepsPerMM + 1).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 1)
                {//get diagonal rod percentages

                    userVariables.deltaTower = ((heights.teX - heights.X) + (tempY - heights.Y) + (tempZ - heights.Z)) / 3;
                    userVariables.deltaOpp = ((tempXOpp - XOpp) + (tempYOpp - YOpp) + (tempZOpp - ZOpp)) / 3;

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset diagonal rod
                        _serialPort.WriteLine("M206 T3 P881 X" + (eeprom.stepsPerMM).ToString());
                        UserInterface.logConsole("Setting diagonal rod to: " + (eeprom.stepsPerMM).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set Hrad +1
                        _serialPort.WriteLine("M206 T3 P885 X" + (eeprom.HRadius + 1).ToString());
                        UserInterface.logConsole("Setting Horizontal Radius to: " + (eeprom.HRadius + 1).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 2)
                {//get HRad percentages
                    userVariables.HRadRatio = -(Math.Abs((X - heights.teX) + (Y - heights.teY) + (Z - heights.teZ) + (XOpp - heights.teXOpp) + (YOpp - heights.teYOpp) + (ZOpp - heights.teZOpp))) / 6;

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset horizontal radius
                        _serialPort.WriteLine("M206 T3 P885 X" + (HRad).ToString());
                        UserInterface.logConsole("Setting Horizontal Radius to: " + (HRad).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set X offset
                        _serialPort.WriteLine("M206 T1 P893 S" + (offsetX + 80).ToString());
                        UserInterface.logConsole("Setting offset X to: " + (offsetX + 80).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 3)
                {//get X offset percentages

                    userVariables.offsetXCorrection = Math.Abs(1 / (X - heights.teX));
                    userVariables.xxOppPerc = Math.Abs((XOpp - heights.teXOpp) / (X - heights.teX));
                    userVariables.xyPerc = Math.Abs((Y - heights.teY) / (X - heights.teX));
                    userVariables.xyOppPerc = Math.Abs((YOpp - heights.teYOpp) / (X - heights.teX));
                    userVariables.xzPerc = Math.Abs((Z - heights.teZ) / (X - heights.teX));
                    userVariables.xzOppPerc = Math.Abs((ZOpp - heights.teZOpp) / (X - heights.teX));

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset X offset
                        _serialPort.WriteLine("M206 T1 P893 S" + (offsetX).ToString());
                        UserInterface.logConsole("Setting offset X to: " + (offsetX).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set Y offset
                        _serialPort.WriteLine("M206 T1 P895 S" + (offsetY + 80).ToString());
                        UserInterface.logConsole("Setting offset Y to: " + (offsetY + 80).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 4)
                {//get Y offset percentages

                    userVariables.offsetYCorrection = Math.Abs(1 / (Y - heights.teY));
                    userVariables.yyOppPerc = Math.Abs((YOpp - heights.teYOpp) / (Y - heights.teY));
                    userVariables.yxPerc = Math.Abs((X - heights.teX) / (Y - heights.teY));
                    userVariables.yxOppPerc = Math.Abs((XOpp - heights.teXOpp) / (Y - heights.teY));
                    userVariables.yzPerc = Math.Abs((Z - heights.teZ) / (Y - heights.teY));
                    userVariables.yzOppPerc = Math.Abs((ZOpp - heights.teZOpp) / (Y - heights.teY));

                    if (Connection._serialPort.IsOpen)
                    {
                        //reset Y offset
                        _serialPort.WriteLine("M206 T1 P895 S" + (offsetY).ToString());
                        UserInterface.logConsole("Setting offset Y to: " + (offsetY).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set Z offset
                        _serialPort.WriteLine("M206 T1 P897 S" + (offsetZ + 80).ToString());
                        UserInterface.logConsole("Setting offset Z to: " + (offsetZ + 80).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 5)
                {//get Z offset percentages

                    userVariables.offsetZCorrection = Math.Abs(1 / (Z - heights.teZ));
                    userVariables.zzOppPerc = Math.Abs((ZOpp - heights.teZOpp) / (Z - heights.teZ));
                    userVariables.zxPerc = Math.Abs((X - heights.teX) / (Z - heights.teZ));
                    userVariables.zxOppPerc = Math.Abs((XOpp - heights.teXOpp) / (Z - heights.teZ));
                    userVariables.zyPerc = Math.Abs((Y - heights.teY) / (Z - heights.teZ));
                    userVariables.zyOppPerc = Math.Abs((YOpp - heights.teYOpp) / (Z - heights.teZ));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set Z offset
                        _serialPort.WriteLine("M206 T1 P897 S" + (offsetZ).ToString());
                        UserInterface.logConsole("Setting offset Z to: " + (offsetZ).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set alpha rotation offset perc X
                        _serialPort.WriteLine("M206 T3 P901 X" + (A + 1).ToString());
                        UserInterface.logConsole("Setting Alpha A to: " + (A + 1).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;

                }
                else if (userVariables.advancedCalCount == 6)//6
                {//get A alpha rotation

                    userVariables.alphaRotationPercentageX = (2 / Math.Abs((YOpp - ZOpp) - (tempYOpp - heights.teZOpp)));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc X
                        _serialPort.WriteLine("M206 T3 P901 X" + (A).ToString());
                        UserInterface.logConsole("Setting Alpha A to: " + (A).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set alpha rotation offset perc Y
                        _serialPort.WriteLine("M206 T3 P905 X" + (B + 1).ToString());
                        UserInterface.logConsole("Setting Alpha B to: " + (B + 1).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 7)//7
                {//get B alpha rotation

                    userVariables.alphaRotationPercentageY = (2 / Math.Abs((ZOpp - XOpp) - (tempZOpp - heights.teXOpp)));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc Y
                        _serialPort.WriteLine("M206 T3 P905 X" + (B).ToString());
                        UserInterface.logConsole("Setting Alpha B to: " + (B).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                        //set alpha rotation offset perc Z
                        _serialPort.WriteLine("M206 T3 P909 X" + (C + 1).ToString());
                        UserInterface.logConsole("Setting Alpha C to: " + (C + 1).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);
                    }

                    //check heights

                    userVariables.advancedCalCount++;
                }
                else if (userVariables.advancedCalCount == 8)//8
                {//get C alpha rotation

                    userVariables.alphaRotationPercentageZ = (2 / Math.Abs((XOpp - YOpp) - (tempXOpp - heights.teYOpp)));

                    if (Connection._serialPort.IsOpen)
                    {
                        //set alpha rotation offset perc Z
                        _serialPort.WriteLine("M206 T3 P909 X" + (C).ToString());
                        UserInterface.logConsole("Setting Alpha C to: " + (C).ToString() + "\n");
                        Thread.Sleep(userVariables.pauseTimeSet);

                    }

                    UserInterface.logConsole("Alpha offset percentages: " + alphaRotationPercentageX + ", " + alphaRotationPercentageY + ", and" + alphaRotationPercentageZ + "\n");

                    advancedCalibration = 0;
                    advancedCalCount = 0;

                    //check heights

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

            public static int userVariables.pauseTimeSet = 500;

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
            public static void setuserVariables.pauseTimeSet(int value)
            {
                userVariables.pauseTimeSet = value;
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
            public static int returnuserVariables.pauseTimeSet()
            {
                return userVariables.pauseTimeSet;
            }
        }
    }
