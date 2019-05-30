using BachelorThesis.Helpers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BachelorThesis.Services
{
    static class CompilerService
    {
        private static readonly HttpClient client = new HttpClient();
        private static HttpRequestMessage request;

        public static async Task DoGetUIDRequest ()
        {
            string url = "https://www.tutorialspoint.com/compile_csharp_online.php";
            string formId = "ff";
            string inputName = "uid";

            request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            HttpResponseMessage response = await client.SendAsync(request);
            var html = await response.Content.ReadAsStringAsync();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var form = doc.GetElementbyId(formId);
            var input = form.GetElementsByName("input");
            var targetNode = input.Where(node => node.GetAttributeValue("name", null) == inputName).First();
            Settings.CompilerUid = targetNode.GetAttributeValue("value", null);
        }

        public static async Task<object> DoPostCodeRequest(string code)
        {
            string url = "https://tpcg.tutorialspoint.com/tpcg.php";
            string lang = "csharp";
            string device = "";
            string stdinput = "";
            string ext = "cs";
            string compile = "mcs *.cs -out:main.exe";
            string execute = "mono main.exe";
            string mainfile = "main.cs";
            string uid = Settings.CompilerUid;

            request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Post;

            request.Headers.Add("Accept", "*/*");
            //request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            request.Headers.Add("Origin", "https://www.tutorialspoint.com");
            request.Headers.Add("Referer", "https://www.tutorialspoint.com/compile_csharp_online.php");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");

            var body = "lang=" + lang + "&";
            body += "device=" + device + "&";
            body += "code=" + HttpUtility.UrlEncode(code) + "&";
            body += "stdinput=" + stdinput + "&";
            body += "ext=" + ext + "&";
            body += "compile=" + compile + "&";
            body += "execute=" + execute + "&";
            body += "mainfile=" + mainfile + "&";
            body += "uid=" + uid;

            //body = HttpUtility.UrlEncode(body);

            request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = await client.SendAsync(request);
            var html = await response.Content.ReadAsStringAsync();
            html = html.Substring(139);
            return html;
            
        }


        public static IEnumerable<HtmlNode> GetElementsByName(this HtmlNode parent, string name)
        {
            return parent.Descendants().Where(node => node.Name == name);
        }
    }
}
