using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.rlkControles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class rlkButtonSeguridad : ContentView
    {
        public rlkButtonSeguridad() : base()
        {
            InitializeComponent();

            const int _animationTime = 2;
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async (t) => {
                    await this.FadeTo(0.5, 150, Easing.CubicOut);
                    await this.ScaleTo(0.90, 150, Easing.CubicOut);
                    await this.FadeTo(1, 150, Easing.CubicIn);
                    await this.ScaleTo(1, 50, Easing.CubicIn);
                })
            });
        }
    }
}