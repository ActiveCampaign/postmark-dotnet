using System.Collections.Generic;

namespace PostmarkDotNet
{
    /// <summary>
    ///   Represents a partial result of bounces obtained from a search.
    /// </summary>
    public class PostmarkBounces
    {
        /// <summary>
        ///   The total number of bounces logged by the server.
        ///   Use this number to base paging parameters in subsequent
        ///   calls to retrieve bounces.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount { get; set; }

        /// <summary>
        ///   The bounces returned in this query result.
        /// </summary>
        /// <value>The bounces.</value>
        public List<PostmarkBounce> Bounces { get; set; }
    }
}