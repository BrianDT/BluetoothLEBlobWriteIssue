// <copyright file="MainPage.xaml.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BLEBW.ViewInterfaces;
    using BluetoothLEBlobWriteIssue.ViewModelInterfaces;
    using Vssl.Samples.Framework;
    using Vssl.Samples.FrameworkInterfaces;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer

    /// <summary>
    /// The content for the image picker page
    /// </summary>
    [DesignTimeVisible(false)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, IMainPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.BindingContext = DependencyHelper.Resolve<IImagePickerViewModel>();
        }

        /// <summary>
        /// Gets the data context cast as the view model interface
        /// </summary>
        public IImagePickerViewModel VM
        {
            get
            {
                return this.BindingContext as IImagePickerViewModel;
            }
        }

        /// <summary>
        /// Event override called when the view is displayed
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.VM.OnViewLoaded();
        }
    }
}
