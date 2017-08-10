using Xunit;
using PostmarkDotNet;
using PostmarkDotNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Postmark.Tests
{
    public class AdminClientSenderSignatureTests : ClientBaseFixture
    {
        private PostmarkAdminClient _adminClient;
        private string _senderEmail;
        private string _replyToAddress;
        private string _senderName;
        private string _senderprefix;
        private string _returnPath;

        protected override void Setup()
        {
            _adminClient = new PostmarkAdminClient(WRITE_ACCOUNT_TOKEN);
            var id = Guid.NewGuid();
            _senderprefix = "test-sender-";
            _returnPath = "test." + WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE.Split('@')[1];
            _senderEmail = WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE.Replace("[TOKEN]", String.Format(_senderprefix + "{0:n}", id));
            _replyToAddress = WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE.Replace("[TOKEN]", String.Format(_senderprefix + "replyto-{0:n}", id));
            _senderName = String.Format("Test Sender {0}", TESTING_DATE);
        }


        public AdminClientSenderSignatureTests():base()
        {
            this.Cleanup().Wait();
        }

        private Task Cleanup(){
            return Task.Run(async () =>
            {
                try
                {
                    var signatures = await _adminClient.GetSenderSignaturesAsync();
                    var pendingDeletes = new List<Task>();
                    foreach (var f in signatures.SenderSignatures)
                    {
                        if (Regex.IsMatch(f.EmailAddress, _senderprefix))
                        {
                            var deleteTask = _adminClient.DeleteSignatureAsync(f.ID);
                            pendingDeletes.Add(deleteTask);
                        }
                    }
                    Task.WaitAll(pendingDeletes.ToArray());
                }
                catch{}
            });
        }

        [Fact]
        public async void AdminClient_CanGetSenderSignature()
        {
            var sigs = await _adminClient.GetSenderSignaturesAsync();
            var retrievedSignature = await _adminClient.GetSenderSignatureAsync(sigs.SenderSignatures.First().ID);

            Assert.NotNull(retrievedSignature);
            Assert.Equal(retrievedSignature.ID, sigs.SenderSignatures.First().ID);
        }


        [Fact]
        public async void AdminClient_CanCreateSenderSignature()
        {
            var signature = await _adminClient
                .CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);

            Assert.NotNull(signature);
            Assert.Equal(_senderEmail, signature.EmailAddress);
            Assert.Equal(_senderName, signature.Name);
            Assert.Equal(_replyToAddress, signature.ReplyToEmailAddress);
        }


        [Fact]
        public async void AdminClient_ShouldProduceErrorStatusForInvalidSenderSignature()
        {
            var threwExpectedException = false;
            try
            {
                await _adminClient.CreateSignatureAsync(Guid.NewGuid().ToString("n") + "@example.com",
                            _senderName, _replyToAddress);
            }
            catch (PostmarkValidationException ex)
            {
                threwExpectedException = true;
                Assert.Equal(PostmarkStatus.UserError, ex.Response.Status);
            }

            Assert.True(threwExpectedException);
        }


        [Fact]
        public async void AdminClient_CanDeleteSenderSignature()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);

            var response = await _adminClient.DeleteSignatureAsync(signature.ID);
            Assert.Equal(PostmarkStatus.Success, response.Status);
            Assert.Equal(0, response.ErrorCode);
        }


        [Fact]
        public async void AdminClient_CanListSenderSignatures()
        {
            var signatures = await _adminClient.GetSenderSignaturesAsync();
            Assert.True(signatures.SenderSignatures.Count() > 0);
        }

        [Fact]
        public async void AdminClient_CanUpdateSenderSignatures()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);

            var prefix = "updated-";

            var updateResult = await _adminClient.UpdateSignatureAsync(signature.ID,
            prefix + signature.Name, prefix + _replyToAddress);

            var updatedSignature = await _adminClient.GetSenderSignatureAsync(signature.ID);

            Assert.Equal(updateResult.Name, updatedSignature.Name);
            Assert.Equal(updateResult.ReplyToEmailAddress, updatedSignature.ReplyToEmailAddress);

            Assert.Equal(prefix + signature.Name, updatedSignature.Name);
            Assert.Equal(prefix + signature.ReplyToEmailAddress, updatedSignature.ReplyToEmailAddress);
        }

        [Fact]
        public async void AdminClient_CanUpdateSenderSignatureReturnPath()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress, _returnPath);

            var prefix = "updated-";

            var updateResult = await _adminClient.UpdateSignatureAsync(signature.ID, returnPathDomain: prefix + _returnPath);

            var updatedSignature = await _adminClient.GetSenderSignatureAsync(signature.ID);

            Assert.Equal(updateResult.ReturnPathDomain, updatedSignature.ReturnPathDomain);
        }


        [Fact]
        public async void AdminClient_CanCreateSignatureWithReturnPath()
        {

            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress, _returnPath);
            Assert.Equal(_returnPath, signature.ReturnPathDomain);
        }

        [Fact]
        public async void AdminClient_ResendSignatureVerificationEmailAsync()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);
            var response = await _adminClient.ResendSignatureVerificationEmailAsync(signature.ID);
            Assert.Equal(PostmarkStatus.Success, response.Status);
            Assert.Equal(0, response.ErrorCode);
        }

        [Fact(Skip="DKIM renewal cannot be triggered frequently.")]
        public async void AdminClient_CanRequestNewDKIM()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);
            var response = await _adminClient.RequestNewSignatureDKIMAsync(signature.ID);
            Assert.Equal(PostmarkStatus.Success, response.Status);
            //TODO: Running this too soon will generate a 505 status code.. hmm. How to test?
            //Assert.Equal(0, response.ErrorCode);
        }

        [Fact]
        public async void AdminClient_CanVerifySPF()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);
            var response = await _adminClient.VerifySignatureSPF(signature.ID);
            Assert.NotNull(response);
        }


    }
}
