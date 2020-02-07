// <copyright file="IBTClient.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BluetoothLEBlobWriteIssue.Models;

    /// <summary>
    /// A sample Bluetooth client service
    /// </summary>
    public interface IBTClient
    {
        /// <summary>
        /// Notify that the service is available
        /// </summary>
        event Action<bool> NotifyServiceAvailable;

        /// <summary>
        /// Notify that images can be sent
        /// </summary>
        event Action NotifyCanSendImages;

        /// <summary>
        /// Notify that the connected device has disconnected
        /// </summary>
        event Action NotifyDisconnected;

        /// <summary>
        /// Start a background scan of bluetooth devices before the filter is known
        /// </summary>
        void StartBackgroundDeviceScan();

        /// <summary>
        /// Connect to the defined service
        /// </summary>
        /// <returns>True if connected</returns>
        Task<bool> Connect();

        /// <summary>
        /// Sends an image from the client to the server
        /// </summary>
        /// <param name="image">The image model</param>
        /// <returns>An awaitable task</returns>
        Task SendImage(Image image);
    }
}
