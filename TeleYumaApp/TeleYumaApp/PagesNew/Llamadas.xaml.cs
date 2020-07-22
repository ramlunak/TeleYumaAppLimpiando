using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeleYumaApp.PagesNew
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Llamadas : TabbedPage
    {
        public Llamadas()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMTabbedLlamardas;
            _Global.VM.VMTabbedLlamardas.CargarContenido(_tbPage);
        }

       
    }
}