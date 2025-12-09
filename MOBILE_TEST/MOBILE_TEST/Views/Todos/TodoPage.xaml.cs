using MOBILE_TEST.Models;
using MOBILE_TEST.Models.UI;
using MOBILE_TEST.Services;
using MOBILE_TEST.ViewModels;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOBILE_TEST.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoPage : ContentPage
    {


        public TodoPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            string userId = Session.CurrentUser.QM05IPID;
            //string userId = "104286";

            var vm = new TodoViewModel();
            BindingContext = vm;

            vm.LoadTodoCommand.Execute(userId);
            
        }

        // 로그아웃 버튼 클릭
        private async void Logout_Clicked(object sender, EventArgs e)
        {

            bool answer = await DisplayAlert("로그아웃", "로그아웃 하시겠습니까?", "예", "아니요");

            if (!answer)
                return;

            var vm = new LoginViewModel();
            vm.Logout();

            Application.Current.MainPage = new NavigationPage(new LoginPage());

        }
    
    }


}
