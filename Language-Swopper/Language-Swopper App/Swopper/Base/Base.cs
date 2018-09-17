using System.Collections.Generic;

namespace Base
{
    public delegate bool ComplexCheckIn(string In, out lsBase Out);
    public delegate string ComplexCheckOut(lsBase Out);

    public class lsBase
    {
        public string lsType;
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