using MOBILE_TEST.Models.Server;
using MOBILE_TEST.Models.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TMXamarinClient;
using TMXamarinClient.ContractTypes;
namespace MOBILE_TEST.Services
{
    internal class TodoService
    {
        /// <summary>
        /// TODO 조회 (TM)
        /// </summary>
        public async Task<List<Todo>> GetTodoList(string userId)
        {
            var result = COMMService.TM.UsrSvc.GetTodoList(userId);

            if (!result.IsSuccess || string.IsNullOrWhiteSpace(result.ResultString))
                throw new Exception(result.ErrorString);

            string json = result.ResultString;

            if ( json.StartsWith("Error"))
                throw new Exception(json);


            if (json.StartsWith("No"))
                return null;

            return JsonHelper.JsonDeserialize<List<Todo>>(json);
        }


        /// <summary>
        /// TODO 삽입
        /// </summary>
        public async Task<string> InsertTodo(TodoModel todo)
        {
            try
            {
                var result = COMMService.TM.UsrSvc.InsertTodo(todo);

                // 결과 문자열이 아예 없는 경우 → 실패
                if (string.IsNullOrWhiteSpace(result.ResultString))
                    throw new Exception(result.ErrorString ?? "Unknown Error");


                if (!result.IsSuccess)
                    throw new Exception(result.ErrorString ?? "Insert failed");

                string newId = result.ResultString;

                if (string.IsNullOrWhiteSpace(newId))
                    throw new Exception("New ID is empty");

                return newId;   // 성공 시 newId 반환
            }
            catch (Exception ex)
            {
                return $"Error - {ex.Message}";
            }
        }

        /// <summary>
        /// TODO 삭제
        /// </summary>
        public async Task<bool> DeleteTodo(String todoId)
        {
            var result = COMMService.TM.UsrSvc.DeleteTodo(todoId);


            if (!result.IsSuccess || string.IsNullOrWhiteSpace(result.ResultString))
                throw new Exception(result.ErrorString);

            string response = result.ResultString;



            if (response.StartsWith("No") || response.StartsWith("Error"))
                throw new Exception(response);



            if (response.StartsWith("OK") || response.StartsWith("Success"))
                return true;

            return false;

        }

        public async Task<bool> UpdateTodoIsDone(string todoId, string isDone)
        {
            var result = COMMService.TM.UsrSvc.UpdateTodoIsDone(todoId, isDone);


            if (!result.IsSuccess || string.IsNullOrWhiteSpace(result.ResultString))
                throw new Exception(result.ErrorString);

            string response = result.ResultString;



            if (response.StartsWith("No") || response.StartsWith("Error"))
                throw new Exception(response);



            if (response.StartsWith("OK") || response.StartsWith("Success"))
                return true;

            return false;

        }

        public async Task<bool> UpdateTodoContent(TodoModel todo)
        {
            var result = COMMService.TM.UsrSvc.UpdateTodoContent(todo);

            if (!result.IsSuccess || string.IsNullOrWhiteSpace(result.ResultString))
                throw new Exception(result.ErrorString);

            string response = result.ResultString;

            if (response.StartsWith("No") || response.StartsWith("Error"))
                throw new Exception(response);

            if (response.StartsWith("OK") || response.StartsWith("Success"))
                return true;

            return false;
        }

    }

}
