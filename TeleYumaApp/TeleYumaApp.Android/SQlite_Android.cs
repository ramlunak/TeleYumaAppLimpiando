using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Xamarin.Forms;
using TeleYumaApp.Droid;
using SQLite;

[assembly: Dependency(typeof(SQlite_Android))]

namespace TeleYumaApp.Droid
{
    public class SQlite_Android : ISQLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var filename = "teleyuma.db3";
            var docpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(docpath, filename);
            var connection = new SQLiteAsyncConnection(path,true);
            return connection;
        }
    }
   
}