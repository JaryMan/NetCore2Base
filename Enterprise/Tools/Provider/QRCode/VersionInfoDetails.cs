using System.Collections.Generic;

namespace MeiMing.Framework.Provider
{
    public struct VersionInfoDetails
    {
        public Dictionary<EncodingMode, int> CapacityDict;
        public ECCLevel ErrorCorrectionLevel;
    }
}