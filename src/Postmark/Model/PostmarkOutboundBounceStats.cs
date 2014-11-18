using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundBounceStats
    {
        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedBounceCount> Days { get; set; }

        public int HardBounce { get; set; }

        public int SMTPApiError { get; set; }

        public int SoftBounce { get; set; }

        public int Transient { get; set; }

        public class DatedBounceCount
        {
            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            public int HardBounce { get; set; }

            public int SMTPApiError { get; set; }

            public int SoftBounce { get; set; }

            public int Transient { get; set; }
        }
    }
}
