using BachelorThesis.Helpers;
using BachelorThesis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BachelorThesis.Services
{
    class AuthorizationService
    {

        public async Task<object> DoLoginRequest(string email, string password)
        {
            var userData = new { Email = email, Password = password};
            string json = JsonConvert.SerializeObject(userData);

            HttpContent content = new StringContent(json);

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://www.rtu-app-api.ml/api/course/login.php");
            request.Method = HttpMethod.Post;
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);
       
            json = await response.Content.ReadAsStringAsync();
            JObject o = JObject.Parse(json);
            JToken errorCode;

            if(o.TryGetValue("ErrorCode", out errorCode))
            {
                var error = JsonConvert.DeserializeObject<LoginError>(o.ToString());
                return error;
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(o.ToString());
                Settings.UserSettings = o.ToString();
                return user;
            }
        }
    }
}
