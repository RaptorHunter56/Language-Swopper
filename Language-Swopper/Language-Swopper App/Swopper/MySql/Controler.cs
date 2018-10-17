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

                Regex stringrgx = new Regex(@"(s|S)(e|E)(t|T) .+ {0,}= {0,}'(([^']{0,}[\\]'[^'\\]{0,}){1,}|([^']{0,}''[^']{0,}){1,}|[^']{1,})';{0,1}");

                if (stringrgx.Match(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd()));
                else
                    Return.Bases.Add(new LsName() { Name = MySqlPositionRef.InLine[MySqlPositionRef.Position].TrimEnd() });
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
                        Return += lswStringPath.Write(item) + " {LsString} " + "\r";
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