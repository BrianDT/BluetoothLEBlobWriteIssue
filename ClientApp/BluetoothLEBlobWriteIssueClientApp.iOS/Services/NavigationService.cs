// <copyright file="NavigationService.cs" company="Visual Software Systems Ltd.">Copyright (c) 2012, 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.iOS.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Foundation;
    using UIKit;
    using Vssl.Samples.Framework;
    using Vssl.Samples.Framework.Forms;
    using Vssl.Samples.FrameworkInterfaces;
    using Xamarin.Forms;

    /// <summary>
    /// Controls the navigation to a new page
    /// </summary>
    public class NavigationService : FormsNavigationService
    {
        /// <summary>
        /// The native navigation controller
        /// </summary>
        private UINavigationController navigationController;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationService"/> class.
        /// </summary>
        /// <param name="dependencyService">The dependency injection service</param>
        public NavigationService(IDependencyResolver dependencyService) : base(dependencyService)
        {
        }

        /// <summary>
        /// Popup a native page
        /// </summary>
        /// <param name="viewObject">A native view</param>
        /// <param name="handler">A completion handler</param>
        public override void Popup(object viewObject, Action handler)
        {
            UIViewController view = viewObject as UIViewController;
            if (this.navigationController == null)
            {
                this.navigationController = this.GetUINavigationController(UIApplication.SharedApplication.KeyWindow.RootViewController);
            }

            this.navigationController.PresentViewController(view, true, handler);
        }

        /// <summary>
        /// Search the controller stack to find a navigation controller
        /// </summary>
        /// <param name="controller">The root controller</param>
        /// <returns>A navigation controller if found in the children</returns>
        private UINavigationController GetUINavigationController(UIViewController controller)
        {
            if (controller == null)
            {
                throw new ArgumentException("controller");
            }

            var navcontroller = controller as UINavigationController;
            if (navcontroller != null)
            {
                return navcontroller;
            }

            var count = controller.ChildViewControllers.Count();
            if (count != 0)
            {
                for (int c = 0; c < count; c++)
                {
                    Debug.WriteLine("local iteration {0}: current controller has {1} children", c, count);
                    var child = this.GetUINavigationController(controller.ChildViewControllers[c]);
                    if (child == null)
                    {
                        Debug.WriteLine("No children left on current controller. Moving back up");
                    }
                    else
                    {
                        return child;
                    }
                }
            }

            return null;
        }
    }
}