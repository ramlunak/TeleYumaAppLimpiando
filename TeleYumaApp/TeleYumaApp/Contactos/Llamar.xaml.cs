

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using static TeleYumaApp.App;

namespace TeleYumaApp.Contactos
{
    public partial class Llamar : ContentPage
    {
        public Llamar()
        {
            InitializeComponent();
            this.BindingContext = _Global.VM.VMLlamar;
        }

       
        public void LlenarTxtTelefono(string numero)
        {
            //txtTelefono.Text = numero;
        }

        void Seleccionar_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                _Global.Vistas.ListaContactos.LlenarLista();
                _Global.Vistas.ListaContactos.Transaction = TipoTransaction.Llamar;
                this.Navigation.PushAsync(_Global.Vistas.ListaContactos);
               // _Global.Vistas.ListaContactos.txtLlamar(ref txtTelefono);
            }
            catch
            {
                ;
            }
        }

        private async void BtnLlamar_Clicked(object sender, EventArgs e)
        {
            if (txtTelefono.Text == "" || txtTelefono.Text == null)
            {
                DisplayAlert("TeleYuma", "Escriba el número", "Ok");
                return;
            }

            try
            {
                var numero = txtTelefono.Text;
                var llamada = "+551142805356,011" + numero;
                DependencyService.Get<ICallService>().Call(llamada);

            }
            catch (Exception ex)
            {

                ;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
          
        }

        private void LabelNumber_Tapped(object sender, EventArgs e)
        {

        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
           
        }

        private void txtTelefono_Unfocused(object sender, FocusEventArgs e)
        {
            ;
        }

        private void txtTelefono_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
