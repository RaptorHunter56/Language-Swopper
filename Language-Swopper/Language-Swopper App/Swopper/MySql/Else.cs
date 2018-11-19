using System;
using System.Collections.Generic;
using Base;

namespace LswMySql
{
    public static class lswElsePath
    {
        public static string Write(object One, ref LswMySql.MySqlPositions MySqlPosition)
        {
            string Return = "ELSE\r\n";
            LsElse Two = (LsElse)One;
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new MySqlControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            Return += "END IF;\r\n";
            return Return;
        }

        public static LsElse Read(string One, ref LswMySql.MySqlPositions MySqlPosition)
        {
            LsElse Two = new LsElse();
            Two.Tabindex = CountTabs(One);
            bool Continu = true;
            do
            {
                MySqlPosition.Position++;
                try
                {
                    string Four = MySqlPosition.InLine[MySqlPosition.Position];
                    if (Four.Trim().ToLower() == "end if;")
                    {
                        Continu = false;
                        MySqlPosition.Position++;
                        MySqlPosition.Position++;
                        Two.EndIf = true;
                    }
                    else if (CheckRepeate(Four, Two))
                    {
                        LsBaseList list = (LsBaseList)(new MySqlControler().In(new string[] { Four.Trim() }));
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

            MySqlPosition.Position = MySqlPosition.Position - 1;
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