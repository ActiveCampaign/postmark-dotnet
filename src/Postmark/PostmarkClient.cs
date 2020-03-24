using Newtonsoft.Json.Linq;
using PostmarkDotNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PostmarkDotNet.Model.Webhooks;

namespace PostmarkDotNet
{
    /// <summary>
    /// The standard Postmark API, allows all interactions with a Postmark "Server" (send/recieve/process/analyze emails).
    /// This client supports normal API interactions, for Administrative API interactions, use the PostmarkAdminClient.
    /// </summary>
    /// <remarks>
    /// Make sure to include "using PostmarkDotNet;" in your class file, which will include extension methods on the base client.
    /// </remarks>
    public class PostmarkClient : PostmarkClientBase
    {
        /// <summary>
        /// The authorization header required, in this case, "X-Postmark-Server-Token"
        /// </summary>
        protected override string AuthHeaderName
        {
            get { return "X-Postmark-Server-Token"; }
        }

        /// <summary>
        /// Instantiate the client.
        /// </summary>
        /// <param name="apiBaseUri">The base uri to use when connecting to Postmark. You should rarely need to modify this, except if you want to disable TLS (not recommended), or you are using a proxy of some sort to connect to the API.</param>
        /// <param name="serverToken">Used for requests that require server level privileges. This token can be found on the Credentials tab under your Postmark server.</param>
        public PostmarkClient(string serverToken, string apiBaseUri = "https://api.postmarkapp.com")
            : base(apiBaseUri)
        {
            _authToken = serverToken;
        }

        #region Email Sending
        /// <summary>
        /// Sends a message through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="message">A prepared message.</param>
        /// <returns></returns>
        public async Task<PostmarkResponse> SendMessageAsync(PostmarkMessage message)
        {
            return await ProcessRequestAsync<PostmarkMessage, PostmarkResponse>("/email", HttpMethod.Post, message);
        }

        /// <summary>
        /// Sends a batch of up to 500 messages through the Postmark API.
        /// All email addresses must be valid, and the sender must be
        /// a valid sender signature according to Postmark. To obtain a valid
        /// sender signature, log in to Postmark and navigate to:
        /// http://postmarkapp.com/signatures.
        /// </summary>
        /// <param name="messages">A prepared batch of messages.</param>
        /// <returns>The processed messages (Complete with system assigned message IDs)</returns>
        public async Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(params PostmarkMessage[] messages)
        {
            return await ProcessRequestAsync<PostmarkMessage[], PostmarkResponse[]>("/email/batch", HttpMethod.Post, messages);
        }
        #endregion

        #region Bounces

        /// <summary>
        /// Retrieves the bounce-related <see cref = "PostmarkDeliveryStats" /> results for the
        /// associated mail server.
        /// </summary>
        /// <returns></returns>
        public async Task<PostmarkDeliveryStats> GetDeliveryStatsAsync()
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkDeliveryStats>("/deliverystats");
        }

        /// <summary>
        /// Retrieves a collection of <see cref = "PostmarkBounce" /> instances along
        /// with a sum total of bounces recorded by the server, based on filter parameters.
        /// </summary>
        /// <param name="type">The type of bounces to filter on.</param>
        /// <param name="inactive">Whether to return only inactive or active bounces; use null to return all bounces.</param>
        /// <param name="emailFilter">Filters based on whether the filter value is contained in the bounce source's email.</param>
        /// <param name="tag">Filters on the bounce tag.</param>
        /// <param name="messageID">Filter by MessageID.</param>
        /// <param name="offset">The page offset for the returned results; defaults to 0.</param>
        /// <param name="count">The number of results to return by the page offset; defaults to 100.</param>
        /// <param name="fromDate">Filter messages starting from the date specified (inclusive). e.g. 2014-02-01</param>
        /// <param name="toDate">Filter messages up to the date specified (inclusive). e.g. 2014-02-01</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounces> GetBouncesAsync(int offset = 0, int count = 100, PostmarkBounceType? type = null,
            bool? inactive = null, string emailFilter = null, string tag = null, string messageID = null, string fromDate = null, string toDate = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["type"] = type;
            parameters["inactive"] = inactive;
            parameters["emailFilter"] = emailFilter;
            parameters["tag"] = tag;
            parameters["messageID"] = messageID;
            parameters["offset"] = offset;
            parameters["count"] = count;
            parameters["fromDate"] = fromDate;
            parameters["toDate"] = toDate;

