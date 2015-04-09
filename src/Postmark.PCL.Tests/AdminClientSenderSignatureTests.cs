using NUnit.Framework;
using PostmarkDotNet;
using PostmarkDotNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Postmark.PCL.Tests
{
    [TestFixture]
    public class AdminClientSenderSignatureTests : ClientBaseFixture
    {
        private PostmarkAdminClient _adminClient;
        private string _senderEmail;
        private string _replyToAddress;
        private string _senderName;
        private string _senderprefix;

        protected override async Task SetupAsync()
        {
            _adminClient = new PostmarkAdminClient(WRITE_ACCOUNT_TOKEN);
            var id = Guid.NewGuid();
            var baseEmail =
            _senderprefix = "test-sender-";

            _senderEmail = WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE.Replace("[token]", String.Format(_senderprefix + "{0:n}", id));
            _replyToAddress = WRITE_TEST_SENDER_SIGNATURE_PROTOTYPE.Replace("[token]", String.Format(_senderprefix + "replyto-{0:n}@example.com", id));
            _senderName = String.Format("Test Sender {0}", TESTING_DATE);
            await CompletionSource;
        }


        [TestFixtureTearDown]
        [TestFixtureSetUp]
        public void Cleanup()
        {
            try
            {
                var signatures = _adminClient.GetSenderSignaturesAsync().WaitForResult();
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
            catch
            {
                //don't fail the test run if deleting all these wasn't possible.
            }
        }

        [TestCase]
        public async void AdminClient_CanGetSenderSignature()
        {
            var sigs = await _adminClient.GetSenderSignaturesAsync();
            var retrievedSignature = await _adminClient.GetSenderSignatureAsync(sigs.SenderSignatures.First().ID);

            Assert.IsNotNull(retrievedSignature);
            Assert.AreEqual(retrievedSignature.ID, sigs.SenderSignatures.First().ID);
        }


        [TestCase]
        public async void AdminClient_CanCreateSenderSignature()
        {
            var signature = await _adminClient
                .CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);

            Assert.NotNull(signature);
            Assert.AreEqual(_senderEmail, signature.EmailAddress);
            Assert.AreEqual(_senderName, signature.Name);
            Assert.AreEqual(_replyToAddress, signature.ReplyToEmailAddress);
        }


        [TestCase]
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
                Assert.AreEqual(PostmarkStatus.UserError, ex.Response.Status);
            }

            Assert.True(threwExpectedException);
        }


        [TestCase]
        public async void AdminClient_CanDeleteSenderSignature()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);

            var response = await _adminClient.DeleteSignatureAsync(signature.ID);
            Assert.AreEqual(PostmarkStatus.Success, response.Status);
            Assert.AreEqual(0, response.ErrorCode);
        }


        [TestCase]
        public async void AdminClient_CanListSenderSignatures()
        {
            var signatures = await _adminClient.GetSenderSignaturesAsync();
            Assert.Greater(signatures.SenderSignatures.Count(), 0);
        }

        [TestCase]
        public async void AdminClient_CanUpdateSenderSignatures()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);

            var prefix = "updated-";

            var updateResult = await _adminClient.UpdateSignatureAsync(signature.ID,
            prefix + signature.Name, prefix + _replyToAddress);

            var updatedSignature = await _adminClient.GetSenderSignatureAsync(signature.ID);

            Assert.AreEqual(updateResult.Name, updatedSignature.Name);
            Assert.AreEqual(updateResult.ReplyToEmailAddress, updatedSignature.ReplyToEmailAddress);

            Assert.AreEqual(prefix + signature.Name, updatedSignature.Name);
            Assert.AreEqual(prefix + signature.ReplyToEmailAddress, updatedSignature.ReplyToEmailAddress);
        }

        [TestCase]
        public async void AdminClient_ResendSignatureVerificationEmailAsync()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);
            var response = await _adminClient.ResendSignatureVerificationEmailAsync(signature.ID);
            Assert.AreEqual(PostmarkStatus.Success, response.Status);
            Assert.AreEqual(0, response.ErrorCode);
        }

        [TestCase]
        [Ignore("DKIM renewal cannot be triggered frequently.")]
        public async void AdminClient_CanRequestNewDKIM()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);
            var response = await _adminClient.RequestNewSignatureDKIMAsync(signature.ID);
            Assert.AreEqual(PostmarkStatus.Success, response.Status);
            //TODO: Running this too soon will generate a 505 status code.. hmm. How to test?
            //Assert.AreEqual(0, response.ErrorCode);
        }

        [TestCase]
        public async void AdminClient_CanVerifySPF()
        {
            var signature = await _adminClient.CreateSignatureAsync(_senderEmail, _senderName, _replyToAddress);
            var response = await _adminClient.VerifySignatureSPF(signature.ID);
            Assert.NotNull(response);
        }


    }
}
