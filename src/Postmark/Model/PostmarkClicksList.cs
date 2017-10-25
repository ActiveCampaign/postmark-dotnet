using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    /// <summary>
    /// A paged list of message clicks information.
    /// </summary>
    public class PostmarkClicksList
    {
        public int TotalCount { get; set; }
        public IEnumerable<PostmarkClick> Clicks { get; set; }
    }

    public class PostmarkClick
    {
        /// <summary>
        /// The original link that was clicked by this recipient.
        /// </summary>
        public string OriginalLink { get; set; }

        /// <summary>
        /// The associated message's ID.
        /// </summary>
        public string MessageID { get; set; }

        /// <summary>
        /// The original user-agent header provided by the recipient's email client.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Any known geo data for the this click.
        /// </summary>
        public PostmarkGeographyInfo Geo { get; set; }

        /// <summary>
        /// What was the platform used to click the message?
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Information about the client application, extracted from the UserAgent string.
        /// </summary>
        public PostmarkAgentInfo Client { get; set; }

        /// <summary>
        /// Information about the client operating system, extracted from the UserAgent string.
        /// </summary>
        public PostmarkAgentInfo OS { get; set; }
    }
}
