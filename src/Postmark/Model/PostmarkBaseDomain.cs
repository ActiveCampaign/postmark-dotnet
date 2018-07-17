
namespace PostmarkDotNet.Model
{
    public class PostmarkBaseDomain
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool SPFVerified { get; set; }
        public bool DKIMVerified { get; set; }
        public bool WeakDKIM { get; set; }
        public bool ReturnPathDomainVerified { get; set; }
    }
}
