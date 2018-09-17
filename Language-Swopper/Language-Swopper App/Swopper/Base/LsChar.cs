using System.Collections.Generic;

namespace Base
{
    public class LsChar : lsBase
    {
        public LsChar(string name)
        { Name = name; Value = null; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsChar"; }
        public LsChar(string name, char value)
        { Name = name; Value = value; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsChar"; }
        public LsChar(string name, char value, List<Prefix> prefixes)
        { Name = name; Value = value; Prefixes = prefixes; ValueType = false; Decleared = false; base.lsType = "LsChar"; }

        public LsChar(string name, string value)
        { Name = name; ValueT = value; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; base.lsType = "LsChar"; }
        public LsChar(string name, string value, List<Prefix> prefixes)
        { Name = name; ValueT = value; Prefixes = prefixes; ValueType = false; Decleared = false; base.lsType = "LsChar"; }

        public string Name { get; set; }
        public char? Value { get; set; }
        public string ValueT { get; set; }
        public List<Prefix> Prefixes { get; set; }
        public bool ValueType { get; set; }
        public bool Decleared { get; set; }
    }
}
