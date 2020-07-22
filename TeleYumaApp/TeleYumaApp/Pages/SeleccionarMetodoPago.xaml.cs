using PayPal.Forms;
using PayPal.Forms.Abstractions;
//using PayPal.Forms.Abstractions.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeleccionarMetodoPago : ContentPage
    {

        public bool isPayPal = false;

        public SeleccionarMetodoPago()
        {
            InitializeComponent();


            if (_Global.TipoRecarga == "movil")
            {
                lblMonto.Text = " " + String.Format("{0:#,##0.00}", _Global.ListaRecargas.MontoLista) + " USD";
                lblTotal.Text = " " + String.Format("{0:#,##0.00}", _Global.ListaRecargas.TotalPagar) + " USD";             
            }
            else if (_Global.TipoRecarga == "nauta")
            {
                lblMonto.Text = " " + String.Format("{0:#,##0.00}", _Global.RecargaNauta.monto) + " USD";
                lblTotal.Text = " " + String.Format("{0:#,##0.00}", _Global.RecargaNauta.TotalPagar) + " USD";
            }
        }

        private void btnTarjeta_Clicked(object sender, EventArgs e)
        {
            _Global.Vistas.Pagar.PagoPayPal = false;
            imgTarjeta.Source = "check";
            imgPaypal.Source = "uncheck";
        }

        private void btnPaypal_Clicked(object sender, EventArgs e)
        {
            _Global.Vistas.Pagar.PagoPayPal = true;
            imgPaypal.Source = "check";
            imgTarjeta.Source = "uncheck";
        }

        private void btnSiguiente_Clicked(object sender, EventArgs e)
        {
           
            _Global.Vistas.Pagar.ActualizarInformacionMonto();
            this.Navigation.PushAsync(_Global.Vistas.Pagar);
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
             

    }
}
