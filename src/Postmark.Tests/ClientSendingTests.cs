using Xunit;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postmark.Tests
{
    public class ClientSendingTests : ClientBaseFixture
    {
        protected override void Setup()
        {
            _client = new PostmarkClient(WRITE_TEST_SERVER_TOKEN);
        }

        //This is a bad test since it requires a Delay before the message has been saved so that we can retrieve details.
        //In other tests (for link and click tracking), we have test accounts that have plenty of messages to sample from
        //We would need to agree on a convetion for metadata and do something similar to avoid having to create and wait for data in this test
        [Fact]
        public async void Client_CanSendAndGetOutboundMessageDetailsWithMetadata()
        {
            var metadata = new Dictionary<string, string>() {
                    {"test-metadata", "value-goes-here"},
                    {"more-metadata", "more-goes-here"}
                };

            var sendResult = await _client.SendMessageAsync(
                WRITE_TEST_SENDER_EMAIL_ADDRESS,
                WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                $"Integration Test - {TESTING_DATE}",
                $"Plain text body, {TESTING_DATE}",
                $"Testing the Postmark .net client, <b>{TESTING_DATE}</b>",
                null,
                metadata
                );

            IDictionary<string, string> storedDetails = null;

            Func<Task> query = async()=>{
                try{
                    while(true){
                        var details = await _client
                            .GetOutboundMessageDetailsAsync(sendResult.MessageID.ToString());
                        storedDetails = details.Metadata;
                        if(storedDetails != null) return; 
                        await Task.Delay(TimeSpan.FromSeconds(2));
                    }
                }catch{

                }
                return;
            };

            //wait up to 30 seconds, checking every two seconds for the metadata.
            await Task.WhenAny(Task.Delay(TimeSpan.FromSeconds(30)), query());

            Assert.Equal(storedDetails, metadata);
        }

        [Fact]
        public async void Client_CanSendASingleMessage()
        {
            var result = await _client.SendMessageAsync(WRITE_TEST_SENDER_EMAIL_ADDRESS,
                WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                String.Format("Integration Test - {0}", TESTING_DATE),
                String.Format("Plain text body, {0}", TESTING_DATE),
                String.Format("Testing the Postmark .net client, <b>{0}</b>", TESTING_DATE),
                new Dictionary<string, string>()
                {
                  {  "X-Integration-Testing" , TESTING_DATE.ToString("o")}
                },
                new Dictionary<string, string>() {
                    {"test-metadata", "value-goes-here"},
                    {"more-metadata", "more-goes-here"}
                });

            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);
        }

        [Fact]
        public async void Client_CanSendAPostmarkMessage()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var message = ConstructMessage(inboundAddress);
            var result = await _client.SendMessageAsync(message);

            Assert.Equal(PostmarkStatus.Success, result.Status);
            Assert.Equal(0, result.ErrorCode);
            Assert.NotEqual(Guid.Empty, result.MessageID);
        }

        private PostmarkMessage ConstructMessage(string inboundAddress, int number = 0)
        {
            var message = new PostmarkMessage()
            {
                From = WRITE_TEST_SENDER_EMAIL_ADDRESS,
                To = WRITE_TEST_SENDER_EMAIL_ADDRESS,
                Cc = WRITE_TEST_EMAIL_RECIPIENT_ADDRESS,
                Bcc = "testing@example.com",
                Subject = String.Format("Integration Test - {0} - Message #{1}", TESTING_DATE, number),
                HtmlBody = String.Format("Testing the Postmark .net client, <b>{0}</b>", TESTING_DATE),
                TextBody = "This is plain text.",
                TrackOpens = true,
                TrackLinks = LinkTrackingOptions.HtmlAndText,
                Headers = new HeaderCollection(){
                  new MailHeader( "X-Integration-Testing-Postmark-Type-Message" , TESTING_DATE.ToString("o"))
                },
                Metadata = new Dictionary<string, string>() { { "something-interesting", "very-interesting" }, {"client-id", "42"} },
                ReplyTo = inboundAddress,
                Tag = "integration-testing"
            };

            var content = "{ \"name\" : \"data\", \"age\" : null }";

            message.Attachments.Add(new PostmarkMessageAttachment()
            {
                ContentType = "application/json",
                Name = "test.json",
                Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content))
            });
            return message;
        }

        [Fact]
        public async void Client_CanSendABatchOfMessages()
        {
            var inboundAddress = (await _client.GetServerAsync()).InboundAddress;
            var messages = Enumerable.Range(0, 10)
                .Select(k => ConstructMessage(inboundAddress, k)).ToArray();

            var results = await _client.SendMessagesAsync(messages);

            Assert.True(results.All(k => k.ErrorCode == 0));
            Assert.True(results.All(k => k.Status == PostmarkStatus.Success));
            Assert.Equal(messages.Length, results.Count());
        }
    }
}
