using System;

namespace Postmark.Model.Suppressions
{
    public class PostmarkSuppressionQuery
    {
        /// <summary>
        /// Filter Suppressions by MessageStream. (optional)
        /// </summary>
        /// <remarks>If not provided, Suppressions for the entire Server will be returned.</remarks>
        public string MessageStream { get; set; }

        /// <summary>
        /// Filter Suppressions by reason. (optional)
        /// </summary>
        /// <remarks>If not provided, Suppressions for all reasons will be returned.</remarks>
        public string SuppressionReason { get; set; }

        /// <summary>
        /// Filter Suppressions by the origin that created them. (optional)
        /// </summary>
        /// <remarks>If not provided, Suppressions for all origins will be returned.</remarks>
        public string Origin { get; set; }

        /// <summary>
        /// Filter suppressions up to the date specified - inclusive. (optional)
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Filter suppressions from the date specified - inclusive. (optional)
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Filter by a specific email address. (optional)
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
