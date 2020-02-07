// <copyright file="INewImageEventArgs.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.ServiceInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BluetoothLEBlobWriteIssue.Models;

    /// <summary>
    /// The event args of the new image event
    /// </summary>
    public interface INewImageEventArgs
    {
        /// <summary>
        /// Gets the image model
        /// </summary>
        Image Image { get; }
    }
}
