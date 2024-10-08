﻿using System;

namespace Postmark.Model.MessageStreams
{
    /// <summary>
    /// Model representing a message stream.
    /// For more information about the MessageStreams API, please visit our API documentation.
    /// </summary>
    public class PostmarkMessageStream
    {
        /// <summary>
        /// User defined identifier for this message stream that is unique at the server level.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Id of the server this stream belongs to.
        /// </summary>
        public int ServerID { get; set; }

        /// <summary>
        /// Friendly name of the message stream.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Friendly description of the message stream.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of this message Stream. Can be Transactional, Inbound or Broadcasts.
        /// </summary>
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<MessageStreamType>))]
        public MessageStreamType MessageStreamType { get; set; }

        /// <summary>
        /// The date when the message stream was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date when the message stream was last updated. If null, this message stream was never updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The date when this message stream has been archived. If null, this message stream is not in an archival state.
        /// </summary>
        public DateTime? ArchivedAt { get; set; }

        /// <summary>
        /// Subscription management options for the Stream.
        /// </summary>
        public PostmarkSubscriptionManagementConfiguration SubscriptionManagementConfiguration { get; set; }
    }
}
