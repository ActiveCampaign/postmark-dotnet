using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundClientStats
    {
        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedClientCount> Days { get; set; }

        /// <summary>
        /// The total number of messages sent for the associated time period.
        /// </summary>
        public int Sent { get; set; }

        public class DatedClientCount
        {
            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// The number of messages sent on this date.
            /// </summary>
            public int Sent { get; set; }
        }
    }
}
