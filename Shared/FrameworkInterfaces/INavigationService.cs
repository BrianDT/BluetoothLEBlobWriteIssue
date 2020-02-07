// <copyright file="INavigationService.cs" company="Visual Software Systems Ltd.">Copyright (c) 2012 All rights reserved</copyright>

namespace Vssl.Samples.FrameworkInterfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Controls the navigation to a new page
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Initialise the navigation service with a native navigation object
        /// </summary>
        /// <param name="nativeObject">The native navigation object</param>
        void Initialise(object nativeObject);

        /// <summary>
        /// Navigate to a new page
        /// </summary>
        /// <param name="viewInterface">The interface of the view to be navigated to</param>
        void GoTo(Type viewInterface);

        /// <summary>
        /// Popup a forms page
        /// </summary>
        /// <param name="viewInterface">The interface of the dialog to popup</param>
        /// <param name="handler">A completion handler</param>
        /// <returns>An awaitable task</returns>
        Task Popup(Type viewInterface, Action handler);

        /// <summary>
        /// Popup a native page
        /// </summary>
        /// <param name="viewObject">A native view</param>
        /// <param name="handler">A completion handler</param>
        void Popup(object viewObject, Action handler);

        /// <summary>
        /// Navigate back to the previous view
        /// </summary>
        /// <returns>True if can go back</returns>
        bool GoBack();
    }
}
