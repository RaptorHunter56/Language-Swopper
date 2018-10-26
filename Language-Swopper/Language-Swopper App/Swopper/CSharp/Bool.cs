using System;
using System.Collections.Generic;
using Base;

namespace LswCSharp
{
    public static class lswBoolPath
    {
        public static string Write(object One)
        {
            LsBool Two = (LsBool)One;
            string pre = "";
            foreach (Prefix prefix in Two.Prefixes)
            {
                switch (prefix)
                {
                    case Prefix.@public:
                        pre = "public " + pre;
                        break;
                    case Prefix.@protected:
                        pre = "protected " + pre;
                        break;
                    case Prefix.@private:
                        pre = "private " + pre;
                        break;
                    case Prefix.@static:
                        pre = "static " + pre;
                        break;
                    case Prefix.@readonly:
                        pre = "readonly " + pre;
                        break;
                    case Prefix.@internal:
                        pre = "internal " + pre;
                        break;
                    default:
                        break;
                }
            }
            string temp = "";
            if (Two.ValueType)
                temp = pre + "bool " + Two.Name + " = " + Two.ValueT + ";";
            else
                temp = pre + "bool " + Two.Name + " = " + Two.Value.ToString().ToLower() + ";";
            return temp;
        }

        public static LsBool Read(string One)
        {
            string Two = One.Split('=')[0].Trim().Split(' ')[One.Split('=')[0].Trim().Split(' ').Length - 1].Trim();
            string Three = One.Split('=')[1].Trim().Trim(';').Trim();
            List<Prefix> prefixes = new List<Prefix>();
            foreach (var item in One.Split('=')[0].Split(' '))
            {
                if (item.Trim() == "protected")
                    prefixes.Add(Prefix.@protected);
                else if (item.Trim() == "private")
                    prefixes.Add(Prefix.@private);
                else if (item.Trim() == "static")
                    prefixes.Add(Prefix.@static);
                else if (item.Trim() == "readonly")
                    prefixes.Add(Prefix.@readonly);
                else if (item.Trim() == "internal")
                    prefixes.Add(Prefix.@internal);
                else if (item.Trim() == "public")
                    prefixes.Add(Prefix.@public);
            }
            if (prefixes.Count() == 0)
                prefixes.Add(Prefix.@public);
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