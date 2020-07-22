using System;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static Android.Resource;
using Android.Content;
using TeleYumaApp;
using TeleYumaApp.Droid;
using Android.Views.InputMethods;

[assembly: ExportRenderer(typeof(EntryKyboard), typeof(EntryKyboardRendererAndroids))]

namespace TeleYumaApp.Droid
{

    class EntryKyboardRendererAndroids : EntryRenderer
    {
      
        public EntryKyboardRendererAndroids(Context element) : base(element)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            //base.OnElementChanged(e);

            //if (e.OldElement != null || e.NewElement == null)
            //    return;

            //element = (EntryKyboard)this.Element;
            base.OnElementChanged(e);
            if (Control != null)
            {
                try
                {
                    Control.Click += (sender, evt) =>
                    {
                        new Handler().Post(delegate
                        {
                            var imm = (InputMethodManager)Control.Context.GetSystemService(Android.Content.Context.InputMethodService);
                            var result = imm.HideSoftInputFromWindow(Control.WindowToken, 0);
                        });
                    };

                    Control.TextChanged += (sender, evt) =>
                    {
                        new Handler().Post(delegate
                        {
                            var imm = (InputMethodManager)Control.Context.GetSystemService(Android.Content.Context.InputMethodService);
                            var result = imm.HideSoftInputFromWindow(Control.WindowToken, 0);
                        });
                    };

                    Control.FocusChange += (sender, evt) =>
                    {
                        new Handler().Post(delegate
                        {
                            var imm = (InputMethodManager)Control.Context.GetSystemService(Android.Content.Context.InputMethodService);
                            var result = imm.HideSoftInputFromWindow(Control.WindowToken, 0);
                        });
                    };
                }
                catch { } ;
            }

        }

    }
}