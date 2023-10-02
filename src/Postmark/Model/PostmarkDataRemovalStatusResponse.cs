namespace PostmarkDotNet.Model
{
    public class PostmarkDataRemovalStatusResponse
    {
        /// <summary>
        ///   The Data Removal ID.
        /// </summary>
        public long ID { get; set; }
        
        /// <summary>
        ///   The status of Data Removal request. 
        /// </summary>
        public PostmarkDataRemovalStatus Status { get; set; }

        /// <summary>
        ///   The error code returned from Postmark.
        ///   This does not map to HTTP status codes.
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        ///   The message from the API.  
        ///   In the event of an error, this message may contain helpful text.
        /// </summary>
        public string Message { get; set; }
    }
}