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

    public class VMPagar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public VMPagar()
        {

        }

        private string _Monto;

        public string Monto
        {
            get { return _Monto; }
            set { _Monto = value; OnPropertyChanged(); }
        }

        private string _Total;

        public string Total
        {
            get { return _Total; }
            set { _Total = value; OnPropertyChanged(); }
        }
        private string _NumeroTarjeta;

        public string NumeroTarjeta
        {
            get { return _NumeroTarjeta; }
            set { _NumeroTarjeta = value; OnPropertyChanged(); }
        }


        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }

        public void ActualizarInformacionMonto()
        {

            if (_Global.TipoRecarga == "movil")
            {
                Monto = " " + String.Format("{0:#,##0.00}", _Global.ListaRecargas.MontoLista) + " USD";
                Total = " " + String.Format("{0:#,##0.00}", _Global.ListaRecargas.TotalPagar) + " USD";

            }
            else if (_Global.TipoRecarga == "nauta")
            {
                Monto = " " + String.Format("{0:#,##0.00}", _Global.RecargaNauta.monto) + " USD";
                Total = " " + String.Format("{0:#,##0.00}", _Global.RecargaNauta.TotalPagar) + " USD";
            }
            else if (_Global.TipoRecarga == "agregar_saldo")
            {
                Monto = " " + String.Format("{0:#,##0.00}", _Global.MontoTransferenciaBancaria) + " USD";
                Total = " " + String.Format("{0:#,##0.00}", _Global.MontoTransferenciaBancaria) + " USD";
            }

        }




    }
}
