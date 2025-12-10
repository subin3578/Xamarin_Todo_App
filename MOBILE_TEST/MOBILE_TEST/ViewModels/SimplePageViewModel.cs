using MOBILE_TEST;
using MOBILE_TEST.Models;
using MOBILE_TEST.Models.Server;
using MOBILE_TEST.Models.UI;
using MOBILE_TEST.Services;
using MOBILE_TEST.ViewModels;
using MOBILE_TEST.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TMXamarinClient;
using TMXamarinClient.TMService;
using Xamarin.Forms;
using Xamarin.Plugin.Calendar.Models;


namespace MOBILE_TEST.ViewModels
{
    public class SimplePageViewModel : BasePageViewModel
    {

        public Command OpenAddModalCommand { get; }
        public Command<TodoModel> IsDoneCommand { get; }

        public ICommand TodayCommand => new Command(() =>
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
        });

        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
      
        public EventCollection Events { get; }

        private readonly TodoService _service;

        public SimplePageViewModel() : base()
        {

            _service = new TodoService();
            Events = new EventCollection();
            _ = LoadTodosFromServer(); // 비동기로 데이터 불러오기
            OpenAddModalCommand = new Command(OpenAddModal);
            IsDoneCommand = new Command<TodoModel>(UpdateTodoIsDone);


        }

        #region modal

        private async void OpenAddModal()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTodoModalPage());
        }

        #endregion

        #region events


        // -----------------------------
        // ✔ Toggle (체크 상태 반전) - 낙관적 UI
        // -----------------------------
        private void UpdateTodoIsDone(TodoModel todo)
        {
            Debug.WriteLine("Update Todo IsDone");
            Debug.WriteLine(todo.ToString());
            // UI 먼저 변경될 객체 새로 생성
            string newDone = (todo.IsDone == "2") ? "0" : "2";

            string oldDone = todo.IsDone; // 실패 시 복구용

            // 1) UI 즉시 반영
            todo.IsDone = newDone;


            Task.Run(async () =>
            {
                try
                {
    
                    bool success = await _service.UpdateTodoIsDone(todo.ID, newDone);
               
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"체크 서버 요청 중 에러 발생 - {ex.Message}");

                    todo.IsDone = oldDone; // 원복

                    await Application.Current.MainPage.DisplayAlert("에러", "서버 오류 발생", "OK");
                   
                }
            });

        }

   
        
        // -----------------------------
        // ✔ Load Todo
        // -----------------------------
        private async Task LoadTodosFromServer()
        {
            //await Task.Delay(1500); // 서버 통신 대기 연출

            var list = await _service.GetTodoList("104286");

            if (list == null || list.Count == 0)
                return;

            // 2) Todo → TodoModel로 변환
            List<TodoModel> todoList = list.Select(s =>
            {
                Debug.WriteLine("ID 있는지 없는지 확인");
                Debug.WriteLine(s.ToString());
                var item = new TodoModel
                {
                    ID = s.ID,
                    Content = s.TODO_CONTENT,
                    Description = s.DESCRIPTION,
                    Startdate = s.START_DATE,
                    Category = s.CATEGORY,
                    Priority = s.PRIORITY,
                    IsDone = s.ISDONE.ToString(),
                    CreatedAt = s.CREATEDAT,
                    WriterId = s.WRITER_ID
                };

                item.ApplyStyle();   // ★ 스타일 적용!
                Debug.WriteLine("TodoModel로 변경 후 ID 있는지 확인");
                Debug.WriteLine(s.ToString());
                return item;

            }).ToList();


            // =============================
            //   📌 EventCollection 구조로 변환
            // =============================
            foreach (var todo in todoList)
            {
                if (DateTime.TryParse(todo.Startdate, out var date))
                {
                    if (!Events.ContainsKey(date))
                        Events[date] = new ObservableCollection<TodoModel>();

                    ((ObservableCollection<TodoModel>)Events[date]).Add(todo);
                }
            }

            // UI 갱신
            OnPropertyChanged(nameof(Events));
        }

        #endregion



        #region information
        /// <summary>
        /// 달력 바인딩 정보들
        /// </summary>

        private int _month = DateTime.Today.Month;

        public int Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        private int _year = DateTime.Today.Year;

        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        private DateTime? _selectedDate = DateTime.Today;

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set => SetProperty(ref _selectedDate, value);
        }

        private DateTime _minimumDate = new DateTime(2019, 4, 29);

        public DateTime MinimumDate
        {
            get => _minimumDate;
            set => SetProperty(ref _minimumDate, value);
        }

        private DateTime _maximumDate = DateTime.Today.AddMonths(5);

        public DateTime MaximumDate
        {
            get => _maximumDate;
            set => SetProperty(ref _maximumDate, value);
        }

        private async Task ExecuteEventSelectedCommand(object item)
        {
            if (item is TodoModel TodoModel)
            {
                await App.Current.MainPage.DisplayAlert(TodoModel.Content, TodoModel.Description, "Ok");
            }
        }
        #endregion
    }
}
