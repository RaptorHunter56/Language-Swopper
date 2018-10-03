using System.Collections.Generic;

namespace Base
{
    public class LsIf : lsBase
    {
        public LsIf()
        { lsType = "LsIf"; }
        public LsBracket Bracket;
        public List<lsBase> InerLines = new List<lsBase>();
        public int Tabindex = 0;
    }
}
