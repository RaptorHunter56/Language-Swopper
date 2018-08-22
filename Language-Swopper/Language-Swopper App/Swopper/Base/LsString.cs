using System.Collections.Generic;

namespace Base
{
    public class LsString
    {
        public LsString(string name)
        { Name = name; Value = null; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; }
        public LsString(string name, string value)
        { Name = name; Value = value; Prefixes = new List<Prefix>(); ValueType = false; Decleared = false; }
        public LsString(string name, string value, List<Prefix> prefixes)
        { Name = name; Value = value; Prefixes = prefixes; ValueType = false; Decleared = false; }

        public string Name { get; set; }
        public string Value { get; set; }
        public List<Prefix> Prefixes { get; set; }
        public bool ValueType { get; set; }
        public bool Decleared { get; set; }


        public static readonly string Type = "LsString";
    }

    public enum Prefix
    {
        @public,
        @private,
        @static,
        @readonly,
        @internal,
        @protected
    }
}
