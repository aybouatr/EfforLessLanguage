using System;
using System.Data;
using DataAccessLayer;
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
        public int FK_Native_Language { get; set; }
        public string PhoneNumber { get; set; }
        public int FK_Nationality { get; set; }

        public PersonDTO( int person_id,string firstName, string lastName,int nativeLanguage ,string email, DateTime dateOfBirth, string phoneNumber,int   nationality)
        {
            this.id = person_id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.FK_Native_Language = nativeLanguage;
            this.DateOfBirth = dateOfBirth;
            this.PhoneNumber = phoneNumber;
            this.FK_Nationality = nationality;
        }
       
    }
   
    public class Person
    {

        public static bool GetPersonById(int id, out PersonDTO person)
        {
            person = null;

            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = new Npgsql.NpgsqlCommand("SELECT * FROM persons WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    // id FirstName Last Name birthday native_language email phone_number fr_nationality
                    using (var reader = command.ExecuteReader())
                    {
                        //  int person_id,string firstName, string lastName,int nativeLanguage ,string email, DateTime dateOfBirth, string phoneNumber,int   nationality
                        if (reader.Read())
                        {
                            person = new PersonDTO(
                                reader.GetInt32(reader.GetOrdinal("id")),
                                reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                reader.GetInt32(reader.GetOrdinal("native_language")),
                                reader.GetString(reader.GetOrdinal("email")),
                                reader.GetDateTime(reader.GetOrdinal("birthday")),
                                reader.GetString(reader.GetOrdinal("phone_number")),
                                reader.GetInt32(reader.GetOrdinal("fr_nationality"))
                            );
                            return true;
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

                using (var command = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM persons WHERE id = @id", connection))
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

        public static bool CreatePerson(PersonDTO person)
        {
            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = new Npgsql.NpgsqlCommand("INSERT INTO persons (first_name, last_name, email, date_of_birth) VALUES (@firstName, @lastName, @email, @dateOfBirth)", connection))
                {
                    command.Parameters.AddWithValue("@firstName", person.FirstName);
                    command.Parameters.AddWithValue("@lastName", person.LastName);
                    command.Parameters.AddWithValue("@email", person.Email);
                    command.Parameters.AddWithValue("@dateOfBirth", person.DateOfBirth);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    
        public static bool UpdatePerson(NpgsqlTransaction transaction,NpgsqlConnection connection,string query  ,string firstName, string lastName, string email, DateTime dateOfBirth, long id)
        {
            using (var command = new Npgsql.NpgsqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public static int AddNewPerson(NpgsqlTransaction transaction,NpgsqlConnection connection,string query  ,string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            using (var command = new Npgsql.NpgsqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@dateOfBirth", dateOfBirth);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    // Retrieve the ID of the newly inserted person
                    command.CommandText = "SELECT LASTVAL()";
                    int newPersonId = Convert.ToInt32(command.ExecuteScalar());
                    return newPersonId;
                }
                else
                {
                    return -1; // Indicate failure to insert
                }
            }
        }

        public static bool DeletePerson(int id)
        {
            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = new Npgsql.NpgsqlCommand("DELETE FROM persons WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public static bool DeletePerson()
        {
            return true;
        }

    }



}


