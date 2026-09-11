using System;
using Npgsql;
using System.Data;
using DataAccessLayerSetting;

namespace DataAccesLayer
{

    public class InterestsDTO
    {
        public long? Id { get; set; }
        public string? Name { get; set; }

        public long ValueToExtractInterests { get; set; }

        public InterestsDTO(long id,long  ValueToExtractInterests,string? name)
        {
            this.Id = id;
            this.ValueToExtractInterests = ValueToExtractInterests;
            this.Name = name;
        }

    }

    // public class Interests
    // {
    //     // public static bool GetLanguageById(long id, out LanguageDTO? language)
    //     // {
    //     //     language = null;

    //     //     using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
    //     //     {
    //     //         connection.Open();

    //     //         string query = @"
    //     //                             SELECT *
    //     //                             FROM ""languages""
    //     //                             WHERE id = @id;
    //     //                         ";

    //     //         using (var command = new Npgsql.NpgsqlCommand(query, connection))
    //     //         {
    //     //             command.Parameters.AddWithValue("@id", id);
    //     //             // id name
    //     //             using (var reader = command.ExecuteReader())
    //     //             {
    //     //                 if (reader.Read())
    //     //                 {
    //     //                     long languageId = reader.GetInt64(
    //     //                         reader.GetOrdinal("id"));

    //     //                     string languageName = reader.GetString(
    //     //                         reader.GetOrdinal("language"));

    //     //                     language = new LanguageDTO(
    //     //                         languageId,
    //     //                         languageName);

    //     //                     return true;
    //     //                 }
    //     //             }
    //     //         }
    //     //     }

    //     //     return false;
    //     // }

    //     public static bool GetInterestByName(string? name, out InterestsDTO? Interest)
    //     {
    //         Interest = null;

    //         using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
    //         {
    //             connection.Open();

    //             string query = @"
    //                                 SELECT *
    //                                 FROM ""interests""
    //                                 WHERE interest = @name;
    //                             ";
                                   

    //             using (var command = new Npgsql.NpgsqlCommand(query, connection))
    //             {
    //                 command.Parameters.AddWithValue("@name", name ?? (object)DBNull.Value);
    //                 // id name
    //                 using (var reader = command.ExecuteReader())
    //                 {
    //                     if (reader.Read())
    //                     {
    //                         long InterestID = reader.GetInt64(
    //                             reader.GetOrdinal("id"));
                            
    //                         long value_to_extract = reader.GetInt64(
    //                             reader.GetOrdinal("value_to_extract"));

    //                         string InterestName = reader.GetString(
    //                             reader.GetOrdinal("interest_name"));

    //                         Interest = new InterestsDTO(
    //                             InterestID,
    //                             value_to_extract,
    //                             InterestName
    //                             );

    //                         return true;
    //                     }
    //                 }
    //             }
    //         }

    //         return false;
    //     }

    //     public static bool GetAllLanguages(out List<LanguageDTO> languages)
    //     {
    //         languages = new List<LanguageDTO>();

    //         using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
    //         {
    //             connection.Open();

    //             string query = @"
    //                                 SELECT *
    //                                 FROM ""languages"";
    //                             ";

    //             using (var command = new Npgsql.NpgsqlCommand(query, connection))
    //             {
    //                 // id name
    //                 using (var reader = command.ExecuteReader())
    //                 {
    //                     while (reader.Read())
    //                     {
    //                         long languageId = reader.GetInt64(
    //                             reader.GetOrdinal("id"));

    //                         string languageName = reader.GetString(
    //                             reader.GetOrdinal("language"));

    //                         LanguageDTO language = new LanguageDTO(
    //                             languageId,
    //                             languageName);

    //                         languages.Add(language);
    //                     }
    //                 }
    //             }
    //         }

    //         return languages.Count > 0;
    //     }

    //     public static long AddNewLanguage(string name)
    //     {
    //         using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
    //         {
    //             connection.Open();

    //             string query = @"
    //                         INSERT INTO ""languages"" (""language"")
    //                         VALUES (@name)
    //                         RETURNING id;
    //                     ";

    //             using (var command = new Npgsql.NpgsqlCommand(query, connection))
    //             {
    //                 try
    //                 {
    //                     command.Parameters.AddWithValue("@name", name);
    //                     long newId = Convert.ToInt64(command.ExecuteScalar());

    //                     return newId;
    //                 }
    //                 catch (Exception ex)
    //                 {
    //                     Console.WriteLine($"Error adding new language: {ex.Message}");
    //                     return -1; // Indicate failure
    //                 }
    //             }
    //         }
    //     }
    
    //     public static bool IsExistingLanguage(string name)
    //     {
    //         using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
    //         {
    //             connection.Open();

    //             string query = @"
    //                                 SELECT COUNT(*)
    //                                 FROM ""languages""
    //                                 WHERE language = @name;
    //                             ";

    //             using (var command = new Npgsql.NpgsqlCommand(query, connection))
    //             {
    //                 command.Parameters.AddWithValue("@name", name ?? (object)DBNull.Value);
    //                 long count = Convert.ToInt64(command.ExecuteScalar());
    //                 return count > 0;
    //             }
    //         }
    //     }
    

    // }

}