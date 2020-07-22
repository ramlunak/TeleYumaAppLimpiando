using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TeleYumaApp.Class
{
    [DataContract(Namespace = "http://www.TeleYuma.com")]
    [Serializable]
    public class EReserva
    {
        [DataMember]
        public int IdReserva { get; set; }
        [DataMember]
        public int IdPromocion { get; set; }
        [DataMember]
        public string IdCuenta { get; set; }
        [DataMember]
        public int i_account { get; set; }
        [DataMember]
        public string Telefono { get; set; }
        [DataMember]
        public float Monto { get; set; }
        [DataMember]
        public float TotalPagar { get; set; }
        [DataMember]
        public string FechaReserva { get; set; }
        [DataMember]
        public string FechaRecarga { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public string Mensaje { get; set; }

    }
}
