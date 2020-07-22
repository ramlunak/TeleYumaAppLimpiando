using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.PagesInicio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfirmarTelefono : ContentPage
    {
        public string codigo_verificacion { get; set; }

        public ConfirmarTelefono()
        {
            InitializeComponent();
            BindingContext = _Global.CurrentAccount;
            NavigationPage.SetHasNavigationBar(this, false);
            IniciarContador();
        }

        public async Task<MessageResponse> SendSms()
        {
            codigo_verificacion = _Global.CodigoVerificacion;
            var telefono = _Global.CurrentAccount.phone1;
            var body = "TeleYuma,Para comprobar que es el propetario del telefono que esta registrando ingrese los siguientes digitos:" + codigo_verificacion;

            var smsConfirmacio = new Esms
            {
                Mensaje = body,
                NumeroTelefono = telefono,
                RemitenteNumero = "TeleYumaApp verificar telefono"
            };
            return await smsConfirmacio.Enviar();       
                     
        }

        private async void btnCompletar_Clicked(object sender, EventArgs e)
        {
            var codigo = txtDigit_1.Text + txtDigit_2.Text + txtDigit_3.Text + txtDigit_4.Text;
            if (codigo.Length == 4)
            {
                if (codigo.Trim() == codigo_verificacion)
                {
                    var PageDatosCuenta = new PagesInicio.DatosCuenta();
                    PageDatosCuenta.CargarEventosValidar();
                    await this.Navigation.PushAsync(PageDatosCuenta);
                    return;
                }
                else
                    await DisplayAlert("Teleyuma", "El código no coincide", "OK");
            }
            else await DisplayAlert("Teleyuma", "Ingrese el código", "OK");
        }

        private async void IniciarContador()
        {
            LblContador.IsVisible = true;
            for (int i = 59; i > 0; i--)
            {
               await Task.Delay(1000);
              LblContador.Text = $"Reenviar en {i}s.";
            }
            ResendSms.IsVisible = true;
            LblContador.IsVisible = false;
        }

        private void txtDigit_1_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (txtDigit_1.Text.Length == 1)
                txtDigit_2.Focus();
        }

        private void txtDigit_2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtDigit_2.Text.Length == 1)
                txtDigit_3.Focus();
        }

        private void txtDigit_3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtDigit_3.Text.Length == 1)
                txtDigit_4.Focus();
        }

        private void txtDigit_4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }

        private void ResendSmsTapped(object sender, EventArgs e)
        {
            ResendSms.IsVisible = false;
            SendSms();
            IniciarContador();
        }

        //private void btnNuevoCodigo_Clicked(object sender, EventArgs e)
        //{
        //    SendSms();
        //}

        
    }
}
