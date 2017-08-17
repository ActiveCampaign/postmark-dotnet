using System;
using System.Collections.Generic;
using PostmarkDotNet.Model;

namespace PostmarkDotNet
{
    public partial interface IPostmarkClient
    {
#if NET35
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
        IAsyncResult BeginSendMessage(string from, string to, string subject, string body);

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
        IAsyncResult BeginSendMessage(string from, string to, string subject, string body, NameValueCollection headers);

        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="message">A prepared message instance</param>
        /// <returns></returns>
        IAsyncResult BeginSendMessage(PostmarkMessage message);

        /// <summary>
        /// Send a templated message through the Postmark API.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        IAsyncResult BeginSendMessage(TemplatedPostmarkMessage message);
        
        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        IAsyncResult BeginSendMessages(IEnumerable<PostmarkMessage> messages);

        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        IAsyncResult BeginSendMessages(params PostmarkMessage[] messages);

        ///<summary>
        /// Completes an asynchronous request to send a message batch.
        ///</summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        ///<returns></returns>
        IEnumerable<PostmarkResponse> EndSendMessages(IAsyncResult asyncResult);

        ///<summary>
        /// Completes an asynchronous request to send a message.
        ///</summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        ///<returns></returns>
        PostmarkResponse EndSendMessage(IAsyncResult asyncResult);

        /// <summary>
        /// Retrieves the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <returns></returns>
        IAsyncResult BeginGetDeliveryStats();

        /// <summary>
        /// Completes a request for the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        PostmarkDeliveryStats EndGetDeliveryStats(IAsyncResult asyncResult);

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
        IAsyncResult BeginGetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count);

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
        IAsyncResult BeginGetBounces(bool? inactive, string emailFilter, string tag, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginGetBounces(PostmarkBounceType type, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginGetBounces(int offset, int count);

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
        IAsyncResult BeginGetBounces(PostmarkBounceType type, bool? inactive, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginGetBounces(bool? inactive, int offset, int count);

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
        IAsyncResult BeginGetBounces(PostmarkBounceType type, string emailFilter, int offset, int count);

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginGetBounces(string emailFilter, int offset, int count);

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
        IAsyncResult BeginGetBounces(PostmarkBounceType type, string emailFilter, string tag, int offset, int count);

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
        IAsyncResult BeginGetBounces(string emailFilter, string tag, int offset, int count);

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
        IAsyncResult BeginGetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count);

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
        IAsyncResult BeginGetBounces(bool? inactive, string emailFilter, int offset, int count);

        /// <summary>
        /// Completes an asynchronous request for a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        PostmarkBounces EndGetBounces(IAsyncResult asyncResult);

        /// <summary>
        /// Retrieves a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginGetBounce(string bounceId);

        /// <summary>
        /// Completes an asynchronous request for a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        PostmarkBounce EndGetBounce(IAsyncResult asyncResult);

        /// <summary>
        /// Returns a list of tags used for the current server.
        /// </summary>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginGetBounceTags();

        /// <summary>
        /// Completes an asynchronous request for a list of tags used for the current server.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        IEnumerable<string> EndGetBounceTags(IAsyncResult asyncResult);

        /// <summary>
        /// Returns the raw source of the bounce we accepted. 
        /// If Postmark does not have a dump for that bounce, it will return an empty string.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginGetBounceDump(string bounceId);

        /// <summary>
        /// Completes an asynchronous request for the raw source of the bounce we accepted.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        PostmarkBounceDump EndGetBounceDump(IAsyncResult asyncResult);

        /// <summary>
        /// Activates a deactivated bounce.
        /// </summary>
        /// <param name="bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        IAsyncResult BeginActivateBounce(string bounceId);

        ///<summary>
        ///  Completes an asynchronous request for a deactivated bounce.
        ///</summary>
        ///<param name="asyncResult"></param>
        ///<returns></returns>
        PostmarkBounceActivation EndActivateBounce(IAsyncResult asyncResult);



        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessages(int count, string subject, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessages(int count, int offset, string recipient);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessages(int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessages(string recipient, string fromemail,
            int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessages(string subject, int count, int offset);

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="tag">Filter by a tag used for the message (messages sent directly through the API only)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessages(string fromemail, string tag,
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
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessages(string recipient, string fromemail, string tag,
            string subject, int count, int offset);

        /// <summary>
        /// Completes an asynchronous request for a <see cref = "PostmarkOutboundMessageList" /> instances along
        /// with a sum total of messages recorded by the server, based on filter parameters.
        /// </summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        PostmarkOutboundMessageList EndGetOutboundMessages(IAsyncResult asyncResult);


        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessageDetail(string messageID);

        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>OutboundMessageDetail</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        OutboundMessageDetail EndGetOutboundMessageDetail(IAsyncResult asyncResult);


        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>MessageDump</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        IAsyncResult BeginGetOutboundMessageDump(string messageID);

        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>MessageDump</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        MessageDump EndGetOutboundMessageDump(IAsyncResult asyncResult);



        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        IAsyncResult BeginGetInboundMessages(int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        IAsyncResult BeginGetInboundMessages(string fromemail, int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        IAsyncResult BeginGetInboundMessages(string fromemail, string subject, int count, int offset);

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        IAsyncResult BeginGetInboundMessages(string recipient, string fromemail, string subject,
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
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        IAsyncResult BeginGetInboundMessages(string recipient, string fromemail, string subject,
            string mailboxhash, int count, int offset);

        /// <summary>
        /// Completes an asynchronous request for a <see cref = "PostmarkInboundMessageList" /> instances along
        /// with a sum total of messages recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>PostmarkInboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        PostmarkInboundMessageList EndGetInboundMessages(IAsyncResult asyncResult);

        /// <summary>
        /// Get the full details of a processed inbound message including all fields, attachment names, etc.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetInboundMessages call.</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        IAsyncResult BeginGetInboundMessageDetail(string messageID);

        /// <summary>
        /// Completes an asynchronous request for a <see cref = "InboundMessageDetail" /> instance
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>InboundMessageDetail</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        InboundMessageDetail EndGetInboundMessageDetail(IAsyncResult asyncResult);
#else
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
    /// <param name="callback">The callback invoked when a <see cref = "PostmarkResponse" /> is received from the API</param>
        void SendMessage(string from, string to, string subject, string body, Action<PostmarkResponse> callback);

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
        /// <param name="callback">The callback invoked when a response is received from the API</param>
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction</returns>
        void SendMessage(string from, string to, string subject, string body, NameValueCollection headers, Action<PostmarkResponse> callback);

        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="message">A prepared message instance</param>
        /// <param name="callback">The callback invoked when a response is received from the API</param>
        void SendMessage(PostmarkMessage message, Action<PostmarkResponse> callback);

        /// <summary>
        /// Sends a batch of up to messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <param name="callback">The callback invoked when a response is received from the API</param>
        void SendMessages(IEnumerable<PostmarkMessage> messages, Action<IEnumerable<PostmarkResponse>> callback);
#endif
    }
}