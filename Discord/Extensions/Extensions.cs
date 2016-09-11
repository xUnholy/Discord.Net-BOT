using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Extensions
{
    public static class Extensions
    {
        public static bool ContainsAny(this string input, params string[] strToCheck)
        {
            foreach (string toFind in strToCheck)
            {
                if (input.Contains(toFind))
                    return true;
            }
            return false;
        }
    }
}
