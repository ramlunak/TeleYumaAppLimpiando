using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.PagesNew
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CambiarClave : ContentPage
    {
        public CambiarClave()
        {
            InitializeComponent();
            BindingContext = new ViewModels.VMCambiarCLave();
        }
    }
}