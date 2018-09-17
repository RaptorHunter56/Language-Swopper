using System;
using Base;

namespace LswPython
{
    public static class lswCharPath
    {
        public static string Write(object One)
        {
            LsChar Two = (LsChar)One;
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
                temp = Two.Name + " = " + Two.Value;
            else if (Two.Value == ('"'))
                temp = Two.Name + " = '" + Two.Value + "'";
            else
                temp = Two.Name + " = \"" + Two.Value + "\"";
            return temp;
        }
		
		public static LsChar Read(string One)
        {
            string Two = One.Split('=')[0].Trim();
            string Three = One.Split('=')[1].Trim();
            LsChar Four = new LsChar(Two);
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
            if (Three.Length == 7 &&
                Three.Substring(0, 3) == "\"\"\"" &&
                Three.Substring(Three.Length - 3, 3) == "\"\"\"")
            {
                Three = Three.Substring(3, Three.Length - 6);
                Four = new LsChar(Two, Three.ToCharArray()[0]);
            }
            else if (Three.Length == 3 &&
                Three.Substring(0, 1) == "'" &&
                Three.Substring(Three.Length - 1, 1) == "'")
            {
                Three = Three.Substring(1, Three.Length - 2);
                Four = new LsChar(Two, Three.ToCharArray()[0]);
            }
            else if (Three.Length == 3 &&
                Three.Substring(0, 1) == "\"" &&
                Three.Substring(Three.Length - 1, 1) == "\"")
            {
                Three = Three.Substring(1, Three.Length - 2);
                Four = new LsChar(Two, Three.ToCharArray()[0]);
            }
            else
            {
                Four = new LsChar(Two, Three);
                Four.ValueType = true;
            }

            return Four;
        }
    }
}

//########
//",255|165|42|42,StartToEnd
//',255|165|42|42.StartToEnd