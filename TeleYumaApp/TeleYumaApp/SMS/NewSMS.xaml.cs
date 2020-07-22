using Rg.Plugins.Popup.Services;
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
    public partial class NewSMS : ContentPage
    {
        public string numero { get; set; }
        public decimal monto { get; set; }
        public string sms { get; set; }
        public int ItemHeight { get; set; }
        public string image
        {
            get
            {
                return _Global.PaisSeleccionado.image;
            }
        }

        public NewSMS()
        {
            InitializeComponent();
        }

        private void txtSms_TextChanged(object sender, TextChangedEventArgs e)
        {

            var Length = ((Editor)sender).Text.Length;
            int cantSMS = Length / 160 + 1;
            int restantes = (160 * cantSMS) - Length;

            LabelCount.Text = restantes + "/" + cantSMS;

            monto = decimal.Round(Convert.ToDecimal(cantSMS * 0.05), 2);
            LabelCosto.Text = "$ " + monto;

            var H = ((Editor)sender).Height;
            var Lineas = Convert.ToInt32(((H - 10) / 20));
            ItemHeight = 60 + (Lineas * 20);


            //if (((Editor)sender).Height > 138)
            //    ((Editor)sender).HeightRequest = 138;


        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (e.NewTextValue != "" && e.NewTextValue != null)
            {
                if (e.NewTextValue == "(+53) Cuba")
                    ValidarNumeroCuba();
            }

        }

        public void ValidarNumeroCuba()
        {

            if (txtTelefono.Text != "" && txtTelefono.Text != null)
            {
                var numero = txtTelefono.Text;
                int tam_var = numero.Length;
                if (tam_var > 8)
                {
                    String Var_Sub = numero.Substring((tam_var - 8), 8);
                    txtTelefono.Text = Var_Sub;
                }
            }
        }

        public async void IMG_ENVIAR_Tapped(object sender, EventArgs e)
        {
            if (txtPais.Text == "" || txtPais.Text == null)
            {
                await DisplayAlert("TeleYuma", "Seleccione el País", "OK");
                return;
            }
            if (txtTelefono.Text == "" || txtTelefono.Text == null)
            {
                await DisplayAlert("TeleYuma", "Seleccione el Télefono", "OK");
                return;
            }
            if (txtSms.Text == "" || txtSms.Text == null)
            {
                await DisplayAlert("TeleYuma", "Ingrese el mensaje que desea enviar", "OK");
                return;
            }
            if (_Global.CurrentAccount.balance < Convert.ToDecimal(monto))
            {
                await DisplayAlert("TeleYuma", "No tiene saldo en la cuenta para mandar el mensaje", "OK");
                await this.Navigation.PopAsync();
                return;
            }

            numero = _Global.PaisSeleccionado.PrefijoTelefonico + txtTelefono.Text;

            var date = DateTime.Now;
            var fecha = date.Month + "/" + date.Day + "/" + date.Year;
            var hora = DateTime.Now.ToString("hh:mm tt").Replace(".", "").ToUpper();

            var id = 0;
            if (_Global.GrupoSMS.ListaSMS.Count > 0)
                id = _Global.GrupoSMS.sms.Id;

            var newSms = new Esms
            {
                Id = id + 1,
                Fecha = fecha,
                Hora = hora,
                Firma = "",
                NumeroTelefono = numero,
                RemitenteNumero = "",
                PrefijoPais = _Global.PaisSeleccionado.PrefijoTelefonico,
                isSend = false,
                monto = monto,
                Mensaje = txtSms.Text,
                i_account = _Global.CurrentAccount.i_account,
                Phone1 = _Global.CurrentAccount.phone1,
                Token = "AFAfytf56AR56AY67T76g67guysdf67",
                ItemHeight = ItemHeight
            };

            
            _Global.GrupoSMS.ListaSMS.Add(newSms);
          
            //newSms.Enviar();

            //_Global.Post<Esms>("sms", newSms);


            var contGrupos = 0;

            try
            {
                _Global.GrupoSMS = _Global.GruposDeListasSMS.First(x => x.numero == numero);
                contGrupos = _Global.GruposDeListasSMS.Where(x => x.numero == numero).ToList().Count;
            }
            catch (Exception)
            {

            }

            var pages = Navigation.NavigationStack.ToList();
            if (contGrupos > 0)
            {
                try
                {

                    //Quitar NewSMS del navegation             
                    foreach (var page in pages)
                    {
                        if (page.GetType() == typeof(SMS.NewSMS))
                            Navigation.RemovePage(page);
                    }
                    _Global.GrupoSMS.ListaSMS.Add(newSms);
                    await this.Navigation.PushAsync(_Global.Vistas.EnviarSMS);
                    _Global.VM.VMMensaje.ActualizarLista();

                }
                catch (Exception)
                {


                }
            }
            else
            {
                try
                {
                    _Global.GrupoSMS = new GrupoSMS
                    {
                        numero = numero,                                  
                        ListaSMS = new List<Esms>()
                    };
                    _Global.GrupoSMS.ListaSMS.Add(newSms);

                    //Quitar NewSMS del navegation             
                    foreach (var page in pages)
                    {
                        if (page.GetType() == typeof(SMS.NewSMS))
                            Navigation.RemovePage(page);
                    }

                    await this.Navigation.PushAsync(_Global.Vistas.EnviarSMS);
                    _Global.VM.VMMensaje.ActualizarLista();

                }
                catch (Exception ex)
                {

                    ;
                }
            }

            _Global.VM.VMMensaje.ActualizarNewSms(newSms);
           
            return;

        }

        public void CargarNewSMS()
        {
            var s = _Global.GruposDeListasSMS;
            var grupo = _Global.GruposDeListasSMS.First(x => x.numero == numero);
            if (grupo != null)
            {
                _Global.GrupoSMS = grupo;
                _Global.ListaSMS = grupo.ListaSMS;

                this.Navigation.PushAsync(_Global.Vistas.EnviarSMS);
            }

        }

        public void CargarListaSMS()
        {
            var s = _Global.GruposDeListasSMS;
            var grupo = _Global.GruposDeListasSMS.First(x => x.numero == numero);
            if (grupo != null)
            {
                _Global.GrupoSMS = grupo;
                _Global.ListaSMS = grupo.ListaSMS;

                this.Navigation.PushAsync(_Global.Vistas.EnviarSMS);
            }

        }

        private void txtPais_Focused(object sender, FocusEventArgs e)
        {
            Navigation.PushAsync(_Global.Vistas.PageNewListaPaises);
            _Global.Vistas.PageNewListaPaises.SearchBarFocus(ref txtPais);
        }

        private async void imgContactos_cliked(object sender, EventArgs e)
        {
            try
            {
                this.Navigation.PushAsync(_Global.Vistas.ListaContactos);
                _Global.Vistas.ListaContactos.Transaction = TipoTransaction.Select;
                _Global.Vistas.ListaContactos.HideButtonAdd(ref txtPais, ref txtTelefono);
                _Global.Vistas.ListaContactos.LlenarLista();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
