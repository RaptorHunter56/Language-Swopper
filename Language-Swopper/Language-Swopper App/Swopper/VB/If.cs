using System;
using System.Collections.Generic;
using Base;

namespace LswVB
{
    public static class lswIfPath
    {
        public static string Write(object One, ref LswVB.VBPositions VBPosition)
        {
            string Return = "if ";
            LsIf Two = (LsIf)One;
            Return += "(" + new VBControler().Out(new LsBaseList() { Bases = new List<lsBase>() { Two.Bracket } }).TrimEnd(")\r\n".ToCharArray()).TrimStart('(') + ")\r\n{\r\n";
            foreach (var item in Two.InerLines)
            {
                Return += "\t" + new VBControler().Out(new LsBaseList() { Bases = new List<lsBase>() { item } });
            }
            Return += "}";
            return Return;
        }

        public static LsIf Read(string One, ref LswVB.VBPositions VBPosition)
        {
            LsIf Two = new LsIf();
            Two.Tabindex = CountTabs(One);
            string Three = "(" + One.Trim().Substring(3, One.Length - 4).Trim() + ")";
            Two.Bracket = (LsBracket)new VBControler().PartInRef(Three, ref VBPosition);
            bool Continu = true;
            do
            {
                VBPosition.Position++;
                try
                {
                    string Four = VBPosition.InLine[VBPosition.Position];
                    if (Four.Trim() == "}")
                    {
                        Continu = false;
                        VBPosition.Position++;
                        if (VBPosition.InLine[VBPosition.Position + 1].Trim().ToLower().Substring(0, 4) == "else")
                            Two.EndIf = true;
                    }
                    else if(Four.Trim() == "{")
                        Continu = true;
                    else if (CheckRepeate(Four, Two))
                    {
                        LsBaseList list = (LsBaseList)(new VBControler().In(new string[] { Four.Trim() }));
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

            VBPosition.Position = VBPosition.Position - 1;
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