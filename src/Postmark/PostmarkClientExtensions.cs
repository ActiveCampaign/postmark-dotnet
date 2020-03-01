using PostmarkDotNet.Model;
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
        /// <param name="client">The postmark client instance on which to do the send.</param>
        /// <param name="from">An email address for a sender.</param>
        /// <param name="to">An email address for a recipient.</param>
        /// <param name="subject">The message subject line.</param>
        /// <param name="textBody">The Plain Text Body to be used for the message, this may be null if HtmlBody is set.</param>
        /// <param name="htmlBody">The HTML Body to be used for the message, this may be null if TextBody is set.</param>
        /// <param name="headers">A collection of additional mail headers to send with the message.</param>
        /// <param name="messageStream">The message stream used to send this message</param>
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction.</returns>
        public static async Task<PostmarkResponse> SendMessageAsync(this PostmarkClient client,
            string from, string to, string subject, string textBody, string htmlBody,
             IDictionary<string, string> headers = null,
             IDictionary<string, string> metadata = null,
             string messageStream = null)
        {
            var message = new PostmarkMessage(from, to, subject, textBody, htmlBody,
            new HeaderCollection(headers), metadata, messageStream);
            return await client.SendMessageAsync(message);
        }

        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="client">The client to use when sending the batch.</param>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        public static async Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(this PostmarkClient client, IEnumerable<PostmarkMessage> messages)
        {
            return await client.SendMessagesAsync(messages.ToArray());
        }
    }
}