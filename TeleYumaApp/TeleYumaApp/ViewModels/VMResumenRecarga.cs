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

namespace TeleYumaApp.ViewModels
{

    public class VMResumenRecarga : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VMResumenRecarga()
        {
            Recargas = new ObservableCollection<Recarga>(_Global.ListaRecargas.Lista);            
        }

        public void ActualizarResumen()
        {
            Recargas = new ObservableCollection<Recarga>(_Global.ListaRecargas.Lista);
        }

        ObservableCollection<Recarga> _Recargas { get; set; }
        public ObservableCollection<Recarga> Recargas { get { return _Recargas; } set { _Recargas = value; OnPropertyChanged(); } }

    }
}
