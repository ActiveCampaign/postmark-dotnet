namespace PostmarkDotNet
{
    /// <summary>
    ///   Represents the type of bounce for a <see cref = "PostmarkBounce" />.
    /// </summary>
    public enum PostmarkBounceType
    {
        /// <summary>
        ///   HardBounce
        /// </summary>
        HardBounce,
        /// <summary>
        ///   Transient
        /// </summary>
        Transient,
        /// <summary>
        ///   Unsubscribe
        /// </summary>
        Unsubscribe,
        /// <summary>
        ///   Subscribe
        /// </summary>
        Subscribe,
        /// <summary>
        ///   AutoResponder
        /// </summary>
        AutoResponder,
        /// <summary>
        ///   AddressChange
        /// </summary>
        AddressChange,
        /// <summary>
        ///   DnsError
        /// </summary>
        DnsError,
        /// <summary>
        ///   SpamNotification
        /// </summary>
        SpamNotification,
        /// <summary>
        ///   OpenRelayTest
        /// </summary>
        OpenRelayTest,
        /// <summary>
        ///   Unknown
        /// </summary>
        Unknown,
        /// <summary>
        ///   SoftBounce
        /// </summary>
        SoftBounce,
        /// <summary>
        ///   VirusNotification
        /// </summary>
        VirusNotification,
        /// <summary>
        ///   ChallengeVerification
        /// </summary>
        ChallengeVerification,
        /// <summary>
        ///   BadEmailAddress
        /// </summary>
        BadEmailAddress,
        /// <summary>
        ///   SpamComplaint
        /// </summary>
        SpamComplaint,
        /// <summary>
        ///   ManuallyDeactivated
        /// </summary>
        ManuallyDeactivated,
        /// <summary>
        ///   Unconfirmed
        /// </summary>
        Unconfirmed,
        /// <summary>
        ///   Blocked
        /// </summary>
        Blocked,
        ///
        ///  SMTPApiError
        /// 
        SMTPApiError,
        /// <summary>
        /// AOL Spam Notification
        /// </summary>
        AOLSpamNotification
    }
}