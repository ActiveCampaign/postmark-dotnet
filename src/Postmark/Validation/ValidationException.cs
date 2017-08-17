using System;

namespace PostmarkDotNet.Validation
{
    /// <summary>
    /// An exception thrown when requests fail to validate against the API.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ValidationException" /> class.
        /// </summary>
        public ValidationException()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ValidationException" /> class.
        /// </summary>
        /// <param name = "message">The message.</param>
        public ValidationException(string message) : base(message)
        {
        }
    }
}