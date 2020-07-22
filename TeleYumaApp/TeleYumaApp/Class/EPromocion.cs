using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TeleYumaApp.Class
{
    [DataContract(Namespace = "http://www.TeleYuma.com")]
    [Serializable]
    public class EPromocion
    {
        [DataMember]
        public int IdPromocion { get; set; }
        [DataMember]
        public int Monto { get; set; }
        [DataMember]
        public int Estado { get; set; }
        [DataMember]
        public string Texto { get; set; }
        [DataMember]
        public string Noti { get; set; }
        [DataMember]
        public int IntervaloNoti { get; set; }
        [DataMember]
        public string FechaInicio { get; set; }
        [DataMember]
        public string FechaFin { get; set; }
    }
}
