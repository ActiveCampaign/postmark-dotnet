using Xunit;
using PostmarkDotNet;
using System;
using System.Linq;

namespace Postmark.Tests
{
    public class ClientStatisticsTests : ClientBaseFixture
    {
        private readonly DateTime _lastMonth;
        private DateTime WindowStartDate { get; set; }

        public ClientStatisticsTests()
        {
            Client = new PostmarkClient(ReadSeleniumTestServerToken, BaseUrl);
            _lastMonth = TestingDate - TimeSpan.FromDays(30);
            WindowStartDate = TestingDate - TimeSpan.FromDays(35);
        }

        [Fact]
        public async void Client_CanGetOutboundStatisticsOverview()
        {
            var outboundStats = await Client.GetOutboundOverviewStatsAsync();
            Assert.True(outboundStats.Bounced > 0, "Bounced should be greater than 0");
            Assert.True(outboundStats.BounceRate > 0, "BounceRade should be greater than 0");
            Assert.True(outboundStats.Opens > 0, "Opens should be greater than 0");
            Assert.True(outboundStats.Sent > 0, "Sent should be greater than 0");
            Assert.True(outboundStats.SMTPApiErrors > 0, "SMTPApiErrors should be greater than 0");
            //FIXME: Nobody complains about our test system, so we can't really test this one.
            //Assert.Greater(outboundStats.SpamComplaints, 0, "SpamComplaints should be greater than 0");
            //Assert.Greater(outboundStats.SpamComplaintsRate, 0, "SpamComplaintsRate should be greater than 0");
            Assert.True(outboundStats.Tracked > 0, "Tracked should be greater than 0");
            Assert.True(outboundStats.UniqueOpens > 0, "UniqueOpens should be greater than 0");
            Assert.True(outboundStats.WithClientRecorded > 0, "WithClientRecorded should be greater than 0");
            Assert.True(outboundStats.WithPlatformRecorded > 0, "WithPlatformRecorded should be greater than 0");
            Assert.True(outboundStats.WithReadTimeRecorded > 0, "WithReadTimeRecorded should be greater than 0");
        }

        [Fact]
        public async void Client_CanGetOutboundStatisticsOverviewWithTimeWindowOrTagFilter()
        {
            var allOutboundStats = await Client.GetOutboundOverviewStatsAsync();
            var allStatsThroughLastMonth = await Client.GetOutboundOverviewStatsAsync(null, _lastMonth);
            var windowFromLastMonth = await Client.GetOutboundOverviewStatsAsync(null, WindowStartDate, _lastMonth);
            var windowFromLastMonthWithTag = await Client.GetOutboundOverviewStatsAsync("test_tag", WindowStartDate, _lastMonth);
            var outboundStatsForStream = await Client.GetOutboundOverviewStatsAsync(null, null, null, "test_stream");

            AssertStats(allOutboundStats, allStatsThroughLastMonth, windowFromLastMonth, windowFromLastMonthWithTag, outboundStatsForStream, f => f.Sent);
        }

        private void AssertStats<T>(
            T alltimeStats, T allstatsThroughLastMonth, T windowFromLastMonth, T windowFromLastMonthWithTag, T outboundStatsForStream, Func<T, int> statGetter)
        {
            Assert.True(statGetter(alltimeStats) > 0, "Stat should be greater than 0");
            Assert.True(statGetter(allstatsThroughLastMonth) > 0, "All Last Month Stat should be greater than 0");
            Assert.True(statGetter(windowFromLastMonth) > 0, "Window from Last Month Stat should be greater than 0");
            Assert.True(statGetter(windowFromLastMonthWithTag) > 0, "Window from Last Month with Tag Stat should be greater than 0");
            Assert.True(statGetter(outboundStatsForStream) > 0, "Stat for stream should be greater than 0");

            Assert.True(statGetter(alltimeStats) > statGetter(allstatsThroughLastMonth));
            Assert.True(statGetter(allstatsThroughLastMonth) > statGetter(windowFromLastMonth));
            Assert.True(statGetter(windowFromLastMonth) > statGetter(windowFromLastMonthWithTag));
        }


