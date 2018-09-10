using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswPython
{
    public class PythonControler
    {
        public object In(string InLine)
        {
            Regex stringrgx = new Regex(@"\w+ {0,}= {0,}""{1}[^""]+""{1}|\w+ {0,}= {0,}""{3}.+""{3}|\w+ {0,}= {0,}'{1}[^']+'{1}");
            Regex boolrgx = new Regex(@"\w+ {0,}= {0,}true|\w+ {0,}= {0,}false");
            Regex defoltrgx = new Regex(@"\w+ {0,}= {0,}\w+");
            if (stringrgx.Match(InLine).Success)
                return lswStringPath.Read(InLine);
            else if (boolrgx.Match(InLine).Success)
                return lswBoolPath.Read(InLine);
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
                    return lswStringPath.Write(OutLine) + "\r";
                else if (((lsBase)OutLine).lsType == "LsBool")
                    return lswBoolPath.Write(OutLine) + "\r";
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
