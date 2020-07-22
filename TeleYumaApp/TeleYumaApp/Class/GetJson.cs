using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace TeleYumaApp.Class
{
    public static class GetJson
    {
        public static AuthInfo AuthInfo = new AuthInfo();

        const string BaseUrl = "http://mybilling.teleyuma.com/rest/";

        private static async void Conectar()
        {
            var admin = new AuthInfo { login = "app-45-yuma", password = "appyuma8708@" };
            var AuthInfo = JsonConvert.SerializeObject(admin);

            using (HttpClient client = new HttpClient())
            {
               // var url1 = @"http://mybilling.teleyuma.com/rest/Session/login/{""login"":""app-45-yuma"",""password"":""appyuma8708@""}";
                //var url = "http://mybilling.teleyuma.com/rest/Session/login/";
                var url3 = "http://mybilling.teleyuma.com/rest/Currency/get_account_list/%7B%22session_id%22:%2286e7f7f68a0b4f01381268b6b317c6cc%22%7D";
                //% 7B % 22login % 22:% 22app - 45 - yuma % 22,% 22password % 22:% 22appyuma8708@% 22 % 7D
                //{ "session_id":"86e7f7f68a0b4f01381268b6b317c6cc"}
                //  client.BaseAddress = new Uri("");
                // client.DefaultRequestHeaders.Add();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetStringAsync(url3);
                //MessageBox.Show(response.ToString());
                //txt_md5.Text = response.ToString();
            }

        }
          

    }
}
