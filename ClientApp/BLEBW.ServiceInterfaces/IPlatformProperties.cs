// <copyright file="IPlatformProperties.cs" company="TFL.">Copyright (c) 2019 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides platform specific hardware interactions
    /// </summary>
    public interface IPlatformProperties
    {
        /// <summary>
        /// An event that notifies an image taken with the camera
        /// </summary>
        event Func<byte[], Task> NotifyPicked;

        /// <summary>
        /// An event that notifies that the pick was cancelled
        /// </summary>
        event Action PickCancelled;

        /// <summary>
        /// Gets a value indicating whether the camera is available and accessible
        /// </summary>
        bool IsCameraAvailable { get; }

        /// <summary>
        /// Put up the camera app
        /// </summary>
        void GotoCamera();

        /// <summary>
        /// Pick an image from the photo library
        /// </summary>
        void GotoLibrary();
    }
}
