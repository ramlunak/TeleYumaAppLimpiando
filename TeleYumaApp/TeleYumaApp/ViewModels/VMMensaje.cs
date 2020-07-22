using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;

namespace TeleYumaApp.ViewModels
{
    public class VMMensaje : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        ObservableCollection<Esms> _items { get; set; }
        public ObservableCollection<Esms> Items { get { return _items; } set { _items = value; OnPropertyChanged(); } }

        ObservableCollection<Grouping<string, Esms>> _itemsGrouped { get; set; }
        public ObservableCollection<Grouping<string, Esms>> ItemsGrouped { get { return _itemsGrouped; } set { _itemsGrouped = value; OnPropertyChanged(); } }

        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public string NumeroTelefono { get; set; }

        private string _Icono;

        public string Icono
        {
            get { return _Icono; }
            set { _Icono = value; OnPropertyChanged(); }
        }

        private bool _popupOpcionesVisible;

        public bool popupOpcionesVisible
        {
            get { return _popupOpcionesVisible; }
            set { _popupOpcionesVisible = value; OnPropertyChanged(); }
        }

        public async void ActualizarLista()
        {
            Icono = _Global.GrupoSMS.icono;

            NumeroTelefono = _Global.GrupoSMS.numero;
            Contacto = _Global.GrupoSMS.contacto;

            var sorted = from item in _Global.GrupoSMS.ListaSMS
                         orderby item.Id ascending
                         group item by item.FechaLarga into itemGroup
                         select new Grouping<string, Esms>(itemGroup.Key, itemGroup);

            ItemsGrouped = new ObservableCollection<Grouping<string, Esms>>(sorted);

            await Task.Delay(1000);
            var last = _Global.GrupoSMS.ListaSMS.LastOrDefault();
            ListView.ScrollTo(last, ScrollToPosition.MakeVisible, false);
        }

        public Esms LongPressSelected { get; set; }

        public CustomEntry txtSearch { get; set; }
        public ListView ListView { get; set; }

        public void ReferenciarTxtSearch(ref CustomEntry search)
        {
            txtSearch = search;
        }

        public void ReferenciarListView(ref ScrollableListView listView)
        {
            ListView = listView;
        }

        private string _Contacto;

        public string Contacto
        {
            get { return _Contacto; }
            set { _Contacto = value; OnPropertyChanged(); }
        }


        private string _Mensaje;

        public string Mensaje
        {
            get { return _Mensaje; }
            set { _Mensaje = value; OnPropertyChanged(); }
        }

        private bool _ShowSerach;

        public bool ShowSerach
        {
            get { return _ShowSerach; }
            set { _ShowSerach = value; OnPropertyChanged(); }
        }


        private bool _ShowSendImage;

