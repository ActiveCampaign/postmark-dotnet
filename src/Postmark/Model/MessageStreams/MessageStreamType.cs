namespace Postmark.Model.MessageStreams
{
    /// <summary>
    /// Valid types for a message stream.
    /// </summary>
    public enum MessageStreamType
    {
        Transactional = 0,
        Inbound = 1,
        Broadcasts = 2
    }
}
