using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.BottonBar;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace TeleYumaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabbedPage : BottomBarPage
    {
        public HomeTabbedPage()
        {
            InitializeComponent();           
        }


        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var valor = await DisplayAlert("TeleYuma", "Esta seguro que desea cerrar su sesión", "SI", "NO");
                if (valor)
                {

                    _Global.SQLiteLogin.Salir();
                    _Global.RunTask = false;
                    Application.Current.MainPage = new NavigationPage(new PagesInicio.Login());

                }
            }
            catch
            {


            }
        }

        private async void accountToolbarItem_Clicked(object sender, EventArgs e)
        {
           
        }


    }
}