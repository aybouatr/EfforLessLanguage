using System;
using System.Collections.Generic;
using Npgsql;
using DataAccessLayerSetting;

namespace DataAccesLayer
{
    public class InfoTargetLanguageDTO
    {
        public long Id { get; set; }
        public LevelLanguageDTO? Level { get; set; }
        public LanguageDTO? Language { get; set; }

        public InfoTargetLanguageDTO(
            long id,
            LevelLanguageDTO? level,
            LanguageDTO? language)
        {
            Id = id;
            Level = level;
            Language = language;
        }
    }

    public class InfoTargetLanguage
    {
        public static bool GetInfoTargetLanguageById(long id,out InfoTargetLanguageDTO? infoTargetLanguage)
        {
            infoTargetLanguage = null;

            long levelId;
            long languageId;
            long infoId;

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT
                        id,
                        ""FK_Languge"",
                        ""FK_lavel_languge""
                    FROM ""info_Target_languge""
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return false;
                        }

                        infoId = reader.GetInt64(
                            reader.GetOrdinal("id"));

                        languageId = reader.GetInt64(
                            reader.GetOrdinal("FK_Languge"));

                        levelId = reader.GetInt64(
                            reader.GetOrdinal("FK_lavel_languge"));
                    }
                }
            }

            LevelLanguageDTO? level;
            LanguageDTO? language;

            bool levelFound =
                LevelLanguage.GetLevelLanguageById(
                    levelId,
                    out level);

            bool languageFound = Language.GetLanguageById(languageId, out language);
            if (!levelFound || !languageFound)
            {
                return false;
            }

            infoTargetLanguage =
                new InfoTargetLanguageDTO(
                    infoId,
                    level,
                    language);

            return true;
        }
        public static bool GetAllInfoTargetLanguages( out List<InfoTargetLanguageDTO> infoTargetLanguages)
        {
            infoTargetLanguages =
                new List<InfoTargetLanguageDTO>();

            List<(long Id, long LanguageId, long LevelId)> rows =
                new List<(long, long, long)>();

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT
                        id,
                        ""FK_Languge"",
                        ""FK_lavel_languge""
                    FROM ""info_Target_languge"";
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = reader.GetInt64(
                                reader.GetOrdinal("id"));

                            long languageId = reader.GetInt64(
                                reader.GetOrdinal("FK_Languge"));

                            long levelId = reader.GetInt64(
                                reader.GetOrdinal("FK_lavel_languge"));

                            rows.Add(
                                (id, languageId, levelId));
                        }
                    }
                }
            }

            foreach (var row in rows)
            {
                LevelLanguageDTO? level;
                LanguageDTO? language;

                bool levelFound =
                    LevelLanguage.GetLevelLanguageById(
                        row.LevelId,
                        out level);

                bool languageFound =
                    Language.GetLanguageById(
                        row.LanguageId,
                        out language);

                if (!levelFound || !languageFound)
                {
                    continue;
                }

                InfoTargetLanguageDTO info =
                    new InfoTargetLanguageDTO(
                        row.Id,
                        level,
                        language);

                infoTargetLanguages.Add(info);
            }

            return infoTargetLanguages.Count > 0;
        }
        public static long AddNewInfoTargetLanguage(long languageId,long levelId)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO ""info_Target_languge""
                    (
                        ""FK_Languge"",
                        ""FK_lavel_languge""
                    )
                    VALUES
                    (
                        @languageId,
                        @levelId
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
                            "@languageId",
                            languageId);

                        command.Parameters.AddWithValue(
                            "@levelId",
                            levelId);

                        long newId = Convert.ToInt64(
                            command.ExecuteScalar());

                        return newId;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"Error adding info target language: {ex.Message}");

                        return -1;
                    }
                }
            }
        }
        public static long AddNewInfoTargetLanguage(InfoTargetLanguageDTO infoTargetLanguage,NpgsqlTransaction transaction,NpgsqlConnection connection)
        {
            string query = @"
                INSERT INTO ""info_Target_languge""
                (
                    ""FK_Languge"",
                    ""FK_lavel_languge""
                )
                VALUES
                (
                    @languageId,
                    @levelId
                )
                RETURNING id;
            ";

            using (var command = new NpgsqlCommand(
                query,
                connection,
                transaction))
            {
                try
                {
                    command.Parameters.AddWithValue(
                        "@languageId",
                        infoTargetLanguage.Language?.Id ?? throw new ArgumentNullException("Language ID cannot be null"));

                    command.Parameters.AddWithValue(
                        "@levelId",
                        infoTargetLanguage.Level?.Id ?? throw new ArgumentNullException("Level ID cannot be null"));

                    long newId = Convert.ToInt64(
                        command.ExecuteScalar());

                    return newId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error adding info target language: {ex.Message}");

                    return -1;
                }
            }
        }
        public static bool IsExistingInfoTargetLanguage( long languageId,long levelId)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT COUNT(*)
                    FROM ""info_Target_languge""
                    WHERE ""FK_Languge"" = @languageId
                      AND ""FK_lavel_languge"" = @levelId;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue(
                        "@languageId",
                        languageId);

                    command.Parameters.AddWithValue(
                        "@levelId",
                        levelId);

                    long count = Convert.ToInt64(
                        command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        public static bool UpdateInfoTargetLanguage( long id,long languageId, long levelId)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    UPDATE ""info_Target_languge""
                    SET
                        ""FK_Languge"" = @languageId,
                        ""FK_lavel_languge"" = @levelId
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
                        "@languageId",
                        languageId);

                    command.Parameters.AddWithValue(
                        "@levelId",
                        levelId);

                    int rowsAffected =
                        command.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
        }
        public static bool UpdateInfoTargetLanguage(InfoTargetLanguageDTO infoTargetLanguage,NpgsqlTransaction transaction,NpgsqlConnection connection)
        {
            string query = @"
                UPDATE ""info_Target_languge""
                SET
                    ""FK_Languge"" = @languageId,
                    ""FK_lavel_languge"" = @levelId
                WHERE id = @id;
            ";

            using (var command = new NpgsqlCommand(
                query,
                connection,
                transaction))
            {
                command.Parameters.AddWithValue(
                    "@id",
                    infoTargetLanguage.Id);

                command.Parameters.AddWithValue(
                    "@languageId",
                    infoTargetLanguage.Language?.Id ?? throw new ArgumentNullException("Language ID cannot be null"));

                command.Parameters.AddWithValue(
                    "@levelId",
                    infoTargetLanguage.Level?.Id ?? throw new ArgumentNullException("Level ID cannot be null"));

                int rowsAffected =
                    command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public static bool DeleteInfoTargetLanguageById(long id)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    DELETE FROM ""info_Target_languge""
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
                            "Cannot delete this record because it is used by another table.");

                        return false;
                    }
                }
            }
        }
    
    }
}