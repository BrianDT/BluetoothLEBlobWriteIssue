// <copyright file="IPageBaseViewModel.cs" company="TFL.">Copyright (c) 2018 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssue.ViewModelInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Properties of a page
    /// </summary>
    public interface IPageBaseViewModel
    {
        /// <summary>
        /// Gets the text for the tool bar
        /// </summary>
        string PageHeader { get; }
    }
}
