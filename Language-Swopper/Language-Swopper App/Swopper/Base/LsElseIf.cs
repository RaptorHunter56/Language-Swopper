using System.Collections.Generic;

namespace Base
{
    public class LsElseIf : lsBase
    {
        public LsElseIf()
        { lsType = "LsElseIf"; }
        public LsBracket Bracket;
        public List<lsBase> InerLines = new List<lsBase>();
        public int Tabindex = 0;
        public bool EndIf = false;
    }
}
