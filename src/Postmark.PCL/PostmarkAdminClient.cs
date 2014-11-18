
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
namespace PostmarkDotNet
{
    /// <summary>
    /// Client Supporting the Administrative APIs.
    /// </summary>
    public class PostmarkAdminClient : PostmarkClientBase
    {
        public PostmarkAdminClient(string accountToken)
        {
            _authToken = accountToken;
        }

        protected override string AuthHeaderName
        {
            get { return "X-Postmark-Account-Token"; }
        }


        /// <summary>
        /// Get a server with the associated serverId.
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<PostmarkServer> GetServerAsync(int serverId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkServer>("/servers/" + serverId);
        }


        /// <summary>
        /// Get a server with the associated serverId.
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<PostmarkMessage> DeleteServerAsync(int serverId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkMessage>("/servers/" + serverId, verb: HttpMethod.Delete);
        }

        /// <summary>
        /// Create a new Server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<PostmarkServer> CreateServerAsync(String name, ServerColors? color = null,
            bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null,
            string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null,
            bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null)
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

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkServer>("/servers/", HttpMethod.Post, body);
        }

        /// <summary>
        /// Update a Server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<PostmarkServer> EditServerAsync(int serverId, String name = null, ServerColors? color = null,
            bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null,
            string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null,
            bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null)
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

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkServer>("/servers/" + serverId, HttpMethod.Put, body);
        }

        public async Task<PostmarkSenderSignatureList> GetSenderSignaturesAsync(int offset = 0, int count = 100)
        {
            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;
            return await this.ProcessNoBodyRequestAsync<PostmarkSenderSignatureList>("/senders", parameters);
        }

        public async Task<PostmarkCompleteSenderSignature> GetSenderSignatureAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkCompleteSenderSignature>("/senders/" + signatureId);
        }

        public async Task<PostmarkResponse> GetDeleteSignatureAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/senders/" + signatureId, verb: HttpMethod.Delete);
        }

        public async Task<PostmarkResponse> GetResendSignatureAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/senders/" + signatureId + "/resend", verb: HttpMethod.Post);
        }

        public async Task<PostmarkResponse> GetRequestNewSignatureDKIMAsync(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>
                ("/senders/" + signatureId + "/requestnewdkim", verb: HttpMethod.Post);
        }

        public async Task<PostmarkCompleteSenderSignature> GetVerifySignatureSPF(int signatureId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkCompleteSenderSignature>
                ("/senders/" + signatureId + "/verifyspf", verb: HttpMethod.Post);
        }

        public async Task<PostmarkCompleteSenderSignature> CreateSignatureAsync(string fromEmail, string name, string replyToEmail = null)
        {
            var parameters = new Dictionary<object, string>();
            parameters["FromEmail"] = fromEmail;
            parameters["Name"] = name;
            parameters["ReplyToEmail"] = replyToEmail;

            return await this.ProcessRequestAsync<Dictionary<Object, string>, PostmarkCompleteSenderSignature>
               ("/senders/", HttpMethod.Post, parameters);
        }

        public async Task<PostmarkCompleteSenderSignature> UpdateSignatureAsync(int signatureId,
            string fromEmail = null, string name = null, string replyToEmail = null)
        {
            var parameters = new Dictionary<object, string>();
            parameters["FromEmail"] = fromEmail;
            parameters["Name"] = name;
            parameters["ReplyToEmail"] = replyToEmail;

            return await this.ProcessRequestAsync<Dictionary<Object, string>, PostmarkCompleteSenderSignature>
               ("/senders/" + signatureId, HttpMethod.Put, parameters);
        }
    }
}
