namespace PostmarkDotNet
{
    public enum InboundMessageStatus
    {
        Queued = 1,
        Failed = 2,
        Blocked = 4,
        Processed = 8
    }
}
