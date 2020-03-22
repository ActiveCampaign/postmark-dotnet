namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Reason why a recipient is suppressed
    /// </summary>
    public enum PostmarkSuppressionReason
    {
        HardBounce, ManualSuppression, SpamComplaint
    }
}
