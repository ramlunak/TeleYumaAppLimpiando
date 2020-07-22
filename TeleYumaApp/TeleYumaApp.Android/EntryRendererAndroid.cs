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

[assembly: ExportRenderer(typeof(CustomEntry), typeof(EntryRendererAndroid))]

namespace TeleYumaApp.Droid
{
    
    class EntryRendererAndroid : EntryRenderer
    {
        CustomEntry element;

        public EntryRendererAndroid(Context element) : base(element)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (CustomEntry)this.Element;


            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }

            editText.CompoundDrawablePadding = 10;
            Control.Background.SetColorFilter(Android.Graphics.Color.Transparent, PorterDuff.Mode.SrcAtop);


            //editText.CompoundDrawablePadding = 30;
            //var gradientDrawable = new GradientDrawable();
            //gradientDrawable.SetCornerRadius(15f);
            //gradientDrawable.SetStroke(3, Android.Graphics.Color.White);
            //gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
            //Control.SetBackground(gradientDrawable);
        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
        }
    }
}