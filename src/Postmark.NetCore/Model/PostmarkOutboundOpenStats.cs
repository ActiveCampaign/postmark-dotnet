using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundOpenStats
    {
        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedOpenCount> Days { get; set; }

        /// <summary>
        /// The number of open events.
        /// </summary>
        public int Opens { get; set; }

        /// <summary>
        /// The number of unique open events.
        /// </summary>
        public int Unique { get; set; }

        public class DatedOpenCount
        {
            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// The number of messages opened on this date.
            /// </summary>
            public int Opens { get; set; }

            /// <summary>
            /// The number of unique open events for this date.
            /// </summary>
            public int Unique { get; set; }
        }
    }
}
