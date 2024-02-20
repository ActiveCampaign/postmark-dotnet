﻿using System;
using PostmarkDotNet;

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
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter<PostmarkBounceType>))]
        public MessageStreamType MessageStreamType { get; set; }

        /// <summary>
        /// The date when the message stream was created.
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date when the message stream was last updated. If null, this message stream was never updated.
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The date when this message stream has been archived. If null, this message stream is not in an archival state.
        /// </summary>
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        public DateTime? ArchivedAt { get; set; }
    }
}
