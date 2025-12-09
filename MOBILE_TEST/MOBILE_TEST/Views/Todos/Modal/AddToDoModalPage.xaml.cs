using MOBILE_TEST.Helpers;
using MOBILE_TEST.Models.Server;
using MOBILE_TEST.Models.UI;
using MOBILE_TEST.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;


namespace MOBILE_TEST.Views
{
    public partial class AddTodoModalPage : ContentPage
    {
        private readonly TodoViewModel _vm;

        
        private string _status = "NotStarted";
        private string _priority = "1";

        public AddTodoModalPage(TodoViewModel viewModel)
        {
            InitializeComponent();
            _vm = viewModel;

            // 초기 상태
            UIHelpers.SetStatusChip("NotStarted",
                chipNotStarted, labelNotStarted,
                chipInProgress, labelInProgress,
                chipCompleted, labelCompleted);

            UIHelpers.SetPriority("3",
                priorityHigh, priorityMedium, priorityLow);

            datePickerStart.Date = DateTime.Today;
            labelStartDate.Text = DateTime.Today.ToString("yy년 M월 dd일 (ddd)", new CultureInfo("ko-KR"));


        }

        // -----------------------------
        // 등록 버튼
        // -----------------------------
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entryTask.Text))
            {
                await DisplayAlert("알림", "내용을 입력해주세요.", "확인");
                return;
            }

            var btn = (Button)sender;
            btn.IsEnabled = false;

            // UI → 모델로 변환
            TodoModel todo = new TodoModel
            {
           
                Content = entryTask.Text,
                Description = editorDescription.Text,
                Startdate = datePickerStart.Date.ToString("yyyy/MM/dd"),
                Category = labelCategory.Text,
                Priority = _priority,
                IsDone = _status == "NotStarted" ? "0" :
                         _status == "InProgress" ? "1" : "2"
            };

            try
            {

                _vm.AddTodoCommand.Execute(todo);

                await Navigation.PopModalAsync();
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Update 실패: " + ex.Message);
                await DisplayAlert("오류", "업데이트 중 문제가 발생했습니다.", "확인");
            }

            finally {
                btn.IsEnabled = true;
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        // -----------------------------
        // 날짜 선택
        // -----------------------------
        private void StartDate_Tapped(object sender, EventArgs e)
        {
            datePickerStart.Focus();
        }

        private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            labelStartDate.Text = e.NewDate.ToString("yy년 M월 dd일 (ddd)", new CultureInfo("ko-KR"));
        }
        


        // -----------------------------
        // 상태칩 선택 (시작전/진행중/완료)
        // -----------------------------
        private void Tap_NotStarted(object sender, EventArgs e)
        {
            UIHelpers.SetStatusChip("NotStarted",
                chipNotStarted, labelNotStarted,
                chipInProgress, labelInProgress,
                chipCompleted, labelCompleted);

            _status = "NotStarted";
        }

        private void Tap_InProgress(object sender, EventArgs e)
        {
            UIHelpers.SetStatusChip("InProgress",
                chipNotStarted, labelNotStarted,
                chipInProgress, labelInProgress,
                chipCompleted, labelCompleted);

            _status = "InProgress";
        }

        private void Tap_Completed(object sender, EventArgs e)
        {
            UIHelpers.SetStatusChip("Completed",
                chipNotStarted, labelNotStarted,
                chipInProgress, labelInProgress,
                chipCompleted, labelCompleted);

            _status = "Completed";
        }


        // -----------------------------
        // 카테고리 Bottom Sheet
        // -----------------------------
        private async void Category_Tapped(object sender, EventArgs e)
        {
            Overlay.IsVisible = true;
            await BottomSheet.TranslateTo(0, 0, 200, Easing.CubicOut);
        }

        private async void SelectCategory(object sender, EventArgs e)
        {
            if (sender is Label lbl)
                labelCategory.Text = lbl.Text;

            await BottomSheet.TranslateTo(0, 400, 200, Easing.CubicIn);
            Overlay.IsVisible = false;
        }

        private async void Overlay_Tapped(object sender, EventArgs e)
        {
            await BottomSheet.TranslateTo(0, 400, 200, Easing.CubicIn);
            Overlay.IsVisible = false;
        }


        // -----------------------------
        // 중요도 선택
        // -----------------------------
        private void Tap_PriorityHigh(object sender, EventArgs e)
        {
            UIHelpers.SetPriority("3",
                priorityHigh, priorityMedium, priorityLow);

            _priority = "3";
        }

        private void Tap_PriorityMedium(object sender, EventArgs e)
        {
            UIHelpers.SetPriority("2",
                priorityHigh, priorityMedium, priorityLow);

            _priority = "2";
        }

        private void Tap_PriorityLow(object sender, EventArgs e)
        {
            UIHelpers.SetPriority("1",
                priorityHigh, priorityMedium, priorityLow);

            _priority = "1";
        }
    }
}
