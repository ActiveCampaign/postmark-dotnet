using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundPlatformStats
    {
        /// <summary>
        /// Outbound message counts, broken out by date.
        /// </summary>
        public IEnumerable<DatedPlatformCount> Days { get; set; }

        /// <summary>
        /// The number of Desktop opens.
        /// </summary>
        public int Desktop { get; set; }

        /// <summary>
        /// The number of Mobile opens.
        /// </summary>
        public int Mobile { get; set; }

        /// <summary>
        /// The number of opens from an 'Unknown' type of client.
        /// </summary>

        public int Unknown { get; set; }
        /// <summary>
        /// The number of WebMail opens.
        /// </summary>
        public int WebMail { get; set; }

        public class DatedPlatformCount
        {
            /// <summary>
            /// The date for these stats, with date resolution.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// The number of Desktop opens for this date.
            /// </summary>
            public int Desktop { get; set; }

            /// <summary>
            /// The number of Mobile opens for this date.
            /// </summary>
            public int Mobile { get; set; }

            /// <summary>
            /// The number of opens from an 'Unknown' type of client for this date.
            /// </summary>

            public int Unknown { get; set; }
            /// <summary>
            /// The number of WebMail opens for this date.
            /// </summary>
            public int WebMail { get; set; }
        }
    }
}
