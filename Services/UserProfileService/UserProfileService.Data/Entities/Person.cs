using System;
using System.Data;
using DataAccessLayer;

namespace DataAccesLayer
{
    
    public class PersonDTO
    {
        public int person_id { get; set; }

        public int IdIdentity { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Nationality { get; set; }

        public PersonDTO( int person_id, int IdIdentity,string firstName, string lastName, string email, DateTime dateOfBirth, string phoneNumber,       string       nationality)
        {
            this.person_id = person_id;
            this.IdIdentity = IdIdentity;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.DateOfBirth = dateOfBirth;
            this.PhoneNumber = phoneNumber;
            this.Nationality = nationality;
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
                    // //person_id Id Identity FirstName Last Name birthday email phone_number nationality 
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person = new PersonDTO(
                                reader.GetInt32(reader.GetOrdinal("IdPerson")),
                                reader.GetInt32(reader.GetOrdinal("IdIdentity")),
                                reader.GetString(reader.GetOrdinal("FirstName")),
                                reader.GetString(reader.GetOrdinal("LastName")),
                                reader.GetString(reader.GetOrdinal("email")),
                                reader.GetDateTime(reader.GetOrdinal("date_of_birth")),
                                reader.GetString(reader.GetOrdinal("phone_number")),
                                reader.GetString(reader.GetOrdinal("nationality"))
                            );
                            return true;
                        }
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
    }
}

