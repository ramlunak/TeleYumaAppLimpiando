using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace TeleYumaApp.SMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mensaje : ContentPage
    {

        public Mensaje()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMMensaje = new ViewModels.VMMensaje();
            _Global.VM.VMMensaje.ReferenciarTxtSearch(ref txtSearch);
            _Global.VM.VMMensaje.ReferenciarListView(ref listSMS);

        }      

        private void listSMS_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {


          //  var sd = listSMS.ItemsSource;
            //if (e.Item == ))
            //{
            //    listSMS.ScrollTo(e.Item, ScrollToPosition.MakeVisible, false);
            //}
            ;
        }

        protected override bool OnBackButtonPressed()
        {
            ;
            //await Navigation.PopAsync(true);
            base.OnBackButtonPressed();
            return true;

        }

        public void ListViewAutoHeight()
        {
            var ContentPageHeight = this.HeightRequest;
            var ListeViewItems = listSMS.ItemTemplate;
            listSMS.HeightRequest = _Global.ListaSMS.Sum(x => x.ItemHeight);
        }

        //public void CargarSMS()
        //{
        //    this.Title = _Global.GrupoSMS.contacto;
        //    _Global.VM.VMMensaje.ActualizarLista();

        //    var id = 0;
        //    Esms last = new Esms();
        //    foreach (var item in _Global.ListaSMS)
        //    {
        //        if (item.Idsms > id)
        //        {
        //            last = item;
        //            id = item.Idsms;
        //        }
        //    }
        //    if (null != last)
        //    {
        //        this.listSMS.ScrollTo(last, ScrollToPosition.MakeVisible, true);
        //    }

        //}

        private void ExpandableEditor_TextChange(object sender, TextChangedEventArgs e)
        {
            var Length = ((Editor)sender).Text.Length;
            int cantSMS = Length / 160 + 1;
            int restantes = (160 * cantSMS) - Length;

            LabelCount.Text = restantes + "/" + cantSMS;

            _Global.VM.VMMensaje.monto = decimal.Round(Convert.ToDecimal(cantSMS * 0.05), 2);
            LabelCosto.Text = "$ " + _Global.VM.VMMensaje.monto;

            var H = ((Editor)sender).Height;
            var Lineas = Convert.ToInt32(((H - 10) / 20));
            _Global.VM.VMMensaje.ItemHeight = 60 + (Lineas * 20);

            //if (((Editor)sender).Height > 138)
            //    ((Editor)sender).HeightRequest = 138;

        }

        //Para poner sms de la pantalla se nuevo sms
        public void PonerSMS(string sms)
        {
            txtSms.Text = sms;

        }

        public void LimpiarControles()
        {
            txtSms.Text = string.Empty;
            LabelCount.Text = "160/1";
            LabelCosto.Text = "$ 0.08";
        }

        private void listSMS_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
                     
            //try
            //{
            //    if (e.SelectedItem != null)
            //    {
            //        ;
            //        SmsSeleccionado = e.SelectedItem as Esms;
            //        btnEliminar.Text = "Eliminar";
            //    }
            //}
            //catch { }
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            //if (SmsSeleccionado == null)
            //{
            //    await DisplayAlert("TeleYuma", "Seleccione el mensaje que desea eliminar", "OK");
            //}

            //try
            //{
            //    var result = await SmsSeleccionado.Eliminar();
            //    if (result)
            //    {

            //        if (_Global.ListaSMS.Count == 1)
            //            await this.Navigation.PopAsync();
            //        try
            //        {
            //            _Global.ListaSMS = _Global.GruposDeListasSMS.First(x => x.numero == SmsSeleccionado.NumeroTelefono).ListaSMS;
            //        }
            //        catch (Exception ex)
            //        {
            //            ;
            //        }
            //        CargarSMS();
            //        listSMS.SelectedItem = null;


            //    }

            //}
            //catch (Exception)
            //{
            //    ;
            //}
        }

        private void listSMS_LongClicked(object sender, EventArgs e)
        {
            _Global.VM.VMMensaje.LongPressSelected = (Esms)((ItemTappedEventArgs)e).Item;
            _Global.VM.VMMensaje.popupOpcionesVisible = true;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _Global.VM.VMMensaje.popupOpcionesVisible = false;
        }


    }

}
