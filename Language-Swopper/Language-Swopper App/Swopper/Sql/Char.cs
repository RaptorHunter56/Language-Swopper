using System.Collections.Generic;
using System;
using Base;

namespace LswSql
{
    public static class lswCharPath
    {
        public static string Write(object One)
        {
            LsChar Two = (LsChar)One;
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
                temp = "SET " + Two.Name + " = '" + Two.Value + "';";
            return temp;
        }

        public static LsChar Read(string One)
        {
            string Two = One.Split('=')[0].Split(' ')[1].Trim();
            string Three = One.Split('=')[1].Trim().Trim(';').Trim();
            List<Prefix> prefixes = new List<Prefix>();
            if (Two[0] == '@')
                prefixes.Add(Prefix.@public);
            else
                prefixes.Add(Prefix.@private);
            LsChar Four;
            if (Three == "'\\''")
                Four = new LsChar(Two, '\'', prefixes);
            else if (Three[0] == '\'' && Three[2] == '\'')
                Four = new LsChar(Two, Three.Substring(1, 1).ToCharArray()[0], prefixes);
            else
                Four = new LsChar(Two, Three, prefixes);
            return Four;
        }
    }
}

//########
//',255|165|42|42.StartToEnd