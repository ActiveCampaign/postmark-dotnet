using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostmarkDotNet.Legacy
{
    /// <summary>
    /// A legacy shim, proxying the 1.x client calls.
    /// </summary>
    /// <remarks>This shim is provided as a convenience to consumers of the 1.x client. 
    /// It is recommended that consumers use the 2.0 Task-based methods instead of these extensions.</remarks>
    [Obsolete("This shim is provided as a convenience to consumers of the 1.x client. " +
        "It is recommended that consumers use the 2.0 Task-based methods instead of these extensions.")]
    public static class FullProfileLegacyClientExtension
    {
        public static IAsyncResult BeginSendMessage(this PostmarkClient client, string from,
            string to, string subject, string body, NameValueCollection headers)
        {
            var headerDictionary = new Dictionary<string, string>();

            foreach (var h in headers.AllKeys)
            {
                headerDictionary[h] = headers[h];
            }

            return client.SendMessageAsync(to, from, subject, body, headerDictionary);
        }

        public static PostmarkResponse SendMessage(this PostmarkClient client, string from,
            string to, string subject, string body, NameValueCollection headers)
        {
            var headerDictionary = new Dictionary<string, string>();

            foreach (var h in headers.AllKeys)
            {
                headerDictionary[h] = headers[h];
            }

            return client.SendMessageAsync(to, from, subject, body, headerDictionary).WaitForResult();
        }
    }
}
