namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// A request to suppress or reactivate one recipient.
    /// </summary>
    public class PostmarkSuppressionChangeRequest
    {
        /// <summary>
        /// Address of the recipient whose suppression status should be changed.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
