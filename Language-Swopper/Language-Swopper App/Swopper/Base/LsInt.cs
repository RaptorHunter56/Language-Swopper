using System.Collections.Generic;

namespace Base
{
    public class LsInt : lsBase
    {
        public LsInt(string name)
        { Name = name; Value = null; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsInt"; }
        public LsInt(string name, int value)
        { Name = name; Value = value; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsInt"; }
        public LsInt(string name, int value, List<Prefix> prefixes)
        { Name = name; Value = value; Prefixes = prefixes; ValueType = false; Decleared = false; base.lsType = "LsInt"; }

        public LsInt(string name, string value)
        { Name = name; ValueT = value; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsInt"; }
        public LsInt(string name, string value, List<Prefix> prefixes)
        { Name = name; ValueT = value; Prefixes = prefixes; ValueType = false; Decleared = false; base.lsType = "LsInt"; }

        public string Name { get; set; }
        public int? Value { get; set; }
        public string ValueT { get; set; }
        public List<Prefix> Prefixes { get; set; }
        public bool ValueType { get; set; }
        public bool Decleared { get; set; }
    }
}
