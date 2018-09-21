using Xunit;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postmark.Tests
{
    public class ClientTriggersTests : ClientBaseFixture, IDisposable
    {
        private string _triggerPrefix = "integration-testing-";
        private string _inboundRulePrefix = "integration-test";

        protected override void Setup()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
        }

        public ClientTriggersTests(): base(){
            this.Cleanup().Wait();
        }

        private Task Cleanup()
        {
            return Task.Run(async() =>
            {
                try
                {
                    var triggers = await _client.SearchTaggedTriggers();
                    var tasks = new List<Task>();
                    foreach (var trigger in triggers.Tags)
                    {
                        if (trigger.MatchName.StartsWith(_triggerPrefix))
                        {
                            var dt = _client.DeleteTagTrigger(trigger.ID);
                            tasks.Add(dt);
                        }
                    }
                    var inboundTriggers = await _client.GetAllInboundRuleTriggers();
                    foreach (var inboundRule in inboundTriggers.InboundRules)
                    {
                        if (inboundRule.Rule.StartsWith(_inboundRulePrefix))
                        {
                            var dt = _client.DeleteInboundRuleTrigger(inboundRule.ID);
                            tasks.Add(dt);
                        }
                    }

                    await Task.WhenAll(tasks);
                }
                catch
                {
                    //don't fail the tests because cleanup didn't happen.
                }
            });
        }

        [Theory]
        [InlineData("qwerty", false)]
        [InlineData("pdq", true)]
        public async void Client_CanCreateTagTrigger(string matchName, bool trackOpens)
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + matchName, trackOpens);
            var savedTrigger = await _client.GetTagTriggerAsync(trigger.ID);

            Assert.Equal(trigger.MatchName, savedTrigger.MatchName);
            Assert.Equal(trigger.TrackOpens, savedTrigger.TrackOpens);
            Assert.Equal(trigger.ID, savedTrigger.ID);
            Assert.Equal(_triggerPrefix + matchName, savedTrigger.MatchName);
            Assert.Equal(trackOpens, savedTrigger.TrackOpens);
        }

        [Fact]
        public async void Client_CanGetTagTrigger()
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + "new-trigger" + DateTime.Now.Ticks);
            var savedTrigger = await _client.GetTagTriggerAsync(trigger.ID);

            Assert.NotNull(savedTrigger);
        }

        [Fact]
        public async void Client_CanEditTagTrigger()
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + "new-trigger" + DateTime.Now.Ticks, false);
            var updatedTrigger = await _client.UpdateTagTriggerAsync(trigger.ID, _triggerPrefix + "updated" + DateTime.Now.Ticks, true);

            Assert.NotEqual(trigger.MatchName, updatedTrigger.MatchName);
            Assert.NotEqual(trigger.TrackOpens, updatedTrigger.TrackOpens);
        }

        [Fact]
        public async void Client_CanDeleteTagTrigger()
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + "new-trigger");
            var result = await _client.DeleteTagTrigger(trigger.ID);

            Assert.NotNull(result);
            Assert.Equal(0, result.ErrorCode);
            Assert.Equal(PostmarkStatus.Success, result.Status);
        }

        [Fact]
        public async void Client_CanSearchTagTriggers()
        {
            var nameprefix = _triggerPrefix + "-tag-" + TESTING_DATE.ToString("o");
            var names = Enumerable.Range(0, 10).Select(k => nameprefix + Guid.NewGuid()).ToArray();

            var awaitables = names.Select(name => _client.CreateTagTriggerAsync(name));
            var results = await Task.WhenAll(awaitables);

            foreach (var name in names)
            {
                var result = (await _client.SearchTaggedTriggers(0, 100, name)).Tags;
                Assert.Equal(name, result.Single().MatchName);
            }

            var allTriggers = await _client.SearchTaggedTriggers();
            Assert.Equal(names.Count(), allTriggers.Tags.Count(k => k.MatchName.StartsWith(nameprefix)));
        }

        [Fact]
        public async void Client_CanCreateInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com",
                _inboundRulePrefix, Guid.NewGuid());

            var newrule = await _client.CreateInboundRuleTriggerAsync(rule);

            Assert.NotNull(newrule);
            Assert.Equal(rule, newrule.Rule);
            Assert.True(newrule.ID >= 0);
        }

        [Fact]
        public async void Client_CanDeleteInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com", _inboundRulePrefix, Guid.NewGuid());
            var newrule = await _client.CreateInboundRuleTriggerAsync(rule);
            await _client.DeleteInboundRuleTrigger(newrule.ID);
            var results = await _client.GetAllInboundRuleTriggers();
            Assert.True(results.InboundRules.All(k => k.Rule != rule));
        }

        [Fact]
        public async void Client_CanListInboundTriggers()
        {
            var names = Enumerable.Range(0, 5)
                .Select(k => String.Format("{0}+{1:n}@example.com",
                    _inboundRulePrefix, Guid.NewGuid())).ToArray();

            var awaitables = names.Select(name => _client.CreateInboundRuleTriggerAsync(name)).ToArray();
            await Task.WhenAll(awaitables);

            var inbound = await _client.GetAllInboundRuleTriggers();
            var rules = inbound.InboundRules.Select(k => k.Rule).ToArray();

            Assert.True(rules.Length >= names.Length);
            Assert.True(names.All(k => rules.Contains(k)));
        }

        [Fact]
        public async void Client_CanGetInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com", _inboundRulePrefix, Guid.NewGuid());
            var newRule = await _client.CreateInboundRuleTriggerAsync(rule);
            var rules = await _client.GetAllInboundRuleTriggers();

            var retrievedRule = rules.InboundRules.First(k => k.ID == newRule.ID);

            Assert.Equal(newRule.ID, retrievedRule.ID);
            Assert.Equal(newRule.Rule, retrievedRule.Rule);
        }

        public void Dispose()
        {
            this.Cleanup().Wait();
        }
    }
}