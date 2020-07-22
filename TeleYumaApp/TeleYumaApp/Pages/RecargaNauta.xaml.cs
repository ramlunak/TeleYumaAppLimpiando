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
    public partial class RecargaNauta : ContentPage
    {
        public RecargaNauta()
        {
            InitializeComponent();

        }


        private async void btnSiguiente_Clicked(object sender, EventArgs e)
        {
            

            if (pkr_monto.SelectedIndex == -1 || txt_usuario.Text == null || txt_usuario.Text == "")
            {
                await DisplayAlert("TeleYuma", "Complete los datos de la recarga", "OK");
                return;
            }


            //Validar 5 recargas nautas
            var Count_Nauta = 0;
            var GetAccountXDRListResponse = await _Global.CurrentAccount.GetAccountXDR(new GetAccountXDRListRequest { from_date = _Global.GetDateFormat_YYMMDD(DateTime.Now), to_date = _Global.GetDateFormat_YYMMDD(DateTime.Now, "final") });
            try {
            foreach (var item in GetAccountXDRListResponse.xdr_list)
            {
                if (item.CLD == "Recarga Nauta")
                {
                    Count_Nauta++;
                }

            }
            if (Count_Nauta == 5)
            {
                await DisplayAlert("TeleYuma", "Para evitar recargas hechas con robo de tarjeta el sistema limita las recargas, contacte al servicio técnico", "OK");
                return;
            }
            }
            catch {
                
            }
            //-----------------------------------


            var usuario = txt_usuario.Text + "@nauta.com.cu";

            var itemValue = pkr_monto.Items[pkr_monto.SelectedIndex];
            var product = 0;
            if (itemValue == "10") product = 175;
            if (itemValue == "12") product = 176;
            if (itemValue == "14") product = 177;
            if (itemValue == "16") product = 178;
            if (itemValue == "18") product = 179;
            if (itemValue == "20") product = 180;
            if (itemValue == "25") product = 181;
            if (itemValue == "30") product = 182;
            if (itemValue == "35") product = 183;
            if (itemValue == "40") product = 184;
            if (itemValue == "45") product = 185;
            if (itemValue == "47") product = 186;


            waintSimularNauta.IsRunning = true;
            var simulacion = await _Global.RecargaNauta.Simular(usuario, product, Convert.ToInt32(itemValue));
            waintSimularNauta.IsRunning = false;

            _Global.TipoRecarga = "nauta";

            if (simulacion.erroe_code != "0")
            {
                await DisplayAlert("TeleYuma", "A ocurrido un error, contacte al servicio técnico", "OK");
            }
            else
            {
                if (simulacion.status != "0")
                {
                    await DisplayAlert("TeleYuma", simulacion.status_message, "OK");
                }
                else
                {
                    //Recargar
                    await this.Navigation.PushAsync(new Pages.SeleccionarMetodoPago());
                }
            }
        }
                

        private void ImgContactos_cliked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(_Global.Vistas.ListaContactos);
            _Global.Vistas.ListaContactos.Transaction = TipoTransaction.Select;
            _Global.Vistas.ListaContactos.HideButtonAdd(ref txt_usuario);
            _Global.Vistas.ListaContactos.LlenarLista();
        }
    }
}
