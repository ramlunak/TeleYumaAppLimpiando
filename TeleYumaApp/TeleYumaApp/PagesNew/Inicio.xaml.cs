using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using TeleYumaApp.Teleyuma;
using Xamarin.Forms;


namespace TeleYumaApp.PagesNew
{
    public partial class Inicio : ContentPage
    {      
        public Inicio()
        {
            try
            {
                InitializeComponent();
                BindingContext = _Global.VM.VMInicio = new ViewModels.VMInicio(imgPerfil, null, null);
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
          
        }


        //void Handle_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        //{
        //    Debug.WriteLine("Position " + e.NewValue + " selected.");
        //}

        //void Handle_Scrolled(object sender, CarouselView.FormsPlugin.Abstractions.ScrolledEventArgs e)
        //{
        //    Debug.WriteLine("Scrolled to " + e.NewValue + " percent.");
        //    Debug.WriteLine("Direction = " + e.Direction);
        //}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Pages.TransferenciaBancaria());
        }
    }
}
