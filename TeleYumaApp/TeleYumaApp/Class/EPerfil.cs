using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TeleYumaApp.Class
{
    public class EPerfil
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string IdCuenta { get; set; }
        [DataMember]
        public byte[] Foto { get; set; }

    }

}
