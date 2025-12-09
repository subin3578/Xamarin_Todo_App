using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MOBILE_TEST.Models.UI
{
    public class TodoModel
    {
        public string ID { get; set; }
        public string WriterId { get; set; }
        public string Content { get; set; }
        public string IsDone { get; set; }
        public string CreatedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Startdate { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }

        // ---------- UI 전용 속성 ----------
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string TextColor { get; set; }
        public TextDecorations TextDecoration { get; set; }

        // ---------- UI 스타일 적용 ----------
        public void ApplyStyle()
        {
            if (IsDone == "1")
            { // 진행중
       

                Icon = "🔄";
                IconColor = "green";
                TextColor = "#374151"; // 회색

            }
            else if (IsDone == "2")
            { // 완료
          
                Icon = "✅";            // 빈 원
                IconColor = "blue"; // 
                TextColor = "#9CA3AF"; // 진한 회색
                TextDecoration = TextDecorations.Strikethrough;

            }
            else
            {
                Icon = "⬜";            // 체크된 표시
                IconColor = "red"; // 파란색
                TextColor = "#374151"; // 진한 회색
                TextDecoration = TextDecorations.None;
                // 시작전
     
            }
        }

        public string PriorityStars
        {
            get
            {
                if (Priority == "3")
                    return "⭐⭐⭐";
                if (Priority == "2")
                    return "⭐⭐";
                if (Priority == "1")
                    return "⭐";

                return "";
            }
        }
        public override string ToString()
        {
            return $"[TodoModel] Content='{Content}', IsDone={IsDone}, CreatedAt='{CreatedAt}',Description='{Description},Startdate={Startdate}, Priority={Priority}, Category={Category}";
        }
    }
}
