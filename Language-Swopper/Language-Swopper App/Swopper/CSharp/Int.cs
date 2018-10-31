using System;
using System.Collections.Generic;
using Base;

namespace LswCSharp
{
    public static class lswIntPath
    {
        public static string Write(object One)
        {
            LsInt Two = (LsInt)One;
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
                temp = pre + "int " + Two.Name + " = " + Two.ValueT + ";";
            else
                temp = pre + "int " + Two.Name + " = " + Two.Value.ToString() + ";";
            return temp;
        }

        public static LsInt Read(string One)
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
