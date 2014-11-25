using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientStatisticsTests : ClientBaseFixture
    {
        public override async Task Setup()
        {
            _client = new PostmarkClient(READ_SELENIUM_TEST_SERVER_TOKEN);
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
            //Nobody complains about our test system, so we can't really test this one.
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

            var lastMonth = TESTING_DATE - TimeSpan.FromDays(30);
            var lastMonthEndWindow = TESTING_DATE - TimeSpan.FromDays(5);

            var allOfLastMonth = await _client.GetOutboundOverviewStatsAsync(null, lastMonth);
            var windowFromLastMonth = await _client.GetOutboundOverviewStatsAsync(null, lastMonth, lastMonthEndWindow);

            var windowFromLastMonthWithTag = await _client.GetOutboundOverviewStatsAsync("test_tag", lastMonth, lastMonthEndWindow);


            Assert.Greater(allOutboundStats.Sent, 0, "Sent should be greater than 0");
            Assert.Greater(allOfLastMonth.Sent, 0, "All Last Month Sent should be greater than 0");
            Assert.Greater(windowFromLastMonth.Sent, 0, "Window from Last Month Sent should be greater than 0");
            Assert.Greater(windowFromLastMonthWithTag.Sent, 0, "Window from Last Month with Tag Sent should be greater than 0");

            Assert.Greater(allOutboundStats.Sent, allOfLastMonth.Sent);
            Assert.Greater(allOfLastMonth.Sent, windowFromLastMonth.Sent);
            Assert.Greater(windowFromLastMonth.Sent, windowFromLastMonthWithTag.Sent);
        }


        [TestCase]
        public async void Client_CanGetOutboundStatisticsSentCounts()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsBounceCounts()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsSpamComplaints()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsTrackingCounts()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsPlatformCounts()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsClientCounts()
        {
            throw new NotImplementedException();
        }

        [TestCase]
        public async void Client_CanGetOutboundStatisticsReadingTimeCounts()
        {
            throw new NotImplementedException();
        }

    }
}
