using System.Collections.Generic;

namespace Base
{
    public delegate bool ComplexCheckIn(string In, out lsBase Out);
    public delegate string ComplexCheckOut(lsBase Out);

    public class lsBase
    {
        public string lsType;
    }

    public class LsBaseList
    {
        public List<lsBase> Bases = new List<lsBase>();
    }

    public class LsName : lsBase
    {
        public LsName() { lsType = "LsName"; }
        public string Name;
        public string Lanaguage;
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