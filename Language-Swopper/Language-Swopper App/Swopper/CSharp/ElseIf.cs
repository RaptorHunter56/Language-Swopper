using System;
using System.Collections.Generic;
using Base;

namespace LswCSharp
{
    public static class lswElseIfPath
    {
        public static string Write(object One, ref LswCSharp.CSharpPositions CSharpPosition)
        {
            string Return = "else if ";
            LsElseIf Two = (LsElseIf)One;
            Return += "(" + new CSharpControler().Out(new LsBaseList() { Bases = new List<lsBase>() { Two.Bracket } }).TrimEnd(")\r\n".ToCharArray()).TrimStart('(') + ")\r\n{\r\n";
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new CSharpControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            Return += "}";
            return Return;
        }

        public static LsElseIf Read(string One, ref LswCSharp.CSharpPositions CSharpPosition)
        {
            LsElseIf Two = new LsElseIf();
            Two.Tabindex = CountTabs(One);
            string Three = "(" + One.Trim().Substring(8, One.Length - 9).Trim() + ")";
            Two.Bracket = (LsBracket)new CSharpControler().PartInRef(Three, ref CSharpPosition);
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
                        if (CSharpPosition.InLine[CSharpPosition.Position + 1].Trim().ToLower().Substring(0, 4) == "else")
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