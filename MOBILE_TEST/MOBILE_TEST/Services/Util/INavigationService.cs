using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MOBILE_TEST.Services.Util
{
    public interface INavigationService
    {
        Task PushModalAsync(Page page);
        Task PopModalAsync();
    }
}