namespace BachelorThesis.Models
{
    public class Content : Item
    {
        public string Text { get; set; }
        public int LessonId { get; set; }
        public int Count { get; set; }
    }
}
