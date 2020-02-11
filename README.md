# BluetoothLEBlobWriteIssue
Reproduces an issue with BlobWrites between iOS and Android

This has been raised as issue #366 raised on Plugin.BluetoothLE nugget package who’s project Url is https://github.com/aritchie/bluetoothle

The sample is a bit verbose, but that reflects the structure of the app that it was extracted from.
The objective of the sample is to transfer a photo taken on the client to the server over Bluetooth LE.

As currently coded this fails with only three segments of the blob reaching the server.