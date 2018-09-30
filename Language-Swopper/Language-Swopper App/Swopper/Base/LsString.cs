using System.Collections.Generic;

namespace Base
{
    public class LsString : lsBase
    {
        public LsString(string name)
        { Name = name; Value = null; Prefixes = new List<Prefix>(); ValueType = false; ComplexType = false; Decleared = false; base.lsType = "LsString"; }
        public LsString(string name, string value)
        { Name = name; Value = value; Prefixes = new List<Prefix>(); ValueType = false; ComplexType = false; Decleared = false; base.lsType = "LsString"; }
        public LsString(string name, string value, List<Prefix> prefixes)
        { Name = name; Value = value; Prefixes = prefixes; ValueType = false; ComplexType = false; Decleared = false; base.lsType = "LsString"; }
        
        public void IsComplex(string Line)
        {
            try
            {
                //lsBase Out;
                //if (false)/*ComplexChecking(Line, out Out))*/
                //{
                //    Complex = Out;
                //    ComplexType = true;
                //}
                //else
                //{
                    Value = Line;
                    ValueType = true;
                //}
            }
            catch
            {
                Value = Line;
                ValueType = true;
            }
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public lsBase Complex { get; set; }
        public List<Prefix> Prefixes { get; set; }
        public bool ValueType { get; set; }
        public bool ComplexType { get; set; }
        public bool Decleared { get; set; }
    }
}
