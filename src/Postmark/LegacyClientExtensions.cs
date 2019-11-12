#pragma warning disable CS1591
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostmarkDotNet.Model;

namespace PostmarkDotNet.Legacy
{
    /// <summary>
    /// A legacy shim, proxying the 1.x IAsyncResult style client calls.
    /// </summary>
    /// <remarks>This shim is provided as a convenience to consumers of the 1.x client. 
    /// It is recommended that consumers use the 2.0 Task-based methods instead of these extensions.</remarks>
    [Obsolete("This shim is provided as a convenience to consumers of the 1.x client. " +
        "It is recommended that consumers use the 2.0 Task-based methods instead of these extensions.")]
    public static class LegacyClientExtensions
    {
        public static IAsyncResult BeginSendMessage(this PostmarkClient client, string from, string to, string subject, string textBody, string htmlBody)
        {
            return client.SendMessageAsync(from, to, subject, textBody, htmlBody);
        }

        public static IAsyncResult BeginSendMessage(this PostmarkClient client, PostmarkMessage message)
        {
            return client.SendMessageAsync(message);
        }

        public static IAsyncResult BeginSendMessages(this PostmarkClient client, IEnumerable<PostmarkMessage> messages)
        {
            return client.SendMessagesAsync(messages);
        }

        public static IAsyncResult BeginSendMessages(this PostmarkClient client, params PostmarkMessage[] messages)
        {
            return client.SendMessagesAsync(messages);
        }

        public static IEnumerable<PostmarkResponse> EndSendMessages(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<IEnumerable<PostmarkResponse>>();
        }

        private static T UnwrapResult<T>(this IAsyncResult result)
        {
            return ((Task<T>)result).Result;
        }

        public static PostmarkResponse EndSendMessage(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkResponse>();
        }

        public static IAsyncResult BeginGetDeliveryStats(this PostmarkClient client)
        {
            return client.GetDeliveryStatsAsync();
        }

        public static PostmarkDeliveryStats EndGetDeliveryStats(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkDeliveryStats>();
        }

