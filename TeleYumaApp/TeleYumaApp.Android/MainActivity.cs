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
using AndroidNotification;

using TeleYumaApp.Class;
using System.Threading.Tasks;
using System.IO;

using Android.Gms.Common;
using TeleYumaApp.Droid.Renderers;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Platform;

namespace TeleYumaApp.Droid
{

    [Activity(Label = "TeleYumaApp", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Context CurrentContext { get; set; }
        internal static MainActivity Instance { get; private set; }
        public static Activity CurrentActivity { get; set; }
        public string Pantalla { get; set; }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            try
            {
                CurrentContext = activity;
                CurrentActivity = activity;
            }
            catch (System.Exception ex)
            { 
                App.Current.MainPage.DisplayAlert("System",ex.Message,"ok");
            }
        }

        protected override void OnCreate(Bundle bundle)
        {

            try
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;
                base.OnCreate(bundle);
                CarouselViewRenderer.Init();
                CachedImageRenderer.Init(true);
            }
            catch (System.Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }


            try
            {
                if (Intent.Extras != null)
                {
                    foreach (var key in Intent.Extras.KeySet())
                    {
                        if (key != null)
                        {
                            var value = Intent.Extras.GetString(key);
                            Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                        }
                    }
                }

                IsPlayServicesAvailable();
                CreateNotificationChannel();

                global::Xamarin.Forms.Forms.Init(this, bundle);

                //need this line to init effect in android
                Xamarin.KeyboardHelper.Platform.Droid.Effects.Init(this);

                CarouselView.FormsPlugin.Android.CarouselViewRenderer.Init();
                DependencyService.Register<PicturePickerImplementation>();
                DependencyService.Register<CallService>();
                DependencyService.Register<CloseApplication>();
            }
            catch (System.Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }

            try
            {
                var config = new PayPalConfiguration(PayPalEnvironment.Production, "Ae4zGNWtak5RGzB1HXRLmUwDqMgpBd_8oxMZZ_CQNiqKZyHTdjudpoDqMDs69XMdHcgi7NUCDpWFLizK")
                {
                    //If you want to accept credit cards
                    AcceptCreditCards = false,
                    //Your business name
                    MerchantName = "TeleYuma",
                    //Your privacy policy Url
                    MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
                    //Your user agreement Url
                    MerchantUserAgreementUri = "https://www.example.com/legal",
                    // OPTIONAL - ShippingAddressOption (Both, None, PayPal, Provided)
                    ShippingAddressOption = ShippingAddressOption.Both,
                    // OPTIONAL - Language: Default languege for PayPal Plug-In
                    Language = "es",
                    // OPTIONAL - PhoneCountryCode: Default phone country code for PayPal Plug-In
                    //PhoneCountryCode = "1",
                };

                CrossPayPalManager.Init(config, this);
            }
            catch (System.Exception ex)
            {
                ;
            }

            try
            {

                if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadContacts) == (int)Permission.Granted)
                    && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.CallPhone) == (int)Permission.Granted)
                     && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted))
                {
                    var IdSms = this.Intent.GetStringExtra("sms");
                    var open = _Global.IsOpen;

                    if (IdSms == null)
                    {
                        // startService();
                    }

                    LoadApplication(new TeleYumaApp.App());

                }
                else
                {
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadContacts, Manifest.Permission.CallPhone, Manifest.Permission.ReadExternalStorage }, 0);

                }

                //if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadContacts) == (int)Permission.Granted)                    
                //     && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == (int)Permission.Granted))
                //{
                //    var IdSms = this.Intent.GetStringExtra("sms");
                //    var open = _Global.IsOpen;

                //    if (IdSms == null)
                //    {
                //        // startService();
                //    }

                //    LoadApplication(new TeleYumaApp.App());

                //}
                //else
                //{
                //    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadContacts,Manifest.Permission.ReadExternalStorage }, 0);

                //}
            }
            catch (System.Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {

            if ((grantResults[0] == Permission.Granted) && (grantResults[1] == Permission.Granted) && (grantResults[2] == Permission.Granted))
            {
                LoadApplication(new TeleYumaApp.App());
            }
            else
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage("La aplicaión necesita permisos de llamada y lectura de contáctos")
                   .SetCancelable(false)
                   .SetNeutralButton("Ok", delegate
                   {
                       Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                   });
                AlertDialog alert = builder.Create();
                alert.Show();
            }
        }


        #region 

        public const string TAG = "MainActivity";
        public static string CHANNEL_ID = "com.teleyuma.chanel";
             
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            return true;
        }
        
        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = CHANNEL_ID;
            var channelDescription = string.Empty;
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
    

        #endregion

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

      
        public void createNotiChanel()
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                ICharSequence noti_chan = new Java.Lang.String("noti_chan_urgent");
                var importance = NotificationImportance.High;
                NotificationChannel channel = new NotificationChannel(CHANNEL_ID, noti_chan, importance);
                channel.Description = "teleyuma.noti.sms";

                NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);

            }

        }
               
        protected override void OnDestroy()
        {
            try
            {
                base.OnDestroy();

                PayPalManagerImplementation.Manager.Destroy();
            }
            catch { }

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }


        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }
        public byte[] ImageByteArray { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
            else
            {
                PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, intent);
            }
        }

        public class CloseApplication : ICloseApplication
        {
                      
            public void closeApplication()
            {
                //var activity = (Activity)Forms.Context;
                //activity.FinishAffinity();
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());

            }
        }

      


    }
}

