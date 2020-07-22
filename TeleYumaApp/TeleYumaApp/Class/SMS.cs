using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TeleYumaApp.Class
{

    [DataContract(Namespace = "http://www.TeleYuma.com")]
    [Serializable]
    public class MessageResponse
    {
        [DataMember]
        public string ErrorCode { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract(Namespace = "http://www.TeleYuma.com")]
    [Serializable]
    public class Esms
    {
        [DataMember]
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [DataMember]
        public string Idsms { get; set; }
        [DataMember]
        public int i_account { get; set; }
        [DataMember]
        public string Phone1 { get; set; }
        [DataMember]
        public decimal monto { get; set; }
        [DataMember]
        public string NumeroTelefono { get; set; }
        [DataMember]
        public string PrefijoPais { get; set; }
        [DataMember]
        public string NombreContacto { get; set; }
        [DataMember]
        public string NombrePhoneContacto
        {
            get
            {
                if (_Global.ListaContactos.Count == 0)
                    return "";
                try
                {
                    var nombre = _Global.ListaContactos.First(x => x.Telefono.Trim().Contains(this.NumeroTelefono)).Nombre;
                    return nombre.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        [DataMember]
        public string Fecha { get; set; }
        [DataMember]
        public string FechaLarga
        {
            get
            {
                if (this.Fecha == "" || this.Fecha == null) return string.Empty;
                string strdate = this.Fecha; // mm/dd/yyyy 
                DateTime oDate = DateTime.ParseExact(strdate, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                return oDate.ToLongDateString();
            }
        }
        [DataMember]
        public bool isLoading
        {
            get
            {
                return !isSend;
            }
        }
        [DataMember]
        public bool isSend { get; set; }
        [DataMember]
        public string Hora { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [DataMember]
        public string RemitenteNumero { get; set; }
        [DataMember]
        public string RemitenteNombre { get; set; }
        [DataMember]
        public string Firma { get; set; }
        [DataMember]
        public int ItemHeight { get; set; }

        [DataMember]
        public string Token { get; set; }

        public bool Entrante
        {
            get
            {
                return !string.IsNullOrEmpty(RemitenteNumero);
            }
        }
        public bool Saliente
        {
            get
            {
                return string.IsNullOrEmpty(RemitenteNumero);
            }
        }

        public int nuevo
        {
            get
            {
                return IsNew ? 1 : 0;
            }
        }
        public bool IsNew { get; set; }
        public bool Notify { get; set; }

        [DefaultValue(false)]
        public bool mostrarIconoUser { get; set; }
        public string icono { get; set; }

        private string _image;

        public string image { get; set; }

        public string inicial { get; set; }

        public bool Error { get; set; }

        public async static Task<string> GetApikey()
        {
            var credenciales = await _Global.Get<Credenciales>("credenciales/4");
            return credenciales.KeyGenerate;
        }

        public async Task<MessageResponse> Enviar()
        {
            var apiKey = await GetApikey();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("apiKey", "a6V9NPooCNWzGaaEMsvPvQ==");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                var URL = "https://www.innoverit.com/api/smssend/?apikey=" + apiKey + "&number=+" + this.NumeroTelefono + "&content=" + this.Mensaje;

                try
                {
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var sendResponse = JsonConvert.DeserializeObject<innoverit.SendResponse>(json);
                    return new MessageResponse { ErrorCode = sendResponse.error_code, ErrorMessage = sendResponse.message };
                }
                catch (Exception ex)
                {
                    return new MessageResponse { ErrorCode = "-1", ErrorMessage = "No se pudo establecer conexión con el servidor." };

                }

            }

        }

        public async Task<bool> Eliminar()
        {
            using (var client = new HttpClient())
            {
                var URL = _Global.MasterURL + "SMS/Eliminar";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsync(URL, this.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    return Convert.ToBoolean(Result);
                }
                catch
                {
                    return false;
                }
            }


        }

        public async Task<bool> ActualizarCampo(ActualizarSMS datos)
        {
            datos.Idsms = this.Idsms.ToString();

            using (var client = new HttpClient())
            {
                var URL = _Global.MasterURL + "SMS/ActualizarCampo";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var response = await client.PostAsync(URL, datos.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    return Convert.ToBoolean(Result);
                }
                catch
                {
                    return false;
                }
            }


        }

        private SQLiteAsyncConnection _connection
        {
            get
            {
                var con = DependencyService.Get<ISQLiteDB>().GetConnection();
                con.CreateTableAsync<Esms>();
                return con;
            }
        }

        public bool Ingresar()
        {
            try
            {
                _connection.InsertAsync(this);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> GuardarNube()
        {
            var rerult = await _Global.Post<Esms>("SMS", this);
            return true;
        }

        public List<Esms> GetAll()
        {
            return _connection.Table<Esms>().ToListAsync().Result;
        }

        public bool Delete()
        {
            try
            {
                _connection.DeleteAsync(this);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update()
        {
            try
            {
                _connection.UpdateAsync(this);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

    public class ActualizarSMS
    {
        public string Idsms { get; set; }
        public string IdCuenta { get; set; }
        public string Campo { get; set; }
        public string Valor { get; set; }
    }

    public class GrupoSMS
    {
        public List<Esms> ListaSMS = new List<Esms>();
        public Esms sms
        {
            get
            {
                return ListaSMS.LastOrDefault();
            }
        }
        public int count
        {
            get
            {
                return (from item in ListaSMS where item.IsNew select item).Count();
            }
        }

        public bool ShowNews
        {
            get
            {
                if (count == 0) { return false; }
                else return true;
            }
        }

        public string prefijo
        {
            get
            {
                if (ListaSMS.Any())
                {
                    return ListaSMS.First().PrefijoPais;
                }
                else
                    return "";
            }
        }
        public string numero { get; set; }
        public string nombreContacto
        {
            get
            {
                var tel = Regex.Replace(numero, @"[^0-9A-Za-z]", "", RegexOptions.None);
                try
                {
                    foreach (var item in _Global.ListaContactos)
                    {
                        try
                        {
                            var numeroContacto = Regex.Replace(item.Telefono, @"[^0-9A-Za-z]", "", RegexOptions.None);

                            if (prefijo == "53")
                            {
                                numeroContacto = "53" + numeroContacto;
                                if (numeroContacto.Contains(tel))
                                {
                                    return item.Nombre;
                                }
                            }
                            else if (numeroContacto.Contains(tel))
                            {
                                return item.Nombre;
                            }
                        }
                        catch (Exception ex)
                        {

                            ;
                        }
                    }

                }
                catch (Exception)
                {

                    return "";
                }
                return "";
            }
        }
        public string ultimoSMS
        {
            get
            {
                return ListaSMS.LastOrDefault().Mensaje;
            }
        }

        [DefaultValue(false)]
        public bool mostrarIconoUser
        {
            get
            {
                if (nombreContacto == "" || nombreContacto == null)
                {
                    return true;
                }
                else
                    return false;
            }
        }

        public string _icono { get; set; }

        public string icono
        {
            get
            {
                if (string.IsNullOrEmpty(_icono) || _icono == "")
                {
                    _icono = _Global.RandomIcon;
                    return _icono;
                }

                else
                    return _icono;
            }

        }

        public string inicial
        {
            get
            {
                if (nombreContacto == "" || nombreContacto == null)
                {
                    return "";
                }
                else
                    return nombreContacto.ToCharArray()[0].ToString().ToUpper();
            }
        }

        public string fecha { get { return sms.Fecha; } }
        public string Hora { get { return sms.Hora; } }
        public string contacto
        {
            get
            {
                if (nombreContacto == "" || nombreContacto == null)
                    return numero;
                else
                    return nombreContacto;
            }
        }

        public async Task<List<Esms>> ActualizarListaSMS()
        {
            int countListas = _Global.GruposDeListasSMS.Count;
            bool ciclo = true;
            var lista = new List<Esms>();

            using (var client = new HttpClient())
            {
                var URL = _Global.MasterURL + "SMS/ConsultarAll/";

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.GetAsync(URL + _Global.CurrentAccount.i_account);
                    var Result = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<Esms>>(Result).Where(x => x.NumeroTelefono == this.numero).ToList();
                    if (lista.Count == 0 && countListas > 0)
                    {
                        while (ciclo)
                        {
                            response = await client.GetAsync(URL + _Global.CurrentAccount.i_account);
                            Result = await response.Content.ReadAsStringAsync();
                            lista = JsonConvert.DeserializeObject<List<Esms>>(Result).Where(x => x.NumeroTelefono == this.numero).ToList();
                            if (lista.Count > 0)
                                ciclo = false;
                        }

                    }

                }
                catch
                {
                    ;
                }
            }
            return lista;

        }

    }

    public class Grupos  //cargar los grupoes y la lista de sms de cada grupo
    {
        //public async Task<List<GrupoSMS>> ListaGrupos()
        //{
        //    int countListas = _Global.GruposDeListasSMS.Count;
        //    int ciclo = 0;
        //    var listaSMS = new List<Esms>();

        //    listaSMS = await _Global.CurrentAccount.GetAllSms();

        //    // agrupar sms por numero

        //    var results = from p in listaSMS
        //                  group p by p.NumeroTelefono into g
        //                  select new
        //                  {
        //                      numero = g.Key
        //                  };
        //    var grupos = results.ToList();

        //    var listaGrupos = new List<GrupoSMS>();

        //    var gruposms = new GrupoSMS();
        //    if (_Global.ListaContactos == new List<EContacto>())
        //        _Global.ListaContactos = await EContacto.GetListaContactos();

        //    foreach (var item in grupos)
        //    {
        //        var listasms = listaSMS.Where(x => x.NumeroTelefono == item.numero).ToList();
        //        var count = listasms.Count.ToString();
        //        if (listasms.Count > 99)
        //            count = "+99";
        //        var numero = item.numero;
        //        gruposms = new GrupoSMS();
        //        gruposms.numero = numero;
        //        gruposms.ListaSMS = listasms;

        //        gruposms.count = count;
        //        gruposms.ultimoSMS = listasms.First().Mensaje;
        //        gruposms.fecha = listasms.First().Fecha;
        //        gruposms.Hora = listasms.First().Hora;

        //        try
        //        {
        //            foreach (var cont in _Global.ListaContactos)
        //            {
        //                if (cont.Telefono != null)
        //                {


        //                    var tel = Regex.Replace(cont.Telefono, @"[^0-9A-Za-z]", "", RegexOptions.None);
        //                    if (numero.Contains(tel))
        //                    {
        //                        gruposms.nombreContacto = cont.Nombre;
        //                    }
        //                }
        //            }
        //            ;
        //        }
        //        catch (Exception ex)
        //        {
        //            ;
        //        }

        //        listaGrupos.Add(gruposms);
        //    }
        //    return listaGrupos;

        //}
    }

}
