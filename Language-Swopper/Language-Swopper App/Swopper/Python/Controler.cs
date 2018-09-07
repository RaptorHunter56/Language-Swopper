using System;
using LswPython;

namespace Base
{
    public static class Controler
    {
        public Lsw In = Lsw.Python;
        public Lsw Out = Lsw.Python;
        public static enum Lsw
        {
            Python
        }
        public string line(string Line)
        {
            object InLine;
            switch (In)
            {
                case Lsw.Python:
                    
                    break;
                default:
                    break;
            }
            string OutLine;
            switch (Out)
            {
                case Lsw.Python:
                    break;
                default:
                    break;
            }
        }
    }
}
