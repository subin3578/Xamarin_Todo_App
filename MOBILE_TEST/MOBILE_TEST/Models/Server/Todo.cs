using System;

namespace MOBILE_TEST.Models.Server
{
    public class Todo
    {
        public string ID { get; set; }
        public string WRITER_ID { get; set; } 
        public string TODO_CONTENT { get; set; }
        public string ISDONE { get; set; }
        public string CREATEDAT { get; set; }


        // 속성 추가
        public string DESCRIPTION { get; set; }
        public string START_DATE { get; set; }
        public string PRIORITY { get; set; }
        public string CATEGORY { get; set; }

        //public string DUE_DATE { get; set; }     
        public override string ToString()
        {
            return $"[Todo] " +
                   $"ID={ID}, " +
                   $"WriterId={WRITER_ID}, " +
                   $"Content={TODO_CONTENT}, " +
                   $"IsDone={ISDONE}, " +
                   $"CreatedAt={CREATEDAT}, " +
                   $"Description={DESCRIPTION}, " +
                   $"StartDate={START_DATE}, " +
                   $"Priority={PRIORITY}, " +
                   $"Category={CATEGORY}";
        }

    }


}

