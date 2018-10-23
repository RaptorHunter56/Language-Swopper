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

                Regex stringrgx = new Regex(@"(s|S)(e|E)(t|T) .+ {0,}= {0,}'(([^']{0,}[\\]'[^'\\]{0,}){1,}|([^']{0,}''[^']{0,}){1,}|[^']{1,})';{0,1}");
                Regex boolrgx = new Regex(@"(s|S)(e|E)(t|T) .+ {0,}= {0,}((t|T)(r|R)(u|U)(e|E)|(f|F)(a|A)(l|L)(s|S)(e|E));{0,1}");

                if (stringrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()));
                else if(boolrgx.Match(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBoolPath.Read(SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd()));
                else
                    Return.Bases.Add(new LsName() { Name = SqlPositionRef.InLine[SqlPositionRef.Position].TrimEnd(), Lanaguage = "C#" });
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
                        Return += lswStringPath.Write(item) + " {LsString} " + "\r";
                    else if (((lsBase)item).lsType == "LsBool")
                        Return += lswBoolPath.Write(item) + " {LsBool} " + "\r";
                    else if (((lsBase)item).lsType == "LsName")
                        try { Return += item.Lanaguage + " Doesn't Have a conversion file for this." + "\r"; } catch { Return += "{No_Type}" + "\r"; }
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
