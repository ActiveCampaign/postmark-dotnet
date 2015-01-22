using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    /// <summary>
    /// Convenience overloads for the core PostmarkClient class.
    /// </summary>
    public static class PostmarkClientExtensions
    {

        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="from">An email address for a sender.</param>
        /// <param name="to">An email address for a recipient.</param>
        /// <param name="subject">The message subject line.</param>
        /// <param name="body">The message body.</param>
        /// <param name="headers">A collection of additional mail headers to send with the message.</param>
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction.</returns>
        public static async Task<PostmarkResponse> SendMessageAsync(this PostmarkClient client,
            string from, string to, string subject, string body, IDictionary<string, string> headers = null)
        {
            var message = new PostmarkMessage(from, to, subject, body, headers);
            return await client.SendMessageAsync(message);
        }

        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(this PostmarkClient client, IEnumerable<PostmarkMessage> messages)
        {
            return await client.SendMessagesAsync(messages.ToArray());
        }

    }
}
