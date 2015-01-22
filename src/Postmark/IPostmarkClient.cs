using System.Collections.Generic;
using PostmarkDotNet.Model;
#if !SILVERLIGHT
using System.Collections.Specialized;
#else
using Hammock.Silverlight.Compat;
#endif

namespace PostmarkDotNet
{
    ///<summary>
    /// Defines the contract for the Postmark API.
    ///</summary>
    public partial interface IPostmarkClient
    {
        ///<summary>
        ///  Override the REST API endpoint by specifying your own address, if necessary.
        ///</summary>
        string Authority { get; set; }

        /// <summary>
        /// Gets the server token issued with your Postmark email server configuration.
        /// </summary>
        /// <value>The server token.</value>
        string ServerToken { get; }

#if !WINDOWS_PHONE
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
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction.</returns>
        PostmarkResponse SendMessage(string from, string to, string subject, string body);

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
        PostmarkResponse SendMessage(string from, string to, string subject, string body, NameValueCollection headers);

        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="message">A prepared message instance.</param>
        /// <returns></returns>
        PostmarkResponse SendMessage(PostmarkMessage message);

        /// <summary>
        /// Sends a batch of up to 500 messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        IEnumerable<PostmarkResponse> SendMessages(params PostmarkMessage[] messages);

        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        IEnumerable<PostmarkResponse> SendMessages(IEnumerable<PostmarkMessage> messages);

        /// <summary>
        /// Retrieves the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <returns></returns>
        PostmarkDeliveryStats GetDeliveryStats();

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
        PostmarkBounces GetBounces(bool? inactive, string emailFilter, string tag, int offset, int count);

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
        PostmarkBounces GetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        PostmarkBounces GetBounces(PostmarkBounceType type, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        PostmarkBounces GetBounces(int offset, int count);
        
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
        PostmarkBounces GetBounces(PostmarkBounceType type, bool? inactive, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        PostmarkBounces GetBounces(bool? inactive, int offset, int count);

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
        PostmarkBounces GetBounces(PostmarkBounceType type, string emailFilter, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        PostmarkBounces GetBounces(string emailFilter, int offset, int count);

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
        PostmarkBounces GetBounces(PostmarkBounceType type, string emailFilter, string tag, int offset, int count);

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
        PostmarkBounces GetBounces(string emailFilter, string tag, int offset, int count);

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
        PostmarkBounces GetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count);

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
        PostmarkBounces GetBounces(bool? inactive, string emailFilter, int offset, int count);

        /// <summary>
        /// Retrieves a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        PostmarkBounce GetBounce(string bounceId);

        /// <summary>
        /// Returns a list of tags used for the current server.
        /// </summary>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IEnumerable<string> GetBounceTags();

        /// <summary>
        /// Returns the raw source of the bounce we accepted. 
        /// If Postmark does not have a dump for that bounce, it will return an empty string.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        PostmarkBounceDump GetBounceDump(string bounceId);

        /// <summary>
        /// Activates a deactivated bounce.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        PostmarkBounceActivation ActivateBounce(string bounceId);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        PostmarkOutboundMessageList GetOutboundMessages(int count, string subject, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        PostmarkOutboundMessageList GetOutboundMessages(int count, int offset, string recipient);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        PostmarkOutboundMessageList GetOutboundMessages(int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        PostmarkOutboundMessageList GetOutboundMessages(string recipient, string fromemail, int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        PostmarkOutboundMessageList GetOutboundMessages(string subject, int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="tag">Filter by a tag used for the message (messages sent directly through the API only)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        PostmarkOutboundMessageList GetOutboundMessages(string fromemail, string tag,
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
        PostmarkOutboundMessageList GetOutboundMessages(string recipient, string fromemail, string tag,
            string subject, int count, int offset);

        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>OutboundMessageDetail</returns>
        OutboundMessageDetail GetOutboundMessageDetail(string messageID);

        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>MessageDump</returns>
        MessageDump GetOutboundMessageDump(string messageID);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        PostmarkInboundMessageList GetInboundMessages(int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        PostmarkInboundMessageList GetInboundMessages(string fromemail, int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        PostmarkInboundMessageList GetInboundMessages(string fromemail, string subject, int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>PostmarkInboundMessageList</returns>
        PostmarkInboundMessageList GetInboundMessages(string recipient, string fromemail, string subject,
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
        PostmarkInboundMessageList GetInboundMessages(string recipient, string fromemail, string subject,
            string mailboxhash, int count, int offset);

        /// <summary>
        /// Get the full details of a processed inbound message including all fields, attachment names, etc.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetInboundMessages call.</param>
        /// <returns>InboundMessageDetail</returns>
        InboundMessageDetail GetInboundMessageDetail(string messageID);
#endif
    }
}