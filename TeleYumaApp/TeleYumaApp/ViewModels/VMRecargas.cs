using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
using TeleYumaApp.Teleyuma;
using System.Threading.Tasks;
using System.Globalization;

namespace TeleYumaApp.ViewModels
{

    public class VMRecargas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;


        public VMRecargas()
        {

            SelectedItem = string.Empty;
            CargarFotoPais();
            MovilSelected = true;
            NautaSelected = false;
            ListaPrecios = false;
            ListaProductoVisible = false;
            OpcionesRecargaVisible = false;
            PaisLeftImage = "place";
            OpcionesCargandoVisible = false;
            txtNauta = string.Empty;

        }

        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }

        #region Pais

        private string _PromoActiva;

        public string PromoActiva
        {
            get { return _PromoActiva; }
            set { _PromoActiva = value; OnPropertyChanged(); }
        }

        private bool _ShowPromoActiva;

        public bool ShowPromoActiva
        {
            get { return _ShowPromoActiva; }
            set { _ShowPromoActiva = value; OnPropertyChanged(); }
        }

        public async void CargarPromo()
        {
            try
            {
                var promo = await Ding.GetPromoActiva();
                PromoActiva = promo.Promo;
                ShowPromoActiva = true;
            }
            catch (Exception ex)
            {
                ShowPromoActiva = false;
            }
            Task.Delay(5000);
            CargarPromo();
        }

        #endregion

        private bool _OpcionesCargandoVisible;

        public bool OpcionesCargandoVisible
        {
            get { return _OpcionesCargandoVisible; }
            set { _OpcionesCargandoVisible = value; OnPropertyChanged(); }
        }

        private bool _OpcionesRecargaVisible;

        public bool OpcionesRecargaVisible
        {
            get { return _OpcionesRecargaVisible; }
            set { _OpcionesRecargaVisible = value; OnPropertyChanged(); }
        }



        #region Pais

        public ICollection<string> ItemsSource
        {
            get => Pais.Paises.ToList();

        }

        private string _PaisLeftImage;
        public string PaisLeftImage
        {
            get { return _PaisLeftImage; }
            set { _PaisLeftImage = value; OnPropertyChanged(); }
        }

        private string _SelectedItem;
        public string SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }

        private string _Prefijo;
        public string Prefijo
        {
            get { return _Prefijo; }
            set { _Prefijo = value; OnPropertyChanged(); }
        }

        private ICommand _PaisSelectedIndexChangedCommand;

        public ICommand PaisSelectedIndexChangedCommand
        {
            get
            {
                if (_PaisSelectedIndexChangedCommand == null)
                {
                    _PaisSelectedIndexChangedCommand = new RelayCommand(PaisSelectedIndexChangedExecute, CanSubmitExecute);
                }
                return _PaisSelectedIndexChangedCommand;
            }
        }

        public void PaisSelectedIndexChangedExecute(object parameter)
        {
            TipoProducto = "movil";
            CargarFotoPais();

        }

        public void CargarFotoPais()
        {
            try
            {
                var PaisSelecionado = Pais.GetList().First(p => p.Nombre.ToLower() == SelectedItem.ToLower());

                if (PaisSelecionado != null)
                {
                    _Global.PaisSeleccionado = PaisSelecionado;

                    PaisLeftImage = PaisSelecionado.image;
                    Prefijo = PaisSelecionado.PrefijoTelefonico;
                    if (PaisSelecionado.Nombre.ToLower() == "Cuba".ToLower())
                        ListaProductoVisible = true;
                }
                else
                    PaisLeftImage = "place";
            }
            catch
            {
                PaisLeftImage = "place";

            }
        }

        public void CargarPaisByPrefijo()
        {
            TipoProducto = "movil";
            EPais pais;
            try
            {
                pais = Pais.GetPaisByPrefijo(Prefijo);
                if (pais == null)
                {
                    CurrentPage.DisplayAlert("TeleYuma", "El sistema no encontró ningun País con ese Prefijo Telefónico", "Ok");
                    ResetForm();
                    return;
                }
                else
                {
                    SelectedItem = pais.Nombre;
                }

            }
            catch (Exception)
            {
                CurrentPage.DisplayAlert("TeleYuma", "El sistema no encontró ningun País con ese Prefijo Telefónico", "Ok");
                ResetForm();
                return;

            }
        }
        #endregion

        #region Producto

        private ObservableCollection<Producto> _preciosProductos;

        public ObservableCollection<Producto> PreciosProductos
        {
            get { return _preciosProductos; }
            set { _preciosProductos = value; OnPropertyChanged(); }
        }

        public string TipoProducto { get; set; }

        private bool _MovilSelected;

        public bool MovilSelected
        {
            get { return _MovilSelected; }
            set { _MovilSelected = value; OnPropertyChanged(); }
        }

        private bool _NautaSelected;

        public bool NautaSelected
        {
            get { return _NautaSelected; }
            set { _NautaSelected = value; OnPropertyChanged(); }
        }

        private bool _ListaPrecios;

        public bool ListaPrecios
        {
            get { return _ListaPrecios; }
            set { _ListaPrecios = value; OnPropertyChanged(); }
        }

        private Producto _PrecioSelected;

        public Producto PrecioSelected
        {
            get { return _PrecioSelected; }
            set { _PrecioSelected = value; OnPropertyChanged(); }
        }

        private bool _ListaProductoVisible;

        public bool ListaProductoVisible
        {
            get { return _ListaProductoVisible; }
            set { _ListaProductoVisible = value; OnPropertyChanged(); }
        }


        private ICommand _ProductoTappedCommand;

        public ICommand ProductoTappedCommand
        {
            get
            {
                if (_ProductoTappedCommand == null)
                {
                    _ProductoTappedCommand = new RelayCommand(ProductoTappedExecute, CanSubmitExecute);
                }
                return _ProductoTappedCommand;
            }
        }

        public void ProductoTappedExecute(object parameter)
        {
            if (parameter.ToString() == "Movil")
            {
                TipoProducto = "movil";
                MovilSelected = true;
                NautaSelected = false;
            }
            else if (parameter.ToString() == "Nauta")
            {
                TipoProducto = "nauta";
                MovilSelected = false;
                NautaSelected = true;
            }
            else if (parameter.ToString() == "datos600MG" || parameter.ToString() == "datos1G" || parameter.ToString() == "datos2_5G")
            {
                TipoProducto = parameter.ToString();
                MovilSelected = true;
                NautaSelected = false;
            }
            ListaProductoVisible = false;
        }

        #endregion

        #region Numero

        private string _txtNumero;
        public string txtNumero
        {
            get { return _txtNumero; }
            set { _txtNumero = value.ToLower(); OnPropertyChanged(); }
        }

        #endregion

        #region Nauta

        private string _txtNauta;
        public string txtNauta
        {
            get { return _txtNauta; }
            set { _txtNauta = value.ToLower(); OnPropertyChanged(); }
        }

        private string _NautaAutoComplete;
        public string NautaAutoComplete
        {
            get { return _NautaAutoComplete; }
            set { _NautaAutoComplete = value; OnPropertyChanged(); }
        }


        private ICommand _NautaAutoCompleteTappedCommand;
        public ICommand NautaAutoCompleteTappedCommand
        {
            get
            {
                if (_NautaAutoCompleteTappedCommand == null)
                {
                    _NautaAutoCompleteTappedCommand = new RelayCommand(NautaAutoCompleteTappedExecute, CanSubmitExecute);
                }
                return _NautaAutoCompleteTappedCommand;
            }
        }

        public void NautaAutoCompleteTappedExecute(object parameter)
        {
            txtNauta = NautaAutoComplete;
        }

        #endregion

        #region Monto
        private string _txtMonto;

        public string txtMonto
        {
            get { return _txtMonto; }
            set { _txtMonto = value; OnPropertyChanged(); }
        }

        #endregion

        private ICommand _AddToCartCommand;
        public ICommand AddToCartCommand
        {
            get
            {
                if (_AddToCartCommand == null)
                {
                    _AddToCartCommand = new RelayCommand(AddToCartExecute, CanSubmitExecute);
                }
                return _AddToCartCommand;
            }
        }


        public async void AddToCartExecute(object parameter)
        {

            //var compra1 = new Compra
            //{
            //    TipoProducto = TipoProducto,
            //    Producto = Prefijo + txtNumero,
            //    Saldo = (float)Convert.ToDecimal(txtMonto),
            //    Precio = 20,
            //    Bono = "",
            //    Estado = EstadoCompra.Espera
            //};

            //_Global.VM.VMCompras.Compras.Add(compra1);

            //var comision = (float)decimal.Parse(_Global.RecMovilConfig.Porciento.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));

            //_Global.ListaRecargas.Lista.Add(new Recarga {numero= Prefijo + txtNumero, monto = (float)Convert.ToDecimal(txtMonto)});

            //ResetForm();
            //return;

            if (_Global.VM.VMCompras.Compras.Count >= 5)
            {
                CurrentPage.DisplayAlert("TeleYuma", "Solo se permiten 5 recargas por lista", "Ok");
                OpcionesCargandoVisible = false;
                return;
            }

            OpcionesCargandoVisible = true;

            if (TipoProducto == "movil")
            {
                try
                {

                    if (SelectedItem == null || string.IsNullOrEmpty(txtNumero))
                    {
                        CurrentPage.DisplayAlert("TeleYuma", "Complete la los datos correctamente", "Ok");
                        OpcionesCargandoVisible = false;
                        return;
                    }
                   
                    var validarMovil = new ValidateProducto();

                    if (string.IsNullOrEmpty(txtMonto))
                    {
                         validarMovil = new ValidateProducto
                        {
                            Producto = Prefijo + txtNumero,
                            TipoProducto = TipoProducto,                          
                            IdCuenta = _Global.CurrentAccount.i_account.ToString()
                        };
                    }
                    else
                    {
                         validarMovil = new ValidateProducto
                        {
                            Producto = Prefijo + txtNumero,
                            TipoProducto = TipoProducto,
                            Monto = (float)Convert.ToDecimal(txtMonto),
                            IdCuenta = _Global.CurrentAccount.i_account.ToString()
                        };
                    }

                    var resultMovil = await _Global.Post<Producto>("Producto", validarMovil);

                    if ((resultMovil != null && resultMovil.Name != null) && string.IsNullOrEmpty(txtMonto))
                    {
                        CurrentPage.DisplayAlert("TeleYuma", "El rango del monto debe ser " + resultMovil.DisplayText, "Ok");
                        OpcionesCargandoVisible = false;
                        return;
                    }
                    
                  
                    if (resultMovil is null || resultMovil.Name is null)
                    {

                        var resul = await _Global.Post<List<Producto>>("Producto", validarMovil);

                        if (resul is null)
                        {
                            CurrentPage.DisplayAlert("TeleYuma", "El número no es correcto", "Ok");
                            OpcionesCargandoVisible = false;
                            return;
                        }
                        if (resul.FirstOrDefault().CountryIso.ToLower() != Pais.GetIso2ByPrefijo(Prefijo).ToLower())
                        {
                            CurrentPage.DisplayAlert("TeleYuma", "Este número no pertenece al País seleccionado", "Ok");
                            OpcionesCargandoVisible = false;
                            return;
                        }
                        else
                        {
                            ListaPrecios = true;
                            PreciosProductos = new ObservableCollection<Producto>(resul);
                            OpcionesCargandoVisible = false;
                            return;
                        }
                    }

                    if (resultMovil.CountryIso.ToLower() != Pais.GetIso2ByPrefijo(Prefijo).ToLower())
                    {
                        CurrentPage.DisplayAlert("TeleYuma", "Este número no pertenece al País seleccionado", "Ok");
                        OpcionesCargandoVisible = false;
                        return;
                    }

                    var monto = (float)Convert.ToDecimal(txtMonto);
                    var min = resultMovil.MinValue;
                    var max = resultMovil.MaxValue;

                    if (monto < min || monto > max)
                    {
                        CurrentPage.DisplayAlert("TeleYuma", "El rango del monto debe ser " + resultMovil.DisplayText, "Ok");
                        OpcionesCargandoVisible = false;
                        return;
                    }

                    var precio = await CalcularPrecioMovil((float)Convert.ToDecimal(txtMonto));

                    var compra = new Compra
                    {
                        TipoProducto = TipoProducto,
                        Producto = Prefijo + txtNumero,
                        Monto = (float)Convert.ToDecimal(txtMonto),
                        Precio = precio,
                        Empresa = resultMovil.Name,
                        Bono = resultMovil.Bono,
                        Estado = EstadoCompra.Espera
                    };
                    OpcionesCargandoVisible = false;


                    _Global.VM.VMCompras.Compras.Add(compra);
                    _Global.ListaRecargas.Lista.Add(new Recarga { Code = resultMovil.Code, tipo = "movil", numero = compra.Producto, monto = compra.Monto, precio = compra.Precio });
                    ResetForm();
                }
                catch (Exception ex)
                {
                    CurrentPage.DisplayAlert("TeleYuma", "Complete la los datos correctamente", "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }
            }
            else if (TipoProducto == "datos600MG" || TipoProducto == "datos1G" || TipoProducto == "datos2_5G")
            {
                if (SelectedItem == null || string.IsNullOrEmpty(txtNumero) || string.IsNullOrEmpty(txtMonto))
                {
                    CurrentPage.DisplayAlert("TeleYuma", "Complete la los datos correctamente", "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                if (_Global.VM.VMCompras.Compras.Where(x => x.Producto == (Prefijo + txtNumero).ToString()).Any())
                {
                    CurrentPage.DisplayAlert("TeleYuma", "Para recargar el mismo numero de telefono espere 5 minutos", "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                var validarDatos = new ValidateProducto
                {
                    Producto = Prefijo + txtNumero,
                    TipoProducto = TipoProducto,
                    IdCuenta = _Global.CurrentAccount.i_account.ToString(),
                    Monto = (float)Convert.ToDecimal(txtMonto)
                };
                var resultDatos = await _Global.Post<Producto>("Producto", validarDatos);

                if (resultDatos is null || resultDatos.Name is null)
                {
                    CurrentPage.DisplayAlert("TeleYuma", "El número no es correcto", "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                var monto = (float)Convert.ToDecimal(txtMonto);
                var min = resultDatos.MinValue;
                var max = resultDatos.MaxValue;

                if (monto < min || monto > max)
                {
                    CurrentPage.DisplayAlert("TeleYuma", "El rango del monto debe ser " + resultDatos.DisplayText, "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                var precio = await CalcularPrecioMovil((float)Convert.ToDecimal(txtMonto));

                var compra = new Compra
                {
                    TipoProducto = TipoProducto,
                    Producto = Prefijo + txtNumero,
                    Monto = (float)Convert.ToDecimal(txtMonto),
                    Precio = precio,
                    Empresa = resultDatos.Name,
                    Bono = resultDatos.Bono,
                    Estado = EstadoCompra.Espera
                };
                OpcionesCargandoVisible = false;

                _Global.VM.VMCompras.Compras.Add(compra);
                _Global.ListaRecargas.Lista.Add(new Recarga { Code = resultDatos.Code, tipo = TipoProducto, numero = compra.Producto, monto = compra.Monto, precio = compra.Precio });
                ResetForm();
            }
            else if (TipoProducto == "nauta")
            {
                if (SelectedItem == null || string.IsNullOrEmpty(txtNauta) || string.IsNullOrEmpty(txtMonto))
                {
                    CurrentPage.DisplayAlert("TeleYuma", "Complete la los datos correctamente", "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                if (_Global.VM.VMCompras.Compras.Where(x => x.Producto == (txtNauta + "@nauta.com.cu").ToString()).Any())
                {
                    CurrentPage.DisplayAlert("TeleYuma", "Para recargar la misma cuenta espere 5 minutos", "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                var validarNauta = new ValidateProducto
                {
                    Producto = txtNauta + "@nauta.com.cu",
                    TipoProducto = TipoProducto,
                    IdCuenta = _Global.CurrentAccount.i_account.ToString(),
                    Monto = (float)Convert.ToDecimal(txtMonto)
                };

                var resultNauta = await _Global.Post<Producto>("Producto", validarNauta);

                if (resultNauta is null || resultNauta.Name is null)
                {
                    CurrentPage.DisplayAlert("TeleYuma", "La cuenta nauta no es correcta", "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                var monto = (float)Convert.ToDecimal(txtMonto);
                var min = resultNauta.MinValue;
                var max = resultNauta.MaxValue;

                if (monto < min || monto > max)
                {
                    CurrentPage.DisplayAlert("TeleYuma", "El rango del monto debe ser " + resultNauta.DisplayText, "Ok");
                    OpcionesCargandoVisible = false;
                    return;
                }

                var precio = await CalcularPrecioMovil((float)Convert.ToDecimal(txtMonto));

                var compra = new Compra
                {
                    TipoProducto = TipoProducto,
                    Producto = txtNauta + "@nauta.com.cu",
                    Monto = (float)Convert.ToDecimal(txtMonto),
                    Empresa = resultNauta.Name,
                    Precio = precio,
                    Bono = "",
                    Estado = EstadoCompra.Espera
                };
                OpcionesCargandoVisible = false;



                _Global.VM.VMCompras.Compras.Add(compra);
                _Global.ListaRecargas.Lista.Add(new Recarga { Code = resultNauta.Code, tipo = "nauta", numero = compra.Producto, monto = compra.Monto, precio = compra.Precio });
                ResetForm();
            }

            OpcionesRecargaVisible = true;
            SelectedItem = null;
            OpcionesCargandoVisible = false;
        }

        private ICommand _PrecioProducoSelectCommand;
        public ICommand PrecioProducoSelectCommand
        {
            get
            {
                if (_PrecioProducoSelectCommand == null)
                {
                    _PrecioProducoSelectCommand = new RelayCommand(PrecioProducoSelectExecute, CanSubmitExecute);
                }
                return _PrecioProducoSelectCommand;
            }
        }

        public async void PrecioProducoSelectExecute(object parameter)
        {
            if (PrecioSelected is null) return;

            OpcionesCargandoVisible = true;

            txtMonto = PrecioSelected.MinValue.ToString();

            var precio = await CalcularPrecioMovil((float)Convert.ToDecimal(txtMonto));

            var compra = new Compra
            {
                TipoProducto = TipoProducto,
                Producto = Prefijo + txtNumero,
                Monto = (float)Convert.ToDecimal(txtMonto),
                Precio = precio,
                Empresa = PrecioSelected.Name,
                Estado = EstadoCompra.Espera
            };
            OpcionesCargandoVisible = false;

            _Global.VM.VMCompras.Compras.Add(compra);
            _Global.ListaRecargas.Lista.Add(new Recarga { Code = PrecioSelected.Code, tipo = "movil", numero = compra.Producto, monto = compra.Monto, precio = compra.Precio });
            ResetForm();

            OpcionesRecargaVisible = true;
            SelectedItem = null;
            PrecioSelected = null;
            OpcionesCargandoVisible = false;
            ListaPrecios = false;
        }


        private ICommand _CleanCommand;
        public ICommand CleanCommand
        {
            get
            {
                if (_CleanCommand == null)
                {
                    _CleanCommand = new RelayCommand(CleanExecute, CanSubmitExecute);
                }
                return _CleanCommand;
            }
        }

        public void CleanExecute(object parameter)
        {
            ResetForm();
        }

        public async Task<float> CalcularPrecioMovil(float monto)
        {
            await new ERecMovilConfig().GetPorcientoRecargaMovil();
            var comision = (float)decimal.Parse(_Global.RecMovilConfig.Porciento.ToString(), NumberStyles.Currency, CultureInfo.GetCultureInfo("en-US"));
            var T = monto / 100 * comision;
            //var T = monto + porciento;
            return (float)decimal.Round(Convert.ToDecimal(T), 2);

        }

        public void ResetForm()
        {
            SelectedItem = null;
            txtMonto = string.Empty;
            txtNauta = string.Empty;
            txtNumero = string.Empty;
            Prefijo = string.Empty;
            NautaSelected = false;
            MovilSelected = true;
            TipoProducto = "movil";
            CargarFotoPais();
        }

    }
}
