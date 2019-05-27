using System;
using System.Collections.Generic;

namespace BachelorThesis.Models
{
    public class Question
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public int? LessonId { get; set; }
        public string UserId { get; set; }
        public List<Answer> Answers { get; set; }

    }
}