        public bool ShowSendImage
        {
            get { return _ShowSendImage; }
            set { _ShowSendImage = value; OnPropertyChanged(); }
        }

        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }


        private ICommand _SearchCommand;
        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new RelayCommand(SearchExecute, CanSubmitExecute);
                }
                return _SearchCommand;
            }
        }

        private ICommand _SearchTappedCommand;
        public ICommand SearchTappedCommand
        {
            get
            {
                if (_SearchTappedCommand == null)
                {
                    _SearchTappedCommand = new RelayCommand(SearchTappedExecute, CanSubmitExecute);
                }
                return _SearchTappedCommand;
            }
        }

        private ICommand _SearchTextChangedCommand;
        public ICommand SearchTextChangedCommand
        {
            get
            {
                if (_SearchTextChangedCommand == null)
                {
                    _SearchTextChangedCommand = new RelayCommand(SearchTextChangedExecute, CanSubmitExecute);
                }
                return _SearchTextChangedCommand;
            }
        }

        private ICommand _LlamarCommand;
        public ICommand LlamarCommand
        {
            get
            {
                if (_LlamarCommand == null)
                {
                    _LlamarCommand = new RelayCommand(LlamarCommandExecute, CanSubmitExecute);
                }
                return _LlamarCommand;
            }
        }

        private ICommand _EditorFocusedCommand;
        public ICommand EditorFocusedCommand
        {
            get
            {
                if (_EditorFocusedCommand == null)
                {
                    _EditorFocusedCommand = new RelayCommand(EditorFocusedExecute, CanSubmitExecute);
                }
                return _EditorFocusedCommand;
            }
        }

        private ICommand _EnviarCommand;
        public ICommand EnviarCommand
        {
            get
            {
                if (_EnviarCommand == null)
                {
                    _EnviarCommand = new RelayCommand(EnviarExecute, CanSubmitExecute);
                }
                return _EnviarCommand;
            }
        }

        private ICommand _ViewCellLongPressCommand;
        public ICommand ViewCellLongPressCommandCommand
        {
            get
            {
                if (_ViewCellLongPressCommand == null)
                {
                    _ViewCellLongPressCommand = new RelayCommand(ViewCellLongPressExecute, CanSubmitExecute);
                }
                return _ViewCellLongPressCommand;
            }
        }

        private ICommand _EliminarCommand;
        public ICommand EliminarCommand
        {
            get
            {
                if (_EliminarCommand == null)
                {
                    _EliminarCommand = new RelayCommand(EliminarExecute, CanSubmitExecute);
                }
                return _EliminarCommand;
            }
        }

        public void EliminarExecute(object parameter)
        {
            //var result = await CurrentPage.DisplayAlert("TeleYuma", "Está seguro que desea eliminar la conversación", "Eliminar", "Cancelar");
            //if (result)
            //{
            popupOpcionesVisible = false;

            ItemsGrouped.First(x => x.First().NumeroTelefono == LongPressSelected.NumeroTelefono).Remove(LongPressSelected);
            LongPressSelected.Delete();
            _Global.GrupoSMS.ListaSMS.Remove(LongPressSelected);
            ActualizarLista();
            //}

        }

        public decimal monto { get; set; }
        public int ItemHeight { get; set; }

        public async void EnviarExecute(object parameter)
        {
            var date = DateTime.Now;
            var fecha = date.Month + "/" + date.Day + "/" + date.Year;
            var hora = DateTime.Now.ToString("hh:mm tt").Replace(".", "").ToUpper();
            var id = 1;
            try
            {
                id = _Global.GrupoSMS.sms.Id;
            }
            catch
            {

            }

            var newSms = new Esms
            {
                Id = id + 1,
                Fecha = fecha,
                Hora = hora,
                // IsNew = false,
                isSend = false,
                Firma = "",
                NumeroTelefono = _Global.GrupoSMS.numero,
                NombreContacto = _Global.GrupoSMS.nombreContacto,
                monto = monto,
                Mensaje = Mensaje,
                i_account = _Global.CurrentAccount.i_account,
                Phone1 = _Global.CurrentAccount.phone1,
                Token = "AFAfytf56AR56AY67T76g67guysdf67",
                ItemHeight = ItemHeight
            };
            newsms = newSms;

            if(newSms.monto > _Global.CurrentAccount.balance)
            {
               CurrentPage.DisplayAlert("TeleYuma", "No tiene sufuciente balance para mandar este mensaje", "ok");              
                return;
            }

            _Global.GrupoSMS.ListaSMS.Add(newSms);
            ActualizarNewSms(newSms);

            try
            {
                ItemsGrouped.First(x => x.First().NumeroTelefono == newsms.NumeroTelefono).Add(newsms);
            }
            catch
            {
                ActualizarLista();
            }

            ListView.ScrollTo(newSms, ScrollToPosition.MakeVisible, false);

            Mensaje = string.Empty;
            
        }
        
        public async void ActualizarNewSms(Esms esms)
        {
            var result = await esms.Enviar();
            Mensaje = string.Empty;
            if (result.ErrorCode is null || result.ErrorCode == "0")

            {
                esms.image = SMSImageInfo.ic_check_ok_18pt_3x.ToString();
                await _Global.CurrentAccount.MakeTransaction_Manualcharge(esms.monto, "sms a "+esms.NumeroTelefono);
            }
            else
            {
                esms.image = SMSImageInfo.ic_error_outline_red_18pt_3x.ToString();
            }

            var grupo = ItemsGrouped.First(x => x.First().NumeroTelefono == esms.NumeroTelefono);
            grupo.Remove(esms);
            _Global.GrupoSMS.ListaSMS.Remove(esms);
            esms.isSend = true;
            _Global.GrupoSMS.ListaSMS.Add(esms);
            grupo.Add(esms);          
            esms.Ingresar();
            ActualizarLista();
        }



        public Esms newsms = new Esms();

        private Esms _SelectedItem;

        public Esms SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        public void SearchExecute(object parameter)
        {
            ShowSerach = true;
            txtSearch.Focus();

        }

        public void SearchTappedExecute(object parameter)
        {
            ShowSerach = false;
            txtSearch.Unfocus();
            txtSearch.Text = string.Empty;
        }

        public void EditorFocusedExecute(object parameter)
        {
            ShowSendImage = true;
        }

        public void LlamarCommandExecute(object parameter)
        {
            var llamada = "7868717144,011" + NumeroTelefono + "#";
            DependencyService.Get<ICallService>().Call(llamada);
        }

        public void SearchTextChangedExecute(object parameter)
        {
            var e = parameter as TextChangedEventArgs;

            Items = new ObservableCollection<Esms>(_Global.ListaSMS.Where(x => x.Mensaje.Contains(e.NewTextValue)));

            var sorted = from item in Items
                         orderby item.Id ascending
                         group item by item.FechaLarga into itemGroup
                         select new Grouping<string, Esms>(itemGroup.Key, itemGroup);

            ItemsGrouped = new ObservableCollection<Grouping<string, Esms>>(sorted);

        }

        public void ViewCellLongPressExecute(object parameter)
        {
            ;
        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            await Task.Delay(2000);

            //----
            IsBusy = false;
        }

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                OnPropertyChanged();
                ((Command)RefreshDataCommand).ChangeCanExecute();
            }

        }

        public class Grouping<K, T> : ObservableCollection<T>
        {
            public K Key { get; private set; }

            public Grouping(K key, IEnumerable<T> items)
            {
                Key = key;
                foreach (var item in items)
                    this.Items.Add(item);
            }
        }

    }
}
