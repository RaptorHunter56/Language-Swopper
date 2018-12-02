//||||||||||
//C# files (*.cs)|*.cs|Text files (*.txt)|*.txt|All files (*.*)|*.*
//||||||||||
using System;
using Base;
using System.Text;
using System.Text.RegularExpressions;

namespace LswVB
{
    public class VBPositions
    {
        public string[] InLine;
        public int Position = 0;
    }
    public class VBControler
    {
        public VBControler()
        { VBPosition = new VBPositions(); }

        public VBPositions VBPosition;
        public object In(string[] InLine) { VBPosition.InLine = InLine; return InRef(InLine, ref VBPosition); }
        public object InRef(string[] InLine, ref VBPositions VBPositionRef)
        {
            LsBaseList Return = new LsBaseList();
            while (VBPositionRef.Position < VBPositionRef.InLine.Length)
            {

                Regex stringrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}string {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex boolrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}bool {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex charrgx = new Regex(@"(public |protected |private |static |readonly |internal ){0,} {0,}char {1,}\w+ {0,}= {0,}.+ {0,};");
                Regex ifrgx = new Regex(@"^if [(].+[)]");

                if (stringrgx.Match(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswStringPath.Read(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd()));
                else if(boolrgx.Match(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswBoolPath.Read(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd()));
                else if (charrgx.Match(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswCharPath.Read(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd()));
                else if (ifrgx.Match(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd()).Success)
                    Return.Bases.Add(lswIfPath.Read(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd(), ref VBPositionRef));
                else if (VBPositionRef.InLine[VBPositionRef.Position].TrimEnd() == "else")
                    Return.Bases.Add(lswElsePath.Read(VBPositionRef.InLine[VBPositionRef.Position].TrimEnd(), ref VBPositionRef));
                else
                    Return.Bases.Add(new LsName() { Name = VBPositionRef.InLine[VBPositionRef.Position].TrimEnd(), Lanaguage = "VB" });
                VBPositionRef.Position++;
            }
            return Return;
        }

        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartIn(string InLine) { return PartInRef(InLine, ref VBPosition); }
        /// <summary>
        /// For When you need to check the type of an input.    
        /// Eg. "Sam" == String, 56 == Int ...
        /// </summary>
        /// <param name="InLine"></param>
        /// <returns></returns>
        public object PartInRef(string InLine, ref VBPositions VBPositionRef)
        {
            Regex stringrgx = new Regex(@"^""{3}[^""]+""{3}$|^""[^""]+""$");
            Regex charrgx = new Regex(@"^'[^']{1}'$|^'\\''$");
            Regex boolrgx = new Regex(@"^true$|^false$");

            if (charrgx.Match(InLine).Success)
                return InLine.Substring(1, InLine.ToString().Length - 2);
            else if (stringrgx.Match(InLine).Success)
            {
                if (InLine.ToString().Length > 6 && InLine.Substring(0, 3) == "\"\"\"" && InLine.Substring(InLine.ToString().Length - 3, 3) == "\"\"\"")
                    return (InLine.ToString().Length == 7) ? InLine.Substring(3, InLine.ToString().Length - 6) : InLine.Substring(3, InLine.ToString().Length - 6);
                else if (InLine.ToString().Length > 2 && InLine.Substring(0, 1) == "\"" && InLine.Substring(InLine.ToString().Length - 1, 1) == "\"")
                    return (InLine.ToString().Length == 3) ? InLine.Substring(1, InLine.ToString().Length - 2) : InLine.Substring(1, InLine.ToString().Length - 2);
            }
            else if (boolrgx.Match(InLine).Success)
                return (InLine[0] == 't') ? true : false;
            return InLine;
        }

        public string Out(object OutLine) { return OutRef(OutLine, ref VBPosition); }
        public string OutRef(object OutLine, ref VBPositions VBPositionRef)
        {
            LsBaseList baseList = (LsBaseList)OutLine;
            string Return = "";
            foreach (var item in baseList.Bases)
            {
                try
                {
                    if (((lsBase)item).lsType == "LsString")
                        Return += lswStringPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsBool")
                        Return += lswBoolPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsChar")
                        Return += lswCharPath.Write(item) + "\r\n";
                    else if (((lsBase)item).lsType == "LsIf")
                        Return += lswIfPath.Write(item, ref VBPositionRef) + "\r\n";
                    else if (((lsBase)item).lsType == "LsName")
                        try { Return += ((LsName)item).Lanaguage + " Doesn't Have a conversion file for this." + "\r\n"; } catch { Return += "{No_Type}" + "\r\n"; }
                    else
                        try { Return += item.ToString() + " {No_Type} " + "\r\n"; } catch { Return += "{No_Type}" + "\r\n"; }
                }
                catch (Exception e)
                {
                    Return += e.Message + "\r\n";
                }
            }
            return Return;
        }
    }
}
