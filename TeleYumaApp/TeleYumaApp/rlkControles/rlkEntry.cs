using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TeleYumaApp.rlkControles
{
    public class entry : Entry
    {
        public entry()
        {
            this.HeightRequest = 50;
        }

        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(entry), string.Empty, BindingMode.TwoWay);

        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create(nameof(LineColor), typeof(Xamarin.Forms.Color), typeof(entry), Color.White);

        public static readonly BindableProperty ImageHeightProperty =
            BindableProperty.Create(nameof(ImageHeight), typeof(int), typeof(entry), 40);

        public static readonly BindableProperty ImageWidthProperty =
            BindableProperty.Create(nameof(ImageWidth), typeof(int), typeof(entry), 40);

        public static readonly BindableProperty ImageAlignmentProperty =
            BindableProperty.Create(nameof(ImageAlignment), typeof(ImageAlignment), typeof(entry), ImageAlignment.Left);

        public static readonly BindableProperty MinValueProperty =
            BindableProperty.Create(nameof(MinValue), typeof(int), typeof(entry), int.MinValue);

        public static readonly BindableProperty MaxValueProperty =
           BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(entry), int.MaxValue);

        public static readonly BindableProperty IsValidProperty =
           BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(entry), false);

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public int ImageWidth
        {
            get { return (int)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public int ImageHeight
        {
            get { return (int)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); OnPropertyChanged(); }
        }

        public ImageAlignment ImageAlignment
        {
            get { return (ImageAlignment)GetValue(ImageAlignmentProperty); }
            set { SetValue(ImageAlignmentProperty, value); }
        }
    }

    public enum ImageAlignment
    {
        Left,
        Right
    }
}
