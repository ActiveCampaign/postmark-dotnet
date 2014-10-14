using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostmarkDotNet.Model;

namespace PostmarkDotNet
{
    public partial interface IPostmarkClient
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
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction</returns>
        Task<PostmarkResponse> SendMessageAsync(string from, string to, string subject, string body);

        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="from">An email address for a sender</param>
        /// <param name="to">An email address for a recipient</param>
        /// <param name="subject">The message subject line</param>
        /// <param name="body">The message body</param>
        /// <param name="headers">A collection of additional mail headers to send with the message</param>
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction</returns>
        Task<PostmarkResponse> SendMessageAsync(string from, string to, string subject, string body, NameValueCollection headers);

        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="message">A prepared message instance</param>
        /// <returns>PostmarkResponse</returns>
        Task<PostmarkResponse> SendMessageAsync(PostmarkMessage message);

        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns>IEnumerable of PostmarkResponse</returns>
        Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(IEnumerable<PostmarkMessage> messages);

        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns>IEnumerable of PostmarkMessage</returns>
        Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(params PostmarkMessage[] messages);        

        /// <summary>
        /// Retrieves the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <returns>PostmarkDeliveryStats</returns>
        Task<PostmarkDeliveryStats> GetDeliveryStatsAsync();       

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
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count);

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
        Task<PostmarkBounces> GetBouncesAsync(bool? inactive, string emailFilter, string tag, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(bool? inactive, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, string emailFilter, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(string emailFilter, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="tag">Filters on the bounce tag</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, string emailFilter, string tag, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="tag">Filters on the bounce tag</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(string emailFilter, string tag, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns>PostmarkBounces</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounces> GetBouncesAsync(bool? inactive, string emailFilter, int offset, int count);


        /// <summary>
        /// Retrieves a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns>PostmarkBounce</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounce> GetBounceAsync(string bounceId);

        /// <summary>
        /// Returns a list of tags used for the current server.
        /// </summary>
        /// <returns>list of tags</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<IEnumerable<string>> GetBounceTagsAsync();       

        /// <summary>
        /// Returns the raw source of the bounce we accepted. 
        /// If Postmark does not have a dump for that bounce, it will return an empty string.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns>PostmarkBounceDump</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounceDump> GetBounceDumpAsync(string bounceId);        

        /// <summary>
        /// Activates a deactivated bounce.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns>PostmarkBounceActivation</returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        Task<PostmarkBounceActivation> ActivateBounceAsync(string bounceId);       

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, string subject, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, int offset, string recipient);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string recipient, string fromemail,
            int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string subject, int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="tag">Filter by a tag used for the message (messages sent directly through the API only)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string fromemail, string tag,
            string subject, int count, int offset);

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
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string recipient, string fromemail, string tag,
            string subject, int count, int offset);

        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>OutboundMessageDetail</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<OutboundMessageDetail> GetOutboundMessageDetailAsync(string messageID);

        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>MessageDump</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        Task<MessageDump> GetOutboundMessageDumpAsync(string messageID);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        Task<PostmarkInboundMessageList> GetInboundMessagesAsync(int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string fromemail, int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string fromemail, string subject, int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string recipient, string fromemail, string subject,
            int count, int offset);

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
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string recipient, string fromemail, string subject,
            string mailboxhash, int count, int offset);

        /// <summary>
        /// Get the full details of a processed inbound message including all fields, attachment names, etc.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetInboundMessages call.</param>
        /// <returns>InboundMessageDetail</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        Task<InboundMessageDetail> GetInboundMessageDetailAsync(string messageID);
    }
}
