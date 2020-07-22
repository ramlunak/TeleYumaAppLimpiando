using System;
using System.Collections.Generic;
using System.Text;

namespace TeleYumaApp.Class
{

    public class CountryObjet
    {
        public List<CountryInfo> countries_list { get; set; }
    }

    public class CountryInfo
    {
        public string iso_3166_1_a2 { get; set; }
        public string name { get; set; }
    }

    public class SubdivisionsObjet
    {
        public List<SubdivisionInfo> subdivisions_list { get; set; }
    }

    public class GetSubdivisionsListRequest
    {
        private string _iso_3166_1_a2 { get; set; }
        public string iso_3166_1_a2
        {
            get
            {
                return _iso_3166_1_a2;
            }
            set
            {
                _iso_3166_1_a2 = value.ToUpper();
            }
        }
    }

    public class SubdivisionInfo
    {
        public int i_country_subdivision { get; set; }
        public string name { get; set; }
        public string iso_3166_1_a2 { get; set; }
        public string iso_3166_2 { get; set; }
    }


}
