using System;
using System.Collections.Generic;
using System.Linq;
using Hammock;
using Hammock.Web;
using Newtonsoft.Json;
using PostmarkDotNet.Converters;
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

            ValidatePostmarkMessage(message);

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
                ValidatePostmarkMessage(message);
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
        public PostmarkBounces GetBounces(PostmarkBounceType type, bool? inactive, string emailFilter, string tag,
                                          int offset, int count)
        {
            var request = NewBouncesRequest();
            request.Path = "bounces";
            if (inactive.HasValue) request.AddParameter("inactive", inactive.Value.ToString().ToLowerInvariant());
            if (!string.IsNullOrEmpty(emailFilter)) request.AddParameter("emailFilter", emailFilter);
            if (!string.IsNullOrEmpty(tag)) request.AddParameter("tag", tag);
            request.AddParameter("type", type.ToString());
            request.AddParameter("offset", offset.ToString());
            request.AddParameter("count", count.ToString());

            var response = _client.Request(request);

            return JsonConvert.DeserializeObject<PostmarkBounces>(response.Content, _settings);
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
            return GetBounces(type, null, null, null, offset, count);
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
            return GetBounces(type, inactive, null, null, offset, count);
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
            return GetBounces(type, null, emailFilter, null, offset, count);
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
            return GetBounces(type, null, emailFilter, tag, offset, count);
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
            return GetBounces(type, inactive, emailFilter, null, offset, count);
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

        private static void ValidatePostmarkMessage(PostmarkMessage message)
        {
            var specification = new ValidEmailSpecification();
            if (string.IsNullOrEmpty(message.From) || !specification.IsSatisfiedBy(message.From))
            {
                throw new ValidationException("You must specify a valid 'From' email address.");
            }
            if (!string.IsNullOrEmpty(message.To))
            {
                var recipients = message.To.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var recipient in recipients.Where(email => !specification.IsSatisfiedBy(email)))
                {
                    throw new ValidationException(
                        string.Format("The provided recipient address '{0}' is not valid", recipient)
                        );
                }
            }

            if (!string.IsNullOrEmpty(message.ReplyTo) && !specification.IsSatisfiedBy(message.ReplyTo))
            {
                throw new ValidationException("If a 'ReplyTo' email address is included, it must be valid.");
            }

            if (!string.IsNullOrEmpty(message.Cc))
            {
                var ccs = message.Cc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cc in ccs.Where(email => !specification.IsSatisfiedBy(email)))
                {
                    throw new ValidationException(
                        string.Format("The provided CC address '{0}' is not valid", cc)
                        );
                }
            }

            if (!string.IsNullOrEmpty(message.Bcc))
            {
                var bccs = message.Bcc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var bcc in bccs.Where(email => !specification.IsSatisfiedBy(email)))
                {
                    throw new ValidationException(
                        string.Format("The provided BCC address '{0}' is not valid", bcc)
                        );
                }
            }
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
    }
}