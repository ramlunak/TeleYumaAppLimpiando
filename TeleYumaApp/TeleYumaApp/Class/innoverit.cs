using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TeleYumaApp.Class
{
    public class innoverit
    {

        public string apikey { get; set; }
        public string number { get; set; }
        public string content { get; set; }
        public string delivery_status { get; set; }
        public string error { get; set; }
        public int idsms { get; set; }
        public string costo { get; set; }
        public string iso { get; set; }
        public decimal balance { get; set; }

        public async Task Send()
        {

            //var r = await _Global.Post <innoverit> ("https://www.innoverit.com/api/smssend",this);
            // ;
            //using (HttpClient client = new HttpClient())
            //{
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            //    var URL = "https://www.innoverit.com/api/smssend/?apikey=04a26d8f1534598bdf73fb93a0025867&number=+" + sms.NumeroTelefono + "&content=" + sms.SMS;

            //    try
            //    {
            //        var response = await client.GetAsync(URL);
            //        var json = await response.Content.ReadAsStringAsync();
            //        var repuesta = JsonConvert.DeserializeObject<innoverit>(json);
            //        if (repuesta.delivery_status == "OK")
            //        {
            //            // Dsms.Ingresar(sms);
            //            return new MessageResponse { ErrorCode = "null", ErrorMessage = "El mensaje ha sido enviado" };

            //        }
            //        else return new MessageResponse { ErrorCode = "1", ErrorMessage = "Número incorrecto" };
            //    }
            //    catch (Exception ex)
            //    {
            //        return new MessageResponse { ErrorCode = "-1", ErrorMessage = ex.Message };

            //    }

            //}

        }

        public class SendResponse
        {
            public string error_code { get; set; }
            public string status { get; set; }
            public string message { get; set; }
            public string error { get; set; }
        }

    }
}