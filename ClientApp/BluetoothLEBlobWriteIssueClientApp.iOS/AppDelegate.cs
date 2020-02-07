// <copyright file="AppDelegate.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Foundation;
    using UIKit;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the 
    /// User Interface of the application, as well as listening (and optionally responding) to 
    /// application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        /// <summary>
        /// This method is invoked when the application has loaded and is ready to run. In this 
        /// method you should instantiate the window, load the UI into it and then make the window
        /// visible.
        /// </summary>
        /// <param name="application">Reference to the UIApplication that invoked this delegate method</param>
        /// <param name="launchOptions">An NSDictionary with the launch options, can be null. Possible key values are UIApplication's LaunchOption static properties.</param>
        /// <returns>True if finished OK</returns>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var bootstrap = new Bootstrapper();
            IDependencyResolver container = bootstrap.Startup();

            global::Xamarin.Forms.Forms.Init();
            this.LoadApplication(new App());

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}
