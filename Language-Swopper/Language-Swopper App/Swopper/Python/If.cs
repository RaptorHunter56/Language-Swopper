using System;
using System.Collections.Generic;
using Base;

namespace LswPython
{
    public static class lswIfPath
    {
        public static string Write(object One, ref LswPython.PythonPositions PythonPosition)
        {
            string Return = "if ";
            LsIf Two = (LsIf)One;
            Return += new PythonControler().Out(new LsBaseList() { Bases = new List<lsBase>() { Two.Bracket } }).TrimEnd(")\r\n".ToCharArray()).TrimStart('(') + ":\r\n";
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new PythonControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            return Return;
        }

        public static LsIf Read(string One, ref LswPython.PythonPositions PythonPosition)
        {
            LsIf Two = new LsIf();
            Two.Tabindex = CountTabs(One);
            string Three = "(" + One.Trim().Substring(3, One.Length - 4).Trim() +  ")";
            Two.Bracket = (LsBracket)new PythonControler().PartInRef(Three, ref PythonPosition);
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
                catch (Exception e)
                {
                    Continu = false;
                }
            } while (Continu);

            PythonPosition.Position = PythonPosition.Position - 1;
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