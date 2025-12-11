using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace MOBILE_TEST.Models.UI
{
    public class TodoModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _id;
        public string ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        private string _writerId;
        public string WriterId
        {
            get => _writerId;
            set { _writerId = value; OnPropertyChanged(); }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(); }
        }

        private string _isDone;
        public string IsDone
        {
            get => _isDone;
            set { _isDone = value; OnPropertyChanged(); ApplyStyle(); }
        }

        public string CreatedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Startdate { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }

        // ---------- UI 바인딩 속성 ----------
        private string _icon;
        public string Icon
        {
            get => _icon;
            set { _icon = value; OnPropertyChanged(); }
        }

        private string _iconColor;
        public string IconColor
        {
            get => _iconColor;
            set { _iconColor = value; OnPropertyChanged(); }
        }

        private string _textColor;
        public string TextColor
        {
            get => _textColor;
            set { _textColor = value; OnPropertyChanged(); }
        }

        private TextDecorations _textDecoration;
        public TextDecorations TextDecoration
        {
            get => _textDecoration;
            set { _textDecoration = value; OnPropertyChanged(); }
        }

        // ---------- UI 스타일 적용 ----------
        public void ApplyStyle()
        {
        if (IsDone == "2")
            {
                Icon = "✅";
                IconColor = "blue";
                TextColor = "#9CA3AF";
                TextDecoration = TextDecorations.Strikethrough;
            }
            else
            {
                Icon = "⬜";
                IconColor = "red";
                TextColor = "#374151";
                TextDecoration = TextDecorations.None;
            }
        }

        public string PriorityStars =>
            Priority == "3" ? "⭐⭐⭐" :
            Priority == "2" ? "⭐⭐" :
            Priority == "1" ? "⭐" : "";

        public override string ToString()
        {
            return $"[TodoModel] ID='{ID}', Content='{Content}', IsDone={IsDone}, CreatedAt='{CreatedAt}', Description='{Description}', Startdate={Startdate}, Priority={Priority}, Category={Category}";
        }
    }
}
