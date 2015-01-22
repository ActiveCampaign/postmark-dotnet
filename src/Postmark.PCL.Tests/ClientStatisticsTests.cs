using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientStatisticsTests : ClientBaseFixture
    {
        private DateTime _lastMonth;
        public DateTime _windowStartDate { get; set; }

        protected override async Task SetupAsync()
        {
            _client = new PostmarkClient(READ_SELENIUM_TEST_SERVER_TOKEN);
            _lastMonth = TESTING_DATE - TimeSpan.FromDays(30);
            _windowStartDate = TESTING_DATE - TimeSpan.FromDays(35);
            await CompletionSource;
        }


        [TestCase]
        public async void Client_CanGetOutboundStatisticsOverview()
        {
            var outboundStats = await _client.GetOutboundOverviewStatsAsync();
            Assert.Greater(outboundStats.Bounced, 0, "Bounced should be greater than 0");
            Assert.Greater(outboundStats.BounceRate, 0, "BounceRade should be greater than 0");
            Assert.Greater(outboundStats.Opens, 0, "Opens should be greater than 0");
            Assert.Greater(outboundStats.Sent, 0, "Sent should be greater than 0");
            Assert.Greater(outboundStats.SMTPApiErrors, 0, "SMTPApiErrors should be greater than 0");
            Assert.Greater(outboundStats.SpamComplaints, 0, "SpamComplaints should be greater than 0");
            //FIXME: Nobody complains about our test system, so we can't really test this one.
            //Assert.Greater(outboundStats.SpamComplaintsRate, 0, "SpamComplaintsRate should be greater than 0");
            Assert.Greater(outboundStats.Tracked, 0, "Tracked should be greater than 0");
            Assert.Greater(outboundStats.UniqueOpens, 0, "UniqueOpens should be greater than 0");
            Assert.Greater(outboundStats.WithClientRecorded, 0, "WithClientRecorded should be greater than 0");
            Assert.Greater(outboundStats.WithPlatformRecorded, 0, "WithPlatformRecorded should be greater than 0");
            Assert.Greater(outboundStats.WithReadTimeRecorded, 0, "WithReadTimeRecorded should be greater than 0");
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsOverviewWithTimeWindowOrTagFilter()
        {
            var allOutboundStats = await _client.GetOutboundOverviewStatsAsync();
            var allStatsThroughLastMonth = await _client.GetOutboundOverviewStatsAsync(null, _lastMonth);
            var windowFromLastMonth = await _client.GetOutboundOverviewStatsAsync(null, _windowStartDate, _lastMonth);
            var windowFromLastMonthWithTag = await _client.GetOutboundOverviewStatsAsync("test_tag", _windowStartDate, _lastMonth);

            AssertStats(allOutboundStats, allStatsThroughLastMonth, windowFromLastMonth, windowFromLastMonthWithTag, f => f.Sent);
        }

        private void AssertStats<T>(T alltimeStats, T allstatsThroughLastMonth, T windowFromLastMonth, T windowFromLastMonthWithTag, Func<T, int> statGetter)
        {
            Assert.Greater(statGetter(alltimeStats), 0, "Stat should be greater than 0");
            Assert.Greater(statGetter(allstatsThroughLastMonth), 0, "All Last Month Stat should be greater than 0");
            Assert.Greater(statGetter(windowFromLastMonth), 0, "Window from Last Month Stat should be greater than 0");
            Assert.Greater(statGetter(windowFromLastMonthWithTag), 0, "Window from Last Month with Tag Stat should be greater than 0");

            Assert.Greater(statGetter(alltimeStats), statGetter(allstatsThroughLastMonth));
            Assert.Greater(statGetter(allstatsThroughLastMonth), statGetter(windowFromLastMonth));
            Assert.Greater(statGetter(windowFromLastMonth), statGetter(windowFromLastMonthWithTag));
        }


        [Ignore]
        [TestCase]
        public async void Client_CanGetOutboundStatisticsSentCounts()
        {
            var allOutboundStats = await _client.GetOutboundSentCountsAsync();
            var allStatsThroughLastMonth = await _client.GetOutboundSentCountsAsync(null, _lastMonth);
            var windowFromLastMonth = await _client.GetOutboundSentCountsAsync(null, _windowStartDate, _lastMonth);
            var windowFromLastMonthWithTag = await _client.GetOutboundSentCountsAsync("test_tag", _windowStartDate, _lastMonth);

            AssertStats(allOutboundStats, allStatsThroughLastMonth, windowFromLastMonth, windowFromLastMonthWithTag, f => f.Sent);

            Assert.Greater(allOutboundStats.Days.Count(), 0);
            Assert.AreEqual(allOutboundStats.Sent, allOutboundStats.Days.Sum(k => k.Sent));

        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsBounceCounts()
        {
            var allOutboundStats = await _client.GetOutboundBounceCountsAsync();
            Assert.Greater(allOutboundStats.Days.Count(), 0);
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsSpamComplaints()
        {
            var allOutboundStats = await _client.GetOutboundSpamComplaintCountsAsync();
            Assert.Greater(allOutboundStats.Days.Count(), 0);
            Assert.AreEqual(allOutboundStats.SpamComplaint, allOutboundStats.Days.Sum(k => k.SpamComplaint));
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsTrackingCounts()
        {
            var allOutboundStats = await _client.GetOutboundTrackingCountsAsync();
            Assert.AreEqual(allOutboundStats.Tracked, allOutboundStats.Days.Sum(k => k.Tracked));
            Assert.Greater(allOutboundStats.Tracked, 0);
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsPlatformCounts()
        {
            var allOutboundStats = await _client.GetOutboundPlatformCountsAsync();

            Assert.Greater(allOutboundStats.Days.Count(), 0);
            Assert.AreEqual(allOutboundStats.Desktop, allOutboundStats.Days.Sum(k => k.Desktop));
            Assert.AreEqual(allOutboundStats.WebMail, allOutboundStats.Days.Sum(k => k.WebMail));
            Assert.AreEqual(allOutboundStats.Mobile, allOutboundStats.Days.Sum(k => k.Mobile));
            Assert.AreEqual(allOutboundStats.Unknown, allOutboundStats.Days.Sum(k => k.Unknown));

            ////There are cases where a platform is going to be zero.. can't test these.
            //Assert.Greater(allOutboundStats.Desktop, 0);
            //Assert.Greater(allOutboundStats.WebMail, 0);
            //Assert.Greater(allOutboundStats.Mobile, 0);
            //Assert.Greater(allOutboundStats.Unknown, 0);

        }

        [Ignore("There is no suitable test data for this test.")]
        [TestCase]
        public async void Client_CanGetOutboundStatisticsClientCounts()
        {
            var allOutboundStats = await _client.GetOutboundClientUsageCountsAsync();
            var allStatsThroughLastMonth = await _client.GetOutboundClientUsageCountsAsync(null, _lastMonth);
            var windowFromLastMonth = await _client.GetOutboundClientUsageCountsAsync(null, _windowStartDate, _lastMonth);
            var windowFromLastMonthWithTag = await _client.GetOutboundClientUsageCountsAsync("test_tag", _windowStartDate, _lastMonth);

            AssertStats(allOutboundStats, allStatsThroughLastMonth, windowFromLastMonth,
                windowFromLastMonthWithTag, f => f.ClientCounts.Sum(k => k.Value));

            Assert.Greater(allOutboundStats.Days.Count(), 0);
            Assert.AreEqual(allOutboundStats.ClientCounts.Sum(k => k.Value),
                allOutboundStats.Days.Sum(k => k.ClientCounts.Sum(d => d.Value)));
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsReadingTimeCounts()
        {
            var readingTimes = await _client.GetOutboundReadtimeStatsAsync();

            Assert.Greater(readingTimes.Days.Count(), 0);
            Assert.IsTrue(readingTimes.ReadCounts.Any());
            Assert.IsTrue(readingTimes.Days.All(f => f.ReadCounts.Any()));
        }


    }
}
