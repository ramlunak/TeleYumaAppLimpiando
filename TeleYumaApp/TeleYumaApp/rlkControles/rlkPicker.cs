using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TeleYumaApp.rlkControles
{
    public class picker : Picker
    {
        public static readonly BindableProperty ImageProperty =
             BindableProperty.Create(nameof(Image), typeof(string), typeof(picker), string.Empty);

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }

}
