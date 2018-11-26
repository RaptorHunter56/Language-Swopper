using System;
using System.Collections.Generic;
using Base;

namespace LswPython
{
    public static class lswElsePath
    {
        public static string Write(object One, ref LswPython.PythonPositions PythonPosition)
        {
            string Return = "else:\r\n";
            LsElse Two = (LsElse)One;
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new PythonControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            return Return;
        }

        public static LsElse Read(string One, ref LswPython.PythonPositions PythonPosition)
        {
            LsElse Two = new LsElse();
            Two.Tabindex = CountTabs(One);
            bool Continu = true;
            do
            {
                PythonPosition.Position++;
                try
                {
                    string Four = PythonPosition.InLine[PythonPosition.Position];
                    if (CheckRepeate(Four, Two))
                    {
                        LsBaseList list = (LsBaseList)(new PythonControler().In(new string[] { Four.Trim() }));
                        Two.InerLines.Add(list.Bases[0]);
                    }
                    else
                    {
                        Continu = false;
                    }
                }
                catch
                {
                    Continu = false;
                }
            } while (Continu);

            PythonPosition.Position = PythonPosition.Position - 1;
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