using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundTrackedStats
    {
        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedTrackedCount> Days { get; set; }

        /// <summary>
        /// The total number of messages tracked for the associated time period.
        /// </summary>
        public int Tracked { get; set; }

        public class DatedTrackedCount
        {
            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// The number of messages tracked on this date.
            /// </summary>
            public int Tracked { get; set; }
        }
    }
}
