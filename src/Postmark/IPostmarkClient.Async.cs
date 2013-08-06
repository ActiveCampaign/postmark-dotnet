using System;
using System.Collections.Generic;
#if !WINDOWS_PHONE
using System.Collections.Specialized;
#else
using Hammock.Silverlight.Compat;
#endif

namespace PostmarkDotNet
{
    public partial interface IPostmarkClient
    {
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