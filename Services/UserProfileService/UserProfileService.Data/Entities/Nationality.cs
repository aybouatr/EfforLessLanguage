using System;
using System.Collections.Generic;
using Npgsql;
using DataAccessLayerSetting;

namespace DataAccesLayer
{
    public class NationalityDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }

        public NationalityDTO(
            long id,
            string? name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Nationality
    {
        public static bool GetNationalityById(long id,out NationalityDTO? nationality)
        {
            nationality = null;

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT
                        id,
                        name_country
                    FROM nationality
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue(
                        "@id",
                        id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long nationalityId =
                                reader.GetInt64(
                                    reader.GetOrdinal("id"));

                            string name =
                                reader.GetString(
                                    reader.GetOrdinal("name_country"));

                            nationality =
                                new NationalityDTO(
                                    nationalityId,
                                    name);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool GetNationalityByName(string name,out NationalityDTO? nationality)
        {
            nationality = null;

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT
                        id,
                        name_country
                    FROM nationality
                    WHERE name_country = @name;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue(
                        "@name",
                        name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long nationalityId =
                                reader.GetInt64(
                                    reader.GetOrdinal("id"));

                            string nationalityName =
                                reader.GetString(
                                    reader.GetOrdinal("name_country"));

                            nationality =
                                new NationalityDTO(
                                    nationalityId,
                                    nationalityName);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool GetAllNationalities(out List<NationalityDTO> nationalities)
        {
            nationalities =
                new List<NationalityDTO>();

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT
                        id,
                        name_country
                    FROM nationality;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long nationalityId =
                                reader.GetInt64(
                                    reader.GetOrdinal("id"));

                            string name =
                                reader.GetString(
                                    reader.GetOrdinal("name_country"));

                            NationalityDTO nationality =
                                new NationalityDTO(
                                    nationalityId,
                                    name);

                            nationalities.Add(
                                nationality);
                        }
                    }
                }
            }

            return nationalities.Count > 0;
        }

        public static long AddNewNationality( string name)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO nationality
                    (
                        name_country
                    )
                    VALUES
                    (
                        @name
                    )
                    RETURNING id;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue(
                            "@name",
                            name);

                        long newId =
                            Convert.ToInt64(
                                command.ExecuteScalar());

                        return newId;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Error adding nationality: {ex.Message}");

                        return -1;
                    }
                }
            }
        }

        public static bool IsExistingNationality( string name)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT COUNT(*)
                    FROM nationality
                    WHERE name_country = @name;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue(
                        "@name",
                        name);

                    long count =
                        Convert.ToInt64(
                            command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public static bool UpdateNationality(long id,string name)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    UPDATE nationality
                    SET
                        name_country = @name
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue(
                        "@id",
                        id);

                    command.Parameters.AddWithValue(
                        "@name",
                        name);

                    int rowsAffected =
                        command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

        public static bool DeleteNationalityById(long id)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    DELETE FROM nationality
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue(
                            "@id",
                            id);

                        int rowsAffected =
                            command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                    catch (PostgresException ex)
                        when (ex.SqlState == "23503")
                    {
                        Console.WriteLine(
                            "Cannot delete this nationality because it is used by another table.");

                        return false;
                    }
                }
            }
        }
    
    
    }//id name_country
}