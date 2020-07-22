using PayPal.Forms;
using PayPal.Forms.Abstractions;

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
    public partial class TransferenciaBancaria : ContentPage
    {

        public bool isPayPal = false;
        
        public TransferenciaBancaria()
        {
            InitializeComponent();
            Limpiar();
        }            

        public void Limpiar(){
            pkr_Monto.SelectedIndex = -1;
            lblTotal.Text = "";
        }

        private void pkr_Monto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkr_Monto.SelectedIndex != -1)
            {
                var value = _Global.Round_2(pkr_Monto.Items[pkr_Monto.SelectedIndex]);
                lblTotal.Text = " "+ String.Format("{0:#,##0.00}", value) + " USD";

            }
        }

        private void btnTarjeta_Clicked(object sender, EventArgs e)
        {
            isPayPal = false;
            imgTarjeta.Source = "check";
            imgPaypal.Source = "uncheck";
        }

        private void btnPaypal_Clicked(object sender, EventArgs e)
        {
            isPayPal = true;
            imgPaypal.Source = "check";
            imgTarjeta.Source = "uncheck";
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Clicked(object sender, EventArgs e)
        {

            if (pkr_Monto.SelectedIndex != -1)
            {
                var monto = pkr_Monto.Items[pkr_Monto.SelectedIndex];
                _Global.MontoTransferenciaBancaria = Convert.ToInt32(monto);
                if (isPayPal)
                {
                    PayPalPayment();
                }
                else
                {
                    _Global.TipoRecarga = "agregar_saldo";
                   _Global.Vistas.Pagar.ActualizarInformacionMonto();
                    this.Navigation.PushAsync(_Global.Vistas.Pagar);
                }

                Limpiar();
            }
            else DisplayAlert("TeleYuma", "Seleccione el monto de la transferencia", "OK");
        }

        public async void PayPalPayment()
        {
            var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Agregar Saldo", new Decimal(_Global.MontoTransferenciaBancaria), "USD"), new Decimal(0));
            if (result.Status == PayPalStatus.Cancelled)
            {
                await DisplayAlert("TeleYuma", "Pago Cancelado", "OK");
            }
            else if (result.Status == PayPalStatus.Error)
            {
                await DisplayAlert("TeleYuma", result.ErrorMessage, "OK");
            }
            else if (result.Status == PayPalStatus.Successful)
            {
                await DisplayAlert("TeleYuma", "Gracias, su pago se realizó correctamente", "OK");

                //Poner dinero en la cuenta manual
                await _Global.CurrentAccount.MakeTransaction_ManualPayment(new Decimal(_Global.MontoTransferenciaBancaria), "Agregar Saldo");

                //Regrasar a pagina HomePage
                var pages = Navigation.NavigationStack.ToList();
                foreach (var page in pages)
                {
                    if (page.GetType() != typeof(Pages.HomeTabbedPage))
                        Navigation.RemovePage(page);
                }
                //------------------------
            }
        }


    }
}
