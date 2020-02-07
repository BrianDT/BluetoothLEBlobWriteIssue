// <copyright file="PermissionsPage.xaml.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueServerApp.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BLEBWS.ViewInterfaces;
    using BLEBWS.ViewModelInterfaces;
    using Vssl.Samples.Framework;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// The content for the permissions page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PermissionsPage : ContentPage, IPermissionsPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsPage" /> class.
        /// </summary>
        public PermissionsPage()
        {
            this.InitializeComponent();
            this.BindingContext = DependencyHelper.Resolve<IPermissionsViewModel>();
        }

        /// <summary>
        /// Gets the data context cast as the view model interface
        /// </summary>
        public IPermissionsViewModel VM
        {
            get
            {
                return this.BindingContext as IPermissionsViewModel;
            }
        }

        /// <summary>
        /// Event override called when the view is displayed
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.VM.CheckPermissions(this.PromptWithReason);
        }

        /// <summary>
        /// A function used to prompt with a reason for the permission request
        /// </summary>
        /// <param name="reasonText">The reason text</param>
        /// <returns>A awaitable task</returns>
        private async Task PromptWithReason(string reasonText)
        {
            await this.DisplayAlert("Permission required", reasonText, "OK");
        }
    }
}