using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOBILE_TEST.Views.Todos.BottomSheet.CategoryBottomSheet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryBottomSheet : ContentPage
    {
        public string SelectedCategory { get; set; }

        public CategoryBottomSheet()
		{
            InitializeComponent();
     
		}

        private async void SelectCategory(object sender, EventArgs e)
        {
            var label = sender as Label;
            if (label == null) return;

            SelectedCategory = label.Text;

            await Navigation.PopModalAsync();
        }

        private async void CloseSheet(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}