// <copyright file="MainActivity.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueServerApp.Droid
{
    using System;

    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using Android.Widget;
    using BluetoothLEBlobWriteIssueServerApp.Views;
    using Plugin.Permissions;

    /// <summary>
    /// The main activity of the Android app
    /// </summary>
    [Activity(Label = "BLE Test App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// Overridden method called with the results of a permission request 
        /// </summary>
        /// <param name="requestCode">The request code</param>
        /// <param name="permissions">The permission requested</param>
        /// <param name="grantResults">The results of the grant indicating whether the grant was approved or not</param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /// <summary>
        /// Called when the activity is created (see lifecycle)
        /// </summary>
        /// <param name="savedInstanceState">The saved state data</param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            MainActivity.TabLayoutResource = Resource.Layout.Tabbar;
            MainActivity.ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            this.LoadApplication(new App());
        }
    }
}