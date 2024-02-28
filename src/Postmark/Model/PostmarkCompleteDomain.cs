
namespace PostmarkDotNet.Model
{
    public class PostmarkCompleteDomain : PostmarkBaseDomain
    {
        public string SPFHost { get; set; }
        public string SPFTextValue { get; set; }
        public string DKIMHost { get; set; }
        public string DKIMTextValue { get; set; }
        public string DKIMPendingHost { get; set; }
        public string DKIMPendingTextValue { get; set; }
        public string DKIMRevokedHost { get; set; }
        public string DKIMRevokedTextValue { get; set; }
        public string DKIMUpdateStatus { get; set; }
        public string ReturnPathDomain { get; set; }
        public string ReturnPathDomainCNAMEValue { get; set; }
        public bool SafeToRemoveRevokedKeyFromDNS { get; set; }
    }
}