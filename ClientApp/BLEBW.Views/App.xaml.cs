// <copyright file="App.xaml.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// The root forms application
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            this.MainPage = new AppShell();
        }

        /// <summary>
        /// Override handler called on start
        /// </summary>
        protected override void OnStart()
        {
        }

        /// <summary>
        /// Override handler called on sleep
        /// </summary>
        protected override void OnSleep()
        {
        }

        /// <summary>
        /// Override handler called on resume
        /// </summary>
        protected override void OnResume()
        {
        }
    }
}
