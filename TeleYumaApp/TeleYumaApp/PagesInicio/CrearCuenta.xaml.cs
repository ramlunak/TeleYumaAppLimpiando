using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.PagesInicio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearCuenta : ContentPage
    {
        public string Codigo_verificacion { get; set; }

        public string Prefijo { get; set; }

        [DefaultValue(TipoTransaction.New)]
        public TipoTransaction Transaction { get; set; }

        public CrearCuenta()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public void CargarEventosValidar()
        {
            txt_firstname.TextChanged += delegate (object o, TextChangedEventArgs args)
            {
                var entry = o as Entry;
                if (string.IsNullOrEmpty(entry.Text))
                {
                    validate_txt_firstname.TextColor = Color.Red;
                    validate_txt_firstname.Text = "Campo Obligatorio";
                }
                else
                {
                    validate_txt_firstname.Text = "";
                }
            };

            txt_lastname.TextChanged += delegate (object o, TextChangedEventArgs args)
            {
                var entry = o as Entry;
                if (string.IsNullOrEmpty(entry.Text))
                {
                    validate_txt_lastname.TextColor = Color.Red;
                    validate_txt_lastname.Text = "Campo Obligatorio";
                }
                else
                {
                    validate_txt_lastname.Text = "";
                }
            };

            txt_email.TextChanged += delegate (object o, TextChangedEventArgs args)
            {
                var entry = o as Entry;
                if (string.IsNullOrEmpty(entry.Text))
                {
                    validate_txt_email.TextColor = Color.Red;
                    validate_txt_email.Text = "Campo Obligatorio";
                }
                else
                {
                    validate_txt_email.Text = "";
                }
            };

            txt_Password.TextChanged += delegate (object o, TextChangedEventArgs args)
            {
                var entry = o as Entry;
                if (string.IsNullOrEmpty(entry.Text))
                {
                    validate_txt_Password.TextColor = Color.Red;
                    validate_txt_Password.Text = "Campo Obligatorio";
                }
                else if (entry.Text.Length < 6)
                {
                    validate_txt_Password.Text = "Mínimo 6 caracteres";
                }
                else if (entry.TextColor == Color.Red)
                {
                    validate_txt_Password.Text = "Contraseña insegura";
                }
                else
                {
                    validate_txt_Password.Text = "";
                }
            };

            txt_Password2.TextChanged += delegate (object o, TextChangedEventArgs args)
            {
                var entry = o as Entry;
                if (string.IsNullOrEmpty(entry.Text))
                {
                    validate_txt_Password2.TextColor = Color.Red;
                    validate_txt_Password2.Text = "Campo Obligatorio";
                }
                else if (entry.Text.Length < 6)
                {
                    validate_txt_Password2.Text = "Mínimo 6 caracteres";
                }              
                else if (txt_Password.Text != txt_Password2.Text)
                {
                    validate_txt_Password2.Text = "Las contraseñas no coinsiden";
                }
                else
                {
                    validate_txt_Password2.Text = "";
                }
            };


            pkr_pais.TextChanged += delegate (object o, TextChangedEventArgs args)
            {
                var entry = o as Entry;
                if (string.IsNullOrEmpty(entry.Text))
                {
                    validate_pkr_pais.TextColor = Color.Red;
                    validate_pkr_pais.Text = "Campo Obligatorio";
                }
                else
                {
                    validate_pkr_pais.Text = "";
                }
            };

            txt_phone1.TextChanged += delegate (object o, TextChangedEventArgs args)
            {
                var entry = o as Entry;
                if (string.IsNullOrEmpty(entry.Text))
                {
                    validate_txt_phone1.TextColor = Color.Red;
                    validate_txt_phone1.Text = "Campo Obligatorio";
                }
                else if (entry.TextColor == Color.Red)
                {
                    validate_txt_phone1.Text = "Solo números";
                }
                else
                {
                    validate_txt_phone1.Text = "";
                }
            };
        }


        public void CargarDatos()
        {

            btnCompletar.Text = "Guardar";
            BindingContext = _Global.CurrentAccount;

            pkr_pais.IsEnabled = false;
            txt_phone1.IsEnabled = false;

            try
            {
                var pais = _Global.CurrentAccount.country.ToString();
                Prefijo = Pais.GetPrefijo(pais);
                _Global.Vistas.PageNewListaPaises.Selected = new EPais { PrefijoTelefonico = Prefijo, Nombre = pais };
                _Global.PaisSeleccionado = new EPais { PrefijoTelefonico = Prefijo, Nombre = pais };
                ;
            }
            catch 
            {
                DisplayAlert("Prefijo", "no existe prefijo del pais:" + _Global.CurrentAccount.country, "ok");
            }
            try
            {
                var cantidad = Prefijo.Count<char>();
                var phone = _Global.CurrentAccount.phone1.Remove(0, cantidad);
                txt_phone1.Text = phone;
            }
            catch (Exception ex)
            {
                DisplayAlert("ex", ex.Message, "ok");
            }
        }

        public void LimpiarForm()
        {
            Transaction = TipoTransaction.New;
            // btnCompletar.Text = "Crear";
            BindingContext = null;
        }

        public bool ValidarForm()
        {
            bool value = true;



            if (string.IsNullOrEmpty(txt_firstname.Text))
            {
                validate_txt_firstname.Text = "Campo obligatorio";
                value = false;
            }
            else if (txt_firstname.TextColor == Color.Red)
            {
                validate_txt_firstname.Text = "Solo Letras";
                value = false;
            }

            if (string.IsNullOrEmpty(txt_lastname.Text))
            {
                validate_txt_lastname.Text = "Campo obligatorio";
                value = false;
            }
            else if (txt_lastname.TextColor == Color.Red)
            {
                validate_txt_lastname.Text = "Solo Letras";
                value = false;
            }

            if (string.IsNullOrEmpty(txt_email.Text))
            {
                validate_txt_email.Text = "Campo obligatorio";
                value = false;
            }
            else if (txt_email.TextColor == Color.Red)
            {
                validate_txt_email.Text = "Ej:user@gmail.com";
                value = false;
            }

            if (string.IsNullOrEmpty(txt_Password.Text))
            {
                validate_txt_Password.Text = "Campo obligatorio";
                value = false;
            }
            else if (txt_Password.Text.Length < 6)
            {
                validate_txt_Password.Text = "Mínimo 6 caracteres";
                value = false;
            }

            if (string.IsNullOrEmpty(txt_Password2.Text))
            {
                validate_txt_Password2.Text = "Campo obligatorio";
                value = false;
            }
            else if (txt_Password.Text.Length < 6)
            {
                validate_txt_Password2.Text = "Mínimo 6 caracteres";
                value = false;
            }
            else if (txt_Password.Text != txt_Password2.Text)
            {
                validate_txt_Password2.Text = "Las contraseñas no coinsiden";
                value = false;
            }

            if (string.IsNullOrEmpty(pkr_pais.Text))
            {
                validate_pkr_pais.Text = "Campo obligatorio";
                value = false;
            }


            if (string.IsNullOrEmpty(txt_phone1.Text))
            {
                validate_txt_phone1.Text = "Campo obligatorio";
                value = false;
            }
            else if (txt_phone1.TextColor == Color.Red)
            {
                validate_txt_phone1.Text = "Solo números";
                value = false;
            }

            return value;
        }


        private void btnAyuda_Clicked(object sender, EventArgs e)
        {
            // this.Navigation.PushModalAsync(new PagesInicio.Ayuda());
        }

        private void btnTerminosCondiciones_Clicked(object sender, EventArgs e)
        {
            // this.Navigation.PushModalAsync(new PagesInicio.TerminosCondiciones());
        }

        private void btnCompletar_Clicked(object sender, EventArgs e)
        {
            if (!ValidarForm()) return;
            if (Transaction == TipoTransaction.New)
                SetAccount();
            if (Transaction == TipoTransaction.Edit)
                ActualizarAccount();
        }

        public async void SetAccount()
        {
            Prefijo = _Global.PaisSeleccionado.PrefijoTelefonico;
            var telefono = (Prefijo + txt_phone1.Text).Trim();
            try
            {


                var now = DateTime.Now;

                var YY = now;
                var MM = now.Month.ToString();
                var DD = now.Day.ToString();

                if (MM.Length == 1)
                    MM = "0" + MM;
                if (DD.Length == 1)
                    DD = "0" + DD;

                var activationDate = now.Year + "-" + MM + "-" + DD;


                _Global.CurrentAccount.id = "a" + telefono.Trim();
                _Global.CurrentAccount.iso_4217 = "USD";
                _Global.CurrentAccount.i_customer = 260271;  //Online customers
                _Global.CurrentAccount.i_distributor = 282645;  //distributor  customers
                _Global.CurrentAccount.batch_name = "260271-di-pinless";
                _Global.CurrentAccount.country = _Global.PaisSeleccionado.Nombre.Trim();
                _Global.CurrentAccount.billing_model = -1;
                _Global.CurrentAccount.control_number = 1;
                _Global.CurrentAccount.h323_password = _Global.ServicePassword.ToString(); ;
                _Global.CurrentAccount.i_product = 22791;
                _Global.CurrentAccount.activation_date = activationDate.Trim();
                _Global.CurrentAccount.firstname = txt_firstname.Text.Trim();
                _Global.CurrentAccount.lastname = txt_lastname.Text.Trim();
                
                _Global.CurrentAccount.email = txt_email.Text.Trim();
                _Global.CurrentAccount.login = txt_email.Text.Trim();
                _Global.CurrentAccount.password = txt_Password.Text.Trim();
            }
            catch (Exception ex)
            {
                await DisplayAlert("TeleYuma", ex.Message, "OK");
            }

            var valid = await ValidarCuenta();
            if (valid)
            {
               // _Global.Vistas.ConfirmarTelefono.SendSms();
                await this.Navigation.PushModalAsync(_Global.Vistas.ConfirmarTelefono);
            }

        }

        public async Task<bool> ValidarCuenta()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = _Global.CurrentAccount });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.validate_account_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        var id = JsonConvert.DeserializeObject<AccountObject>(json).account_info.id;
                        if (id is null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        await DisplayAlert("TeleYuma", ErrorHandling.faultstring, "ok");
                        return false;
                    }

                }
                catch
                {
                    await DisplayAlert("TeleYuma", "Error al conectarse con el servidor, compruebe su conexión a internet.", "ok");
                    return false;
                }

            }

        }

        public async void ActualizarAccount()
        {

            Prefijo = _Global.PaisSeleccionado.PrefijoTelefonico;
            var telefono = (Prefijo + txt_phone1.Text).Trim();
            try
            {

               // _Global.CurrentAccount.id = "a" + telefono.Trim();
             //   _Global.CurrentAccount.iso_4217 = "USD";
               // _Global.CurrentAccount.i_customer = 260271;  //Online customers
                _Global.CurrentAccount.i_distributor = 282645;  //distributor  customers
               // _Global.CurrentAccount.batch_name = "260271-di-pinless";
               // _Global.CurrentAccount.country = _Global.PaisSeleccionado.Nombre.Trim();
                //_Global.CurrentAccount.billing_model = -1;
               // _Global.CurrentAccount.control_number = 1;
                //_Global.CurrentAccount.h323_password = _Global.ServicePassword.ToString(); ;
               // _Global.CurrentAccount.i_product = 22791;
               // _Global.CurrentAccount.activation_date = _Global.CurrentAccount.activation_date;
                _Global.CurrentAccount.firstname = txt_firstname.Text.Trim();
                _Global.CurrentAccount.lastname = txt_lastname.Text.Trim();
                //_Global.CurrentAccount.phone1 = telefono.Trim();
                _Global.CurrentAccount.email = txt_email.Text.Trim();
                _Global.CurrentAccount.login = txt_email.Text.Trim();
                _Global.CurrentAccount.password = txt_Password.Text.Trim();
            }
            catch (Exception ex)
            {
                await DisplayAlert("TeleYuma", ex.Message, "OK");
            }

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {

                    var param = JsonConvert.SerializeObject(new { account_info = _Global.CurrentAccount });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.update_account + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                         //_Global.Vistas.PageHome.ActualizarInformacionCuenta();
                        await DisplayAlert("TeleYuma", "Datos actualizados correctamente.", "ok");
                        _Global.SQLiteLogin.Salir();
                        try
                        {
                            //actualizar usuario SQlite
                            _Global.SQLiteLogin.i_account = _Global.CurrentAccount.i_account;
                            _Global.SQLiteLogin.phone1 = _Global.CurrentAccount.phone1;
                            _Global.SQLiteLogin.isloged = true;
                            _Global.SQLiteLogin.user = _Global.CurrentAccount.login;
                            _Global.SQLiteLogin.password = _Global.CurrentAccount.password;
                            _Global.SQLiteLogin.Ingresar();
                        }
                        catch{}
                      
                        await this.Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("TeleYuma", ErrorHandling.faultstring, "ok");
                    }

                }
                catch
                {
                    await DisplayAlert("TeleYuma", "Error al conectarse con el servidor, compruebe su conexión a internet.", "ok");
                }

            }

        }

        private void pkr_pais_Focused(object sender, FocusEventArgs e)
        {
            Navigation.PushAsync(_Global.Vistas.PageNewListaPaises);
            _Global.Vistas.PageNewListaPaises.SearchBarFocus(ref pkr_pais);
        }


    }
}
