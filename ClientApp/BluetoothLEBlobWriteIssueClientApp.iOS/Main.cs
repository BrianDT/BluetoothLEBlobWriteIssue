// <copyright file="Main.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019, 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Foundation;
    using UIKit;

    /// <summary>
    /// The application root
    /// </summary>
    public class Application
    {
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args">Any parameters</param>
        public static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
