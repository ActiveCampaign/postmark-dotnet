using System;
namespace PostmarkDotNet.Model
{
    public class PostmarkBaseDomain
    {
        public int ID { get; set; }
        public string Name { get; set; }
        [Obsolete("This property is no longer used because we do not require SPF Verification")]
        public bool SPFVerified { get; set; }
        public bool DKIMVerified { get; set; }
        public bool WeakDKIM { get; set; }
        public bool ReturnPathDomainVerified { get; set; }
    }
}
