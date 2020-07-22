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
	public partial class TipoRecarga : ContentPage
	{
		public TipoRecarga ()
		{
			InitializeComponent ();
            BindingContext = _Global.Promocion;

		}

        private void btnReservar_Clicked(object sender, EventArgs e)
        {
            _Global.AccionRecarga = "Reserva";
            this.Navigation.PushAsync(_Global.Vistas.ListaRecargas);
        }

        private void btnRecargarAhora_Clicked(object sender, EventArgs e)
        {
            _Global.AccionRecarga = "RecargarAhora";
            this.Navigation.PushAsync(_Global.Vistas.ListaRecargas);
        }
    }
}
