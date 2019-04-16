namespace BachelorThesis.Models
{
    public class URLHttpParams
    {
        public const string url = "http://rtu-app-api.ml/api/course/read_items.php";
        private string type;
        private string @params;
        public string Type
        {
            get { return type.Remove(0,6); }
            set { type = "?type=" + value; }
        }
        public string Params
        {
            get { return @params; }
            set { @params = "&id=" + value; }
        }

        public string URL
        {
            get { return url + type + @params; }
        }

        public URLHttpParams(string type, string @params = "")
        {
            Type = type;
            Params = @params;
        }

        public URLHttpParams(ItemPageType type, string @params = "")
        {
            Type = type.ToString("G");
            Params = @params;
        }
    }
}
