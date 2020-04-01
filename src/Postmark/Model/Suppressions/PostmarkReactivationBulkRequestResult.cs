using System.Collections.Generic;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Result of applying bulk Reactivation requests.
    /// </summary>
    public class PostmarkReactivationBulkRequestResult
    {
        public IEnumerable<PostmarkReactivationRequestResult> Suppressions { get; set; }
    }
}
