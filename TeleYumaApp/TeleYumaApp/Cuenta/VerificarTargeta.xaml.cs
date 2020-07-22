using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TeleYumaApp.Class;

namespace TeleYumaApp.Cuenta
{
    public partial class VerificarTarjeta : ContentPage
    {

        public VerificarTarjeta()
        {
            InitializeComponent();
            CargarMontoGuardao();
        }

        private decimal CaptureMonto = 0;

        private MakeAccountTransactionResponse makeAccountTransactionResponse = new MakeAccountTransactionResponse();

        public async void CargarMontoGuardao()
        {
            if (_Global.CaptureMonto == 0)
            {
                CaptureMonto = RandomMonto();
                _Global.StaticCaptureMonto = CaptureMonto.ToString();                
                await EnviarAutorize();
                await _Global.CurrentAccount.New_Actualizar(_Global.StaticCaptureMonto);
                _Global.StaticCaptureMonto = null;
            }
        }

        decimal RandomMonto()
        {
            var Ran = new Random();
            int numeroAleatorio = Ran.Next(51, 99);
            decimal monto;
            monto = Convert.ToDecimal("0," + numeroAleatorio.ToString());
            if (monto >= 1)
            {
                monto = Convert.ToDecimal("0." + numeroAleatorio.ToString());
            }
            return monto;
        }

        public void MostarCangandoAutorize(bool value)
        {
            layoutEnviandoAutorize.IsVisible = value;
            layoutContenido.IsVisible = !value;
        }

        public async Task<bool> EnviarAutorize()
        {
            MostarCangandoAutorize(true);         
            var respuesta_autorize = await _Global.CurrentAccount.New_MakeTransaction_AuthorizationOnly(CaptureMonto);
            if ((respuesta_autorize.result_code == "1" || respuesta_autorize.result_code == "A01") && respuesta_autorize.transaction_id != "0")
            {               
                _Global.CurrentAccount.AuthorizationOnlyTransaction_id = respuesta_autorize.transaction_id;
                MostarCangandoAutorize(false);               
                return true;
            }
            else
            {
                _Global.CurrentAccount.cont2 = "0";
                _Global.CurrentAccount.AuthorizationOnlyTransaction_id = "0";
                await _Global.CurrentAccount.New_Actualizar();
                MostarCangandoAutorize(false);

                await DisplayAlert("TeleYuma", "Transacción denegada,Verifique el balance de su tarjeta de crédito o que sus datos sean correctos", "Ok");
                try
                {
                    MostarCangandoAutorize(false);
                    await this.Navigation.PopAsync();
                }
                catch
                {

                }
                MostarCangandoAutorize(false);
                return false;
            }

           
        }

        //public async Task<innoverit> SendSms()
        //{
           
        //    var telefono = _Global.CurrentAccount.phone1;
        //    var body = "verificar tarjeta:" + CaptureMonto;

        //    var smsConfirmacio = new Esms
        //    {
        //        SMS = body,
        //        NumeroTelefono = "5355043317",
        //        RemitenteNumero = "TeleYumaApp"
        //    };
        //    var respuesta = await smsConfirmacio.Enviar();
        //    if (respuesta.ErrorCode == "null")
        //    {
        //        return new innoverit { delivery_status = "OK" };
        //    }
        //    else
        //    {
        //        await DisplayAlert("TeleYuma", respuesta.ErrorMessage, "OK");
        //        return new innoverit { error = "1" };
        //    }


        //}

        public async void Comprobar()
        {
                              
            if (_Global.CurrentAccount.phone2 == EstadoTarjeta.IntentoFallido.ToString())
            {
                _Global.CurrentAccount.phone2 = EstadoTarjeta.CuentaBloqueada.ToString();
                await _Global.CurrentAccount.New_Actualizar();

                frameIntentoFallido.IsVisible = true;
                frameComprovacionCorrecta.IsVisible = false;
                frameFormulario.IsVisible = true;

                return;
            }

            if (_Global.CurrentAccount.phone2 == EstadoTarjeta.CuentaBloqueada.ToString())
            {

                _Global.CurrentAccount.blocked = "Y";
                _Global.CurrentAccount.phone2 = "";
                _Global.CurrentAccount.cont2 = "0";
                await _Global.CurrentAccount.New_Actualizar();

                frameIntentoFallido.IsVisible = false;
                frameComprovacionCorrecta.IsVisible = false;
                frameFormulario.IsVisible = true;

                return;
            }

        }


        private void txtUnidadTextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtUnidad.Text.Length == 1)
                txtDecena.Focus();
            if (e.NewTextValue.Length > 1)
                txtUnidad.Text = e.OldTextValue;
        }

        private async void txtDecenaTextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtDecena.Text.Length == 1)
            {
                loadinComprobando.IsVisible = true;
                await Task.Delay(3000);

                decimal monto;

                monto = Convert.ToDecimal("0," + txtUnidad.Text + txtDecena.Text);
                if (monto >= 1)
                {
                    monto = Convert.ToDecimal("0." + txtUnidad.Text + txtDecena.Text);
                }

                if (_Global.CaptureMonto == monto)
                {
                    //Se ha comprobado la tarjeta
                    _Global.CurrentAccount.AuthorizationOnlyTransaction_id = "";
                    _Global.CurrentAccount.phone2 = EstadoTarjeta.TarjetaComprovada.ToString();
                    await _Global.CurrentAccount.New_Actualizar();

                    frameIntentoFallido.IsVisible = false;
                    frameComprovacionCorrecta.IsVisible = true;
                    frameFormulario.IsVisible = false;
                    await DisplayAlert("TeleYuma", "Ha comprobado que es el propietario de la tarjeta, gracias por usar nuestros servicios ", "Ok");
                                        
                    try
                    {
                        //Quitar pagina de actualizar cuenta
                        var pages = Navigation.NavigationStack.ToList();
                        foreach (var page in pages)
                        {
                            if (page.GetType() == typeof(Cuenta.TarjetaCredito))
                                Navigation.RemovePage(page);
                        }

                        await this.Navigation.PopAsync();                      
                                            
                        return;
                    }
                    catch
                    {

                    }
                }
                else
                {
                    txtUnidad.Text = string.Empty;
                    txtDecena.Text = string.Empty;                   
                    txtUnidad.Focus();

                    if (_Global.CurrentAccount.phone2 == "" || _Global.CurrentAccount.phone2 == null)
                    {
                        _Global.CurrentAccount.phone2 = EstadoTarjeta.IntentoFallido.ToString();
                        frameIntentoFallido.IsVisible = true;
                        frameComprovacionCorrecta.IsVisible = false;
                        frameFormulario.IsVisible = true;
                        await _Global.CurrentAccount.New_Actualizar();                        
                    }
                    Comprobar();
                }
                loadinComprobando.IsVisible = false;
            }
            if (e.NewTextValue.Length > 1)
                txtDecena.Text = e.OldTextValue;
        }
    }
}
