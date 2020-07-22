using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TeleYumaApp.Droid
{
    class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://teleyumahub.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=QWMCW7RsROQTelnIDCvKBI4LJaH0HTfNxMFYHOO0kq0=";
        public const string NotificationHubName = "teleyumahub";
    }
}