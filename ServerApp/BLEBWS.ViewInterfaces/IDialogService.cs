// <copyright file="IDialogService.cs" company="TFL.">Copyright (c) 2018, 2019, 2020 All rights reserved</copyright>

namespace BLEBWS.ViewInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The interface to the platform specific dialog presenter
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Display a status toast
        /// </summary>
        /// <param name="message">The message to display</param>
        void Notice(string message);
    }
}
