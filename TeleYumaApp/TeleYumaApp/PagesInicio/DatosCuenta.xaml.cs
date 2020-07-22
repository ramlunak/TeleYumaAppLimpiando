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
    public partial class DatosCuenta : ContentPage
    {
        public string Codigo_verificacion { get; set; }

        public string Prefijo { get; set; }

        [DefaultValue(TipoTransaction.New)]
        public TipoTransaction Transaction { get; set; }

        public DatosCuenta()
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
                    validate_txt_firstname.TextColor = Color.Orange;
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
                    validate_txt_lastname.TextColor = Color.Orange;
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
                    validate_txt_email.TextColor = Color.Orange;
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
                    validate_txt_Password.TextColor = Color.Orange;
                    validate_txt_Password.Text = "Campo Obligatorio";
                }
                else if (entry.Text.Length < 6)
                {
                    validate_txt_Password.Text = "Mínimo 6 caracteres";
                }
                else if (entry.TextColor == Color.Orange)
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
                    validate_txt_Password2.TextColor = Color.Orange;
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
            
        }


        public void CargarDatos()
        {
                    
            BindingContext = _Global.CurrentAccount;

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

            }
            catch (Exception ex)
            {
                DisplayAlert("ex", ex.Message, "ok");
            }
        }

        public void LimpiarForm()
        {
            Transaction = TipoTransaction.New;
           
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



            return value;
        }
              

        private void btnCompletar_Clicked(object sender, EventArgs e)
        {
           
            if (!ValidarForm()) return;           
                SetAccount();
          
        }

        public async void SetAccount()
        {
            _Global.CurrentAccount.firstname = txt_firstname.Text.Trim();
            _Global.CurrentAccount.lastname = txt_lastname.Text.Trim();
            _Global.CurrentAccount.email = txt_email.Text.Trim();
            _Global.CurrentAccount.login = txt_email.Text.Trim();
            _Global.CurrentAccount.h323_password = _Global.ServicePassword.ToString();                    
            _Global.CurrentAccount.password = txt_Password.Text.Trim();
            Loading.IsVisible = true;
            var valid = await ValidarCuenta();
            if (valid)
            {
              await CrearCuenta();
            }
            Loading.IsVisible = false;
        }

        public async Task CrearCuenta()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = _Global.CurrentAccount });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.add_account + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                       
                        await DisplayAlert("TeleYuma", "Cuenta creada correctamente,utilize su correo como usuario para acceder a la aplicación", "ok");
                        Application.Current.MainPage = new PagesInicio.Login();
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}
