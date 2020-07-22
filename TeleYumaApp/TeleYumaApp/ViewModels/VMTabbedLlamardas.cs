using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Windows.Input;

namespace TeleYumaApp.ViewModels
{


    public class VMTabbedLlamardas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;

        private TabbedPage _tbPage;
        private ContentPage _page;
               
        
        public VMTabbedLlamardas()
        {
           
        }

        public void CargarContenido(TabbedPage tabbedPage)
        {
            if (tabbedPage != null)
            {
                this._tbPage = tabbedPage;                
            }
        }

        public void SelectedPage(string name)
        {
            this._page = this._tbPage.FindByName<ContentPage>(name);
            this._tbPage.CurrentPage = _page;
        }
               
    }
}