            return await ProcessNoBodyRequestAsync<PostmarkBounces>("/bounces", parameters);
        }

        /// <summary>
        /// Retrieves a single <see cref = "PostmarkBounce" /> based on a specified ID.
        /// </summary>
        /// <param name="bounceId">The bounce ID of the bounce to retrieve.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounce> GetBounceAsync(int bounceId)
        {
            return await ProcessNoBodyRequestAsync<PostmarkBounce>("/bounces/" + bounceId);
        }

        /// <summary>
        /// Returns the raw source of the bounce we accepted. 
        /// If Postmark does not have a dump for that bounce, it will return an empty string.
        /// </summary>
        /// <param name="bounceId">The bounce ID of the bounce dump to retrieve.</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounceDump> GetBounceDumpAsync(int bounceId)
        {
            return await ProcessNoBodyRequestAsync<PostmarkBounceDump>("/bounces/" + bounceId + "/dump");
        }

        /// <summary>
        /// Activates a deactivated bounce.
        /// </summary>
        /// <param name="bounceId">The bounce ID of the bounce to Activate</param>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<PostmarkBounceActivation> ActivateBounceAsync(int bounceId)
        {
            return await ProcessNoBodyRequestAsync<PostmarkBounceActivation>("/bounces/" + bounceId + "/activate", verb: HttpMethod.Put);
        }

        /// <summary>
        /// Returns a list of tags used for the current server.
        /// </summary>
        /// <returns></returns>
        /// <seealso href = "http://developer.postmarkapp.com/bounces" />
        public async Task<IEnumerable<string>> GetBounceTagsAsync()
        {
            return await ProcessNoBodyRequestAsync<String[]>("/bounces/tags");
        }

        #endregion

        #region Outbound Message Retrieval

        /// <summary>
        /// Return a listing of Outbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="tag">Filter by a tag used for the message (messages sent directly through the API only)</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="status">The status of the outbound message.</param>
        /// <param name="fromDate">Get messages on or after YYYY-MM-DD.</param>
        /// <param name="toDate">Get messages on or before YYYY-MM-DD.</param>
        /// <returns>PostmarkOutboundMessageList</returns>
        public async Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int offset = 0, int count = 100,
            string recipient = null, string fromemail = null, string tag = null, string subject = null,
            OutboundMessageStatus status = OutboundMessageStatus.Sent, string toDate = null, string fromDate = null, IDictionary<string, string> metadata = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["count"] = count;
            parameters["offset"] = offset;
            parameters["recipient"] = recipient;
            parameters["fromemail"] = fromemail;
            parameters["tag"] = tag;
            parameters["subject"] = subject;
            parameters["todate"] = toDate;
            parameters["fromdate"] = fromDate;
            parameters["status"] = status.ToString().ToLower();

            if (metadata != null)
            {
                foreach (var a in metadata)
                {
                    parameters[$"metadata_{a.Key}"] = a.Value;
                }
            }

            return await ProcessNoBodyRequestAsync<PostmarkOutboundMessageList>("/messages/outbound", parameters);
        }

        /// <summary>
        /// Get the full details of a sent message including all fields, raw body, attachment names, etc
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>OutboundMessageDetail</returns>
        public async Task<OutboundMessageDetail> GetOutboundMessageDetailsAsync(string messageID)
        {
            return await ProcessNoBodyRequestAsync<OutboundMessageDetail>("/messages/outbound/" + messageID + "/details");
        }

        /// <summary>
        /// Get the original raw message dump of on outbound message including all SMTP headers and data.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetOutboundMessages call.</param>
        /// <returns>MessageDump</returns>
        public async Task<MessageDump> GetOutboundMessageDumpAsync(string messageID)
        {
            return await ProcessNoBodyRequestAsync<MessageDump>("/messages/outbound/" + messageID + "/dump");
        }

        #endregion

        #region Inbound Message Retrieval
        /// <summary>
        /// Return a listing of Inbound sent messages using the filters supported by the API.
        /// </summary>
        /// <param name="recipient">Filter by the recipient(s) of the message.</param>
        /// <param name="fromemail">Filter by the email address the message is sent from.</param>
        /// <param name="subject">Filter by message subject.</param>
        /// <param name="mailboxhash">Filter by mailbox hash that was parsed from the inbound message.</param>
        /// <param name="count">Number of messages to return per call. (required)</param>
        /// <param name="offset">Number of messages to offset/page per call. (required)</param>
        /// <param name="status">The status of the inbound message.</param>
        /// <param name="fromDate">Get messages on or after YYYY-MM-DD.</param>
        /// <param name="toDate">Get messages on or before YYYY-MM-DD.</param>
        /// <returns>PostmarkInboundMessageList</returns>
        public async Task<PostmarkInboundMessageList> GetInboundMessagesAsync(int offset = 0, int count = 100,
            string recipient = null, string fromemail = null, string subject = null,
            string mailboxhash = null, InboundMessageStatus? status = InboundMessageStatus.Processed, String toDate = null, String fromDate = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["count"] = count;
            parameters["offset"] = offset;
            parameters["recipient"] = recipient;
            parameters["fromemail"] = fromemail;
            parameters["subject"] = subject;
            parameters["mailboxhash"] = mailboxhash;
            parameters["todate"] = toDate;
            parameters["fromdate"] = fromDate;
            parameters["status"] = status.ToString().ToLower();

            return await ProcessNoBodyRequestAsync<PostmarkInboundMessageList>("/messages/inbound", parameters);
        }

        /// <summary>
        /// Get the full details of a processed inbound message including all fields, attachment names, etc.
        /// </summary>
        /// <param name="messageID">The MessageID of a message which can be optained either from the initial API send call or a GetInboundMessages call.</param>
        /// <returns>InboundMessageDetail</returns>
        public async Task<InboundMessageDetail> GetInboundMessageDetailsAsync(string messageID)
        {
            return await ProcessNoBodyRequestAsync<InboundMessageDetail>("/messages/inbound/" + messageID + "/details");
        }

        /// <summary>
        /// Bypass rules for a blocked inbound message.
        /// </summary>
        /// <param name="messageid"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> BypassBlockedInboundMessage(string messageid)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>(String.Format("/messages/inbound/{0}/bypass", messageid), verb: HttpMethod.Put);
        }

        /// <summary>
        /// Request that Postmark retries POSTing to your Inbound Hook for the specified inbound message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> RetryInboundHookForMessage(string messageId)
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkResponse>(String.Format("/messages/inbound/{0}/retry", messageId), verb: HttpMethod.Put);
        }


        #endregion

        #region Servers

        /// <summary>
        /// Gets the server associated with this client based on 
        /// the ServerToken supplied when the client was constructed.
        /// </summary>
        /// <returns></returns>
        public async Task<PostmarkServer> GetServerAsync()
        {
            return await this.ProcessNoBodyRequestAsync<PostmarkServer>("/server");
        }

        /// <summary>
        /// Updates the server associated with this client. Only parameters that are passed into this method are modified. 
        /// Any parameters that are left null will use the current value for the server.
        /// </summary>
        /// <returns></returns>
        public async Task<PostmarkServer> EditServerAsync(String name = null, string color = null,
            bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null,
            string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null,
            bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null,
            LinkTrackingOptions? trackLinks = null,
            string clickHookUrl = null, string deliveryHookUrl = null)
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

            body = body.Where(kv => kv.Value != null).ToDictionary(k => k.Key, v => v.Value);

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkServer>("/server", HttpMethod.Put, body);
        }

        #endregion

        #region Stats

        /// <summary>
        /// Create parameters for the stats filtering from normal params.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        private IDictionary<string, object>
           ConstructSentStatsFilter(string tag, DateTime? fromDate, DateTime? toDate)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(tag))
            {
                parameters["tag"] = tag;
            }
            if (fromDate.HasValue)
            {
                parameters["fromdate"] = fromDate.Value.ToString(DATE_FORMAT);
            }
            if (toDate.HasValue)
            {
                parameters["todate"] = toDate.Value.ToString(DATE_FORMAT);
            }
            return parameters;
        }

        /// <summary>
        /// Get the Open Events for messages, optionally filtering by various
        /// attributes of the Open Events and Messages.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="recipient"></param>
        /// <param name="tag"></param>
        /// <param name="clientName"></param>
        /// <param name="clientCompany"></param>
        /// <param name="clientFamily"></param>
        /// <param name="operatingSystemName"></param>
        /// <param name="operatingSystemFamily"></param>
        /// <param name="operatingSystemCompany"></param>
        /// <param name="platform"></param>
        /// <param name="country"></param>
        /// <param name="region"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<PostmarkOpensList> GetOpenEventsForMessagesAsync(
            int offset = 0, int count = 100, string recipient = null, string tag = null,
            string clientName = null, string clientCompany = null, string clientFamily = null,
            string operatingSystemName = null, string operatingSystemFamily = null, string operatingSystemCompany = null,
            string platform = null, string country = null, string region = null, string city = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;
            parameters["recipient"] = recipient;
            parameters["tag"] = tag;
            parameters["client_name"] = clientName;
            parameters["client_company"] = clientCompany;
            parameters["client_family"] = clientFamily;
            parameters["os_name"] = operatingSystemName;
            parameters["os_family"] = operatingSystemFamily;
            parameters["os_company"] = operatingSystemCompany;
            parameters["platform"] = platform;
            parameters["country"] = country;
            parameters["region"] = region;
            parameters["city"] = city;

            return await this
                .ProcessNoBodyRequestAsync<PostmarkOpensList>("/messages/outbound/opens", parameters);
        }


        /// <summary>
        /// Get the Click Events for messages, optionally filtering by various
        /// attributes of the Click Events and Messages.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="recipient"></param>
        /// <param name="tag"></param>
        /// <param name="clientName"></param>
        /// <param name="clientCompany"></param>
        /// <param name="clientFamily"></param>
        /// <param name="operatingSystemName"></param>
        /// <param name="operatingSystemFamily"></param>
        /// <param name="operatingSystemCompany"></param>
        /// <param name="platform"></param>
        /// <param name="country"></param>
        /// <param name="region"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<PostmarkClicksList> GetClickEventsForMessagesAsync(
            int offset = 0, int count = 100, string recipient = null, string tag = null,
            string clientName = null, string clientCompany = null, string clientFamily = null,
            string operatingSystemName = null, string operatingSystemFamily = null, string operatingSystemCompany = null,
            string platform = null, string country = null, string region = null, string city = null)
        {
            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;
            parameters["recipient"] = recipient;
            parameters["tag"] = tag;
            parameters["client_name"] = clientName;
            parameters["client_company"] = clientCompany;
            parameters["client_family"] = clientFamily;
            parameters["os_name"] = operatingSystemName;
            parameters["os_family"] = operatingSystemFamily;
            parameters["os_company"] = operatingSystemCompany;
            parameters["platform"] = platform;
            parameters["country"] = country;
            parameters["region"] = region;
            parameters["city"] = city;

            return await this
                .ProcessNoBodyRequestAsync<PostmarkClicksList>("/messages/outbound/clicks", parameters);
        }


        /// <summary>
        /// Get the Open events for a specific message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PostmarkOpensList> GetOpenEventsForMessageAsync(
            string messageId, int offset = 0, int count = 100)
        {

            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;

            return await this.ProcessNoBodyRequestAsync<PostmarkOpensList>
                (String.Format("/messages/outbound/opens/{0}", messageId), parameters);
        }

        /// <summary>
        /// Get the Clicks events for a specific message.
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PostmarkClicksList> GetClickEventsForMessageAsync(
            string messageId, int offset = 0, int count = 100)
        {

            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;

            return await this.ProcessNoBodyRequestAsync<PostmarkClicksList>
                (String.Format("/messages/outbound/clicks/{0}", messageId), parameters);
        }

        /// <summary>
        /// Get an overview of outbound statistics, optionally limiting by tag or time window.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundOverviewStats> GetOutboundOverviewStatsAsync(
            string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            return await this.ProcessNoBodyRequestAsync<PostmarkOutboundOverviewStats>
                ("/stats/outbound", parameters);
        }

        /// <summary>
        /// Retrieve sent counts for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundSentStats>
            GetOutboundSentCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            return await this.ProcessNoBodyRequestAsync<PostmarkOutboundSentStats>
                ("/stats/outbound/sends", parameters);
        }

        /// <summary>
        /// Retrieve bounce counts for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundBounceStats>
            GetOutboundBounceCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            return await this.ProcessNoBodyRequestAsync<PostmarkOutboundBounceStats>
                ("/stats/outbound/bounces", parameters);
        }

        /// <summary>
        /// Retrieve SPAM complaint counts for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundSpamComplaintStats> GetOutboundSpamComplaintCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            return await this.ProcessNoBodyRequestAsync<PostmarkOutboundSpamComplaintStats>
                ("/stats/outbound/spam", parameters);
        }

        /// <summary>
        /// Retrieve open tracking for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundTrackedStats> GetOutboundTrackingCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            return await this.ProcessNoBodyRequestAsync<PostmarkOutboundTrackedStats>
                ("/stats/outbound/tracked", parameters);
        }

        /// <summary>
        /// Retrieve open counts for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundOpenStats> GetOutboundOpenCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            return await this.ProcessNoBodyRequestAsync<PostmarkOutboundOpenStats>
                ("/stats/outbound/opens", parameters);
        }

        /// <summary>
        /// Retrieve platform statistics for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundPlatformStats> GetOutboundPlatformCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            return await this.ProcessNoBodyRequestAsync<PostmarkOutboundPlatformStats>
                ("/stats/outbound/opens/platforms", parameters);
        }

        /// <summary>
        /// Retrieve client usage statistics for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundClientStats> GetOutboundClientUsageCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            var result = await this.ProcessNoBodyRequestAsync<Dictionary<string, object>>("/stats/outbound/opens/emailclients", parameters);

            var retval = new PostmarkOutboundClientStats();
            var clientCounts = new Dictionary<string, int>();
            foreach (var a in result)
            {
                if (a.Key != "Days")
                {
                    clientCounts[a.Key] = (int)(long)a.Value;
                }
            }
            retval.ClientCounts = clientCounts;

            var dayCount = (JArray)result["Days"];
            var dayList = new List<PostmarkOutboundClientStats.DatedClientCount>();
            foreach (var obj in dayCount)
            {
                var newCount = new PostmarkOutboundClientStats.DatedClientCount();
                foreach (var i in (JObject)obj)
                {
                    if (i.Key == "Date")
                    {
                        newCount.Date = DateTime.Parse(i.Value.ToString());
                    }
                    else
                    {
                        newCount.ClientCounts[i.Key] = (int)(long)i.Value;
                    }
                }
                dayList.Add(newCount);
            }

            retval.Days = dayList;

            return retval;
        }

        /// <summary>
        /// Retrieve read time statistics for outbound emails, optionally including a time or tag filter.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public async Task<PostmarkOutboundReadStats> GetOutboundReadtimeStatsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var parameters = ConstructSentStatsFilter(tag, fromDate, toDate);
            var result = await this.ProcessNoBodyRequestAsync<Dictionary<string, object>>("/stats/outbound/opens/readtimes", parameters);

            var retval = new PostmarkOutboundReadStats();
            var clientCounts = new Dictionary<string, int>();
            foreach (var a in result)
            {
                if (a.Key != "Days")
                {
                    clientCounts[a.Key] = (int)(long)a.Value;
                }
            }
            retval.ReadCounts = clientCounts;

            var dayCount = (JArray)result["Days"];
            var dayList = new List<PostmarkOutboundReadStats.DatedReadCount>();
            foreach (var obj in dayCount)
            {
                var newCount = new PostmarkOutboundReadStats.DatedReadCount();
                foreach (var i in (JObject)obj)
                {
                    if (i.Key == "Date")
                    {
                        newCount.Date = DateTime.Parse(i.Value.ToString());
                    }
                    else
                    {
                        newCount.ReadCounts[i.Key] = (int)(long)i.Value;
                    }
                }
                dayList.Add(newCount);
            }

            retval.Days = dayList;

            return retval;
        }
        #endregion

        #region Inbound Triggers

        /// <summary>
        /// Define a new Inbound Rule Trigger
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public async Task<PostmarkInboundRuleTriggerInfo> CreateInboundRuleTriggerAsync(string rule)
        {
            var parameters = new Dictionary<string, object>();
            parameters["Rule"] = rule;

            return await this.ProcessRequestAsync<Dictionary<string, object>, PostmarkInboundRuleTriggerInfo>
                ("/triggers/inboundrules", HttpMethod.Post, parameters);
        }

        /// <summary>
        /// Delete an Inbound Rule Trigger
        /// </summary>
        /// <param name="triggerId"></param>
        /// <returns></returns>
        public async Task<PostmarkResponse> DeleteInboundRuleTrigger(int triggerId)
        {
            return await this
                .ProcessNoBodyRequestAsync<PostmarkResponse>("/triggers/inboundrules/" + triggerId,
                verb: HttpMethod.Delete);
        }

        /// <summary>
        /// List Inbound Rule Triggers.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<PostmarkInboundRuleTriggerList> GetAllInboundRuleTriggers(int offset = 0, int count = 100)
        {
            var parameters = new Dictionary<string, object>();
            parameters["offset"] = offset;
            parameters["count"] = count;

            return await this.ProcessNoBodyRequestAsync<PostmarkInboundRuleTriggerList>("/triggers/inboundrules", parameters);
        }

        #endregion

        #region Templates

        /// <summary>
        /// Get basic info associated with the specified ID.
        /// </summary>
        /// <param name="templateId">The ID of the template you wish to retrive.</param>
        /// <returns></returns>
        public async Task<PostmarkTemplate> GetTemplateAsync(long templateId)
        {
            return await ProcessNoBodyRequestAsync<PostmarkTemplate>("/templates/" + templateId, null, HttpMethod.Get);
        }

        /// <summary>
        /// Get basic info associated with the specified Alias.
        /// </summary>
        /// <param name="alias">The alias of the template you wish to retrive.</param>
        /// <returns></returns>
        public async Task<PostmarkTemplate> GetTemplateAsync(string alias)
        {
            return await ProcessNoBodyRequestAsync<PostmarkTemplate>("/templates/" + alias, null, HttpMethod.Get);
        }

        /// <summary>
        /// Get a listing of templates.
        /// </summary>
        /// <param name="count">The number of templates to return. Defaults to 0.</param>
        /// <param name="offset">The number of templates to "skip" before returning results. Defaults to 100.</param>
        /// <param name="templateType">Filter the resulting templates by their TemplateType. Defaults to: All</param>
        /// <param name="layoutTemplate">Filter results by layout template alias.</param>
        /// <returns></returns>
        public async Task<PostmarkTemplateListingResponse> GetTemplatesAsync(int offset = 0, int count = 100, TemplateTypeFilter templateType = TemplateTypeFilter.All,
            string layoutTemplate = null)
        {
            var query = new Dictionary<string, object>();
            query["Count"] = count;
            query["Offset"] = offset;
            query["TemplateType"] = Enum.GetName(typeof(TemplateTypeFilter), templateType);
            query["LayoutTemplate"] = layoutTemplate;

            return await ProcessNoBodyRequestAsync<PostmarkTemplateListingResponse>("/templates/", query, HttpMethod.Get);
        }

        /// <summary>
        /// Store a new template associated with this server.
        /// </summary>
        /// <param name="name">A display name for this template.</param>
        /// <param name="subject">The subject to be used when sending with this template.</param>
        /// <param name="htmlBody">The HTMLBody to be used when sending with this template. Optional if TextBody is specified.</param>
        /// <param name="textBody">The TextBody to be used when sending with this template. Optional if HtmlBody is specified.</param>
        /// <param name="alias">A friendly name to use for this template to access it, or to send with it.</param>
        /// <param name="templateType">The type of the template to create.</param>
        /// <param name="layoutTemplate">The alias of the Layout template that you want to use as layout for this Standard template.</param>
        /// <returns></returns>
        public async Task<BasicTemplateInformation> CreateTemplateAsync(string name, string subject, string htmlBody = null, string textBody = null, string alias = null,
            TemplateType templateType = TemplateType.Standard, string layoutTemplate = null)
        {
            var body = new Dictionary<string, object>();
            body["Name"] = name;
            body["HTMLBody"] = htmlBody;
            body["TextBody"] = textBody;
            body["Subject"] = subject;
            body["Alias"] = alias;
            body["TemplateType"] = Enum.GetName(typeof(TemplateType), templateType);
            body["LayoutTemplate"] = layoutTemplate;

            return await ProcessRequestAsync<Dictionary<string, object>, BasicTemplateInformation>("/templates/", HttpMethod.Post, body);
        }

        public async Task<BasicTemplateInformation> EditTemplateAsync(string alias, string name = null, string subject = null, string htmlBody = null, string textBody = null,
            string layoutTemplate = null)
        {
            var body = new Dictionary<string, object>();
            body["Name"] = name;
            body["HTMLBody"] = htmlBody;
            body["TextBody"] = textBody;
            body["Subject"] = subject;
            body["LayoutTemplate"] = layoutTemplate;

            return await ProcessRequestAsync<Dictionary<string, object>, BasicTemplateInformation>("/templates/" + alias, HttpMethod.Put, body);
        }

        public async Task<BasicTemplateInformation> EditTemplateAsync(long templateId, string name = null, string subject = null, string htmlBody = null, string textBody = null, string alias = null,
            string layoutTemplate = null)
        {
            var body = new Dictionary<string, object>();
            body["Name"] = name;
            body["HTMLBody"] = htmlBody;
            body["TextBody"] = textBody;
            body["Subject"] = subject;
            body["Alias"] = alias;
            body["LayoutTemplate"] = layoutTemplate;

            return await ProcessRequestAsync<Dictionary<string, object>, BasicTemplateInformation>("/templates/" + templateId, HttpMethod.Put, body);
        }

        /// <summary>
        /// Delete a template from the server.
        /// </summary>
        /// <param name="templateId">The ID of the template you wish to delete from the server.</param>
        /// <returns></returns>
        public async Task<PostmarkResponse> DeleteTemplateAsync(long templateId)
        {
            return await ProcessNoBodyRequestAsync<PostmarkResponse>("/templates/" + templateId, null, HttpMethod.Delete);
        }

        /// <summary>
        /// Delete a template from the server.
        /// </summary>
        /// <param name="templateAlias">The Alias of the template you wish to delete from the server.</param>
        /// <returns></returns>
        public async Task<PostmarkResponse> DeleteTemplateAsync(string templateAlias)
        {
            return await ProcessNoBodyRequestAsync<PostmarkResponse>("/templates/" + templateAlias, null, HttpMethod.Delete);
        }

        public async Task<PostmarkResponse> SendMessageAsync(TemplatedPostmarkMessage emailToSend)
        {
            return await SendEmailWithTemplateAsync(emailToSend);
        }

        public async Task<PostmarkResponse> SendEmailWithTemplateAsync(TemplatedPostmarkMessage emailToSend)
        {
            return await ProcessRequestAsync<TemplatedPostmarkMessage, PostmarkResponse>("/email/withTemplate", HttpMethod.Post, emailToSend);
        }

        public async Task<PostmarkResponse> SendEmailWithTemplateAsync<T>(string templateAlias, T templateModel,
            string to, string from,
            bool? inlineCss = null, string cc = null,
            string bcc = null, string replyTo = null,
            bool? trackOpens = null,
            IDictionary<string, string> headers = null,
            IDictionary<string, string> metadata = null,
            string messageStream = null,
            params PostmarkMessageAttachment[] attachments)
        {
            return await InternalSendEmailWithTemplateAsync(templateAlias, templateModel, to, from, inlineCss, cc,
            bcc, replyTo, trackOpens, headers, metadata, messageStream, attachments);
        }

        public async Task<PostmarkResponse> SendEmailWithTemplateAsync<T>(long templateId, T templateModel,
            string to, string from,
            bool? inlineCss = null, string cc = null,
            string bcc = null, string replyTo = null,
            bool? trackOpens = null,
            IDictionary<string, string> headers = null,
            IDictionary<string, string> metadata = null,
            string messageStream = null,
            params PostmarkMessageAttachment[] attachments)
        {
            return await InternalSendEmailWithTemplateAsync(templateId, templateModel, to, from, inlineCss, cc,
            bcc, replyTo, trackOpens, headers, metadata, messageStream, attachments);
        }

        private async Task<PostmarkResponse> InternalSendEmailWithTemplateAsync<T>(object templateReference, T templateModel,
            string to, string from,
            bool? inlineCss = null, string cc = null,
            string bcc = null, string replyTo = null,
            bool? trackOpens = null,
            IDictionary<string, string> headers = null,
            IDictionary<string, string> metadata = null,
            string messageStream = null,
            params PostmarkMessageAttachment[] attachments)
        {

            var email = new TemplatedPostmarkMessage();
            if (templateReference is long)
            {
                email.TemplateId = (long)templateReference;
            }
            else
            {
                email.TemplateAlias = (string)templateReference;
            }
            email.TemplateModel = templateModel;
            email.MessageStream = messageStream;
            email.To = to;
            email.From = from;
            if (inlineCss.HasValue)
            {
                email.InlineCss = inlineCss.Value;
            }
            email.Cc = cc;
            email.Bcc = bcc;
            if (trackOpens.HasValue)
            {
                email.TrackOpens = trackOpens.Value;
            }
            email.ReplyTo = replyTo;
            if (headers != null)
            {
                email.Headers = new HeaderCollection(headers);
            }
            email.Metadata = metadata;
            if (attachments != null)
            {
                email.Attachments = attachments;
            }
            return await SendEmailWithTemplateAsync(email);
        }

        /// <summary>
        /// Validate a template.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subject">The subject content to validate.</param>
        /// <param name="htmlBody">The HTML body content to validate.</param>
        /// <param name="textBody">The plain text body content to validate.</param>
        /// <param name="testRenderModel">The template model to be used when rendering test content.</param>
        /// <param name="inlineCssForHtmlTestRender">Controls whether style blocks will be inlined as style attributes on matching html elements in HtmlBody.</param>
        /// <param name="templateType">Validate templates based on template type.</param>
        /// <param name="layoutTemplate">An optional string to specify which layout template alias to use to validate a standard template.</param>
        /// <returns></returns>
        public async Task<TemplateValidationResponse> ValidateTemplateAsync<T>(string subject = null, string htmlBody = null,
            string textBody = null, T testRenderModel = default(T), bool inlineCssForHtmlTestRender = true,
            TemplateType templateType = TemplateType.Standard, string layoutTemplate = null)
        {
            var body = new Dictionary<string, object>();
            body["TestRenderModel"] = testRenderModel;
            body["Subject"] = subject;
            body["HtmlBody"] = htmlBody;
            body["TextBody"] = textBody;
            body["InlineCssForHtmlTestRender"] = inlineCssForHtmlTestRender;
            body["TemplateType"] = Enum.GetName(typeof(TemplateType), templateType);
            body["LayoutTemplate"] = layoutTemplate;

            return await ProcessRequestAsync<Dictionary<string, object>, TemplateValidationResponse>("/templates/validate", HttpMethod.Post, body);
        }
        #endregion

        #region Webhooks

        /// <summary>
        /// Gets the webhook configuration for the provided configuration id
        /// </summary>
        /// <param name="configurationId">Configuration Id to search for</param>
        /// <returns></returns>
        public async Task<WebhookConfiguration> GetWebhookConfigurationAsync(long configurationId)
        {
            return await ProcessNoBodyRequestAsync<WebhookConfiguration>($"/webhooks/{configurationId}", null, HttpMethod.Get);
        }

        /// <summary>
        /// Gets a listing of webhook configurations for the provided server
        /// </summary>
        /// <param name="messageStream"> Optional message stream to search for.
        /// If not provided, all configurations for the server will be returned.</param>
        /// <returns></returns>
        public async Task<WebhookConfigurationListingResponse> GetWebhookConfigurationsAsync(string messageStream = null)
        {
            var query = new Dictionary<string, object> { ["MessageStream"] = messageStream };

            return await ProcessNoBodyRequestAsync<WebhookConfigurationListingResponse>("/webhooks/", query, HttpMethod.Get);
        }

        /// <summary>
        /// Delete a webhook configuration from the server.
        /// </summary>
        /// <param name="configurationId">Configuration id to search for</param>
        /// <returns></returns>
        public async Task<PostmarkResponse> DeleteWebhookConfigurationAsync(long configurationId)
        {
            return await ProcessNoBodyRequestAsync<PostmarkResponse>($"/webhooks/{configurationId}", null, HttpMethod.Delete);
        }

        /// <summary>
        /// Creates a new webhook configuration
        /// </summary>
        /// <param name="url">The webhook URL</param>
        /// <param name="messageStream">Message stream this configuration should belong to.
        /// If not provided, it will belong to the default transactional stream.</param>
        /// <param name="httpAuth">Optional Basic HTTP Authentication</param>
        /// <param name="httpHeaders">Optional list of custom HTTP headers</param>
        /// <param name="triggers">Optional triggers for this webhook configuration</param>
        /// <returns></returns>
        public async Task<WebhookConfiguration> CreateWebhookConfigurationAsync(string url, string messageStream = null,
            HttpAuth httpAuth = null, IEnumerable<HttpHeader> httpHeaders = null, WebhookConfigurationTriggers triggers = null)
        {
            var body = new Dictionary<string, object>();
            body["Url"] = url;
            body["MessageStream"] = messageStream;
            body["HttpAuth"] = httpAuth;
            body["HttpHeaders"] = httpHeaders;
            body["Triggers"] = triggers;

            return await ProcessRequestAsync<Dictionary<string, object>, WebhookConfiguration>("/webhooks/", HttpMethod.Post, body);
        }

        /// <summary>
        /// Update a webhook configuration for the provided configuration id.
        /// </summary>
        /// <param name="configurationId">Configuration id to search for</param>
        /// <param name="url">The webhook URL</param>
        /// <param name="httpAuth">Optional Basic HTTP Authentication</param>
        /// <param name="httpHeaders">Optional list of custom HTTP headers</param>
        /// <param name="triggers">Optional triggers for this webhook configuration</param>
        /// <returns></returns>
        public async Task<WebhookConfiguration> EditWebhookConfigurationAsync(long configurationId, string url,
            HttpAuth httpAuth = null, IEnumerable<HttpHeader> httpHeaders = null, WebhookConfigurationTriggers triggers = null)
        {
            var body = new Dictionary<string, object>();
            body["Url"] = url;
            body["HttpAuth"] = httpAuth;
            body["HttpHeaders"] = httpHeaders;
            body["Triggers"] = triggers;

            return await ProcessRequestAsync<Dictionary<string, object>, WebhookConfiguration>($"/webhooks/{configurationId}",
                HttpMethod.Put, body);
        }
        #endregion
    }
}
