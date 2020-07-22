using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;

namespace TeleYumaApp.Class
{


    public class topupInfo
    {
        public string login { get; set; }
        public int key { get; set; }
        public string md5 { get; set; }
        public string action { get; set; }
        public string destination_msisdn { get; set; }
        public string msisdn { get; set; }
        public string product { get; set; }

        [DefaultValue("yes")]
        public string sender_sms { get; set; }

    }

    public class topupResponse
    {
        public string transactionid { get; set; }
        public string country { get; set; }
        public string countryid { get; set; }
        public string operador { get; set; }
        public string operatorid { get; set; }
        public string destination_msisdn { get; set; }
        public string authentication_key { get; set; }
        public string error_code { get; set; }
        public string error_txt { get; set; }
    }
    

}
