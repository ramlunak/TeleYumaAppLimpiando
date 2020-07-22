using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using System.Runtime.Remoting.Contexts;
using TeleYumaApp;
using TeleYumaApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace TeleYumaApp.Droid
{
    public class CustomPickerRenderer : PickerRenderer
    {
        CustomPicker element;

        public CustomPickerRenderer(Android.Content.Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            element = (CustomPicker)this.Element;
                       

            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
            {
                Control.Background = AddPickerStyles(element.Image);
                Control.SetHintTextColor(Android.Graphics.Color.Black);
            }
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            ShapeDrawable border = new ShapeDrawable();
            border.Paint.Color = Android.Graphics.Color.Gray;
            border.SetPadding(10, 10, 10, 10);
            border.Paint.SetStyle(Paint.Style.Stroke);
            Drawable[] layers = {
                            border,
                            GetDrawable(imagePath)
                        };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);
            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            var drawable = Resources.GetDrawable(imagePath);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;
            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 70, 70, true));
            result.Gravity = Android.Views.GravityFlags.Right;
            return result;
        }
    }
}


    //protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
    //{
    //    base.OnElementChanged(e);

    //    element = (CustomPicker)this.Element;

    //    if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
    //        Control.Background = AddPickerStyles(element.Image);

    //}

    //public LayerDrawable AddPickerStyles(string imagePath)
    //{
    //    ShapeDrawable border = new ShapeDrawable();
    //    border.Paint.Color = Android.Graphics.Color.Transparent;
    //    border.SetPadding(10, 10, 10, 10);
    //    border.Paint.SetStyle(Paint.Style.Stroke);

    //    Drawable[] layers = { border, GetDrawable(imagePath) };
    //    LayerDrawable layerDrawable = new LayerDrawable(layers);
    //    layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

    //    return layerDrawable;
    //}

    //private BitmapDrawable GetDrawable(string imagePath)
    //{
    //    int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
    //    var drawable = ContextCompat.GetDrawable(this.Context, resID);
    //    var bitmap = ((BitmapDrawable)drawable).Bitmap;

    //    var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 70, 70, true));
    //    result.Gravity = Android.Views.GravityFlags.Right;

    //    return result;
    //}




//using PickerDemo.CustomControls;  
//using PickerDemo.iOS;  
//using UIKit;  
//using Xamarin.Forms;  
//using Xamarin.Forms.Platform.iOS;  
//[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]  
//namespace PickerDemo.iOS
//{
//    public class CustomPickerRenderer : PickerRenderer
//    {
//        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
//        {
//            base.OnElementChanged(e);
//            var element = (CustomPicker)Element;
//            if (Control != null && Element != null && !string.IsNullOrEmpty(element.Icon))
//            {
//                var downarrow = UIImage.FromBundle(element.Icon);
//                RightViewMode = UITextFieldViewMode.Always;
//                RightView = new UIImageView(downarrow);
//                TextColor = UIColor.From#533f95;   
////Control.BackgroundColor = UIColor.Clear;  
//                AttributedPlaceholder = new Foundation.NSAttributedString(Control.AttributedPlaceholder.Value, foregroundColor: UIColor.FromRGB(83, 63, 149);
//                //Control.BorderStyle = UITextBorderStyle.RoundedRect;  
//                //Layer.BorderWidth = 1.0f;   
//                //Layer.CornerRadius = 4.0f;   
//                //Layer.MasksToBounds = true;   
//                //Layer.BorderColor = UIColor.FromRGB(83,63,149.CGColor;   
//            }
//        }
//    }
//}