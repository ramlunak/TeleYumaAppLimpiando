using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
	public partial class RespuestaRecarga : PopupPage
    {
		public RespuestaRecarga ()
		{
			InitializeComponent ();
            list.ItemsSource = _Global.ListaRecargas.Lista;
        }

        public void ActualizarItemSource() {
            list.ItemsSource = null;
            list.ItemsSource = _Global.ListaRecargas.Lista;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
          
        }
    }
}
