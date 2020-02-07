// <copyright file="MainViewModel.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BLEBWS.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using BLEBWS.ServiceInterfaces;
    using BLEBWS.ViewModelInterfaces;
    using Vssl.Samples.ViewModels;

    /// <summary>
    /// Displays the last transferred image
    /// </summary>
    public class MainViewModel : BaseViewModel, IMainViewModel
    {
        /// <summary>
        /// Runs a bluetooth LE service with characteristics that can interact with a sidekick client
        /// </summary>
        private IBTService bluetoothLEService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="bluetoothLEService">Runs a bluetooth LE service with characteristics that can interact with a sidekick client</param>
        public MainViewModel(IBTService bluetoothLEService)
        {
            this.bluetoothLEService = bluetoothLEService;
            this.bluetoothLEService.NotifyBytesReceived += (s, args) =>
            {
                this.BytesReceived = args.ByteCount.ToString();
                this.OnPropertyChanged("BytesReceived");
            };

            this.bluetoothLEService.NotifyNewImage += (s, args) =>
            {
                this.LastImage = args.Image.BinaryData;
                this.OnPropertyChanged("LastImage");
            };
        }

        /// <summary>
        /// Gets the last image transmitted from the client
        /// </summary>
        public byte[] LastImage { get; private set; }

        /// <summary>
        /// Gets the number of bytes received from the client
        /// </summary>
        public string BytesReceived { get; private set; }

        /// <summary>
        /// Called when the page is navigated to
        /// </summary>
        public void OnNavigatedTo()
        {
            bool isStarted = true;
            if (!this.bluetoothLEService.IsStartedOrStarting)
            {
                if (this.bluetoothLEService.CanStartServer())
                {
                    Task.Run(async () =>
                    {
                        await this.bluetoothLEService.StartServer();
                        this.bluetoothLEService.StartAdvertiser();
                    });
                }
                else
                {
                    isStarted = false;
                }
            }
        }
    }
}
