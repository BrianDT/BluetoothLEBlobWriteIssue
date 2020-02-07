// <copyright file="FormsNavigationService.cs" company="Visual Software Systems Ltd.">Copyright (c) 2012, 2020 All rights reserved</copyright>

namespace Vssl.Samples.Framework.Forms
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Vssl.Samples.FrameworkInterfaces;
    using Xamarin.Forms;

    /// <summary>
    /// Controls the navigation to a new page
    /// </summary>
    public class FormsNavigationService : INavigationService
    {
        /// <summary>
        /// The dependency injection service
        /// </summary>
        private IDependencyResolver dependencyService;

        /// <summary>
        /// The navigation controller
        /// </summary>
        private Shell navigationShell;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsNavigationService"/> class.
        /// </summary>
        /// <param name="dependencyService">The dependency injection service</param>
        public FormsNavigationService(IDependencyResolver dependencyService)
        {
            this.dependencyService = dependencyService;
        }

        /// <summary>
        /// Initialise the navigation service with a native navigation object
        /// </summary>
        /// <param name="nativeObject">The native navigation object</param>
        public void Initialise(object nativeObject)
        {
            this.navigationShell = nativeObject as Shell;
        }

        /// <summary>
        /// Navigate to a new page
        /// </summary>
        /// <param name="viewInterface">The interface of the view to be navigated to</param>
        public void GoTo(Type viewInterface)
        {
            this.navigationShell.GoToAsync("//" + viewInterface.Name, true);
        }

        /// <summary>
        /// Popup a Xamarin forms page
        /// </summary>
        /// <param name="viewInterface">The interface of the dialog to popup</param>
        /// <param name="handler">A completion handler</param>
        /// <returns>An awaitable task</returns>
        public async Task Popup(Type viewInterface, Action handler)
        {
            var modalPage = this.dependencyService.Resolve(viewInterface) as Page;
            await this.navigationShell.Navigation.PushModalAsync(modalPage);
            handler();
        }

        /// <summary>
        /// Popup a native page
        /// </summary>
        /// <param name="viewObject">A native view</param>
        /// <param name="handler">A completion handler</param>
        public virtual void Popup(object viewObject, Action handler)
        {
            // Needs to be implemented in a platform specific override
            throw new NotImplementedException();
        }

        /// <summary>
        /// Navigate back to the previous view
        /// </summary>
        /// <returns>True if can go back</returns>
        public bool GoBack()
        {
            this.navigationShell.Navigation.RemovePage(this.navigationShell.Navigation.NavigationStack.Last());
            return true;
        }
    }
}
