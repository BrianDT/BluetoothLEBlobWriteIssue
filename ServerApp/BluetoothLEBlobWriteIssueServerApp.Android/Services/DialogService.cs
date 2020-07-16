// <copyright file="DialogService.cs" company="TFL.">Copyright (c) 2018, 2019, 2020 All rights reserved</copyright>

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
    using BLEBWS.ViewInterfaces;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// A wrapper round the platform specific dialog presenter
    /// </summary>
    public class DialogService : IDialogService
    {
        /// <summary>
        /// The dispatcher that marshals actions back onto the UI thread
        /// </summary>
        private IDispatchOnUIThread dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService" /> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher that marshals actions back onto the UI thread</param>
        public DialogService(IDispatchOnUIThread dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        /// <summary>
        /// Display a status toast
        /// </summary>
        /// <param name="message">The message to display</param>
        public void Notice(string message)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            this.dispatcher.Invoke(() => Toast.MakeText(activity, message, ToastLength.Long).Show());
        }
    }
}