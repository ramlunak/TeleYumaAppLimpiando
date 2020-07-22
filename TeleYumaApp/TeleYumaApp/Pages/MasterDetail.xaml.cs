
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static TeleYumaApp.App;

namespace TeleYumaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
       
    public class opcion
    {
        public string icon { get; set; }
        public string text { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetail : MasterDetailPage
    {

        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public List<opcion> opciones;
        public MasterDetail()
        {
            InitializeComponent();          
           
                Detail = new NavigationPage(new Pages.HomeTabbedPage())
                {
                    BarBackgroundColor = Color.FromHex("#1648CA"),
                    BarTextColor = Color.White
                };          
        }

        private void LvOpciones_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var opc = e.SelectedItem as opcion;

                if (opc.text == "Inicio")
                {

                    try
                    {
                        this.IsPresented = false;
                        this.Unfocus();
                        _Global.VM.VMHome.TabIndex = _Global.VM.VMHome.BarPage.Children[0];
                        _Global.VM.VMHome.BarPage.Children[0].Focus();
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("itemselct", ex.ToString(), "OK");
                    }

                }
                if (opc.text == "Chat")
                {
                    try
                    {
                        this.IsPresented = false;
                        this.Unfocus();
                        _Global.VM.VMHome.TabIndex = _Global.VM.VMHome.BarPage.Children[1];
                        _Global.VM.VMHome.BarPage.Children[1].Focus();
                    }
                    catch
                    {


                    }
                }
                if (opc.text == "Recargas")
                {
                    try
                    {
                        this.IsPresented = false;
                        this.Unfocus();
                        _Global.VM.VMHome.TabIndex = _Global.VM.VMHome.BarPage.Children[2];
                        _Global.VM.VMHome.BarPage.Children[2].Focus();
                    }
                    catch
                    {


                    }
                }
                if (opc.text == "Llamar")
                {
                    try
                    {
                        this.IsPresented = false;
                        this.Unfocus();
                        _Global.VM.VMHome.TabIndex = _Global.VM.VMHome.BarPage.Children[3];
                        _Global.VM.VMHome.BarPage.Children[3].Focus();
                    }
                    catch
                    {


                    }
                }
                if (opc.text == "Historial")
                {
                    try
                    {
                        this.IsPresented = false;
                        this.Unfocus();
                        _Global.VM.VMHome.TabIndex = _Global.VM.VMHome.BarPage.Children[4];
                        _Global.VM.VMHome.BarPage.Children[4].Focus();
                    }
                    catch
                    {


                    }
                }
                if (opc.text == "305 447 7549")
                {
                    try
                    {
                        DependencyService.Get<ICallService>().Call("+13054477549");
                    }
                    catch
                    {

                    }
                }

                if (opc.text == "Cuenta")
                {
                    try
                    {

                        var cuenta = new PagesInicio.CrearCuenta();
                        cuenta.CargarDatos();
                        cuenta.Transaction = TipoTransaction.Edit;
                        this.IsPresented = false;
                        this.Unfocus();
                        this.Navigation.PushModalAsync(cuenta);
                        cuenta.Focus();
                    }
                    catch
                    {


                    }
                }
                if (opc.text == "support@teleyuma.com")
                {
                    try
                    {
                        Device.OpenUri(new Uri("mailto:support@teleyuma.com"));
                    }
                    catch
                    {

                    }
                }
                if (opc.text == "Términos y condiciones")
                {
                    try
                    {
                        Device.OpenUri(new Uri("http://teleyuma.com/terms-and-conditions/"));
                    }
                    catch
                    {

                    }
                }

            }

             ((ListView)sender).SelectedItem = null;
            this.IsPresented = false;
            this.Unfocus();
        }

        private void lblCuenta_Tapped(object sender, EventArgs e)
        {
            //var cuenta = new PagesInicio.CrearCuenta();
            //cuenta.CargarDatos();
            //cuenta.Transaction = TipoTransaction.Edit;
            //this.Navigation.PushModalAsync(cuenta);
        }

        private void lblContactos_Tapped(object sender, EventArgs e)
        {
            //this.Navigation.PushModalAsync(_Global.Vistas.ListaContactos);
            //_Global.Vistas.ListaContactos.Transaction = TipoTransaction.New;
            //_Global.Vistas.ListaContactos.LlenarLista();
        }

        private void lblSalir_Tapped(object sender, EventArgs e)
        {

        }

        private void LlamarSoporte_Tapped(object sender, EventArgs e)
        {
            //
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {
            //DependencyService.Get<ICallService>().Call("+"+ lblNumeroSoporte.Text.Replace(" ",""));
        }

        private void TapGestureRecognizer_Tapped_6(object sender, EventArgs e)
        {

        }

        private async void TapGestureRecognizer_Tapped_7(object sender, EventArgs e)
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

        private void TapGestureRecognizer_Tapped_8(object sender, EventArgs e)
        {

        }


    }
}
