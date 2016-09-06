using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    /// <summary>
    /// A paged list of message opens information.
    /// </summary>
    public class PostmarkOpensList
    {
        public int TotalCount { get; set; }
        public IEnumerable<PostmarkOpen> Opens { get; set; }
    }

    public class PostmarkOpen
    {
        /// <summary>
        /// Indicates that this is the first open for the associated message.
        /// </summary>
        public bool FirstOpen { get; set; }

        /// <summary>
        /// The associated message's ID.
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// The original user-agent header provided by the recipient's email client.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Any known geo data for the this Open.
        /// </summary>
        public PostmarkGeographyOpenInfo Geo { get; set; }

        /// <summary>
        /// What was the platform used to open the message?
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// The reading time.
        /// </summary>
        public int ReadSeconds { get; set; }

        /// <summary>
        /// Information about the opening client application, extracted from the UserAgent string.
        /// </summary>
        public PostmarkAgentInfo Client { get; set; }

        /// <summary>
        /// Information about the opening client operating system, extracted from the UserAgent string.
        /// </summary>
        public PostmarkAgentInfo OS { get; set; }
    }

    /// <summary>
    /// Representation of the payload of the open tracking webhook
    /// </summary>
    public class PostmarkOpenWebhookMessage : PostmarkOpen
    {
        /// <summary>
        ///   The time the open was received by the Postmark servers.
        /// </summary>
        /// <value>The time the open was received</value>
        public DateTime ReceivedAt { get; set; }

        /// <summary>
        /// The tags users add to emails
        /// </summary>
        /// <value>The specific tag string</value>
        public string Tag { get; set; }

        /// <summary>
        /// The email address of the recipient who opened the email.
        /// </summary>
        /// <value>Email address of the recipient</value>
        public string Recipient { get; set; }
    }


    public class PostmarkAgentInfo
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Family { get; set; }
    }

    public class PostmarkGeographyOpenInfo
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string IP { get; set; }
        public string RegionISOCode { get; set; }
        public string CountryISOCode { get; set; }
        public string Coords { get; set; }
    }
}
