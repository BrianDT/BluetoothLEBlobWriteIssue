// <copyright file="ServerState.cs" company="TFL.">Copyright (c) 2019 All rights reserved</copyright>

namespace BLEBWS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// An enumeration of server states
    /// </summary>
    public enum ServerState
    {
        /// <summary>
        /// Not yet started
        /// </summary>
        NotStarted = 0,

        /// <summary>
        ///  Starting up
        /// </summary>
        Starting = 1,

        /// <summary>
        /// Started OK
        /// </summary>
        Started = 2,

        /// <summary>
        /// Stopped OK
        /// </summary>
        Stopped = 3,

        /// <summary>
        /// Can't be started
        /// </summary>
        NotStartable = 4,

        /// <summary>
        /// Failed to start
        /// </summary>
        FailedToStart = 5
    }
}
