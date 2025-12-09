using MOBILE_TEST.ViewModels;
using MOBILE_TEST.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MOBILE_TEST
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(TodoPage), typeof(TodoPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
