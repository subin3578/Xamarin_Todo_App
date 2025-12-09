using MOBILE_TEST.Models;
using MOBILE_TEST.Models.Server;
using MOBILE_TEST.Models.UI;
using MOBILE_TEST.Services;
using MOBILE_TEST.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TMXamarinClient;
using Xamarin.Forms;
using XBEAN.Device.WrenchTool;


//ViewModels 에서  Service DI해서 사용
namespace MOBILE_TEST.ViewModels
{
    public class TodoViewModel : BaseViewModel
    {
        // 속성
        private readonly TodoService _service;
        public ObservableCollection<TodoModel> Todos { get; set; }

        public Command LoadTodoCommand { get; }
        public Command<TodoModel> AddTodoCommand { get; }
        public Command<TodoModel> DeleteCommand { get; }
        public Command<TodoModel> ToggleDoneCommand { get; }
        public Command<TodoModel> UpdateTodoCommand { get; }

        public Command OpenAddModalCommand { get; }
        public Command<TodoModel> OpenUpdateModalCommand { get; }




        public string currentUser;
        public string TodayText => DateTime.Now.ToString("dddd, dd MMM");

        private string _taskCountText;
        public string TaskCountText
        {
            get => _taskCountText;
            set => SetProperty(ref _taskCountText, value);
        }


        // 생성자
        public TodoViewModel()
        {
            _service = new TodoService();

            Todos = new ObservableCollection<TodoModel>();

            // CRUD
            LoadTodoCommand = new Command(async () => await LoadTodos());
            AddTodoCommand = new Command<TodoModel>(AddTodo);
            DeleteCommand = new Command<TodoModel>(DeleteTodo);
            UpdateTodoCommand = new Command<TodoModel>(UpdateTodo);
            ToggleDoneCommand = new Command<TodoModel>(UpdateTodoIsDone);

            // 모달 열기
            OpenAddModalCommand = new Command(OpenAddModal);
            OpenUpdateModalCommand = new Command<TodoModel>(OpenUpdateModal);
          

        }

        private async void OpenAddModal()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AddTodoModalPage(this));
        }

        private async void OpenUpdateModal(TodoModel todo)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new UpdateTodoModalPage(this, todo));
        }

        // -----------------------------
        // ✔ LoadTodos
        // -----------------------------
        private async Task LoadTodos()
        {
            currentUser = Session.CurrentUser.QM05IPID;
            var list = await _service.GetTodoList(currentUser);

            Todos.Clear();

            if (list == null || list.Count == 0)
            {
                TaskCountText = "0 tasks";
                return;
            }

            var temp = new List<TodoModel>();

            foreach (var s in list)
            {
                var item = new TodoModel
                {
                    ID = s.ID,
                    Content = s.TODO_CONTENT,
                    IsDone = s.ISDONE.ToString(),
                    CreatedAt = FormatTime(s.CREATEDAT),
                    Description = s.DESCRIPTION,
                    Category = s.CATEGORY,
                    Startdate = s.START_DATE,
                    Priority =s.PRIORITY

                };

                item.ApplyStyle();
                temp.Add(item);
            }

            foreach (var item in temp.OrderBy(t => t.IsDone))
            {
                Todos.Add(item);
            }

            Debug.WriteLine(string.Join("\n", Todos));
            TaskCountText = $"{Todos.Count} tasks";
        }


        // -----------------------------
        // ✔ Add Todo
        // -----------------------------
        private async void AddTodo(TodoModel todo)
        {


            currentUser = Session.CurrentUser.QM05IPID;
            todo.WriterId = currentUser;

            bool success = await _service.InsertTodo(todo);

            if (!success)
            {
                await Application.Current.MainPage.DisplayAlert("에러", "서버 추가 실패", "OK");
                return;
            }

            await LoadTodos();

            TaskCountText = $"{Todos.Count} tasks";

        }


        // -----------------------------
        // ✔ Delete Todo
        // -----------------------------
        private async void DeleteTodo(TodoModel todo)
        {
            // todo Id 못가져오고 잇음
            Debug.WriteLine(todo.ID);
            bool success = await _service.DeleteTodo(todo.ID);

            if (!success)
            {
                await Application.Current.MainPage.DisplayAlert("에러", "서버 추가 실패", "OK");
                return;
            }

            Todos.Remove(todo);
            TaskCountText = $"{Todos.Count} tasks";
        }



        // -----------------------------
        // ✔ Update Todo
        // -----------------------------
        private async void UpdateTodo(TodoModel todo)
        {
            if (string.IsNullOrWhiteSpace(todo.Content))
                return;

            bool success = await _service.UpdateTodoContent(todo);

            if (!success)
            {
                await Application.Current.MainPage.DisplayAlert("에러", "수정 실패", "OK");
                return;
            }


            var updated = new TodoModel
            {
                ID = todo.ID,
                Content = todo.Content,
                IsDone = todo.IsDone,
                CreatedAt = todo.CreatedAt,
                Category = todo.Category,
                Description = todo.Description,
                Priority = todo.Priority,
                WriterId = todo.WriterId,
                Startdate = todo.Startdate
            };

            updated.ApplyStyle();

            // ID로 찾아서 교체
            var old = Todos.FirstOrDefault(t => t.ID == todo.ID);
            if (old != null)
            {
                int idx = Todos.IndexOf(old);
                Todos[idx] = updated;
            }

        }

        // -----------------------------
        // ✔ Toggle (체크 상태 반전) - 낙관적 UI
        // -----------------------------
        private void UpdateTodoIsDone(TodoModel todo)
        {
            // UI 먼저 변경될 객체 새로 생성
            string newDone = (todo.IsDone == "2") ? "0" : "2";


            var updated = new TodoModel
            {
                ID = todo.ID,
                Content = todo.Content,
                IsDone = newDone,
                CreatedAt = todo.CreatedAt,
                Startdate = todo.Startdate,
                Category = todo.Category,
                Description = todo.Description,
                Priority = todo.Priority,
                WriterId = todo.WriterId

            };
            updated.ApplyStyle();

            int idx = Todos.IndexOf(todo);
            Todos[idx] = updated;   // ← 여기서 UI가 강제로 Refresh 됨

            // 서버는 백그라운드에서 처리
            Task.Run(async () =>
            {
                bool success = await _service.UpdateTodoIsDone(todo.ID, newDone);
                if (!success)
                {
                    // 실패 시 원래 객체 복구(여기도 교체 방식)
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Todos[idx] = todo;
                        Application.Current.MainPage.DisplayAlert("에러", "서버 반영 실패", "OK");
                    });
                }
            });
        }



        private string FormatTime(string raw)
        {
            if (DateTime.TryParse(raw, out var dt))
                return dt.ToString("h:mm tt");
            return "";
        }
    }


    

}
