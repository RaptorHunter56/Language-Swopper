using System;
using Base;

namespace LswPython
{
    public static class lswBoolPath
    {
        public static string Write(object One)
        {
            LsBool Two = (LsBool)One;
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
            LsBool Four;
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
//true,255|0|0|190
//false,255|0|0|190