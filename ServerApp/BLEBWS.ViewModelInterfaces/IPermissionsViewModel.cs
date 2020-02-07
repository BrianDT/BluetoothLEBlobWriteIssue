// <copyright file="IPermissionsViewModel.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.ViewModelInterfaces
{
    using System;
    using System.Threading.Tasks;
    using Vssl.Samples.ViewModelInterfaces;

    /// <summary>
    /// Requests the required permissions
    /// </summary>
    public interface IPermissionsViewModel : IBaseViewModel
    {
        /// <summary>
        /// Check that the permissions required by the application have been requested.
        /// </summary>
        /// <param name="promptInDialog">A dialog that displays the reason for the request</param>
        /// <returns>An awaitable task</returns>
        Task CheckPermissions(Func<string, Task> promptInDialog);
    }
}
