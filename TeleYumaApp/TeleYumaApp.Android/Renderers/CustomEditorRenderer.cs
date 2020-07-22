using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using CustomRenderer.Android;
using TeleYumaApp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExpandableEditor), typeof(CustomEditorRenderer))]
namespace CustomRenderer.Android
{
    [Obsolete]
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(global::Android.Graphics.Color.Transparent);
                this.Control.SetBackgroundDrawable(gd);
                this.Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                //  Control.SetHintTextColor(ColorStateList.ValueOf(global::Android.Graphics.Color.White));
            }
        }
    }
}


// [assembly: ExportRenderer (typeof(CustomEditor), typeof(CustomEditorRenderer))]
//        namespace CustomRenderer.iOS
//{
//    public class CustomEditorRenderer : EditorRenderer
//    {
//        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
//        {

//            base.OnElementChanged(e);
//            if (Control != null)
//            {

//                Control.BorderStyle = UITextBorderStyle.None;
//                //Control.Layer.CornerRadius = 10;
//                //Control.TextColor = UIColor.White;
//            }
//        }
//    }
//}