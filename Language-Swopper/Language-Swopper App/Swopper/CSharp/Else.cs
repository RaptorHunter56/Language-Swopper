using System;
using System.Collections.Generic;
using Base;

namespace LswCSharp
{
    public static class lswElsePath
    {
        public static string Write(object One, ref LswCSharp.CSharpPositions CSharpPosition)
        {
            string Return = "else ";
            LsElse Two = (LsElse)One;
            Return += "\r\n{\r\n";
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new CSharpControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            Return += "}";
            return Return;
        }

        public static LsElse Read(string One, ref LswCSharp.CSharpPositions CSharpPosition)
        {
            LsElse Two = new LsElse();
            Two.Tabindex = CountTabs(One);
            bool Continu = true;
            do
            {
                CSharpPosition.Position++;
                try
                {
                    string Four = CSharpPosition.InLine[CSharpPosition.Position];
                    if (Four.Trim() == "}")
                    {
                        Continu = false;
                        CSharpPosition.Position++;
                        Two.EndIf = true;
                    }
                    else if (Four.Trim() == "{")
                        Continu = true;
                    else if (CheckRepeate(Four, Two))
                    {
                        LsBaseList list = (LsBaseList)(new CSharpControler().In(new string[] { Four.Trim() }));
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

            CSharpPosition.Position = CSharpPosition.Position - 1;
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