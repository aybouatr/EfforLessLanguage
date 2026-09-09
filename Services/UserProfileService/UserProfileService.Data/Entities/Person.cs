using System;
using System.Data;
using DataAccessLayerSetting;
using Npgsql;

namespace DataAccesLayer
{
    
    public class PersonDTO
    {

        // id FirstName Last Name birthday native_language email phone_number fr_nationality
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public LanguageDTO? Native_Language { get; set; }
        public string PhoneNumber { get; set; }
        public NationalityDTO? Nationality { get; set; }

        public PersonDTO(int person_id, string firstName, string lastName, LanguageDTO nativeLanguage, string email, DateTime dateOfBirth, NationalityDTO nationality, string phoneNumber)
        {
                if (nativeLanguage == null)
                {
                    throw new ArgumentNullException(nameof(nativeLanguage), "Native language cannot be null.");
                }
                if (nationality == null)
                {
                    throw new ArgumentNullException(nameof(nationality), "Nationality cannot be null.");
                }

                this.id = person_id;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.Native_Language = nativeLanguage;
                this.Email = email;
                this.DateOfBirth = dateOfBirth;
                this.Nationality = nationality;
                this.PhoneNumber = phoneNumber;
        }
    
    }
   
    public class Person
    {

        public static bool GetPersonById(int id, out PersonDTO? person)
        {
            person = null;

            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = new Npgsql.NpgsqlCommand("SELECT * FROM person WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    // id FirstName Last Name birthday native_language email phone_number fr_nationality
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int personId = reader.GetInt32(reader.GetOrdinal("id"));
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("Last Name"));
                            string email = reader.GetString(reader.GetOrdinal("email"));
                            DateTime dateOfBirth = reader.GetDateTime(reader.GetOrdinal("birthday"));
                            string phoneNumber = reader.GetString(reader.GetOrdinal("phone_number"));

                            int nativeLanguageId = reader.GetInt32(reader.GetOrdinal("native_language"));
                             Language.GetLanguageById(nativeLanguageId, out LanguageDTO? nativeLanguage);

                            int nationalityId = reader.GetInt32(reader.GetOrdinal("fr_nationality"));
                             Nationality.GetNationalityById(nationalityId, out NationalityDTO? nationality);

                            person = new PersonDTO(personId, firstName, lastName, nativeLanguage!, email, dateOfBirth, nationality!, phoneNumber);
                            if (person != null)
                            {
                                return true;
                            }
                            
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsExistingPerson(int id)
        {
            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM person WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count > 0)
                    {
                        return true;
                    }
                }
            } 
            return false;
        }
        
        public static long? AddNewPerson(PersonDTO person, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {   
            string query = @"
                INSERT INTO person
                (
                    ""FirstName"",
                    ""Last Name"",
                    birthday,
                    native_language,
                    email,
                    phone_number,
                    fr_nationality
                )
                VALUES
                (
                    @firstName,
                    @lastName,
                    @dateOfBirth,
                    @nativeLanguage,
                    @email,
                    @phoneNumber,
                    @nationality
                )
                RETURNING id;
            ";
            using (var command = new Npgsql.NpgsqlCommand(
                query,
                connection,
                transaction))
            {
                command.Parameters.AddWithValue("@firstName", person.FirstName);
                command.Parameters.AddWithValue("@lastName", person.LastName);
                command.Parameters.AddWithValue("@dateOfBirth", person.DateOfBirth);
                command.Parameters.AddWithValue(
                    "@nativeLanguage",
                    person.Native_Language?.Id
                        ?? throw new InvalidOperationException(
                            "Native language cannot be null."));
                command.Parameters.AddWithValue("@email", person.Email);
                command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                command.Parameters.AddWithValue(
                    "@nationality",
                    person.Nationality?.Id
                        ?? throw new InvalidOperationException(
                            "Nationality cannot be null."));
                object? scalar = command.ExecuteScalar();

                return scalar is null || scalar == DBNull.Value ? null : Convert.ToInt64(scalar);
            }
        }
    
        public static bool UpdatePerson(PersonDTO person,NpgsqlTransaction transaction,NpgsqlConnection connection)
        {
            string query = @"
                UPDATE person
                SET
                    ""FirstName"" = @firstName,
                    ""Last Name"" = @lastName,
                    birthday = @dateOfBirth,
                    native_language = @nativeLanguage,
                    email = @email,
                    phone_number = @phoneNumber,
                    fr_nationality = @nationality
                WHERE id = @id;
            ";

            using (var command = new Npgsql.NpgsqlCommand(query,connection,transaction))
            {
                command.Parameters.AddWithValue("@id", person.id);

                command.Parameters.AddWithValue("@firstName", person.FirstName);
                command.Parameters.AddWithValue("@lastName", person.LastName);
                command.Parameters.AddWithValue("@dateOfBirth", person.DateOfBirth);

                command.Parameters.AddWithValue(
                    "@nativeLanguage",
                    person.Native_Language?.Id
                        ?? throw new InvalidOperationException(
                            "Native language cannot be null."));

                command.Parameters.AddWithValue("@email", person.Email);
                command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);

                command.Parameters.AddWithValue(
                    "@nationality",
                    person.Nationality?.Id
                        ?? throw new InvalidOperationException(
                            "Nationality cannot be null."));

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }


    }

}


