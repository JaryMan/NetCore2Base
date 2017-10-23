using System.Collections.Generic;
using System.Text;

namespace MeiMing.Framework.Provider
{
    public class Polynom
    {
        public Polynom()
        {
            PolyItems = new List<PolynomItem>();
        }

        public List<PolynomItem> PolyItems { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            PolyItems.ForEach(x => sb.Append("a^" + x.Coefficient + "*x^" + x.Exponent + " + "));

            return sb.ToString().TrimEnd(' ', '+');
        }
    }
}