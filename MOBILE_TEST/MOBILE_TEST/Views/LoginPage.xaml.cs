using MOBILE_TEST.Models;
using MOBILE_TEST.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOBILE_TEST.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = new LoginViewModel();

        }

        // 로그인 버튼 클릭

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;

            var vm = BindingContext as LoginViewModel;
            try
            {
                var user = new Models.Server.USER_QCT05M
                {

                    //QM05IPID = entryUserId.Text,
                    //QM05PSWD = entryPassword.Text

                    //개발 - ex) ID 104286
                    QM05IPID = "104286",
                    QM05PSWD = "104286"

                };

                await vm.Login(user);

            }

            catch (Exception ex)
            {
                Debug.WriteLine("로그인 실패: " + ex.Message);
                await DisplayAlert("오류", "로그인 중 문제가 발생했습니다.", "확인");
            }

            finally
            {
                btn.IsEnabled = true;
            }
        }


        private void EntryUserId_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as LoginViewModel;
            if (vm == null) return;

            string newText = e.NewTextValue ?? "";

          
            bool isNumeric = newText.All(char.IsDigit);

            if (!isNumeric)
            {
                vm.HasError = true;
                vm.ErrorMessage = "User ID must contain numbers only.";
                return;
            }

            vm.HasError = false;
            vm.ErrorMessage = string.Empty;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as LoginViewModel;
            if (vm == null) return;

            // 공란이면 오류 숨기기
       
                vm.HasError = false;
                vm.ErrorMessage = string.Empty;
            
        }


    }
}