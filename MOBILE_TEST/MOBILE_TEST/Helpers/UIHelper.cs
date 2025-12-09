using Xamarin.Forms;

namespace MOBILE_TEST.Helpers
{
    public static class UIHelpers
    {
        /* ------------------------------
         *  상태칩 스타일 적용 (0/1/2)
         * ------------------------------ */
        public static void SetStatusChip(string status,
            Frame notStarted, Label notLabel,
            Frame inProgress, Label inLabel,
            Frame completed, Label comLabel)
        {
            ResetStatusChip(notStarted, notLabel, "#FEE2E2");
            ResetStatusChip(inProgress, inLabel, "#DBEAFE");
            ResetStatusChip(completed, comLabel, "#D1FAE5");

            switch (status)
            {
                case "NotStarted":
                    SelectStatusChip(notStarted, notLabel, "#FCA5A5");
                    break;

                case "InProgress":
                    SelectStatusChip(inProgress, inLabel, "#BFDBFE");
                    break;

                case "Completed":
                    SelectStatusChip(completed, comLabel, "#A7F3D0");
                    break;
            }
        }

        private static void ResetStatusChip(Frame frame, Label label, string color)
        {
            frame.BackgroundColor = Color.FromHex(color);
            label.FontAttributes = FontAttributes.None;
        }

        private static void SelectStatusChip(Frame frame, Label label, string color)
        {
            frame.BackgroundColor = Color.FromHex(color);
            label.FontAttributes = FontAttributes.Bold;
        }


        /* ------------------------------
         *  중요도 스타일 1/2/3
         * ------------------------------ */
        public static void SetPriority(string level,
            Frame high, Frame medium, Frame low)
        {
            ResetPriority(high);
            ResetPriority(medium);
            ResetPriority(low);

            switch (level)
            {
                case "3": // 높음
                    ApplyPriority(high);
                    break;

                case "2": // 중간
                    ApplyPriority(medium);
                    break;

                case "1": // 낮음
                    ApplyPriority(low);
                    break;
            }
        }

        private static void ResetPriority(Frame frame)
        {
            frame.BackgroundColor = Color.White;
            frame.BorderColor = Color.FromHex("#E1E3E8");

            if (frame.Content is Label lbl)
            {
                lbl.TextColor = Color.FromHex("#1F1F22");
                lbl.FontAttributes = FontAttributes.None;
            }
        }

        private static void ApplyPriority(Frame frame)
        {
            frame.BackgroundColor = Color.FromHex("#356BDE");
            frame.BorderColor = Color.Transparent;

            if (frame.Content is Label lbl)
            {
                lbl.TextColor = Color.White;
                lbl.FontAttributes = FontAttributes.Bold;
            }
        }
    }
}
