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

namespace TeleYumaApp.ViewModels
{
    public class prueba
    {
        public string nombre { get; set; }
    }

    public class VMPrueba : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VMPrueba()
        {          
            Grupos = new ObservableCollection<prueba>();           
        }

        public void ViewCell_ChildAdded(object sender, ElementEventArgs e)
        {
            ;
        }

        ObservableCollection<prueba> _grupos { get; set; }
        public ObservableCollection<prueba> Grupos { get { return _grupos; } set { _grupos = value; OnPropertyChanged(); } }

        private prueba _ItemSelected;
        public prueba ItemSelected
        {
            get { return _ItemSelected; }
            set { _ItemSelected = value; OnPropertyChanged(); }
        }
             

        private bool CanSubmitExecute(object parameter)
        {
            return true;
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

        public static object _last = null;

        public async void EnviarExecute(object parameter)
        {
            var p = new prueba { nombre = "asdas" };
            Grupos.Add(p);
            ItemSelected = p;
        }


    }
}
