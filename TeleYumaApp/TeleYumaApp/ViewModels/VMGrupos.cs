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
    public class VMGrupos : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public CustomEntry txtSearch { get; set; }

        public GrupoSMS LongPressSelected { get; set; }

        public VMGrupos()
        {
            popupOpcionesVisible = false;
            //CargarSMS();

        }

        public async void CargarGrupos()
        {
            try
            {
                //var lista1 = await _Global.CurrentAccount.GetAllSms();

                //foreach (var item in lista1)
                //{
                //    item.Ingresar();
                //}

                var lista = await _Global.phone.GetAllSms();

                var results = from p in lista
                              group p by p.NumeroTelefono into g
                              select new GrupoSMS
                              {
                                  numero = g.Key,
                                  ListaSMS = g.ToList(),

                              };


                _Global.GruposDeListasSMS = results.ToList();
                Grupos = new ObservableCollection<GrupoSMS>(_Global.GruposDeListasSMS);

            }
            catch (Exception ex)
            {

                ;
            }


        }


        public void ReferenciarTxtSearch(ref CustomEntry search)
        {
            txtSearch = search;
        }

        private bool _popupOpcionesVisible;

        public bool popupOpcionesVisible
        {
            get { return _popupOpcionesVisible; }
            set { _popupOpcionesVisible = value; OnPropertyChanged(); }
        }

        private bool _ShowNews;

        public bool ShowNews
        {
            get { return _ShowNews; }
            set { _ShowNews = value; }
        }


        private bool _ShowSerach;

        public bool ShowSerach
        {
            get { return _ShowSerach; }
            set { _ShowSerach = value; OnPropertyChanged(); }
        }


        ObservableCollection<GrupoSMS> _grupos { get; set; }
        public ObservableCollection<GrupoSMS> Grupos { get { return _grupos; } set { _grupos = value; OnPropertyChanged(); } }


        public async Task CargarSMS()
        {
            CargarGrupos();
            //_Global.GruposDeListasSMS = await _Global.ListaGrupos.ListaGrupos();
            //Grupos = new ObservableCollection<GrupoSMS>(_Global.GruposDeListasSMS);
            await Task.Delay(3000);
            CargarSMS();
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

        public void SearchTextChangedExecute(object parameter)
        {
            var e = parameter as TextChangedEventArgs;

            Grupos = new ObservableCollection<GrupoSMS>(_Global.GruposDeListasSMS.Where(x => x.ultimoSMS.Contains(e.NewTextValue)));
        }

        public async void EliminarExecute(object parameter)
        {
            var result = await CurrentPage.DisplayAlert("TeleYuma", "Está seguro que desea eliminar la conversación", "Eliminar", "Cancelar");
            if (result)
            {
                popupOpcionesVisible = false;
                foreach (var item in LongPressSelected.ListaSMS)
                {
                    item.Delete();
                }

            }
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


    }
}
