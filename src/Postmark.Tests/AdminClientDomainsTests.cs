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
    public class AdminClientDomainsTests : ClientBaseFixture
    {
        private PostmarkAdminClient _adminClient;
        private string _domainName;
        private string _returnPath;

        protected override void Setup()
        {
            _adminClient = new PostmarkAdminClient(WRITE_ACCOUNT_TOKEN);
            var id = Guid.NewGuid();
            _domainName = "dotnet-lib-test.com";
            _returnPath = $"return-path.{_domainName}";
        }

        public AdminClientDomainsTests():base()
        {
            this.Cleanup().Wait();
        }

        private Task Cleanup(){
            return Task.Run(async () =>
            {
                try
                {
                    var domains = await _adminClient.GetDomainsAsync();
                    var pendingDeletes = new List<Task>();
                    foreach (var d in domains.Domains)
                    {
                        if (d.Name == _domainName)
                        {
                            var deleteTask = _adminClient.DeleteDomainAsync(d.ID);
                            pendingDeletes.Add(deleteTask);
                        }
                    }
                    Task.WaitAll(pendingDeletes.ToArray());
                }
                catch{}
            });
        }

        [Fact]
        public async void AdminClient_CanGetDomain()
        {
            var domains = await _adminClient.GetDomainsAsync();
            var retrievedDomain = await _adminClient.GetDomainAsync(domains.Domains.First().ID);

            Assert.NotNull(retrievedDomain);
            Assert.Equal(retrievedDomain.ID, retrievedDomain.ID);
        }


        [Fact]
        public async void AdminClient_CanCreateDomain()
        {
            var domain = await _adminClient.CreateDomainAsync(_domainName, _returnPath);

            Assert.NotNull(domain);
            Assert.Equal(_domainName, domain.Name);
            Assert.Equal(_returnPath, domain.ReturnPathDomain);
        }


        [Fact]
        public async void AdminClient_ShouldProduceErrorStatusForInvalidDomain()
        {
            var threwExpectedException = false;
            try
            {
                await _adminClient.CreateDomainAsync("thisisntadomain");
            }
            catch (PostmarkValidationException ex)
            {
                threwExpectedException = true;
                Assert.Equal(PostmarkStatus.UserError, ex.Response.Status);
            }

            Assert.True(threwExpectedException);
        }


        [Fact]
        public async void AdminClient_CanDeleteDomain()
        {
            var domain = await _adminClient.CreateDomainAsync(_domainName);

            var response = await _adminClient.DeleteDomainAsync(domain.ID);
            Assert.Equal(PostmarkStatus.Success, response.Status);
            Assert.Equal(0, response.ErrorCode);
        }


        [Fact]
        public async void AdminClient_CanListDomains()
        {
            var domains = await _adminClient.GetDomainsAsync();
            Assert.True(domains.Domains.Count() > 0);
        }

        [Fact]
        public async void AdminClient_CanUpdateDomains()
        {
            var domain = await _adminClient.CreateDomainAsync(_domainName, _returnPath);

            var prefix = "updated-";

            var updateResult = await _adminClient.UpdateDomainAsync(domain.ID,
            prefix + _returnPath);

            var updateddomain = await _adminClient.GetDomainAsync(domain.ID);

            Assert.Equal(updateResult.ReturnPathDomain, updateddomain.ReturnPathDomain);
            Assert.Equal(prefix + domain.ReturnPathDomain, updateddomain.ReturnPathDomain);
        }

        [Fact]
        public async void AdminClient_CanUpdateDomainReturnPath()
        {
            var domain = await _adminClient.CreateDomainAsync(_domainName, _returnPath);

            var prefix = "updated-";

            var updateResult = await _adminClient.UpdateDomainAsync(domain.ID, returnPathDomain: prefix + _returnPath);

            var updateddomain = await _adminClient.GetDomainAsync(domain.ID);

            Assert.Equal(updateResult.ReturnPathDomain, updateddomain.ReturnPathDomain);
        }


        [Fact]
        public async void AdminClient_CanCreatedomainWithReturnPath()
        {
            var domain = await _adminClient.CreateDomainAsync(_domainName, _returnPath);
            Assert.Equal(_returnPath, domain.ReturnPathDomain);
        }

        [Fact(Skip="DKIM renewal cannot be triggered frequently.")]
        public async void AdminClient_CanRequestNewDKIM()
        {
            var domain = await _adminClient.CreateDomainAsync(_domainName);
            var response = await _adminClient.RequestNewDomainDKIMAsync(domain.ID);
            Assert.Equal(PostmarkStatus.Success, response.Status);
            //TODO: Running this too soon will generate a 505 status code.. hmm. How to test?
            //Assert.Equal(0, response.ErrorCode);
        }
    }
}
