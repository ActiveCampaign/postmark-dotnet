using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundSpamComplaintStats
    {
        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedSpamComplaintCount> Days { get; set; }

        /// <summary>
        /// Indicates total number of spam complaints.
        /// </summary>
        public int SpamComplaint { get; set; }

        public class DatedSpamComplaintCount
        {
            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Indicates total number of spam complaints for this date.
            /// </summary>
            public int SpamComplaint { get; set; }
        }
    }
}
