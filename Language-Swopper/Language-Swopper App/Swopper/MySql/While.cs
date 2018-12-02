using System;
using System.Collections.Generic;
using Base;

namespace LswMySql
{
    public static class lswWhilePath
    {
        public static string Write(object One, ref LswMySql.MySqlPositions MySqlPosition)
        {
            string Return = "While ";
            LsWhile Two = (LsWhile)One;
            Return += new MySqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { Two.Bracket } }).TrimEnd("\r\n".ToCharArray()) + " THEN\r\n";
            Return = Return.Replace("((", "(");
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new MySqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            Return.Trim("\r\n".ToCharArray());
            if (Two.EndWhile)
                Return += "END While;";
            return Return;
        }

        //"The name 'CountTabs' does not exist in the current context"

        public static LsWhile Read(string One, ref LswMySql.MySqlPositions MySqlPosition)
        {
            LsWhile Two = new LsWhile();
            string Three = One.Trim().Substring(2, One.Length - 6).Trim();
            //string Three = One.Trim().Substring(2,One.Length - 3).Split("THEN".ToCharArray())[0].Trim() + ")";
            Two.Bracket = (LsBracket)new MySqlControler().PartInRef(Three, ref MySqlPosition);
            bool Continu = true;
            do
            {
                MySqlPosition.Position++;
                try
                {
                    string Four = MySqlPosition.InLine[MySqlPosition.Position];
                    if (Four.Trim().ToLower() == "end While;")
                    {
                        Continu = false;
                        MySqlPosition.Position++;
                        MySqlPosition.Position++;
                        Two.EndWhile = true;
                    }
                    else if (CheckRepeate(Four, Two))
                    {
                        LsBaseList list = (LsBaseList)(new MySqlControler().In(new string[] { Four.Trim() }));
                        Two.InerLines.Add(list.Bases[0]);
                    }
                    else if (Four.Trim().ToLower().Substring(0, 4) == "else")
                    {
                        Continu = false;
                        MySqlPosition.Position++;
                        MySqlPosition.Position++;
                        Two.EndWhile = true;
                    }
                    else
                        Continu = false;
                }
                catch
                {
                    Continu = false;
                }
            } while (Continu);

            MySqlPosition.Position = MySqlPosition.Position - 1;
            return Two;
        }

        public static bool CheckRepeate(string One, LsWhile Two)
        {
            return (CountTabs(One) == Two.Tabindex + 1);
        }

        public static int CountTabs(string One)
        {
            int Prop = 0;
            foreach (char item in One)
            {
                if (item == '\t')
                    Prop++;
                else
                    break;
            }
            return Prop;
        }
    }
}