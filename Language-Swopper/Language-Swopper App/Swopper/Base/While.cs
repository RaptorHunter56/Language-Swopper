using System.Collections.Generic;

namespace Base
{
    public class LsWhile : lsBase
    {
        public LsWhile()
        { lsType = "LsWhile"; }
        public LsBracket Bracket;
        public List<lsBase> InerLines = new List<lsBase>();
        public int Tabindex = 0;
    }
}
