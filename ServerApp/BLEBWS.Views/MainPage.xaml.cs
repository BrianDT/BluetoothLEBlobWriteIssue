// <copyright file="MainPage.xaml.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

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
    /// The content for the main image display page
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, IMainPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.BindingContext = DependencyHelper.Resolve<IMainViewModel>();
        }

        /// <summary>
        /// Gets the data context cast as the view model interface
        /// </summary>
        public IMainViewModel VM
        {
            get
            {
                return this.BindingContext as IMainViewModel;
            }
        }

        /// <summary>
        /// Event override called when the view is displayed
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.VM.OnNavigatedTo();
        }
    }
}
