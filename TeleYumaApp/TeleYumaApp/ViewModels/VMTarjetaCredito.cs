using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace TeleYumaApp.ViewModels
{
    public class VMTarjetaCredito : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public VMTarjetaCredito()
        {
            CargarPaises();
        }

        private CountryInfo _CountrySelectedItem;

        public CountryInfo CountrySelectedItem
        {
            get { return _CountrySelectedItem; }
            set { _CountrySelectedItem = value; }
        }


        private SubdivisionInfo _SubCountrySelectedItem;

        public SubdivisionInfo SubCountrySelectedItem
        {
            get { return _SubCountrySelectedItem; }
            set { _SubCountrySelectedItem = value; }
        }


        public List<CountryInfo> _Countrys { get; set; }
        public List<CountryInfo> Countrys
        {
            get { return _Countrys; }
            set
            {
                if (_Countrys != value)
                {
                    _Countrys = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<SubdivisionInfo> _SubCountrys { get; set; }
        public List<SubdivisionInfo> SubCountrys
        {
            get { return _SubCountrys; }
            set
            {
                if (_SubCountrys != value)
                {
                    _SubCountrys = value;
                    OnPropertyChanged();
                }
            }
        }

        public async void CargarPaises(string iso_3166_1_a2 = null)
        {
            var contrys = await _Global.telinta.GetCountryList();
            Countrys = new List<CountryInfo>(contrys);
            if (iso_3166_1_a2 != null)
            {
                var country = contrys.Where(x => x.iso_3166_1_a2 == iso_3166_1_a2).First();
                CountrySelectedItem = country;
            }
        }

        public async void CargarSubCountris(int i_country_subdivision)
        {
            var subcountry = SubCountrys.Where(x => x.i_country_subdivision == i_country_subdivision).First();
            SubCountrySelectedItem = subcountry;

        }

        public async void CargarPaises()
        {
            var contrys = await _Global.telinta.GetCountryList();
            Countrys = new List<CountryInfo>(contrys);

        }

        public List<string> PickerData
        {
            get
            {
                var dataCard = new List<string>();
                dataCard.Add("MasterCard");
                dataCard.Add("American Express");
                dataCard.Add("Discover");
                dataCard.Add("VISA");
                return dataCard;
            }
        }



        private string _cardNumber;

        public string cardNumber
        {
            get
            {
                return _cardNumber;
            }
            set
            {
                cardNumber = value;
                OnPropertyChanged();
            }

        }

        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }


        private ICommand _SelectedIndexChangedCommand;
        public ICommand SelectedIndexChangedCommand
        {
            get
            {
                if (_SelectedIndexChangedCommand == null)
                {
                    _SelectedIndexChangedCommand = new RelayCommand(SelectedIndexChangedExecute, CanSubmitExecute);
                }
                return _SelectedIndexChangedCommand;
            }
        }

        public async void SelectedIndexChangedExecute(object parameter)
        {
            if (CountrySelectedItem != null)
            {
                var subcountrys = await _Global.telinta.GetSubdivisionList(CountrySelectedItem.iso_3166_1_a2);
                SubCountrys = new List<SubdivisionInfo>(subcountrys);
            }

        }
    }
}
