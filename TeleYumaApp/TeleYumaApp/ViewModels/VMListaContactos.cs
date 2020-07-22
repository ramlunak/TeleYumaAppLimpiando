using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
//using TeleYumaApp.BottonBar;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

namespace TeleYumaApp.ViewModels
{
    public class VMListaContactos : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VMListaContactos(object page = null)
        {
            CargarContactos();
        }

        private Page _TabIndex;
        public Page TabIndex
        {
            get { return _TabIndex; }
            set { _TabIndex = value; OnPropertyChanged(); }
        }

        public async Task CargarContactos()
        {
            await _Global.phone.CargarContactos();
            AgruparContactos(_Global.ListaContactos);
        }

        public List<EContacto> Contactos { get; set; }
        public ObservableCollection<EContacto> Items { get; set; }
        private ObservableCollection<Grouping<string, EContacto>> _ItemsGrouped { get;set; }
        public ObservableCollection<Grouping<string, EContacto>> ItemsGrouped
        {
            get { return _ItemsGrouped; }
            set { _ItemsGrouped = value; OnPropertyChanged(); }
        }

        public void AgruparContactos(List<EContacto> list)
        {
            Contactos = list;

            Items = new ObservableCollection<EContacto>(Contactos);

            var sorted = from item in Items
                         orderby item.Nombre
                         group item by item.Nombre[0].ToString() into itemGroup
                         select new Grouping<string, EContacto>(itemGroup.Key, itemGroup);

            ItemsGrouped = new ObservableCollection<Grouping<string, EContacto>>(sorted);

            RefreshDataCommand = new Command(
                async () => await RefreshData());
        }

        public async void GetListContactos()
        {

            Contactos = await EContacto.GetListaContactos();
            _Global.ListaContactos = Contactos;

        }

        public ICommand RefreshDataCommand { get; set; }

        async Task RefreshData()
        {
            IsBusy = true;
            await Task.Delay(2000);
            _Global.Vistas.ListaContactos.LlenarLista();
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
