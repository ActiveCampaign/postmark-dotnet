using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Postmark.Model.MessageStreams;
using Postmark.Model.Suppressions;
using PostmarkDotNet.Model;
using PostmarkDotNet.Model.Webhooks;

namespace PostmarkDotNet
{
    public interface IPostmarkClient
    {
        // Email Sending
        Task<PostmarkResponse> SendMessageAsync(PostmarkMessage message);
        Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(params PostmarkMessage[] messages);
        Task<PostmarkResponse> SendMessageAsync(TemplatedPostmarkMessage message);
        Task<PostmarkResponse> SendEmailWithTemplateAsync(TemplatedPostmarkMessage emailToSend);
        Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(params TemplatedPostmarkMessage[] messages);
        Task<IEnumerable<PostmarkResponse>> SendEmailsWithTemplateAsync(params TemplatedPostmarkMessage[] messages);
        Task<PostmarkResponse> SendEmailWithTemplateAsync<T>(string templateAlias, T templateModel, string to, string from, bool? inlineCss = null, string cc = null, string bcc = null, string replyTo = null, bool? trackOpens = null, IDictionary<string, string> headers = null, IDictionary<string, string> metadata = null, string messageStream = null, params PostmarkMessageAttachment[] attachments);
        Task<PostmarkResponse> SendEmailWithTemplateAsync<T>(long templateId, T templateModel, string to, string from, bool? inlineCss = null, string cc = null, string bcc = null, string replyTo = null, bool? trackOpens = null, IDictionary<string, string> headers = null, IDictionary<string, string> metadata = null, string messageStream = null, params PostmarkMessageAttachment[] attachments);

        // Bounces
        Task<PostmarkDeliveryStats> GetDeliveryStatsAsync();
        Task<PostmarkBounces> GetBouncesAsync(int offset = 0, int count = 100, PostmarkBounceType? type = null, bool? inactive = null, string emailFilter = null, string tag = null, string messageID = null, string fromDate = null, string toDate = null);
        Task<PostmarkBounce> GetBounceAsync(long bounceId);
        Task<PostmarkBounceDump> GetBounceDumpAsync(long bounceId);
        Task<PostmarkBounceActivation> ActivateBounceAsync(long bounceId);

        // Outbound Messages
        Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int offset = 0, int count = 100, string recipient = null, string fromemail = null, string tag = null, string subject = null, OutboundMessageStatus status = OutboundMessageStatus.Sent, string toDate = null, string fromDate = null, IDictionary<string, string> metadata = null, string messagestream = null);
        Task<OutboundMessageDetail> GetOutboundMessageDetailsAsync(string messageID);
        Task<MessageDump> GetOutboundMessageDumpAsync(string messageID);

        // Inbound Messages
        Task<PostmarkInboundMessageList> GetInboundMessagesAsync(int offset = 0, int count = 100, string recipient = null, string fromemail = null, string subject = null, string mailboxhash = null, InboundMessageStatus? status = InboundMessageStatus.Processed, string toDate = null, string fromDate = null);
        Task<PostmarkInboundMessage> GetInboundMessageDetailsAsync(string messageID);
        Task<PostmarkResponse> BypassBlockedInboundMessage(string messageid);
        Task<PostmarkResponse> RetryInboundHookForMessage(string messageId);

        // Server
        Task<PostmarkServer> GetServerAsync();
        Task<PostmarkServer> EditServerAsync(string name = null, string color = null, bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null, string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null, bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null, LinkTrackingOptions? trackLinks = null, string clickHookUrl = null, string deliveryHookUrl = null);

        // Stats
        Task<PostmarkOpensList> GetOpenEventsForMessagesAsync(int offset = 0, int count = 100, string recipient = null, string tag = null, string clientName = null, string clientCompany = null, string clientFamily = null, string operatingSystemName = null, string operatingSystemFamily = null, string operatingSystemCompany = null, string platform = null, string country = null, string region = null, string city = null);
        Task<PostmarkClicksList> GetClickEventsForMessagesAsync(int offset = 0, int count = 100, string recipient = null, string tag = null, string clientName = null, string clientCompany = null, string clientFamily = null, string operatingSystemName = null, string operatingSystemFamily = null, string operatingSystemCompany = null, string platform = null, string country = null, string region = null, string city = null);
        Task<PostmarkOpensList> GetOpenEventsForMessageAsync(string messageId, int offset = 0, int count = 100);
        Task<PostmarkClicksList> GetClickEventsForMessageAsync(string messageId, int offset = 0, int count = 100);
        Task<PostmarkOutboundOverviewStats> GetOutboundOverviewStatsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundSentStats> GetOutboundSentCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundBounceStats> GetOutboundBounceCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundSpamComplaintStats> GetOutboundSpamComplaintCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundTrackedStats> GetOutboundTrackingCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundOpenStats> GetOutboundOpenCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundPlatformStats> GetOutboundPlatformCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundClientStats> GetOutboundClientUsageCountsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<PostmarkOutboundReadStats> GetOutboundReadtimeStatsAsync(string tag = null, DateTime? fromDate = null, DateTime? toDate = null);

