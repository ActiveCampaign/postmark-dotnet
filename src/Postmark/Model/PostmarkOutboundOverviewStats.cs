
namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundOverviewStats
    {
        public int Sent { get; set; }
        public int Bounced { get; set; }
        public int SMTPApiErrors { get; set; }
        public int BounceRate { get; set; }
        public int SpamComplaints { get; set; }
        public int SpamComplaintsRate { get; set; }
        public int Opens { get; set; }
        public int UniqueOpens { get; set; }
        public int Tracked { get; set; }
        public int WithClientRecorded { get; set; }
        public int WithPlatformRecorded { get; set; }
        public int WithReadTimeRecorded { get; set; }
    }
}
