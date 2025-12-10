using MOBILE_TEST.Models;
using MOBILE_TEST.Models.UI;
using MOBILE_TEST.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOBILE_TEST.Views.Todos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoPageDesign : ContentPage
    {
        public TodoPageDesign()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            var vm = new TodoViewModel();
            BindingContext = vm;

        }

    }
}
