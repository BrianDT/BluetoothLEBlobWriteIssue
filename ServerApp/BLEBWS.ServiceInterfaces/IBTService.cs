// <copyright file="IBTService.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.ServiceInterfaces
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The Bluetooth LE service manager
    /// </summary>
    public interface IBTService
    {
        /// <summary>
        /// Notify that a new image has been received
        /// </summary>
        event EventHandler<INewImageEventArgs> NotifyNewImage;

        /// <summary>
        /// Notify that image bytes have been received from the client
        /// </summary>
        event EventHandler<IBytesReceivedEventArgs> NotifyBytesReceived;

        /// <summary>
        /// Gets a value indicating whether the server is started or is starting
        /// </summary>
        bool IsStartedOrStarting { get; }

        /// <summary>
        /// Check that the preconditions for server start are set
        /// </summary>
        /// <returns>True if a server can be started</returns>
        bool CanStartServer();

        /// <summary>
        /// Start a LE server and pass the identifier to the client manager
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task StartServer();

        /// <summary>
        /// Start the advertiser
        /// </summary>
        void StartAdvertiser();

        /// <summary>
        /// Stop the server
        /// </summary>
        void StopServer();
    }
}