        public static IAsyncResult BeginGetBounces
            (this PostmarkClient client, PostmarkBounceType type, bool? inactive,
            string emailFilter, string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, inactive, emailFilter, tag);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client, bool? inactive,
            string emailFilter, string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, null, inactive, emailFilter, tag);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client, PostmarkBounceType type, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count);
        }

        public static IAsyncResult BeginGetBounces
            (this PostmarkClient client, PostmarkBounceType type,
            bool? inactive, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, inactive);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client,
            bool? inactive, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, inactive: inactive);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client,
            PostmarkBounceType type, string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, emailFilter: emailFilter);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client, string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, emailFilter: emailFilter);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client,
            PostmarkBounceType type, string emailFilter, string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, emailFilter: emailFilter, tag: tag);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client, string emailFilter, string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, emailFilter: emailFilter, tag: tag);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client,
            PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, inactive, emailFilter);
        }

        public static IAsyncResult BeginGetBounces(this PostmarkClient client,
            bool? inactive, string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, inactive: inactive, emailFilter: emailFilter);
        }

        public static PostmarkBounces EndGetBounces(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkBounces>();
        }

        public static IAsyncResult BeginGetBounce(this PostmarkClient client, string bounceId)
        {
            return client.GetBounceAsync(int.Parse(bounceId));
        }

        public static PostmarkBounce EndGetBounce(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkBounce>();
        }

        public static IAsyncResult BeginGetBounceDump(this PostmarkClient client, string bounceId)
        {
            return client.GetBounceDumpAsync(int.Parse(bounceId));
        }

        public static PostmarkBounceDump EndGetBounceDump(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkBounceDump>();
        }

        public static IAsyncResult BeginActivateBounce(this PostmarkClient client, string bounceId)
        {
            return client.ActivateBounceAsync(int.Parse(bounceId));
        }

        public static PostmarkBounceActivation EndActivateBounce(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkBounceActivation>();
        }

        public static IAsyncResult BeginGetOutboundMessages(this PostmarkClient client, int count, string subject, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, subject: subject);
        }

        public static IAsyncResult BeginGetOutboundMessages(this PostmarkClient client, int count, int offset, string recipient)
        {
            return client.GetOutboundMessagesAsync(offset, count, recipient: recipient);
        }

        public static IAsyncResult BeginGetOutboundMessages(this PostmarkClient client, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count);
        }

        public static IAsyncResult BeginGetOutboundMessages(this PostmarkClient client,
            string recipient, string fromemail, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, recipient, fromemail);
        }

        public static IAsyncResult BeginGetOutboundMessages(this PostmarkClient client, string subject, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, subject: subject);
        }

        public static IAsyncResult BeginGetOutboundMessages
            (this PostmarkClient client, string fromemail, string tag,
            string subject, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, fromemail: fromemail, tag: tag, subject: subject);
        }

        public static IAsyncResult BeginGetOutboundMessages
            (this PostmarkClient client, string recipient, string fromemail,
            string tag, string subject, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, recipient, fromemail, tag, subject);
        }

        public static PostmarkOutboundMessageList EndGetOutboundMessages(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkOutboundMessageList>();
        }

        public static IAsyncResult BeginGetOutboundMessageDetail(this PostmarkClient client, string messageID)
        {
            return client.GetOutboundMessageDetailsAsync(messageID);
        }

        public static OutboundMessageDetail EndGetOutboundMessageDetail(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<OutboundMessageDetail>();
        }

        public static IAsyncResult BeginGetOutboundMessageDump(this PostmarkClient client, string messageID)
        {
            return client.GetOutboundMessageDumpAsync(messageID);
        }

        public static MessageDump EndGetOutboundMessageDump(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<MessageDump>();
        }

        public static IAsyncResult BeginGetInboundMessages(this PostmarkClient client, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count);
        }

        public static IAsyncResult BeginGetInboundMessages
            (this PostmarkClient client, string fromemail, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, fromemail: fromemail);
        }

        public static IAsyncResult BeginGetInboundMessages
            (this PostmarkClient client, string fromemail, string subject, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, fromemail: fromemail, subject: subject);
        }

        public static IAsyncResult BeginGetInboundMessages
            (this PostmarkClient client, string recipient, string fromemail,
            string subject, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, recipient, fromemail, subject);
        }

        public static IAsyncResult BeginGetInboundMessages
            (this PostmarkClient client, string recipient, string fromemail,
            string subject, string mailboxhash, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, recipient, fromemail, subject, mailboxhash);
        }

        public static PostmarkInboundMessageList EndGetInboundMessages(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<PostmarkInboundMessageList>();
        }

        public static IAsyncResult BeginGetInboundMessageDetail(this PostmarkClient client, string messageID)
        {
            return client.GetInboundMessageDetailsAsync(messageID);
        }

        public static InboundMessageDetail EndGetInboundMessageDetail(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<InboundMessageDetail>();
        }

        public static PostmarkResponse SendMessage(this PostmarkClient client, string from, string to, string subject, string textBody, string htmlBody)
        {
            return Task.Run(async () => await client.SendMessageAsync(from, to, subject, textBody, htmlBody)).Result;
        }

        public static PostmarkResponse SendMessage(this PostmarkClient client, PostmarkMessage message)
        {
            return Task.Run(async () => await client.SendMessageAsync(message)).Result;
        }

        public static IEnumerable<PostmarkResponse> SendMessages(this PostmarkClient client, params PostmarkMessage[] messages)
        {
            return Task.Run(async () => await client.SendMessagesAsync(messages)).Result;
        }

        public static IEnumerable<PostmarkResponse> SendMessages(this PostmarkClient client, IEnumerable<PostmarkMessage> messages)
        {
            return Task.Run(async () => await client.SendMessagesAsync(messages)).Result;
        }

        public static PostmarkDeliveryStats GetDeliveryStats(this PostmarkClient client)
        {
            return Task.Run(async () => await client.GetDeliveryStatsAsync()).Result;
        }

        public static PostmarkBounces GetBounces
            (this PostmarkClient client, bool? inactive, string emailFilter,
            string tag, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, null, inactive, emailFilter, tag)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, type, inactive, emailFilter, tag)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client, PostmarkBounceType type, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, type)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count)).Result;
        }

        public static PostmarkBounces GetBounces
            (this PostmarkClient client, PostmarkBounceType type, bool? inactive, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, type, inactive)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client, bool? inactive, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, inactive: inactive)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, string emailFilter, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, type, emailFilter: emailFilter)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            string emailFilter, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, emailFilter: emailFilter)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, string emailFilter, string tag, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, type, emailFilter: emailFilter, tag: tag)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            string emailFilter, string tag, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, emailFilter: emailFilter, tag: tag)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count, type,
                inactive: inactive, emailFilter: emailFilter)).Result;
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            bool? inactive, string emailFilter, int offset, int count)
        {
            return Task.Run(async () => await client.GetBouncesAsync(offset, count,
                 inactive: inactive, emailFilter: emailFilter)).Result;
        }

        public static PostmarkBounce GetBounce(this PostmarkClient client, string bounceId)
        {
            return Task.Run(async () => await client.GetBounceAsync(int.Parse(bounceId))).Result;
        }

        public static PostmarkBounceDump GetBounceDump(this PostmarkClient client, string bounceId)
        {
            return Task.Run(async () => await client.GetBounceDumpAsync(int.Parse(bounceId))).Result;
        }

        public static PostmarkBounceActivation ActivateBounce(this PostmarkClient client, string bounceId)
        {
            return Task.Run(async () => await client.ActivateBounceAsync(int.Parse(bounceId))).Result;
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client, int count, string subject, int offset)
        {
            return Task.Run(async () => await client.GetOutboundMessagesAsync(offset, count, subject: subject)).Result;
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client, int count, int offset, string recipient)
        {
            return Task.Run(async () => await client.GetOutboundMessagesAsync(offset, count, recipient)).Result;
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client, int count, int offset)
        {
            return Task.Run(async () => await client.GetOutboundMessagesAsync(offset, count)).Result;
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string recipient, string fromemail, int count, int offset)
        {
            return Task.Run(async () => await client.GetOutboundMessagesAsync(offset, count, recipient, fromemail)).Result;
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string subject, int count, int offset)
        {
            return Task.Run(async () => await client.GetOutboundMessagesAsync(offset, count, subject: subject)).Result;
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string fromemail, string tag, string subject, int count, int offset)
        {
            return Task.Run(async () => await client.GetOutboundMessagesAsync(offset, count,
                fromemail: fromemail, subject: subject, tag: tag)).Result;
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string recipient, string fromemail, string tag, string subject, int count, int offset)
        {
            return Task.Run(async () => await client.GetOutboundMessagesAsync(offset, count, recipient, fromemail, tag, subject)).Result;
        }

        public static OutboundMessageDetail GetOutboundMessageDetail(this PostmarkClient client, string messageID)
        {
            return Task.Run(async () => await client.GetOutboundMessageDetailsAsync(messageID)).Result;
        }

        public static MessageDump GetOutboundMessageDump(this PostmarkClient client, string messageID)
        {
            return Task.Run(async () => await client.GetOutboundMessageDumpAsync(messageID)).Result;
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client, int count, int offset)
        {
            return Task.Run(async () => await client.GetInboundMessagesAsync(offset, count)).Result;
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string fromemail, int count, int offset)
        {
            return Task.Run(async () => await client.GetInboundMessagesAsync(offset, count, fromemail: fromemail)).Result;
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string fromemail, string subject, int count, int offset)
        {
            return Task.Run(async () => await client.GetInboundMessagesAsync(offset, count, fromemail: fromemail, subject: subject)).Result;
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string recipient, string fromemail, string subject, int count, int offset)
        {
            return Task.Run(async () => await client.GetInboundMessagesAsync(offset, count, recipient, fromemail, subject)).Result;
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string recipient, string fromemail, string subject, string mailboxhash, int count, int offset)
        {
            return Task.Run(async () => await client.GetInboundMessagesAsync(offset, count, recipient, fromemail, subject, mailboxhash)).Result;
        }

        public static InboundMessageDetail GetInboundMessageDetail(this PostmarkClient client, string messageID)
        {
            return Task.Run(async () => await client.GetInboundMessageDetailsAsync(messageID)).Result;
        }
    }
}
#pragma warning restore CS1591