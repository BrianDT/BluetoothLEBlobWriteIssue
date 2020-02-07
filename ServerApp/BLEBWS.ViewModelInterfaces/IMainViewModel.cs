// <copyright file="IMainViewModel.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.ViewModelInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Vssl.Samples.ViewModelInterfaces;

    /// <summary>
    /// Displays the last transferred image
    /// </summary>
    public interface IMainViewModel : IBaseViewModel
    {
        /// <summary>
        /// Gets the last image transmitted from the client
        /// </summary>
        byte[] LastImage { get; }

        /// <summary>
        /// Gets the number of bytes received from the client
        /// </summary>
        string BytesReceived { get; }

        /// <summary>
        /// Called when the page is navigated to
        /// </summary>
        void OnNavigatedTo();
    }
}
