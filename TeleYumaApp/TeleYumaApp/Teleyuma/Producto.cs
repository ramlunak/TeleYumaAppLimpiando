using System;
using System.Collections.Generic;
using System.Text;

namespace TeleYumaApp.Teleyuma
{
    public class Producto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string ProviderCode { get; set; }
        public string CountryIso { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float CommissionRate { get; set; }
        public string ValidationRegex { get; set; }
        public string DisplayText { get; set; }
        public string Bono { get; set; }
    }

    public class ValidateProducto
    {
        public string Producto { get; set; }
        public string IdCuenta { get; set; }
        public string TipoProducto { get; set; }
        public float Monto { get; set; }
    }



}
