using System;
using System.Collections.Generic;
using Npgsql;
using DataAccessLayerSetting;

namespace DataAccesLayer
{
    public class LevelLanguageDTO
    {
        public long Id { get; set; }
        public string? LevelName { get; set; }

        public LevelLanguageDTO(long id, string? levelName)
        {
            Id = id;
            LevelName = levelName;
        }
    }

    public class LevelLanguage
    {
        public static bool GetLevelLanguageById(
            long id,
            out LevelLanguageDTO? levelLanguage)
        {
            levelLanguage = null;

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT id, ""lavel_Name""
                    FROM ""lavel_languge""
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long levelId = reader.GetInt64(
                                reader.GetOrdinal("id"));

                            string levelName = reader.GetString(
                                reader.GetOrdinal("lavel_Name"));

                            levelLanguage = new LevelLanguageDTO(
                                levelId,
                                levelName);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool GetLevelLanguageByName(
            string? levelName,
            out LevelLanguageDTO? levelLanguage)
        {
            levelLanguage = null;

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT id, ""lavel_Name""
                    FROM ""lavel_languge""
                    WHERE ""lavel_Name"" = @levelName;
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@levelName",
                        levelName ?? (object)DBNull.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long levelId = reader.GetInt64(
                                reader.GetOrdinal("id"));

                            string name = reader.GetString(
                                reader.GetOrdinal("lavel_Name"));

                            levelLanguage = new LevelLanguageDTO(
                                levelId,
                                name);

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool GetAllLevelLanguages(
            out List<LevelLanguageDTO> levelLanguages)
        {
            levelLanguages = new List<LevelLanguageDTO>();

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT id, ""lavel_Name""
                    FROM ""lavel_languge"";
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long levelId = reader.GetInt64(
                                reader.GetOrdinal("id"));

                            string levelName = reader.GetString(
                                reader.GetOrdinal("lavel_Name"));

                            LevelLanguageDTO levelLanguage =
                                new LevelLanguageDTO(
                                    levelId,
                                    levelName);

                            levelLanguages.Add(levelLanguage);
                        }
                    }
                }
            }

            return levelLanguages.Count > 0;
        }

        public static long AddNewLevelLanguage(string levelName)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO ""lavel_languge"" (""lavel_Name"")
                    VALUES (@levelName)
                    RETURNING id;
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue(
                            "@levelName",
                            levelName);

                        long newId = Convert.ToInt64(
                            command.ExecuteScalar());

                        return newId;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Error adding new level language: {ex.Message}");

                        return -1;
                    }
                }
            }
        }

        public static bool IsExistingLevelLanguage(
            string levelName)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT COUNT(*)
                    FROM ""lavel_languge""
                    WHERE ""lavel_Name"" = @levelName;
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@levelName",
                        levelName);

                    long count = Convert.ToInt64(
                        command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        public static bool UpdateLevelLanguage(
            long id,
            string levelName)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    UPDATE ""lavel_languge""
                    SET ""lavel_Name"" = @levelName
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@id",
                        id);

                    command.Parameters.AddWithValue(
                        "@levelName",
                        levelName);

                    int rowsAffected =
                        command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }

      
    }
}