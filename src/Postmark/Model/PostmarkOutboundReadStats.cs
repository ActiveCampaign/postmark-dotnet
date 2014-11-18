using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundReadStats
    {

        public PostmarkOutboundReadStats()
        {
            this.ReadCounts = new Dictionary<string, int>();
        }

        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedReadCount> Days { get; set; }

        public IDictionary<string, int> ReadCounts { get; set; }

        public class DatedReadCount
        {
            public DatedReadCount()
            {
                this.ReadCounts = new Dictionary<string, int>();
            }

            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            public IDictionary<string, int> ReadCounts { get; set; }
        }
    }
}
