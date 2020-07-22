using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
//using TeleYumaApp.BottonBar;
using System.Linq;

namespace TeleYumaApp.ViewModels
{
    public class VMHome : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public BottonBar.BottomBarPage BarPage = new BottonBar.BottomBarPage();

        public VMHome(BottonBar.BottomBarPage page = null)
        {
            if (page is null) return;
            BarPage = page;
            _Global.phone.CargarContactos();          
        }
        
        public void CargarPantallaSms()
        {
            //  TabIndex = BarPage.Children[1];
        }

        private Page _TabIndex;
        public Page TabIndex
        {
            get { return _TabIndex; }
            set { _TabIndex = value; OnPropertyChanged(); }
        }

    }
}
