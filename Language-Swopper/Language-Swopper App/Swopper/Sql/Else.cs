using System;
using System.Collections.Generic;
using Base;

namespace LswSql
{
    public static class lswElsePath
    {
        public static string Write(object One, ref LswSql.SqlPositions SqlPosition)
        {
            string Return = "ELSE\r\n";
            LsElse Two = (LsElse)One;
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new SqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            Return += "END IF;\r\n";
            return Return;
        }

        public static LsElse Read(string One, ref LswSql.SqlPositions SqlPosition)
        {
            LsElse Two = new LsElse();
            Two.Tabindex = CountTabs(One);
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
                        Two.EndIf = true;
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

        public static bool CheckRepeate(string One, LsElse Two)
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