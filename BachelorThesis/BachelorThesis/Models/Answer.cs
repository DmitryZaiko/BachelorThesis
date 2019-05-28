using System;

namespace BachelorThesis.Models
{
    public class Answer
    {
        public string Id { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId { get; set; }
        public string QuestionId { get; set; }
        public string UserName { get; set; }

    }
}
