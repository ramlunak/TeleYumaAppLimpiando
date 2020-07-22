using Newtonsoft.Json;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pagar : ContentPage
    {

        public bool PagoPayPal = false;

        public Pagar()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMPagar;
            listRecargas.ItemsSource = _Global.ListaRecargas.Lista;
        }


        public void ActualizarInformacionMonto()
        {
            _Global.VM.VMPagar.ActualizarInformacionMonto();
            //if (_Global.TipoRecarga == "movil")
            //{
            //    lblMonto.Text = " " + String.Format("{0:#,##0.00}", _Global.ListaRecargas.MontoLista) + " USD";
            //    lblTotal.Text = " " + String.Format("{0:#,##0.00}", _Global.ListaRecargas.TotalPagar) + " USD";

            //}
            //else if (_Global.TipoRecarga == "nauta")
            //{
            //    Monto = " " + String.Format("{0:#,##0.00}", _Global.RecargaNauta.monto) + " USD";
            //    Monto = " " + String.Format("{0:#,##0.00}", _Global.RecargaNauta.TotalPagar) + " USD";
            //}
            //else if (_Global.TipoRecarga == "agregar_saldo")
            //{
            //    Monto = " " + String.Format("{0:#,##0.00}", _Global.MontoTransferenciaBancaria) + " USD";
            //    Monto = " " + String.Format("{0:#,##0.00}", _Global.MontoTransferenciaBancaria) + " USD";
            //}

            if (!PagoPayPal)
                VerificarTarjeta();
        }

        public async void ActualizarLbabelNumero()
        {
            var PaymentMethodInfo = await _Global.CurrentAccount.PaymentMethodInfo();
            if (PaymentMethodInfo.payment_method_info != null)
            {
                _Global.VM.VMPagar.NumeroTarjeta = PaymentMethodInfo.payment_method_info.number;
            }
        }

        public void ActualizarListViewRecargas()
        {
            // listRecargas.ItemsSource = null;
            // listRecargas.ItemsSource = _Global.ListaRecargas.Lista;
        }

        public async Task<bool> VerificarTarjeta()
        {

            var PaymentMethodInfo = await _Global.CurrentAccount.PaymentMethodInfo();
            if (PaymentMethodInfo.payment_method_info != null)
            {
                _Global.VM.VMPagar.NumeroTarjeta = PaymentMethodInfo.payment_method_info.number;
                return true;
            }
            else
            {
                await this.Navigation.PushAsync(new Cuenta.TarjetaCredito());
                return false;
            }

        }

        private void OtraTarjeta_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new Cuenta.TarjetaCredito());

        }

        public void MostrarCargando(bool value)
        {
            ActivityIndicator.IsVisible = value;
            LabelAccion.IsVisible = value;

            LayoutCargando.IsVisible = value;
            LayoutPagar.IsVisible = !value;

        }

        private async void Pagar_clicked(object sender, EventArgs e)
        {

            if (!await VerificarTarjeta())
            {
                return;
            }
            if (_Global.CurrentAccount.phone2 != EstadoTarjeta.TarjetaComprovada.ToString())
            {
                var array = lblNumeroTarjeta.Text.Split('x');
                var ultimosnumeros = array.Last();
                var result = await DisplayAlert("Una cosa más", "Debemos verificar su tarjeta que termina en (" + ultimosnumeros + ") para asegurarnos que usted es el propetario de esta tarjeta", "ENTIENDO", "CANCELAR");
                if (result)
                    await this.Navigation.PushAsync(new Cuenta.VerificarTarjeta());
                return;
            }

            if (_Global.TipoRecarga == "agregar_saldo")
            {
                MostrarCargando(true);
                await _Global.CurrentAccount.MakeTransaction_EcommercePayment(Convert.ToInt32(_Global.MontoTransferenciaBancaria), "Agregar Saldo");

                if ((_Global.TransactionResponse.result_code == "1" || _Global.TransactionResponse.result_code == "A01") && _Global.TransactionResponse.transaction_id != "0")
                {
                    //_Global.Vistas.PageHome.ActualizarInformacionCuenta();

                    MostrarCargando(false);
                    await DisplayAlert("TeleYuma", "Gracias, su pago se realizó correctamente", "OK");

                    //Regrasar a pagina HomePage
                    var pages = Navigation.NavigationStack.ToList();
                    foreach (var page in pages)
                    {
                        if (page.GetType() != typeof(Pages.HomeTabbedPage))
                            Navigation.RemovePage(page);
                    }
                    //------------------------
                }
                else
                {
                    MostrarCargando(false);
                    await DisplayAlert("TeleYuma", "Revise la información de su cuenta bancaria o contacte al servicio técnico", "OK");
                }
            }

            if (_Global.TipoRecarga == "movil")
            {
                MostrarCargando(true);

                await _Global.CurrentAccount.MakeTransaction_EcommercePayment(_Global.ListaRecargas.TotalPagar, "Lista de Recargas");

                if ((_Global.TransactionResponse.result_code == "1" || _Global.TransactionResponse.result_code == "A01") && _Global.TransactionResponse.transaction_id != "0")
                {
                    if (_Global.AccionRecarga == "Reserva")
                    {
                        ReservarRecargas(_Global.TransactionResponse);
                        MostrarCargando(false);
                        return;
                    }
                    var termino = await RecargarMovil(_Global.TransactionResponse);
                    ActualizarListViewRecargas();
                }
                else
                {
                    await DisplayAlert("TeleYuma", "Revise la información de su cuenta bancaria o contacte al servicio técnico", "Ok");
                    MostrarCargando(false);
                    return;
                }

                _Global.VM.VMCompras.Compras = new System.Collections.ObjectModel.ObservableCollection<Teleyuma.Compra>();

                //Regrasar a pagina HomePage
                var pages = Navigation.NavigationStack.ToList();
                foreach (var page in pages)
                {
                    if (page.GetType() != typeof(Pages.HomeTabbedPage))
                        Navigation.RemovePage(page);
                }

                _Global.VM.VMResumenRecarga.ActualizarResumen();
                await this.Navigation.PushAsync(new ResumenRecarga());

                _Global.ListaRecargas = new ListaRecarga();
                MostrarCargando(false);

            }



        }

        //public async void RecargarNauta(MakeAccountTransactionResponse TransactionResponse)
        //{

        //    var recarga = await _Global.RecargaNauta.Recargar();
        //    if (recarga.erroe_code != "0")
        //    {
        //        MostrarCargando(false);
        //        await DisplayAlert("TeleYuma", recarga.error_message, "OK");
        //        return;
        //    }
        //    else
        //    {
        //        if (recarga.status != "0")
        //        {
        //            MostrarCargando(false);
        //            await DisplayAlert("TeleYuma", recarga.status_message, "OK");
        //            return;
        //        }
        //        else
        //        {
        //            var transaccion = await _Global.CurrentAccount.MakeTransaction_CapturePayment(_Global.RecargaNauta.TotalPagar, TransactionResponse);
        //            if (_Global.TransactionResponse.result_code == "1" && _Global.TransactionResponse.transaction_id != "0")
        //            {
        //                MostrarCargando(false);

        //                await _Global.CurrentAccount.MakeTransaction_Manualcharge(_Global.RecargaNauta.TotalPagar, "Recarga Nauta a" + recarga.account_number);
        //                await DisplayAlert("TeleYuma", "La recarga ha sido realizada", "OK");

        //                //Regrasar a pagina HomePage
        //                var pages = Navigation.NavigationStack.ToList();
        //                foreach (var page in pages)
        //                {
        //                    if (page.GetType() != typeof(Pages.HomeTabbedPage))
        //                        Navigation.RemovePage(page);
        //                }
        //                //------------------------
        //            }
        //            else
        //            {
        //                MostrarCargando(false);
        //                await DisplayAlert("TeleYuma", "Recarga Realizada con Error en la transferencia bancaria", "OK");
        //            }

        //        }
        //    }

        //}

        public async Task<bool> RecargarMovil(MakeAccountTransactionResponse TransactionResponse)
        {
            decimal montoRecargaSinError = 0;
            if (_Global.ListaRecargas.Lista.Any())
            {
                // recargar
                foreach (var recarga in _Global.ListaRecargas.Lista)
                {
                    if (_Global.ModoPrueba)
                        await recarga.Simular();
                    else
                        await recarga.Recargar();
                }

                //monto de las recargas echas sin error               
                foreach (var recarga in _Global.ListaRecargas.Lista)
                {
                    if (recarga.topupResponse.error_code == "0")
                    {
                        await _Global.CurrentAccount.MakeTransaction_Manualcharge(Convert.ToDecimal(recarga.precio), "Recarga a " + recarga.numero);
                        montoRecargaSinError += recarga.TotalPagar;
                    }

                }

            }

            if (montoRecargaSinError > 0)
            {
                await DisplayAlert("TeleYuma", "El sistema completó la solicitud", "OK");

            }
            else
            {

                await DisplayAlert("TeleYuma", "El sistema completó la solicitud con errores", "OK");
            }
            ActivityIndicator.IsVisible = false;
            LabelAccion.IsVisible = false;
            LayoutListaRecargas.IsVisible = false;
            ActualizarListViewRecargas();

            return true;
        }

        public async void ReservarRecargas(MakeAccountTransactionResponse TransactionResponse)
        {

            var transaccion = await _Global.CurrentAccount.MakeTransaction_CapturePayment(Convert.ToDecimal(_Global.ListaRecargas.TotalPagar), TransactionResponse);
            if (_Global.TransactionResponse.result_code == "1" && _Global.TransactionResponse.transaction_id != "0")
            {

                if (_Global.ListaRecargas.Lista.Any())
                {
                    // guardar recargas
                    foreach (var recarga in _Global.ListaRecargas.Lista)
                    {
                        var resul = await recarga.Reservar();
                        if (resul)
                        {
                            await _Global.CurrentAccount.MakeTransaction_Manualcharge(0, "Reserva Recarga Movil para" + recarga.numero);
                        }
                    }


                }

                await _Global.CurrentAccount.MakeTransaction_Manualcharge(Convert.ToDecimal(_Global.ListaRecargas.TotalPagar), "Monto Reservas Recargas Movil");


                // guardar recargas             
                await DisplayAlert("TeleYuma", "Las recargas han sido reservadas con éxito", "OK");

                _Global.ListaRecargas = new ListaRecarga();
                _Global.Vistas.ListaRecargas.ActualizarListView();
                MostrarCargando(false);

                LayoutListaRecargas.IsVisible = false;
                //Regrasar a pagina HomePage
                var pages = Navigation.NavigationStack.ToList();
                foreach (var page in pages)
                {
                    if (page.GetType() != typeof(Pages.HomeTabbedPage))
                        Navigation.RemovePage(page);
                }
                //------------------------
            }
            else
            {
                // hacer validacion
                await DisplayAlert("TeleYuma", "No se pudo reservar las recargas, hubo problemas con el pago", "OK");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            _Global.ListaRecargas = new ListaRecarga();
            _Global.Vistas.ListaRecargas.ActualizarListView();
            MostrarCargando(false);

            LayoutListaRecargas.IsVisible = false;
            //Regrasar a pagina HomePage
            var pages = Navigation.NavigationStack.ToList();
            foreach (var page in pages)
            {
                if (page.GetType() != typeof(Pages.HomeTabbedPage))
                    Navigation.RemovePage(page);
            }
            //------------------------
        }

        public async void PayPalPayment()
        {

            var nombrePago = "";
            decimal monto = 0;

            if (_Global.TipoRecarga == "movil")
            {
                nombrePago = "Recarga Móvil";
                monto = Convert.ToDecimal(_Global.ListaRecargas.TotalPagar);
            }
            else if (_Global.TipoRecarga == "nauta")
            {
                nombrePago = "Recarga Nauta";
                monto = Convert.ToDecimal(_Global.RecargaNauta.TotalPagar);
            }

            var result = await CrossPayPalManager.Current.Buy(new PayPalItem(nombrePago, monto, "USD"), new Decimal(0));
            if (result.Status == PayPalStatus.Cancelled)
            {
                await DisplayAlert("TeleYuma", "Pago cancelado", "OK");
            }
            else if (result.Status == PayPalStatus.Error)
            {
                await DisplayAlert("TeleYuma", result.ErrorMessage, "OK");
            }
            else if (result.Status == PayPalStatus.Successful)
            {
                await DisplayAlert("TeleYuma", "El pago ha sido realizado", "OK");
            }
        }
    }
}
