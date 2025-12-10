using System;
using System.Collections.Generic;
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
        double _startY;

        private async void OnSwipe(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    _startY = e.TotalY;
                    break;

                case GestureStatus.Completed:
                    double deltaY = e.TotalY - _startY;

                    // 👆 위로 스와이프: 캘린더 숨기기
                    if (deltaY < -80 && CalendarView.IsVisible)
                    {
                        await CalendarView.FadeTo(0, 150);
                        CalendarView.IsVisible = false;
                    }

                    // 👇 아래로 스와이프: 캘린더 보이기
                    else if (deltaY > 80 && !CalendarView.IsVisible)
                    {
                        CalendarView.IsVisible = true;
                        await CalendarView.FadeTo(1, 150);
                    }

                    break;
            }
        }
    }
}