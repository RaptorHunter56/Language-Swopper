using System.Collections.Generic;

namespace Base
{
    public class LsBracket : lsBase
    {
        public LsBracket()
        { BaseProperties = new Dictionary<int, object>(); StringProperties = new Dictionary<int, LsName>(); BracketType = BracketTypes.Round; lsType = "LsBracket"; }
        public LsBracket(BracketTypes bracketType)
        { BaseProperties = new Dictionary<int, object>(); StringProperties = new Dictionary<int, LsName>(); BracketType = bracketType; lsType = "LsBracket"; }
        public void AddBase(object Add)
        { BaseProperties.Add((BaseProperties.Count + StringProperties.Count), Add);}
        public void AddString(LsName Add)
        { StringProperties.Add((BaseProperties.Count + StringProperties.Count), Add);}

        public Dictionary<int, object> BaseProperties;
        public Dictionary<int, LsName> StringProperties;
        public BracketTypes BracketType;

        public enum BracketTypes
        {
            Round,
            Square,
            Curly,
            Angle
        }
        public Dictionary<BracketTypes, string> BracketTypesPears = new Dictionary<BracketTypes, string>()
        {
            {BracketTypes.Angle, "<|>" },
            {BracketTypes.Curly, "{|}" },
            {BracketTypes.Round, "(|)" },
            {BracketTypes.Square, "[|]" }
        };

    }
    public static class Dic
    {
        public static Dictionary<LsBracket.BracketTypes, string> dic = new Dictionary<LsBracket.BracketTypes, string>()
        {
            {LsBracket.BracketTypes.Angle, "<|>" },
            {LsBracket.BracketTypes.Curly, "{|}" },
            {LsBracket.BracketTypes.Round, "(|)" },
            {LsBracket.BracketTypes.Square, "[|]" }
        };
        public static LsBracket.BracketTypes KeyByValue(string val)
        {
            LsBracket.BracketTypes key = LsBracket.BracketTypes.Round;
            foreach (KeyValuePair<LsBracket.BracketTypes, string> pair in dic)
            {
                if (pair.Value == val)
                {
                    key = pair.Key;
                    break;
                }
            }
            return key;
        }
    }
}
