using MOBILE_TEST.Services;
using MOBILE_TEST.ViewModels;
using MOBILE_TEST.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOBILE_TEST
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();

            // COMMService.TM.Init() - App()에서 필수로 실행해줘야함
            COMMService.TM.Init();

            TodoViewModel vm = new TodoViewModel();

            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.White,
            };

        }

        protected override void OnStart()
        {
        
        }

        protected override void OnSleep()
        {
    
        }

        protected override void OnResume()
        {
     
        }
    }
}
