using System.ComponentModel;
using ListViewDemo.Droid.CustomControls;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TeleYumaApp;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace ListViewDemo.Droid.CustomControls
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {

         public void bh()
        {
            var lv = new CustomEntry();
            
        }
        private Android.Views.View _cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);
            _selected = false;
            _unselectedBackground = _cellCore.Background;
            return _cellCore;                       
        }
        
        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            
            base.OnCellPropertyChanged(sender, args);
            if (args.PropertyName == "IsSelected")
            {
                _selected = !_selected;
                _cellCore.SetBackgroundColor(Android.Graphics.Color.White);
                //if (_selected)
                //{
                //    var extendedViewCell = sender as CustomViewCell;
                //    _cellCore.SetBackgroundColor(extendedViewCell.SelectedItemBackgroundColor.ToAndroid());
                //}
                //else
                //{
                //    _cellCore.SetBackground(_unselectedBackground);
                //}
            }
        }
               
        
    }
}