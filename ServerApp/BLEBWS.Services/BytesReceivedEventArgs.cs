// <copyright file="BytesReceivedEventArgs.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BLEBWS.ServiceInterfaces;

    /// <summary>
    /// Notify that image bytes have been received from the client
    /// </summary>
    public class BytesReceivedEventArgs : IBytesReceivedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BytesReceivedEventArgs" /> class.
        /// </summary>
        /// <param name="byteCount">The number of bytes transferred from the given client</param>
        /// <param name="clientId">The client id</param>
        public BytesReceivedEventArgs(long byteCount, Guid clientId)
        {
            this.ByteCount = byteCount;
            this.ClientId = clientId;
        }

        /// <summary>
        /// Gets the number of bytes transferred from the given client
        /// </summary>
        public long ByteCount { get; private set; }

        /// <summary>
        /// Gets the client id
        /// </summary>
        public Guid ClientId { get; private set; }
    }
}
