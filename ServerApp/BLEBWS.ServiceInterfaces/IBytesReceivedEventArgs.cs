// <copyright file="IBytesReceivedEventArgs.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Notify that image bytes have been received from the client
    /// </summary>
    public interface IBytesReceivedEventArgs
    {
        /// <summary>
        /// Gets the number of bytes transferred from the given client
        /// </summary>
        long ByteCount { get; }

        /// <summary>
        /// Gets the client id
        /// </summary>
        Guid ClientId { get; }
    }
}
