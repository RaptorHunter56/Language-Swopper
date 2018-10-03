using System;
using Base;

namespace LswPython
{
    public static class lswIfPath
    {
        public static string Write(object One, ref LswPython.PythonPositions PythonPosition)
        {
            return "Write";
        }

        public static LsIf Read(string One, ref LswPython.PythonPositions PythonPosition)
        {
            LsIf Two = new LsIf();
            Two.Tabindex = CountTabs(One);
            string Three = "(" + One.Trim().Substring(3, One.Length - 4).Trim() +  ")";
            PythonControler Pcontroler = new PythonControler();
            Two.Bracket = (LsBracket)Pcontroler.PartInRef(Three, ref PythonPosition);
            bool Continu = true;
            while (Continu)
            {
                PythonPosition.Position++;
                try
                {
                    string Four = PythonPosition.InLine[PythonPosition.Position];
                    if (CheckRepeate(Four, Two))
                    {
                        LsBaseList list = (LsBaseList)(Pcontroler.InRef(new string[] { Four.Trim() }, ref PythonPosition));
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
            }

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