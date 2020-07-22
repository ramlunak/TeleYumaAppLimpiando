using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TeleYumaApp.Class;
using TeleYumaApp.Teleyuma;
using Xamarin.Forms;


namespace TeleYumaApp
{
    public partial class App : Application
    {
        public interface ICloseApplication
        {
            void closeApplication();
        }

        public interface ICallService
        {
            void Call(string number);
        }

        public App()
        {
            InitializeComponent();
            MainPage = new Pages.HomeTabbedPage();
        }
        
      
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            _Global.IsOpen = false;
            //  Application.Current.Properties["MainPage"] = Application.Current.MainPage;
            ;
        }

        protected override void OnResume()
        {
            // Application.Current.MainPage = (Page)Application.Current.Properties["MainPage"];
        }

    }

}
