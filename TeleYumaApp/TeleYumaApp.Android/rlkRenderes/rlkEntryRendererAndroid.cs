using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Text;
using Android.Views;
using Android.Widget;
using TeleYumaApp.Droid.rlkRenderes;
using TeleYumaApp.rlkControles;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(entry), typeof(RlkEntryRendererAndroid))]

namespace TeleYumaApp.Droid.rlkRenderes
{
    class RlkEntryRendererAndroid : EntryRenderer
    {
        entry element;

        public RlkEntryRendererAndroid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (entry)this.Element;

            element.TextChanged += Element_TextChanged;

            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                //switch (element.ImageAlignment)
                //{
                //    case ImageAlignment.Left:
                //        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                //        break;
                //    case ImageAlignment.Right:
                //        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                //        break;
                //}
            }

            
            //editText.CompoundDrawablePadding = 0;
            //Control.Background.SetColorFilter(Android.Graphics.Color.Transparent, PorterDuff.Mode.SrcAtop);
            Control.SetPadding(0,0,0,0);
           
            editText.CompoundDrawablePadding = 0;
            editText.SetPadding(0,0,0,0) ;
            var gradientDrawable = new GradientDrawable();
           // gradientDrawable.SetCornerRadius(15f);
           // gradientDrawable.SetStroke(3, Android.Graphics.Color.White);
            gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
            Control.SetBackground(gradientDrawable);
        }

        private void Element_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var obj = ((entry)sender);
                if (obj.Keyboard == Keyboard.Numeric)
                {
                    try
                    {

                        if (Convert.ToInt32(e.NewTextValue) > obj.MinValue && Convert.ToInt32(e.NewTextValue) < obj.MaxValue)
                            obj.IsValid = true;
                        else
                            obj.IsValid = true;

                    }
                    catch
                    {
                        ((entry)sender).IsValid = false;

                    }

                }
            }
            catch 
            {

               
            }
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