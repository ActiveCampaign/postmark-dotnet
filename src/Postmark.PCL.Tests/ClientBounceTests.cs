using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientBounceTests : ClientBaseFixture
    {
        public async override Task Setup()
        {
            _client = new PostmarkClient(READ_INBOUND_TEST_SERVER_TOKEN);
        }

        [TestCase]
        public async void Client_CanGetBounceDeliveryStats()
        {
            var result = await _client.GetDeliveryStatsAsync();
            Assert.NotNull(result);
            Assert.Greater(result.Bounces.Count, 0);
        }

        [TestCase]
        public async void Client_CanGetBounces()
        {
            var result = await _client.GetBouncesAsync();
            Assert.NotNull(result);
            Assert.Greater(result.Bounces.Count, 0);
        }

        [TestCase]
        public async void Client_CanRetrieveSingleBounce()
        {
            var bounces = await _client.GetBouncesAsync(0, 20);
            var firstBounce = bounces.Bounces.First();

            var retrievedBounce = await _client.GetBounceAsync(firstBounce.ID);
            Assert.NotNull(retrievedBounce);
        }


        [TestCase]
        public async void Client_CanGetABounceDump()
        {
            var bounces = await _client.GetBouncesAsync();
            var firstBounceWithDump = bounces.Bounces.First(g => g.DumpAvailable);
            var dump = await _client.GetBounceDumpAsync(firstBounceWithDump.ID);
            Assert.NotNull(dump);
        }

        [TestCase]
        public async void Client_CanActivateABounce()
        {
            throw new NotImplementedException();
        }


    }
}
