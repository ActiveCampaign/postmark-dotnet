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
        protected async override Task SetupAsync()
        {
            _client = new PostmarkClient(READ_SELENIUM_TEST_SERVER_TOKEN);
            await CompletionSource;
        }

        [TestCase]
        public async void Client_CanGetBounceDeliveryStats()
        {
            var result = await _client.GetDeliveryStatsAsync();
            Assert.Greater(result.Bounces.Count, 0);
        }

        [TestCase]
        public async void Client_CanGetBounces()
        {
            var result = await _client.GetBouncesAsync();
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

        [Ignore("We can't run this test because can't do a write on the testing server account.")]
        [TestCase]
        public void Client_CanActivateABounce()
        {
            //Unfortunately, can't activate bounces on the testing server account.
        }


    }
}
