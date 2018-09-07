using System;
using Base;

namespace LswPython
{
    public class lswStringPath
    {
        public static string Print(object One)
        {
            LsString Two = (LsString)One;
            string temp = "";
            if (Two.ValueType)
                temp = Two.Name + " = " + Two.Value;
            else if (Two.Value.Contains("\""))
                temp = Two.Name + " = \"\"\"" + Two.Value + "\"\"\"";
            else
                temp = Two.Name + " = \"" + Two.Value + "\"";
            return temp;
        }
		
		public static LsString Read(string One)
        {
            string Two = One.Split('=')[0].Trim();
            string Three = One.Split('=')[1].Trim();
            LsString Four;
            if (Three.Length > 6 &&
                Three.Substring(0, 3) == "\"\"\"" &&
                Three.Substring(Three.Length - 3, 3) == "\"\"\"")
            {
                Three = Three.Substring(3, Three.Length - 6);
                Four = new LsString(Two, Three);
            }
            else if (Three.Length > 2 &&
                Three.Substring(0, 1) == "'" &&
                Three.Substring(Three.Length - 1, 1) == "'")
            {
                Three = Three.Substring(1, Three.Length - 2);
                Four = new LsString(Two, Three);
            }
            else if (Three.Length > 2 &&
                Three.Substring(0, 1) == "\"" &&
                Three.Substring(Three.Length - 1, 1) == "\"")
            {
                Three = Three.Substring(1, Three.Length - 2);
                Four = new LsString(Two, Three);
            }
            else
            {
                Four = new LsString(Two, Three);
                Four.ValueType = true;
            }
            return Four;
        }
    }
}

//########
//=,255|0|0|205
//",255|0|205|0
//',255|0|205|0