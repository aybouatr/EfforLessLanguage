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

    public class Interests
    {

        public static bool GetInterestByName(string? name, out InterestsDTO? Interest)
        {
            Interest = null;

            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                                    SELECT *
                                    FROM ""interests""
                                    WHERE interest = @name;
                                ";
                                   

                using (var command = new Npgsql.NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name ?? (object)DBNull.Value);
                    // id name
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long InterestID = reader.GetInt64(
                                reader.GetOrdinal("id"));
                            
                            long value_to_extract = reader.GetInt64(
                                reader.GetOrdinal("value_to_extract"));

                            string InterestName = reader.GetString(
                                reader.GetOrdinal("interest_name"));

                            Interest = new InterestsDTO(
                                InterestID,
                                value_to_extract,
                                InterestName
                                );

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool GetAllInterests(out List<InterestsDTO> interests)
        {
            interests = new List<InterestsDTO>();

            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                                    SELECT *
                                    FROM ""interests"";
                                ";

                using (var command = new Npgsql.NpgsqlCommand(query, connection))
                {
                    // id name
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long interestID = reader.GetInt64(
                                reader.GetOrdinal("id"));

                            long value_to_extract = reader.GetInt64(
                                reader.GetOrdinal("value_to_extract"));

                            string interestName = reader.GetString(
                                reader.GetOrdinal("interest_name"));

                            InterestsDTO Interest = new InterestsDTO(
                                interestID,
                                value_to_extract,
                                interestName
                                );

                            interests.Add(Interest);
                        }
                    }
                }
            }

            return interests.Count > 0;
        }

        public static long AddNewInetrest(string? name)
        {
            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO interests (interest_name, value_to_extract)
                    VALUES (
                        @InterestName,
                        COALESCE(
                            (SELECT MAX(value_to_extract) * 2 FROM interests),
                            1
                        )
                    );
                ";

                using (var command = new Npgsql.NpgsqlCommand(query, connection))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@InterestName", name);
                        long newId = Convert.ToInt64(command.ExecuteScalar());
                        
                        return  newId;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding new language: {ex.Message}");
                        return -1; // Indicate failure
                    }
                }
            }
        }
    
    }

}