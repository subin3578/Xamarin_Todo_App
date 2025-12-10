using System;
using System.Globalization;
using MOBILE_TEST.Models.UI;
using Xamarin.Forms;
using MOBILE_TEST.Helpers;

namespace MOBILE_TEST.Helpers
{
    public static class TodoMapper
    {
        public static void ApplyToUI(
            TodoModel todo,
            Entry contentEditor,
            Label startLabel, DatePicker startPicker,
            Label categoryLabel,
            Editor descriptionEditor,
            Frame chipNot, Label lblNot,
            Frame chipProg, Label lblProg,
            Frame chipComp, Label lblComp,
            Frame priHigh, Frame priMed, Frame priLow)
        {
            
            // 내용
            if (contentEditor != null)
                contentEditor.Text = todo.Content;

            // 시작일
            if (DateTime.TryParse(todo.Startdate, out var start))
            {
                startPicker.Date = start;
                startLabel.Text = start.ToString("yy년 M월 d일 (ddd)", new CultureInfo("ko-KR"));
            }

            // 카테고리
            if (categoryLabel != null)
                categoryLabel.Text = string.IsNullOrWhiteSpace(todo.Category)
                    ? "선택하세요"
                    : todo.Category;

            // 설명
            if (descriptionEditor != null)
                descriptionEditor.Text = todo.Description;

            // 상태: 0/1/2 → 문자열로 변환
            string status = "NotStarted"; // 기본값

            switch (todo.IsDone)
            {
                case "0":
                    status = "NotStarted";
                    break;
                case "1":
                    status = "InProgress";
                    break;
                case "2":
                    status = "Completed";
                    break;
            }

            UIHelpers.SetStatusChip(status,
                chipNot, lblNot,
                chipProg, lblProg,
                chipComp, lblComp);

            // 중요도
            UIHelpers.SetPriority(todo.Priority,
                priHigh, priMed, priLow);
        }
    }
}
