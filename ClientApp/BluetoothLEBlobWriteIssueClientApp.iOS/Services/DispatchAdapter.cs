// <copyright file="DispatchAdapter.cs" company="Visual Software Systems Ltd.">Copyright (c) 2016, 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.iOS.Services
{
	using System;
	using System.Threading.Tasks;
	using Foundation;
	using Vssl.Samples.FrameworkInterfaces;

	/// <summary>
	/// The UI Dispatcher facade
	/// </summary>
	public class DispatchAdapter : IDispatchOnUIThread
	{
		/// <summary>
		/// The owning UI object
		/// </summary>
		private readonly NSObject owner;

		/// <summary>
		/// Initializes a new instance of the <see cref="DispatchAdapter"/> class.
		/// </summary>
		/// <param name="owner">The owning view</param>
		public DispatchAdapter(NSObject owner)
		{
			this.owner = owner;
		}

		/// <summary>
		/// Initialise the dispatcher
		/// </summary>
		public void Initialize()
		{
		}

		/// <summary>
		/// Check the dispatcher is initialised and if not initialise it
		/// </summary>
		public void CheckDispatcher()
		{
		}

		/// <summary>
		/// Execute an action via the dispatcher
		/// </summary>
		/// <param name="action">The action</param>
		public void Invoke(Action action)
		{
			this.owner.BeginInvokeOnMainThread(action);
		}

		/// <summary>
		/// Async Execution of an action via the dispatcher
		/// </summary>
		/// <param name="action">The action</param>
		/// <returns>An awaitable task</returns>
		public Task InvokeAsync(Action action)
		{
			throw new NotImplementedException();
		}
	}
}
