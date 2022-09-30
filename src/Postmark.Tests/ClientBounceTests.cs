using System;
using Xunit;
using PostmarkDotNet;
using System.Linq;

namespace Postmark.Tests
{
    public class ClientBounceTests : ClientBaseFixture
    {
        public ClientBounceTests()
        {
            Client = new PostmarkClient(ReadSeleniumTestServerToken, BaseUrl);
        }

        [Fact]
        public async void Client_CanGetOutboundBounceCounts()
        {
            var result = await Client.GetOutboundBounceCountsAsync();
            Assert.NotEmpty(result.Days);
        }

        [Fact]
        public async void Client_CanGetBounceDeliveryStats()
        {
            var result = await Client.GetDeliveryStatsAsync();
            Assert.True(result.Bounces.Count > 0);
        }

        [Fact]
        public async void Client_CanGetBounces()
        {
            var result = await Client.GetBouncesAsync();
            Assert.True(result.Bounces.Count > 0);
        }

        [Fact]
        public async void Client_CanRetrieveSingleBounce()
        {
            var bounces = await Client.GetBouncesAsync(0, 20);
            var firstBounce = bounces.Bounces.First();

            var retrievedBounce = await Client.GetBounceAsync(firstBounce.ID);
            Assert.NotNull(retrievedBounce);
        }


        [Fact]
        public async void Client_CanGetABounceDump()
        {
            var bounces = await Client.GetBouncesAsync();
            var firstBounceWithDump = bounces.Bounces.First(g => g.DumpAvailable);
            var dump = await Client.GetBounceDumpAsync(firstBounceWithDump.ID);
            Assert.NotNull(dump);
        }

        [Fact(Skip = "We can't run this test because can't do a write on the testing server account.")]
        public void Client_CanActivateABounce()
        {
            //Unfortunately, can't activate bounces on the testing server account.
        }
    }
}