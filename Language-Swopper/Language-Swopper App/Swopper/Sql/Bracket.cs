using System;
using Base;

namespace LswSql
{
    public static class lswBracketPath
    {
        public static string Write(object One, ref LswSql.SqlPositions SqlPosition)
        {
            string Return = "";
            LsBracket Two = (LsBracket)One;
            Return += Two.BracketTypesPears[Two.BracketType][0];
            int Point = 0;
            while (Two.BaseProperties.Count + Two.StringProperties.Count > Point)
            {
                object ToAdd;
                LsName ToAdd2;
                if (Two.BaseProperties.TryGetValue(Point, out ToAdd))
                {
                    if (ToAdd.GetType() == typeof(string))
                        Return += (ToAdd.ToString().Contains("\"")) ? "@" + '"' + DoubleSpeatch(ToAdd.ToString()) + '"' : '"' + ToAdd.ToString() + @"""";
                    else if (ToAdd.GetType() == typeof(char))
                        Return += (ToAdd.ToString().Contains("'")) ? "'\\''" : "'" + ToAdd.ToString() + @"''";
                    else if (ToAdd.GetType() == typeof(int))
                        Return += ToAdd.ToString() + "{int}";
                    else if (ToAdd.GetType() == typeof(bool))
                        Return += ToAdd.ToString().ToLower();
                    else if (ToAdd.GetType() == typeof(LsBracket))
                        Return += Write(ToAdd, ref SqlPosition);
                    Return += Seperater(Two.BracketType) + " ";
                }
                else if (Two.StringProperties.TryGetValue(Point, out ToAdd2))
                    Return += ToAdd2.Name + Seperater(Two.BracketType) + " ";
                Point++;
            }
            Return = Return.TrimEnd();
            Return = Return.Substring(0, Return.Length - 1);
            Return += ")";
            return Return;
        }

        public static string DoubleSpeatch(string InString)
        {
            string Return = "";
            foreach (var item in InString)
            {
                if (item == '"')
                    Return += '"' + '"';
                else
                    Return += item;
            }
            return Return;
        }

        public static string Seperater(LsBracket.BracketTypes type)
        {
            switch (type)
            {
                case LsBracket.BracketTypes.Angle:
                case LsBracket.BracketTypes.Round:
                case LsBracket.BracketTypes.Square:
                    return ",";
                case LsBracket.BracketTypes.Curly:
                    return "/n/t";
                default:
                    return ",";
            }
        }

        public static LsBracket Read(string One, ref LswSql.SqlPositions SqlPosition)
        {
            LsBracket Two = new LsBracket();
            string Three = One[0].ToString();
            switch (Three)
            {
                case "<":
                    Three = Three + "|>";
                    break;
                case "(":
                    Three = Three + "|)";
                    break;
                case "[":
                    Three = Three + "|]";
                    break;
                case "{":
                    Three = Three + "|}";
                    break;
                default:
                    Three = "(|)";
                    break;
            }
            if (Two.BracketTypesPears.ContainsValue(Three))
                Two.BracketType = Dic.KeyByValue(Three); // //
            else
                Two.BracketType = LsBracket.BracketTypes.Round;
            //do
            //{
                string[] Parts;
                Parts = One.Trim().Substring(1, One.Length - 2).Split(Seperater(Two.BracketType).ToCharArray());
                foreach (var item in Parts)
                {
                    string itemT = item.Trim();
                    var Four = new SqlControler().PartInRef(itemT, ref SqlPosition);
                    if (Four is string && (itemT[0] != '"' && itemT[0] != "'"[0]))
                        Two.AddString(new LsName() { Name = Four.ToString() });
                    else
                        Two.AddBase(Four);
                }
                SqlPosition.Position++;
                try
                { One = SqlPosition.InLine[SqlPosition.Position]; }
                catch { }
            //} while(CheckRepeate(One, Two));
            SqlPosition.Position = SqlPosition.Position - 1;
            return Two;
        }

        public static bool CheckRepeate(string One, LsBracket Two)
        {
            if (Two.BracketType != LsBracket.BracketTypes.Curly)
                return (Two.BracketTypesPears[Two.BracketType][Two.BracketTypesPears[Two.BracketType].Length - 1] != One[One.Length - 1]);
            else
            {
                //int StartOne = 0;
                //foreach (var item in One)
                //{
                //    if (item == '\t')
                //        StartOne++;
                //    else
                //        break;
                //}
                //Seven = SqlControler.requestLine();
                //int StartSeven = 0;
                //foreach (var item in One)
                //{
                //    if (item == '\t')
                //        StartSeven++;
                //    else
                //        break;
                //}
                //SqlControler.backLine();
                //return (StartOne == StartSeven);
                return false;
            }
        }
    }
}