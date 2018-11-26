using System.Collections.Generic;
using System;
using Base;

namespace LswVB
{
    public static class lswCharPath
    {
        public static string Write(object One)
        {
            LsChar Two = (LsChar)One;
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
                temp = pre + "char " + Two.Name + " = " + Two.Value + ";";
            else if (Two.Value == '\'')
                temp = pre + "char " + Two.Name + " = '\\'';";
            else
                temp = pre + "char " + Two.Name + " = '" + Two.Value + "';";
            return temp;
        }

        public static LsChar Read(string One)
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
            if (prefixes.Count == 0)
                prefixes.Add(Prefix.@public);
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