using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Windows.Input;

namespace TeleYumaApp.ViewModels
{


    public class VMLlamar : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;

        public rlkControles.entry TxtTelefono { get; set; }

        public VMLlamar()
        {
           
        }

        private string _numero;

        public string Numero
        {
            get { return _numero; }
            set { _numero = value; OnPropertyChanged(); }
        }

        private int _CursorPosition;

        public int CursorPosition
        {
            get { return _CursorPosition; }
            set { _CursorPosition = value; OnPropertyChanged(); }
        }

        public Command MyCommand { protected set; get; }

        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }

        private ICommand _LabelNumeroTappedCommand;
        public ICommand LabelNumeroTappedCommand
        {
            get
            {
                if (_LabelNumeroTappedCommand == null)
                {
                    _LabelNumeroTappedCommand = new RelayCommand(LabelNumeroTappedExecute, CanSubmitExecute);
                }
                return _LabelNumeroTappedCommand;
            }
        }
        public async void LabelNumeroTappedExecute(object parameter)
        {
            if (parameter is null) return;

            try
            {

                if (string.IsNullOrEmpty(Numero))
                {
                    Numero = Numero + (string)parameter;
                    return;
                }

                var lista = Numero.ToList();


                var i = 0;
                bool insert = false;
                var text = string.Empty;

                var posi = CursorPosition;

                if (posi == 0)
                    posi = lista.Count;

                foreach (var item in lista)
                {
                    if (posi == i)
                    {
                        text = text+ (string)parameter + item;
                        insert = true;
                    }
                    else
                        text = text + item;
                    i++;
                }

                if (insert)
                {
                    var position = CursorPosition;
                    Numero = text;
                    CursorPosition = position + 1;
                    //if(CursorPosition !=0)
                    //CursorPosition = position + 1;
                }

                else
                    Numero = text + (string)parameter;

            }
            catch (Exception ex)
            {
                Numero = Numero + (string)parameter;
            }



            //TxtTelefono.Focus();

        }


        private ICommand _BorrarTappedCommand;
        public ICommand BorrarTappedCommand
        {
            get
            {
                if (_BorrarTappedCommand == null)
                {
                    _BorrarTappedCommand = new RelayCommand(BorrarTappedExecute, CanSubmitExecute);
                }
                return _BorrarTappedCommand;
            }
        }


        public static int Position { get; set; }

        public async void BorrarTappedExecute(object parameter)
        {
            if (string.IsNullOrEmpty(Numero)) return;
            var lista = Numero.ToList();
            try
            {
                Position = CursorPosition;
                if (Position == 0)
                    Position = lista.Count;
                lista.RemoveAt(Position - 1);

                var text = string.Empty;
                foreach (var item in lista)
                {
                    text = text + item;
                }
                Numero = text;
                if(CursorPosition != 0)
                CursorPosition = Position - 1;
            }
            catch (Exception ex)
            {
                lista.RemoveAt(lista.Count - 1);
                var text = string.Empty;
                foreach (var item in lista)
                {
                    text = text + item;
                }
                Numero = text;
                ;
            }
            // TxtTelefono.Focus();
        }

    }
}
