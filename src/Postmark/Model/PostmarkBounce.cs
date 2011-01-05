using System;

namespace PostmarkDotNet
{
    /// <summary>
    ///   Represents an email bounce logged by Postmark.
    /// </summary>
    public class PostmarkBounce
    {
        /// <summary>
        ///   The bounce ID.
        ///   This is used for other API calls that require the ID.
        /// </summary>
        /// <value>The ID</value>
        public string ID { get; set; }

        /// <summary>
        ///   The <see cref = "PostmarkBounceType" /> for this bounce.
        /// </summary>
        /// <value>The type</value>
        public PostmarkBounceType Type { get; set; }

        /// <summary>
        ///   The bounce details set by the server.
        /// </summary>
        /// <value>The details</value>
        public string Details { get; set; }

        /// <summary>
        ///   The email recipient that initiated the bounce.
        /// </summary>
        /// <value>The email</value>
        public string Email { get; set; }

        /// <summary>
        ///   The time the bounce occurred.
        /// </summary>
        /// <value>The time of the bounce</value>
        public DateTime BouncedAt { get; set; }

        /// <summary>
        ///   A value indicating whether a raw STMP dump is available.
        /// </summary>
        /// <value><c>true</c> if a dump is available; otherwise, <c>false</c></value>
        public bool DumpAvailable { get; set; }

        /// <summary>
        ///   A value indicating whether this <see cref = "PostmarkBounce" /> is inactive.
        /// </summary>
        /// <value><c>true</c> if inactive; otherwise, <c>false</c></value>
        public bool Inactive { get; set; }

        /// <summary>
        ///   A value indicating whether this bounce can be activated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can be activated; otherwise, <c>false</c>
        /// </value>
        public bool CanActivate { get; set; }
    }
}