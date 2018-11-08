//||||||||||
//MySql files (*.sql)|*.sql|Text files (*.txt)|*.txt|All files (*.*)|*.*
//||||||||||
using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswMySql
{
    public class MySqlPositions
    {
        public string[] InLine;
        public int Position = 0;
    }
    public class MySqlControler
    {
        public MySqlControler()
        { MySqlPosition = new MySqlPositions(); }

        public MySqlPositions MySqlPosition;
        public object In(string[] InLine) { MySqlPosition.InLine = InLine; return InRef(InLine, ref MySqlPosition); }
        public object InRef(string[] InLine, ref MySqlPositions MySqlPositionRef)
        {
            LsBaseList Return = new LsBaseList();
            while (MySqlPositionRef.Position < MySqlPositionRef.InLine.Length)
            {

                Regex stringrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}'(([^']{0,}[\\]'[^'\\]{0,}){1,}|([^']{0,}''[^']{0,}){1,}|[^']{1,})';{0,1}");
                Regex boolrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}((t|T)(r|R)(u|U)(e|E)|(f|F)(a|A)(l|L)(s|S)(e|E));{0,1}");
                Regex chargrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}'(([\\]'){1}|[^']{1})';{0,1}");
                Regex intgrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}(\d{1,}|.+);{0,1}");

                Regex bracketrgx = new Regex(@"^[(] {0,}.+ {0,}[)]$");
                Regex ifrgx = new Regex(@"(i|I)(f|F) {0,}[(] {0,}.+ {0,}[)] {0,}(t|T)(h|H)(e|E)(n|N)");

                if (chargrgx.Match(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswCharPath.Read(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()));
                else if (stringrgx.Match(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()));
                else if (boolrgx.Match(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBoolPath.Read(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()));
                else if (intgrgx.Match(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIntPath.Read(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()));
                else if (bracketrgx.Match(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBracketPath.Read(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd(), ref MySqlPositionRef));
                else if (ifrgx.Match(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIfPath.Read(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd(), ref MySqlPositionRef));
                else
                    Return.Bases.Add(new LsName() { Name = MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd(), Lanaguage = "MySql" });
                MySqlPositionRef.Position++;
            }
            return Return;
        }

        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartIn(string InLine) { return PartInRef(InLine, ref MySqlPosition); }
        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartInRef(string InLine, ref MySqlPositions MySqlPositionRef)
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
                return lswBracketPath.Read(InLine, ref MySqlPositionRef);
            return InLine;
        }

        public string Out(object OutLine) { return OutRef(OutLine, ref MySqlPosition); }
        public string OutRef(object OutLine, ref MySqlPositions MySqlPositionRef)
        {
            LsBaseList baseList = (LsBaseList)OutLine;
            string Return = "";
            foreach (var item in baseList.Bases)
            {
                try
                {
                    if (((lsBase)item).lsType == "LsString")
                        Return += lswStringPath.Write(item) + "\r";
                    else if (((lsBase)item).lsType == "LsBool")
                        Return += lswBoolPath.Write(item) + "\r";
                    else if (((lsBase)item).lsType == "LsChar")
                        Return += lswCharPath.Write(item) + "\r";
                    else if (((lsBase)item).lsType == "LsInt")
                        Return += lswIntPath.Write(item) + "\r";
                    else if (((lsBase)item).lsType == "LsBracket")
                        Return += lswBracketPath.Write(item, ref MySqlPositionRef) + "\r";
                    else if (((lsBase)item).lsType == "LsIf")
                        Return += lswIfPath.Write(item, ref MySqlPositionRef) + "\r";
                    else if (((lsBase)item).lsType == "LsName")
                        try { Return += ((LsName)item).Lanaguage + " Doesn't Have a conversion file for this." + "\r"; } catch { Return += "{No_Type}" + "\r"; }
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
