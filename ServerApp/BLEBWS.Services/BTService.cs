// <copyright file="BTService.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using BLEBWS.ServiceInterfaces;
    using BLEBWS.ViewInterfaces;
    using BluetoothLEBlobWriteIssue.Models;
    using NLog;
    using Plugin.BluetoothLE;
    using Plugin.BluetoothLE.Server;
    using ProtoBuf;

    /// <summary>
    /// The Bluetooth LE service manager
    /// </summary>
    public class BTService : IBTService
    {
        #region [ Private Fields ]

        /// <summary>
        /// The Bluetooth LE adapter
        /// </summary>
        private IAdapter adapter;

        /// <summary>
        /// The state of the server
        /// </summary>
        private ServerState serverState;

        /// <summary>
        /// The NLog logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Access to platform specific dialogs
        /// </summary>
        private IDialogService dialogService;

        /// <summary>
        /// The Bluetooth LE server
        /// </summary>
        private IGattServer server;

        /// <summary>
        /// Enables waiting for then server to start
        /// </summary>
        private SemaphoreSlim completionMonitor;

        /// <summary>
        /// A list of descriptions used for diagnostics
        /// </summary>
        private Dictionary<Guid, string> indexOfCharacteristicDescriptions;

        /// <summary>
        /// An index of partially received image objects
        /// </summary>
        private Dictionary<Guid, byte[]> partialImagePerDevice = new Dictionary<Guid, byte[]>();

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Initializes a new instance of the <see cref="BTService" /> class.
        /// </summary>
        /// <param name="dialogService">Access to platform specific dialogs</param>
        public BTService(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.logger = LogManager.GetCurrentClassLogger();
            this.adapter = CrossBleAdapter.Current;
        }

        #endregion

        #region [ Events ]

        /// <summary>
        /// Notify that a new image has been received
        /// </summary>
        public event EventHandler<INewImageEventArgs> NotifyNewImage;

        /// <summary>
        /// Notify that image bytes have been received from the client
        /// </summary>
        public event EventHandler<IBytesReceivedEventArgs> NotifyBytesReceived;
        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets a value indicating whether the server is started or is starting
        /// </summary>
        public bool IsStartedOrStarting
        {
            get
            {
                return this.serverState == ServerState.Starting || this.serverState == ServerState.Started;
            }
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Check that the preconditions for server start are set
        /// </summary>
        /// <returns>True if a server can be started</returns>
        public bool CanStartServer()
        {
            if (this.adapter.Status != AdapterStatus.PoweredOn)
            {
                this.dialogService.Notice("Could not start GATT Server.  Adapter Status: " + this.adapter.Status);
                this.serverState = ServerState.NotStartable;
                return false;
            }

            if (!this.adapter.Features.HasFlag(AdapterFeatures.ServerGatt))
            {
                this.dialogService.Notice("GATT Server is not supported on this platform configuration");
                this.serverState = ServerState.NotStartable;
                return false;
            }

            this.completionMonitor = new SemaphoreSlim(initialCount: 0);
            return true;
        }

        /// <summary>
        /// Start a LE server and pass the identifier to the client manager
        /// </summary>
        /// <returns>An awaitable task</returns>
        public async Task StartServer()
        {
            this.serverState = ServerState.Starting;
            try
            {
                this.server = await this.adapter.CreateGattServer();
                var serviceId = Guid.Parse(Constants.ServiceUUID);
                var service = this.server.CreateService(serviceId, true);

                this.indexOfCharacteristicDescriptions = new Dictionary<Guid, string>();
                this.indexOfCharacteristicDescriptions.Add(Constants.ImageCharacteristic, "Image sent");
                this.indexOfCharacteristicDescriptions.Add(Constants.ImageReceivedCharacteristic, "Image Received");

                ////this.indexOfCharacteristics = new Dictionary<Guid, Plugin.BluetoothLE.Server.IGattCharacteristic>();
                this.BuildCharacteristics(service, Constants.ImageReceivedCharacteristic, isNotification: true);
                this.BuildCharacteristics(service, Constants.ImageCharacteristic);

                this.server.AddService(service);

                this.server.WhenAnyCharacteristicSubscriptionChanged().Subscribe(x =>
                {
                    this.OnEvent($"[WhenAnyCharacteristicSubscriptionChanged] UUID: {x.Characteristic.Uuid} - Device: {x.Device.Uuid} - Subscription: {x.IsSubscribing}");
                });

                this.OnEvent("GATT Server Started");
                this.serverState = ServerState.Started;
            }
            catch (Exception ex)
            {
                this.serverState = ServerState.FailedToStart;
                this.logger.Error(ex, "Error building gatt server - ");
                this.dialogService.Notice("Bluetooth LE service failed to start");
            }
            finally
            {
                this.completionMonitor.Release();
            }
        }

        /// <summary>
        /// Start the advertiser
        /// </summary>
        public void StartAdvertiser()
        {
            try
            {
                var serviceIds = this.server.Services.Select(s => s.Uuid).ToList();
                this.adapter.Advertiser.Start(new AdvertisementData
                {
                    LocalName = "Sample GATT",
                    ServiceUuids = serviceIds
                });

                StringBuilder serviceIdList = new StringBuilder();
                foreach (var serviceId in serviceIds)
                {
                    serviceIdList.Append(serviceId).Append(", ");
                }

                this.OnEvent($"GATT Advertiser Started on {this.adapter.DeviceName}, advertising services {serviceIdList.ToString()}");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex, "starting advertiser - ");
            }
        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void StopServer()
        {
            this.adapter.Advertiser.Stop();
            this.OnEvent("GATT Server Stopped");
            this.server.Dispose();
            this.server = null;
            this.serverState = ServerState.Stopped;
        }

        #endregion

        #region [ Protected Methods ]

        /// <summary>
        /// Called when a new image is received from a device
        /// </summary>
        /// <param name="deviceName">The device name</param>
        protected virtual void OnNewImage(string deviceName)
        {
            // Send TOAST via dialog server
            this.dialogService.Notice($"New image from: {deviceName}");
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Create a characteristic
        /// </summary>
        /// <param name="service">The service</param>
        /// <param name="characteristicId">The characteristic identifier</param>
        /// <param name="isNotification">True if a notification characteristic</param>
        private void BuildCharacteristics(Plugin.BluetoothLE.Server.IGattService service, Guid characteristicId, bool isNotification = false)
        {
            Plugin.BluetoothLE.Server.IGattCharacteristic characteristic = null;
            if (isNotification)
            {
                characteristic = service.AddCharacteristic(
                    characteristicId,
                    CharacteristicProperties.Indicate | CharacteristicProperties.Read | CharacteristicProperties.Notify,
                    GattPermissions.Read | GattPermissions.Write);
            }
            else
            {
                characteristic = service.AddCharacteristic(
                    characteristicId,
                    CharacteristicProperties.Read | CharacteristicProperties.Write | CharacteristicProperties.WriteNoResponse,
                    GattPermissions.Read | GattPermissions.Write);
            }

            ////this.indexOfCharacteristics.Add(characteristicId, characteristic);

            characteristic.WhenDeviceSubscriptionChanged().Subscribe(e =>
            {
                var @event = e.IsSubscribed ? "Subscribed" : "Unsubcribed";
                this.OnEvent($"Device {e.Device.Uuid} {@event}");
                this.OnEvent($"Charcteristic Subcribers: {characteristic.SubscribedDevices.Count}");
            });

            characteristic.WhenReadReceived().Subscribe(x =>
            {
                this.OnEvent($"{this.indexOfCharacteristicDescriptions[characteristic.Uuid]} characteristic Read Received");

                if (Constants.ImageReceivedCharacteristic == characteristic.Uuid)
                {
                    var deviceId = x.Device.Uuid;
                    byte[] dataSoFar = null;
                    bool hasStoredValue = this.partialImagePerDevice.TryGetValue(deviceId, out dataSoFar);

                    if (hasStoredValue)
                    {
                        Guid imageId;
                        x.Status = this.HandleImage(deviceId, dataSoFar, out imageId);
                        if (x.Status == GattStatus.Success)
                        {
                            x.Value = Encoding.UTF8.GetBytes(imageId.ToString());
                        }

                        this.partialImagePerDevice.Remove(deviceId);
                    }
                    else
                    {
                        this.logger.Error($"Reading Image from {deviceId}, end of image marker read, but there is no stored image");
                        x.Status = GattStatus.Failure;
                    }
                }
            });

            characteristic.WhenWriteReceived().Subscribe(x =>
            {
                var deviceId = x.Device.Uuid;

                if (Constants.ImageCharacteristic == characteristic.Uuid)
                {
                    this.logger.Info($"Reading Image from {deviceId}, appending {x.Value.Length} bytes");
                    byte[] dataSoFar = null;
                    long bytesSoFar = x.Value.Length;
                    if (this.partialImagePerDevice.TryGetValue(deviceId, out dataSoFar))
                    {
                        byte[] rv = new byte[dataSoFar.Length + x.Value.Length];
                        System.Buffer.BlockCopy(dataSoFar, 0, rv, 0, dataSoFar.Length);
                        System.Buffer.BlockCopy(x.Value, 0, rv, dataSoFar.Length, x.Value.Length);
                        this.partialImagePerDevice[deviceId] = rv;
                        bytesSoFar += dataSoFar.Length;
                    }
                    else
                    {
                        this.partialImagePerDevice.Add(deviceId, x.Value);
                    }

                    this.NotifyBytesReceived?.Invoke(this, new BytesReceivedEventArgs(bytesSoFar, deviceId));
                    x.Status = GattStatus.Success;
                }
            });
        }

        /// <summary>
        /// Record events to the log file
        /// </summary>
        /// <param name="msg">The message to log</param>
        private void OnEvent(string msg)
        {
            this.logger.Info(msg);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        /// <summary>
        /// Deserialises the image received
        /// </summary>
        /// <param name="deviceId">The device Id as a string</param>
        /// <param name="value">The byte string containing the serialised model</param>
        /// <param name="imageId">The image id</param>
        /// <returns>Success or failed</returns>
        private GattStatus HandleImage(Guid deviceId, byte[] value, out Guid imageId)
        {
            GattStatus result = GattStatus.Success;
            imageId = Guid.Empty;

            this.logger.Info($"Handling Image from {deviceId}");
            try
            {
                Image image = null;
                using (var stream = new MemoryStream(value))
                {
                    image = Serializer.Deserialize<Image>(stream);
                }

                imageId = image.Guid;

                this.OnNewImage(image.DeviceName);
                this.NotifyNewImage?.Invoke(this, new NewImageEventArgs(image));
            }
            catch (Exception e)
            {
                this.logger.Info($"Io Exception while reading an image from bluetooth input stream on {deviceId}");
                System.Diagnostics.Debug.WriteLine(e);
                result = GattStatus.Failure;
            }

            return result;
        }

        #endregion
    }
}
