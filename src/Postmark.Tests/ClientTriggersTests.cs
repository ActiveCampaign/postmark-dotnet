using Xunit;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postmark.Tests
{
    public class ClientTriggersTests : ClientBaseFixture, IAsyncLifetime
    {
        private string _triggerPrefix = "integration-testing-";
        private string _inboundRulePrefix = "integration-test";

        protected override void Setup()
        {
            Client = new PostmarkClient(WriteTestServerToken, BaseUrl);
        }

        [Fact]
        public async void Client_CanCreateInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com",
                _inboundRulePrefix, Guid.NewGuid());

            var newrule = await Client.CreateInboundRuleTriggerAsync(rule);

            Assert.NotNull(newrule);
            Assert.Equal(rule, newrule.Rule);
            Assert.True(newrule.ID >= 0);
        }

        [Fact]
        public async void Client_CanDeleteInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com", _inboundRulePrefix, Guid.NewGuid());
            var newrule = await Client.CreateInboundRuleTriggerAsync(rule);
            await Client.DeleteInboundRuleTrigger(newrule.ID);
            var results = await Client.GetAllInboundRuleTriggers();
            Assert.True(results.InboundRules.All(k => k.Rule != rule));
        }

        [Fact]
        public async void Client_CanListInboundTriggers()
        {
            var names = Enumerable.Range(0, 5)
                .Select(k => String.Format("{0}+{1:n}@example.com",
                    _inboundRulePrefix, Guid.NewGuid())).ToArray();

            var awaitables = names.Select(name => Client.CreateInboundRuleTriggerAsync(name)).ToArray();
            await Task.WhenAll(awaitables);

            var inbound = await Client.GetAllInboundRuleTriggers();
            var rules = inbound.InboundRules.Select(k => k.Rule).ToArray();

            Assert.True(rules.Length >= names.Length);
            Assert.True(names.All(k => rules.Contains(k)));
        }

        [Fact]
        public async void Client_CanGetInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com", _inboundRulePrefix, Guid.NewGuid());
            var newRule = await Client.CreateInboundRuleTriggerAsync(rule);
            var rules = await Client.GetAllInboundRuleTriggers();

            var retrievedRule = rules.InboundRules.First(k => k.ID == newRule.ID);

            Assert.Equal(newRule.ID, retrievedRule.ID);
            Assert.Equal(newRule.Rule, retrievedRule.Rule);
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try
            {
                var tasks = new List<Task>();

                var inboundTriggers = await Client.GetAllInboundRuleTriggers();
                foreach (var inboundRule in inboundTriggers.InboundRules)
                {
                    if (inboundRule.Rule.StartsWith(_inboundRulePrefix))
                    {
                        var dt = Client.DeleteInboundRuleTrigger(inboundRule.ID);
                        tasks.Add(dt);
                    }
                }

                await Task.WhenAll(tasks);
            }
            catch
            {
                //don't fail the tests because cleanup didn't happen.
            }
        }
    }
}