using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    public static class NameValueCollectionCompatibilityExtensions
    {
        /// <summary>
        /// Converts a NameValueCollection 
        /// (as is used in the 1.x client) 
        /// to the HeaderCollection required for the 2.0 client.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static HeaderCollection AsHeaderCollection(this NameValueCollection collection)
        {
            var retval = new HeaderCollection();
            if (collection != null)
            {
                foreach (var key in collection.AllKeys)
                {
                    retval[key] = collection[key];
                }
            }
            return retval;
        }
    }
}
