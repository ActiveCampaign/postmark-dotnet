using System.Collections.Generic;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Response model for listing Suppressions.
    /// </summary>
    public class PostmarkSuppressionListing
    {
        /// <summary>
        /// Active Suppressions
        /// </summary>
        public IEnumerable<PostmarkSuppression> Suppressions { get; set; }
    }
}
