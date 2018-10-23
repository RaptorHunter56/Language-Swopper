using System;
using System.Collections.Generic;
using System.Text;
using Base;

namespace LswMySql
{
    public static class lswBoolPath
    {
        public static string Write(object One)
        {
            LsBool Two = (LsBool)One;
            foreach (Prefix prefix in Two.Prefixes)
            {
                if (prefix == Prefix.@public)
                {
                    Two.Name = "@" + Two.Name;
                }
            }
            string temp = "";
            if (Two.ValueType)
                temp = "SET " + Two.Name + " = " + Two.ValueT + ";";
            else
                temp = "SET " + Two.Name + " = " + Two.Value.ToString().ToUpper() + ";";
            return temp;
        }

        public static LsBool Read(string One)
        {
            string Two = One.Split('=')[0].Split(' ')[1].Trim();
            string Three = One.Split('=')[1].Trim().Trim(';').Trim();
            List<Prefix> prefixes = new List<Prefix>();
            if (Two[0] == '@')
                prefixes.Add(Prefix.@public);
            else
                prefixes.Add(Prefix.@private);
            LsBool Four;
            if (Three.ToLower().Trim() == "true")
                Four = new LsBool(Two, true, prefixes);
            else if (Three.ToLower().Trim() == "false")
                Four = new LsBool(Two, false, prefixes);
            else
                Four = new LsBool(Two, Three, prefixes);
            return Four;
        }
    }
}

//########
//true,255|0|0|190,Normal
//false,255|0|0|190,Normal