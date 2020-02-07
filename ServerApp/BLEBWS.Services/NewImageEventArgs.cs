// <copyright file="NewImageEventArgs.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BLEBWS.ServiceInterfaces;
    using BluetoothLEBlobWriteIssue.Models;

    /// <summary>
    /// The event args of the new image event
    /// </summary>
    public class NewImageEventArgs : EventArgs, INewImageEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewImageEventArgs" /> class.
        /// </summary>
        /// <param name="image">The image model</param>
        public NewImageEventArgs(Image image)
        {
            this.Image = image;
        }

        /// <summary>
        /// Gets the image model
        /// </summary>
        public Image Image { get; private set; }
    }
}
