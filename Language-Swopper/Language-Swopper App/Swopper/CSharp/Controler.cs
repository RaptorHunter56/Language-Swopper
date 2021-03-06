//||||||||||
//C# files (*.cs)|*.cs|Text files (*.txt)|*.txt|All files (*.*)|*.*
//||||||||||
using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswCSharp
{
    public class CSharpPositions
    {
        public string[] InLine;
        public int Position = 0;
    }
    public class CSharpControler
    {
        public CSharpControler()
        { CSharpPosition = new CSharpPositions(); }

        public CSharpPositions CSharpPosition;
        public object In(string[] InLine) { CSharpPosition.InLine = InLine; return InRef(InLine, ref CSharpPosition); }
        public object InRef(string[] InLine, ref CSharpPositions CSharpPositionRef)
        {
            LsBaseList Return = new LsBaseList();
            while (CSharpPositionRef.Position < CSharpPositionRef.InLine.Length)
            {

                Regex stringrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}string {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex boolrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}bool {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex charrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}char {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex intrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}int {1,}\w+ {0,}= {0,}.+ {0,};");

                Regex bracketrgx = new Regex(@"^[(].+[)]$");
                Regex ifrgx = new Regex(@"^if [(].+[)]");
                Regex elseifrgx = new Regex(@"^else if [(].+[)]");

                if (stringrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else if(boolrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBoolPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else if (charrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswCharPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else if (bracketrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBracketPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd(), ref CSharpPositionRef));
                else if (intrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIntPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else if (ifrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIfPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd(), ref CSharpPositionRef));
                else if (CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd() == "else")
                    Return.Bases.Add(lswElsePath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd(), ref CSharpPositionRef));
                else if (elseifrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswElseIfPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd(), ref CSharpPositionRef));
                else
                    Return.Bases.Add(new LsName() { Name = CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd(), Lanaguage = "C#" });
                CSharpPositionRef.Position++;
            }
            return Return;
        }

        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartIn(string InLine) { return PartInRef(InLine, ref CSharpPosition); }
        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartInRef(string InLine, ref CSharpPositions CSharpPositionRef)
        {
            Regex stringrgx = new Regex(@"^""{3}[^""]+""{3}$|^""[^""]+""$");
            Regex charrgx = new Regex(@"^'[^']{1}'$|^'\\''$");
            Regex intrgx = new Regex(@"^\d+.{0}\d$");
            Regex boolrgx = new Regex(@"^true$|^false$");
            Regex bracketrgx = new Regex(@"^[(].+[)]$");

            if (charrgx.Match(InLine).Success)
                return InLine.Substring(1, InLine.ToString().Length - 2);
            else if (stringrgx.Match(InLine).Success)
            {
                if (InLine.ToString().Length > 6 && InLine.Substring(0, 3) == "\"\"\"" && InLine.Substring(InLine.ToString().Length - 3, 3) == "\"\"\"")
                    return (InLine.ToString().Length == 7) ? InLine.Substring(3, InLine.ToString().Length - 6) : InLine.Substring(3, InLine.ToString().Length - 6);
                else if (InLine.ToString().Length > 2 && InLine.Substring(0, 1) == "\"" && InLine.Substring(InLine.ToString().Length - 1, 1) == "\"")
                    return (InLine.ToString().Length == 3) ? InLine.Substring(1, InLine.ToString().Length - 2) : InLine.Substring(1, InLine.ToString().Length - 2);
            }
            else if (intrgx.Match(InLine).Success)
                return Int32.Parse(InLine);
            else if (boolrgx.Match(InLine).Success)
                return (InLine[0] == 't') ? true : false;
            else if (bracketrgx.Match(InLine).Success)
                return lswBracketPath.Read(InLine, ref CSharpPositionRef);
            return InLine;
        }

        public string Out(object OutLine) { return OutRef(OutLine, ref CSharpPosition); }
        public string OutRef(object OutLine, ref CSharpPositions CSharpPositionRef)
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
                    else if (((lsBase)item).lsType == "LsChar")
                        Return += lswCharPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsBracket")
                        Return += lswBracketPath.Write(item, ref CSharpPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsInt")
                        Return += lswIntPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsIf")
                        Return += lswIfPath.Write(item, ref CSharpPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsElseIf")
                        Return += lswElseIfPath.Write(item, ref CSharpPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsElse")
                        Return += lswElsePath.Write(item, ref CSharpPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsName")
                        try { Return += ((LsName)item).Lanaguage + " Doesn't Have a conversion file for this." + "\r\n"; } catch { Return += "{No_Type}" + "\r\n"; }
                    else
                        try { Return += item.ToString() + " {No_Type} " + "\r\n"; } catch { Return += "{No_Type}" + "\r\n"; }
                }
                catch (Exception e)
                {
                    Return += e.Message + "\r\n";
                }
            }
            return Return;
        }
    }
}
