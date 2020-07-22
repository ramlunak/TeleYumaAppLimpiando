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

namespace TeleYumaApp.Cuenta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TarjetaCredito : ContentPage
    {
        private const string V = "/";

        public TarjetaCredito()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMTarjetaCredito;
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            UpdatePaymentMethod();
        }


        public async void CargarDatos()
        {

            var PaymentMethodInfo = await _Global.CurrentAccount.PaymentMethodInfo();
            if (PaymentMethodInfo.payment_method_info != null)
            {
                var payment_method_info = PaymentMethodInfo.payment_method_info;
                BindingContext = null;
                BindingContext = payment_method_info;

                txt_number.Placeholder = payment_method_info.number;
                txt_cvv.Placeholder = "xxx";
                //Cargar fecha expedicion
                var fecha = payment_method_info.exp_date.Split('-');
                var year = fecha[0];
                var mes = Convert.ToInt32(fecha[1]);
                pkr_exp_date_year.SelectedIndex = GetPikerIndex(pkr_exp_date_year, year);
                pkr_exp_date_mes.SelectedIndex = GetPikerIndex(pkr_exp_date_mes, mes.ToString());
                //Cargar tipo tarjeta
                if (payment_method_info.payment_method == "MasterCard") pkr_payment_method.SelectedIndex = 0;
                if (payment_method_info.payment_method == "American Express") pkr_payment_method.SelectedIndex = 1;
                if (payment_method_info.payment_method == "Discover") pkr_payment_method.SelectedIndex = 2;
                if (payment_method_info.payment_method == "VISA") pkr_payment_method.SelectedIndex = 3;
                //Cargar Country and subcountry
                _Global.VM.VMTarjetaCredito.CargarPaises(payment_method_info.iso_3166_1_a2);
                _Global.VM.VMTarjetaCredito.CargarSubCountris(payment_method_info.i_country_subdivision);
            }
            else
            {

            }
        }

        public int GetPikerIndex(Picker piker, string value)
        {
            var listaItem = piker.Items;
            var index = 0;
            foreach (var item in listaItem)
            {
                if (item == value)
                    return index;
                index++;
            }
            return -1;
        }

        public async void UpdatePaymentMethod()
        {
            var number = pkr_payment_method.SelectedIndex;
            var y = pkr_payment_method.SelectedIndex;
            var m = pkr_payment_method.SelectedIndex;

            if (number == -1 || y == -1 || m == -1 || txt_name.Text == "" || txt_addres.Text == "" || txt_zip.Text == "" || txt_number.Text == "" || txt_cvv.Text == "" || txt_number.Text == null || txt_cvv.Text == null || _Global.VM.VMTarjetaCredito.CountrySelectedItem == null || _Global.VM.VMTarjetaCredito.CountrySelectedItem == null)
            {
                await DisplayAlert("TeleYuma", "Complete la información de la tarjeta", "OK");
                return;
            }

            var PaymentInfo = new payment_method_info();

            PaymentInfo.payment_method = pkr_payment_method.Items[pkr_payment_method.SelectedIndex].ToString();
            PaymentInfo.name = txt_name.Text;
            PaymentInfo.iso_3166_1_a2 = _Global.VM.VMTarjetaCredito.CountrySelectedItem.iso_3166_1_a2;
            PaymentInfo.i_country_subdivision = _Global.VM.VMTarjetaCredito.SubCountrySelectedItem.i_country_subdivision;
            PaymentInfo.address = txt_addres.Text;
            PaymentInfo.zip = txt_zip.Text;
            PaymentInfo.number = txt_number.Text.Replace(" ", "");
            PaymentInfo.i_country_subdivision = _Global.VM.VMTarjetaCredito.SubCountrySelectedItem.i_country_subdivision;


            var year = txt_fecha.Text.Split('/')[1];
            var mes = txt_fecha.Text.Split('/')[0];
            //var mes = pkr_exp_date_mes.Items[pkr_exp_date_mes.SelectedIndex];

            //var MM = mes;              


            //if (MM.Length == 1)
            //    MM = "0" + MM;      

            var exp_date = "20" + year + "-" + mes + "-28";
            PaymentInfo.exp_date = exp_date;
            PaymentInfo.cvv = txt_cvv.Text;

            var UpdateAccountPaymentMethodRequest = new UpdateAccountPaymentMethodRequest
            {
                i_account = _Global.CurrentAccount.i_account,
                payment_method_info = PaymentInfo
            };

            //var URL = "";
            //var param = JsonConvert.SerializeObject(UpdateAccountPaymentMethodRequest);
            //URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.update_payment_method + "/" + _Global.AuthInfoAdminJson + "/" + param;

            //var result = await _Global.Get<object>(URL);
            //;
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var URL = "";
                    var param = JsonConvert.SerializeObject(UpdateAccountPaymentMethodRequest);
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + V + _Global.Metodo.update_payment_method + V +await _Global.GetAuthInfoAdminJson() + V + param;
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();

                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    var i_credit_card = JsonConvert.DeserializeObject<UpdateAccountPaymentMethodResponse>(json).i_credit_card;
                    if (i_credit_card == 0)
                        await DisplayAlert("Update Payment Method Error", ErrorHandling.faultstring, "OK");
                    else
                    {
                        var cantidad = UpdateAccountPaymentMethodRequest.payment_method_info.number.Length;                       
                        var ultimosnumeros = UpdateAccountPaymentMethodRequest.payment_method_info.number.Substring(cantidad - 4,4);
                        var result = await DisplayAlert("Una cosa más", "Debemos verificar su tarjeta que termina en (" + ultimosnumeros + ") para asegurarnos que usted es el propetario de esta tarjeta", "ENTIENDO", "CANCELAR");
                                               
                        if (result) {
                            _Global.Vistas.Pagar.ActualizarLbabelNumero();
                            _Global.CurrentAccount.phone2 = "";
                            _Global.CurrentAccount.cont2 = "0";
                            _Global.CaptureMonto = 0;

                            await _Global.CurrentAccount.New_Actualizar();
                            await this.Navigation.PushAsync(new Cuenta.VerificarTarjeta());
                        }
                        else
                        {
                            return;
                        }
                        
                    }
                }
                catch
                {
                    await DisplayAlert("TeleYuma", "Error de conección al servidor", "OK");

                }

            }

        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}
