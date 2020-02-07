// <copyright file="Bootstrapper.cs" company="Visual Software Systems Ltd.">Copyright (c) 2019, 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.iOS
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BluetoothLEBlobWriteIssue.ViewModelInterfaces;
    using BluetoothLEBlobWriteIssue.ViewModels;
    using BluetoothLEBlobWriteIssueClientApp.iOS.Services;
    using BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces;
    using BluetoothLEBlobWriteIssueClientApp.Services;
    using Unity;
    using Unity.Lifetime;
    using UnityDIFacade;
    using Vssl.Samples.Framework;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// Bootstraps the DI
    /// </summary>
    public class Bootstrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public Bootstrapper()
        {
        }

        /// <summary>
        /// Create the DI container and register all classes against their interfaces
        /// </summary>
        /// <returns>The interface to the DI facade</returns>
        public IDependencyResolver Startup()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container, new ContainerControlledLifetimeManager());
            container.RegisterType<IDependencyResolver, UnityDI>(new ContainerControlledLifetimeManager());

            // Framework
            container.RegisterType<IDispatchOnUIThread, DispatchAdapter>(new ContainerControlledLifetimeManager());
            container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPlatformProperties, PlatformProperties>(new ContainerControlledLifetimeManager());

            // Services
            container.RegisterType<IBTClient, BTClient>(new ContainerControlledLifetimeManager());

            // View models
            container.RegisterType<IImagePickerViewModel, ImagePickerViewModel>();

            var injectionFacade = container.Resolve<IDependencyResolver>();
            DependencyHelper.Container = injectionFacade;

            // ensure the singleton dispatcher is created.
            DispatchHelper.Initialise(injectionFacade.Resolve<IDispatchOnUIThread>());

            return injectionFacade;
        }
    }
}