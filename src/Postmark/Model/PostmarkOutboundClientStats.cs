using System;
using System.Collections.Generic;
using System.Linq;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundClientStats
    {
        public PostmarkOutboundClientStats()
        {
            ClientCounts = new Dictionary<string, int>();
        }

        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedClientCount> Days { get; set; }

        public int Total()
        {
            return this.ClientCounts.Sum(k => k.Value);
        }

        /// <summary>
        /// The number of messages sent where 
        /// the key is the client name, and the value is the
        /// number sent for that client.
        /// </summary>
        public IDictionary<string, int> ClientCounts { get; set; }

        public class DatedClientCount
        {
            public DatedClientCount()
            {
                this.ClientCounts = new Dictionary<string, int>();
            }

            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// The number of messages sent on this date where 
            /// the key is the client name, and the value is the
            /// number sent for that client.
            /// </summary>
            public IDictionary<string, int> ClientCounts { get; set; }

            public int Total()
            {
                return this.ClientCounts.Sum(k => k.Value);
            }
        }
    }
}
