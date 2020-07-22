using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using TeleYumaApp.Class;
using System.Security.Cryptography;
using System.ComponentModel;

using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace TeleYumaApp.Class
{
    public class nautaUser
    {
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public Int64 mobile { get; set; }

    }

    public class nautaInfo
    {
        public string account_number { get; set; }
        public int product_id { get; set; }
        public float monto { get; set; }
        public decimal TotalPagar
        {
            get
            {
                if (monto == 0) return 0;
                var porciento = monto / 100 * 7.5;
                return Convert.ToDecimal(monto + porciento);
                ;
            }
        }
        public int external_id { get; set; }
        public string simulation { get; set; }
        public string recipient_sms_notification { get; set; }
        public bool sender_sms_notification { get; set; }
        public nautaUser sender { get; set; }
        public nautaUser recipient { get; set; }

        public async Task<nautaResponse> Simular(string usuario, int product,int monto)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var nauta = new nautaInfo
                {
                    account_number = usuario,
                    product_id = product,
                    monto = monto,
                    external_id = Convert.ToInt32(_Global.CodigoVerificacion),
                    simulation = "1",
                    recipient_sms_notification = "1",
                    sender_sms_notification = true,
                    sender = new nautaUser
                    {
                        last_name = _Global.CurrentAccount.lastname,
                        middle_name = "",
                        first_name = _Global.CurrentAccount.firstname,
                        email = _Global.CurrentAccount.email,
                        mobile = 13050000000
                    },
                    recipient = new nautaUser
                    {
                        last_name = "Arias",
                        middle_name = "",
                        first_name = "Royber",
                        email = "royberariasmoreno@gmail.com",
                        mobile = 5355043317
                    }
                };
                _Global.RecargaNauta = nauta;
                try
                {
                    var response = await client.PostAsync("nauta", nauta.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    var nautaResponse = JsonConvert.DeserializeObject<nautaResponse>(Result);
                    return nautaResponse;
                }
                catch (Exception)
                {
                    var nautaResponse = new nautaResponse();
                    nautaResponse.erroe_code = "-1";
                    nautaResponse.error_message = "Error de conección";
                    return nautaResponse;
                }
               

            }
        }

        public async Task<nautaResponse> Recargar()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _Global.RecargaNauta.simulation = "0";
                try
                {
                    var response = await client.PostAsync("nauta", _Global.RecargaNauta.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    var nautaResponse = JsonConvert.DeserializeObject<nautaResponse>(Result);
                    return nautaResponse;
                }
                catch (Exception)
                {
                    var nautaResponse = new nautaResponse();
                    nautaResponse.erroe_code = "-1";
                    nautaResponse.error_message = "Error de conección";
                    return nautaResponse;
                }


            }

        }

    }

    public class nautaResponse
    {
        public string transaction_id { get; set; }
        public string simulation { get; set; }
        public string status { get; set; }
        public string status_message { get; set; }
        public string date { get; set; }
        public string account_number { get; set; }
        public string external_id { get; set; }
        public string operator_reference { get; set; }
        public string product_id { get; set; }
        public string product { get; set; }
        public string product_desc { get; set; }
        public string product_currency { get; set; }
        public string product_value { get; set; }
        public string local_currency { get; set; }
        public string local_value { get; set; }
        public string operator_id { get; set; }
        public string country_id { get; set; }
        public string country { get; set; }
        public string account_currency { get; set; }
        public string wholesale_price { get; set; }
        public string retail_price { get; set; }
        public string fee { get; set; }

        public nautaUser sender { get; set; }
        public nautaUser recipient { get; set; }

        public string erroe_code { get; set; }
        public string error_message { get; set; }
    }

}
