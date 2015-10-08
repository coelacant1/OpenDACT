using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDACT.Class_Files
{
    public class Test1
    {
        public bool initialized;
        public Test1()
        {
            initialized = true;
        }
    }

    class Test2
    {
        static void Main()
        {
            Test1 test = new Test1();
            Console.WriteLine(test.initialized);

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

}
