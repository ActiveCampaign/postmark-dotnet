using System.Threading.Tasks;
using PostmarkDotNet.Model;

namespace PostmarkDotNet
{
    public interface IPostmarkAdminClient
    {
        Task<PostmarkServer> GetServerAsync(int serverId);
        Task<PostmarkResponse> DeleteServerAsync(int serverId);
        Task<PostmarkServer> CreateServerAsync(string name, string color = null, bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null, string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null, bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null, LinkTrackingOptions? trackLinks = null, string clickHookUrl = null, string deliveryHookUrl = null, bool? enableSmtpApiErrorHooks = null, string deliveryType = null);
        Task<PostmarkServer> EditServerAsync(int serverId, string name = null, string color = null, bool? rawEmailEnabled = null, bool? smtpApiActivated = null, string inboundHookUrl = null, string bounceHookUrl = null, string openHookUrl = null, bool? postFirstOpenOnly = null, bool? trackOpens = null, string inboundDomain = null, int? inboundSpamThreshold = null, LinkTrackingOptions? trackLinks = null, string clickHookUrl = null, string deliveryHookUrl = null, bool? enableSmtpApiErrorHooks = null);
        Task<PostmarkServerList> GetServersAsync(int offset = 0, int count = 100, string name = null);
        Task<PostmarkSenderSignatureList> GetSenderSignaturesAsync(int offset = 0, int count = 100);
        Task<PostmarkCompleteSenderSignature> GetSenderSignatureAsync(int signatureId);
        Task<PostmarkResponse> DeleteSignatureAsync(int signatureId);
        Task<PostmarkResponse> ResendSignatureVerificationEmailAsync(int signatureId);
        Task<PostmarkResponse> RequestNewSignatureDKIMAsync(int signatureId);
        Task<PostmarkCompleteSenderSignature> CreateSignatureAsync(string fromEmail, string name, string replyToEmail = null, string returnPathDomain = null, string confirmationPersonalNote = null);
        Task<PostmarkCompleteSenderSignature> UpdateSignatureAsync(int signatureId, string name = null, string replyToEmail = null, string returnPathDomain = null, string confirmationPersonalNote = null);
        Task<PostmarkDomainList> GetDomainsAsync(int offset = 0, int count = 100);
        Task<PostmarkCompleteDomain> GetDomainAsync(int domainId);
        Task<PostmarkResponse> DeleteDomainAsync(int domainId);
        Task<PostmarkResponse> RequestNewDomainDKIMAsync(int domainId);
        Task<PostmarkCompleteDomain> CreateDomainAsync(string name, string returnPathDomain = null);
        Task<PostmarkCompleteDomain> UpdateDomainAsync(int domainId, string returnPathDomain);
        Task<PostmarkCompleteDomain> VerifyDomainDkim(int domainId);
        Task<PostmarkCompleteDomain> VerifyDomainReturnPath(int domainId);
        Task<PostmarkDataRemoval> RequestDataRemoval(string requestedBy, string requestedFor, bool notifyWhenCompleted);
        Task<PostmarkDataRemoval> GetDataRemovalStatus(long id);
        Task<PostmarkCompleteSenderSignature> VerifySignatureSPF(int signatureId);
    }
}