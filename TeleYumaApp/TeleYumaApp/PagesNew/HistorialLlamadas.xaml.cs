using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.PagesNew
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistorialLlamadas : ContentPage
    {
        public HistorialLlamadas()
        {
            InitializeComponent();
            RecargarLista();
        }

        public async Task RecargarLista()
        {
            CargarXDR();
            await Task.Delay(5);
            RecargarLista();
        }

        public async void CargarXDR()
        {
            var GetAccountXDRListResponse = await _Global.CurrentAccount.GetAccountXDR(new GetAccountXDRListRequest { i_service = 3, from_date = _Global.GetDateFormat_YYMMDD(DateTime.Now.AddMonths(-2)), to_date = _Global.GetDateFormat_YYMMDD(DateTime.Now, "final") });
            listGistorial.ItemsSource = null;

            //Use linq to sorty our monkeys by name and then group them by the new name sort property
            try
            {
                var xdr_list = GetAccountXDRListResponse.xdr_list.ToList();
                var sorted = from xdr in xdr_list
                             orderby xdr.i_xdr descending
                             group xdr by xdr.data into xdrGroup
                             select new Grouping<string, XDRInfo>(xdrGroup.Key, xdrGroup);

                listGistorial.ItemsSource = new ObservableCollection<Grouping<string, XDRInfo>>(sorted);
            }
            catch (Exception ex)
            {
                ;
            }
        }


        private async void imgBuscar_Tapped(object sender, EventArgs e)
        {
            var desde = _Global.GetDateFormat_YYMMDD(pkrDesde.Date);
            var hasta = _Global.GetDateFormat_YYMMDD(pkrHasta.Date, "final");

            var GetAccountXDRListResponse = await _Global.CurrentAccount.GetAccountXDR(new GetAccountXDRListRequest { from_date = desde, to_date = hasta });

            listGistorial.ItemsSource = null;
            listGistorial.ItemsSource = GetAccountXDRListResponse.xdr_list;

        }

        private void listGistorial_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var evento = e as TappedEventArgs;
                var llamada = "7868717144,011" + evento.Parameter;
                DependencyService.Get<ICallService>().Call(llamada);
            }
            catch
            {

            }
        }
    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }

}