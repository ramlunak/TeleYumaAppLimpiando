using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using System.Runtime.Remoting.Contexts;
using TeleYumaApp;
using TeleYumaApp.Droid;
using TeleYumaApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Context = Android.Content.Context;

[assembly: ExportRenderer(typeof(ScrollableListView), typeof(CustomListViewDroid))]

namespace TeleYumaApp.Droid.Renderers
{
    public class CustomListViewDroid : ListViewRenderer
    {
        public CustomListViewDroid(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var view = (ScrollableListView)Element;

                
                this.Control.ItemLongClick += (s, args) =>
                {
                    this.Control.SetItemChecked(args.Position, true);
                    Java.Lang.Object item = this.Control.GetItemAtPosition(args.Position);
                    view.OnLongClicked(item.GetType().GetProperty("Instance").GetValue(item), args.Position);
                };
            }
        }

        //protected override void OnElementChanged(ElementChangedEventArgs e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.OldElement == null)
        //    {
        //        var view = (CustomListView)Element;

        //        this.Control.ItemLongClick += (s, args) =>
        //        {
        //            this.Control.SetItemChecked(args.Position, true);
        //            Java.Lang.Object item = this.Control.GetItemAtPosition(args.Position);
        //            view.OnLongClicked(item.GetType().GetProperty("Instance").GetValue(item));
        //        };
        //    }
        //}

    }
}