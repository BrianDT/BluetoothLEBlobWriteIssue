// <copyright file="ImagePickerViewModel.cs" company="TFL.">Copyright (c) 2018 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssue.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using BluetoothLEBlobWriteIssue.Models;
    using BluetoothLEBlobWriteIssue.ViewModelInterfaces;
    using BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces;
    using Vssl.Samples.Framework;
    using Vssl.Samples.FrameworkInterfaces;
    using Vssl.Samples.ViewModels;

    /// <summary>
    /// The view model for image source selection page
    /// </summary>
    public class ImagePickerViewModel : BaseViewModel, IImagePickerViewModel
    {
        #region [ Private Fields ]

        /// <summary>
        /// The client connection manager
        /// </summary>
        private IBTClient client;

        /// <summary>
        /// Provides platform specific hardware interactions
        /// </summary>
        private IPlatformProperties platformProperties;

        /// <summary>
        /// A service for national language string resources
        /// </summary>
        ////private IStringResourceServices stringResourceServices;

        /// <summary>
        /// True if the connect command is active
        /// </summary>
        private bool isConnecteing;

        /// <summary>
        /// True id the bluetooth service is available
        /// </summary>
        private bool bluetoothServiceIsStarted;
        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePickerViewModel" /> class.
        /// </summary>
        /// <param name="client">The bluetooth client connection manager</param>
        /// <param name="platformProperties">Provides platform specific hardware interactions</param>
        public ImagePickerViewModel(IBTClient client, IPlatformProperties platformProperties)
        {
            this.platformProperties = platformProperties;
            this.client = client;
            this.client.NotifyServiceAvailable += (bool status) =>
            {
                this.bluetoothServiceIsStarted = status;
                this.ConnectCommand.RaiseCanExecuteChanged();
            };
            this.client.NotifyCanSendImages += () =>
            {
                this.IsConnected = true;
                this.OnPropertyChanged("IsConnected");
                this.GotoCameraCommand.RaiseCanExecuteChanged();
            };
            this.client.NotifyDisconnected += () =>
            {
                this.IsConnected = false;
                this.OnPropertyChanged("IsConnected");
                this.GotoCameraCommand.RaiseCanExecuteChanged();
            };

            this.platformProperties.NotifyPicked += this.OnImagePicked;
            this.platformProperties.PickCancelled += this.OnPickCancelled;

            this.CameraSourceAvailable = platformProperties.IsCameraAvailable;
            this.ConnectCommand = new DelegateCommandAsync(this.Connect, o => this.bluetoothServiceIsStarted);
            this.GotoCameraCommand = new DelegateCommandAsync(this.GotoCamera, o => this.isConnecteing || this.IsConnected);
            this.GotoImageLibraryCommand = new DelegateCommandAsync(this.GotoImageLibrary);
            this.PageHeader = "Image picker"; ////this.stringResourceServices.FindNLString("ImageSource");
        }

        #endregion

        #region [ IImagePickerViewModel Properties ]

        /// <summary>
        /// Gets the command that connects to the service
        /// </summary>
        public ICommandEx ConnectCommand { get; private set; }

        /// <summary>
        /// Gets the command that displays the camera
        /// </summary>
        public ICommandEx GotoCameraCommand { get; private set; }

        /// <summary>
        /// Gets the command that displays the image library
        /// </summary>
        public ICommand GotoImageLibraryCommand { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the camera is available and accessible
        /// </summary>
        public bool CameraSourceAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the service is connected
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the image is being transmitted
        /// </summary>
        public bool SendingImage { get; private set; }

        #endregion

        #region [ IPageBaseViewModel Properties ]

        /// <summary>
        /// Gets the text for the tool bar
        /// </summary>
        public string PageHeader { get; private set; }

        #endregion

        #region [ IImagePickerViewModel Methods ]

        /// <summary>
        /// Called when the view is first displayed
        /// </summary>
        public void OnViewLoaded()
        {
            this.client.StartBackgroundDeviceScan();
        }

        #endregion

        #region [ Command handlers ]

        /// <summary>
        /// Connects to the service
        /// </summary>
        /// <param name="parameter">An optional parameter</param>
        /// <returns>An awaitable task</returns>
        private async Task Connect(object parameter)
        {
            this.isConnecteing = true;
            this.ConnectCommand.RaiseCanExecuteChanged();
            if (!await this.client.Connect())
            {
                this.isConnecteing = false;
                this.ConnectCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Displays the camera
        /// </summary>
        /// <param name="parameter">An optional parameter</param>
        /// <returns>An awaitable task</returns>
        private async Task GotoCamera(object parameter)
        {
            this.platformProperties.GotoCamera();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Displays the image library
        /// </summary>
        /// <param name="parameter">An optional parameter</param>
        /// <returns>An awaitable task</returns>
        private async Task GotoImageLibrary(object parameter)
        {
            this.platformProperties.GotoLibrary();
            await Task.CompletedTask;
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// An event handler for image picked
        /// </summary>
        /// <param name="image">The image as a byte array</param>
        /// <returns>An awaitable task</returns>
        private async Task OnImagePicked(byte[] image)
        {
            this.SendingImage = true;
            this.OnPropertyChanged("SendingImage");

            var model = new Image();
            model.BinaryData = image;
            model.Guid = Guid.NewGuid();
            await this.client.SendImage(model);

            this.SendingImage = false;
            this.OnPropertyChanged("SendingImage");
        }

        /// <summary>
        /// An event handler for pick cancelled
        /// </summary>
        private void OnPickCancelled()
        {
            ////this.navigationService.GoBack();
        }

        #endregion
    }
}
