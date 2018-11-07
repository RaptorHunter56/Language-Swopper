using System;
using System.Collections.Generic;
using System.Text;
using Base;

namespace LswSql
{
    public static class lswStringPath
    {
        public static string Write(object One)
        {
            LsString Two = (LsString)One;
            foreach (Prefix prefix in Two.Prefixes)
            {
				if (prefix == Prefix.@public)
				{
					Two.Name = "@" + Two.Name;
				}
            }
            string temp = "";
            string NewString = "";
            foreach (var item in Two.Value)
            {
                if (item == "'".ToCharArray()[0])
                    NewString += "\\" + item;
                else
                    NewString += item;
            }
            if (Two.ValueType)
                temp = "SET " + Two.Name + " = " + NewString + ";";
            else
                temp = "SET " + Two.Name + " = '" + NewString + "';";
            return temp;
        }

        public static LsString Read(string One)
        {
            string Two = One.Split('=')[0].Split(' ')[1].Trim();
            string Three = One.Split('=')[1].Trim().Trim(';').Trim();
            List<Prefix> prefixes = new List<Prefix>();
			if (Two[0] == '@')
				prefixes.Add(Prefix.@public);
			else
				prefixes.Add(Prefix.@private);
            LsString Four;
            StringBuilder builder = new StringBuilder(Three.Substring(1, Three.Length - 2));
            builder.Replace("''", "'");
            builder.Replace("\'", "'");

            string y = builder.ToString();
            if ((Three[0] == "'".ToCharArray()[0] && Three[Three.Length - 1] == "'".ToCharArray()[0]))
                Four = new LsString(Two, builder.ToString(), prefixes);
            else
                Four = new LsString(Two, Three, prefixes);
            return Four;
        }
    }
}

//########
//",255|165|42|42,StartToEnd
//',255|165|42|42.StartToEnd