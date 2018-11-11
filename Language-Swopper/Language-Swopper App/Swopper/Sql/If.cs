using System;
using System.Collections.Generic;
using Base;

namespace LswSql
{
    public static class lswIfPath
    {
        public static string Write(object One, ref LswSql.SqlPositions SqlPosition)
        {
            string Return = "IF ";
            LsIf Two = (LsIf)One;
            Return += new SqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { Two.Bracket } }).TrimEnd("\r\n".ToCharArray()) + " THEN\r\n";
            Return = Return.Replace("((", "(");
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new SqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            Return += "END IF;\r\n";
            return Return;
        }

        //"The name 'CountTabs' does not exist in the current context"

        public static LsIf Read(string One, ref LswSql.SqlPositions SqlPosition)
        {
            LsIf Two = new LsIf();
            string Three =  One.Trim().Substring(2, One.Length - 6).Trim();
            //string Three = One.Trim().Substring(2,One.Length - 3).Split("THEN".ToCharArray())[0].Trim() + ")";
            Two.Bracket = (LsBracket)new SqlControler().PartInRef(Three, ref SqlPosition);
            bool Continu = true;
            do
            {
                SqlPosition.Position++;
                try
                {
                    string Four = SqlPosition.InLine[SqlPosition.Position];
                    if (Four.Trim().ToLower() == "end if;")
                    {
                        Continu = false;
                        SqlPosition.Position++;
                        SqlPosition.Position++;
                    }
                    else if (CheckRepeate(Four, Two))
                    {
                        LsBaseList list = (LsBaseList)(new SqlControler().In(new string[] { Four.Trim() }));
                        Two.InerLines.Add(list.Bases[0]);
                    }
                    else
                        Continu = false;
                }
                catch
                {
                    Continu = false;
                }
            } while (Continu);

            SqlPosition.Position = SqlPosition.Position - 1;
            return Two;
        }

        public static bool CheckRepeate(string One, LsIf Two)
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