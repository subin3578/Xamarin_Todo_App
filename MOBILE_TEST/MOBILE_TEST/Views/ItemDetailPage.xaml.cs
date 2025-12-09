using MOBILE_TEST.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MOBILE_TEST.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}