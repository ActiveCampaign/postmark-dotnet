﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Postmark.Model.Suppressions
{
    /// <summary>
    /// Model representing a Suppressed recipient.
    /// For more information about the Suppressions API and possible options, please visit:
    /// https://postmarkapp.com/developer/api/suppressions-api
    /// </summary>
    public class PostmarkSuppression
    {
        /// <summary>
        /// Email address of the suppressed recipient.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Reason why this recipient was suppressed. E.g.: ManualSuppression, HardBounce, SpamComplaint.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PostmarkSuppressionReason SuppressionReason { get; set; }

        /// <summary>
        /// Origin that suppressed this recipient. E.g.: Customer, Recipient, Admin.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PostmarkSuppressionOrigin Origin { get; set; }

        /// <summary>
        /// Date when the suppression was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
