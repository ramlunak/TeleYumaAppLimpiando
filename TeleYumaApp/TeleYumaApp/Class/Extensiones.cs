using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;

namespace TeleYumaApp.Class
{
    public static class Extensions
    {
        public static StringContent AsJsonStringContent(this object o)
         => new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");

        public static string AsJson(this object o)
        => JsonConvert.SerializeObject(o);


       public static ImageSource img { get; set; }
        public static ImageSource AsImageSource(this byte[] o)
        {

            MemoryStream ms = new MemoryStream();
            try
            {
                img = ImageSource.FromStream(() => { return ms; });
                return img;
            }
            catch
            {
                // "other code" failed, dispose the stream before throwing out the Exception
                ms.Dispose();
                throw;
            }
                       
        }
               
        public static ObservableCollection<View> AsCollectionView(this List<EPromo> o)
        {
            var views = new ObservableCollection<View>();
            if (o is null) {

                var image = new Image() { Source = "tienda.jpg", Aspect = Aspect.AspectFill };
                views.Add(image);
                return views;
            }

            try
            {
                foreach (var item in o)
                {
                    var d = new FileImageSource();
                   
                    var image = new Image() { Source = item.image.AsImageSource(), Aspect = Aspect.AspectFill };
                    views.Add(image);
                }
            }
            catch (Exception ex)
            {
                var image = new Image() { Source = "tienda.jpg", Aspect = Aspect.AspectFill };
                views.Add(image);
                return views;
            }

            return views;

        }


        static readonly string PasswordHash = "P@@Sw0rd";
        static readonly string SaltKey = "S@LT&KEY";
        static readonly string VIKey = "@1B2c3D4e5F6g7H8";
        public static string Encrypt(this string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(this string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }

}
