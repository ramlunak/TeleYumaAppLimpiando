using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.SMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Grupos : ContentPage
    {
        public Grupos()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMGrupos;
           _Global.VM.VMGrupos.ReferenciarTxtSearch(ref txtSearch);


        }
        
        
        private void listGrupos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
              var grupo  = e.SelectedItem as GrupoSMS;
                if (grupo != null)
                {
                    _Global.GrupoSMS = grupo;
                    this.Navigation.PushAsync(_Global.Vistas.EnviarSMS);
                    _Global.VM.VMMensaje.ActualizarLista();
                    ClearNews();
                }
                listGrupos.SelectedItem = null;
            }
            catch 
            {
                ;
            }
            _Global.VM.VMMensaje.Mensaje = string.Empty;
        }
        
        public async Task ClearNews()
        {
            var news = (from sms in _Global.GrupoSMS.ListaSMS where sms.IsNew select sms).ToList();
            foreach (var item in news)
            {
                item.IsNew = false;
                item.Update();
            }

        }

        private void IMG_NEW_SMS_Tapped(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new SMS.NewSMS());
        }
        
        private void listGrupos_LongClicked(object sender, EventArgs e)
        {
          _Global.VM.VMGrupos.LongPressSelected = (GrupoSMS)((ItemTappedEventArgs)e).Item;
           _Global.VM.VMGrupos.popupOpcionesVisible = true;         
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _Global.VM.VMGrupos.popupOpcionesVisible = false;
        }
    }
}
