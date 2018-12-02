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
        public PythonControler()
        { PythonPosition = new PythonPositions(); }

        public PythonPositions PythonPosition;
        public object In(string[] InLine) { PythonPosition.InLine = InLine; return InRef(InLine, ref PythonPosition); }
        public object InRef(string[] InLine, ref PythonPositions PythonPositionRef)
        {
            LsBaseList Return = new LsBaseList();
            while (PythonPositionRef.Position < PythonPositionRef.InLine.Length)
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
                Regex ifrgx = new Regex(@"^if .+:$");
                Regex elseifrgx = new Regex(@"^elif .+:$");
                if (charrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswCharPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()));
                else if (stringrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()));
                else if (boolrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBoolPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()));
                else if (intrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIntPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()));
                else if (bracketrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBracketPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd(), ref PythonPositionRef));
                else if (ifrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIfPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd(), ref PythonPositionRef));
                else if (defoltrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()));
                else if (elseifrgx.Match(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswElseIfPath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd(), ref PythonPositionRef));
                else if (PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd().ToLower().Substring(0, 4) == "else")
                    Return.Bases.Add(lswElsePath.Read(PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd(), ref PythonPositionRef));
                else
                    Return.Bases.Add(new LsName() { Name = PythonPositionRef.InLine[PythonPositionRef.Position].TrimEnd(), Lanaguage = "Python" });
                PythonPositionRef.Position++;
            }
            return Return;
        }

        public object PartIn(string InLine) { return PartInRef(InLine, ref PythonPosition); }
        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartInRef(string InLine, ref PythonPositions PythonPositionRef)
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
                return lswBracketPath.Read(InLine, ref PythonPositionRef);
            return InLine;
        }

        public string Out(object OutLine) { return OutRef(OutLine, ref PythonPosition); }
        public string OutRef(object OutLine, ref PythonPositions PythonPositionRef)
        {
            LsBaseList baseList = (LsBaseList)OutLine;
            string Return = "";
            foreach (var item in baseList.Bases)
            {
                try
                {
                    if (((lsBase)item).lsType == "LsString")
                        Return += lswStringPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsBool")
                        Return += lswBoolPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsInt")
                        Return += lswIntPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsChar")
                        Return += lswCharPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsBracket")
                        Return += lswBracketPath.Write(item, ref PythonPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsIf")
                        Return += lswIfPath.Write(item, ref PythonPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsElseIf")
                        Return += lswElseIfPath.Write(item, ref PythonPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsElse")
                        Return += lswElsePath.Write(item, ref PythonPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsName")
                        try { Return += ((LsName)item).Lanaguage + " Doesn't Have a conversion file for this." + "\r"; } catch { Return += "{No_Type}" + "\r\n"; }
                    else
                        try { Return += item.ToString() + " {No_Type} " + "\r"; } catch { Return += "{No_Type}" + "\r\n"; }
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
