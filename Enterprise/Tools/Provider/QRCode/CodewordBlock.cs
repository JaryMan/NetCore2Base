using System.Collections.Generic;

namespace MeiMing.Framework.Provider
{
    public struct CodewordBlock
    {
        public string BitString;
        public int BlockNumber;
        public List<string> CodeWords;
        public List<string> ECCWords;
        public int GroupNumber;
    }
}