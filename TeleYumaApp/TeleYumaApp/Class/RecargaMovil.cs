using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using TeleYumaApp.Class;
using System.Security.Cryptography;
using System.ComponentModel;

using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Globalization;
using TeleYumaApp.Teleyuma;

namespace TeleYumaApp.Class
{
    public class Recarga
    {
        public int idRecarga { get; set; }

        public string Code { get; set; }
        public string numero { get; set; }
        public float monto { get; set; }
        public float precio { get; set; }
        public string tipo { get; set; }
        public string icon
        {
            get
            {
                if (tipo == "movil")
                    return "phone_blue";
                else return "wifi_green";
            }
        }

        public decimal TotalPagar
        {
            get
            {
                return decimal.Round(Convert.ToDecimal(precio), 2);

                //if (monto == 0) return 0;
                //// var comision = (float)decimal.Parse(_Global.RecMovilConfig.Porciento.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
                //var porciento = monto / 100 * comision;
                //var T = monto + porciento;
                //return decimal.Round(Convert.ToDecimal(T), 2);
            }
        }

        public string topupResponseCountry { get { return topupResponse.country; } }

        public string topupResponseOperador { get { return topupResponse.operador; } }

        public string topupResponseErrorCode { get { return topupResponse.error_code; } }

        public string ColorSimulacion { get { return "#70AD47"; } }

        public topupResponse topupResponse = new topupResponse();

        public string color
        {
            get
            {
                if (topupResponse.error_code == "0")
                    return "Green";
                else return "Red";

            }
        }

        public string error
        {
            get
            {
                if (topupResponse.error_code == "0")
                    return "Recarga satisfactoria";
                else return "Error en la recarga";
            }
        }

        public async Task<bool> Simular()
        {
            var result = await Ding.SimulateTransfer(new Ding.SendTransferRequest
            {
                SkuCode = this.Code,
                AccountNumber = this.numero,
                SendValue = (float)this.monto,
                DistributorRef = _Global.CurrentAccount.phone1,
                ValidateOnly = true
            });

            if (result is null)
            {
                this.topupResponse = new topupResponse { error_code = "-1" };
                return true;
            }
            if (result.TransferRecord.ProcessingState == "Complete")
            {
                this.topupResponse = new topupResponse { error_code = "0" };
            }
            else
            {
                this.topupResponse = new topupResponse { error_code = "-1" };
            }
            return true;
        }

        public async Task<bool> Recargar()
        {
            var result = await Ding.SimulateTransfer(new Ding.SendTransferRequest
            {
                SkuCode = this.Code,
                AccountNumber = this.numero,
                SendValue = (float)this.monto,
                DistributorRef = _Global.CurrentAccount.phone1,
                ValidateOnly = false
            });

            if (result is null)
            {
                this.topupResponse = new topupResponse { error_code = "-1" };
                return true;
            }
            if (result.TransferRecord.ProcessingState == "Complete")
            {
                this.topupResponse = new topupResponse { error_code = "0" };
            }
            else
            {
                this.topupResponse = new topupResponse { error_code = "-1" };
            }
            return true;

            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            //    var param = new topupInfo()
            //    {
            //        msisdn = "69999999999",
            //        destination_msisdn = this.numero,
            //        product = this.monto.ToString(),
            //        sender_sms = "yes",
            //        action = "topup"
            //    };

            //    var response = await client.PostAsync("topup", param.AsJsonStringContent());
            //    var Result = await response.Content.ReadAsStringAsync();
            //    try
            //    {
            //        this.topupResponse = JsonConvert.DeserializeObject<topupResponse>(Result);
            //        return true;
            //    }
            //    catch
            //    {
            //        return false;
            //    }

            //}


        }

        public async Task<bool> Reservar()
        {
            using (var client = new HttpClient())
            {
                //var urlServidor  = "http://192.168.101.1/service/Service1.svc/";// wifi
                //var urlServidor  = "http://192.168.42.42/service/Service1.svc/";// usb
                //var urlServidor = "http://169.254.80.80/service/Service1.svc/";// emulador
                var urlServidor = "http://smsteleyuma.azurewebsites.net/Service1.svc/";// azurewebsites               

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Reserva = new EReserva();

                Reserva.IdPromocion = _Global.Promocion.IdPromocion;
                Reserva.IdCuenta = _Global.CurrentAccount.id;
                Reserva.i_account = _Global.CurrentAccount.i_account;
                Reserva.Telefono = this.numero;
                Reserva.Monto = this.monto;
                Reserva.TotalPagar = (float)this.TotalPagar;
                Reserva.FechaReserva = DateTime.Now.ToShortDateString();
                Reserva.Estado = EstadoReserva.Espera.ToString();

                try
                {
                    var URL = urlServidor + "/Reservas/IngresarReserva";
                    var response = await client.PostAsync(URL, Reserva.AsJsonStringContent());
                    var Result = await response.Content.ReadAsStringAsync();
                    if (Convert.ToInt32(Result) > 0)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }

            }
        }

    }

    public class ListaRecarga
    {
        public ListaRecarga()
        {
            Lista = new ObservableCollection<Recarga>();
        }

        public ObservableCollection<Recarga> Lista = new ObservableCollection<Recarga>();

        public Command<Recarga> Delete
        {
            get
            {
                return new Command<Recarga>((r) =>
                {
                    Lista.Remove(r);
                });
            }
        }

        public float MontoLista
        {
            get
            {
                float sum = 0;
                foreach (var rec in Lista)
                {
                    sum += rec.monto;
                }
                return (float)decimal.Round(Convert.ToDecimal(sum), 2);
            }
        }

        public double TotalPagar
        {
            get
            {
                float sum = 0;
                foreach (var rec in Lista)
                {
                    sum += rec.precio;
                }
                return (double)decimal.Round(Convert.ToDecimal(sum), 2);
            }

            // get
            // {
            //if (MontoLista == 0) return 0;
            //// var porciento = MontoLista / 100 * (float)decimal.Parse(_Global.RecMovilConfig.Porciento.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
            //var porciento = MontoLista / 100 * comision;
            //var T = MontoLista + porciento;
            //return (double)decimal.Round(Convert.ToDecimal(T), 2);
            // }
        }

        public async Task<bool> Simular()
        {
            return true;
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("http://smsteleyuma.azurewebsites.net/Service1.svc/TransferTo/");
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    foreach (var item in this.Lista)
            //    {
            //        var param = new topupInfo()
            //        {
            //            msisdn = "69999999999",
            //            destination_msisdn = item.numero,
            //            product = item.monto.ToString(),
            //            sender_sms = "yes",
            //            action = "simulation"
            //        };

            //        var response = await client.PostAsync("topup", param.AsJsonStringContent());
            //        var Result = await response.Content.ReadAsStringAsync();
            //        try
            //        {
            //            item.topupResponse = JsonConvert.DeserializeObject<topupResponse>(Result);
            //            return true;
            //        }
            //        catch
            //        {
            //            return false;
            //        }

            //    }
            //}

            return false;
        }

    }

}
