using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Plugin.CurrentActivity;
using TeleYumaApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidForceKeyboardDismissalService))]

namespace TeleYumaApp.Droid
{
    public class AndroidForceKeyboardDismissalService : IForceKeyboardDismissalService
    {
        public void DismissKeyboard()
        {
            InputMethodManager imm = InputMethodManager.FromContext(Android.App.Application.Context);

            imm.HideSoftInputFromWindow(CrossCurrentActivity.Current.Activity.Window.DecorView.WindowToken, HideSoftInputFlags.NotAlways);
        }
    }

}