﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinBase.BottonBar;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace XamarinBase.Pages
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
           
        }

        private async void accountToolbarItem_Clicked(object sender, EventArgs e)
        {
           
        }


    }
}