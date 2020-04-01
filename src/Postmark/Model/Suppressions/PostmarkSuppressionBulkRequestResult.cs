using System.Collections.Generic;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Result of applying bulk Suppression requests.
    /// </summary>
    public class PostmarkSuppressionBulkRequestResult
    {
        public IEnumerable<PostmarkSuppressionRequestResult> Suppressions { get; set; }
    }
}
