using NUnit.Framework;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class ClientTemplateTests : ClientBaseFixture
    {
        protected override async Task SetupAsync()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN, API_BASE_URL);
            await CompletionSource;
        }

        [TestCase]
        public async Task ClientCanCreateTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlbody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await _client.CreateTemplateAsync(name, subject, htmlbody, textBody);

            Assert.AreEqual(name, newTemplate.Name);
        }

        [TestCase]
        public async Task ClientCanEditTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlbody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await _client.CreateTemplateAsync(name, subject, htmlbody, textBody);

            var existingTemplate = await _client.GetTemplateAsync(newTemplate.TemplateId);

            await _client.EditTemplateAsync(existingTemplate.TemplateId, name + name, subject + subject, htmlbody + htmlbody, textBody + textBody);

            var updatedTemplate = await _client.GetTemplateAsync(existingTemplate.TemplateId);

            Assert.AreEqual(existingTemplate.Name + existingTemplate.Name, updatedTemplate.Name);
            Assert.AreEqual(existingTemplate.HtmlBody + existingTemplate.HtmlBody, updatedTemplate.HtmlBody);
            Assert.AreEqual(existingTemplate.Subject + existingTemplate.Subject, updatedTemplate.Subject);
            Assert.AreEqual(existingTemplate.TextBody + existingTemplate.TextBody, updatedTemplate.TextBody);
        }

        [TestCase]
        public async Task ClientCanDeleteTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlbody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await _client.CreateTemplateAsync(name, subject, htmlbody, textBody);
            await _client.DeleteTemplateAsync(newTemplate.TemplateId);
            var deletedTemplate = await _client.GetTemplateAsync(newTemplate.TemplateId);

            Assert.False(deletedTemplate.Active);
        }

        [TestCase]
        public async Task ClientCanGetTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlbody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await _client.CreateTemplateAsync(name, subject, htmlbody, textBody);

            var result = await _client.GetTemplateAsync(newTemplate.TemplateId);

            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(htmlbody, result.HtmlBody);
            Assert.AreEqual(textBody, result.TextBody);
            Assert.AreEqual(subject, result.Subject);
            Assert.IsTrue(result.Active);
            Assert.Greater(result.AssociatedServerId, 0);
            Assert.AreEqual(newTemplate.TemplateId, result.TemplateId);
        }

        [TestCase]
        public async Task ClientCanGetListTemplates()
        {
            for (var i = 0; i < 10; i++)
            {
                await _client.CreateTemplateAsync("test " + i, "test subject" + i, "body");
            }

            var result = await _client.GetTemplatesAsync();
            Assert.AreEqual(10, result.TotalCount);
            var toDelete = result.Templates.First().TemplateId;
            await _client.DeleteTemplateAsync(toDelete);
            result = await _client.GetTemplatesAsync();
            Assert.AreEqual(9, result.TotalCount);
            Assert.False(result.Templates.Any(k => k.TemplateId == toDelete));
            var offsetResults = await _client.GetTemplatesAsync(5);

            Assert.True(result.Templates.Skip(5).Select(k => k.TemplateId).SequenceEqual(offsetResults.Templates.Select(k => k.TemplateId)));

            result = await _client.GetTemplatesAsync(count: 10, includeDeletedTemplates: true);
            Assert.AreEqual(10, result.TotalCount);
        }

        [TestCase]
        public async Task ClientCanValidateTemplate()
        {
            var result = await _client.ValidateTemplateAsync("{{name}}", "<html><body>{{content}}{{company.address}}</body></html>", "{{content}}", new { name = "Johnny", content = "hello, world!" });

            Assert.IsTrue(result.AllContentIsValid);
            Assert.IsTrue(result.HtmlBody.ContentIsValid);
            Assert.IsTrue(result.TextBody.ContentIsValid);
            Assert.IsTrue(result.Subject.ContentIsValid);
            var inferredAddress = result.SuggestedTemplateModel.company.address;
            Assert.IsNotNull(inferredAddress);
        }

        [TestCase]
        public async Task ClientCanSendWithTemplate()
        {
            var template = await _client.CreateTemplateAsync("test template name", "test subject", "test html body");
            var sendResult = await _client.SendEmailWithTemplateAsync(template.TemplateId, new { name = "Andrew" }, WRITE_TEST_SENDER_EMAIL_ADDRESS, WRITE_TEST_SENDER_EMAIL_ADDRESS, false);
            Assert.AreNotEqual(Guid.Empty, sendResult.MessageID);
        }

        [TearDown]
        public virtual async void CleanupTemplates()
        {
            var tasks = new List<Task>();
            foreach (var t in (await _client.GetTemplatesAsync()).Templates)
            {
                tasks.Add(_client.DeleteTemplateAsync(t.TemplateId));
            }
            await Task.WhenAll(tasks);
        }

    }
}
