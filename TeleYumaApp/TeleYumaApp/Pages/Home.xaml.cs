using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TeleYumaApp.ViewModels;

using static TeleYumaApp.App;

namespace TeleYumaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
          
        }


        public async void CargarCuentaBySQLiteUser(string user)
        {
            await _Global.CurrentAccount.getAccountByLogin(user);
        }

        //public async Task DoSomethingAsync()
        //{
        //    _Global.CurrentAccount.phone1 = "5355043317";
        //    _Global.CurrentAccount.i_account = 20616781;
        //    _Global.CurrentAccount.balance = new decimal(25);
        //    //try
        //    //{

        //    //    if (_Global.RunTask)
        //    //    {
        //    //        if (_Global.CurrentAccount.firstname == null)
        //    //        {
        //    //            LayoutInfoAccount.IsVisible = false;
        //    //            LayoutMenu.IsVisible = false;
        //    //            LayoutCargandoParametros.IsVisible = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            LayoutInfoAccount.IsVisible = true;
        //    //            LayoutMenu.IsVisible = true;
        //    //            LayoutCargandoParametros.IsVisible = false;
        //    //        }
        //    //        var result = await ActualizarInformacionCuenta();

        //    //        if (_Global.CurrentAccount.firstname == null)
        //    //        {
        //    //            LayoutInfoAccount.IsVisible = false;
        //    //            LayoutMenu.IsVisible = false;
        //    //            LayoutCargandoParametros.IsVisible = true;
        //    //        }
        //    //        else
        //    //        {
        //    //            LayoutInfoAccount.IsVisible = true;
        //    //            LayoutMenu.IsVisible = true;
        //    //            LayoutCargandoParametros.IsVisible = false;
        //    //        }
        //    //        if (!result)
        //    //        {
        //    //            await DisplayAlert("TeleYuma", "No se ha podido cargar los parámetros de configuración, verifique su conexión ó contacte a soporte", "ok");

        //    //            Application.Current.MainPage = new PagesInicio.Login();

        //    //            return;
        //    //        }
        //    //        else

        //    //        if (!await _Global.RecMovilConfig.GetPorcientoRecargaMovil())
        //    //        {
        //    //            await DisplayAlert("TeleYuma", "No se ha podido cargar los parámetros de configuración, verifique su conexión ó contacte a soporte", "ok");

        //    //            Application.Current.MainPage = new PagesInicio.Login();

        //    //            return;
        //    //        }
        //    //        else
        //    //        if (_Global.CurrentAccount.blocked == "Y")
        //    //        {
        //    //            await DisplayAlert("TeleYuma", "Su cuenta ha sido bloqueada contacte a soporte", "ok");
        //    //            Application.Current.MainPage = new PagesInicio.Login();
        //    //        }
        //    //    }

        //    //}
        //    //catch { }
        //    await Task.Delay(3000); // 3 second delay       
        //    DoSomethingAsync();
        //}

        //public async Task<bool> ActualizarInformacionCuenta()
        //{

        //    //_Global.CurrentAccount = await _Global.CurrentAccount.GetAccountInfo();
        //    //if (_Global.CurrentAccount.id == null)
        //    //{
        //    //    return false;
        //    //}
        //    //else
        //    //{
        //    //    _Global.VM.VMHome.ActualizarDatos();
        //    //    return true;
        //    //}


        //    return true;

        //}



        private void btnEditCuenta_Clicked(object sender, EventArgs e)
        {

            try
            {
                var cuenta = new PagesInicio.CrearCuenta();
                cuenta.CargarDatos();
                cuenta.Transaction = TipoTransaction.Edit;
                this.Navigation.PushAsync(cuenta);
            }
            catch 
            {

              
            }


        }

        private async void BtnSalir_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var valor = await DisplayAlert("TeleYuma", "Esta seguro que desea cerrar su sesión", "SI", "NO");
                if (valor)
                {

                    _Global.SQLiteLogin.Salir();
                    _Global.RunTask = false;
                    Application.Current.MainPage = new NavigationPage(new PagesInicio.Login());

                }
            }
            catch 
            {

               
            }

        }

        private async void BtnRecargaMovil_Tapped(object sender, EventArgs e)
        {
            try
            {
                var promo = await GetPromocion();
                if (promo.Estado == 1)
                    await this.Navigation.PushAsync(new Pages.TipoRecarga());
                else
                {
                    _Global.AccionRecarga = "RecargarAhora";

                    await this.Navigation.PushAsync(_Global.Vistas.ListaRecargas);


                }
            }
            catch 
            {

                
            }
        }

        private void BtnLlamar_Tapped(object sender, EventArgs e)
        {

            try
            {
                this.Navigation.PushAsync(_Global.Vistas.Llamar);
            }
            catch 
            {

               
            }


        }

        private void BtnTransferirSaldo_Tapped(object sender, EventArgs e)
        {

            try
            {
                this.Navigation.PushAsync(_Global.Vistas.TransferenciaBancaria);
            }
            catch 
            {

              
            }
            
        }

        public async Task cargarGruposSms()
        {
            try
            {
               
                await this.Navigation.PushAsync(_Global.Vistas.Grupos);
            }
            catch
            {


            }
        }


        private async void Btnsend_sms_Tapped(object sender, EventArgs e)
        {
            cargarGruposSms();
        }

        private void btnContastos_Clicked(object sender, EventArgs e)
        {

            try
            {
                _Global.Vistas.ListaContactos.Transaction = TipoTransaction.New;
                _Global.Vistas.ListaContactos.LlenarLista();
                this.Navigation.PushAsync(_Global.Vistas.ListaContactos);
            }
            catch 
            {

              
            }


        }

        private void BtnRecargaNauta_Tapped(object sender, EventArgs e)
        {

            try
            {
                this.Navigation.PushAsync(new Pages.RecargaNauta());
            }
            catch
            {

               
            }


        }

        private void BtnHistorilal_Tapped(object sender, EventArgs e)
        {

            try
            {
                this.Navigation.PushAsync(new Pages.Historial());
            }
            catch 
            {

              
            }

        }



        public async Task<EPromocion> GetPromocion()
        {
            using (HttpClient client = new HttpClient())
            {
                var URL = _Global.MasterURL + "Promocion/ConsultarActiva";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.Promocion = JsonConvert.DeserializeObject<EPromocion>(Result);
                    return _Global.Promocion;
                }

                catch
                {
                    return _Global.Promocion = new EPromocion();
                }

            }

        }
    }
}
