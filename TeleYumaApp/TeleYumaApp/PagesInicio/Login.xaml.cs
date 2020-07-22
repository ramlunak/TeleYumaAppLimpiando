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
    public partial class Login : ContentPage
    {
        public ImageSource SourceChecked = ImageSource.FromFile("check");
        public ImageSource SourceUnChecked = ImageSource.FromFile("uncheck");

        public Login()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }

        }

        public async Task MostrarAlerta()
        {
            await DisplayAlert("TeleYuma", "No se ha podido cargar los parámetros de configuración, verifique su conexión ó contacte a soporte", "ok");
        }

        public void MostrarCargando(bool value)
        {
            Loading.IsVisible = value;
        }

        decimal RandomMonto()
        {
            var Ran = new Random();
            int numeroAleatorio = Ran.Next(10, 99);
            decimal monto;
            monto = Convert.ToDecimal("0," + numeroAleatorio.ToString());
            if (monto >= 1)
            {
                monto = Convert.ToDecimal("0." + numeroAleatorio.ToString());
            }
            return monto;
        }

        //public async void Loged(string user, string clave)
        //{
        //   // _Global.CurrentAccount.cont2 = RandomMonto().ToString();
        //   // await _Global.CurrentAccount.New_Actualizar();
        //   // Application.Current.MainPage = new Cuenta.VerificarTarjeta(); return;

        //    MostrarCargando(true);
        //    var account1 = new account_info();
        //    account1.i_account = 20694194;
        //    account1.firstname = "Royber";
        //    account1.lastname = "Arias Moreno";
        //    account1.email = "royber@gmail.com";
        //    account1.balance = new decimal(15);
        //    account1.password = "royber2018*";
        //    account1.country = "UNITED STATES OF AMERICA";
        //    account1.phone1 = "15355043317";
        //    account1.id = "a15355043317";
        //    _Global.CurrentAccount = account1;
        //    _Global.RunTask = true;           
        //    await Task.Delay(1000);
        //    MostrarCargando(false);
        //    _Global.Vistas.PageHome.CargarDatosCuenta();
        //    Application.Current.MainPage = new Pages.MasterDetail();
        //}

        public async void Loged(string user, string clave)
        {
            try
            {

                if (user == "" || user == null || clave == "" || clave == null)
                {
                    await DisplayAlert("TeleYuma", "Ingrese usuario y contraseña", "ok");
                    return;
                }

                MostrarCargando(true);
                var account = await GetAccountrByLogin(user, clave);
                MostrarCargando(false);
                if (_Global.CurrentAccount.id is null)
                {
                    return;
                }
                    if (_Global.CurrentAccount.blocked is null)
                {
                    await DisplayAlert("TeleYuma", "Error al conectarse con el servidor, compruebe su conexión a internet.", "ok");
                    return;
                }
                if (_Global.CurrentAccount.blocked == "Y")
                {
                    await DisplayAlert("TeleYuma", "Su cuenta ha sido bloqueada", "ok");
                    return;
                }
                if (_Global.CurrentAccount.bill_status != "O")
                {
                    await DisplayAlert("TeleYuma", "Su cuenta ha sido bloqueada", "ok");
                    return;
                }

                if (account)
                {
                    try
                    {
                        _Global.SQLiteLogin.i_account = _Global.CurrentAccount.i_account;
                        _Global.SQLiteLogin.phone1 = _Global.CurrentAccount.phone1;
                        _Global.SQLiteLogin.isloged = true;
                        _Global.SQLiteLogin.user = user;
                        _Global.SQLiteLogin.password = clave;
                        _Global.SQLiteLogin.Ingresar();
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                    var login = _Global.SQLiteLogin.GetInfoLogin();
                    _Global.RunTask = true;
                    _Global.VM.VMInicio.ActualizarDatos();
                    _Global.VM.VMInicio.ActualizarInformacionCuenta();
                    Application.Current.MainPage = new Pages.MasterDetail();
                }
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
            return;

            ///////*------------------*/


            //var account1 = new account_info();

            //account1.i_account = 20616781;
            //account1.login = "55043317";
            //account1.firstname = "Royber";
            //account1.lastname = "Arias Moreno";
            //account1.email = "royber@gmail.com";
            //account1.balance = new decimal(25);
            //account1.password = "royber2019*";
            //account1.country = "CUBA";
            //account1.phone1 = "5355043317";
            //account1.id = "a5355043317";
            //_Global.CurrentAccount = account1;
            // _Global.VM.VMInicio.ActualizarDatos();

            //try
            //{
            //    _Global.SQLiteLogin.phone1 = _Global.CurrentAccount.phone1;
            //_Global.SQLiteLogin.i_account = _Global.CurrentAccount.i_account;
            // _Global.SQLiteLogin.isloged = true;
            //    _Global.SQLiteLogin.user = account1.login;
            //    _Global.SQLiteLogin.password = account1.password;
            //    _Global.SQLiteLogin.phone1 = account1.phone1;
            //    _Global.SQLiteLogin.Ingresar();
            //}
            //catch (Exception ex)
            //{
            //    ;
            //}

            //Application.Current.MainPage = new Pages.MasterDetail();

        }

        private void bntConectar_Clicked(object sender, EventArgs e)
        {
          Loged(txtLogin.Text, txtPassword.Text);
        }

        private void bntCrearCuenta_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new PagesInicio.Telefono());
        }

        public async Task<bool> GetAccountrByLogin(string user, string clave)
        {

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { login = user });
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_account_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {

                        if (JsonConvert.DeserializeObject<AccountObject>(json).account_info != null)
                            _Global.CurrentAccount = JsonConvert.DeserializeObject<AccountObject>(json).account_info;
                        else
                        {
                            await DisplayAlert("TeleYuma", "El usuario no esta registrado", "ok");
                            return false;
                        }

                        if (_Global.CurrentAccount.password == clave)
                        {
                            return true;
                        }
                        else
                        {
                            await DisplayAlert("TeleYuma", "La contraseña es incorrecta", "ok");
                            return false;
                        }
                    }
                    else
                    {
                        await DisplayAlert("TeleYuma", ErrorHandling.faultstring, "ok");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("TeleYuma", "Error al conectarse con el servidor, compruebe su conexión a internet.", "ok");
                    return false;
                }

            }

        }


        public async void lblRecuperar_Tapped(object sender, EventArgs e)
        {
            if (txtLogin.Text == "" || txtLogin.Text == null)
            {
                await DisplayAlert("TeleYuma", "Ingrese el usuario.", "ok");
                return;
            }

            lblVerPaswword.Text = "Recuperando....";
            var Account = await _Global.CurrentAccount.getAccountByLogin(txtLogin.Text);
            if (Account)
            {
                var body = "La clave de su cuenta en TeleYuma es " + _Global.CurrentAccount.password;

                var smsConfirmacio = new Esms
                {
                    Mensaje = body,
                    NumeroTelefono = _Global.CurrentAccount.phone1,
                    RemitenteNumero = "TeleYumaApp recupera clave"
                };
                var respuesta = await smsConfirmacio.Enviar();
                if (respuesta.ErrorCode is null || respuesta.ErrorCode == "0")
                {
                    await DisplayAlert("TeleYuma", "El sistema le ha enviado un sms con su clave", "OK");
                }
                else
                {
                    await DisplayAlert("TeleYuma", "El sistema no pudo recuperar su clave, contácte con el sopórte técnico", "OK");
                }

            }
            else
                await DisplayAlert("TeleYuma", "El sistema no ha encontrado el usuario", "OK");
            lblVerPaswword.Text = " Recuperar contraseña.";

        }

        private void terminosCondiciones_Tapped(object s, EventArgs e)
        {
            Device.OpenUri(new Uri("http://teleyuma.com/terms-conditions/"));
        }

        private void politicaPrivacidad_Tapped(object s, EventArgs e)
        {
            Device.OpenUri(new Uri("https://teleyuma.com/privacy-policy/"));
        }

    }
}
