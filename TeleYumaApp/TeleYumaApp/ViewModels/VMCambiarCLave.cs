using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace TeleYumaApp.ViewModels
{

    public class VMCambiarCLave : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VMCambiarCLave()
        {


        }

        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }

        #region Propiedades


        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private string _claveActual;

        public string ClaveActual
        {
            get { return _claveActual; }
            set { _claveActual = value; OnPropertyChanged(); }
        }

        private string _nuevaClave;

        public string NuevaClave
        {
            get { return _nuevaClave; }
            set { _nuevaClave = value; OnPropertyChanged(); }
        }

        private string _confirmarClave;

        public string ConfirmarClave
        {
            get { return _confirmarClave; }
            set { _confirmarClave = value; OnPropertyChanged(); }
        }


        private bool _claveIncorrecta;

        public bool ClaveIncorrecta
        {
            get { return _claveIncorrecta; }
            set { _claveIncorrecta = value; OnPropertyChanged(); }
        }

        private bool _claveInsegura;

        public bool ClaveInsegura
        {
            get { return _claveInsegura; }
            set { _claveInsegura = value; OnPropertyChanged(); }
        }


        private bool _claveNoCoinciden;

        public bool ClaveNoCoinciden
        {
            get { return _claveNoCoinciden; }
            set { _claveNoCoinciden = value; OnPropertyChanged(); }
        }

        #endregion

        #region CambiarClaveCommand
        private ICommand _CambiarClaveCommand;
        public ICommand CambiarClaveCommand
        {
            get
            {
                if (_CambiarClaveCommand == null)
                {
                    _CambiarClaveCommand = new RelayCommand(CambiarClaveExecute, CanSubmitExecute);
                }
                return _CambiarClaveCommand;
            }
        }

        public async void CambiarClaveExecute(object parameter)
        {
            if (string.IsNullOrEmpty(ClaveActual) || string.IsNullOrEmpty(NuevaClave) || string.IsNullOrEmpty(ConfirmarClave))
            {
                CurrentPage.DisplayAlert("TeleYuma", "Complete los campos del formulario", "OK");
                if (ClaveIncorrecta) return;
            }

            ClaveIncorrecta = _Global.CurrentAccount.password != ClaveActual;
            if (ClaveIncorrecta) return;

            ClaveInsegura = !Regex.IsMatch(NuevaClave, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,16}$");
            if (ClaveInsegura) return;

            ClaveNoCoinciden = NuevaClave != ConfirmarClave;
            if (ClaveNoCoinciden) return;

            IsLoading = true;
            var old_password = _Global.CurrentAccount.password;
            _Global.CurrentAccount.password = NuevaClave;

            if (await UpdateCuenta(true))
            {
                if (await UpdateCuenta(false))
                {
                    await CurrentPage.DisplayAlert("TeleYuma", "La contraseña ha sido cambiada exitosamente", "OK");
                    await CurrentPage.Navigation.PopModalAsync();
                }
                else
                {
                    _Global.CurrentAccount.password = old_password;
                }
            }
            IsLoading = false;

        }
        #endregion

        #region CancelarCommand
        private ICommand _CancelarCommand;
        public ICommand CancelarCommand
        {
            get
            {
                if (_CancelarCommand == null)
                {
                    _CancelarCommand = new RelayCommand(CancelarExecute, CanSubmitExecute);
                }
                return _CancelarCommand;
            }
        }

        public async void CancelarExecute(object parameter)
        {
            try
            {
                CurrentPage.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {

                ;
            }
        }
        #endregion

        public async Task<bool> UpdateCuenta(bool validate)
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = _Global.CurrentAccount });
                    if (validate)
                    {
                        URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.validate_account_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    }
                    else
                    {
                        URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.update_account + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    }
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        string id = null;
                        if (validate)
                        {
                            id = JsonConvert.DeserializeObject<AccountObject>(json).account_info.id;
                        }
                        else
                        {
                            id = JsonConvert.DeserializeObject<account_info>(json).i_account.ToString();
                        }


                        if (id is null)
                        {
                            await CurrentPage.DisplayAlert("TeleYuma", "Error al conectarse con el servidor, compruebe su conexión a internet.", "ok");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        await CurrentPage.DisplayAlert("TeleYuma", ErrorHandling.faultstring, "ok");
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    await CurrentPage.DisplayAlert("TeleYuma", "Error al conectarse con el servidor, compruebe su conexión a internet.", "ok");
                    return false;
                }

            }

        }

    }
}
