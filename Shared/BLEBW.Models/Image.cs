// <copyright file="Image.cs" company="TFL.">Copyright (c) 2017 - 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssue.Models
{
    using System;
    using ProtoBuf;

    /// <summary>
    /// An image model written to local storage and used for cross app transport
    /// </summary>
    [ProtoContract]
    public class Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image" /> class.
        /// </summary>
        public Image()
        {
            this.CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the record identifier and database primary key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the device that supplied the image
        /// </summary>
        [ProtoMember(2)]
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the image
        /// </summary>
        [ProtoMember(3)]
        public byte[] BinaryData { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier
        /// </summary>
        [ProtoMember(1)]
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the date the model was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date the model was deleted
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
