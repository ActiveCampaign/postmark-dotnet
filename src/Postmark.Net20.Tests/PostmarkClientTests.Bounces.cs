using NUnit.Framework;
using PostmarkDotNet;

namespace Postmark.Tests
{
    partial class PostmarkClientTests
    {
        [Test]
        public void Can_get_delivery_stats()
        {
            var postmark = new PostmarkClient(_serverToken);
            var stats = postmark.GetDeliveryStats();
            Assert.IsNotNull(stats);
        }

        [Test]
        public void Can_get_bounces()
        {
            var postmark = new PostmarkClient(_serverToken);
            var stats = postmark.GetBounces(PostmarkBounceType.HardBounce, 1, 10);
            Assert.IsNotNull(stats);
        }

        [Test]
        public void Can_get_bounce_tags()
        {
            var postmark = new PostmarkClient(_serverToken);
            var stats = postmark.GetBounceTags();
            Assert.IsNotNull(stats);
        }
    }
}