        [Fact(Skip = "Not possible to test this easily, due to infrequent test runs.")]
        public async void Client_CanGetOutboundStatisticsSentCounts()
        {
            var allOutboundStats = await Client.GetOutboundSentCountsAsync();
            var allStatsThroughLastMonth = await Client.GetOutboundSentCountsAsync(null, _lastMonth);
            var windowFromLastMonth = await Client.GetOutboundSentCountsAsync(null, WindowStartDate, _lastMonth);
            var windowFromLastMonthWithTag = await Client.GetOutboundSentCountsAsync("test_tag", WindowStartDate, _lastMonth);
            var outboundStatsForStream = await Client.GetOutboundSentCountsAsync(null, null, null, "test_stream");

            AssertStats(allOutboundStats, allStatsThroughLastMonth, windowFromLastMonth, windowFromLastMonthWithTag, outboundStatsForStream, f => f.Sent);

            Assert.True(allOutboundStats.Days.Count() > 0);
            Assert.Equal(allOutboundStats.Sent, allOutboundStats.Days.Sum(k => k.Sent));
        }

        [Fact]
        public async void Client_CanGetOutboundStatisticsBounceCounts()
        {
            var allOutboundStats = await Client.GetOutboundBounceCountsAsync();
            Assert.True(allOutboundStats.Days.Count() > 0);
        }

        [Fact(Skip = "There is no suitable test data for this test.")]
        public async void Client_CanGetOutboundStatisticsSpamComplaints()
        {
            var allOutboundStats = await Client.GetOutboundSpamComplaintCountsAsync();
            Assert.True(allOutboundStats.Days.Count() > 0);
            Assert.Equal(allOutboundStats.SpamComplaint, allOutboundStats.Days.Sum(k => k.SpamComplaint));
        }

        [Fact]
        public async void Client_CanGetOutboundStatisticsTrackingCounts()
        {
            var allOutboundStats = await Client.GetOutboundTrackingCountsAsync();
            Assert.Equal(allOutboundStats.Tracked, allOutboundStats.Days.Sum(k => k.Tracked));
            Assert.True(allOutboundStats.Tracked > 0);
        }

        [Fact]
        public async void Client_CanGetOutboundStatisticsPlatformCounts()
        {
            var allOutboundStats = await Client.GetOutboundPlatformCountsAsync();

            Assert.True(allOutboundStats.Days.Count() > 0);
            Assert.Equal(allOutboundStats.Desktop, allOutboundStats.Days.Sum(k => k.Desktop));
            Assert.Equal(allOutboundStats.WebMail, allOutboundStats.Days.Sum(k => k.WebMail));
            Assert.Equal(allOutboundStats.Mobile, allOutboundStats.Days.Sum(k => k.Mobile));
            Assert.Equal(allOutboundStats.Unknown, allOutboundStats.Days.Sum(k => k.Unknown));

            ////There are cases where a platform is going to be zero.. can't test these.
            //Assert.Greater(allOutboundStats.Desktop, 0);
            //Assert.Greater(allOutboundStats.WebMail, 0);
            //Assert.Greater(allOutboundStats.Mobile, 0);
            //Assert.Greater(allOutboundStats.Unknown, 0);
        }

        [Fact(Skip = "There is no suitable test data for this test.")]
        public async void Client_CanGetOutboundStatisticsClientCounts()
        {
            var allOutboundStats = await Client.GetOutboundClientUsageCountsAsync();
            var allStatsThroughLastMonth = await Client.GetOutboundClientUsageCountsAsync(null, _lastMonth);
            var windowFromLastMonth = await Client.GetOutboundClientUsageCountsAsync(null, WindowStartDate, _lastMonth);
            var windowFromLastMonthWithTag = await Client.GetOutboundClientUsageCountsAsync("test_tag", WindowStartDate, _lastMonth);
            var outboundStatsForStream = await Client.GetOutboundClientUsageCountsAsync(null, null, null, "test_stream");

            AssertStats(allOutboundStats, allStatsThroughLastMonth, windowFromLastMonth,
                windowFromLastMonthWithTag, outboundStatsForStream, f => f.ClientCounts.Sum(k => k.Value));

            Assert.True(allOutboundStats.Days.Count() > 0);
            Assert.Equal(allOutboundStats.ClientCounts.Sum(k => k.Value),
                allOutboundStats.Days.Sum(k => k.ClientCounts.Sum(d => d.Value)));
        }

        [Fact]
        public async void Client_CanGetOutboundStatisticsReadingTimeCounts()
        {
            var readingTimes = await Client.GetOutboundReadtimeStatsAsync();

            Assert.True(readingTimes.Days.Count() > 0);
            Assert.True(readingTimes.ReadCounts.Any());
            Assert.True(readingTimes.Days.All(f => f.ReadCounts.Any()));
        }
    }
}