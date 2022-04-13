using System;
using System.Runtime.Serialization;

namespace Postmark.Exceptions
{
    [Serializable]
    public class PostmarkResponseException : Exception
    {
        public string Body { get; private set; }

        public PostmarkResponseException(string body)
        {
            Body = body;
        }

        public PostmarkResponseException(string message, string body) : base(message)
        {
            Body = body;
        }

        public PostmarkResponseException(string message, string body, Exception inner) : base(message, inner)
        {
            Body = body;
        }

        protected PostmarkResponseException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
