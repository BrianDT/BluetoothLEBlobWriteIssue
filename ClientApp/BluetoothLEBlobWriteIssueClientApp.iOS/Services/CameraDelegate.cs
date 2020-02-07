// <copyright file="CameraDelegate.cs" company="TFL.">Copyright (c) 2019 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.iOS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Foundation;
    using UIKit;

    /// <summary>
    /// Handles image picking using the camera
    /// </summary>
    public class CameraDelegate : UIImagePickerControllerDelegate
    {
        /// <summary>
        /// Notify a successful image pick
        /// </summary>
        public event Action<UIImage> NotifyPicked;

        /// <summary>
        /// Notify tat the pick was cancelled
        /// </summary>
        public event Action NotifyCancelled;

        /// <summary>
        /// Called on completion of an image pick
        /// </summary>
        /// <param name="picker">The picker</param>
        /// <param name="info">Additional contextual data</param>
        public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            picker.DismissModalViewController(true);
            var image = info.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
            this.NotifyPicked?.Invoke(image);
        }

        /// <summary>
        /// Called when the pick has been cancelled
        /// </summary>
        /// <param name="picker">The picker</param>
        public override void Canceled(UIImagePickerController picker)
        {
            base.Canceled(picker);
            picker.DismissModalViewController(true);
            this.NotifyCancelled?.Invoke();
        }
    }
}