//||||||||||
//Sql files (*.sql)|*.sql|Text files (*.txt)|*.txt|All files (*.*)|*.*
//||||||||||
using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswSql
{
    public class SqlPositions
    {
        public string[] InLine;
        public int Position = 0;
    }
    public class SqlControler
    {
        public SqlControler()
        { SqlPosition = new SqlPositions(); }

        public SqlPositions SqlPosition;
        public object In(string[] InLine) { SqlPosition.InLine = InLine; return InRef(InLine, ref SqlPosition); }
        public object InRef(string[] InLine, ref SqlPositions SqlPositionRef)
        {
            LsBaseList Return = new LsBaseList();
            while (SqlPositionRef.Position < SqlPositionRef.InLine.Length)
            {

                Regex stringrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}'(([^']{0,}[\\]'[^'\\]{0,}){1,}|([^']{0,}''[^']{0,}){1,}|[^']{1,})';{0,1}");
                Regex boolrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}((t|T)(r|R)(u|U)(e|E)|(f|F)(a|A)(l|L)(s|S)(e|E));{0,1}");
                Regex chargrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}'(([\\]'){1}|[^']{1})';{0,1}");
                Regex intgrgx = new Regex(@"(s|S)(e|E)(t|T) {1,}.+ {0,}= {0,}(\d{1,}|.+);{0,1}");

                Regex bracketrgx = new Regex(@"^[(] {0,}.+ {0,}[)]$");
                Regex ifrgx = new Regex(@"(i|I)(f|F) {0,}[(] {0,}.+ {0,}[)] {0,}(t|T)(h|H)(e|E)(n|N)");
                Regex elseifrgx = new Regex(@"(e|E)(l|L)(s|S)(e|E)(i|I)(i|I)(f|F) {0,}[(] {0,}.+ {0,}[)] {0,}(t|T)(h|H)(e|E)(n|N)");

                if (chargrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswCharPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()));
                else if (stringrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()));
                else if (boolrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBoolPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()));
                else if (intgrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIntPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()));
                else if (bracketrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBracketPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd(), ref SqlPositionRef));
                else if (ifrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIfPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd(), ref SqlPositionRef));
                else if (elseifrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswElseIfPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd(), ref SqlPositionRef));
                else if (SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd().ToLower().Substring(0, 4) == "else")
                    Return.Bases.Add(lswElsePath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd(), ref SqlPositionRef));
                else
                    Return.Bases.Add(new LsName() { Name = SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd(), Lanaguage = "Sql" });
                SqlPositionRef.Position++;
            }
            return Return;
        }

        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartIn(string InLine) { return PartInRef(InLine, ref SqlPosition); }
        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartInRef(string InLine, ref SqlPositions SqlPositionRef)
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
                return lswBracketPath.Read(InLine, ref SqlPositionRef);
            return InLine;
        }

        public string Out(object OutLine) { return OutRef(OutLine, ref SqlPosition); }
        public string OutRef(object OutLine, ref SqlPositions SqlPositionRef)
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
                    else if (((lsBase)item).lsType == "LsInt")
                        Return += lswIntPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsBracket")
                        Return += lswBracketPath.Write(item, ref SqlPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsIf")
                        Return += lswIfPath.Write(item, ref SqlPositionRef);
                    else if (((lsBase)item).lsType == "LsElseIf")
                        Return += lswElseIfPath.Write(item, ref SqlPositionRef);
                    else if (((lsBase)item).lsType == "LsElse")
                        Return += lswElsePath.Write(item, ref SqlPositionRef);
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
