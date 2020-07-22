using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TeleYumaApp.rlkControles
{
    public class rlkLabelButton : Label
    {
        public rlkLabelButton() : base()
        {
            const int _animationTime = 2;
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
              Command = new Command(async (t) => {
                  await this.FadeTo(0.5, 150,Easing.CubicOut);
                  await this.ScaleTo(0.90, 150, Easing.CubicOut);
                  await this.FadeTo(1, 150, Easing.CubicIn);
                  await this.ScaleTo(1, 50, Easing.CubicIn);
              })
            });


           // var iconTap = new TapGestureRecognizer();
            //iconTap.Tapped += async (sender, e) =>
            //{
            //    try
            //    {
            //        var btn = (rlkImageButton)sender;
            //        await btn.ScaleTo(5, _animationTime);
            //        //await btn.ScaleTo(1, _animationTime);
            //    }
            //    catch (Exception ex)
            //    {
            //        ;
            //    }
            //};

        }
    }
}
