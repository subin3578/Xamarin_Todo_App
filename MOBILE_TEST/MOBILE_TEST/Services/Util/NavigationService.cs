using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MOBILE_TEST.Services.Util
{
    public class NavigationService : INavigationService
    {
        public Task PushModalAsync(Page page)
        {
            return Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public Task PopModalAsync()
        {
            return Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
