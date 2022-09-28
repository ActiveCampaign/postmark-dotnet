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

        public int AddressChange { get; set; }

        public int AutoResponder { get; set; }

        public int BadEmailAddress { get; set; }

        public int Blocked { get; set; }

        public int ChallengeVerification { get; set; }

        public int DMARCPolicy { get; set; }

        public int DnsError { get; set; }

        public int InboundError { get; set; }

        public int ManuallyDeactivated { get; set; }

        public int OpenRelayTest { get; set; }

        public int SpamNotification { get; set; }

        public int Subscribe { get; set; }

        public int TemplateRenderingFailed { get; set; }

        public int Unconfirmed { get; set; }

        public int Unknown { get; set; }

        public int Unsubscribe { get; set; }

        public int VirusNotification { get; set; }

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

            public int AddressChange { get; set; }

            public int AutoResponder { get; set; }

            public int BadEmailAddress { get; set; }

            public int Blocked { get; set; }

            public int ChallengeVerification { get; set; }

            public int DMARCPolicy { get; set; }

            public int DnsError { get; set; }

            public int InboundError { get; set; }

            public int ManuallyDeactivated { get; set; }

            public int OpenRelayTest { get; set; }

            public int SpamNotification { get; set; }

            public int Subscribe { get; set; }

            public int TemplateRenderingFailed { get; set; }

            public int Unconfirmed { get; set; }

            public int Unknown { get; set; }

            public int Unsubscribe { get; set; }

            public int VirusNotification { get; set; }
        }
    }
}