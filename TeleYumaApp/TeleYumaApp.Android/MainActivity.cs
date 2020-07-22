using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using static TeleYumaApp.App;
using Android.Content;
using Xamarin.Forms;
using static TeleYumaApp.Droid.MainActivity;
using PayPal.Forms.Abstractions;

using PayPal.Forms;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Util;
using Android.Nfc;
using Android.Support.Design.Widget;
using Java.Lang;

using System.Threading.Tasks;
using System.IO;

using Android.Gms.Common;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Platform;

namespace TeleYumaApp.Droid
{

    [Activity(Label = "TeleYumaApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new TeleYumaApp.App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {

        }

    }
}

