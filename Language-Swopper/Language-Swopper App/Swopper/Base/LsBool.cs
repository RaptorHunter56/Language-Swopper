using System.Collections.Generic;

namespace Base
{
    public class LsBool : lsBase
    {
        public LsBool(string name)
        { Name = name; Value = null; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsBool"; }
        public LsBool(string name, bool value)
        { Name = name; Value = value; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsBool"; }
        public LsBool(string name, string value)
        { Name = name; ValueT = value; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsBool"; }
        public LsBool(string name, bool value, List<Prefix> prefixes)
        { Name = name; Value = value; Prefixes = prefixes; ValueType = false; Decleared = false; base.lsType = "LsBool"; }

        public string Name { get; set; }
        public bool? Value { get; set; }
        public string ValueT { get; set; }
        public List<Prefix> Prefixes { get; set; }
        public bool ValueType { get; set; }
        public bool Decleared { get; set; }
    }
}
