using System.Collections.Generic;

namespace Base
{
    class LsBrackets : lsBase
    {
        public Dictionary<int, LsBase> BaseProperties;
        public Dictionary<int, string> StringProperties;
        public BracketType BracketType;

        public enum BracketType
        {
            Round,
            Square,
            Curly,
            Angle
        }
    }
}
