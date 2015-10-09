using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace OpenDACT.Class_Files
{

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
    }
}


