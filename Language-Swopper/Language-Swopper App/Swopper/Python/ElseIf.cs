using System;
using System.Collections.Generic;
using Base;

namespace LswPython
{
    public static class lswElseIfPath
    {
        public static string Write(object One, ref LswPython.PythonPositions PythonPosition)
        {
            string Return = "elif ";
            LsElseIf Two = (LsElseIf)One;
            Return += new PythonControler().Out(new LsBaseList() { Bases = new List<lsBase>() { Two.Bracket } }).TrimEnd(")\r\n".ToCharArray()).TrimStart('(') + ":\r\n";
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new PythonControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            return Return;
        }

        public static LsElseIf Read(string One, ref LswPython.PythonPositions PythonPosition)
        {
            LsElseIf Two = new LsElseIf();
            Two.Tabindex = CountTabs(One);
            string Three = "(" + One.Trim().Substring(5, One.Length - 6).Trim() +  ")";
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
                    else if (Four.Trim().ToLower().Substring(0, 4) != "elif" || Four.Trim().ToLower().Substring(0, 4) != "else")
                    {
                        Continu = false;
                        Two.EndIf = false;
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

        public static bool CheckRepeate(string One, LsElseIf Two)
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