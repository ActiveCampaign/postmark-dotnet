using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Hammock;
using Hammock.Web;
using Newtonsoft.Json;
using PostmarkDotNet.Converters;
using PostmarkDotNet.Model;
using PostmarkDotNet.Serializers;
using PostmarkDotNet.Validation;

#if !WINDOWS_PHONE
using System.Net.Mail;
using System.Collections.Specialized;
#else
using Hammock.Silverlight.Compat;
#endif

namespace PostmarkDotNet
{
    /// <summary>
    ///   A client for the Postmark application. 
    ///   Use this client in place of an <see cref = "SmtpClient" /> to send messages
    ///   through this service.
    /// </summary>
    public partial class PostmarkClient : IPostmarkClient
    {
        private static readonly JsonSerializerSettings _settings;
        private static readonly PostmarkSerializer _serializer;
        private readonly RestClient _client;

        static PostmarkClient()
        {
            _settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include
            };

            _settings.Converters.Add(new UnicodeJsonStringConverter());                                                                              
            _settings.Converters.Add(new NameValueCollectionConverter());
            _serializer = new PostmarkSerializer(_settings);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkClient" /> class.
        ///   If you do not have a server token you can request one by signing up to
        ///   use Postmark: http://postmarkapp.com.
        /// </summary>
        /// <param name = "serverToken">The server token.</param>
        public PostmarkClient(string serverToken)
        {
            ServerToken = serverToken;
            _client = new RestClient
            {
                Authority = "https://api.postmarkapp.com"
            };
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PostmarkClient" /> class.
        ///   If you do not have a server token you can request one by signing up to
        ///   use Postmark: http://postmarkapp.com.
        /// </summary>
        /// <param name = "serverToken">The server token.</param>
        /// <param name = "timeout">Time to wait for the API in seconds.</param>
        public PostmarkClient(string serverToken, int timeout)
        {
            ServerToken = serverToken;

            // Configure timespam from number of seconds the user wants to set the timeout for
            TimeSpan timeoutInSeconds = DateTime.Now.AddSeconds(timeout).Subtract(DateTime.Now);

            _client = new RestClient
            {
                Authority = "https://api.postmarkapp.com",
                Timeout = timeoutInSeconds
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "PostmarkClient" /> class.
        /// If you do not have a server token you can request one by signing up to
        /// use Postmark: http://postmarkapp.com.
        /// Extra signature to override https
        /// </summary>
        /// <param name="serverToken">You can get a server token by signing up at http://www.postmarkapp.com</param>
        /// <param name="noSSL">Skip https usage</param>
        public PostmarkClient(string serverToken, bool noSSL)
        {
            ServerToken = serverToken;
            _client = new RestClient();

            _client.Authority = noSSL ? "http://api.postmarkapp.com" : "https://api.postmarkapp.com";
        }

        ///<summary>
        ///  Override the REST API endpoint by specifying your own address, if necessary.
        ///</summary>
        public string Authority
        {
            get { return _client.Authority; }
            set { _client.Authority = value; }
        }

        /// <summary>
        ///   Gets the server token issued with your Postmark email server configuration.
        /// </summary>
        /// <value>The server token.</value>
        public string ServerToken { get; private set; }
        
#if !WINDOWS_PHONE

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
        public PostmarkResponse SendMessage(string from, string to, string subject, string body)
        {
            return SendMessage(from, to, subject, body, null);
        }

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
        /// <param name = "headers">A collection of additional mail headers to send with the message.</param>
        /// <returns>A <see cref = "PostmarkResponse" /> with details about the transaction.</returns>
        public PostmarkResponse SendMessage(string from, string to, string subject, string body, NameValueCollection headers)
        {
            var message = new PostmarkMessage(from, to, subject, body, headers);

            return SendMessage(message);
        }

        /// <summary>
        ///   Sends a message through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name = "message">A prepared message instance.</param>
        /// <returns></returns>
        public PostmarkResponse SendMessage(PostmarkMessage message)
        {
            var request = NewEmailRequest();

            CleanPostmarkMessage(message);

            request.Entity = message;

            return GetPostmarkResponse(request);
        }

        /// <summary>
        ///   Sends a batch of messages through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        public IEnumerable<PostmarkResponse> SendMessages(params PostmarkMessage[] messages)
        {
            if(messages.Count() > 500)
            {
                throw new ValidationException("You may only send up to 500 messages in a batched call");
            }
            return SendMessages(messages.ToList());
        }

        /// <summary>
        ///   Sends a batch of messages through the Postmark API.
        ///   All email addresses must be valid, and the sender must be
        ///   a valid sender signature according to Postmark. To obtain a valid
        ///   sender signature, log in to Postmark and navigate to:
        ///   http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared message batch.</param>
        /// <returns></returns>
        public IEnumerable<PostmarkResponse> SendMessages(IEnumerable<PostmarkMessage> messages)
        {
            var request = NewBatchedEmailRequest();
            var batch = new List<PostmarkMessage>(messages.Count());

            foreach (var message in messages)
            {
                CleanPostmarkMessage(message);
                batch.Add(message);
            }

            request.Entity = messages;
            return GetPostmarkResponses(request);
        }

        #endregion

        #region Bounce API

        /// <summary>
        ///   Retrieves the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        ///   associated mail server.
        /// </summary>
        /// <returns></returns>
        public PostmarkDeliveryStats GetDeliveryStats()
        {
            var request = NewBouncesRequest();
            request.Path = "deliverystats";

            var response = _client.Request(request);

            return JsonConvert.DeserializeObject<PostmarkDeliveryStats>(response.Content, _settings);
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
        public PostmarkBounces GetBounces(bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            return GetBouncesImpl(null, inactive, emailFilter, tag, offset, count);
        }

        /// <summary>
        ///   Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        ///   with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name = "type">The type of bounces to filter on</param>
        /// <param name = "inactive">Whether to return only inactive or active bounces; use null to return all bounces</param>
        /// <param name = "emailFilter">Filters based on whether the filter value is contained in the bounce source's email</param>
        /// <param name = "tag">Filters on the bounce tag</param>
        /// <param name = "offset">The page offset for the returned results; mandatory</param>
        /// <param name = "count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public PostmarkBounces GetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            return GetBouncesImpl(type, inactive, emailFilter, tag, offset, count);
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
        public PostmarkBounces GetBounces(PostmarkBounceType type, int offset, int count)
        {
            return GetBouncesImpl(type, null, null, null, offset, count);
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="offset">The page offset for the returned results; mandatory</param>
        /// <param name="count">The number of results to return by the page offset; mandatory.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public PostmarkBounces GetBounces(int offset, int count)
        {
            return GetBouncesImpl(null, null, null, null, offset, count);
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
        public PostmarkBounces GetBounces(PostmarkBounceType type, bool? inactive, int offset, int count)
        {
            return GetBouncesImpl(type, inactive, null, null, offset, count);
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
        public PostmarkBounces GetBounces(bool? inactive, int offset, int count)
        {
            return GetBouncesImpl(null, inactive, null, null, offset, count);
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
        public PostmarkBounces GetBounces(PostmarkBounceType type, string emailFilter, int offset, int count)
        {
            return GetBouncesImpl(type, null, emailFilter, null, offset, count);
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
        public PostmarkBounces GetBounces(string emailFilter, int offset, int count)
        {
            return GetBouncesImpl(null, null, emailFilter, null, offset, count);
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
        public PostmarkBounces GetBounces(PostmarkBounceType type, string emailFilter, string tag, int offset, int count)
        {
            return GetBouncesImpl(type, null, emailFilter, tag, offset, count);
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
        public PostmarkBounces GetBounces(string emailFilter, string tag, int offset, int count)
        {
            return GetBouncesImpl(null, null, emailFilter, tag, offset, count);
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
        public PostmarkBounces GetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count)
        {
            return GetBouncesImpl(type, inactive, emailFilter, null, offset, count);
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
        public PostmarkBounces GetBounces(bool? inactive, string emailFilter, int offset, int count)
        {
            return GetBouncesImpl(null, inactive, emailFilter, null, offset, count);
        }

        private PostmarkBounces GetBouncesImpl(PostmarkBounceType? type, bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            var request = NewBouncesRequest();
            request.Path = "bounces";
            if (inactive.HasValue) request.AddParameter("inactive", inactive.Value.ToString().ToLowerInvariant());
            if (!string.IsNullOrEmpty(emailFilter)) request.AddParameter("emailFilter", emailFilter);
            if (!string.IsNullOrEmpty(tag)) request.AddParameter("tag", tag);
            if (type.HasValue)
            {
                request.AddParameter("type", type.ToString());
            }
            request.AddParameter("offset", offset.ToString());
            request.AddParameter("count", count.ToString());

            var response = _client.Request(request);
            return JsonConvert.DeserializeObject<PostmarkBounces>(response.Content, _settings);
        }

        /// <summary>
        ///   Retrieves a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name = "bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public PostmarkBounce GetBounce(string bounceId)
        {
            var request = NewBouncesRequest();
            request.Path = string.Format("bounces/{0}", bounceId);

            var response = _client.Request(request);

            return JsonConvert.DeserializeObject<PostmarkBounce>(response.Content, _settings);
        }

        /// <summary>
        ///   Returns a list of tags used for the current server.
        /// </summary>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public IEnumerable<string> GetBounceTags()
        {
            var request = NewBouncesRequest();
            request.Path = "bounces/tags";

            var response = _client.Request(request);

            return JsonConvert.DeserializeObject<IEnumerable<string>>(response.Content, _settings);
        }

        /// <summary>
        ///   Returns the raw source of the bounce we accepted. 
        ///   If Postmark does not have a dump for that bounce, it will return an empty string.
        /// </summary>
        /// <param name = "bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public PostmarkBounceDump GetBounceDump(string bounceId)
        {
            var request = NewBouncesRequest();
            request.Path = string.Format("bounces/{0}/dump", bounceId.Trim());

            var response = _client.Request(request);

            return JsonConvert.DeserializeObject<PostmarkBounceDump>(response.Content, _settings);
        }

        /// <summary>
        ///   Activates a deactivated bounce.
        /// </summary>
        /// <param name = "bounceId">The bounce ID</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public PostmarkBounceActivation ActivateBounce(string bounceId)
        {
            var request = NewBouncesRequest();
            request.Method = WebMethod.Put;
            request.Path = string.Format("bounces/{0}/activate", bounceId.Trim());
            
            var response = _client.Request(request);

            return JsonConvert.DeserializeObject<PostmarkBounceActivation>(response.Content, _settings);
        }

        #endregion

        #region Messages API

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns></returns>
        public PostmarkOutboundMessageList GetOutboundMessages(int count, string subject, int offset)
        {
            return GetOutboundMessagesImpl(null, null, null, subject, count, offset);
        }


        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <returns></returns>
        public PostmarkOutboundMessageList GetOutboundMessages(int count, int offset, string recipient)
        {
            return GetOutboundMessagesImpl(recipient, null, null, null, count, offset);
        }


        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns></returns>
        public PostmarkOutboundMessageList GetOutboundMessages(int count, int offset)
        {
            return GetOutboundMessagesImpl(null, null, null, null, count, offset);
        }

         /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns></returns>
        public PostmarkOutboundMessageList GetOutboundMessages(string recipient, string fromemail,
            int count, int offset)
        {
            return GetOutboundMessagesImpl(recipient, fromemail, null, null, count, offset);
        }

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns></returns>
        public PostmarkOutboundMessageList GetOutboundMessages(string subject, int count, int offset)
        {
            return GetOutboundMessagesImpl(null, null, null, subject, count, offset);
        }

       /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="tag">Filter by a tag used for the message (messages sent directly through the API only)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <returns></returns>
        public PostmarkOutboundMessageList GetOutboundMessages(string fromemail, string tag,
            string subject, int count, int offset)
        {
            return GetOutboundMessagesImpl(null, fromemail, tag, subject, count, offset);
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
        /// <returns></returns>
        public PostmarkOutboundMessageList GetOutboundMessages(string recipient, string fromemail, string tag,
            string subject, int count, int offset)
        {
            return GetOutboundMessagesImpl(recipient, fromemail, tag, subject, count, offset);
        }


        /// <summary>
        /// Implementation called to do the actual messages call and return a <see cref="PostmarkOutboundMessageList"/>
        /// </summary>s
        private PostmarkOutboundMessageList GetOutboundMessagesImpl(string recipient, string fromemail, string tag, string subject, int count, int offset)
        {
            var request = NewMessagesRequest();
            request.Path = "messages/outbound";

            if (!string.IsNullOrEmpty(recipient)) request.AddParameter("recipient", recipient);
            if (!string.IsNullOrEmpty(fromemail)) request.AddParameter("fromemail", fromemail);
            if (!string.IsNullOrEmpty(tag)) request.AddParameter("tag", tag);
            if (!string.IsNullOrEmpty(subject)) request.AddParameter("subject", subject);
               
            request.AddParameter("count", count.ToString());
            request.AddParameter("offset", offset.ToString());

            var response = _client.Request(request);
            return JsonConvert.DeserializeObject<PostmarkOutboundMessageList>(response.Content, _settings);
        }

        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns></returns>
        public MessageDetail GetMessageDetail(string messageID)
        {
            var request = NewMessagesRequest();
            request.Path = string.Format("messages/outbound/{0}/details", messageID.Trim());
            
            var response = _client.Request(request);
            return JsonConvert.DeserializeObject<MessageDetail>(response.Content, _settings);
        }

        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns></returns>
        public MessageDump GetMessageDump(string messageID)
        {
            var request = NewMessagesRequest();
            request.Path = string.Format("messages/outbound/{0}/dump", messageID.Trim());

            var response = _client.Request(request);
            return JsonConvert.DeserializeObject<MessageDump>(response.Content, _settings);
        }

        #endregion
#endif

        private void SetPostmarkMeta(RestBase request)
        {
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("X-Postmark-Server-Token", ServerToken);
            request.AddHeader("User-Agent", "Postmark.NET");
        }

        private static void CleanPostmarkMessage(PostmarkMessage message)
        {
            message.From = message.From.Trim();
            if(!string.IsNullOrEmpty(message.To))
            {
                message.To = message.To.Trim();    
            }
            message.Subject = message.Subject != null ? message.Subject.Trim() : "";
        }

#if !WINDOWS_PHONE
        private PostmarkResponse GetPostmarkResponse(RestRequest request)
        {
            var response = _client.Request(request);

            return GetPostmarkResponseImpl(response);
        }

        private IEnumerable<PostmarkResponse> GetPostmarkResponses(RestRequest request)
        {
            var response = _client.Request(request);

            return GetPostmarkResponsesImpl(response);
        }
#endif

        private static IEnumerable<PostmarkResponse> GetPostmarkResponsesImpl(RestResponseBase response)
        {
            var results = TryGetPostmarkResponses(response) ?? new List<PostmarkResponse>
                        {
                            new PostmarkResponse
                                {
                                    Status = PostmarkStatus.Unknown,
                                    Message = response.StatusDescription
                                }
                        };

            foreach(var result in results)
            {
                switch ((int)response.StatusCode)
                {
                    case 200:
                        result.Status = PostmarkStatus.Success;
                        break;
                    case 401:
                    case 422:
                        result.Status = PostmarkStatus.UserError;
                        break;
                    case 500:
                        result.Status = PostmarkStatus.ServerError;
                        break;
                }
            }
            
            return results;
        }

        private static PostmarkResponse GetPostmarkResponseImpl(RestResponseBase response)
        {
            var result = TryGetPostmarkResponse(response) ?? new PostmarkResponse
            {
                Status = PostmarkStatus.Unknown,
                Message = response.StatusDescription
            };

            switch ((int)response.StatusCode)
            {
                case 200:
                    result.Status = PostmarkStatus.Success;
                    break;
                case 401:
                case 422:
                    result.Status = PostmarkStatus.UserError;
                    break;
                case 500:
                    result.Status = PostmarkStatus.ServerError;
                    break;
            }

            return result;
        }

        private static PostmarkResponse TryGetPostmarkResponse(RestResponseBase response)
        {
            PostmarkResponse result = null;
            var statusCode = (int)response.StatusCode;
            if (statusCode == 200 || statusCode == 401 || statusCode == 422 || statusCode == 500)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<PostmarkResponse>(response.Content, _settings);
                }
                catch (JsonReaderException)
                {
                    result = null;
                }
            }
            return result;
        }

        private static IEnumerable<PostmarkResponse> TryGetPostmarkResponses(RestResponseBase response)
        {
            IEnumerable<PostmarkResponse> result = null;
            var statusCode = (int)response.StatusCode;
            if (statusCode == 200 || statusCode == 401 || statusCode == 422 || statusCode == 500)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<IEnumerable<PostmarkResponse>>(response.Content, _settings);
                }
                catch (JsonReaderException)
                {
                    result = null;
                }
            }
            return result;
        }

        private RestRequest NewBouncesRequest()
        {
            var request = new RestRequest
            {
                Serializer = _serializer
            };

            SetPostmarkMeta(request);

            return request;
        }

        private RestRequest NewEmailRequest()
        {
            var request = new RestRequest
            {
                Path = "email",
                Method = WebMethod.Post,
                Serializer = _serializer
            };

            SetPostmarkMeta(request);

            return request;
        }

        private RestRequest NewBatchedEmailRequest()
        {
            var request = new RestRequest
            {
                Path = "email/batch",
                Method = WebMethod.Post,
                Serializer = _serializer
            };

            SetPostmarkMeta(request);

            return request;
        }

        private RestRequest NewMessagesRequest()
        {
            var request = new RestRequest
            {
                Method = WebMethod.Get,
                Serializer = _serializer
            };

            SetPostmarkMeta(request);

            return request;
        }
    }
}