// <copyright file="AppHelper.cs" company="TFL.">Copyright (c) 2019, 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueServerApp.Droid.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;

    /// <summary>
    /// Retains the app reference that is not dependent on any particular app
    /// </summary>
    public static class AppHelper
    {
        /// <summary>
        /// Gets or sets the application context
        /// </summary>
        public static Context AppContext { get; set; }
    }
}