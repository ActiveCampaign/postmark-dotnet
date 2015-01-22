
namespace PostmarkDotNet.Model
{
    public class PostmarkBaseSenderSignature
    {
        public int ID { get; set; }
        public string Domain { get; set; }
        public string EmailAddress { get; set; }
        public string ReplyToEmailAddress { get; set; }
        public string Name { get; set; }
        public bool Confirmed { get; set; }
    }
}
