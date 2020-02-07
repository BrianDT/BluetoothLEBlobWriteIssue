// <copyright file="DispatchAdapter.cs" company="TFL.">Copyright (c) 2013, 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueServerApp.Droid.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    ////using System.Threading;
    ////using System.Threading.Tasks;
    using Android.App;
    using Android.Content;
    using Android.OS;
    using Java.Lang;
    using Vssl.Samples.FrameworkInterfaces;

    /// <summary>
    /// The UI Dispatcher facade for android
    /// </summary>
    public class DispatchAdapter : IDispatchOnUIThread
    {
        /// <summary>
        /// The loop that the UI thread runs in
        /// </summary>
        private Handler handler;

        /// <summary>
        /// The UI thread
        /// </summary>
        private Thread uiThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchAdapter"/> class.
        /// </summary>
        public DispatchAdapter()
        {
        }

        /// <summary>
        /// Initialise the dispatcher
        /// </summary>
        public void Initialize()
        {
            Context context = AppHelper.AppContext;
            this.handler = new Handler(context.MainLooper);
            this.uiThread = this.handler.Looper.Thread;
        }

        /// <summary>
        /// Check the dispatcher is initialised and if not initialise it
        /// </summary>
        public void CheckDispatcher()
        {
            if (this.handler == null)
            {
                this.Initialize();
            }
        }

        /// <summary>
        /// Execute an action via the dispatcher
        /// </summary>
        /// <param name="action">The action</param>
        public void Invoke(Action action)
        {
            this.CheckDispatcher();
            if (Thread.CurrentThread() != this.uiThread)
            {
                this.handler.Post(action);
            }
            else
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Async Execution of an action via the dispatcher
        /// </summary>
        /// <param name="action">The action</param>
        /// <returns>An awaitable task</returns>
        public System.Threading.Tasks.Task InvokeAsync(Action action)
        {
            var task = System.Threading.Tasks.Task.Run(() =>
            {
                this.Invoke(action);
            });

            return task;
        }
    }
}