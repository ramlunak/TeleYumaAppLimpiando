using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Animations;
using TeleYumaApp.Class;
using Rg.Plugins.Popup.Services;
using Plugin.ContactService;
using System;
using System.Text.RegularExpressions;

namespace TeleYumaApp.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhoneContacs : PopupPage
    {
        public IList<Plugin.ContactService.Shared.Contact> ListaContactos = new List<Plugin.ContactService.Shared.Contact>();

        public Entry txtTelefono { get; set; }
        public PhoneContacs()
        {
            InitializeComponent();
           
        }

        public async void CargarContastos()
        {
            var Contactos = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            list.ItemsSource = null;
            list.ItemsSource = Contactos;
            ListaContactos = Contactos;
        }

        public void SearchBarFocus(ref Entry txt)
        {
            txtTelefono = txt;
            txt.Unfocus();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list.ItemsSource = ListaContactos;
            }

            else
            {
                try
                {
                    try
                    {
                        string result = "*554#878";
                        //string result = Regex.Replace("+554878", "[^0-9]+", string.Empty);
                        var ass = result.Contains(e.NewTextValue);
                        ;
                     //   var busqueda = ListaContactos.Where(x => Regex.Replace(x.Number, "[^0-9]+", string.Empty).Contains(e.NewTextValue)).ToList();
                      //  list.ItemsSource = busqueda;
                    }
                    catch 
                    {
                        ;
                    }
                       
                }
                catch
                {
                    var busqueda = ListaContactos.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
                    list.ItemsSource = busqueda;
                }


            }
        }

        private void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (list.SelectedItem != null)
                {
                    var Selected = (Plugin.ContactService.Shared.Contact)list.SelectedItem;
                    txtTelefono.Text = Selected.Number;
                    list.SelectedItem = null;
                    PopupNavigation.PopAsync();
                }
            }
            catch
            {

            }

        }
    }
}
