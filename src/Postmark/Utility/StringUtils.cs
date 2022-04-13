using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Postmark.Utility
{
    internal static class StringUtils
    {
        /// <summary>
        ///   Takes an IEnumerable and returns an IEnumerable that has whitespace trimmed for all entries and doesn't include items with whitespace only
        /// </summary>
        internal static IEnumerable<string> TrimStringEnum(IEnumerable<string> input)
        {
            if (input == null)
                return null;

            List<string> output = new List<string>(input.Count());

            foreach (var p in input)
            {
                string result = p.Trim();
                if (!string.IsNullOrWhiteSpace(result))
                    output.Add(result);
            }

            return output;
        }
    }
}
