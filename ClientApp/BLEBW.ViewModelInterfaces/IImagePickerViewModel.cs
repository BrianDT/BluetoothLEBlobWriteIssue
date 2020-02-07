// <copyright file="IImagePickerViewModel.cs" company="TFL.">Copyright (c) 2018 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssue.ViewModelInterfaces
{
    using System;
    using System.Windows.Input;
    using Vssl.Samples.FrameworkInterfaces;
    using Vssl.Samples.ViewModelInterfaces;

    /// <summary>
    /// The view model for image source selection page
    /// </summary>
    public interface IImagePickerViewModel : IBaseViewModel, IPageBaseViewModel
    {
        /// <summary>
        /// Gets the command that displays the camera
        /// </summary>
        ICommandEx GotoCameraCommand { get; }

        /// <summary>
        /// Gets the command that displays the image library
        /// </summary>
        ICommand GotoImageLibraryCommand { get; }

        /// <summary>
        /// Gets a value indicating whether the camera is available and accessible
        /// </summary>
        bool CameraSourceAvailable { get; }

        /// <summary>
        /// Gets a value indicating whether the image is being transmitted
        /// </summary>
        bool SendingImage { get; }

        /// <summary>
        /// Called when the view is first displayed
        /// </summary>
        void OnViewLoaded();
    }
}
