using System.Collections.Generic;

namespace Base
{
    class LsBracket : lsBase
    {
        public LsBracket()
        { BaseProperties = new Dictionary<int, LsBase>(); StringProperties = new Dictionary<int, string>(); }
        public LsBracket(BracketTypes bracketType)
        { BaseProperties = new Dictionary<int, LsBase>(); StringProperties = new Dictionary<int, string>(); BracketType = bracketType; }
        public void AddBase(LsBase Add)
        { BaseProperties.Add((BaseProperties.Count + StringProperties.Count), Add); }
        public void AddString(string Add)
        { StringProperties.Add((BaseProperties.Count + StringProperties.Count), Add); }

        public Dictionary<int, LsBase> BaseProperties;
        public Dictionary<int, string> StringProperties;
        public BracketTypes BracketType;

        public enum BracketTypes
        {
            Round,
            Square,
            Curly,
            Angle
        }
        public static Dictionary<BracketTypes, string> BracketTypesPears = new Dictionary<BracketTypes, string>()
        {
            {BracketTypes.Angle, "<|>" },
            {BracketTypes.Curly, "{|}" },
            {BracketTypes.Round, "(|)" },
            {BracketTypes.Square, "[|]" }
        };
    }
}
