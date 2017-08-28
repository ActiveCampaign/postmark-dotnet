
namespace PostmarkDotNet.Model
{
    public class PostmarkOutboundOverviewStats
    {
        public int Sent { get; set; }
        public int Bounced { get; set; }
        public int SMTPApiErrors { get; set; }
        public double BounceRate { get; set; }
        public int SpamComplaints { get; set; }
        public double SpamComplaintsRate { get; set; }
        public int Opens { get; set; }
        public int UniqueOpens { get; set; }
        public int Tracked { get; set; }
        public int WithClientRecorded { get; set; }
        public int WithPlatformRecorded { get; set; }
        public int WithReadTimeRecorded { get; set; }
        public int TotalClicks { get; set; }
        public int UniqueLinksClicked { get; set; }
        public int TotalTrackedLinksSent { get; set; }
        public int WithLinkTracking { get; set; }
        public int WithOpenTracking { get; set; }
    }
}
