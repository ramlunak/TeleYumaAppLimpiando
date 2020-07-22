using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
using TeleYumaApp.Teleyuma;
using System.Threading.Tasks;

namespace TeleYumaApp.ViewModels
{

    public class VMCompras : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public VMCompras()
        {
            popupOpcionesVisible = false;
            Compras = new ObservableCollection<Compra>();
            CargarDetalle();
        }

        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }

        public void CargarDetalle()
        {

            Subtotal = Compras.ToList().Sum(x => x.Precio);
            Cargo = 0;

        }

        public Compra SelectedItem { get; set; }

        private bool _popupOpcionesVisible;

        public bool popupOpcionesVisible
        {
            get { return _popupOpcionesVisible; }
            set { _popupOpcionesVisible = value; OnPropertyChanged(); }
        }

        private float _Subtotal;

        public float Subtotal
        {
            get { return _Subtotal; }
            set { _Subtotal = value; OnPropertyChanged(); }
        }
        

        private float _Cargo;

        public float Cargo
        {
            get { return _Cargo; }
            set { _Cargo = value; OnPropertyChanged(); }
        }


        ObservableCollection<Compra> _Compras { get; set; }
        public ObservableCollection<Compra> Compras { get { return _Compras; } set { _Compras = value; OnPropertyChanged(); } }

        private ICommand _TarjetaCreditoCommand;
        public ICommand TarjetaCreditoCommand
        {
            get
            {
                if (_TarjetaCreditoCommand == null)
                {
                    _TarjetaCreditoCommand = new RelayCommand(TarjetaCreditoExecute, CanSubmitExecute);
                }
                return _TarjetaCreditoCommand;
            }
        }

        public async void TarjetaCreditoExecute(object parameter)
        {
            ;
        }

        private ICommand _FrameCommand;
        public ICommand FrameCommand
        {
            get
            {
                if (_FrameCommand == null)
                {
                    _FrameCommand = new RelayCommand(FrameExecute, CanSubmitExecute);
                }
                return _FrameCommand;
            }
        }

        public void FrameExecute(object parameter)
        {
            if (SelectedItem != null)
            {
                this.Compras.Remove(SelectedItem);
                var rec = _Global.ListaRecargas.Lista.Where(x=>x.numero == SelectedItem.Producto).First();
                _Global.ListaRecargas.Lista.Remove(rec);
                CargarDetalle();
                popupOpcionesVisible = false;
            }
        }

        public void Eliminar()
        {
            if (SelectedItem != null)
            {
                this.Compras.Remove(SelectedItem);
                var rec = _Global.ListaRecargas.Lista.Where(x => x.numero == SelectedItem.Producto).First();
                _Global.ListaRecargas.Lista.Remove(rec);
                CargarDetalle();
                popupOpcionesVisible = false;
            }
        }

        private ICommand _deleteCommand;
        public ICommand deleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(deleteExecute, CanSubmitExecute);
                }
                return _deleteCommand;
            }
        }

        public async void deleteExecute(object parameter)
        {
            ;
        }

        private ICommand _popupOpcionesHideCommand;
        public ICommand popupOpcionesHideCommand
        {
            get
            {
                if (_popupOpcionesHideCommand == null)
                {
                    _popupOpcionesHideCommand = new RelayCommand(popupOpcionesHideExecute, CanSubmitExecute);
                }
                return _popupOpcionesHideCommand;
            }
        }

        public void popupOpcionesHideExecute(object parameter)
        {
            popupOpcionesVisible = false;
        }


    }
}
