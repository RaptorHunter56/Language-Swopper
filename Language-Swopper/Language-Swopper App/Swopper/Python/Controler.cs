using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswPython
{
    public class PythonControler
    {
        public static bool CheckIn(string In, out lsBase Out)
        {
            Out = null;
            return false;
        }
        public static string CheckOut(lsBase Out)
        {
            return "";
        }

        public object In(string InLine)
        {
            Regex stringrgx = new Regex(@"\w+ {0,}= {0,}""{1}[^""]+""{1}|\w+ {0,}= {0,}""{3}.+""{3}|\w+ {0,}= {0,}'{1}[^']+'{1}");
            Regex charrgx = new Regex(@"\w+ {0,}= {0,}""{1}[^""]{1}""{1}|\w+ {0,}= {0,}""{3}.{1}""{3}|\w+ {0,}= {0,}'{1}[^']{1}'{1}");
            Regex boolrgx = new Regex(@"\w+ {0,}= {0,}true|\w+ {0,}= {0,}false");
            Regex intrgx = new Regex(@"\w+ {0,}= {0,}[0-9]+");
            Regex defoltrgx = new Regex(@"\w+ {0,}= {0,}\w+");
            if (charrgx.Match(InLine).Success)
                return lswCharPath.Read(InLine);
            else if(stringrgx.Match(InLine).Success)
                return lswStringPath.Read(InLine);
            else if (boolrgx.Match(InLine).Success)
                return lswBoolPath.Read(InLine);
            else if (intrgx.Match(InLine).Success)
                return lswIntPath.Read(InLine);
            else if (defoltrgx.Match(InLine).Success)
                return lswStringPath.Read(InLine);
            else
                return new LsString("Name", "Value");
        }

        public string Out(object OutLine)
        {
            try
            {
                if (((lsBase)OutLine).lsType == "LsString")
                    return lswStringPath.Write(OutLine) + " {LsString} " + "\r";
                else if (((lsBase)OutLine).lsType == "LsBool")
                    return lswBoolPath.Write(OutLine) + " {LsBool} " + "\r";
                else if (((lsBase)OutLine).lsType == "LsInt")
                    return lswIntPath.Write(OutLine) + " {LsInt} " + "\r";
                else if (((lsBase)OutLine).lsType == "LsChar")
                    return lswCharPath.Write(OutLine) + " {LsChar} " + "\r";
                else
                    return "No_Type" + "\r";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
