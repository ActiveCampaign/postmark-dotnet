using System.Collections.Generic;

namespace PostmarkDotNet
{
    /// <summary>
    /// Represents delivery statistics at a particular time.
    /// </summary>
    public class PostmarkDeliveryStats
    {
        /// <summary>
        /// The total number of inactive or bounced mails delivered.
        /// </summary>
        /// <value>The inactive mails.</value> 
        public int InactiveMails { get; set; }

        /// <summary>
        /// The list of summary results for the bounces in this report.
        /// </summary>
        /// <value>The bounces.</value>
        public List<PostmarkBounceSummary> Bounces { get; set; }
    }
}
