
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
        public async Task<PostmarkServer> GetServer(int serverId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkServer>("/servers/" + serverId);
        }


        /// <summary>
        /// Get a server with the associated serverId.
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<PostmarkMessage> DeleteServer(int serverId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkMessage>("/servers/" + serverId, verb: HttpMethod.Delete);
        }

        /// <summary>
        /// Create a new Server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<PostmarkServer> CreateServer(String name, ServerColors? color = null,
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
        public async Task<PostmarkServer> EditServer(int serverId, String name = null, ServerColors? color = null,
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

    }
}
