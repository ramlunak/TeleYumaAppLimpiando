using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using Xamarin.Forms;

namespace TeleYumaApp
{
    public partial class ResumenRecarga : ContentPage
    {
        public ResumenRecarga()
        {
            InitializeComponent();
            BindingContext = _Global.VM.VMResumenRecarga;
        }
    }
}
