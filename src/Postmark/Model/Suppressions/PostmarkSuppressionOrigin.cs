namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Origin that generated a Suppression
    /// </summary>
    public enum PostmarkSuppressionOrigin
    {
        Customer, SpamComplaintAuthorizedCustomer, Recipient, Admin
    }
}
