using System;
using System.Collections.Generic;
using Base;

namespace LswMySql
{
    public static class lswIfPath
    {
        public static string Write(object One, ref LswMySql.MySqlPositions MySqlPosition)
        {
            string Return = "if ";
            //LsIf Two = (LsIf)One;
            //Return += new MySqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { Two.Bracket } }).TrimEnd(")\r\n".ToCharArray()).TrimStart('(') + ":\r\n";
            //foreach (var item in Two.InerLines)
            //{
            //    Return += "\t" + new MySqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            //}
            return Return;
        }

        public static LsIf Read(string One, ref LswMySql.MySqlPositions MySqlPosition)
        {
            LsIf Two = new LsIf();
            //string Three = "(" + One.Trim().Substring(3, One.Length - 4).Trim() +  ")";
            string Three = One.Trim().Substring(2,One.Length - 3).Split(',')[0].Trim() + ")";
            Two.Bracket = (LsBracket)new MySqlControler().PartInRef(Three, ref MySqlPosition);
            Two.InerLines.Add((lsBase)new MySqlControler().PartIn(One.Split(',')[1].Trim()));
            ///FIX Add Else
            return Two;
        }

        public static bool CheckRepeate(string One, LsIf Two)
        {
            return (CountTabs(One) == Two.Tabindex + 1);
        }
    }
}