using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;

namespace TeleYumaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaRecargas : ContentPage
    {
        public ListaRecargas()
        {
            InitializeComponent();
            BindingContext = _Global.ListaRecargas;
            ListView.ItemsSource = _Global.ListaRecargas.Lista;
        }

        public void ActualizarListView()
        {
            ListView.ItemsSource = null;
            ListView.ItemsSource = _Global.ListaRecargas.Lista;
        }

        private async void btnAddRecarga_Clicked(object sender, EventArgs e)
        {

            //if (_Global.ListaRecargas.Lista.Count == 5)
            //{
            //    await DisplayAlert("TeleYuma", "Solo se admiten 5 recargas en la lista", "OK");
            //    return;
            //}
            //if (pkrMonto.SelectedIndex == -1 || txtTelefono.Text == "" || txtTelefono.Text == null)
            //{
            //    await DisplayAlert("TeleYuma", "Complete los datos", "OK");
            //    return;
            //}
            //try
            //{
            //    var recarga = new Recarga();
            //    recarga.numero = _Global.PaisSeleccionado.PrefijoTelefonico + txtTelefono.Text;
            //    recarga.monto = Convert.ToInt32(pkrMonto.Items[pkrMonto.SelectedIndex]);


            //    // Verificar si el monto de la lista existe en transferto
            //    //if (_Global.ListaRecargas.Lista.Count > 0)
            //    //{
            //    //    var recargaParaSimularDinero = new Recarga
            //    //    {
            //    //        numero = recarga.numero,
            //    //        monto = Convert.ToInt32(_Global.ListaRecargas.TotalPagar) + Convert.ToInt32(recarga.TotalPagar)
            //    //    };
            //    //    var sumulacionDinero = await recargaParaSimularDinero.Simular();
            //    //    if (recargaParaSimularDinero.topupResponseErrorCode != "0")
            //    //    {
            //    //        await DisplayAlert("TeleYuma", "Esta recarga no se puede agregar a la lista, contacte al soporte técnico", "OK");
            //    //        return;
            //    //    }
            //    //}


            //    WaintTelefono.IsRunning = true;
            //    var sumulacion = await recarga.Simular();
            //    WaintTelefono.IsRunning = false;

            //    if (sumulacion)
            //    {
            //        if (recarga.topupResponseErrorCode == "224")
            //        {
            //            await DisplayAlert("TeleYuma", "El número te teléfono no es correcto", "OK");
            //            return;
            //        }
            //        else
            //         if (recarga.topupResponseErrorCode == "101")
            //        {
            //            await DisplayAlert("TeleYuma", "Número de destino fuera de rango", "OK");
            //            return;
            //        }
            //        else
            //        if (recarga.topupResponseErrorCode != "0")
            //        {
            //            await DisplayAlert("TeleYuma", "Esta recarga no se puede realizar, contacte al soporte técnico", "OK");
            //            return;
            //        }

            //        //Validar 10 recargas movil
            //        var Count_Movil = 0;
            //        var GetAccountXDRListResponse = await _Global.CurrentAccount.GetAccountXDR(new GetAccountXDRListRequest { from_date = _Global.GetDateFormat_YYMMDD(DateTime.Now), to_date = _Global.GetDateFormat_YYMMDD(DateTime.Now, "final") });
            //        try
            //        {
            //            foreach (var item in GetAccountXDRListResponse.xdr_list)
            //            {
            //                if (item.CLD == "Recarga Movil")
            //                {
            //                    Count_Movil++;
            //                }

            //            }
            //            if ((Count_Movil + _Global.ListaRecargas.Lista.Count) == 11)
            //            {
            //                await DisplayAlert("TeleYuma", "Para evitar recargas hechas con robo de tarjeta el sistema limita las recargas, contacte al servicio técnico", "OK");
            //                return;
            //            }
            //        }
            //        catch { }
            //        //-----------------------------------


            //        _Global.ListaRecargas.Lista.Add(recarga);
            //        ListView.ItemsSource = null;
            //        ListView.ItemsSource = _Global.ListaRecargas.Lista;

            //        pkrMonto.SelectedIndex = -1;
            //        txtPais.Text = string.Empty;
            //        txtTelefono.Text = string.Empty;
            //    }
            //    else
            //    {
            //        WaintTelefono.IsRunning = false;
            //        await DisplayAlert("TeleYuma", "No se pudo conectar al servidor para comprobar el número de teléfono", "OK");

            //    }
            //}
            //catch 
            //{
            //    WaintTelefono.IsRunning = false;
            //    await DisplayAlert("TeleYuma", "No se pudo conectar al servidor para comprobar el número de teléfono", "OK");

            //}
        }

        private void txtPais_Focused(object sender, FocusEventArgs e)
        {
            Navigation.PushAsync(_Global.Vistas.PageNewListaPaises);
            _Global.Vistas.PageNewListaPaises.SearchBarFocus(ref txtPais);
        }

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }

        private void btnSiguiente_Clicked(object sender, EventArgs e)
        {
                       

            if (!_Global.ListaRecargas.Lista.Any())
            {
                 DisplayAlert("TeleYuma", "Añada la recarga al carrito", "OK");
                return; 
            }
            _Global.TipoRecarga = "movil";
            this.Navigation.PushAsync(new Pages.SeleccionarMetodoPago());
        }
              
        private void btnDelete_Clicked(object sender, EventArgs e)
        {
            var buton = sender as Image;
            var recarga = buton.BindingContext as Recarga;
            var vm = BindingContext as ListaRecarga;
            vm.Delete.Execute(recarga);
        }

        private void btnCancelar_ChildAdded(object sender, ElementEventArgs e)
        {

        }

        private void txtPais_TextChanged(object sender, TextChangedEventArgs e)
        {
            if( e.NewTextValue != "" && e.NewTextValue != null)
            {
                if(e.NewTextValue == "(+53) Cuba")
                ValidarNumeroCuba();              
            }

        }

        public void ValidarNumeroCuba(){
           
            if(txtTelefono.Text != "" && txtTelefono.Text != null){
                var numero = txtTelefono.Text;
                int tam_var = numero.Length;
                if (tam_var > 8)
                {
                    String Var_Sub = numero.Substring((tam_var - 8), 8);
                    txtTelefono.Text = Var_Sub;
                }
              }
        }

        private void imgClearPais_cliked(object sender, EventArgs e)
        {
            txtPais.Text = string.Empty;
        }

        private void imgContactos_cliked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(_Global.Vistas.ListaContactos);
            _Global.Vistas.ListaContactos.Transaction = TipoTransaction.Select;
            _Global.Vistas.ListaContactos.HideButtonAdd(ref txtPais, ref txtTelefono);
            _Global.Vistas.ListaContactos.LlenarLista();
        }


    }
}
