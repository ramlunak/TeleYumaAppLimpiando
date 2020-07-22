using Xamarin.Forms;
namespace TeleYumaApp
{
    public class CustomViewCell : ViewCell
    {  
        public static readonly BindableProperty SelectedItemBackgroundColorProperty 
            = BindableProperty.Create("SelectedItemBackgroundColor", typeof(Color), typeof(CustomViewCell),Color.White);
        public Color SelectedItemBackgroundColor
        {
            get
            {
                return (Color)GetValue(SelectedItemBackgroundColorProperty);
            }
            set
            {
                SetValue(SelectedItemBackgroundColorProperty, value);
            }
        }
    }
}