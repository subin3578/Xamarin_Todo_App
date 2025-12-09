using System;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using TMXamarinClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOBILE_TEST.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            UpdateTodoList();
            //GetLoginUser();
            //try
            //{

            //    COMMService.CommResult test = COMMService.TM.UsrSvc.ConnectionCheck();

            //    if (test.IsSuccess && test.ResultString != null)
            //    {
            //        if (test.ResultString.Contains("Error!") == false)
            //        {

            //        }
            //    }
            //    test.Dispose();

            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //}
        }

        private void GetLoginUser()
        {
            COMMService.CommResult login = COMMService.TM.UsrSvc.CheckUserLogin("103680", "3307");

            if (login.IsSuccess && login.ResultString != null)
            {
                USER_QCT05M user = JsonHelper.JsonDeserialize<USER_QCT05M>(login.ResultString);
            }
            login.Dispose();
        }


        private async void UpdateTodoList()
        {
            COMMService.CommResult login = COMMService.TM.UsrSvc.UpdateUserPassword("12321321", "1");

            if (login.IsSuccess && login.ResultString != null)
            {
                 await DisplayAlert("Alart", login.ResultString, "OK");
            }
            login.Dispose();
        }


        public class USER_QCT05M
        {
            public string QM05IPID { get; set; }
            public string QM05NAME { get; set; }
            public string QM05AUTH { get; set; }
            public string QM05SHOP { get; set; }
            public string QM05LINE { get; set; }
            public string QM05MNPR { get; set; }
            public string QM05SBPR { get; set; }
            public string QM05WRKC { get; set; }
            public string QM05COMP { get; set; }
            public string IP_ADDR { get; set; }

        }
    }
}