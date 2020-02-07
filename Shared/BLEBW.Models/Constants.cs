// <copyright file="Constants.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssue.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Cross app identifiers
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The identifier of the service containing the communication characteristics
        /// </summary>
        public const string ServiceUUID = "79CB4F3A-1DFC-4B32-9DE3-24A86FCCAECF";

        /// <summary>
        /// The identifier of the image characteristic
        /// </summary>
        public static readonly Guid ImageCharacteristic = new Guid("4C71BB1A-CEEC-4746-90D1-37A36F6A41C9");

        /// <summary>
        /// The identifier of the image received characteristic
        /// </summary>
        public static readonly Guid ImageReceivedCharacteristic = new Guid("425E724B-C72D-4EF2-88CF-07AC9712C9D5");
    }
}
