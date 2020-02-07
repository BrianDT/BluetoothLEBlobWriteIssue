// <copyright file="BTClient.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BluetoothLEBlobWriteIssue.Models;
    using BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces;
    using Plugin.BluetoothLE;

    /// <summary>
    /// A sample Bluetooth client service
    /// </summary>
    public class BTClient : IBTClient
    {
        #region [ Private Fields ]

        /// <summary>
        /// A collection of discovered devices indexed by their identifier
        /// </summary>
        private Dictionary<Guid, IBTDevice> discoveredDevices = new Dictionary<Guid, IBTDevice>();

        /// <summary>
        /// The currently connected device
        /// </summary>
        private IBTDevice connectedDevice;

        /// <summary>
        /// True if a device scan was requested and the adapter is not ready yet
        /// </summary>
        private bool scanRequested;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="BTClient" /> class.
        /// </summary>
        public BTClient()
        {
            CrossBleAdapter.Current.WhenStatusChanged().Subscribe(status => { this.NotifyAdapterStateChanged(); });
        }

        #endregion

        #region [ Events ]

        /// <summary>
        /// Notify that the service is available
        /// </summary>
        public event Action<bool> NotifyServiceAvailable;

        /// <summary>
        /// Notify that images can be sent
        /// </summary>
        public event Action NotifyCanSendImages;

        /// <summary>
        /// Notify that the connected device has disconnected
        /// </summary>
        public event Action NotifyDisconnected;

        #endregion

        #region [ IBTClient Methods ]

        /// <summary>
        /// Start a background scan of bluetooth devices before the filter is known
        /// </summary>
        public void StartBackgroundDeviceScan()
        {
            if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn)
            {
                this.StopDiscovery();
                CrossBleAdapter.Current.Scan().Subscribe(sr =>
                {
                    var device = sr.Device;
                    if (BTDevice.HostsService(Guid.Parse(Constants.ServiceUUID), sr.AdvertisementData))
                    {
                        if (!this.discoveredDevices.ContainsKey(device.Uuid))
                        {
                            this.discoveredDevices.Add(device.Uuid, new BTDevice(device));
                        }
                    }
                });
            }
            else
            {
                this.scanRequested = true;
            }
        }

        /// <summary>
        /// Connect to the defined service
        /// </summary>
        /// <returns>True if connected</returns>
        public async Task<bool> Connect()
        {
            if (this.ConnectDiscoveredDevice())
            {
                return true;
            }

            // Try again
            this.discoveredDevices.Clear();
            this.StartBackgroundDeviceScan();

            await Task.Delay(TimeSpan.FromSeconds(30));
            if (this.ConnectDiscoveredDevice())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sends an image from the client to the server
        /// </summary>
        /// <param name="image">The image model</param>
        /// <returns>An awaitable task</returns>
        public async Task SendImage(Image image)
        {
            await this.connectedDevice.Write(image);
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Terminate device discovery
        /// </summary>
        private void StopDiscovery()
        {
            if (CrossBleAdapter.Current.IsScanning)
            {
                CrossBleAdapter.Current.StopScan();
            }
        }

        /// <summary>
        /// Notifies a change in the bluetooth adapter state
        /// </summary>
        private void NotifyAdapterStateChanged()
        {
            this.NotifyServiceAvailable?.Invoke(CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn);
            if (this.scanRequested)
            {
                if (CrossBleAdapter.Current.Status == AdapterStatus.PoweredOn)
                {
                    this.scanRequested = false;
                    this.StartBackgroundDeviceScan();
                }
            }
        }

        /// <summary>
        /// Checks the list of discovered devices and connects to the first
        /// </summary>
        /// <returns>True if a device was found</returns>
        private bool ConnectDiscoveredDevice()
        {
            this.StopDiscovery();
            if (this.discoveredDevices.Count > 0)
            {
                this.connectedDevice = this.discoveredDevices.First().Value;
                this.connectedDevice.NotifyCanSendImages += () =>
                {
                    this.NotifyCanSendImages?.Invoke();
                };
                this.connectedDevice.NotifyDisconnected += () =>
                {
                    this.NotifyDisconnected?.Invoke();
                };

                this.connectedDevice.OnConnected();
                return true;
            }

            return false;
        }

        #endregion
    }
}
