using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{
    class Calibration
    {
        UserInterface UserInterface;
        EEPROM EEPROM;
        HeightFunctions HeightFunctions;
        UserVariables UserVariables;
        Validation Validation;

        public Calibration(UserInterface _UserInterface, EEPROM _EEPROM, HeightFunctions _HeightFunctions, UserVariables _UserVariables, Validation _Validation)
        {
            this.UserInterface = _UserInterface;
            this.EEPROM = _EEPROM;
            this.HeightFunctions = _HeightFunctions;
            this.UserVariables = _UserVariables;
            this.Validation = _Validation;
        }

        

        public bool calibrationState = false;
        public int calibrationSelection = 0;

        public void calibrate(int value)
        {
            switch (value)
            {
                case 0:
                    basicCalibration();
                    break;
                case 1:
                    learningCalibration();
                    break;
                case 2:
                    iterativeCalibration();
                    break;
                case 3:
                    learningIterativeCalibration();
                    break;
            }
        }

        public void basicCalibration()
        {

            float X = Heights.returnX();
            float XOpp = Heights.returnXOpp();
            float Y = Heights.returnY();
            float YOpp = Heights.returnYOpp();
            float Z = Heights.returnZ();
            float ZOpp = Heights.returnZOpp();

            HRad(ref X, ref XOpp, ref Y, ref YOpp, ref Z, ref ZOpp);
            DRad(ref X, ref XOpp, ref Y, ref YOpp, ref Z, ref ZOpp);

            UserInterface.setHeightMap();
            /*
            Heights.setX(X);
            Heights.setXOpp(XOpp);
            Heights.setY(Y);
            Heights.setYOpp(YOpp);
            Heights.setZ(Z);
            Heights.setZOpp(ZOpp);
            */
        }
        public void learningCalibration()
        {

        }

        public void iterativeCalibration()
        {

        }
        public void learningIterativeCalibration()
        {

        }

        public void HRad(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);
            float HRadRatio = UserVariables.returnHRadRatio();
            float HRadius = EEPROM.returnHRadius();

            HRadius = HRadius + (HRadSA / HRadRatio);

            X -= HRadSA;
            Y -= HRadSA;
            Z -= HRadSA;
            XOpp -= HRadSA;
            YOpp -= HRadSA;
            ZOpp -= HRadSA;

            UserInterface.logConsole("HRad:" + EEPROM.HRadius.ToString() + "\n");

            EEPROM.setHRadius(HRadius);
        }

        public float[] DRad(ref float X, ref float XOpp, ref float Y, ref float YOpp, ref float Z, ref float ZOpp)
        {
            float DASA = ((X + XOpp) / 2);
            float DBSA = ((Y + YOpp) / 2);
            float DCSA = ((Z + ZOpp) / 2);
            float DRadRatio = UserVariables.DRadRatio;
            
            float DA = EEPROM.DA + ((DASA) / DRadRatio);
            float DB = EEPROM.DB + ((DBSA) / DRadRatio);
            float DC = EEPROM.DC + ((DCSA) / DRadRatio);

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
            
            EEPROM.setDA(DA);
            EEPROM.setDB(DB);
            EEPROM.setDC(DC);

            UserInterface.logConsole("Delta Radii Offsets: " + DA.ToString() + ", " + DB.ToString() + ", " + DC.ToString());

            Connection._serialPort.WriteLine("M206 T3 P913 X" + Validation.ToLongString(DA));
            Thread.Sleep(UserVariables.pauseTimeSet);
            Connection._serialPort.WriteLine("M206 T3 P917 X" + Validation.ToLongString(DB));
            Thread.Sleep(UserVariables.pauseTimeSet);
            Connection._serialPort.WriteLine("M206 T3 P921 X" + Validation.ToLongString(DC));
            Thread.Sleep(UserVariables.pauseTimeSet);


            float[] heights = { 1 };
            return heights;
        }

        public void analyzeGeometry(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            int analyzeCount = 0;


            UserInterface.logConsole("Expect a slight inaccuracy in the geometry analysis; basic calibration.");
        }

        public void towerOffsets(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            int j = 0;
            float accuracy = 0.001F;
            float tempX2 = X;
            float tempXOpp2 = XOpp;
            float tempY2 = Y;
            float tempYOpp2 = YOpp;
            float tempZ2 = Z;
            float tempZOpp2 = ZOpp;
            float offsetX = EEPROM.returnOffsetX();
            float offsetY = EEPROM.returnOffsetY();
            float offsetZ = EEPROM.returnOffsetZ();
            float stepsPerMM = EEPROM.returnSPM();

            //
            float offsetXCorrection = 1.5F;
            float offsetYCorrection = 1.5F;
            float offsetZCorrection = 1.5F;
            float xxOppPerc = 0.5F;
            float xyPerc = 0.25F;
            float xyOppPerc = 0.25F;
            float xzPerc = 0.25F;
            float xzOppPerc = 0.25F;
            float yyOppPerc = 0.5F;
            float yxPerc = 0.25F;
            float yxOppPerc = 0.25F;
            float yzPerc = 0.25F;
            float yzOppPerc = 0.25F;
            float zzOppPerc = 0.5F;
            float zxPerc = 0.25F;
            float zxOppPerc = 0.25F;
            float zyPerc = 0.25F;
            float zyOppPerc = 0.25F;

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
                UserInterface.logConsole("XYZ offsets before damage prevention: X" + offsetX + " Y" + offsetY + " Z" + offsetZ + "\n");
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
            offsetX = Convert.ToInt32(offsetX);
            offsetY = Convert.ToInt32(offsetY);
            offsetZ = Convert.ToInt32(offsetZ);

            UserInterface.logConsole("XYZ:" + offsetX + " " + offsetY + " " + offsetZ + "\n");

            //send data back to printer
            Connection._serialPort.WriteLine("M206 T1 P893 S" + offsetX.ToString());
            Thread.Sleep(UserVariables.returnPauseTimeSet());
            Connection._serialPort.WriteLine("M206 T1 P895 S" + offsetY.ToString());
            Thread.Sleep(UserVariables.returnPauseTimeSet());
            Connection._serialPort.WriteLine("M206 T1 P897 S" + offsetZ.ToString());
            Thread.Sleep(UserVariables.returnPauseTimeSet());
        }
        
        public void alphaRotation(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            if (offsetX != 0 && offsetY != 0 && offsetZ != 0)
            {
                int k = 0;
                while (k < 100)
                {
                    //X Alpha Rotation
                    if (YOpp > ZOpp)
                    {
                        float ZYOppAvg = (YOpp - ZOpp) / 2;
                        A = A + (ZYOppAvg * alphaRotationPercentageX); // (0.5/((diff y0 and z0 at X + 0.5)-(diff y0 and z0 at X = 0))) * 2 = 1.75
                        YOpp = YOpp - ZYOppAvg;
                        ZOpp = ZOpp + ZYOppAvg;
                    }
                    else if (YOpp < ZOpp)
                    {
                        float ZYOppAvg = (ZOpp - YOpp) / 2;

                        A = A - (ZYOppAvg * alphaRotationPercentageX);
                        YOpp = YOpp + ZYOppAvg;
                        ZOpp = ZOpp - ZYOppAvg;
                    }

                    //Y Alpha Rotation
                    if (ZOpp > XOpp)
                    {
                        float XZOppAvg = (ZOpp - XOpp) / 2;
                        B = B + (XZOppAvg * alphaRotationPercentageY);
                        ZOpp = ZOpp - XZOppAvg;
                        XOpp = XOpp + XZOppAvg;
                    }
                    else if (ZOpp < XOpp)
                    {
                        float XZOppAvg = (XOpp - ZOpp) / 2;

                        B = B - (XZOppAvg * alphaRotationPercentageY);
                        ZOpp = ZOpp + XZOppAvg;
                        XOpp = XOpp - XZOppAvg;
                    }
                    //Z Alpha Rotation
                    if (XOpp > YOpp)
                    {
                        float YXOppAvg = (XOpp - YOpp) / 2;
                        C = C + (YXOppAvg * alphaRotationPercentageZ);
                        XOpp = XOpp - YXOppAvg;
                        YOpp = YOpp + YXOppAvg;
                    }
                    else if (XOpp < YOpp)
                    {
                        float YXOppAvg = (YOpp - XOpp) / 2;

                        C = C - (YXOppAvg * alphaRotationPercentageZ);
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
                }

                //log
                LogConsole("ABC:" + A + " " + B + " " + C + "\n");

                //send obtained values back to the printer*************************************
                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("M206 T3 P901 X" + checkZero(A).ToString());
                LogConsole("Setting A Rotation\n");
                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("M206 T3 P905 X" + checkZero(B).ToString());
                LogConsole("Setting B Rotation\n");
                Thread.Sleep(pauseTimeSet);
                _serialPort.WriteLine("M206 T3 P909 X" + checkZero(C).ToString());
                LogConsole("Setting C Rotation\n");
                Thread.Sleep(pauseTimeSet);
            }
        }

        public void SPM(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {

            //opp = 0.21; //4/5
            //tower = 0.27; //9/32

            float diagChange = 1 / deltaOpp;
            float towOppDiff = deltaTower / deltaOpp; //0.5
            float XYZ = (X + Y + Z) / 3;
            float XYZOpp = (XOpp + YOpp + ZOpp) / 3;

            LogConsole(X.ToString() + " " + XOpp.ToString() + " " + Y.ToString() + " " + YOpp.ToString() + " " + Z.ToString() + " " + ZOpp.ToString());

            if (Math.Abs(XYZOpp - XYZ) > accuracy * 2)
            {
                int i = 0;
                while (i < 100)
                {
                    XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                    stepsPerMM = stepsPerMM + (XYZOpp * diagChange);

                    X = X - towOppDiff * XYZOpp;
                    Y = Y - towOppDiff * XYZOpp;
                    Z = Z - towOppDiff * XYZOpp;
                    XOpp = XOpp - XYZOpp;
                    YOpp = YOpp - XYZOpp;
                    ZOpp = ZOpp - XYZOpp;

                    XYZOpp = (XOpp + YOpp + ZOpp) / 3;
                    XYZOpp = checkZero(XYZOpp);

                    //HRAD recalibration
                    XYZ = (X + Y + Z) / 3;
                    HRad = HRad + ((XYZOpp * diagChange) / HRadRatio);

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

                    X = checkZero(X);
                    Y = checkZero(Y);
                    Z = checkZero(Z);
                    XOpp = checkZero(XOpp);
                    YOpp = checkZero(YOpp);
                    ZOpp = checkZero(ZOpp);

                    //XYZ is zero
                    if (XYZOpp < accuracy && XYZOpp > -accuracy && XYZ < accuracy && XYZ > -accuracy)
                    {
                        //end calculation
                        stepsPerMM = checkZero(stepsPerMM);

                        float changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / tempSPM;

                        LogConsole("zMaxLength changed by: " + changeInMM);
                        LogConsole("zMaxLength before: " + centerHeight);
                        LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                        float tempChange = centerHeight - changeInMM;

                        _serialPort.WriteLine("M206 T3 P153 X" + tempChange.ToString());
                        LogConsole("Resetting Z Max Length\n");
                        Thread.Sleep(pauseTimeSet);

                        _serialPort.WriteLine("M206 T3 P11 X" + stepsPerMM.ToString());
                        LogConsole("Steps Per Millimeter Changed: " + stepsPerMM.ToString());
                        Thread.Sleep(pauseTimeSet);

                        i = 100;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (HRad < 250 && HRad > 50)
                {
                    //send obtained values back to the printer*************************************
                    LogConsole("HRad Recalibration:" + HRad + "\n");
                    _serialPort.WriteLine("M206 T3 P885 X" + checkZero(HRad).ToString());
                    LogConsole("Setting Horizontal Radius\n");
                    Thread.Sleep(pauseTimeSet);
                }
                else
                {
                    LogConsole("Horizontal radius not set\n");
                }
            }

        }
    }
}
