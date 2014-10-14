using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostmarkDotNet.Model;

namespace PostmarkDotNet
{
    public partial class PostmarkClient : IPostmarkClient
    {
        public Task<PostmarkResponse> SendMessageAsync(string @from, string to, string subject, string body)
        {
            var result = this.BeginSendMessage(@from, to, subject, body);
            return Task<PostmarkResponse>.Factory.FromAsync(result, this.EndSendMessage);
        }

        public Task<PostmarkResponse> SendMessageAsync(string @from, string to, string subject, string body, NameValueCollection headers)
        {
            var result = this.BeginSendMessage(@from, to, subject, body,headers);
            return Task<PostmarkResponse>.Factory.FromAsync(result, this.EndSendMessage);
        }

        public Task<PostmarkResponse> SendMessageAsync(PostmarkMessage message)
        {
            var result = this.BeginSendMessage(message);
            return Task<PostmarkResponse>.Factory.FromAsync(result, this.EndSendMessage);
        }

        public Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(IEnumerable<PostmarkMessage> messages)
        {
            var result = this.BeginSendMessages(messages);
            return Task<IEnumerable<PostmarkResponse>>.Factory.FromAsync( result, this.EndSendMessages);
        }

        public Task<IEnumerable<PostmarkResponse>> SendMessagesAsync(params PostmarkMessage[] messages)
        {
            var result = this.BeginSendMessages(messages);
            return Task<IEnumerable<PostmarkResponse>>.Factory.FromAsync(result, this.EndSendMessages);
        }

        public Task<PostmarkDeliveryStats> GetDeliveryStatsAsync()
        {
            var result = this.BeginGetDeliveryStats();
            return Task<PostmarkDeliveryStats>.Factory.FromAsync(result, this.EndGetDeliveryStats);
        }

