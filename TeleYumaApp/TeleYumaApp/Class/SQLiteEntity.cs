using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;
using System.Globalization;

namespace TeleYumaApp.Class
{
    public class SQ_Login
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int i_account { get; set; }
        public string user { get; set; }
        public string phone1 { get; set; }
        public byte[] foto { get; set; }
        public string password { get; set; }
        public bool isloged { get; set; }

        private SQLiteAsyncConnection _connection
        {
            get
            {
                var con = DependencyService.Get<ISQLiteDB>().GetConnection();
                con.CreateTableAsync<SQ_Login>();
                return con;
            }
        }

        public void Ingresar()
        {
            var registros = _connection.Table<SQ_Login>().ToListAsync().Result;
            if (registros.Count == 0)
            {
                _connection.InsertAsync(this);
                return;
            }

            var exist = registros.Where(x => x.phone1 == this.phone1).ToList();
            if (exist.Any())
            {
                var user = exist.First();
                user.isloged = true;
                _connection.UpdateAsync(user);
            }
            else
            {
                _connection.InsertAsync(this);
                return;
            }

        }

        public bool SetPhotoPerfil()
        {

            try
            {
                _connection.UpdateAsync(this);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public SQ_Login GetInfoLogin()
        {
            var registros = _connection.Table<SQ_Login>().ToListAsync().Result;
            if (registros.Count > 0)
            {
                var l = registros.Where(x => x.isloged);

                if (l.Any())
                {
                    return _Global.SQLiteLogin = l.First();                   
                }
                else return null;                
            }

            else return null;
        }

        public void DeleteLogin()
        {
            var registros = _connection.Table<SQ_Login>().ToListAsync().Result;
            if (registros.Count > 0)
            {
                var log = (from login in registros select login).ToList().First();
                _connection.DeleteAsync(log);
            }
        }

        public void Salir()
        {
            var registros = _connection.Table<SQ_Login>().ToListAsync().Result;
            if (registros.Count > 0)
            {
                foreach (var item in registros)
                {
                    item.isloged = false;
                    _connection.UpdateAsync(item);
                }

            }
            _Global.SQLiteLogin = new SQ_Login();
        }

    }

    public class SQ_Recarga
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int i_account { get; set; }
        public string producto { get; set; }
        public string numero { get; set; }
        public string monto { get; set; }
        public DateTime hora { get; set; }
        public EstadoReserva estado { get; set; }
        public string error { get; set; }

        private SQLiteAsyncConnection _connection
        {
            get
            {
                var con = DependencyService.Get<ISQLiteDB>().GetConnection();
                con.CreateTableAsync<SQ_Recarga>();
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


        public List<SQ_Recarga> GetAll()
        {
            return _connection.Table<SQ_Recarga>().ToListAsync().Result;
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

    }


}
