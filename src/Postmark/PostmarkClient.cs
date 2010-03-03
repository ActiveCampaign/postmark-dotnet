#region License

// Postmark
// http://postmarkapp.com
// (c) 2010 Wildbit
// 
// 
// Postmark.NET
// http://github.com/lunarbits/postmark-dotnet
// 
// The MIT License
// 
// Copyright (c) 2010 Daniel Crenna
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// 
// Json.NET 
// http://codeplex.com/json
// 
// Copyright (c) 2007 James Newton-King
// 
// The MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// 
// RestSharp
// http://github.com/johnsheehan/RestSharp 
// 
// Copyright (c) 2010 John Sheehan
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License. 

#endregion

using System.Collections.Specialized;
using System.Net.Mail;
using Newtonsoft.Json;
using PostmarkDotNet.Converters;
using PostmarkDotNet.Serializers;
using PostmarkDotNet.Validation;
using RestSharp;

/*
del /q "$(TargetDir)$(TargetName).dll"
del /q "$(TargetDir)$(TargetName).pdb"
"$(SolutionDir)..\lib\ILMerge.exe" /t:library /out:"$(TargetDir)$(TargetName).dll" "$(TargetDir)Newtonsoft.Json.dll" "$(TargetDir)RestSharp.dll" "$(TargetDir)RestSharp.dll"
 */

namespace PostmarkDotNet
{
    /// <summary>
    /// A client for the Postmark application. 
    /// Use this client in place of an <see cref="SmtpClient" /> to send messages
    /// through this service.
    /// </summary>
    public class PostmarkClient
    {
        private readonly RestClient _client;
        private static readonly JsonSerializerSettings _settings;
        private static readonly PostmarkSerializer _serializer;

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
        /// Initializes a new instance of the <see cref="PostmarkClient"/> class.
        /// If you do not have a server token you can request one by signing up to
        /// use Postmark: http://postmarkapp.com.
        /// </summary>
        /// <param name="serverToken">The server token.</param>
        public PostmarkClient(string serverToken)
        {
            ServerToken = serverToken;
            _client = new RestClient
                          {
                              BaseUrl = "http://api.postmarkapp.com/email"
                          };
        }

        /// <summary>
        /// Gets the server token issued with your Postmark email server configuration.
        /// </summary>
        /// <value>The server token.</value>
        public string ServerToken { get; private set; }

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
        /// <returns>A <see cref="PostmarkResponse"/> with details about the transaction.</returns>
        public PostmarkResponse SendMessage(string from, string to, string subject, string body)
        {
            return SendMessage(from, to, subject, body, null);
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
        /// <returns>A <see cref="PostmarkResponse"/> with details about the transaction.</returns>       
        public PostmarkResponse SendMessage(string from, string to, string subject, string body, NameValueCollection headers)
        {
            var message = new PostmarkMessage(from, to, subject, body, headers);

            return SendMessage(message);
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
        public PostmarkResponse SendMessage(PostmarkMessage message)
        {
            var request = NewRequest();

            request.AddParameter("Accept", "application/json", ParameterType.HttpHeader);
            request.AddParameter("Content-Type", "application/json; charset=utf-8", ParameterType.HttpHeader);
            request.AddParameter("X-Postmark-Server-Token", ServerToken, ParameterType.HttpHeader);
            request.AddParameter("User-Agent", "Postmark.NET", ParameterType.HttpHeader);

            ValidatePostmarkMessage(message);
            CleanPostmarkMessage(message);

            request.AddBody(message);

            return GetResponse(request);
        }

        private static void CleanPostmarkMessage(PostmarkMessage message)
        {
            message.From = message.From.Trim();
            message.To = message.To.Trim();
            message.Subject = message.Subject != null ? message.Subject.Trim() : "";
        }

        private static void ValidatePostmarkMessage(PostmarkMessage message)
        {
            var specification = new ValidEmailSpecification();
            if (string.IsNullOrEmpty(message.From) || !specification.IsSatisfiedBy(message.From))
            {
                throw new ValidationException("You must specify a valid 'From' email address.");
            }
            if (string.IsNullOrEmpty(message.To) || !specification.IsSatisfiedBy(message.To))
            {
                throw new ValidationException("You must specify a valid 'To' email address.");
            }
            if (!string.IsNullOrEmpty(message.ReplyTo) && !specification.IsSatisfiedBy(message.ReplyTo))
            {
                throw new ValidationException("If a 'ReplyTo' email address is included, it must be valid.");
            }
        }

        private PostmarkResponse GetResponse(RestRequest request)
        {
            var response = _client.Execute(request);

            PostmarkResponse result;
            switch ((int) response.StatusCode)
            {
                case 200:
                    result = new PostmarkResponse
                                 {
                                     Status = PostmarkStatus.Success,
                                     Message = response.StatusDescription
                                 };
                    break;
                case 401:
                case 422:
                    result = JsonConvert.DeserializeObject<PostmarkResponse>(response.Content, _settings);
                    result.Status = PostmarkStatus.UserError;
                    break;
                case 500:
                    result = new PostmarkResponse
                                 {
                                     Status = PostmarkStatus.ServerError,
                                     Message = response.StatusDescription
                                 };
                    break;
                default:
                    result = new PostmarkResponse
                                 {
                                     Status = PostmarkStatus.Unknown,
                                     Message = response.StatusDescription
                                 };
                    break;
            }

            return result;
        }

        private static RestRequest NewRequest()
        {
            var request = new RestRequest
                              {
                                  Verb = Method.POST,
                                  RequestFormat = RequestFormat.Json,
                                  JsonSerializer = _serializer
                              };

            return request;
        }
    }
}