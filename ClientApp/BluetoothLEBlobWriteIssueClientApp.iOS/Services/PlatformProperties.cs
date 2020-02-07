// <copyright file="PlatformProperties.cs" company="TFL.">Copyright (c) 2019 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.iOS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces;
    using Foundation;
    using UIKit;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// iOS platform specific hardware interactions
    /// </summary>
    public class PlatformProperties : IPlatformProperties
    {
        /// <summary>
        /// The current image picker
        /// </summary>
        private static UIImagePickerController picker;

        /// <summary>
        /// Controls the navigation to a new page
        /// </summary>
        private INavigationService navigationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformProperties" /> class.
        /// </summary>
        /// <param name="navigationService">Controls the navigation to a new page</param>
        public PlatformProperties(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        /// <summary>
        /// An event that notifies an image taken with the camera
        /// </summary>
        public event Func<byte[], Task> NotifyPicked;

        /// <summary>
        /// An event that notifies that the pick was cancelled
        /// </summary>
        public event Action PickCancelled;

        /// <summary>
        /// Gets a value indicating whether the camera is available and accessible
        /// </summary>
        public bool IsCameraAvailable
        {
            get
            {
                return UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);
            }
        }

        /// <summary>
        /// Put up the camera app
        /// </summary>
        public void GotoCamera()
        {
            picker = new UIImagePickerController();
            var cameraDelegate = new CameraDelegate();
            cameraDelegate.NotifyPicked += (i) =>
            {
                this.NotifyPicked?.Invoke(this.ImageToBytes(i));
            };

            cameraDelegate.NotifyCancelled += () =>
            {
                this.PickCancelled?.Invoke();
            };

            picker.Delegate = cameraDelegate;
            picker.SourceType = UIImagePickerControllerSourceType.Camera;
            this.navigationService.Popup(picker, () => { });
        }

        /// <summary>
        /// Pick an image from the photo library
        /// </summary>
        public void GotoLibrary()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts an image into a byte array
        /// </summary>
        /// <param name="image">The image</param>
        /// <returns>The byte array</returns>
        private byte[] ImageToBytes(UIImage image)
        {
            byte[] byteArray = new byte[0];
            using (NSData imageData = image.AsPNG())
            {
                byteArray = new byte[imageData.Length];
                System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, byteArray, 0, Convert.ToInt32(imageData.Length));
            }

            return byteArray;
        }
    }
}