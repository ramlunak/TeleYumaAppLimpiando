using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace TeleYumaApp.Class
{
    public class GetServiceFeatures
    {
        
        public static service_features_info voice_dialing
        {
            get
            {
                service_attribute_info[] atributos = new service_attribute_info[3];
                atributos[0] = new service_attribute_info{name = "translate_cli_out", effective_flag_value = new []{"N"}, values = new[] { "N" } };
                atributos[1] = new service_attribute_info{name = "i_dial_rule", effective_flag_value = new []{ "24997" }, values = new[] { "24997" } };
                atributos[2] = new service_attribute_info{name = "translate_cli_in", effective_flag_value = new []{"N"}, values = new[] { "N" } };
                return new service_features_info
                {
                    name = "voice_dialing",
                    effective_flag_value = "Y",
                    flag_value = "Y",
                    attributes = atributos
                };
            }
        }
    }

    public class service_features_info
    {
        [DataMember]
        public string flag_value { get; set; }
        [DataMember]
        public string effective_flag_value { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public service_attribute_info[] attributes { get; set; }

    }

    public class service_attribute_info
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string[] effective_flag_value { get; set; }
        [DataMember]
        public string[] values { get; set; }

    }

    public class update_service_features_request
    {
        [DataMember]
        public int i_customer { get; set; }
        [DataMember]
        public service_features_info[] service_features { get; set; }

    }
}