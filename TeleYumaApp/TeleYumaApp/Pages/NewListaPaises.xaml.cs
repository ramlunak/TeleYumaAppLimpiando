using System;
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

namespace TeleYumaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewListaPaises : ContentPage
    {
        public EPais Selected { get; set; }
        public Entry txtPais { get; set; }
        public Label lblPrefijo { get; set; }

        public Button btnPrefijo { get; set; }

        public NewListaPaises()
        {
            InitializeComponent();
            list.ItemsSource = Pais.GetList();

        }

       
        public void SearchBarFocus(ref Entry txt)
        {
            txtPais = txt;
            txt.Unfocus();
            // SearchBar.Focus();
        }

        public void SearchBarFocus(ref CustomEntry txt)
        {
            txtPais = txt;
            txt.Unfocus();
            // SearchBar.Focus();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list.ItemsSource = Pais.GetList();
            }

            else
            {
                list.ItemsSource = Pais.GetList().Where(x => x.Nombre.ToLower().Contains(e.NewTextValue.ToLower()));
            }
        }

        private void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ;
            try
            {
                if (list.SelectedItem != null)
                {
                    Selected = (EPais)list.SelectedItem;
                    txtPais.Text = "(+" + Selected.PrefijoTelefonico + ") " + Selected.Nombre;
                    _Global.PaisSeleccionado = (EPais)list.SelectedItem;
                    this.Navigation.PopAsync();
                    list.SelectedItem = null;
                   
                }
            }
            catch
            {

            }

        }
    }
}
