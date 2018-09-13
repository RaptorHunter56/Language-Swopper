using System;
using Base;

namespace LswPython
{
    public static class lswBoolPath
    {
        public static string Write(object One)
        {
            LsBool Two = (LsBool)One;
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
                temp = Two.Name + " = " + Two.Value.ToString().ToLower();
            return temp;
        }

        public static LsBool Read(string One)
        {
            string Two = One.Split('=')[0].Trim();
            string Three = One.Split('=')[1].Trim();
            LsBool Four = new LsBool(Two);
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
            if (Three == "true")
            {
                Four = new LsBool(Two, true);
            }
            else if (Three == "false")
            {
                Four = new LsBool(Two, false);
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
//true,255|0|0|190,Normal
//false,255|0|0|190,Normal