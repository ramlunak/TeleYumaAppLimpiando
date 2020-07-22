using System;
using System.Collections.Generic;
using System.Text;
using TeleYumaApp.Class;

namespace TeleYumaApp.Teleyuma
{
    public class Compra
    {
        public string TipoProducto { get; set; }
        public string Producto { get; set; }
        public string Empresa { get; set; }
        public string LabelProducto
        {
            get
            {
                if (TipoProducto == "movil")
                    return "Número";
                else return "Usuario";
            }
        }
        public string LabelBono
        {
            get
            {
                if (string.IsNullOrEmpty(Bono))
                    return "";
                else return "Bono:";
            }
        }
        public string ProductoIcon
        {
            get
            {
                if (TipoProducto == "movil")
                    return "movil2";
                else return "conexion3g";
            }
        }
        public float Monto { get; set; }
        public float Precio { get; set; }
        public string Bono { get; set; }
        
        public bool BonoVisible
        {
            get
            {
                return string.IsNullOrEmpty(Bono) ? false : true;
            }
        }
        public EstadoCompra Estado { get; set; }

    }
}
