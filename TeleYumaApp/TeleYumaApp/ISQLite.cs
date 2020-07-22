using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace TeleYumaApp
{
    public interface ISQLiteDB
    {
        SQLiteAsyncConnection GetConnection();
    }
}
