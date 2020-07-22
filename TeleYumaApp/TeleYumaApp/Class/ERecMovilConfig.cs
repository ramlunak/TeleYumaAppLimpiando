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
    public class ERecMovilConfig
    {
        [DataMember]
        public float Porciento { get; set; }

        public async Task<bool> GetPorcientoRecargaMovil()
        {
            try
            {
                var CuentaConfig = new account_info { id = "RecMovilConfig" };
                var RecMovilConfig = await CuentaConfig.GetAccountInfo();
                if (RecMovilConfig.firstname == null)
                {
                    return false;
                }
                else
                {
                    _Global.RecMovilConfig = JsonConvert.DeserializeObject<ERecMovilConfig>(RecMovilConfig.firstname);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

    }
}
