using System;
using Base;

namespace LswPython
{
    public static class lswIntPath
    {
        public static string Write(object One)
        {
            LsInt Two = (LsInt)One;
            foreach (Prefix prefix in Two.Prefixes)
            {
                switch (prefix)
                {
                    case Prefix.@protected:
                        Two.Name = "self._" + Two.Name;
                        break;
                    case Prefix.@private:
                        Two.Name = "self.__" + Two.Name;
                        break;
                    default:
                        break;
                }
            }
            string temp = "";
            if (Two.ValueType)
                temp = Two.Name + " = " + Two.ValueT;
            else
                temp = Two.Name + " = " + Two.Value.ToString();
            return temp;
        }

        public static LsInt Read(string One)
        {
            string Two = One.Split('=')[0].Trim();
            string Three = One.Split('=')[1].Trim();
            LsInt Four;
            if (Two.StartsWith("self.__") || Two.StartsWith("__"))
                Four.Prefixes.Add(Prefix.@private);
            else if (Two.StartsWith("self._") || Two.StartsWith("_"))
                Four.Prefixes.Add(Prefix.@protected);
            else
                Four.Prefixes.Add(Prefix.@public);
            if (Int32.TryParse(Three))
            {
                Four = new LsBool(Two, Int32.Parse(Three));
            }
            else
            {
                Four = new LsBool(Two, Three);
            }
            return Four;
        }
    }
}

//########
//1,255|255|0|0
//2,255|255|0|0
//3,255|255|0|0
//4,255|255|0|0
//5,255|255|0|0
//6,255|255|0|0
//7,255|255|0|0
//8,255|255|0|0
//9,255|255|0|0
//0,255|255|0|0