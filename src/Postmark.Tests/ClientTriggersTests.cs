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

        public ClientTriggersTests() : base()
        {
            this.Cleanup().Wait();
        }

        private Task Cleanup()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var tasks = new List<Task>();
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