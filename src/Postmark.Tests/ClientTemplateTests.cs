using Xunit;
using PostmarkDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostmarkDotNet.Model;

namespace Postmark.Tests
{
    public class ClientTemplateTests : ClientBaseFixture, IDisposable
    {
        private readonly string _layoutContentPlaceholder = "{{{@content}}}";

        protected override void Setup()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN, BASE_URL);
        }

        [Fact]
        public async void ClientCanCreateTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlBody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await _client.CreateTemplateAsync(name, subject, htmlBody, textBody);

            Assert.Equal(name, newTemplate.Name);
        }

        [Fact]
        public async void ClientCanCreateLayoutTemplates()
        {
            var newLayoutTemplate = await GenerateLayoutTemplate();
            Assert.Equal(TemplateType.Layout, newLayoutTemplate.TemplateType);

            // Creating a standard template that uses the layout template above
            var newStandardTemplate = await GenerateStandardTemplate(newLayoutTemplate.Alias);

            Assert.Equal(TemplateType.Standard, newStandardTemplate.TemplateType);
            Assert.Equal(newLayoutTemplate.Alias, newStandardTemplate.LayoutTemplate);
        }

        [Fact]
        public async void ClientCanEditTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlbody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await _client.CreateTemplateAsync(name, subject, htmlbody, textBody);

            var existingTemplate = await _client.GetTemplateAsync(newTemplate.TemplateId);

            await _client.EditTemplateAsync(existingTemplate.TemplateId, name + name, subject + subject, htmlbody + htmlbody, textBody + textBody);

            var updatedTemplate = await _client.GetTemplateAsync(existingTemplate.TemplateId);

            Assert.Equal(existingTemplate.Name + existingTemplate.Name, updatedTemplate.Name);
            Assert.Equal(existingTemplate.HtmlBody + existingTemplate.HtmlBody, updatedTemplate.HtmlBody);
            Assert.Equal(existingTemplate.Subject + existingTemplate.Subject, updatedTemplate.Subject);
            Assert.Equal(existingTemplate.TextBody + existingTemplate.TextBody, updatedTemplate.TextBody);
        }

        [Fact]
        public async void ClientCanEditLayoutTemplateProperty()
        {
            var newLayoutTemplate = await GenerateLayoutTemplate();
            var newStandardTemplate = await GenerateStandardTemplate(newLayoutTemplate.Alias);

            // Setting the LayoutTemplate to null
            var templateWithNoLayoutTemplate = await _client.EditTemplateAsync(newStandardTemplate.TemplateId, layoutTemplate: "");
            Assert.Null(templateWithNoLayoutTemplate.LayoutTemplate);

            // Setting the LayoutTemplate back to the layout template that was created
            var templateWithLayoutTemplate = await _client.EditTemplateAsync(newStandardTemplate.TemplateId,
                layoutTemplate: newLayoutTemplate.Alias);
            Assert.Equal(newLayoutTemplate.Alias, templateWithLayoutTemplate.LayoutTemplate);
        }

        [Fact]
        public async void ClientCanDeleteTemplate()
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

        [Fact]
        public async void ClientCanGetTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlbody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await _client.CreateTemplateAsync(name, subject, htmlbody, textBody);

            var result = await _client.GetTemplateAsync(newTemplate.TemplateId);

            Assert.Equal(name, result.Name);
            Assert.Equal(htmlbody, result.HtmlBody);
            Assert.Equal(textBody, result.TextBody);
            Assert.Equal(subject, result.Subject);
            Assert.True(result.Active);
            Assert.True(result.AssociatedServerId > 0);
            Assert.Equal(newTemplate.TemplateId, result.TemplateId);
            Assert.Equal(TemplateType.Standard, result.TemplateType);
            Assert.Null(result.LayoutTemplate);
        }

        [Fact]
        public async void ClientCanGetListTemplates()
        {
            for (var i = 0; i < 10; i++)
            {
                await _client.CreateTemplateAsync("test " + i, "test subject" + i, "body");
            }

            var result = await _client.GetTemplatesAsync();
            Assert.Equal(10, result.TotalCount);
            var toDelete = result.Templates.First().TemplateId;
            await _client.DeleteTemplateAsync(toDelete);
            result = await _client.GetTemplatesAsync();
            Assert.Equal(9, result.TotalCount);
            Assert.False(result.Templates.FirstOrDefault(k => k.TemplateId == toDelete) != null);
            var offsetResults = await _client.GetTemplatesAsync(5);
            Assert.True(result.Templates.Skip(5).Select(k => k.TemplateId).SequenceEqual(offsetResults.Templates.Select(k => k.TemplateId)));
        }

        [Fact]
        public async void GetTemplatesReturnsProperResults()
        {
            var newLayoutTemplate = await GenerateLayoutTemplate();
            var newStandardTemplate = await GenerateStandardTemplate(newLayoutTemplate.Alias);

            var result = await _client.GetTemplatesAsync();
            Assert.Equal(2, result.TotalCount);

            var standardTemplateFromResult = result.Templates.First(t => t.TemplateId == newStandardTemplate.TemplateId);
            Assert.Equal(newStandardTemplate.TemplateId, standardTemplateFromResult.TemplateId);
            Assert.Equal(newStandardTemplate.Alias, standardTemplateFromResult.Alias);
            Assert.Equal(newStandardTemplate.Name, standardTemplateFromResult.Name);
            Assert.Equal(TemplateType.Standard, standardTemplateFromResult.TemplateType);
            Assert.Equal(newLayoutTemplate.Alias, standardTemplateFromResult.LayoutTemplate);

            var layoutTemplateFromResult = result.Templates.First(t => t.TemplateId == newLayoutTemplate.TemplateId);
            Assert.Equal(newLayoutTemplate.TemplateId, layoutTemplateFromResult.TemplateId);
            Assert.Equal(newLayoutTemplate.Alias, layoutTemplateFromResult.Alias);
            Assert.Equal(newLayoutTemplate.Name, layoutTemplateFromResult.Name);
            Assert.Equal(TemplateType.Layout, layoutTemplateFromResult.TemplateType);
            Assert.Null(layoutTemplateFromResult.LayoutTemplate);

            var filteredResultByType = await _client.GetTemplatesAsync(0, 100, TemplateTypeFilter.Layout);
            Assert.Equal(1, filteredResultByType.TotalCount);

            var filteredResultByLayoutAlias = await _client.GetTemplatesAsync(0, 100, TemplateTypeFilter.All, newLayoutTemplate.Alias);
            Assert.Equal(1, filteredResultByLayoutAlias.TotalCount);
        }

        [Fact]
        public async void ClientCanValidateTemplate()
        {
            var result = await _client.ValidateTemplateAsync("{{name}}", "<html><body>{{content}}{{company.address}}{{#each products}}{{/each}}{{^competitors}}There are no substitutes.{{/competitors}}</body></html>", "{{content}}", new { name = "Johnny", content = "hello, world!" });

            Assert.True(result.AllContentIsValid);
            Assert.True(result.HtmlBody.ContentIsValid);
            Assert.True(result.TextBody.ContentIsValid);
            Assert.True(result.Subject.ContentIsValid);
            var inferredAddress = result.SuggestedTemplateModel.company.address;
            var products = result.SuggestedTemplateModel.products;
            Assert.NotNull(inferredAddress);
            Assert.Equal(3, products.Length);
        }

        [Fact]
        public async void ClientCanUseLayoutTemplatesWhenValidating()
        {
            var layoutTemplate = await GenerateLayoutTemplate();

            var content = "Mr. Jones";
            var result = await _client.ValidateTemplateAsync("Subject", null, content, new { }, true, TemplateType.Standard, layoutTemplate.Alias);

            Assert.True(result.AllContentIsValid);
            Assert.True(result.TextBody.ContentIsValid);
            Assert.True(result.Subject.ContentIsValid);

            // Testing to see that indeed the validation has used the LayoutTemplate
            var expectedTextBody = layoutTemplate.TextBody.Replace(_layoutContentPlaceholder, content);
            Assert.Equal(expectedTextBody, result.TextBody.RenderedContent);
        }

        [Fact]
        public async void ClientCanSendWithTemplate()
        {
            var template = await _client.CreateTemplateAsync("test template name", "test subject", "test html body");
            var sendResult = await _client.SendEmailWithTemplateAsync(template.TemplateId, new { name = "Andrew" }, WRITE_TEST_SENDER_EMAIL_ADDRESS, WRITE_TEST_SENDER_EMAIL_ADDRESS, false);
            Assert.NotEqual(Guid.Empty, sendResult.MessageID);
        }

        [Fact]
        public async void ClientCanSendTemplateWithStringModel()
        {
            var template = await _client.CreateTemplateAsync("test template name", "test subject", "test html body");
            var sendResult = await _client.SendEmailWithTemplateAsync(template.TemplateId, "{ \"name\" : \"Andrew\" }", WRITE_TEST_SENDER_EMAIL_ADDRESS, WRITE_TEST_SENDER_EMAIL_ADDRESS, false);
            Assert.NotEqual(Guid.Empty, sendResult.MessageID);
        }

        [Fact]
        public async void ClientCanSendBatchWithTemplates()
        {
            var template = await _client.CreateTemplateAsync("test template name", "Test Message - #{{testKey}}", "test html body");

            var messages = Enumerable.Range(0, 10)
                .Select(k => BuildTemplatedMessage(template.TemplateId, k.ToString())).ToArray();

            var results = (await _client.SendEmailsWithTemplateAsync(messages)).ToList();

            Assert.True(results.All(k => k.ErrorCode == 0));
            Assert.True(results.All(k => k.Status == PostmarkStatus.Success));
            Assert.Equal(messages.Length, results.Count());
        }

        private TemplatedPostmarkMessage BuildTemplatedMessage(long templateId, string testValue)
        {
            var message = new TemplatedPostmarkMessage
            {
                TemplateId = templateId,
                TemplateModel = new { testKey = $"{testValue}" },
                From = WRITE_TEST_SENDER_EMAIL_ADDRESS,
                To = WRITE_TEST_SENDER_EMAIL_ADDRESS,
                Headers = new HeaderCollection
                {
                    new MailHeader( "X-Integration-Testing-Postmark-Type-Message" , TESTING_DATE.ToString("o"))
                },
                Metadata = new Dictionary<string, string> { { "test-key", "test-value" }, { "client-id", "42" } },
                Tag = "integration-testing"
            };

            var content = "{ \"name\" : \"data\", \"age\" : null }";

            message.Attachments.Add(new PostmarkMessageAttachment
            {
                ContentType = "application/json",
                Name = "test.json",
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content))
            });
            return message;
        }

        private Task Cleanup()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var tasks = new List<Task>();
                    var templates = await _client.GetTemplatesAsync();

                    foreach (var t in templates.Templates)
                    {
                        tasks.Add(_client.DeleteTemplateAsync(t.TemplateId));
                    }
                    await Task.WhenAll(tasks);
                }
                catch { }
            });
        }

        private async Task<PostmarkTemplate> GenerateLayoutTemplate()
        {
            var layoutName = Guid.NewGuid().ToString();
            var layoutHtmlBody = $"<b>header</b> {_layoutContentPlaceholder} <b>footer</b>";
            var layoutTextBody = $"header {_layoutContentPlaceholder} footer";

            var newLayoutTemplate = await _client.CreateTemplateAsync(layoutName, null, layoutHtmlBody, layoutTextBody, null, TemplateType.Layout);
            return await _client.GetTemplateAsync(newLayoutTemplate.TemplateId);
        }

        private async Task<PostmarkTemplate> GenerateStandardTemplate(string layoutTemplateAlias = null)
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlBody = "<b>Hello, {{name}}!</b>";
            var textBody = "Hello, {{name}}!";

            var newStandardTemplate = await _client.CreateTemplateAsync(name, subject, htmlBody, textBody, null,
                TemplateType.Standard, layoutTemplateAlias);

            return await _client.GetTemplateAsync(newStandardTemplate.TemplateId);
        }

        public void Dispose()
        {
            Cleanup().Wait();
        }
    }
}