        public Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            var result = this.BeginGetBounces(type,inactive,emailFilter,tag,offset,count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(bool? inactive, string emailFilter, string tag, int offset, int count)
        {
            var result = this.BeginGetBounces(inactive, emailFilter, tag, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, int offset, int count)
        {
            var result = this.BeginGetBounces(type, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(int offset, int count)
        {
            var result = this.BeginGetBounces(offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, int offset, int count)
        {
            var result = this.BeginGetBounces(type, inactive, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(bool? inactive, int offset, int count)
        {
            var result = this.BeginGetBounces(inactive,offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, string emailFilter, int offset, int count)
        {
            var result = this.BeginGetBounces(type, emailFilter, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(string emailFilter, int offset, int count)
        {
            var result = this.BeginGetBounces(emailFilter, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, string emailFilter, string tag, int offset, int count)
        {
            var result = this.BeginGetBounces(type, emailFilter, tag, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(string emailFilter, string tag, int offset, int count)
        {
            var result = this.BeginGetBounces(emailFilter, tag, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(PostmarkBounceType type, bool? inactive, string emailFilter, int offset, int count)
        {
            var result = this.BeginGetBounces(type, inactive, emailFilter, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounces> GetBouncesAsync(bool? inactive, string emailFilter, int offset, int count)
        {
            var result = this.BeginGetBounces(inactive, emailFilter, offset, count);
            return Task<PostmarkBounces>.Factory.FromAsync(result, this.EndGetBounces);
        }

        public Task<PostmarkBounce> GetBounceAsync(string bounceId)
        {
            var result = this.BeginGetBounce(bounceId);
            return Task<PostmarkBounce>.Factory.FromAsync(result, this.EndGetBounce);
        }

        public Task<IEnumerable<string>> GetBounceTagsAsync()
        {
            var result = this.BeginGetBounceTags();
            return Task<IEnumerable<string>>.Factory.FromAsync(result, this.EndGetBounceTags);
        }

        public Task<PostmarkBounceDump> GetBounceDumpAsync(string bounceId)
        {
            var result = this.BeginGetBounceDump(bounceId);
            return Task<PostmarkBounceDump>.Factory.FromAsync(result, this.EndGetBounceDump);
        }

        public Task<PostmarkBounceActivation> ActivateBounceAsync(string bounceId)
        {
            var result = this.BeginActivateBounce(bounceId);
            return Task<PostmarkBounceActivation>.Factory.FromAsync(result, this.EndActivateBounce);
        }

        public Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, string subject, int offset)
        {
            var result = this.BeginGetOutboundMessages(count,subject,offset);
            return Task<PostmarkOutboundMessageList>.Factory.FromAsync(result, this.EndGetOutboundMessages);
        }

        public Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, int offset, string recipient)
        {
            var result = this.BeginGetOutboundMessages(count, offset,recipient);
            return Task<PostmarkOutboundMessageList>.Factory.FromAsync(result, this.EndGetOutboundMessages);
        }

        public Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(int count, int offset)
        {
            var result = this.BeginGetOutboundMessages(count, offset);
            return Task<PostmarkOutboundMessageList>.Factory.FromAsync(result, this.EndGetOutboundMessages);
        }

        public Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string recipient, string fromemail, int count, int offset)
        {
            var result = this.BeginGetOutboundMessages(recipient,fromemail,count,offset);
            return Task<PostmarkOutboundMessageList>.Factory.FromAsync(result, this.EndGetOutboundMessages);
        }

        public Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string subject, int count, int offset)
        {
            var result = this.BeginGetOutboundMessages(subject, count, offset);
            return Task<PostmarkOutboundMessageList>.Factory.FromAsync(result, this.EndGetOutboundMessages);
        }

        public Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string fromemail, string tag, string subject, int count, int offset)
        {
            var result = this.BeginGetOutboundMessages(fromemail,tag, subject, count, offset);
            return Task<PostmarkOutboundMessageList>.Factory.FromAsync(result, this.EndGetOutboundMessages);
        }

        public Task<PostmarkOutboundMessageList> GetOutboundMessagesAsync(string recipient, string fromemail, string tag, string subject, int count, int offset)
        {
            var result = this.BeginGetOutboundMessages(recipient,fromemail, tag, subject, count, offset);
            return Task<PostmarkOutboundMessageList>.Factory.FromAsync(result, this.EndGetOutboundMessages);
        }

        public Task<OutboundMessageDetail> GetOutboundMessageDetailAsync(string messageID)
        {
            var result = this.BeginGetOutboundMessageDetail(messageID);
            return Task<OutboundMessageDetail>.Factory.FromAsync(result, this.EndGetOutboundMessageDetail);
        }

        public Task<MessageDump> GetOutboundMessageDumpAsync(string messageID)
        {
            var result = this.BeginGetOutboundMessageDump(messageID);
            return Task<MessageDump>.Factory.FromAsync(result, this.EndGetOutboundMessageDump);
        }

        public Task<PostmarkInboundMessageList> GetInboundMessagesAsync(int count, int offset)
        {
            var result = this.BeginGetInboundMessages(count,offset);
            return Task<PostmarkInboundMessageList>.Factory.FromAsync(result, this.EndGetInboundMessages);
        }

        public Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string fromemail, int count, int offset)
        {
            var result = this.BeginGetInboundMessages(fromemail,count, offset);
            return Task<PostmarkInboundMessageList>.Factory.FromAsync(result, this.EndGetInboundMessages);
        }

        public Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string fromemail, string subject, int count, int offset)
        {
            var result = this.BeginGetInboundMessages(fromemail,subject, count, offset);
            return Task<PostmarkInboundMessageList>.Factory.FromAsync(result, this.EndGetInboundMessages);
        }

        public Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string recipient, string fromemail, string subject, int count, int offset)
        {
            var result = this.BeginGetInboundMessages(recipient,fromemail, subject, count, offset);
            return Task<PostmarkInboundMessageList>.Factory.FromAsync(result, this.EndGetInboundMessages);
        }

        public Task<PostmarkInboundMessageList> GetInboundMessagesAsync(string recipient, string fromemail, string subject, string mailboxhash, int count,
            int offset)
        {
            var result = this.BeginGetInboundMessages(recipient, fromemail, subject,mailboxhash, count, offset);
            return Task<PostmarkInboundMessageList>.Factory.FromAsync(result, this.EndGetInboundMessages);
        }

        public Task<InboundMessageDetail> GetInboundMessageDetailAsync(string messageID)
        {
            var result = this.BeginGetInboundMessageDetail(messageID);
            return Task<InboundMessageDetail>.Factory.FromAsync(result, this.EndGetInboundMessageDetail);
        }
    }
}
