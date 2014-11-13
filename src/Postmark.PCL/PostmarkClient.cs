using Newtonsoft.Json;
using PostmarkDotNet.Model;
using PostmarkDotNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    /// <summary>
    /// The main entry point to Postmark.
    /// </summary>
    public class PostmarkClient
    {
        private static readonly string _agent = "Postmark.NET 2.x (" +
            typeof(PostmarkClient).AssemblyQualifiedName + ")";

        private static Uri API_BASE = new Uri("https://api.postmarkapp.com");

        private string _serverToken;

        /// <summary>
        /// Instantiate the client.
        /// </summary>
        /// <param name="serverToken"></param>
        public PostmarkClient(string serverToken)
        {
            _serverToken = serverToken;
        }

        private void SetPostmarkMeta(HttpRequestMessage message)
        {
            message.Headers.Add("Accept", "application/json");
            //message.Headers.Add("Content-Type", "application/json; charset=utf-8");
            message.Headers.Add("X-Postmark-Server-Token", _serverToken);
            message.Headers.Add("User-Agent", _agent);
        }

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
        public async Task<PostmarkResponse> SendMessageAsync(string from, string to,
            string subject, string body, IDictionary<string, string> headers = null)
        {
            var message = new PostmarkMessage(from, to, subject, body, headers);
            return await SendMessageAsync(message);
        }

        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="message">A prepared message instance.</param>
        /// <returns></returns>
        public async Task<PostmarkResponse> SendMessageAsync(PostmarkMessage message)
        {
            PostmarkResponse retval = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = API_BASE;
                var content = new JsonContent<PostmarkMessage>(message);
                var payload = new HttpRequestMessage(HttpMethod.Post, "/email")
                {
                    Content = content
                };

                SetPostmarkMeta(payload);

                var result = await client.SendAsync(payload);

                retval = await JsonConvert
                    .DeserializeObjectAsync<PostmarkResponse>(await result.Content.ReadAsStringAsync());
            }
            return retval;
        }

        /// <summary>
        /// Sends a batch of up to 500 messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        public async Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(params PostmarkMessage[] messages)
        {
            throw new NotImplementedException();
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
        public async Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(IEnumerable<PostmarkMessage> messages)
        {
            return await SendMessagesAsync(messages.ToArray());
        }

        /// <summary>
        /// Retrieves the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <returns></returns>
        public async Task<PostmarkDeliveryStats> GetDeliveryStatsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="tag">Filters on the bounce tag</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="tag">Filters on the bounce tag</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive,
            string emailFilter, string tag, int offset, int count) { throw new NotImplementedException(); }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, int offset, int count) { throw new NotImplementedException(); }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(int offset, int count) { throw new NotImplementedException(); }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, int offset, int count) { throw new NotImplementedException(); }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(bool? inactive, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, string emailFilter, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(string emailFilter, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="tag">Filters on the bounce tag</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, string emailFilter, string tag, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="tag">Filters on the bounce tag</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(string emailFilter, string tag, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(bool? inactive, string emailFilter, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounce> GetBounceAsync(string bounceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of tags used for the current server.
        /// </summary>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<IEnumerable<string>> GetBounceTagsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the raw source of the bounce we accepted. 
        /// If Postmark does not have a dump for that bounce, it will return an empty string.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounceDump> GetBounceDumpAsync(string bounceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Activates a deactivated bounce.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounceActivation> ActivateBounceAsync(string bounceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, string subject, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, int offset, string recipient)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string recipient, string fromemail, int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string subject, int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="tag">Filter by a tag used for the message (messages sent directly through the API only)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string fromemail, string tag,
            string subject, int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="tag">Filter by a tag used for the message (messages sent directly through the API only)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string recipient, string fromemail, string tag,
            string subject, int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>OutboundMessageDetail</returns>
        public async Task<OutboundMessageDetail> GetOutboundMessageDetailAsync(string messageID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>MessageDump</returns>
        public async Task<MessageDump> GetOutboundMessageDumpAsync(string messageID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        public async Task<PostmarkInboundMessageList> GetInboundMessagesAsync(int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        public async Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string fromemail, int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        public async Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string fromemail, string subject, int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        public async Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string recipient, string fromemail, string subject,
            int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="mailboxhash">Filter by mailbox hash that was parsed from the inbound message.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        public async Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string recipient, string fromemail, string subject,
            string mailboxhash, int count, int offset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the full details of a processed inbound message including all fields, attachment names, etc.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetInboundMessages call.</param>
        /// <returns>InboundMessageDetail</returns>
        public async Task<InboundMessageDetail> GetInboundMessageDetailAsync(string messageID)
        {
            throw new NotImplementedException();
        }

    }
}
