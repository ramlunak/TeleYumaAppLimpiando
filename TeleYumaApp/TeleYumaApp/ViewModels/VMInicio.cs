using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using TeleYumaApp.Class;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Windows.Input;


namespace TeleYumaApp.ViewModels
{
    public class Person
    {
        public string PhotoUrl { get; set; }
    }

    public class VMInicio : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        Page CurrentPage => Application.Current.MainPage.Navigation?.NavigationStack?.LastOrDefault() ?? Application.Current.MainPage;

        ImageCircle ImagePerfil { get; set; }


        ObservableCollection<Image> _ImageCarousel { get; set; }

        public ObservableCollection<Image> ImageCarousel
        {
            get { return _ImageCarousel; }
            set { _ImageCarousel = value; OnPropertyChanged(); }
        }


        EPerfil CurrentPerfil { get; set; }
        //CarouselViewControl Carousel { get; set; }
        Image ImgPromo { get; set; }

        public int ItemsCount = 1;


        ObservableCollection<ImageSource> _FileImageSource { get; set; }
        public ObservableCollection<ImageSource> FileImageSource { get { return _FileImageSource; } set { _FileImageSource = value; OnPropertyChanged(); } }


        ObservableCollection<EPromo> _Promos { get; set; }
        public ObservableCollection<EPromo> Promos { get { return _Promos; } set { _Promos = value; OnPropertyChanged(); } }

        private int _PositionIndex;

        public int PositionIndex
        {
            get { return _PositionIndex; }
            set { _PositionIndex = value; OnPropertyChanged(); }
        }


