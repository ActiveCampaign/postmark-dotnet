namespace PostmarkDotNet
{
    /// <summary>
    ///   Represents an aggregate view of bounces.
    /// </summary>
    public class PostmarkBounceSummary
    {
        /// <summary>
        ///   An summary for a <see cref = "PostmarkBounceType" />.
        /// </summary>
        /// <value>The type.</value>
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<PostmarkBounceType>))]
        public PostmarkBounceType Type { get; set; }

        /// <summary>
        ///   The name of the summary.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        ///   The number of results in the summary.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get; set; }
    }
}