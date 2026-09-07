using System;
using System.Data;
using Npgsql;

namespace DataAccessLayer
{
    public class SettingDataAccessConnectionString
    {
        //for API 
        // public static string ConnectionString =
        //     "Host=postgres;Port=5432;Database=userprofiledb;Username=postgres;Password=postgres";

        //for manual test
        public static string ConnectionString =
            "Host=localhost;Port=5433;Database=userprofiledb;Username=postgres;Password=postgres";

    }
}