        // Inbound Triggers
        Task<PostmarkInboundRuleTriggerInfo> CreateInboundRuleTriggerAsync(string rule);
        Task<PostmarkResponse> DeleteInboundRuleTrigger(int triggerId);
        Task<PostmarkInboundRuleTriggerList> GetAllInboundRuleTriggers(int offset = 0, int count = 100);

        // Templates
        Task<PostmarkTemplate> GetTemplateAsync(long templateId);
        Task<PostmarkTemplate> GetTemplateAsync(string alias);
        Task<PostmarkTemplateListingResponse> GetTemplatesAsync(int offset = 0, int count = 100, TemplateTypeFilter templateType = TemplateTypeFilter.All, string layoutTemplate = null);
        Task<BasicTemplateInformation> CreateTemplateAsync(string name, string subject, string htmlBody = null, string textBody = null, string alias = null, TemplateType templateType = TemplateType.Standard, string layoutTemplate = null);
        Task<BasicTemplateInformation> EditTemplateAsync(string alias, string name = null, string subject = null, string htmlBody = null, string textBody = null, string layoutTemplate = null);
        Task<BasicTemplateInformation> EditTemplateAsync(long templateId, string name = null, string subject = null, string htmlBody = null, string textBody = null, string alias = null, string layoutTemplate = null);
        Task<PostmarkResponse> DeleteTemplateAsync(long templateId);
        Task<PostmarkResponse> DeleteTemplateAsync(string templateAlias);
        Task<TemplateValidationResponse> ValidateTemplateAsync<T>(string subject = null, string htmlBody = null, string textBody = null, T testRenderModel = default, bool inlineCssForHtmlTestRender = true, TemplateType templateType = TemplateType.Standard, string layoutTemplate = null);

        // Webhooks
        Task<WebhookConfiguration> GetWebhookConfigurationAsync(long configurationId);
        Task<WebhookConfigurationListingResponse> GetWebhookConfigurationsAsync(string messageStream = null);
        Task<PostmarkResponse> DeleteWebhookConfigurationAsync(long configurationId);
        Task<WebhookConfiguration> CreateWebhookConfigurationAsync(string url, string messageStream = null, HttpAuth httpAuth = null, IEnumerable<HttpHeader> httpHeaders = null, WebhookConfigurationTriggers triggers = null);
        Task<WebhookConfiguration> EditWebhookConfigurationAsync(long configurationId, string url, HttpAuth httpAuth = null, IEnumerable<HttpHeader> httpHeaders = null, WebhookConfigurationTriggers triggers = null);

        // Suppressions
        Task<PostmarkSuppressionListing> ListSuppressions(PostmarkSuppressionQuery query, string messageStream = "outbound");
        Task<PostmarkBulkSuppressionResult> CreateSuppressions(IEnumerable<PostmarkSuppressionChangeRequest> suppressionChanges, string messageStream = "outbound");
        Task<PostmarkBulkReactivationResult> DeleteSuppressions(IEnumerable<PostmarkSuppressionChangeRequest> suppressionChanges, string messageStream = "outbound");

        // Message Streams
        Task<PostmarkMessageStream> CreateMessageStream(string id, MessageStreamType type, string name, string description = null, UnsubscribeHandlingType? unsubscriptionHandlingType = null);
        Task<PostmarkMessageStream> EditMessageStream(string id, string name = null, string description = null, UnsubscribeHandlingType? unsubscriptionHandlingType = null);
        Task<PostmarkMessageStream> GetMessageStream(string id);
        Task<PostmarkMessageStreamListing> ListMessageStreams(MessageStreamTypeFilter messageStreamType = MessageStreamTypeFilter.All, bool includeArchivedStreams = false);
        Task<PostmarkMessageStreamArchivalConfirmation> ArchiveMessageStream(string id);
        Task<PostmarkMessageStream> UnArchiveMessageStream(string id);
    }
}