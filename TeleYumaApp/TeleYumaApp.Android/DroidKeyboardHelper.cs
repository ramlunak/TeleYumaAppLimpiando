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
using TeleYumaApp.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DroidKeyboardHelper))]

namespace TeleYumaApp.Droid
{
    public class DroidKeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            //var context = Forms.Context;
            //var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            //if (inputMethodManager != null && context is Activity)
            //{
            //    var activity = context as Activity;
            //    var token = activity.CurrentFocus?.WindowToken;
            //    inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);
            //    activity.Window.DecorView.ClearFocus();
            //}

          
        }

        //public class iOSKeyboardHelper : IKeyboardHelper
        //{
        //    public void HideKeyboard()
        //    {
        //        UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        //    }
        //}

    }

}