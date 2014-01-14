﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Hammock;
using Hammock.Web;
using Newtonsoft.Json;
using PostmarkDotNet.Model;

namespace PostmarkDotNet
{
    partial class PostmarkClient
    {
        #region Mail API

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
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction.</returns>
        public IAsyncResult BeginSendMessage(string from, string to, string subject, string body)
        {
            return BeginSendMessage(from, to, subject, body, null);
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
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction</returns>
        public IAsyncResult BeginSendMessage(string from, string to, string subject, string body, NameValueCollection headers)
        {
            var message = new PostmarkMessage(from, to, subject, body, headers);

            return BeginSendMessage(message);
        }

        /// <summary>
        ///   Sends a message through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name = "message">A prepared message instance</param>
        /// <returns></returns>
        public IAsyncResult BeginSendMessage(PostmarkMessage message)
        {
            var request = NewEmailRequest();

            CleanPostmarkMessage(message);

            request.Entity = message;

            return BeginGetPostmarkResponse(request);
        }

        ///<summary>
        /// Completes an asynchronous request to send a message.
        ///</summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        ///<returns></returns>
        public PostmarkResponse EndSendMessage(IAsyncResult asyncResult)
        {
            return EndGetPostmarkResponse(asyncResult);
        }

        /// <summary>
        ///   Sends a batch of up to messages through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        public IAsyncResult BeginSendMessages(IEnumerable<PostmarkMessage> messages)
        {
            var request = NewBatchedEmailRequest();
            var batch = new List<PostmarkMessage>(messages.Count());

            foreach (var message in messages)
            {
                CleanPostmarkMessage(message);
                batch.Add(message);
            }

            return BeginGetPostmarkResponses(request);
        }

        /// <summary>
        ///   Sends a batch of up to messages through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        public IAsyncResult BeginSendMessages(params PostmarkMessage[] messages)
        {
            return BeginSendMessages(messages.ToList());
        }

        ///<summary>
        /// Completes an asynchronous request to send a message batch.
        ///</summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        ///<returns></returns>
        public IEnumerable<PostmarkResponse> EndSendMessages(IAsyncResult asyncResult)
        {
            return EndGetPostmarkResponses(asyncResult);
        }

        private IAsyncResult BeginGetPostmarkResponse(RestRequest request)
        {
            var response = _client.BeginRequest(request);
            return response;
        }

        private PostmarkResponse EndGetPostmarkResponse(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);

            return GetPostmarkResponseImpl(response);
        }

        private IAsyncResult BeginGetPostmarkResponses(RestRequest request)
        {
            var response = _client.BeginRequest(request);
            return response;
        }

        private IEnumerable<PostmarkResponse> EndGetPostmarkResponses(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);

            return GetPostmarkResponsesImpl(response);
        }

        #endregion

        #region Bounce API

        /// <summary>
        /// Retrieves the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <returns></returns>
        public IAsyncResult BeginGetDeliveryStats()
        {
            var request = NewBouncesRequest();
            request.Path = "deliverystats";

            return _client.BeginRequest(request);
        }

