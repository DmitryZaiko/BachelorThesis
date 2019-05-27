using BachelorThesis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BachelorThesis.Services
{
    class QuestionsService
    {
        public static async Task<object> DoQuestionsGetRequest(int? lessonId = null)
        {
                string url = "http://www.rtu-app-api.ml/api/course/read_questions.php";
                if (lessonId != null)
                {
                    url = url + "?id=" + lessonId;
                }
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
                    List<Question> records = new List<Question>();
                    IEnumerable<JToken> questions = o.SelectTokens(@"$.records[*]");

                    foreach( JToken question in questions)
                    {
                        records.Add(JsonConvert.DeserializeObject<Question>(question.ToString()));
                    }

                    return records;
                }
        }

        public static async Task<object> DoQuestionsAddRequest(Question question)
        {
            string json = JsonConvert.SerializeObject(question);

            HttpContent content = new StringContent(json);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://www.rtu-app-api.ml/api/course/add_question.php");
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
                var newQuestion = JsonConvert.DeserializeObject<Question>(o.ToString());
                Debug.WriteLine("HEADER: " + newQuestion.Header);               
                return newQuestion;
            }
        }
    }
}

