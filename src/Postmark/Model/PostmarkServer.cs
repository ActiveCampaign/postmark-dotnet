using System;
using System.Collections.Generic;

namespace PostmarkDotNet.Model
{
    /// <summary>
    /// Represents a virtual mail server from which mail 
    /// is sent and received.
    /// </summary>
    public class PostmarkServer
    {
        public PostmarkServer()
        {
            ApiTokens = new List<String>(0);
        }

        /// <summary>
        /// Name of server
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Name of server
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of API tokens associated with server.
        /// </summary>
        public List<String> ApiTokens { get; set; }

        /// <summary>
        /// URL to your server overview page in Postmark.
        /// </summary>
        public string ServerLink { get; set; }

        /// <summary>
        /// Color of the server in the rack screen.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Specifies whether or not SMTP is enabled on this server.
        /// </summary>
        public bool SmtpApiActivated { get; set; }

        /// <summary>
        /// Allow raw email to be sent with inbound.
        /// </summary>
        public bool RawEmailEnabled { get; set; }
        
        /// <summary>
        /// Specifies the type of environment for your server. Possible options: Live, Sandbox. Defaults to Live if not specified. This cannot be changed after the server has been created.
        /// </summary>
        public string DeliveryType { get; set; }

        /// <summary>
        /// Inbound email address
        /// </summary>
        public string InboundAddress { get; set; }

        /// <summary>
        /// URL to POST to everytime an inbound event occurs.
        /// </summary>
        public string InboundHookUrl { get; set; }

        /// <summary>
        /// URL to POST to everytime a bounce event occurs.
        /// </summary>
        public string BounceHookUrl { get; set; }

        /// <summary>
        /// URL to POST to everytime an open event occurs.
        /// </summary>
        public string OpenHookUrl { get; set; }

        /// <summary>
        /// If set to true, only the first open by a particular recipient will initiate the open webhook. Any subsequent opens of the same email by the same recipient will not initiate the webhook.
        /// </summary>
        public bool PostFirstOpenOnly { get; set; }

        /// <summary>
        /// Indicates if all emails being sent through this server have open tracking enabled.
        /// </summary>
        public bool TrackOpens { get; set; }

        /// <summary>
        /// The link tracking setting for this server.
        /// </summary>
        public LinkTrackingOptions TrackLinks { get; set; }

        /// <summary>
        /// URL to POST to everytime an click event occurs.
        /// </summary>
        public string ClickHookUrl { get; set; }

        /// <summary>
        /// URL to POST to when messages delivery occurs.
        /// </summary>
        public string DeliveryHookUrl { get; set; }

        /// <summary>
        /// Inbound domain for MX setup
        /// </summary>
        public string InboundDomain { get; set; }

        /// <summary>
        /// The inbound hash of your inbound email address.
        /// </summary>
        public string InboundHash { get; set; }

        /// <summary>
        /// The maximum spam score for an inbound message before it's blocked.
        /// </summary>
        public int InboundSpamThreshold { get; set; }

        /// <summary>
        /// Include SMTP API Errors in error webhooks.
        /// </summary>
        public bool EnableSmtpApiErrorHooks { get; set; }

    }

    /// <summary>
    /// The colors shown by the Postmark server interface.
    /// </summary>
    public class ServerColors
    {
        public static string Grey { get { return "ggrey"; } }
        public static string Purple { get { return "purple"; } }
        public static string Blue { get { return "blue"; } }
        public static string Turqoise { get { return "turqoise"; } }
        public static string Green { get { return "green"; } }
        public static string Red { get { return "red"; } }
        public static string Yellow { get { return "yellow"; } }
    }

}
