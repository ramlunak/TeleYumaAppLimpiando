using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TeleYumaApp.Class
{
    [DataContract(Namespace = "http://www.TeleYuma.com")]
    [Serializable]
    public class ELogin
    {
        [DataMember]
        public int IdLogin { get; set; }
        [DataMember]
        public int i_customer { get; set; }
        [DataMember]
        public string user { get; set; }
        [DataMember]
        public int loged { get; set; }
        [DataMember]
        public string Token { get; set; }

        public async Task<bool> Ingresar()
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://192.168.101.1/service/Service1.svc/");// wifi
                //client.BaseAddress = new Uri("http://192.168.42.42/service/Service1.svc/");// usb
                client.BaseAddress = new Uri("http://169.254.80.80/service/Service1.svc/");// emulador

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsync("Login/Ingresar", this.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    if (Convert.ToInt32(Result) > 0)
                        return true;
                    else
                        return false;
                }
                catch 
                {
                    return false;
                }

            }

        }

    }
}
