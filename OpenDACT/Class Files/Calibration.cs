using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class Calibration
    {
        UserInterface UserInterface;
        EEPROM EEPROM;
        Heights Heights;

        public Calibration(UserInterface _UserInterface, EEPROM _EEPROM, Heights _Heights)
        {
            this.UserInterface = _UserInterface;
            this.EEPROM = _EEPROM;
            this.Heights = _Heights;
        }


        public void basicCalibration()
        {

        }
        public void heuristicCalibration()
        {

        }

        public void normalCalibration()
        {

        }

        public void advancedCalibration()
        {

        }

        public float[] HRad(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            double HRadSA = ((X + XOpp + Y + YOpp + Z + ZOpp) / 6);

            HRad = HRad + (HRadSA / HRadRatio);

            X = X - HRadSA;
            Y = Y - HRadSA;
            Z = Z - HRadSA;
            XOpp = XOpp - HRadSA;
            YOpp = YOpp - HRadSA;
            ZOpp = ZOpp - HRadSA;

            X = checkZero(X);
            Y = checkZero(Y);
            Z = checkZero(Z);
            XOpp = checkZero(XOpp);
            YOpp = checkZero(YOpp);
            ZOpp = checkZero(ZOpp);

            LogConsole("HRad:" + HRad + "\n");

            float[] heights = { 1 };
            return heights;
        }

        public float[] DRad(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            DASA = ((X + XOpp) / 2);
            DBSA = ((Y + YOpp) / 2);
            DCSA = ((Z + ZOpp) / 2);

            DA = DA + ((DASA) / HRadRatio);
            DB = DB + ((DBSA) / HRadRatio);
            DC = DC + ((DCSA) / HRadRatio);

            X = X + ((DASA) / HRadRatio) * 0.5;
            XOpp = XOpp + ((DASA) / HRadRatio) * 0.225;
            Y = Y + ((DASA) / HRadRatio) * 0.1375;
            YOpp = YOpp + ((DASA) / HRadRatio) * 0.1375;
            Z = Z + ((DASA) / HRadRatio) * 0.1375;
            ZOpp = ZOpp + ((DASA) / HRadRatio) * 0.1375;

            X = X + ((DBSA) / HRadRatio) * 0.1375;
            XOpp = XOpp + ((DBSA) / HRadRatio) * 0.1375;
            Y = Y + ((DBSA) / HRadRatio) * 0.5;
            YOpp = YOpp + ((DBSA) / HRadRatio) * 0.225;
            Z = Z + ((DBSA) / HRadRatio) * 0.1375;
            ZOpp = ZOpp + ((DBSA) / HRadRatio) * 0.1375;

            X = X + ((DCSA) / HRadRatio) * 0.1375;
            XOpp = XOpp + ((DCSA) / HRadRatio) * 0.1375;
            Y = Y + ((DCSA) / HRadRatio) * 0.1375;
            YOpp = YOpp + ((DCSA) / HRadRatio) * 0.1375;
            Z = Z + ((DCSA) / HRadRatio) * 0.5;
            ZOpp = ZOpp + ((DCSA) / HRadRatio) * 0.225;

            DA = checkZero(DA);
            DB = checkZero(DB);
            DC = checkZero(DC);

            LogConsole("Delta Radii Offsets: " + DA.ToString() + ", " + DB.ToString() + ", " + DC.ToString());

            _serialPort.WriteLine("M206 T3 P913 X" + ToLongString(DA));
            Thread.Sleep(pauseTimeSet);
            _serialPort.WriteLine("M206 T3 P917 X" + ToLongString(DB));
            Thread.Sleep(pauseTimeSet);
            _serialPort.WriteLine("M206 T3 P921 X" + ToLongString(DC));
            Thread.Sleep(pauseTimeSet);


            float[] heights = { 1 };
            return heights;
        }

        public void analyzeGeometry(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            analyzeCount++;
            analyzeGeometry();

            LogConsole("Expect a slight inaccuracy in the geometry analysis; basic calibration.");
        }

        public float[] towerOffsets(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            int j = 0;
            tempX2 = X;
            tempXOpp2 = XOpp;
            tempY2 = Y;
            tempYOpp2 = YOpp;
            tempZ2 = Z;
            tempZOpp2 = ZOpp;

            while (j < 100)
            {
                double theoryX = offsetX + tempX2 * stepsPerMM * offsetXCorrection;

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
                    tempZ2 = tempZ2 - (tempX2 * 2 * yxPerc);
                    tempXOpp2 = tempXOpp2 + (tempX2 * 2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 + (tempX2 * 2 * yxOppPerc);
                    tempY2 = tempY2 + tempX2 * 2;

                    tempZOpp2 = tempZOpp2 - (tempX2 * 2 * zzOppPerc);
                    tempX2 = tempX2 - (tempX2 * 2 * zxPerc);
                    tempY2 = tempY2 - (tempX2 * 2 * zyPerc);
                    tempXOpp2 = tempXOpp2 + (tempX2 * 2 * yxOppPerc);
                    tempYOpp2 = tempYOpp2 + (tempX2 * 2 * zyOppPerc);
                    tempZ2 = tempZ2 + tempX2 * 2;
                }

                double theoryY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                //Y
                if (tempY2 > 0)
                {
                    offsetY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                    tempYOpp2 = tempYOpp2 + (tempY2 * yyOppPerc);
                    tempX2 = tempX2 + (tempY2 * yxPerc);
                    tempZ2 = tempZ2 + (tempY2 * yxPerc);
                    tempXOpp2 = tempXOpp2 - (tempY2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 - (tempY2 * yxOppPerc);
                    tempY2 = tempY2 - tempY2;
                }
                else if (theoryY > 0 && tempY2 < 0)
                {
                    offsetY = offsetY + tempY2 * stepsPerMM * offsetYCorrection;

                    tempYOpp2 = tempYOpp2 + (tempY2 * yyOppPerc);
                    tempX2 = tempX2 + (tempY2 * yxPerc);
                    tempZ2 = tempZ2 + (tempY2 * yxPerc);
                    tempXOpp2 = tempXOpp2 - (tempY2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 - (tempY2 * yxOppPerc);
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
                    tempXOpp2 = tempXOpp2 + (tempY2 * 2 * yxOppPerc);
                    tempYOpp2 = tempYOpp2 + (tempY2 * 2 * zyOppPerc);
                    tempZ2 = tempZ2 + tempY2 * 2;
                }

                double theoryZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                //Z
                if (tempZ2 > 0)
                {
                    offsetZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                    tempZOpp2 = tempZOpp2 + (tempZ2 * zzOppPerc);
                    tempX2 = tempX2 + (tempZ2 * zxPerc);
                    tempY2 = tempY2 + (tempZ2 * zyPerc);
                    tempXOpp2 = tempXOpp2 - (tempZ2 * yxOppPerc);
                    tempYOpp2 = tempYOpp2 - (tempZ2 * zyOppPerc);
                    tempZ2 = tempZ2 - tempZ2;
                }
                else if (theoryZ > 0 && tempZ2 < 0)
                {
                    offsetZ = offsetZ + tempZ2 * stepsPerMM * offsetZCorrection;

                    tempZOpp2 = tempZOpp2 + (tempZ2 * zzOppPerc);
                    tempX2 = tempX2 + (tempZ2 * zxPerc);
                    tempY2 = tempY2 + (tempZ2 * zyPerc);
                    tempXOpp2 = tempXOpp2 - (tempZ2 * yxOppPerc);
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
                    tempZ2 = tempZ2 - (tempZ2 * 2 * yxPerc);
                    tempXOpp2 = tempXOpp2 + (tempZ2 * 2 * yxOppPerc);
                    tempZOpp2 = tempZOpp2 + (tempZ2 * 2 * yxOppPerc);
                    tempY2 = tempY2 + tempZ2 * 2;
                }

                tempX2 = checkZero(tempX2);
                tempY2 = checkZero(tempY2);
                tempZ2 = checkZero(tempZ2);
                tempXOpp2 = checkZero(tempXOpp2);
                tempYOpp2 = checkZero(tempYOpp2);
                tempZOpp2 = checkZero(tempZOpp2);

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
                    offsetXCorrection = 1.5;
                    xxOppPerc = 0.5;
                    xyPerc = 0.25;
                    xyOppPerc = 0.25;
                    xzPerc = 0.25;
                    xzOppPerc = 0.25;

                    //Y
                    offsetYCorrection = 1.5;
                    yyOppPerc = 0.5;
                    yxPerc = 0.25;
                    yxOppPerc = 0.25;
                    yzPerc = 0.25;
                    yzOppPerc = 0.25;

                    //Z
                    offsetZCorrection = 1.5;
                    zzOppPerc = 0.5;
                    zxPerc = 0.25;
                    zxOppPerc = 0.25;
                    zyPerc = 0.25;
                    zyOppPerc = 0.25;

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
                LogConsole("XYZ offset calibration error, setting default values.");
                LogConsole("XYZ offsets before damage prevention: X" + offsetX + " Y" + offsetY + " Z" + offsetZ + "\n");
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
            offsetX = Math.Round(offsetX);
            offsetY = Math.Round(offsetY);
            offsetZ = Math.Round(offsetZ);

            LogConsole("XYZ:" + offsetX + " " + offsetY + " " + offsetZ + "\n");

            //send data back to printer
            _serialPort.WriteLine("M206 T1 P893 S" + offsetX.ToString());
            Thread.Sleep(pauseTimeSet);
            _serialPort.WriteLine("M206 T1 P895 S" + offsetY.ToString());
            Thread.Sleep(pauseTimeSet);
            _serialPort.WriteLine("M206 T1 P897 S" + offsetZ.ToString());
            Thread.Sleep(pauseTimeSet);


            float[] heights = { 1 };
            return heights;
        }
        
        public float[] alphaRotation(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {
            if (offsetX != 0 && offsetY != 0 && offsetZ != 0)
            {
                int k = 0;
                while (k < 100)
                {
                    //X Alpha Rotation
                    if (YOpp > ZOpp)
                    {
                        double ZYOppAvg = (YOpp - ZOpp) / 2;
                        A = A + (ZYOppAvg * alphaRotationPercentageX); // (0.5/((diff y0 and z0 at X + 0.5)-(diff y0 and z0 at X = 0))) * 2 = 1.75
                        YOpp = YOpp - ZYOppAvg;
                        ZOpp = ZOpp + ZYOppAvg;
                    }
                    else if (YOpp < ZOpp)
                    {
                        double ZYOppAvg = (ZOpp - YOpp) / 2;

                        A = A - (ZYOppAvg * alphaRotationPercentageX);
                        YOpp = YOpp + ZYOppAvg;
                        ZOpp = ZOpp - ZYOppAvg;
                    }

                    //Y Alpha Rotation
                    if (ZOpp > XOpp)
                    {
                        double XZOppAvg = (ZOpp - XOpp) / 2;
                        B = B + (XZOppAvg * alphaRotationPercentageY);
                        ZOpp = ZOpp - XZOppAvg;
                        XOpp = XOpp + XZOppAvg;
                    }
                    else if (ZOpp < XOpp)
                    {
                        double XZOppAvg = (XOpp - ZOpp) / 2;

                        B = B - (XZOppAvg * alphaRotationPercentageY);
                        ZOpp = ZOpp + XZOppAvg;
                        XOpp = XOpp - XZOppAvg;
                    }
                    //Z Alpha Rotation
                    if (XOpp > YOpp)
                    {
                        double YXOppAvg = (XOpp - YOpp) / 2;
                        C = C + (YXOppAvg * alphaRotationPercentageZ);
                        XOpp = XOpp - YXOppAvg;
                        YOpp = YOpp + YXOppAvg;
                    }
                    else if (XOpp < YOpp)
                    {
                        double YXOppAvg = (YOpp - XOpp) / 2;

                        C = C - (YXOppAvg * alphaRotationPercentageZ);
                        XOpp = XOpp + YXOppAvg;
                        YOpp = YOpp - YXOppAvg;
                    }

                    //determine if value is close enough
                    double hTow = Math.Max(Math.Max(XOpp, YOpp), ZOpp);
                    double lTow = Math.Min(Math.Min(XOpp, YOpp), ZOpp);
                    double towDiff = hTow - lTow;

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

        public float[] SPM(float X, float XOpp, float Y, float YOpp, float Z, float ZOpp)
        {

            //opp = 0.21; //4/5
            //tower = 0.27; //9/32

            double diagChange = 1 / deltaOpp;
            double towOppDiff = deltaTower / deltaOpp; //0.5
            double XYZ = (X + Y + Z) / 3;
            double XYZOpp = (XOpp + YOpp + ZOpp) / 3;

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

                        double changeInMM = ((stepsPerMM * zMaxLength) - (tempSPM * zMaxLength)) / tempSPM;

                        LogConsole("zMaxLength changed by: " + changeInMM);
                        LogConsole("zMaxLength before: " + centerHeight);
                        LogConsole("zMaxLength after: " + (centerHeight - changeInMM));

                        double tempChange = centerHeight - changeInMM;

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
