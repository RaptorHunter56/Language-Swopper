//||||||||||
//Python files (*.py)|*.py|Text files (*.txt)|*.txt|All files (*.*)|*.*
//||||||||||
using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswPython
{
    public class PythonPositions
    {
        public string[] InLine;
        public int Position = 0;
    }
    public class PythonControler
    {

        public PythonPositions PythonPosition = new PythonPositions();
        public object In(string[] InLine)
        {
            PythonPosition.InLine = InLine;
            LsBaseList Return = new LsBaseList();
            while (PythonPosition.Position < PythonPosition.InLine.Length)
            {
                //Done
                Regex stringrgx = new Regex(@"\w+ {0,}=[ (]{0,}""{1}[^""]+""{1}([ +)(]{0,}""{1}[^""]+""{1}|[ +)(]{0,}'{1}[^']+'{1}|[ +)(]{0,}""{3}.+""{3}|[ +)(]{0,}\w+){0,}[)]{0,}|\w+ {0,}=[ (]{0,}""{3}.+""{3}([ +)(]{0,}""{1}[^""]+""{1}|[ +)(]{0,}'{1}[^']+'{1}|[ +)(]{0,}""{3}.+""{3}|[ +)(]{0,}\w+){0,}[)]{0,}|\w+ {0,}=[ (]{0,}'{1}[^']+'{1}([ +)(]{0,}""{1}[^""]+""{1}|[ +)(]{0,}'{1}[^']+'{1}|[ +)(]{0,}""{3}.+""{3}|[ +)(]{0,}\w+){0,}[)]{0,}");
                Regex charrgx = new Regex(@"\w+ {0,}=[ (]{0,}""{1}[^""]{1}""{1}[)]{0,}|\w+ {0,}=[ (]{0,}""{3}.{1}""{3}[)]{0,}|\w+ {0,}=[ (]{0,}'{1}[^']{1}'{1}[)]{0,}");
                //Not Done
                Regex boolrgx = new Regex(@"\w+ {0,}= {0,}true|\w+ {0,}= {0,}false");
                Regex intrgx = new Regex(@"\w+ {0,}= {0,}[0-9]+");
                Regex defoltrgx = new Regex(@"\w+ {0,}= {0,}\w+");
                //Finished
                Regex bracketrgx = new Regex(@"^[(].+[)]$");
                if (charrgx.Match(PythonPosition.InLine[PythonPosition.Position]).Success)
                    Return.Bases.Add(lswCharPath.Read(PythonPosition.InLine[PythonPosition.Position]));
                else if (stringrgx.Match(PythonPosition.InLine[PythonPosition.Position]).Success)
                    Return.Bases.Add(lswStringPath.Read(PythonPosition.InLine[PythonPosition.Position]));
                else if (boolrgx.Match(PythonPosition.InLine[PythonPosition.Position]).Success)
                    Return.Bases.Add(lswBoolPath.Read(PythonPosition.InLine[PythonPosition.Position]));
                else if (intrgx.Match(PythonPosition.InLine[PythonPosition.Position]).Success)
                    Return.Bases.Add(lswIntPath.Read(PythonPosition.InLine[PythonPosition.Position]));
                else if (bracketrgx.Match(PythonPosition.InLine[PythonPosition.Position]).Success)
                    Return.Bases.Add(lswBracketPath.Read(PythonPosition.InLine[PythonPosition.Position], PythonPosition));
                else if (defoltrgx.Match(PythonPosition.InLine[PythonPosition.Position]).Success)
                    Return.Bases.Add(lswStringPath.Read(PythonPosition.InLine[PythonPosition.Position]));
                else
                    Return.Bases.Add(new LsName() { Name = PythonPosition.InLine[PythonPosition.Position] });
                PythonPosition.Position++;
            }
            return Return;
        }

        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartIn(string InLine)
        {
            Regex stringrgx = new Regex(@"^""{3}[^""]+""{3}$|^""[^""]+""$|^'[^']+'$");
            Regex charrgx = new Regex(@"^""{3}[^""]{1}""{3}$|^""[^""]{1}""$|^'[^']{1}'$");
            Regex intrgx = new Regex(@"^\d+.{0}\d$");
            Regex boolrgx = new Regex(@"^true$|^false$");
            Regex bracketrgx = new Regex(@"^[(].+[)]$");

            if (stringrgx.Match(InLine).Success || charrgx.Match(InLine).Success)
            {
                if (InLine.ToString().Length > 6 && InLine.Substring(0, 3) == "\"\"\"" && InLine.Substring(InLine.ToString().Length - 3, 3) == "\"\"\"")
                    return (InLine.ToString().Length == 7) ? InLine.Substring(3, InLine.ToString().Length - 6) : InLine.Substring(3, InLine.ToString().Length - 6);
                else if (InLine.ToString().Length > 2 && InLine.Substring(0, 1) == "'" && InLine.Substring(InLine.ToString().Length - 1, 1) == "'")
                    return (InLine.ToString().Length == 3) ? InLine.Substring(1, InLine.ToString().Length - 2) : InLine.Substring(1, InLine.ToString().Length - 2);
                else if (InLine.ToString().Length > 2 && InLine.Substring(0, 1) == "\"" && InLine.Substring(InLine.ToString().Length - 1, 1) == "\"")
                    return (InLine.ToString().Length == 3) ? InLine.Substring(1, InLine.ToString().Length - 2) : InLine.Substring(1, InLine.ToString().Length - 2);
            }
            else if (intrgx.Match(InLine).Success)
                return Int32.Parse(InLine);
            else if (boolrgx.Match(InLine).Success)
                return (InLine[0] == 't') ? true : false;
            else if (bracketrgx.Match(InLine).Success)
                return lswBracketPath.Read(InLine, PythonPosition);
            return InLine;
        }

        public string Out(object OutLine)
        {
            LsBaseList baseList = (LsBaseList)OutLine;
            string Return = "";
            foreach (var item in baseList.Bases)
            {
                try
                {
                    if (((lsBase)item).lsType == "LsString")
                        Return += lswStringPath.Write(item) + " {LsString} " + "\r";
                    else if (((lsBase)item).lsType == "LsBool")
                        Return += lswBoolPath.Write(item) + " {LsBool} " + "\r";
                    else if (((lsBase)item).lsType == "LsInt")
                        Return += lswIntPath.Write(item) + " {LsInt} " + "\r";
                    else if (((lsBase)item).lsType == "LsChar")
                        Return += lswCharPath.Write(item) + " {LsChar} " + "\r";
                    else if (((lsBase)item).lsType == "LsBracket")
                        Return += lswBracketPath.Write(item, PythonPosition) + " {LsBracket} " + "\r";
                    else
                        try { Return += item.ToString() + " {No_Type} " + "\r"; } catch { Return += "{No_Type}" + "\r"; }
                }
                catch (Exception e)
                {
                    Return += e.Message + "\r";
                }
            }
            return Return;
        }
    }
}
