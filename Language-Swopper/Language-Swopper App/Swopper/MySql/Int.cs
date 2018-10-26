using System;
using System.Collections.Generic;
using System.Text;
using Base;

namespace LswMySql
{
    public static class lswIntPath
    {
        public static string Write(object One)
        {
            LsInt Two = (LsInt)One;
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
                temp = "SET " + Two.Name + " = " + Two.Value.ToString() + ";";
            return temp;
        }

        public static LsInt Read(string One)
        {
            string Two = One.Split('=')[0].Split(' ')[1].Trim();
            string Three = One.Split('=')[1].Trim().Trim(';').Trim();
            List<Prefix> prefixes = new List<Prefix>();
            if (Two[0] == '@')
                prefixes.Add(Prefix.@public);
            else
                prefixes.Add(Prefix.@private);
            LsInt Four;
            try { Four = new LsInt(Two, int.Parse(Three), prefixes); }
            catch { Four = new LsInt(Two, Three, prefixes); }
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