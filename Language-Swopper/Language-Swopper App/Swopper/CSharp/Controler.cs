//||||||||||
//C# files (*.cs)|*.cs|Text files (*.txt)|*.txt|All files (*.*)|*.*
//||||||||||
using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswCSharp
{
    public class CSharpPositions
    {
        public string[] InLine;
        public int Position = 0;
    }
    public class CSharpControler
    {
        public CSharpControler()
        { CSharpPosition = new CSharpPositions(); }

        public CSharpPositions CSharpPosition;
        public object In(string[] InLine) { CSharpPosition.InLine = InLine; return InRef(InLine, ref CSharpPosition); }
        public object InRef(string[] InLine, ref CSharpPositions CSharpPositionRef)
        {
            LsBaseList Return = new LsBaseList();
            while (CSharpPositionRef.Position < CSharpPositionRef.InLine.Length)
            {

                Regex stringrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}string {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex boolrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}bool {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex charrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}char {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex intrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}int {1,}\w+ {0,}= {0,}.+ {0,};");

                if (stringrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else if(boolrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBoolPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else if (charrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswCharPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else if (intrgx.Match(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIntPath.Read(CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd()));
                else
                    Return.Bases.Add(new LsName() { Name = CSharpPositionRef.InLine[CSharpPositionRef.Position].TrimEnd(), Lanaguage = "C#" });
                CSharpPositionRef.Position++;
            }
            return Return;
        }

        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartIn(string InLine) { return PartInRef(InLine, ref CSharpPosition); }
        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartInRef(string InLine, ref CSharpPositions CSharpPositionRef)
        {
            
            return InLine;
        }

        public string Out(object OutLine) { return OutRef(OutLine, ref CSharpPosition); }
        public string OutRef(object OutLine, ref CSharpPositions CSharpPositionRef)
        {
            LsBaseList baseList = (LsBaseList)OutLine;
            string Return = "";
            foreach (var item in baseList.Bases)
            {
                try
                {
                    if (((lsBase)item).lsType == "LsString")
                        Return += lswStringPath.Write(item) + " {LsString} " + "\r";
                    else if (((lsBase)item).lsType == "LsBool")
                        Return += lswBoolPath.Write(item) + " {LsBool} " + "\r";
                    else if (((lsBase)item).lsType == "LsChar")
                        Return += lswCharPath.Write(item) + " {LsChar} " + "\r";
                    else if (((lsBase)item).lsType == "LsInt")
                        Return += lswIntPath.Write(item) + " {LsInt} " + "\r";
                    else if (((lsBase)item).lsType == "LsName")
                        try { Return += ((LsName)item).Lanaguage + " Doesn't Have a conversion file for this." + "\r"; } catch { Return += "{No_Type}" + "\r"; }
                    else
                        try { Return += item.ToString() + " {No_Type} " + "\r"; } catch { Return += "{No_Type}" + "\r"; }
                }
                catch (Exception e)
                {
                    Return += e.Message + "\r";
                }
            }
            return Return;
        }
    }
}
