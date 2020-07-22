using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TeleYumaApp.Class
{
    public class EPromo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public byte[] image { get; set; }

        public async Task<List<EPromo>> GetPromos()
        {
            return await _Global.Get<List<EPromo>>("Promo");
        }
    }

}
