using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    public class PostmarkSenderSignatureList
    {
        public int TotalCount { get; set; }
        public IEnumerable<PostmarkBaseSenderSignature> SenderSignatures { get; set; }
    }
}
