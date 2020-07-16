// <copyright file="PermissionsViewModel.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BLEBWS.ViewInterfaces;
    using BLEBWS.ViewModelInterfaces;
    using Vssl.Samples.FrameworkInterfaces;
    using Vssl.Samples.ViewModels;
    using Xamarin.Essentials;

    /// <summary>
    /// Requests the required permissions
    /// </summary>
    public class PermissionsViewModel : BaseViewModel, IPermissionsViewModel
    {
        /// <summary>
        /// Controls the navigation to a new page
        /// </summary>
        private INavigationService navigationService;

        /// <summary>
        /// Access to platform specific dialogs
        /// </summary>
        private IDialogService dialogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">Controls the navigation to a new page</param>
        /// <param name="dialogService">Access to platform specific dialogs</param>
        public PermissionsViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            this.navigationService = navigationService;
            this.dialogService = dialogService;
        }

        /// <summary>
        /// Check that the permissions required by the application have been requested.
        /// </summary>
        /// <param name="promptWithReason">A dialog that displays the reason for the request</param>
        /// <returns>An awaitable task</returns>
        public async Task CheckPermissions(Func<string, Task> promptWithReason)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    ////if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    ////{
                    ////    await promptWithReason("Required for bluetooth access");
                    ////}

                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        this.dialogService.Notice("Permissions not granted - ending");
                        await Task.Delay(TimeSpan.FromSeconds(10));
                        return;
                    }
                }

                // Navigate to the next page
                this.navigationService.GoTo(typeof(IMainPage));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                await promptWithReason("Unable to request permissions");
            }
        }
    }
}
