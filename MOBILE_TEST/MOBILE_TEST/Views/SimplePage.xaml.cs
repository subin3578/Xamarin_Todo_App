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
    public partial class SimplePage : ContentPage
    {
        public SimplePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        // 누를 때 애니메이션
        private async void OnItemTapped(object sender, EventArgs e)
        {
            if (sender is View view)
            {
                await AnimateTap(view);
            }
        }

        private async Task AnimateTap(View view)
        {
            const uint duration = 50; // 더 빠르게
            var originalScale = view.Scale;

            // 눌릴 때 (요즘 방식: 조금만 작게)
            await view.ScaleTo(0.97, duration, Easing.CubicOut);

            // 즉시 복귀 (더 빠르게)
            await view.ScaleTo(originalScale, 80, Easing.CubicIn);
        }
    



    }


}