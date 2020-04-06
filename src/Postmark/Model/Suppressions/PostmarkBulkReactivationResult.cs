using System.Collections.Generic;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Result of applying Reactivation requests in bulk.
    /// </summary>
    public class PostmarkBulkReactivationResult
    {
        public IEnumerable<PostmarkReactivationRequestResult> Suppressions { get; set; }
    }
}
