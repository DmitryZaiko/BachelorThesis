using BachelorThesis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BachelorThesis.Services
{
    class AnswersService
    {
        public static async Task<object> DoAnswersGetRequest(string questionId)
        {
            string url = "http://www.rtu-app-api.ml/api/course/read_answers.php";

            url = url + "?id=" + questionId;
     
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();

            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            HttpResponseMessage response = await client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(json);
            JToken errorCode;

            if (o.TryGetValue("ErrorCode", out errorCode))
            {
                var error = JsonConvert.DeserializeObject<ErrorMessage>(o.ToString());
                return error;
            }
            else
            {
                List<Answer> records = new List<Answer>();
                IEnumerable<JToken> questions = o.SelectTokens(@"$.records[*]");

                foreach (JToken question in questions)
                {
                    records.Add(JsonConvert.DeserializeObject<Answer>(question.ToString()));
                }

                return records;
            }
        }

        public static async Task<object> DoAnswersAddRequest(Answer answer)
        {
            string json = JsonConvert.SerializeObject(answer);

            HttpContent content = new StringContent(json);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://www.rtu-app-api.ml/api/course/add_answer.php");
            request.Method = HttpMethod.Post;
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);

            json = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(json);
            JToken errorCode;

            if (o.TryGetValue("ErrorCode", out errorCode))
            {
                var error = JsonConvert.DeserializeObject<ErrorMessage>(o.ToString());
                return error;
            }
            else
            {
                var newAnswer = JsonConvert.DeserializeObject<Answer>(o.ToString());
                return newAnswer;
            }
        }
    }
}
