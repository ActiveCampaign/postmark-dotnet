using System;
using System.Collections.Generic;
using System.Linq;
using Hammock;
using Hammock.Silverlight.Compat;

namespace PostmarkDotNet
{
    public partial class PostmarkClient
    {
        /// <summary>
        ///   Sends a message through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name = "from">An email address for a sender.</param>
        /// <param name = "to">An email address for a recipient.</param>
        /// <param name = "subject">The message subject line.</param>
        /// <param name = "body">The message body.</param>
        /// <param name="callback">The callback invoked when a <see cref = "PostmarkResponse" /> is received from the API</param>
        public void SendMessage(string from, string to, string subject, string body, Action<PostmarkResponse> callback)
        {
            SendMessage(from, to, subject, body, null, callback);
        }

        /// <summary>
        ///   Sends a message through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name = "from">An email address for a sender</param>
        /// <param name = "to">An email address for a recipient</param>
        /// <param name = "subject">The message subject line</param>
        /// <param name = "body">The message body</param>
        /// <param name = "headers">A collection of additional mail headers to send with the message</param>
        /// <param name="callback">The callback invoked when a response is received from the API</param>
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction</returns>
        public void SendMessage(string from, string to, string subject, string body, NameValueCollection headers, Action<PostmarkResponse> callback)
        {
            var message = new PostmarkMessage(from, to, subject, body, headers);

            SendMessage(message, callback);
        }

        /// <summary>
        ///   Sends a message through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name = "message">A prepared message instance</param>
        /// <param name="callback">The callback invoked when a response is received from the API</param>
        public void SendMessage(PostmarkMessage message, Action<PostmarkResponse> callback)
        {
            var request = NewEmailRequest();

            CleanPostmarkMessage(message);

            request.Entity = message;

            BeginGetPostmarkResponse(request, callback);
        }

        /// <summary>
        ///   Sends a batch of up to messages through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <param name="callback">The callback invoked when a response is received from the API</param>
        public void SendMessages(IEnumerable<PostmarkMessage> messages, Action<IEnumerable<PostmarkResponse>> callback)
        {
            var request = NewBatchedEmailRequest();
            var batch = new List<PostmarkMessage>(messages.Count());

            foreach (var message in messages)
            {
                CleanPostmarkMessage(message);
                batch.Add(message);
            }

            request.Entity = messages;

            BeginGetPostmarkResponses(request, callback);
        }

        private void BeginGetPostmarkResponse(RestRequest request, Action<PostmarkResponse> callback)
        {
            _client.BeginRequest(request, new RestCallback((req, resp, state) =>
            {
                var postmark = GetPostmarkResponseImpl(resp);

                callback(postmark);
            }));
        }

        private void BeginGetPostmarkResponses(RestRequest request, Action<IEnumerable<PostmarkResponse>> callback)
        {
            _client.BeginRequest(request, new RestCallback((req, resp, state) =>
            {
                var postmark = GetPostmarkResponsesImpl(resp);

                callback(postmark);
            }));
        }
    }
}
