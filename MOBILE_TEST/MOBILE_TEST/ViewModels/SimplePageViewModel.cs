using MOBILE_TEST;
using MOBILE_TEST.Models;
using MOBILE_TEST.Models.Server;
using MOBILE_TEST.Models.UI;
using MOBILE_TEST.Services;
using MOBILE_TEST.ViewModels;
using MOBILE_TEST.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        private readonly Guid _instanceId = Guid.NewGuid();

        public ICommand TodayCommand => new Command(() =>
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
        });

        public ICommand EventSelectedCommand => new Command(async (item) => await ExecuteEventSelectedCommand(item));
      
        public EventCollection Events { get; set; }

        private readonly TodoService _service;

        public SimplePageViewModel() : base()
        {
             Debug.WriteLine($"[SimplePageVM] 생성자 this {_instanceId}");
            // 화면 관련 command
            HideCalendarCommand = new Command(HideCalendar);
            ShowCalendarCommand = new Command(ShowCalendar);
            ToggleCalendarCommand = new Command(ToggleCalendar);

            OpenAddModalCommand = new Command<DateTime>(OpenAddModal);


            // 로직 관련 command
            IsDoneCommand = new Command<TodoModel>(UpdateTodoIsDone);
            DeleteCommand = new Command<TodoModel>(DeleteTodo);
            AddCommand = new Command<TodoModel>(async (todo) =>
         {
                await AddTodo(todo);
            });
         



            _service = new TodoService();
            Events = new EventCollection();
            _ = LoadTodosFromServer(); // 비동기로 데이터 불러오기



        }


        #region Animation

        private bool _isCalendarVisible = true;

        public bool IsCalendarVisible
        {
            get => _isCalendarVisible;
            set
            {
                SetProperty(ref _isCalendarVisible, value);
                CalendarArrowIcon = value ? "arrowup_24.png" : "arrowdown_24.png";
            }
        }

        private string _calendarArrowIcon = "arrowup_24.png";
        public string CalendarArrowIcon
        {
            get => _calendarArrowIcon;
            set => SetProperty(ref _calendarArrowIcon, value);
        }

        public Command HideCalendarCommand { get; }
        public Command ShowCalendarCommand { get; }
        public Command ToggleCalendarCommand { get; }


        private void HideCalendar()
        {
            IsCalendarVisible = false;
            MessagingCenter.Send(this, "HideCalendar");
            OnPropertyChanged(nameof(CalendarArrowIcon));
        }

        private void ShowCalendar()
        {
            IsCalendarVisible = true;
            MessagingCenter.Send(this, "ShowCalendar");
            OnPropertyChanged(nameof(CalendarArrowIcon));
        }

        private void ToggleCalendar()
        {
            Debug.WriteLine("ToggleCanlendar 들어옴");

            if (IsCalendarVisible)
                HideCalendar();
            else
                ShowCalendar();
        }

        #endregion

        #region modal

        public Command OpenAddModalCommand { get; }

        private async void OpenAddModal(DateTime _selectedDate)
        {
            Debug.WriteLine("OpenAddModal 함수 IN");
                
            try 
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new AddTodoModalPage(_selectedDate,this));
            
            } catch (Exception ex) {
            
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("에러발생", ex.Message, "OK");
            
            }
        }

        #endregion

        #region events
        public Command<TodoModel> IsDoneCommand { get; }
        public Command<TodoModel> DeleteCommand { get; }
        public Command<TodoModel> AddCommand { get;  }

        private void DebugDumpEvents()
        {
            Debug.WriteLine("=========== 📌 EventCollection 전체 상태 출력 ===========");

            if (Events == null || Events.Count == 0)
            {
                Debug.WriteLine("📭 EventCollection 비어 있음!");
                return;
            }

            foreach (var key in Events.Keys.OrderBy(d => d))
            {
                var list = Events[key] as IEnumerable<TodoModel>;
                int count = list?.Count() ?? 0;

                Debug.WriteLine($"📅 날짜: {key:yyyy-MM-dd}  →  {count}개");

                if (count == 0)
                {
                    Debug.WriteLine("   (이 날짜의 Todo 비어 있음)");
                    continue;
                }

                foreach (var item in list)
                {
                    Debug.WriteLine($"   • ID={item.ID}, 내용=\"{item.Content}\", 완료={item.IsDone}, 카테고리={item.Category}");
                }
            }

            Debug.WriteLine("======================================================");
        }
        // -----------------------------
        // ✔ AddTodo
        // -----------------------------
        public async Task<bool> AddTodo(TodoModel todo)
        {
            try
            {
                string result = await _service.InsertTodo(todo);
                if (string.IsNullOrWhiteSpace(result) || result.StartsWith("Error"))

                {
                    await Application.Current.MainPage.DisplayAlert("에러", "서버 삽입 실패", "OK");
                    return false;
                }


                todo.ID = result;   // TodoModel에 Id 문자열로 세팅했다고 가정

                if (DateTime.TryParse(todo.Startdate, out var date))
                {
                    // Delete와 동일한 패턴: 직접 수정
                    if (!Events.ContainsKey(date))
                        Events[date] = new ObservableCollection<TodoModel>();

                    ((ObservableCollection<TodoModel>)Events[date]).Add(todo);
          
                    OnPropertyChanged(nameof(Events)); 
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("에러", "삽입 실패", "OK");
                return false;
            }
        }

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
            var userId = Session.CurrentUser.QM05IPID;
            var list = await _service.GetTodoList(userId);

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
                    Debug.WriteLine("Load하면서 EventCollection Key값 확인");
                    Debug.WriteLine(date);

                }
            }

            // UI 갱신
        }


        // -----------------------------
        // ✔ Delete Todo
        // -----------------------------
         private async void DeleteTodo(TodoModel todo)
        {
            Debug.WriteLine(todo.ID);

            // ============================
            //  📌 1) UI 먼저 변경 (낙관적 UI)
            // ============================
            bool dateParsed = DateTime.TryParse(todo.Startdate, out var date);
            ObservableCollection<TodoModel> backupList = null;  // 복구용

            if (dateParsed && Events.ContainsKey(date))
            {
                var list = (ObservableCollection<TodoModel>)Events[date];

                // 복구를 위해 백업
                backupList = new ObservableCollection<TodoModel>(list);

                // 실제 삭제
                list.Remove(todo);

                // 리스트 비면 그 날짜 자체를 삭제
                if (list.Count == 0)
                    Events.Remove(date);
            }

            // UI 갱신
            OnPropertyChanged(nameof(Events));



            // ============================
            //  📌 2) 서버 통신
            // ============================
            bool success = await _service.DeleteTodo(todo.ID);



            // ============================
            //  📌 3) 서버 실패 → UI 원복
            // ============================
            if (!success)
            {
                await Application.Current.MainPage.DisplayAlert("에러", "서버 삭제 실패", "OK");

                if (dateParsed)
                {
                    // Events 전체를 복구
                    Events[date] = backupList;
                    OnPropertyChanged(nameof(Events));  
                }
            }
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
