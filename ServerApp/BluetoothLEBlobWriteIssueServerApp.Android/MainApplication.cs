// <copyright file="MainApplication.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueServerApp.Droid
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Android.App;
    using Android.Content;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using BLEBWS.ServiceInterfaces;
    using BLEBWS.Services;
    using BLEBWS.ViewInterfaces;
    using BLEBWS.ViewModelInterfaces;
    using BLEBWS.ViewModels;
    using BluetoothLEBlobWriteIssueServerApp.Droid.Services;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using Plugin.CurrentActivity;
    using Unity;
    using Unity.Lifetime;
    using UnityDIFacade;
    using Vssl.Samples.Framework;
    using Vssl.Samples.Framework.Forms;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// The main android application
    /// Sets up plugin libraries and injection
    /// </summary>
    #if DEBUG
    [Application(Debuggable = true)]
    #else
    [Application(Debuggable = false)]
    #endif
    public class MainApplication : Application
    {
        /// <summary>
        /// The format used for the log entries
        /// </summary>
        private const string LogLayout = @"${longdate} [${threadid}] ${level:uppercase=true} ${logger} ${callsite:className=true:includeNamespace=false:fileName=false:includeSourcePath=true} ${message} ${exception:format=ToString,StackTrace,Data:innerFormat=ToString,StackTrace,Data}";

        /// <summary>
        /// The NLog logger
        /// </summary>
        private static NLog.ILogger log;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainApplication"/> class.
        /// </summary>
        /// <param name="handle">System handle</param>
        /// <param name="transfer">Second system handle</param>
        public MainApplication(IntPtr handle, JniHandleOwnership transfer)
        : base(handle, transfer)
        {
        }

        /// <summary>
        /// Called when the application is created (see lifecycle)
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
            AppHelper.AppContext = this;
            this.ConfigureLogging();
            this.Bootstrap();
        }

        /// <summary>
        /// Configure the log files
        /// </summary>
        private void ConfigureLogging()
        {
            var logLevel = LogLevel.Trace;

            var config = new LoggingConfiguration();
            var logPath = this.GetExternalFilesDir(null).Path;
            var fileTarget = new FileTarget
            {
                Name = "File",
                FileName = Path.Combine(logPath, "sample.log"),
                ArchiveFileName = Path.Combine(logPath, "archive.{#}.log"),
                ArchiveEvery = FileArchivePeriod.Day,
                Layout = LogLayout,
                MaxArchiveFiles = 1
            };

            config.AddTarget("file", fileTarget);

            var fileRule = new LoggingRule("*", logLevel, fileTarget);
            config.LoggingRules.Add(fileRule);

            var consoleTarget = new ConsoleTarget()
            {
                Name = "Console",
                Layout = LogLayout
            };
            config.AddTarget("console", consoleTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, consoleTarget));

            LogManager.Configuration = config;

            MainApplication.log = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Create the DI container and register all classes against their interfaces
        /// </summary>
        private void Bootstrap()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container, new ContainerControlledLifetimeManager());
            container.RegisterType<IDependencyResolver, UnityDI>(new ContainerControlledLifetimeManager());

            // Initialise logging
            container.RegisterInstance<ILogger>(MainApplication.log, new ContainerControlledLifetimeManager());

            // Framework
            container.RegisterType<IDispatchOnUIThread, DispatchAdapter>(new ContainerControlledLifetimeManager());
            container.RegisterType<INavigationService, FormsNavigationService>(new ContainerControlledLifetimeManager());

            // Services
            container.RegisterType<IBTService, BTService>(new ContainerControlledLifetimeManager());

            // View models
            container.RegisterType<IPermissionsViewModel, PermissionsViewModel>();
            container.RegisterType<IMainViewModel, MainViewModel>();

            // Views
            container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());

            var injectionFacade = container.Resolve<IDependencyResolver>();
            DependencyHelper.Container = injectionFacade;

            // ensure the singleton dispatcher is created.
            IDispatchOnUIThread dispatcher = injectionFacade.Resolve<IDispatchOnUIThread>();
            DispatchHelper.Initialise(dispatcher);
        }
    }
}