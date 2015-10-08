using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    class UserVariables
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


    class UserInterface
    {
        mainForm mainForm;

        public UserInterface(mainForm _mainForm)
        {
            this.mainForm = _mainForm;
        }
        


        public void logConsole(string value)
        {
            mainForm.consoleMain.AppendText(value + "\n");
        }


        public void logPrinter(string value)
        {
            mainForm.consolePrinter.AppendText(value + "\n");
        }
        
        /*
            BUTTONS:
            connect
            disconnect
            calibrate - readeeprom, checkheights, calibrate, checkheights, calibrate etc - while loop
            checkHeights - set gcode bool to true
            
            UI:
            visible: console log, 
            not: printer log, tabs: settings, advanced, calibration graph
        */
    }
}
