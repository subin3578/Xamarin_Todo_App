using MOBILE_TEST.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TMXamarinClient;
using MOBILE_TEST.Models;
using System.Net.Http.Headers;
using MOBILE_TEST.Models.Server;

namespace MOBILE_TEST.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        private string _userId;
        public string UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        // 비밀번호
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        // 로그인 버튼 활성 여부
        public bool CanLogin =>
            !string.IsNullOrWhiteSpace(UserId) &&
            !string.IsNullOrWhiteSpace(Password);


        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login(new USER_QCT05M()));
        }

        public async Task Login(USER_QCT05M user)
        {
            var login = COMMService.TM.UsrSvc.CheckUserLogin(user.QM05IPID, user.QM05PSWD);

            // 문자열 자체
            string result = login.ResultString;

            // 실패 문자열이면(JSON이 아닌 경우)

            if (string.IsNullOrWhiteSpace(result) || !result.TrimStart().StartsWith("{"))
            // 결과값이 성공이라면 JSON {} 형식, 실패라면 String 형식
            {
                if (string.IsNullOrWhiteSpace(result) || !result.TrimStart().StartsWith("{"))
                {
                    HasError = true;
                    ErrorMessage = result;   // 서버 메시지 그대로 사용

                    login.Dispose();
                    return;
                }
            }

            // 여기까지 왔으면 → 로그인성공
            try
            {
                var loginUser = JsonHelper.JsonDeserialize<USER_QCT05M>(result);

                if (loginUser != null)
                {
                    //  앱 세션에 로그인 사용자 저장
                    Session.CurrentUser = loginUser;


                    await Application.Current.MainPage.DisplayAlert(
                        "로그인 성공",
                        $"{loginUser.QM05NAME}님 환영합니다!",
                        "OK"
                    );

                    // 페이지 이동도 가능
                    Application.Current.MainPage = new NavigationPage(new SimplePage());


                    // 추가 로직: 로그인 정보 저장 or 페이지 이동
                    login.Dispose();
                    return;
                }
            }

            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = "로그인 처리 중 오류가 발생했습니다.";
            }

            HasError = false;
            ErrorMessage = string.Empty;
            login.Dispose();
        }


        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set
            {
                _hasError = value;
                OnPropertyChanged();
            }
        }


        public async Task Logout() {

            // TODO: 세션 제거
            Session.CurrentUser = null;

        }

    }
}
