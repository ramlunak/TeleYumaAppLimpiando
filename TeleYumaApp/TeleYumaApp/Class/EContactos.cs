using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace TeleYumaApp.Class
{

    public class EContacto
    {
        [DataMember]
        public int IdContacto { get; set; }
        [DataMember]
        public string Nombre { get; set; }

        //Para probar 
        [DataMember]
        public string Fecha { get; set; }

        [DataMember]
        public string UserNauta { get; set; }
        [DataMember]
        public string Telefono { get; set; }
        [DataMember]
        public string Pais { get; set; }
        [DataMember]
        public string Prefijo { get; set; }
        [DataMember]
        public string IdCuenta { get; set; }
        public override string ToString() => Nombre;

        public static async Task<List<EContacto>> GetListaContactos()
        {

            var ListaContactos = new List<EContacto>();
            var PhoneContactos = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            foreach (var item in PhoneContactos)
            {
                var nauta = "";
                if (item.Emails.Count > 0)
                {
                    foreach (var con in item.Emails)
                    {
                        if (con.Contains("@nauta.com.cu"))
                        {
                            nauta = con;
                        }

                    }
                }
                else
                {
                    if (item.Email != null)
                        if (item.Email.Contains("@nauta.com.cu"))
                            nauta = item.Email;
                }

                if (nauta != "")
                {
                    var arr = nauta.Split('@');
                    nauta = arr[0].ToString();
                    ;
                }
                var contacto = new EContacto();
                contacto.Nombre = item.Name;
                contacto.Telefono = item.Number;
                contacto.UserNauta = nauta;
                ListaContactos.Add(contacto);
            }

           return ListaContactos;

        }



        public async Task<bool> IngresarContacto()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = _Global.MasterURL + "Contactos/IngresarContacto";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsync(URL, this.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.Vistas.ListaContactos.LlenarLista();
                    return true;
                }

                catch
                {
                    return false;
                }


            }
        }

        public async Task<bool> EditarContacto()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = _Global.MasterURL + "Contactos/ActualizarContacto";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsync(URL, this.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.Vistas.ListaContactos.LlenarLista();
                    return true;
                }

                catch
                {
                    return false;
                }


            }
        }

        public async Task<bool> EliminarContacto()
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = _Global.MasterURL + "Contactos/EliminarContacto";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsync(URL, this.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.Vistas.ListaContactos.LlenarLista();
                    return true;
                }

                catch
                {
                    return false;
                }

            }
        }

     

    }

}
