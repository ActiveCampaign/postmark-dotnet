using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostmarkDotNet
{
    /// <summary>
    /// Postmark Client that supports access to the Administrative APIs,
    /// to send email, use the PostmarkClient, instead.
    /// </summary>
    ///<remarks>
    /// Make sure to include "using PostmarkDotNet;" in your class file, which will include extension methods on the base client.
    ///</remarks>
    public class PostmarkAdminClient : PostmarkClientBase
    {
        /// <summary>
        /// Construct a PostmarkAdminClient.
        /// </summary>
        /// <param name="accountToken">The "accountToken" can be found by logging into your Postmark and navigating to https://postmarkapp.com/account/edit - Keep this token secret and safe.</param>
        /// <param name="apiBaseUri">Optionally override the base url to the API. For example, you may fallback to HTTP (non-SSL) if your app requires it, though, this is not recommended.</param>
        public PostmarkAdminClient(string accountToken, string apiBaseUri = "https://api.postmarkapp.com")
            : base(apiBaseUri)
        {
            _authToken = accountToken;
        }

        /// <summary>
        /// The authentication header required for Admin API interactions, in this case: "X-Postmark-Account-Token"
        /// </summary>
        protected override string AuthHeaderName => "X-Postmark-Account-Token";

        /// <summary>
        /// Get a server with the associated serverId.
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<PostmarkServer> GetServerAsync(int serverId)
        {
            var retval = await this.ProcessNoBodyRequestAsync<PostmarkServer>("/servers/" + serverId);
            //the API doesn't return the server ID here, which would be helpful.
            retval.ID = serverId;
            return retval;
        }


        /// <summary>
        /// Get a server with the associated serverId.
        /// </summary>
        /// <param name="serverId"></param>
        /// <remarks>To protected your account, you must first request access to use this endpont from support@postmarkapp.com</remarks>
        /// <returns></returns>
        public async Task<PostmarkResponse> DeleteServerAsync(int serverId)
        {
            // Adding a retry mechanism because server deletion currently fails intermittently due to deadlocks.
            // This is a temporary fix, and should be removed once the server delete operation is more reliable.
            var attemptsRemain = 5;
            while (true)
            {
                try
                {
                    return await this.ProcessNoBodyRequestAsync<PostmarkResponse>("/servers/" + serverId, verb: HttpMethod.Delete);
                }
                catch
                {
                    if (--attemptsRemain == 0)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Create a new Server.
        /// </summary>
        /// <returns></returns>
        public async Task<PostmarkServer> CreateServerAsync(String name, string color = null,
            bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null,
            string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null,
            bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null,
            LinkTrackingOptions? trackLinks = null, string clickHookUrl = null, string deliveryHookUrl = null,
            bool? enableSmtpApiErrorHooks = null, string deliveryType = null)
        {

            var body = new Dictionary<string, object>();
            body["Name"] = name;
            body["Color"] = color;
            body["RawEmailEnabled"] = rawEmailEnabled;
            body["SmtpApiActivated"] = smtpApiActivated;
            body["InboundHookUrl"] = inboundHookUrl;
            body["BounceHookUrl"] = bounceHookUrl;
            body["OpenHookUrl"] = openHookUrl;
            body["PostFirstOpenOnly"] = postFirstOpenOnly;
            body["TrackOpens"] = trackOpens;
            body["InboundDomain"] = inboundDomain;
            body["InboundSpamThreshold"] = inboundSpamThreshold;
            body["TrackLinks"] = trackLinks;
            body["ClickHookUrl"] = clickHookUrl;
            body["DeliveryHookUrl"] = deliveryHookUrl;
            body["EnableSmtpApiErrorHooks"] = enableSmtpApiErrorHooks;
            body["DeliveryType"] = deliveryType;

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkServer>("/servers/", HttpMethod.Post, body);
        }

        /// <summary>
        /// Update a Server.
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="bounceHookUrl"></param>
        /// <param name="color"></param>
        /// <param name="inboundDomain"></param>
        /// <param name="inboundHookUrl"></param>
        /// <param name="inboundSpamThreshold"></param>
        /// <param name="name"></param>
        /// <param name="openHookUrl"></param>
        /// <param name="postFirstOpenOnly"></param>
        /// <param name="rawEmailEnabled"></param>
        /// <param name="smtpApiActivated"></param>
        /// <param name="trackOpens"></param>
        /// <param name="trackLinks"></param>
        /// <param name="clickHookUrl"></param>
        /// <param name="deliveryHookUrl"></param>
        /// <param name="enableSmtpApiErrorHooks"></param>
        /// <returns></returns>
        public async Task<PostmarkServer> EditServerAsync(int serverId, String name = null, string color = null,
            bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null,
            string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null,
            bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null, 
            LinkTrackingOptions? trackLinks = null, string clickHookUrl = null, string deliveryHookUrl = null, bool? enableSmtpApiErrorHooks = null)
        {

            var body = new Dictionary<string, object>();
            body["Name"] = name;
            body["Color"] = color;
            body["RawEmailEnabled"] = rawEmailEnabled;
            body["SmtpApiActivated"] = smtpApiActivated;
            body["InboundHookUrl"] = inboundHookUrl;
            body["BounceHookUrl"] = bounceHookUrl;
            body["OpenHookUrl"] = openHookUrl;
            body["PostFirstOpenOnly"] = postFirstOpenOnly;
            body["TrackOpens"] = trackOpens;
            body["InboundDomain"] = inboundDomain;
            body["InboundSpamThreshold"] = inboundSpamThreshold;
            body["TrackLinks"] = trackLinks;
            body["ClickHookUrl"] = clickHookUrl;
            body["DeliveryHookUrl"] = deliveryHookUrl;
            body["EnableSmtpApiErrorHooks"] = enableSmtpApiErrorHooks;

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkServer>
                ("/servers/" + serverId, HttpMethod.Put, body);
        }

        /// <summary>
        /// Get a specific sender signature.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PostmarkSenderSignatureList> GetSenderSignaturesAsync(int offset = 0, int count = 100)
        {
            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;
            return await this.ProcessNoBodyRequestAsync<PostmarkSenderSignatureList>("/senders", parameters);
        }


        /// <summary>
        /// Retrieve a sender signature.
        /// </summary>
        /// <param name="signatureId"></param>
        /// <returns></returns>
        public async Task<PostmarkCompleteSenderSignature> GetSenderSignatureAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkCompleteSenderSignature>("/senders/" + signatureId);
        }

        /// <summary>
        /// Delete a sender signature.
        /// </summary>
        /// <param name="signatureId"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> DeleteSignatureAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/senders/" + signatureId, verb: HttpMethod.Delete);
        }

        /// <summary>
        /// Cause a new sender signature verification email to be sent to the associated email address.
        /// </summary>
        /// <param name="signatureId"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> ResendSignatureVerificationEmailAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/senders/" + signatureId + "/resend", verb: HttpMethod.Post);
        }

        /// <summary>
        /// Creates a new DKIM key to be created. Until the DNS entries are confirmed, the new values will be in the DKIMPendingHost and DKIMPendingTextValue fields. After the new DKIM value is verified in DNS, the pending values will migrate to DKIMTextValue and DKIMPendingTextValue and Postmark will begin to sign emails with the new DKIM key.
        /// </summary>
        /// <param name="signatureId"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> RequestNewSignatureDKIMAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/senders/" + signatureId + "/requestnewdkim", verb: HttpMethod.Post);
        }

        /// <summary>
        /// Will query DNS for your domain and attempt to verify the SPF record contains the information for Postmark's servers.
        /// </summary>
        /// <param name="signatureId"></param>
        /// <returns></returns>
        [Obsolete("We no longer require SPF verification. See here for details: https://postmarkapp.com/blog/why-we-no-longer-ask-for-spf-records")]
        public async Task<PostmarkCompleteSenderSignature> VerifySignatureSPF(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkCompleteSenderSignature>
                ("/senders/" + signatureId + "/verifyspf", verb: HttpMethod.Post);
        }

        /// <summary>
        /// Create a new sender signature. Note that you'll need to
        /// verify this by clicking on the verification link sent to the associated email address.
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="name"></param>
        /// <param name="replyToEmail"></param>
        /// <param name="returnPathDomain"></param>
        /// <param name="confirmationPersonalNote"></param>
        /// <returns></returns>
        public async Task<PostmarkCompleteSenderSignature> CreateSignatureAsync(string fromEmail, string name, string replyToEmail = null, string returnPathDomain = null, string confirmationPersonalNote = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["FromEmail"] = fromEmail;
            parameters["Name"] = name;
            parameters["ReplyToEmail"] = replyToEmail;
            parameters["ReturnPathDomain"] = returnPathDomain;
            parameters["ConfirmationPersonalNote"] = confirmationPersonalNote;

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkCompleteSenderSignature>
               ("/senders/", HttpMethod.Post, parameters);
        }

        /// <summary>
        /// Modift an existing sender signature.
        /// </summary>
        /// <param name="signatureId"></param>
        /// <param name="name"></param>
        /// <param name="replyToEmail"></param>
        /// <param name="returnPathDomain"></param>
        /// <param name="confirmationPersonalNote"></param>
        /// <returns></returns>
        public async Task<PostmarkCompleteSenderSignature> UpdateSignatureAsync
            (int signatureId, string name = null, string replyToEmail = null, string returnPathDomain = null, string confirmationPersonalNote = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["Name"] = name;
            parameters["ReplyToEmail"] = replyToEmail;
            parameters["ReturnPathDomain"] = returnPathDomain;
            parameters["ConfirmationPersonalNote"] = confirmationPersonalNote;

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkCompleteSenderSignature>
               ("/senders/" + signatureId, HttpMethod.Put, parameters);
        }

        /// <summary>
        /// Get a list of domains.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PostmarkDomainList> GetDomainsAsync(int offset = 0, int count = 100)
        {
            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;
            return await this.ProcessNoBodyRequestAsync<PostmarkDomainList>("/domains", parameters);
        }


        /// <summary>
        /// Retrieve a domain.
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public async Task<PostmarkCompleteDomain> GetDomainAsync(int domainId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkCompleteDomain>($"/domains/{domainId}");
        }

        /// <summary>
        /// Delete a domain.
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> DeleteDomainAsync(int domainId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/domains/" + domainId, verb: HttpMethod.Delete);
        }

        /// <summary>
        /// Creates a new DKIM key to be created. Until the DNS entries are confirmed, the new values will be in the DKIMPendingHost and DKIMPendingTextValue fields. After the new DKIM value is verified in DNS, the pending values will migrate to DKIMTextValue and DKIMPendingTextValue and Postmark will begin to sign emails with the new DKIM key.
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> RequestNewDomainDKIMAsync(int domainId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/domains/" + domainId + "/rotatedkim", verb: HttpMethod.Post);
        }

        /// <summary>
        /// Create a new domain.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnPathDomain"></param>
        /// <returns></returns>
        public async Task<PostmarkCompleteDomain> CreateDomainAsync(string name, string returnPathDomain = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["Name"] = name;
            parameters["ReturnPathDomain"] = returnPathDomain;

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkCompleteDomain>
               ("/domains/", HttpMethod.Post, parameters);
        }

        /// <summary>
        /// Modify an existing domain.
        /// </summary>
        /// <param name="domainId"></param>
        /// <param name="name"></param>
        /// <param name="returnPathDomain">Setting this to null or empty will clear your Return-Path</param>
        /// <returns></returns>
        public async Task<PostmarkCompleteDomain> UpdateDomainAsync
            (int domainId, string returnPathDomain)
        {
            var parameters = new Dictionary<string, object>();
            parameters["ReturnPathDomain"] = returnPathDomain ?? "";

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkCompleteDomain>
               ($"/domains/{domainId}", HttpMethod.Put, parameters);
        }

        /// <summary>
        /// Verify DKIM record for a domain
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public async Task<PostmarkCompleteDomain> VerifyDomainDkim(int domainId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkCompleteDomain>
               ($"/domains/{domainId}/verifyreturnpath", verb: HttpMethod.Put);
        }

        /// <summary>
        /// Verify DKIM record for a domain
        /// </summary>
        /// <param name="domainId"></param>
        /// <returns></returns>
        public async Task<PostmarkCompleteDomain> VerifyDomainReturnPath(int domainId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkCompleteDomain>
               ($"/domains/{domainId}/verifydkim", verb: HttpMethod.Put);
        }


        /// <summary>
        /// List all servers that are currently configured for this account.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<PostmarkServerList> GetServersAsync(int offset = 0, int count = 100, string name = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;
            parameters["name"] = name;

            return await this.ProcessNoBodyRequestAsync<PostmarkServerList>("/servers", parameters);
        }

        /// <summary>
        /// Create a data removal request.
        /// </summary>
        /// <param name="requestedBy">The email address of the user that is making the request. This must belong to an account user in your Postmark account.</param>
        /// <param name="requestedFor">The email address of the recipient who's asking for their data to be removed. This must be a valid email address.</param>
        /// <param name="notifyWhenCompleted">Specifies whether the RequestedBy email address is notified when the data removal request is complete.</param>
        /// <returns></returns>
        public async Task<PostmarkDataRemoval> RequestDataRemoval(string requestedBy, string requestedFor, bool notifyWhenCompleted)
        {
            var parameters = new Dictionary<string, object>();
            parameters["RequestedBy"] = requestedBy;
            parameters["RequestedFor"] = requestedFor;
            parameters["NotifyWhenCompleted"] = notifyWhenCompleted;
            
            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkDataRemoval>("/data-removals", HttpMethod.Post, parameters);
        }

        /// <summary>
        /// Check a data removal request status.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PostmarkDataRemoval> GetDataRemovalStatus(long id)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkDataRemoval>($"/data-removals/{id}");
        }
    }
}