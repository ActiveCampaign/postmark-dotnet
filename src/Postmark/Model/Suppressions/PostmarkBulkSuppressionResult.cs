using System.Collections.Generic;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Result of applying Suppression requests in bulk.
    /// </summary>
    public class PostmarkBulkSuppressionResult
    {
        public IEnumerable<PostmarkSuppressionRequestResult> Suppressions { get; set; }
    }
}
