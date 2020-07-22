using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using TeleYumaApp.Teleyuma;
using Xamarin.Forms;

namespace TeleYumaApp
{

    public partial class Recargas : ContentPage
    {
        public Recargas()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMRecargas;
            _Global.VM.VMRecargas.CargarPromo();

        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _Global.VM.VMRecargas.ListaProductoVisible = false;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            _Global.Vistas.ListaContactos.Transaction = TipoTransaction.Llamar;
            this.Navigation.PushAsync(_Global.Vistas.ListaContactos);
            _Global.Vistas.ListaContactos.ReferenciarNumero();
        }

        private void btnPagar_Clicked(object sender, EventArgs e)
        {

            if (!_Global.VM.VMCompras.Compras.Any())
            {
                DisplayAlert("TeleYuma", "La lista de compras esta vacia", "OK");
            }
            else
            {
                _Global.VM.VMRecargas.OpcionesRecargaVisible = false;
                _Global.VM.VMCompras.CargarDetalle();
                this.Navigation.PushAsync(new Compras());
            }
        }

        private void btnNew_Clicked(object sender, EventArgs e)
        {
            _Global.VM.VMRecargas.OpcionesRecargaVisible = false;
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            _Global.VM.VMRecargas.ListaPrecios = false;
        }

        private void TexBoxPrefijo_Focused(object sender, FocusEventArgs e)
        {
            _Global.VM.VMRecargas.Prefijo = string.Empty;
        }

        private void TexBoxPrefijo_Unfocused(object sender, FocusEventArgs e)
        {
            _Global.VM.VMRecargas.CargarPaisByPrefijo();
        }
    }
}
