using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TeleYumaApp.Class
{
        
    public class AuthInfo
    {       
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string password { get; set; } 
        [DataMember]
        public string session_id { get; set; }
        
    }
}
