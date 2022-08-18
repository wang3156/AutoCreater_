using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommLibrary
{
    public static class CommExtend
    {
        public static bool Contains(this IEnumerable<string> t, string b, bool IgnoreCase = false)
        {
            bool r = false;

            StringComparison sc = IgnoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

            foreach (var s in t)
            {
                if (s.Equals(b, sc))
                {
                    r = true;
                    break;
                }
            }
            return r;
        }
    }
}
