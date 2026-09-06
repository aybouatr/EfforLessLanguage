using System;
using System.Data;
using DataAccessLayer;


namespace DataAccesLayer
{
    public class StudentDTO
    {
        
      
    }   //person_id Id Identity FirstName Last Name birthday email


    public class Student
    {
        public static bool GetStudentById(int id, out StudentDTO student)
        {
            student = null;

            using (var connection = new Npgsql.NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = new Npgsql.NpgsqlCommand("SELECT * FROM students WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new StudentDTO(
                                // Map the fields from the database to the StudentDTO properties
                            );
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }

}