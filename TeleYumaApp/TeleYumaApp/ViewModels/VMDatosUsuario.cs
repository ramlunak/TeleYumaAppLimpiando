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

    public class VMDatosUsuario : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VMDatosUsuario()
        {
            Nombre = _Global.CurrentAccount.fullname;
            Email = _Global.CurrentAccount.email;
            Phone = _Global.CurrentAccount.phone1;
            Bandera = Pais.GetIso2(_Global.CurrentAccount.country);

            ShowEditName = false;
            ShowEditEmail = false;

        }

        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }

        #region Propiedades

        private string _nombre;

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        private string _phone;

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged(); }
        }

        private string _bandera;

        public string Bandera
        {
            get { return _bandera; }
            set { _bandera = value; OnPropertyChanged(); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; OnPropertyChanged(); }
        }

        private bool _showEditName;

        public bool ShowEditName
        {
            get { return _showEditName; }
            set { _showEditName = value; OnPropertyChanged(); ShowLabelName = !value; }
        }

        private bool _showEditEmail;

        public bool ShowEditEmail
        {
            get { return _showEditEmail; }
            set { _showEditEmail = value; OnPropertyChanged(); ShowLabelEmail = !value; }
        }

        private bool _showLabelName;

        public bool ShowLabelName
        {
            get { return _showLabelName; }
            set { _showLabelName = value; OnPropertyChanged(); }
        }

        private bool _showLabelemail;

        public bool ShowLabelEmail
        {
            get { return _showLabelemail; }
            set { _showLabelemail = value; OnPropertyChanged(); }
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
            CurrentPage.Navigation.PushModalAsync(new PagesNew.CambiarClave());
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

        #region ActualizarNameCommand
        private ICommand _ActualizarNameCommand;
        public ICommand ActualizarNameCommand
        {
            get
            {
                if (_ActualizarNameCommand == null)
                {
                    _ActualizarNameCommand = new RelayCommand(ActualizarNameExecute, CanSubmitExecute);
                }
                return _ActualizarNameCommand;
            }
        }

        public async void ActualizarNameExecute(object parameter)
        {
            if (string.IsNullOrEmpty(Nombre))
            {
                await CurrentPage.DisplayAlert("TeleYuma", "El nombre no puede estar em blanco.", "ok");
                return;
            }

            IsLoading = true;
            
            var account = _Global.CurrentAccount;


            account.firstname = Nombre;
            account.lastname = "";
            account.cont1 = Nombre;

            if (await UpdateCuenta(true, account))
            {
                if (!await UpdateCuenta(false, account))
                {                   
                    Nombre = _Global.CurrentAccount.fullname;
                }
            }
            else
            {             
                Nombre = _Global.CurrentAccount.fullname;
            }

            IsLoading = false;

            ShowEditName = false;


        }

        #endregion

        #region ActualizarEmailCommand
        private ICommand _ActualizarEmailCommand;
        public ICommand ActualizarEmailCommand
        {
            get
            {
                if (_ActualizarEmailCommand == null)
                {
                    _ActualizarEmailCommand = new RelayCommand(ActualizarEmailExecute, CanSubmitExecute);
                }
                return _ActualizarEmailCommand;
            }
        }

        public async void ActualizarEmailExecute(object parameter)
        {
            bool valido = Regex.IsMatch(Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (!valido)
            {
                await CurrentPage.DisplayAlert("TeleYuma", "Escriba un correo válido.", "ok");
                return;
            }

            IsLoading = true;
                    
            var account = _Global.CurrentAccount;

            account.email = Email;
            account.login = Email;
         
            if (await UpdateCuenta(true, account))
            {
                if (!await UpdateCuenta(false, account))
                {
                    Email = _Global.CurrentAccount.email;
                }
            }
            else
            {              
                Email = _Global.CurrentAccount.email; 
            }
            IsLoading = false;

            ShowEditEmail = false;
        }

        #endregion

        #region ShowEditNameCommand
        private ICommand _ShowEditNameCommand;
        public ICommand ShowEditNameCommand
        {
            get
            {
                if (_ShowEditNameCommand == null)
                {
                    _ShowEditNameCommand = new RelayCommand(ShowEditNameExecute, CanSubmitExecute);
                }
                return _ShowEditNameCommand;
            }
        }

        public async void ShowEditNameExecute(object parameter)
        {
            ShowEditName = true;
        }

        #endregion

        #region ShowEditEmailCommand
        private ICommand _ShowEditEmailCommand;
        public ICommand ShowEditEmailCommand
        {
            get
            {
                if (_ShowEditEmailCommand == null)
                {
                    _ShowEditEmailCommand = new RelayCommand(ShowEditEmailExecute, CanSubmitExecute);
                }
                return _ShowEditEmailCommand;
            }
        }

        public async void ShowEditEmailExecute(object parameter)
        {
            ShowEditEmail = true;
        }

        #endregion

        public async Task<bool> UpdateCuenta(bool validate,account_info accout_Info)
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = accout_Info });
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
