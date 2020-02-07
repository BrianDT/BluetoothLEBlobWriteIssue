// <copyright file="BTDevice.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueClientApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using BluetoothLEBlobWriteIssue.Models;
    using BluetoothLEBlobWriteIssueClientApp.ServiceInterfaces;
    using NLog;
    using Plugin.BluetoothLE;
    using ProtoBuf;

    /// <summary>
    /// A cross platform representation of a bluetooth device
    /// </summary>
    public class BTDevice : IBTDevice
    {
        #region [ Private Fields ]

        /// <summary>
        /// The read /write timeout
        /// </summary>
        private const int ConnectionTimeout = 30000;

        /// <summary>
        /// The plugin device model
        /// </summary>
        private IDevice device;

        /// <summary>
        /// The logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// The characteristic used to transmit images
        /// </summary>
        private IGattCharacteristic imageCharacteristic;

        /// <summary>
        /// The characteristic used to confirm that an image has been received
        /// </summary>
        private IGattCharacteristic imageReceivedCharacteristic;

        #endregion

        #region [ Constructor ]

        /// <summary>
        /// Initializes a new instance of the <see cref="BTDevice" /> class.
        /// </summary>
        /// <param name="device">The plugin bluetooth device</param>
        public BTDevice(IDevice device)
        {
            this.device = device;
            this.logger = LogManager.GetCurrentClassLogger();
        }

        #endregion

        #region [ Events ]

        /// <summary>
        /// Notify that images can be sent
        /// </summary>
        public event Action NotifyCanSendImages;

        /// <summary>
        /// Notify that the device has disconnected
        /// </summary>
        public event Action NotifyDisconnected;

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Gets a value indicating whether the device supports the service with the given id
        /// </summary>
        /// <param name="serviceGuid">The id of the service</param>
        /// <param name="advertisementData">The advertisement data from the bluetooth device</param>
        /// <returns>True if the device does host the service</returns>
        public static bool HostsService(Guid serviceGuid, IAdvertisementData advertisementData)
        {
            List<Guid> serviceIds = new List<Guid>();
            if (advertisementData.ServiceUuids != null)
            {
                serviceIds = advertisementData.ServiceUuids.ToList();
            }

            return serviceIds.Contains(serviceGuid);
        }

        /// <summary>
        /// Actions taken when the device connects
        /// </summary>
        public void OnConnected()
        {
            this.device.Connect();
            this.device.WhenDisconnected().Subscribe(device => this.OnDisconnect(device));

            this.device.WhenAnyCharacteristicDiscovered().Subscribe(characteristic =>
            {
                this.OnCharacteristicDiscovered(characteristic);
            });
        }

        /// <summary>
        /// Write an image to the output stream
        /// </summary>
        /// <param name="image">The image to transmit</param>
        /// <returns>An awaitable task</returns>
        public async Task Write(Image image)
        {
            if (image.BinaryData == null)
            {
                this.logger.Error("Image is empty");
                return;
            }

            if (this.imageCharacteristic == null)
            {
                this.logger.Error("No image characteristic to send to");
                return;
            }

            this.logger.Info($"Started writing image {image.Guid}");
            long bytesSent = 0;

            var bytes = Serialize(image);
            SemaphoreSlim imageWriteCompletionMonitor = new SemaphoreSlim(initialCount: 0);
            this.imageCharacteristic.BlobWrite(bytes, reliableWrite: false).Subscribe(
                segment =>
                {
                    bytesSent = segment.Position;
                },
                ex =>
                {
                    this.logger.Error(ex, $"Error sending {image.Id}, {bytesSent} sent out of {bytes.Length}");
                },
                () =>
                {
                    // Flag write as complete
                    imageWriteCompletionMonitor.Release();
                    this.logger.Info($"Completed sending image {image.Id}");

                    this.imageReceivedCharacteristic.Read().Subscribe(result =>
                    {
                        bool validReply = result.Data != null && result.Data.Length != 0;
                        if (validReply)
                        {
                            var read = Encoding.UTF8.GetString(result.Data, 0, result.Data.Length);
                            Guid imageId = Guid.Parse(read);
                            this.logger.Info($"Confirmed image {imageId} received");
                        }
                    });
                });

            await this.WaitForTaskWithTimeout(imageWriteCompletionMonitor, ConnectionTimeout);
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Serialise to binary using protobuf
        /// </summary>
        /// <param name="item">The object to be serialised</param>
        /// <returns>The resultant byte array</returns>
        private static byte[] Serialize(object item)
        {
            using (var stream = new MemoryStream())
            {
                Serializer.NonGeneric.Serialize(stream, item);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Called when a new characteristic is discovered
        /// </summary>
        /// <param name="characteristic">The characteristic</param>
        private void OnCharacteristicDiscovered(IGattCharacteristic characteristic)
        {
            var id = characteristic.Uuid;
            if (id == Constants.ImageCharacteristic)
            {
                this.imageCharacteristic = characteristic;
                this.NotifyCanSendImages?.Invoke();
            }
            else if (id == Constants.ImageReceivedCharacteristic)
            {
                this.imageReceivedCharacteristic = characteristic;
            }
        }

        /// <summary>
        /// Wait for a semaphore to be release or time out
        /// </summary>
        /// <param name="completionMonitor">The semaphore</param>
        /// <param name="timeout">The time out period</param>
        /// <returns>An awaitable task</returns>
        private async Task WaitForTaskWithTimeout(SemaphoreSlim completionMonitor, int timeout)
        {
            if (!await completionMonitor.WaitAsync(timeout))
            {
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// Event handler for connection lost
        /// </summary>
        /// <param name="device">The device that lost connection</param>
        private void OnDisconnect(IDevice device)
        {
            if (device.Status != ConnectionStatus.Connecting)
            {
                this.NotifyDisconnected?.Invoke();
            }
        }

        #endregion
    }
}
