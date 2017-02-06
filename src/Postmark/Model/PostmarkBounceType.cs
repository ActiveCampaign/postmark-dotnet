namespace PostmarkDotNet
{
    /// <summary>
    /// Represents the type of Bounce.
    /// More details:
    /// - http://developer.postmarkapp.com/developer-api-bounce.html#bounce-types
    /// </summary>
    public enum PostmarkBounceType
    {
        /// <summary>
        /// Hard bounce — The server was unable to deliver your message (ex: unknown user, mailbox not found).
        /// </summary>
        HardBounce = 1,

        /// <summary>
        /// Message delayed — The server could not temporarily deliver your message (ex: Message is delayed due to network troubles).
        /// </summary>
        Transient = 2,

        /// <summary>
        /// Unsubscribe request — Unsubscribe or Remove request.
        /// </summary>
        Unsubscribe = 16,

        /// <summary>
        /// Subscribe request — Subscribe request from someone wanting to get added to the mailing list.
        /// </summary>
        Subscribe = 32,

        /// <summary>
        /// Auto responder — Automatic email responder (ex: "Out of Office" or "On Vacation").
        /// </summary>
        AutoResponder = 64,

        /// <summary>
        /// Address change — The recipient has requested an address change.
        /// </summary>
        AddressChange = 128,

        /// <summary>
        /// DNS error — A temporary DNS error.
        /// </summary>
        DnsError = 256,

        /// <summary>
        /// Spam notification — The message was delivered, but was either blocked by the user, or classified as spam, bulk mail, or had rejected content.
        /// </summary>
        SpamNotification = 512,

        /// <summary>
        /// Open relay test — The NDR is actually a test email message to see if the mail server is an open relay.
        /// </summary>
        OpenRelayTest = 1024,

        /// <summary>
        /// Unknown — Unable to classify the NDR.
        /// </summary>
        Unknown = 2048,

        /// <summary>
        /// Soft bounce — Unable to temporarily deliver message (i.e. mailbox full, account disabled, exceeds quota, out of disk space).
        /// </summary>
        SoftBounce = 4096,

        /// <summary>
        /// Virus notification — The bounce is actually a virus notification warning about a virus/code infected message.
        /// </summary>
        VirusNotification = 8192,

        /// <summary>
        /// Spam challenge verification — The bounce is a challenge asking for verification you actually sent the email. Typcial challenges are made by Spam Arrest, or MailFrontier Matador.
        /// </summary>
        ChallengeVerification = 16384,

        /// <summary>
        /// Invalid email address — The address is not a valid email address.
        /// </summary>
        BadEmailAddress = 100000,

        /// <summary>
        /// Spam complaint — The subscriber explicitly marked this message as spam.
        /// </summary>
        SpamComplaint = 100001,

        /// <summary>
        /// Manually deactivated — The email was manually deactivated.
        /// </summary>
        ManuallyDeactivated = 100002,

        /// <summary>
        /// Registration not confirmed — The subscriber has not clicked on the confirmation link upon registration or import.
        /// </summary>
        Unconfirmed = 100003,

        /// <summary>
        /// ISP block — Blocked from this ISP due to content or blacklisting.
        /// </summary>
        Blocked = 100006,

        /// <summary>
        /// SMTP API error — An error occurred while accepting an email through the SMTP API.
        /// </summary>
        SMTPApiError = 100007,

        /// <summary>
        /// Processing failed — Unable to deliver inbound message to destination inbound hook.
        /// </summary>
        InboundError = 100008,

        /// <summary>
        /// DMARC Policy — Email rejected due DMARC Policy.
        /// </summary>
        DMARCPolicy = 100009,

        /// <summary>
        /// Template rendering failed — An error occurred while attempting to render your template.
        /// </summary>
        TemplateRenderingFailed = 100010,
    }
}
