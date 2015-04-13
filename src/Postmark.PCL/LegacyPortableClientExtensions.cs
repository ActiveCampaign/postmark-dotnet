using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostmarkDotNet;
using PostmarkDotNet.Model;

namespace PostmarkDotNet.Legacy
{
    /// <summary>
    /// A legacy shim, proxying the 1.x client calls.
    /// </summary>
    /// <remarks>This shim is provided as a convenience to consumers of the 1.x client. 
    /// It is recommended that consumers use the 2.0 Task-based methods instead of these extensions.</remarks>
    [Obsolete("This shim is provided as a convenience to consumers of the 1.x client. " +
        "It is recommended that consumers use the 2.0 Task-based methods instead of these extensions.")]
    public static class LegacyClientExtensions
    {
        public static IAsyncResult BeginSendMessage(this PostmarkClient client, string from, string to, string subject, string body)
        {
            return client.SendMessageAsync(from, to, subject, body);
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

        public static IAsyncResult BeginGetBounceTags(this PostmarkClient client)
        {
            return client.GetBounceTagsAsync();
        }

        public static IEnumerable<string> EndGetBounceTags(this PostmarkClient client, IAsyncResult asyncResult)
        {
            return asyncResult.UnwrapResult<IEnumerable<string>>();
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

        //public string Authority
        //{
        //    get
        //    {

        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public string ServerToken
        //{
        //    get { throw new NotImplementedException(); }
        //}

        public static PostmarkResponse SendMessage(this PostmarkClient client, string from, string to, string subject, string body)
        {
            return client.SendMessageAsync(from, to, subject, body).WaitForResult();
        }

        public static PostmarkResponse SendMessage(this PostmarkClient client, PostmarkMessage message)
        {
            return client.SendMessageAsync(message).WaitForResult();
        }

        public static IEnumerable<PostmarkResponse> SendMessages(this PostmarkClient client, params PostmarkMessage[] messages)
        {
            return client.SendMessagesAsync(messages).WaitForResult();
        }

        public static IEnumerable<PostmarkResponse> SendMessages(this PostmarkClient client, IEnumerable<PostmarkMessage> messages)
        {
            return client.SendMessagesAsync(messages).WaitForResult();
        }

        public static PostmarkDeliveryStats GetDeliveryStats(this PostmarkClient client)
        {
            return client.GetDeliveryStatsAsync().WaitForResult();
        }

        public static PostmarkBounces GetBounces
            (this PostmarkClient client, bool? inactive, string emailFilter,
            string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, null, inactive, emailFilter, tag).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, inactive, emailFilter, tag).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client, PostmarkBounceType type, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count).WaitForResult();
        }

        public static PostmarkBounces GetBounces
            (this PostmarkClient client, PostmarkBounceType type, bool? inactive, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, inactive).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client, bool? inactive, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, inactive: inactive).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, emailFilter: emailFilter).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, emailFilter: emailFilter).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, string emailFilter, string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type, emailFilter: emailFilter, tag: tag).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            string emailFilter, string tag, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, emailFilter: emailFilter, tag: tag).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count, type,
                inactive: inactive, emailFilter: emailFilter).WaitForResult();
        }

        public static PostmarkBounces GetBounces(this PostmarkClient client,
            bool? inactive, string emailFilter, int offset, int count)
        {
            return client.GetBouncesAsync(offset, count,
                 inactive: inactive, emailFilter: emailFilter).WaitForResult();
        }

        public static PostmarkBounce GetBounce(this PostmarkClient client, string bounceId)
        {
            return client.GetBounceAsync(int.Parse(bounceId)).WaitForResult();
        }

        public static IEnumerable<string> GetBounceTags(this PostmarkClient client)
        {
            return client.GetBounceTagsAsync().WaitForResult();
        }

        public static PostmarkBounceDump GetBounceDump(this PostmarkClient client, string bounceId)
        {
            return client.GetBounceDumpAsync(int.Parse(bounceId)).WaitForResult();
        }

        public static PostmarkBounceActivation ActivateBounce(this PostmarkClient client, string bounceId)
        {
            return client.ActivateBounceAsync(int.Parse(bounceId)).WaitForResult();
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client, int count, string subject, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, subject: subject).WaitForResult();
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client, int count, int offset, string recipient)
        {
            return client.GetOutboundMessagesAsync(offset, count, recipient).WaitForResult();
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count).WaitForResult();
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string recipient, string fromemail, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, recipient, fromemail).WaitForResult();
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string subject, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, subject: subject).WaitForResult();
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string fromemail, string tag, string subject, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count,
                fromemail: fromemail, subject: subject, tag: tag).WaitForResult();
        }

        public static PostmarkOutboundMessageList GetOutboundMessages(this PostmarkClient client,
            string recipient, string fromemail, string tag, string subject, int count, int offset)
        {
            return client.GetOutboundMessagesAsync(offset, count, recipient, fromemail, tag, subject).WaitForResult();
        }

        public static OutboundMessageDetail GetOutboundMessageDetail(this PostmarkClient client, string messageID)
        {
            return client.GetOutboundMessageDetailsAsync(messageID).WaitForResult();
        }

        public static MessageDump GetOutboundMessageDump(this PostmarkClient client, string messageID)
        {
            return client.GetOutboundMessageDumpAsync(messageID).WaitForResult();
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count).WaitForResult();
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string fromemail, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, fromemail: fromemail).WaitForResult();
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string fromemail, string subject, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, fromemail: fromemail, subject: subject).WaitForResult();
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string recipient, string fromemail, string subject, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, recipient, fromemail, subject).WaitForResult();
        }

        public static PostmarkInboundMessageList GetInboundMessages(this PostmarkClient client,
            string recipient, string fromemail, string subject, string mailboxhash, int count, int offset)
        {
            return client.GetInboundMessagesAsync(offset, count, recipient, fromemail, subject, mailboxhash).WaitForResult();
        }

        public static InboundMessageDetail GetInboundMessageDetail(this PostmarkClient client, string messageID)
        {
            return client.GetInboundMessageDetailsAsync(messageID).WaitForResult();
        }
    }
}
