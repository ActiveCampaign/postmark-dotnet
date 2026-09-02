using System;
using System.Net;
using System.Runtime.Serialization;

namespace Postmark.Exceptions
{
    [Serializable]
    public class PostmarkResponseException : Exception
    {
        public string Body { get; private set; }

        /// <summary>
        /// The status code on the response for the request that caused this exception.  This may be null if the
        /// exception was thrown before a response was received or if no response is available.
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; private set; }

        public PostmarkResponseException(string body) : this(body, (HttpStatusCode?)null)
        {
        }

        public PostmarkResponseException(string body, HttpStatusCode? responseStatusCode)
        {
            Body = body;
            ResponseStatusCode = responseStatusCode;
        }

        public PostmarkResponseException(string message, string body) : this(message, body, (HttpStatusCode?)null)
        {
        }

        public PostmarkResponseException(string message, string body, HttpStatusCode? responseStatusCode) : base(message)
        {
            Body = body;
            ResponseStatusCode = responseStatusCode;
        }

        public PostmarkResponseException(string message, string body, Exception inner) : this(message, body, inner, null)
        {
        }

        public PostmarkResponseException(string message, string body, Exception inner, HttpStatusCode? responseStatusCode) : base(message, inner)
        {
            Body = body;
            ResponseStatusCode = responseStatusCode;
        }

        /// <summary>
        /// Note: Body and ResponseStatusCode do not survive binary serialization; restoring them would
        /// require overriding GetObjectData and reading them back here. Binary exception serialization
        /// is obsolete in modern .NET, so this is left unimplemented deliberately.
        /// </summary>
        protected PostmarkResponseException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