        public VMInicio(ImageCircle fotoPerfil = null, object carousel = null, Image promo = null)
        {

            //Promos = new ObservableCollection<EPromo>();

            //FileImageSource = new ObservableCollection<ImageSource>
            //{
            //    ImageSource.FromUri(new Uri("http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/23c1dd13-333a-459e-9e23-c3784e7cb434/2016-06-02_1049.png"))
            //    ,ImageSource.FromUri(new Uri("http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/23c1dd13-333a-459e-9e23-c3784e7cb434/2016-06-02_1049.png"))
            //    ,ImageSource.FromUri(new Uri("http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/23c1dd13-333a-459e-9e23-c3784e7cb434/2016-06-02_1049.png"))
            //};

            //Device.StartTimer(TimeSpan.FromSeconds(2), (Func<bool>)(() =>
            //{
            //    PositionIndex = (PositionIndex + 1) % FileImageSource.Count;
            //    return true;
            //}));
            //if (carousel is null) return;
            //Carousel = carousel;
            try
            {
                ImgPromo = promo;
                ImagePerfil = fotoPerfil;
                FotoPerfil = "perfil.jpg";
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
            ////var views = new ObservableCollection<View>();
            ////var image = new Image() { Source = "tienda.jpg", Aspect = Aspect.AspectFill };
            ////views.Add(image);
            ////MyItemsSource = views;
            ////StarAnimation = true;
            try
            {

                fullname = "";
                email = "";
                saldo = "Saldo:$0.00 usd";
                ActualizarInformacionCuenta();
                CargarPromociones();
                CargarPerfil();


                //CargarContactos();
                ImgPromoSource = "tienda.jpg";
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
        }


        private ImageSource _ImgPromoSource;

        public ImageSource ImgPromoSource
        {
            get { return _ImgPromoSource; }
            set { _ImgPromoSource = value; OnPropertyChanged(); }
        }


        public async Task CargarPromociones()
        {
            var lista = new List<ImageSource>();
            try
            {
                var promos = await _Global.Promociones.GetPromos();
                               
                foreach (var item in promos)
                {
                    Stream newstream = new MemoryStream(item.image);
                    ImgPromoSource = ImageSource.FromStream(() => { return newstream; });
                    lista.Add(ImgPromoSource);                    
                }
            }
            catch (Exception ex)
            {
               // App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
                     
               
                FileImageSource = new ObservableCollection<ImageSource>();
                FileImageSource = new ObservableCollection<ImageSource>(lista);
                       
            // ItemsCount = promos.AsCollectionView().Count;
            //  MyItemsSource = promos.AsCollectionView();
          
            
             await Task.Delay(10000);
            CargarPromociones();

        }


        public async void CargarContactos()
        {
            try
            {
                _Global.ListaContactos = await EContacto.GetListaContactos();
                _Global.Vistas.ListaContactos.LlenarLista();
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
        }

        public static bool StarAnimation { get; set; }

        private string _PromoActiva;

        public string PromoActiva
        {
            get { return _PromoActiva; }
            set { _PromoActiva = value; }
        }


        public async Task StartCarousel()
        {
            //try
            //{
            //    if (StarAnimation)
            //    {
            //        var Position = (Carousel.Position + 1) % ItemsCount;

            //        Carousel.Position = Position;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ;
            //}

            CargarPromociones();
            await Task.Delay(5000);
            StartCarousel();
        }

        public async Task CargarPerfil()
        {
            try
            {
                var login = _Global.SQLiteLogin.GetInfoLogin();
                // var login = await _Global.Get<EPerfil>("Perfil");
                if (login.foto is null)
                {
                    FotoPerfil = "perfil.jpg";
                    return;
                }

                //try
                //{
                //    var im = DependencyService.Get<IMediaService>().ResizeImage(login.foto, 300, 300);
                //    login.foto = im;
                //}
                //catch (Exception ex)
                //{

                //    ;
                //}

                Stream newstream = new MemoryStream(login.foto);
                ImagePerfil.Source = ImageSource.FromStream(() => { return newstream; });
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
        }

        ObservableCollection<View> _myItemsSource;
        public ObservableCollection<View> MyItemsSource
        {
            set
            {
                _myItemsSource = value;
                OnPropertyChanged();
            }
            get
            {
                return _myItemsSource;
            }
        }

        public Command MyCommand { protected set; get; }

        public void Salir()
        {
            _Global.SQLiteLogin.Salir();
            _Global.RunTask = false;
            Application.Current.MainPage = new NavigationPage(new PagesInicio.Login());
        }

        public async Task ActualizarInformacionCuenta()
        {
            try
            {
                await _Global.CurrentAccount.GetAccountInfo();


                if (_Global.CurrentAccount.blocked == "Y")
                {
                    await CurrentPage.DisplayAlert("TeleYuma", "Su cuenta ha sido bloqueada", "ok");

                    Salir();
                    return;
                }
                if (_Global.CurrentAccount.bill_status != "O" && !string.IsNullOrEmpty(_Global.CurrentAccount.bill_status))
                {
                    await CurrentPage.DisplayAlert("TeleYuma", "Su cuenta ha sido bloqueada", "ok");
                    Salir();
                    return;
                }

                ActualizarDatos();
                await Task.Delay(3000);
                await ActualizarInformacionCuenta();
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
        }

        public void ActualizarDatos()
        {
            try
            {
                if (_Global.CurrentAccount is null) return;
                saldo = _Global.CurrentAccount.balance.ToString();
                fullname = _Global.CurrentAccount.fullname;
                email = _Global.CurrentAccount.email;
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
        }

        private string _saldo { get; set; }
        private string _fullname { get; set; }
        private string _email { get; set; }

        public string saldo
        {
            get
            {
                _saldo = "Saldo: $" + String.Format("{0:#,##0.00}", _Global.CurrentAccount.balance) + " USD";
                return _saldo;
            }
            set
            {
                _saldo = value;
                OnPropertyChanged();
            }
        }

        public string fullname
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                OnPropertyChanged();
            }

        }

        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }


        private string _FotoPerfil;

        public string FotoPerfil
        {
            get { return _FotoPerfil; }
            set { _FotoPerfil = value; OnPropertyChanged(); }
        }


        private bool CanSubmitExecute(object parameter)
        {
            return true;
        }


        private ICommand _FotoPerfilTappedCommand;
        public ICommand FotoPerfilTappedCommand
        {
            get
            {
                if (_FotoPerfilTappedCommand == null)
                {
                    _FotoPerfilTappedCommand = new RelayCommand(FotoPerfilTappedExecute, CanSubmitExecute);
                }
                return _FotoPerfilTappedCommand;
            }
        }

        

        public async void FotoPerfilTappedExecute(object parameter)
        {

            try
            {
                var result = await CurrentPage.DisplayAlert("Teleyuma.", "Desea actualizar la foto de perfil?.", "Si", "No");
                if (!result) return;

                Stream stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();
                if (stream != null)
                {
                    byte[] img = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        img = ms.ToArray();
                    }
                    
                    //var p = new EPerfil
                    //{
                    //    IdCuenta = _Global.CurrentAccount.phone1,
                    //    Foto = img
                    //};

                    //var promo = new EPromo
                    //{

                    //    image = img
                    //};

                    //var fg = _Global.Post<EPromo>("Promo", promo);              
                    //StarAnimation = false;
                    //await  CargarPromociones();
                    //EPerfil perfil;

                    _Global.SQLiteLogin.foto = img;
                    if (_Global.SQLiteLogin.SetPhotoPerfil())
                    {
                       
                       

                        Stream newstream = new MemoryStream(img);
                        ImagePerfil.Source = ImageSource.FromStream(() => { return newstream; });
                    }
                    else
                    {
                        CurrentPage.DisplayAlert("Teleyuma.", "No se pudo actualizar su foto de perfil.", "Ok");
                        return;
                    }

                    //try
                    //{
                    //   // perfil = await _Global.CurrentAccount.PostPerfil(p);
                    //    //perfil = await _Global.Post<EPerfil>("Perfil", p);
                    //}
                    //catch (Exception ex)
                    //{
                    //    CurrentPage.DisplayAlert("Teleyuma.", "No se pudo actualizar su foto de perfil.", "Ok");
                    //    return;
                    //}

                    //if (perfil.Foto is null)
                    //{
                    //    CurrentPage.DisplayAlert("Teleyuma.", "No se pudo actualizar su foto de perfil.", "Ok");
                    //    return;
                    //}

                    //if (perfil != null)
                    //{
                    //    CurrentPerfil = perfil;
                    //    Stream newstream = new MemoryStream(CurrentPerfil.Foto);
                    //    ImagePerfil.Source = ImageSource.FromStream(() => { return newstream; });
                    //}

                }


            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("System", ex.Message, "ok");
            }
        }

    }
}
