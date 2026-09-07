using System;
using System.Collections.Generic;
using Npgsql;
using DataAccessLayer;

namespace DataAccesLayer
{
    public class StudentDTO
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime JoinDate { get; set; }
        public long Interest { get; set; }
        public string Language { get; set; }
        public string LevelLanguage { get; set; }

        public StudentDTO(
            long id,
            string firstName,
            string lastName,
            DateTime birthday,
            DateTime joinDate,
            long interest,
            string language,
            string levelName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            JoinDate = joinDate;
            Interest = interest;
            Language = language;
            LevelLanguage = levelName;
        }
    }

    public class Student
    {
      
        public static bool GetStudentById( long id, out StudentDTO? student)
        {
            student = null;

            try
            {
                using (var connection = new NpgsqlConnection(
                    SettingDataAccessConnectionString.ConnectionString))
                {
                    string query = @"
                        SELECT 
                            p.id,
                            p.""FirstName"",
                            p.""Last Name"",
                            p.birthday,
                            s.join_date,
                            s.interest,
                            l.language,
                            ll.""lavel_Name""
                        FROM ""student"" s
                        JOIN ""person"" p
                            ON s.""FK_person"" = p.id
                        JOIN ""info_Target_languge"" T
                            ON s.""FK_info_Target_languge"" = T.id
                        JOIN ""languages"" l
                            ON l.id = T.""FK_Languge""
                        JOIN ""lavel_languge"" ll
                            ON ll.id = T.""FK_lavel_languge""
                        WHERE p.id = @id;
                    ";

                    connection.Open();

                    using (var command = new NpgsqlCommand(
                        query,
                        connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                student = new StudentDTO(
                                    reader.GetInt64(
                                        reader.GetOrdinal("id")),

                                    reader.GetString(
                                        reader.GetOrdinal("FirstName")),

                                    reader.GetString(
                                        reader.GetOrdinal("Last Name")),

                                    reader.GetDateTime(
                                        reader.GetOrdinal("birthday")),

                                    reader.GetDateTime(
                                        reader.GetOrdinal("join_date")),

                                    reader.GetInt64(
                                        reader.GetOrdinal("interest")),

                                    reader.GetString(
                                        reader.GetOrdinal("language")),

                                    reader.GetString(
                                        reader.GetOrdinal("lavel_Name"))
                                );

                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error getting student by ID: {ex.Message}");

                return false;
            }

            return false;
        }

      public static bool UpdateStudent(StudentDTO student)
{
    try
    {
        using (var connection = new NpgsqlConnection(
            SettingDataAccessConnectionString.ConnectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Update person
                    string updatePersonQuery = @"
                        UPDATE ""person""
                        SET 
                            ""FirstName"" = @FirstName,
                            ""Last Name"" = @LastName,
                            birthday = @Birthday
                        WHERE id = @PersonId;
                    ";

                    using (var command = new NpgsqlCommand(
                        updatePersonQuery,
                        connection,
                        transaction))
                    {
                        command.Parameters.AddWithValue(
                            "@FirstName", student.FirstName);

                        command.Parameters.AddWithValue(
                            "@LastName", student.LastName);

                        command.Parameters.AddWithValue(
                            "@Birthday", student.Birthday);

                        command.Parameters.AddWithValue(
                            "@PersonId", student.Id);

                        command.ExecuteNonQuery();
                    }


                    // 2. Update student
                    string updateStudentQuery = @"
                        UPDATE ""student""
                        SET 
                            join_date = @JoinDate,
                            interest = @Interest
                        WHERE ""FK_person"" = @PersonId;
                    ";

                    using (var command = new NpgsqlCommand(
                        updateStudentQuery,
                        connection,
                        transaction))
                    {
                        command.Parameters.AddWithValue(
                            "@JoinDate", student.JoinDate);

                        command.Parameters.AddWithValue(
                            "@Interest", student.Interest);

                        command.Parameters.AddWithValue(
                            "@PersonId", student.Id);

                        command.ExecuteNonQuery();
                    }


                    // 3. If everything succeeded
                    transaction.Commit();

                    return true;
                }
                catch
                {
                    // If anything fails, undo everything
                    transaction.Rollback();

                    return false;
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"Error updating student: {ex.Message}");

        return false;
    }
}

        public static bool IsExistingStudent(long id)
        {
            try
            {
                using (var connection = new NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM ""student"" s
                        JOIN ""person"" p
                            ON s.""FK_person"" = p.id
                        WHERE p.id = @id;
                    ";

                    connection.Open();

                    using (var command = new NpgsqlCommand(
                        query,
                        connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        long? count = (long ?)command.ExecuteScalar();

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error checking if student exists: {ex.Message}");

                return false;
            }
        }

         public static bool IsExistingStudent(string firstName, string lastName, DateTime birthday)
        {
            try
            {
                using (var connection = new NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM ""student"" s
                        JOIN ""person"" p
                            ON s.""FK_person"" = p.id
                        WHERE p.FirstName = @firstName AND p.""Last Name"" = @lastName AND p.birthday = @birthday;
                    ";

                    connection.Open();

                    using (var command = new NpgsqlCommand(
                        query,
                        connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        long count = (long)command.ExecuteScalar();

                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error checking if student exists: {ex.Message}");

                return false;
            }
        }

        public static bool AddStudent(StudentDTO student)
        {   
            using (var connection = new NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                       

                        transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        Console.WriteLine(ex.Message);

                        return false;
                    }
                }
            }
        }
        
        public static bool DeleteStudentById(long id)
        {
            // TODO: implement later
            return true;
        }



    }
}