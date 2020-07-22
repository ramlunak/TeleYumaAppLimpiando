using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Telefono : ContentPage
    {
        public string Codigo_verificacion { get; set; }

        public string Prefijo { get; set; }

        [DefaultValue(TipoTransaction.New)]
        public TipoTransaction Transaction { get; set; }

        public Telefono()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void btnCompletar_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnSiguiente_Clicked(object sender, EventArgs e)
        {
            Confirmar();
        }

        public async void Confirmar()
        {
            if (txtTelefono.Text == "" || txtTelefono.Text == null || txtPais.Text == "" || txtPais.Text == null)
            {
                await DisplayAlert("TeleYuma", "Ingrese el País y télefono", "ok");
                return;
            }

            CargarDatosAccount();

            MostrarCargando(true);
            if (await ValidarEnTelinta())
            {
                var confirmar = new PagesInicio.ConfirmarTelefono();

                var repuesta = await confirmar.SendSms();

                if (repuesta.ErrorCode is null || repuesta.ErrorCode == "0")
                {
                    MostrarCargando(false);
                    await this.Navigation.PushAsync(confirmar);
                }
                else
                {
                    MostrarCargando(false);
                    await DisplayAlert("TeleYuma", repuesta.ErrorMessage, "ok");
                }

            }
            else
            {
                MostrarCargando(false);
                await DisplayAlert("TeleYuma", "El número de télefono ya está registrado", "ok");
            }

            MostrarCargando(false);
        }

        private void txtPais_Focused(object sender, FocusEventArgs e)
        {
             Navigation.PushAsync(_Global.Vistas.PageNewListaPaises);
            _Global.Vistas.PageNewListaPaises.SearchBarFocus(ref txtPais);
        }

        public void MostrarCargando(bool value)
        {
            cargando.IsVisible = value;
            formulario.IsVisible = !value;
        }

        public void CargarDatosAccount()
        {
            _Global.CurrentAccount = new account_info();

            Prefijo = _Global.PaisSeleccionado.PrefijoTelefonico;
            var telefono = (Prefijo + txtTelefono.Text).Trim();
            var now = DateTime.Now;
            var YY = now;
            var MM = now.Month.ToString();
            var DD = now.Day.ToString();
            if (MM.Length == 1)
                MM = "0" + MM;
            if (DD.Length == 1)
                DD = "0" + DD;
            var activationDate = now.Year + "-" + MM + "-" + DD;
            _Global.CurrentAccount.id = "a" + telefono.Trim();
            _Global.CurrentAccount.iso_4217 = "USD";
            _Global.CurrentAccount.i_customer = 260271;  //Online customers
            _Global.CurrentAccount.i_distributor = 282645;  //distributor  customers
            _Global.CurrentAccount.billing_model = -1;
            _Global.CurrentAccount.control_number = 1;
            _Global.CurrentAccount.i_product = 22791;
            _Global.CurrentAccount.batch_name = "260271-di-pinless";
            _Global.CurrentAccount.country = _Global.PaisSeleccionado.Nombre.Trim();
            _Global.CurrentAccount.activation_date = activationDate.Trim();
            _Global.CurrentAccount.phone1 = telefono.Trim();
        }

        public async Task<bool> ValidarEnTelinta()
        {
            
            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = _Global.CurrentAccount });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.validate_account_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        var id = JsonConvert.DeserializeObject<AccountObject>(json).account_info.id;
                        if (id is null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                catch 
                {
                    await DisplayAlert("TeleYuma", "Error al conectarse con el servidor, compruebe su conexión a internet.", "ok");
                    return false;
                }

            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }

}
