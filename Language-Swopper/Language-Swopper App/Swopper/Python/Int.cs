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
            LsInt Four = new LsInt(Two);
            if (Two.StartsWith("self.__"))
            {
                Two.TrimStart("self.__".ToCharArray());
                Four.Prefixes.Add(Prefix.@private);
            }
            else if (Two.StartsWith("self._"))
            {
                Two.TrimStart("self._".ToCharArray());
                Four.Prefixes.Add(Prefix.@protected);
            }
            else
            {
                Four.Prefixes.Add(Prefix.@public);
            }
            try { Four = new LsInt(Two, int.Parse(Three)); }
            catch { Four = new LsInt(Two, Three); }
            return Four;
        }
    }
}

//########
//1,255|255|0|0,Connected
//2,255|255|0|0,Connected
//3,255|255|0|0,Connected
//4,255|255|0|0,Connected
//5,255|255|0|0,Connected
//6,255|255|0|0,Connected
//7,255|255|0|0,Connected
//8,255|255|0|0,Connected
//9,255|255|0|0,Connected
//0,255|255|0|0,Connected