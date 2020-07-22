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
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Java.Lang;
using Android.Support.V4.App;
using TeleYumaApp.Class;
using TeleYumaApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationService))]
namespace TeleYumaApp.Droid
{

    public class NotificationService : IPushNotification
    {

        public bool isNotification = false;

        const int NOTIFICATION_ID = 9000;

        public static string CHANEL_ID = "com.teleyuma.chanel";
        
        public void SetNotification(Esms sms)
        {

            Intent notiIntent = new Intent(MainActivity.CurrentContext, typeof(MainActivity));
            notiIntent.PutExtra("sms", "1");
            PendingIntent pendingIntent = PendingIntent.GetActivity(MainActivity.CurrentContext, 0, notiIntent, PendingIntentFlags.UpdateCurrent);
            NotificationCompat.Builder notification = new NotificationCompat.Builder(MainActivity.CurrentContext, MainActivity.CHANNEL_ID)
                .SetContentTitle("Service")
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true)
                .SetContentText("El servicio de notificaciones está ejecutandose.")                
                .SetSmallIcon(Resource.Drawable.ic_announcement_black_48dp);                
          
            // Get the notification manager:
            NotificationManager notificationManager =
               MainActivity.CurrentContext.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:

            notificationManager.Notify(sms.Id, notification.Build());


        }
    }

}