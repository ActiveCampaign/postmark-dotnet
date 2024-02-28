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
        public long ID { get; set; }

        /// <summary>
        ///   The <see cref = "PostmarkBounceType" /> for this bounce.
        /// </summary>
        /// <value>The type</value>
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<PostmarkBounceType>))]
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

        /// <summary>
        ///   The MessageID of the email that caused the bounce, can be Empty.
        /// </summary>
        /// <value>
        ///   The original email message ID
        /// </value>
        public Guid MessageID { get; set; }

        /// <summary>
        ///   The detailed information about the cause of the bounce 
        /// </summary>
        /// <value>The description of the bounce</value>
        public string Description { get; set; }

        /// <summary>
        /// The tags users add to emails
        /// </summary>
        /// <value>The specific tag string</value>
        public string Tag { get; set; }

        /// <summary>
        /// Subject of the message sent
        /// </summary>
        /// <value>Subject text of the original message</value>
        public string Subject { get; set; }
        
        /// <summary>
        /// The original sender of the email, if available.
        /// </summary>
        public string From { get; set; }
        
        /// <summary>
        /// The ID of the Server that sent the original message.
        /// </summary>
        public int ServerID { get; set; }
        
        /// <summary>
        /// The Bounce Type "Friendly Name"
        /// </summary>
        public string Name { get; set; }
    }
}
