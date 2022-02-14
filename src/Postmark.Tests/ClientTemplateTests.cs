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
    public class ClientTemplateTests : ClientBaseFixture, IAsyncLifetime
    {
        private readonly string _layoutContentPlaceholder = "{{{@content}}}";

        public Task InitializeAsync()
        {
            Client = new PostmarkClient(WriteTestServerToken, BaseUrl);
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try
            {
                var tasks = new List<Task>();
                var templates = await Client.GetTemplatesAsync();

                foreach (var t in templates.Templates)
                {
                    tasks.Add(Client.DeleteTemplateAsync(t.TemplateId));
                }
                await Task.WhenAll(tasks);
            }
            catch { }
        }

        [Fact]
        public async void ClientCanCreateTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlBody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await Client.CreateTemplateAsync(name, subject, htmlBody, textBody);

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

            var newTemplate = await Client.CreateTemplateAsync(name, subject, htmlbody, textBody);

            var existingTemplate = await Client.GetTemplateAsync(newTemplate.TemplateId);

            await Client.EditTemplateAsync(existingTemplate.TemplateId, name + name, subject + subject, htmlbody + htmlbody, textBody + textBody);

            var updatedTemplate = await Client.GetTemplateAsync(existingTemplate.TemplateId);

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
            var templateWithNoLayoutTemplate = await Client.EditTemplateAsync(newStandardTemplate.TemplateId, layoutTemplate: "");
            Assert.Null(templateWithNoLayoutTemplate.LayoutTemplate);

            // Setting the LayoutTemplate back to the layout template that was created
            var templateWithLayoutTemplate = await Client.EditTemplateAsync(newStandardTemplate.TemplateId,
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

            var newTemplate = await Client.CreateTemplateAsync(name, subject, htmlbody, textBody);
            await Client.DeleteTemplateAsync(newTemplate.TemplateId);
            var deletedTemplate = await Client.GetTemplateAsync(newTemplate.TemplateId);

            Assert.False(deletedTemplate.Active);
        }

        [Fact]
        public async void ClientCanGetTemplate()
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlbody = "<b>Hello, {{name}}</b>";
            var textBody = "Hello, {{name}}!";

            var newTemplate = await Client.CreateTemplateAsync(name, subject, htmlbody, textBody);

            var result = await Client.GetTemplateAsync(newTemplate.TemplateId);

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
                await Client.CreateTemplateAsync("test " + i, "test subject" + i, "body");
            }

            var result = await Client.GetTemplatesAsync();
            Assert.Equal(10, result.TotalCount);
            var toDelete = result.Templates.First().TemplateId;
            await Client.DeleteTemplateAsync(toDelete);
            result = await Client.GetTemplatesAsync();
            Assert.Equal(9, result.TotalCount);
            Assert.False(result.Templates.FirstOrDefault(k => k.TemplateId == toDelete) != null);
            var offsetResults = await Client.GetTemplatesAsync(5);
            Assert.True(result.Templates.Skip(5).Select(k => k.TemplateId).SequenceEqual(offsetResults.Templates.Select(k => k.TemplateId)));
        }

        [Fact]
        public async void GetTemplatesReturnsProperResults()
        {
            var newLayoutTemplate = await GenerateLayoutTemplate();
            var newStandardTemplate = await GenerateStandardTemplate(newLayoutTemplate.Alias);

            var result = await Client.GetTemplatesAsync();
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

            var filteredResultByType = await Client.GetTemplatesAsync(0, 100, TemplateTypeFilter.Layout);
            Assert.Equal(1, filteredResultByType.TotalCount);

            var filteredResultByLayoutAlias = await Client.GetTemplatesAsync(0, 100, TemplateTypeFilter.All, newLayoutTemplate.Alias);
            Assert.Equal(1, filteredResultByLayoutAlias.TotalCount);
        }

        [Fact]
        public async void ClientCanValidateTemplate()
        {
            var result = await Client.ValidateTemplateAsync("{{name}}", "<html><body>{{content}}{{company.address}}{{#each products}}{{/each}}{{^competitors}}There are no substitutes.{{/competitors}}</body></html>", "{{content}}", new { name = "Johnny", content = "hello, world!" });

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
            var result = await Client.ValidateTemplateAsync("Subject", null, content, new { }, true, TemplateType.Standard, layoutTemplate.Alias);

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
            var template = await Client.CreateTemplateAsync("test template name", "test subject", "test html body");
            var sendResult = await Client.SendEmailWithTemplateAsync(template.TemplateId, new { name = "Andrew" }, WriteTestSenderEmailAddress, WriteTestSenderEmailAddress, false);
            Assert.NotEqual(Guid.Empty, sendResult.MessageID);
        }

        [Fact]
        public async void ClientCanSendTemplateWithStringModel()
        {
            var template = await Client.CreateTemplateAsync("test template name", "test subject", "test html body");
            var sendResult = await Client.SendEmailWithTemplateAsync(template.TemplateId, "{ \"name\" : \"Andrew\" }", WriteTestSenderEmailAddress, WriteTestSenderEmailAddress, false);
            Assert.NotEqual(Guid.Empty, sendResult.MessageID);
        }

        [Fact]
        public async void ClientCanSendBatchWithTemplates()
        {
            var template = await Client.CreateTemplateAsync("test template name", "Test Message - #{{testKey}}", "test html body");

            var messages = Enumerable.Range(0, 10)
                .Select(k => BuildTemplatedMessage(template.TemplateId, k.ToString())).ToArray();

            var results = (await Client.SendEmailsWithTemplateAsync(messages)).ToList();

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
                From = WriteTestSenderEmailAddress,
                To = WriteTestSenderEmailAddress,
                Headers = new HeaderCollection
                {
                    new MailHeader( "X-Integration-Testing-Postmark-Type-Message" , TestingDate.ToString("o"))
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

        private async Task<PostmarkTemplate> GenerateLayoutTemplate()
        {
            var layoutName = Guid.NewGuid().ToString();
            var layoutHtmlBody = $"<b>header</b> {_layoutContentPlaceholder} <b>footer</b>";
            var layoutTextBody = $"header {_layoutContentPlaceholder} footer";

            var newLayoutTemplate = await Client.CreateTemplateAsync(layoutName, null, layoutHtmlBody, layoutTextBody, null, TemplateType.Layout);
            return await Client.GetTemplateAsync(newLayoutTemplate.TemplateId);
        }

        private async Task<PostmarkTemplate> GenerateStandardTemplate(string layoutTemplateAlias = null)
        {
            var name = Guid.NewGuid().ToString();
            var subject = "A subject: " + Guid.NewGuid();
            var htmlBody = "<b>Hello, {{name}}!</b>";
            var textBody = "Hello, {{name}}!";

            var newStandardTemplate = await Client.CreateTemplateAsync(name, subject, htmlBody, textBody, null,
                TemplateType.Standard, layoutTemplateAlias);

            return await Client.GetTemplateAsync(newStandardTemplate.TemplateId);
        }
    }
}
