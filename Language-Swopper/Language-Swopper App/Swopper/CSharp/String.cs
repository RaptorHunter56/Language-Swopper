using System;
using System.Collections.Generic;
using Base;

namespace LswCSharp
{
    public static class lswStringPath
    {
        public static string Write(object One)
        {
            LsString Two = (LsString)One;
            foreach (Prefix prefix in Two.Prefixes)
            {
                switch (prefix)
                {
                    case Prefix.@public:
                        Two.Name = "public " + Two.Name;
                        break;
                    case Prefix.@protected:
                        Two.Name = "protected " + Two.Name;
                        break;
                    case Prefix.@private:
                        Two.Name = "private " + Two.Name;
                        break;
                    case Prefix.@static:
                        Two.Name = "static " + Two.Name;
                        break;
                    case Prefix.@readonly:
                        Two.Name = "readonly " + Two.Name;
                        break;
                    case Prefix.@internal:
                        Two.Name = "internal " + Two.Name;
                        break;
                    default:
                        break;
                }
            }
            string temp = "";
            string NewString = "";
            foreach (var item in Two.Value)
            {
                if (item == '"')
                    NewString += "\\" + item;
                else
                    NewString += item;
            }
            if (Two.ValueType)
                temp = "string " + Two.Name + " = " + NewString + ";";
            else
                temp = "string " + Two.Name + " = \"" + NewString + "\";";
            return temp;
        }

        public static LsString Read(string One)
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
            LsString Four;
            if ((Three[0] == '"' && Three[Three.Length - 1] == '"')||(Three[0] == "'".ToCharArray()[0] && Three[Three.Length - 1] == "'".ToCharArray()[0]))
                Four = new LsString(Two, Three.Substring(1, Three.Length - 2), prefixes);
            else
                Four = new LsString(Two, Three, prefixes);
            return Four;
        }
    }
}

//########
//",255|165|42|42,StartToEnd
//',255|165|42|42.StartToEnd