namespace PostmarkDotNet
{
    public enum PostmarkBounceType
    {
        HardBounce,
        Transient,
        Unsubscribe,
        Subscribe,
        AutoResponder,
        AddressChange,
        DnsError,
        SpamNotification,
        OpenRelayTest,
        Unknown,
        SoftBounce,
        VirusNotification,
        ChallengeVerification,
        BadEmailAddress,
        SpamComplaint,
        ManuallyDeactivated,
        Unconfirmed,
        Blocked
    }
}