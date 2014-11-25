using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientTriggersTests : ClientBaseFixture
    {
        private string _triggerPrefix = "integration-testing-";
        private string _inboundRulePrefix = "integration-test";

        public override async Task Setup()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
        }

        [TearDown]
        public async void Teardown()
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

        [TestCase("qwerty", false)]
        [TestCase("pdq", true)]
        public async void Client_CanCreateTagTrigger(string matchName, bool trackOpens)
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + matchName, trackOpens);
            var savedTrigger = await _client.GetTagTriggerAsync(trigger.ID);

            Assert.AreEqual(trigger.MatchName, savedTrigger.MatchName);
            Assert.AreEqual(trigger.TrackOpens, savedTrigger.TrackOpens);
            Assert.AreEqual(trigger.ID, savedTrigger.ID);
            Assert.AreEqual(_triggerPrefix + matchName, savedTrigger.MatchName);
            Assert.AreEqual(trackOpens, savedTrigger.TrackOpens);
        }

        [TestCase]
        public async void Client_CanGetTagTrigger()
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + "new-trigger");
            var savedTrigger = await _client.GetTagTriggerAsync(trigger.ID);

            Assert.NotNull(savedTrigger);
        }

        [TestCase]
        public async void Client_CanEditTagTrigger()
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + "new-trigger");
            var updatedTrigger = await _client.UpdateTagTriggerAsync(trigger.ID, _triggerPrefix + "updated", true);

            Assert.AreNotEqual(trigger.MatchName, updatedTrigger.MatchName);
            Assert.AreNotEqual(trigger.TrackOpens, updatedTrigger.TrackOpens);
        }

        [TestCase]
        public async void Client_CanDeleteTagTrigger()
        {
            var trigger = await _client.CreateTagTriggerAsync(_triggerPrefix + "new-trigger");
            var result = await _client.DeleteTagTrigger(trigger.ID);

            Assert.NotNull(result);
            Assert.AreEqual(0, result.ErrorCode);
            Assert.AreEqual(PostmarkStatus.Success, result.Status);
        }

        [TestCase]
        public async void Client_CanSearchTagTriggers()
        {
            var nameprefix = _triggerPrefix + "-tag-" + TESTING_DATE.ToString("o");
            var names = Enumerable.Range(0, 10).Select(k => nameprefix + Guid.NewGuid()).ToArray();

            var awaitables = names.Select(name => _client.CreateTagTriggerAsync(name));
            var results = await Task.WhenAll(awaitables);

            foreach (var name in names)
            {
                var result = (await _client.SearchTaggedTriggers(0, 100, name)).Tags;
                Assert.AreEqual(name, result.Single().MatchName);
            }

            var allTriggers = await _client.SearchTaggedTriggers();
            Assert.AreEqual(names.Count(), allTriggers.Tags.Count(k => k.MatchName.StartsWith(nameprefix)));
        }

        [TestCase]
        public async void Client_CanCreateInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com",
                _inboundRulePrefix, Guid.NewGuid());

            var newrule = await _client.CreateInboundRuleTriggerAsync(rule);

            Assert.NotNull(newrule);
            Assert.AreEqual(rule, newrule.Rule);
            Assert.Greater(newrule.ID, 0);
        }

        [TestCase]
        public async void Client_CanDeleteInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com", _inboundRulePrefix, Guid.NewGuid());
            var newrule = await _client.CreateInboundRuleTriggerAsync(rule);
            await _client.DeleteInboundRuleTrigger(newrule.ID);
            var results = await _client.GetAllInboundRuleTriggers();
            Assert.IsTrue(results.InboundRules.All(k => k.Rule != rule));
        }

        [TestCase]
        public async void Client_CanListInboundTriggers()
        {
            var names = Enumerable.Range(0, 5)
                .Select(k => String.Format("{0}+{1:n}@example.com",
                    _inboundRulePrefix, Guid.NewGuid())).ToArray();

            var awaitables = names.Select(name => _client.CreateInboundRuleTriggerAsync(name)).ToArray();
            await Task.WhenAll(awaitables);

            var inbound = await _client.GetAllInboundRuleTriggers();
            var rules = inbound.InboundRules.Select(k => k.Rule).ToArray();

            Assert.GreaterOrEqual(rules.Length, names.Length);
            Assert.IsTrue(names.All(k => rules.Contains(k)));
        }

        [TestCase]
        public async void Client_CanGetInboundTrigger()
        {
            var rule = String.Format("{0}+{1:n}@example.com", _inboundRulePrefix, Guid.NewGuid());
            var newRule = await _client.CreateInboundRuleTriggerAsync(rule);
            var rules = await _client.GetAllInboundRuleTriggers();

            var retrievedRule = rules.InboundRules.First(k => k.ID == newRule.ID);

            Assert.AreEqual(newRule.ID, retrievedRule.ID);
            Assert.AreEqual(newRule.Rule, retrievedRule.Rule);
        }
    }
}