        /// <summary>
        /// Completes a request for the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public PostmarkDeliveryStats EndGetDeliveryStats(IAsyncResult asyncResult)
        {
            return EndBounceRequest<PostmarkDeliveryStats>(asyncResult);
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name = "type">The type of bounces to filter on</param>
        /// <param name = "inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name = "emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name = "tag">Filters on the bounce tag</param>
        /// <param name = "offset">The page offset for the returned results; mandatory</param>
        /// <param name = "count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            return BeginGetBouncesImpl(type, inactive, emailFilter, tag, offset, count);
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
        public IAsyncResult BeginGetBounces(bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            return BeginGetBouncesImpl(null, inactive, emailFilter, tag, offset, count);
        }

        /// <summary>
        ///   Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name = "type">The type of bounces to filter on</param>
        /// <param name = "offset">The page offset for the returned results; mandatory</param>
        /// <param name = "count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounces(PostmarkBounceType type, int offset, int count)
        {
            return BeginGetBouncesImpl(type, null, null, null, offset, count);
        }

        public IAsyncResult BeginGetBounces(int offset, int count)
        {
            return BeginGetBouncesImpl(null, null, null, null, offset, count);
        }

        /// <summary>
        ///   Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name = "type">The type of bounces to filter on</param>
        /// <param name = "inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name = "offset">The page offset for the returned results; mandatory</param>
        /// <param name = "count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounces(PostmarkBounceType type, bool? inactive, int offset, int count)
        {
            return BeginGetBouncesImpl(type, inactive, null, null, offset, count);
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounces(bool? inactive, int offset, int count)
        {
            return BeginGetBouncesImpl(null, inactive, null, null, offset, count);
        }

        /// <summary>
        ///   Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name = "type">The type of bounces to filter on</param>
        /// <param name = "emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name = "offset">The page offset for the returned results; mandatory</param>
        /// <param name = "count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounces(PostmarkBounceType type, string emailFilter, int offset, int count)
        {
            return BeginGetBouncesImpl(type, null, emailFilter, null, offset, count);
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
        public IAsyncResult BeginGetBounces(string emailFilter, int offset, int count)
        {
            return BeginGetBouncesImpl(null, null, emailFilter, null, offset, count);
        }

        /// <summary>
        ///   Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name = "type">The type of bounces to filter on</param>
        /// <param name = "emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name = "tag">Filters on the bounce tag</param>
        /// <param name = "offset">The page offset for the returned results; mandatory</param>
        /// <param name = "count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounces(PostmarkBounceType type, string emailFilter, string tag, int offset, int count)
        {
            return BeginGetBouncesImpl(type, null, emailFilter, tag, offset, count);
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
        public IAsyncResult BeginGetBounces(string emailFilter, string tag, int offset, int count)
        {
            return BeginGetBouncesImpl(null, null, emailFilter, tag, offset, count);
        }

        /// <summary>
        ///   Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name = "type">The type of bounces to filter on</param>
        /// <param name = "inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name = "emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name = "offset">The page offset for the returned results; mandatory</param>
        /// <param name = "count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count)
        {
            return BeginGetBouncesImpl(type, inactive, emailFilter, null, offset, count);
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
        public IAsyncResult BeginGetBounces(bool? inactive, string emailFilter, int offset, int count)
        {
            return BeginGetBouncesImpl(null, inactive, emailFilter, null, offset, count);
        }

        private IAsyncResult BeginGetBouncesImpl(PostmarkBounceType? type, bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            var request = NewBouncesRequest();
            request.Path = "bounces";
            if (inactive.HasValue) request.AddParameter("inactive", inactive.Value.ToString().ToLowerInvariant());
            if (!string.IsNullOrEmpty(emailFilter)) request.AddParameter("emailFilter", emailFilter);
            if (!string.IsNullOrEmpty(tag)) request.AddParameter("tag", tag);
            if(type.HasValue)
            {request.AddParameter("type", type.ToString());}
            request.AddParameter("offset", offset.ToString());
            request.AddParameter("count", count.ToString());

            return _client.BeginRequest(request);
        }

        /// <summary>
        ///   Completes an asynchronous request for a collection of <see cref = "PostmarkBounce" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        ///<param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        public PostmarkBounces EndGetBounces(IAsyncResult asyncResult)
        {
            return EndBounceRequest<PostmarkBounces>(asyncResult);
        }

        /// <summary>
        ///   Retrieves a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name = "bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounce(string bounceId)
        {
            var request = NewBouncesRequest();
            request.Path = string.Format("bounces/{0}", bounceId);

            return _client.BeginRequest(request);
        }

        /// <summary>
        /// Completes an asynchronous request for a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        public PostmarkBounce EndGetBounce(IAsyncResult asyncResult)
        {
            return EndBounceRequest<PostmarkBounce>(asyncResult);
        }

        /// <summary>
        ///   Returns a list of tags used for the current server.
        /// </summary>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounceTags()
        {
            var request = NewBouncesRequest();
            request.Path = "bounces/tags";

            return _client.BeginRequest(request);
        }

        /// <summary>
        /// Completes an asynchronous request for a list of tags used for the current server.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        public IEnumerable<string> EndGetBounceTags(IAsyncResult asyncResult)
        {
            return EndBounceRequest<IEnumerable<string>>(asyncResult);
        }

        /// <summary>
        ///   Returns the raw source of the bounce we accepted. 
        ///   If Postmark does not have a dump for that bounce, it will return an empty string.
        /// </summary>
        /// <param name = "bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginGetBounceDump(string bounceId)
        {
            var request = NewBouncesRequest();
            request.Path = string.Format("bounces/{0}/dump", bounceId.Trim());

            return _client.BeginRequest(request);
        }

        /// <summary>
        /// Completes an asynchronous request for the raw source of the bounce we accepted.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns></returns>
        public PostmarkBounceDump EndGetBounceDump(IAsyncResult asyncResult)
        {
            return EndBounceRequest<PostmarkBounceDump>(asyncResult);
        }

        /// <summary>
        ///   Activates a deactivated bounce.
        /// </summary>
        /// <param name = "bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IAsyncResult BeginActivateBounce(string bounceId)
        {
            var request = NewBouncesRequest();
            request.Method = WebMethod.Put;
            request.Path = string.Format("bounces/{0}/activate", bounceId.Trim());

            return _client.BeginRequest(request);
        }

        ///<summary>
        ///  Completes an asynchronous request for a deactivated bounce.
        ///</summary>
        ///<param name="asyncResult"></param>
        ///<returns></returns>
        public PostmarkBounceActivation EndActivateBounce(IAsyncResult asyncResult)
        {
            return EndBounceRequest<PostmarkBounceActivation>(asyncResult);
        }

        private T EndBounceRequest<T>(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);

            return JsonConvert.DeserializeObject<T>(response.Content, _settings);
        }

        #endregion

        #region Messages API

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessages(int count, string subject, int offset)
        {
            return BeginGetOutboundMessagesImpl(null, null, null, subject, count, offset);
        }


        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessages(int count, int offset, string recipient)
        {
            return BeginGetOutboundMessagesImpl(recipient, null, null, null, count, offset);
        }


        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessages(int count, int offset)
        {
            return BeginGetOutboundMessagesImpl(null, null, null, null, count, offset);
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessages(string recipient, string fromemail,
            int count, int offset)
        {
            return BeginGetOutboundMessagesImpl(recipient, fromemail, null, null, count, offset);
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessages(string subject, int count, int offset)
        {
            return BeginGetOutboundMessagesImpl(null, null, null, subject, count, offset);
        }

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
        public IAsyncResult BeginGetOutboundMessages(string fromemail, string tag,
            string subject, int count, int offset)
        {
            return BeginGetOutboundMessagesImpl(null, fromemail, tag, subject, count, offset);
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
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessages(string recipient, string fromemail, string tag,
            string subject, int count, int offset)
        {
            return BeginGetOutboundMessagesImpl(recipient, fromemail, tag, subject, count, offset);
        }

        /// <summary>
        /// Implementation called to do the actual messages call and return a <see cref="PostmarkOutboundMessageList"/>
        /// </summary>s
        private IAsyncResult BeginGetOutboundMessagesImpl(string recipient, string fromemail, string tag, string subject, int count, int offset)
        {
            var request = NewMessagesRequest();
            request.Path = "messages/outbound";

            if (!string.IsNullOrEmpty(recipient)) request.AddParameter("recipient", recipient);
            if (!string.IsNullOrEmpty(fromemail)) request.AddParameter("fromemail", fromemail);
            if (!string.IsNullOrEmpty(tag)) request.AddParameter("tag", tag);
            if (!string.IsNullOrEmpty(subject)) request.AddParameter("subject", subject);

            request.AddParameter("count", count.ToString());
            request.AddParameter("offset", offset.ToString());

            return _client.BeginRequest(request);
        }


        /// <summary>
        ///   Completes an asynchronous request for a <see cref = "PostmarkOutboundMessageList" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public PostmarkOutboundMessageList EndGetOutboundMessages(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);

            return JsonConvert.DeserializeObject<PostmarkOutboundMessageList>(response.Content, _settings);
        }


        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessageDetail(string messageID)
        {
            var request = NewMessagesRequest();
            request.Path = string.Format("messages/outbound/{0}/details", messageID.Trim());

            return _client.BeginRequest(request);
        }

        /// <summary>
        ///   Completes an asynchronous request for a <see cref = "OutboundMessageDetail" /> instance.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>OutboundMessageDetail</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public OutboundMessageDetail EndGetOutboundMessageDetail(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);
            return JsonConvert.DeserializeObject<OutboundMessageDetail>(response.Content, _settings);
        }


        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public IAsyncResult BeginGetOutboundMessageDump(string messageID)
        {
            var request = NewMessagesRequest();
            request.Path = string.Format("messages/outbound/{0}/dump", messageID.Trim());

            return _client.BeginRequest(request);
        }

        /// <summary>
        ///   Completes an asynchronous request for a <see cref = "MessageDump" /> instance.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>MessageDump</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-messages.html" />
        public MessageDump EndGetOutboundMessageDump(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);
            return JsonConvert.DeserializeObject<MessageDump>(response.Content, _settings);
        }



        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        public IAsyncResult BeginGetInboundMessages(int count, int offset)
        {
            return BeginGetInboundMessagesImpl(null, null, null, null, count, offset);
        }

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        public IAsyncResult BeginGetInboundMessages(string fromemail, int count, int offset)
        {
            return BeginGetInboundMessagesImpl(null, fromemail, null, null, count, offset);
        }

        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        public IAsyncResult BeginGetInboundMessages(string fromemail, string subject, int count, int offset)
        {
            return BeginGetInboundMessagesImpl(null, fromemail, subject, null, count, offset);
        }

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
        public IAsyncResult BeginGetInboundMessages(string recipient, string fromemail, string subject, int count, int offset)
        {
            return BeginGetInboundMessagesImpl(recipient, fromemail, subject, null, count, offset);
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
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        public IAsyncResult BeginGetInboundMessages(string recipient, string fromemail, string subject, string mailboxhash, int count, int offset)
        {
            return BeginGetInboundMessagesImpl(recipient, fromemail, subject, mailboxhash, count, offset);
        }


        /// <summary>
        /// Implementation to get Inbound messages for the public search APIs
        /// </summary>
        private IAsyncResult BeginGetInboundMessagesImpl(string recipient, string fromemail, string subject, string mailboxhash, int count, int offset)
        {
            var request = NewMessagesRequest();
            request.Path = "messages/inbound";

            if (!string.IsNullOrEmpty(recipient)) request.AddParameter("recipient", recipient);
            if (!string.IsNullOrEmpty(fromemail)) request.AddParameter("fromemail", fromemail);
            if (!string.IsNullOrEmpty(subject)) request.AddParameter("subject", subject);
            if (!string.IsNullOrEmpty(subject)) request.AddParameter("mailboxhash", mailboxhash);

            request.AddParameter("count", count.ToString());
            request.AddParameter("offset", offset.ToString());

            return _client.BeginRequest(request);
        }

        /// <summary>
        ///   Completes an asynchronous request for a <see cref = "PostmarkOutboundMessageList" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>PostmarkInboundMessageList</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        public PostmarkInboundMessageList EndGetInboundMessages(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);
            return JsonConvert.DeserializeObject<PostmarkInboundMessageList>(response.Content, _settings);
        }

        /// <summary>
        /// Get the full details of a processed inbound message including all fields, attachment names, etc.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetInboundMessages call.</param>
        /// <returns>IAsyncResult</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        public IAsyncResult BeginGetInboundMessageDetail(string messageID)
        {
            var request = NewMessagesRequest();
            request.Path = string.Format("messages/inbound/{0}/details", messageID.Trim());

            return _client.BeginRequest(request);
        }

        /// <summary>
        ///   Completes an asynchronous request for a <see cref = "InboundMessageDetail" /> instance.
        /// </summary>
        /// <param name="asyncResult">An <see cref="IAsyncResult" /> for the desired response</param>
        /// <returns>InboundMessageDetail</returns>
        /// <seealso href = "http://developer.postmarkapp.com/developer-inbound-messages.html" />
        public InboundMessageDetail EndGetInboundMessageDetail(IAsyncResult asyncResult)
        {
            var response = _client.EndRequest(asyncResult);
            return JsonConvert.DeserializeObject<InboundMessageDetail>(response.Content, _settings);
        }

        #endregion

    }
}
