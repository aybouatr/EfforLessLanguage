using System;
using System.Data;
using Npgsql;

namespace DataAccessLayer
{
    public class SettingDataAccessConnectionString
    {
        public static string ConnectionString = "Host=postgres;Port=5432;Database=userprofiledb;Username=postgres;Password=postgres";
    }
}