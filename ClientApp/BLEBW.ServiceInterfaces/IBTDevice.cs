// <copyright file="IBTDevice.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BluetoothLEBlobWriteIssue.Models;

    /// <summary>
    /// A cross platform representation of a bluetooth device
    /// </summary>
    public interface IBTDevice
    {
        /// <summary>
        /// Notify that images can be sent
        /// </summary>
        event Action NotifyCanSendImages;

        /// <summary>
        /// Notify that the device has disconnected
        /// </summary>
        event Action NotifyDisconnected;

        /// <summary>
        /// Actions taken when the device connects
        /// </summary>
        void OnConnected();

        /// <summary>
        /// Write an image to the output stream
        /// </summary>
        /// <param name="image">The image to transmit</param>
        /// <returns>An awaitable task</returns>
        Task Write(Image image);
    }
}
