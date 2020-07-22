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
    public partial class Historial : ContentPage
    {
        public Historial()
        {
            InitializeComponent();
            try
            {
                pkrDesde.Date = DateTime.Now.AddMonths(-2);
                pkrHasta.Date = DateTime.Now.AddDays(1);
                CargarXDR();
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public async void CargarXDR()
        {
            //var desde = _Global.GetDateFormat_YYMMDD(pkrDesde.Date);
            //var hasta = _Global.GetDateFormat_YYMMDD(pkrHasta.Date, "final");
            //var GetAccountXDRListResponse = await _Global.CurrentAccount.GetAccountXDR(new GetAccountXDRListRequest { from_date = desde, to_date = hasta });
            ////var GetAccountXDRListResponse = await _Global.CurrentAccount.GetAccountXDR(new GetAccountXDRListRequest { from_date = _Global.GetDateFormat_YYMMDD(DateTime.Now), to_date = _Global.GetDateFormat_YYMMDD(DateTime.Now, "final") });
            //listGistorial.ItemsSource = null;
            //listGistorial.ItemsSource = GetAccountXDRListResponse.xdr_list;
            //if (GetAccountXDRListResponse.xdr_list.Length == 0)
            //    DisplayAlert("TeleYuma", "No se encontraron registros en el periodo seleccionado", "ok");
            ActivityIndicatorLoading.IsVisible = true;
            try
            {
                var desde = _Global.GetDateFormat_YYMMDD(pkrDesde.Date);
                var hasta = _Global.GetDateFormat_YYMMDD(pkrHasta.Date, "final");

                var GetAccountXDRListResponse = await _Global.CurrentAccount.GetAccountXDR(new GetAccountXDRListAllRequest {  from_date = desde, to_date = hasta });

                listGistorial.ItemsSource = null;
                listGistorial.ItemsSource = GetAccountXDRListResponse.xdr_list;
                if (GetAccountXDRListResponse.xdr_list is null || GetAccountXDRListResponse.xdr_list.Length == 0)
                {
                    StackLayoutSinResultados.IsVisible = true;
                }
                else
                {
                    StackLayoutSinResultados.IsVisible = false;
                }
                   
            }
            catch (Exception ex)
            {
                ;
            }
            ActivityIndicatorLoading.IsVisible = false;
        }

        private async void imgBuscar_Tapped(object sender, EventArgs e)
        {
            CargarXDR();
        }

    }
}
