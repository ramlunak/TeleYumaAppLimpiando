
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace TeleYumaApp.Contactos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaContactos : ContentPage
    {
        public EContacto ContactoSeleccionado = new EContacto();

        public string Tipo { get; set; }
        public Entry txtUserNauta { get; set; }
        public Entry txtTelefono { get; set; }
        public Entry txtPais { get; set; }
        public bool txtNumero { get; set; }

        [DefaultValue(TipoTransaction.New)]
        public TipoTransaction Transaction { get; set; }

        public ListaContactos()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMListaContactos;
        }


        void imgLlamar_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                Image img = (Image)sender;
                var paren = img.Parent;
                var contac = (EContacto)paren.BindingContext;
                var numero = Regex.Replace(contac.Telefono, @"[^0-9A-Za-z]", "", RegexOptions.None);

                _Global.Vistas.Llamar.LlenarTxtTelefono(numero);
                this.Navigation.PopAsync();
                this.Navigation.PushAsync(_Global.Vistas.Llamar);

            }
            catch { }
        }

        public void txtLlamar(ref CustomEntry telefono)
        {
            Tipo = "llamar";
            txtTelefono = telefono;
           
        }

        public void ReferenciarNumero()
        {
            Tipo = "llamar";
            txtNumero = true;
          
        }

        public void HideButtonAdd(ref Entry pais, ref Entry telefono)
        {
            Tipo = "movil";
            txtTelefono = telefono;
            txtPais = pais;
          
        }


        public void HideButtonAdd(ref CustomEntry pais, ref CustomEntry telefono)
        {
            Tipo = "movil";
            txtTelefono = telefono;
            txtPais = pais;
          
        }


        public void HideButtonAdd(ref Entry UserNauta)
        {
            Tipo = "nauta";
            txtUserNauta = UserNauta;
         
        }

        public void HideButtonAdd(ref CustomEntry UserNauta)
        {
            Tipo = "nauta";
            txtUserNauta = UserNauta;
          
        }
        
        public async void LlenarLista()
        {                    
            //try
            //{
            //    BindingContext = null;
            //    BindingContext = new ListaContactosViewModel(_Global.ListaContactos);

            //    if (Transaction == TipoTransaction.New)
            //        LayoutButtonAdd.IsVisible = true;
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }


        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;

       public async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem == null)
                return;
            txtBuscar.Text = string.Empty;

            if (Transaction == TipoTransaction.Select)
            {
                var d = e.SelectedItem;
                //Telefono
                var contactoSelect = _Global.VM.VMListaContactos.Contactos.First(x => x.Nombre.Equals(e.SelectedItem.ToString()));
                _Global.ContactoSeleccionado = contactoSelect;
                var numero = Regex.Replace(contactoSelect.Telefono, @"[^0-9A-Za-z]", "", RegexOptions.None);

                if (_Global.PaisSeleccionado.PrefijoTelefonico == "53")
                {
                    int tam_var = numero.Length;
                    String Var_Sub = numero.Substring((tam_var - 8), 8);
                    numero = Var_Sub;
                }

                if (Tipo == "movil")
                {
                    txtTelefono.Text = numero;


                    // txtPais.Text = "(+" + contactoSelect.Prefijo + ")" + contactoSelect.Pais;
                }
                else
                {
                    txtUserNauta.Text = contactoSelect.UserNauta;
                }

                //_Global.PaisSeleccionado = new EPais { Nombre = contactoSelect.Pais, PrefijoTelefonico = contactoSelect.Prefijo };

                ((ListView)sender).SelectedItem = null;
                await this.Navigation.PopAsync();
            }

            if (Transaction == TipoTransaction.New)
            {
                ContactoSeleccionado = _Global.VM.VMListaContactos.Contactos.First(x => x.Nombre.Equals(ListViewContactos.SelectedItem.ToString()));
                _Global.PaisSeleccionado = new EPais { Nombre = ContactoSeleccionado.Pais, PrefijoTelefonico = ContactoSeleccionado.Prefijo };
              
            }

            if (Transaction == TipoTransaction.Llamar)
            {
                _Global.ContactoSeleccionado = (EContacto)e.SelectedItem;
                //Telefono               
                var numero = Regex.Replace(_Global.ContactoSeleccionado.Telefono, @"[^0-9A-Za-z]", "", RegexOptions.None);

                if (_Global.PaisSeleccionado.PrefijoTelefonico != null)
                    if (_Global.PaisSeleccionado.PrefijoTelefonico == "53")
                    {
                        int tam_var = numero.Length;
                        String Var_Sub = numero.Substring((tam_var - 8), 8);
                        numero = Var_Sub;
                    }

                if (txtNumero)
                {
                    _Global.VM.VMRecargas.txtNumero = numero;
                    txtNumero = false;
                }
                else
                    txtTelefono.Text = numero;

                ((ListView)sender).SelectedItem = null;
                await this.Navigation.PopAsync();
            }

        }

        private void BtnAddContacto_Tapped(object sender, EventArgs e)
        {
            //var FormAdd = new Contactos.AddContacto();
            //FormAdd.Transaction = TipoTransaction.New;
            //PopupNavigation.PushAsync(FormAdd);
        }

        //private async void BtnDelContacto_Tapped(object sender, EventArgs e)
        //{
        //    var con = (ContentView)sender;
        //    var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");

        //    if (result)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int tel = Convert.ToInt32(e.NewTextValue);
                var busqueda = _Global.ListaContactos.Where(x => Regex.Replace(x.Telefono, @"[^0-9A-Za-z]", "", RegexOptions.None).Contains(e.NewTextValue)).ToList();
                 _Global.VM.VMListaContactos.AgruparContactos(busqueda);
            }
            catch (Exception)
            {
                try
                {
                    var busqueda = _Global.ListaContactos.Where(x => x.Nombre.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
                    _Global.VM.VMListaContactos.AgruparContactos(busqueda);
                }
                catch (Exception ex)
                {
                    _Global.VM.VMListaContactos.AgruparContactos(new List<EContacto>());
                }
            }

        }

      
    }


    //class ListaContactosViewModel : INotifyPropertyChanged
    //{
    //    public static List<EContacto> Contactos = new List<EContacto>();
    //    public ObservableCollection<EContacto> Items { get; }
    //    private ObservableCollection<Grouping<string, EContacto>> _ItemsGrouped { get; set; }
    //    public ObservableCollection<Grouping<string, EContacto>> ItemsGrouped
    //    {
    //        get { return _ItemsGrouped; }
    //        set { _ItemsGrouped = value; OnPropertyChanged(); }
    //    }

    //    public ListaContactosViewModel(List<EContacto> list)
    //    {
    //        Contactos = list;
           
    //        Items = new ObservableCollection<EContacto>(Contactos);

    //        var sorted = from item in Items
    //                     orderby item.Nombre
    //                     group item by item.Nombre[0].ToString() into itemGroup
    //                     select new Grouping<string, EContacto>(itemGroup.Key, itemGroup);

    //        ItemsGrouped = new ObservableCollection<Grouping<string, EContacto>>(sorted);

    //        RefreshDataCommand = new Command(
    //            async () => await RefreshData());
    //    }

    //    public async void GetListContactos()
    //    {

    //        Contactos = await EContacto.GetListaContactos();
    //        _Global.ListaContactos = Contactos;

    //    }

    //    public ICommand RefreshDataCommand { get; }

    //    async Task RefreshData()
    //    {
    //        IsBusy = true;
    //        await Task.Delay(2000);
    //        _Global.Vistas.ListaContactos.LlenarLista();
    //        IsBusy = false;
    //    }

    //    bool busy;
    //    public bool IsBusy
    //    {
    //        get { return busy; }
    //        set
    //        {
    //            busy = value;
    //            OnPropertyChanged();
    //            ((Command)RefreshDataCommand).ChangeCanExecute();
    //        }
    //    }


    //    public event PropertyChangedEventHandler PropertyChanged;
    //    void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    //    public class Grouping<K, T> : ObservableCollection<T>
    //    {
    //        public K Key { get; private set; }

    //        public Grouping(K key, IEnumerable<T> items)
    //        {
    //            Key = key;
    //            foreach (var item in items)
    //                this.Items.Add(item);
    //        }
    //    }
    //}
}
