using MOBILE_TEST.Helpers;
using MOBILE_TEST.Models.UI;
using MOBILE_TEST.ViewModels;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MOBILE_TEST.Views
{
    public partial class UpdateTodoModalPage : ContentPage
    {
        private readonly TodoViewModel _vm;
        private readonly TodoModel _todo;

        public UpdateTodoModalPage(TodoViewModel vm, TodoModel todo)
        {
            InitializeComponent();
            _vm = vm;
            _todo = todo;

            // 공통 Mapper를 이용하여 UI 초기화
            TodoMapper.ApplyToUI(
                   todo,
                   ContentEditor,
                   labelStartDate, datePickerStart,
                   labelCategory,
                   editorDescription,
                   chipNotStarted, labelNotStarted,
                   chipInProgress, labelInProgress,
                   chipCompleted, labelCompleted,
                   priorityHigh, priorityMedium, priorityLow
               );
            _priority = todo.Priority; // 우선순위 저장


        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            btn.IsEnabled = false;   

             _todo.Content = ContentEditor.Text;
            _todo.Description = editorDescription.Text;
            _todo.Startdate = datePickerStart.Date.ToString("yyyy/MM/dd");
            _todo.Category = labelCategory.Text;
            _todo.Priority = _priority;
            _todo.Category = labelCategory.Text;

            Debug.WriteLine("Update - onSaveClicked()");
            Debug.WriteLine(_todo.ToString());

            try
            {
                _vm.UpdateTodoCommand.Execute(_todo);
                await Navigation.PopModalAsync();

            }
            catch (Exception ex)
            {
                // 실패 시 화면 유지 + 로그 확인
                Debug.WriteLine("Update 실패: " + ex.Message);
                await DisplayAlert("오류", "업데이트 중 문제가 발생했습니다.", "확인");
            }
            finally
            {
                btn.IsEnabled = true; // 버튼 다시 활성화
            }



        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        /// <summary>
        /// 시작 / 만료 기한 날짜
        /// </summary>
        private void StartDate_Tapped(object sender, EventArgs e)
        {
            datePickerStart.Focus();
        }

        private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            labelStartDate.Text = e.NewDate.ToString("yy년 M월 dd일 (ddd)", new CultureInfo("ko-KR"));
        }


        /// <summary>
        /// 상태 선택 ( 시작전 | 진행중 | 완료 )
        /// </summary>
       

        // --- Tap Events ---
        private void Tap_NotStarted(object sender, EventArgs e)
        {
            UIHelpers.SetStatusChip("NotStarted",
                      chipNotStarted, labelNotStarted,
                      chipInProgress, labelInProgress,
                      chipCompleted, labelCompleted);

            _todo.IsDone = "0";
        }

        private void Tap_InProgress(object sender, EventArgs e)
        {
            UIHelpers.SetStatusChip("InProgress",
                     chipNotStarted, labelNotStarted,
                     chipInProgress, labelInProgress,
                     chipCompleted, labelCompleted);

            _todo.IsDone = "1";
        }

        private void Tap_Completed(object sender, EventArgs e)
        {
            UIHelpers.SetStatusChip("Completed",
                      chipNotStarted, labelNotStarted,
                      chipInProgress, labelInProgress,
                      chipCompleted, labelCompleted);

            _todo.IsDone = "2";
        }


        /// <summary>
        /// 카테고리 선택
        /// </summary>

        private async void Category_Tapped(object sender, EventArgs e)
        {
            Overlay.IsVisible = true;

            // 아래에서 위로 부드럽게 올라오는 애니메이션
            await BottomSheet.TranslateTo(0, 0, 200, Easing.CubicOut);
        }

        private async void SelectCategory(object sender, EventArgs e)
        {
            var label = sender as Label;
            if (label == null) return;

            labelCategory.Text = label.Text;

            // 부드럽게 아래로 내리기
            await BottomSheet.TranslateTo(0, 400, 200, Easing.CubicIn);
            Overlay.IsVisible = false;
        }

        public async void Overlay_Tapped(object sender, EventArgs e)
        {
            await BottomSheet.TranslateTo(0, 400, 200, Easing.CubicIn);

            Overlay.IsVisible = false;
        }


        /// <summary>
        /// 중요도 선택
        /// </summary>

        string _priority = "1";



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
