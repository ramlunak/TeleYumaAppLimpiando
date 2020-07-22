using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace TeleYumaApp.Class
{
    public class SMSTwilio
    {
        [DataMember]
        public string to { get; set; }
        [DataMember]
        public string body { get; set; }
        [DataMember]
        public string token { get; set; }
    }
    

}
