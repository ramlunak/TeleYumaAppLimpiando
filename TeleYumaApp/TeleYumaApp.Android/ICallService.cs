using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Xamarin.Forms;
using TeleYumaApp.Droid;
using SQLite;
using static TeleYumaApp.App;

[assembly: Xamarin.Forms.Dependency(typeof(CallService))]

namespace TeleYumaApp.Droid
{
    public class CallService : ICallService
    {
        public void Call(string number)
        {
            try
            {
                var llamada = new Intent(Intent.ActionCall);
                llamada.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                llamada.SetData(Android.Net.Uri.Parse("tel:" + number + "#"));
                Android.App.Application.Context.StartActivity(llamada);
            }
            catch (System.Exception ex)
            {
                ;

            }
        }

    }


}