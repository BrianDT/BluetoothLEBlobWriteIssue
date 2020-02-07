// <copyright file="AppShell.xaml.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueServerApp.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BLEBWS.ViewInterfaces;
    using Vssl.Samples.Framework;
    using Vssl.Samples.FrameworkInterfaces;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// The code behind the shell view
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        /// <summary>
        /// The index of navigation routes
        /// </summary>
        private Dictionary<string, Type> routes = new Dictionary<string, Type>();

        /// <summary>
        /// The navigation service that is initialised on the first navigation
        /// </summary>
        private INavigationService navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppShell" /> class.
        /// </summary>
        public AppShell()
        {
            this.InitializeComponent();
            this.RegisterRoutes();
            this.BindingContext = this;

            this.content.Route = this.InitialPage;
            this.imageDisplay.Route = typeof(IMainPage).Name;
        }

        /// <summary>
        /// Gets the text key to the initial page
        /// </summary>
        public string InitialPage
        {
            get
            {
                return typeof(IPermissionsPage).Name;
            }
        }

        /// <summary>
        /// Register the navigation routes and navigate to the main page
        /// </summary>
        private void RegisterRoutes()
        {
            /*
            this.routes.Add(typeof(IMainPage).Name, typeof(MainPage));

            foreach (var item in this.routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
            */
        }

        /// <summary>
        /// Event handler for navigation
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            //// Cancel any back navigation
            ////if (e.Source == ShellNavigationSource.Pop)
            ////{
            ////    e.Cancel();
            ////}
        }

        /// <summary>
        /// Event handler for navigation completion
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            if (this.navigationService == null && Shell.Current != null)
            {
                this.navigationService = DependencyHelper.Resolve<INavigationService>();
                this.navigationService.Initialise(Shell.Current);
            }
        }
    }
}