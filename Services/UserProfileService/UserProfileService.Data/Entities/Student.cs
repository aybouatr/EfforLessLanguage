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
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long Nationality { get; set; }
        public long FK_Person { get; set; }


        public StudentDTO(
            long id,
            string firstName,
            string lastName,
            string passWord,
            DateTime birthday,
            DateTime joinDate,
            long interest,
            string language,
            string levelName,
            string email,
            string phoneNumber,
            long nationality,
            long fk_person )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
            JoinDate = joinDate;
            Interest = interest;
            PassWord = passWord;
            Language = language;
            LevelLanguage = levelName;
            Email = email;
            PhoneNumber = phoneNumber;
            Nationality = nationality;
            FK_Person = fk_person;
        }
    }

    public class Student
    {
      
       public static bool GetStudentById(long id, out StudentDTO? student)
{
    student = null;

    try
    {
        using var connection = new NpgsqlConnection(
            SettingDataAccessConnectionString.ConnectionString);

        string query = @"
            SELECT 
                s.id,
                p.""FirstName"",
                p.""Last Name"",
                p.birthday,
                s.join_date,
                s.password,
                s.interest,
                l.language,
                ll.""lavel_Name"",
                p.email,
                p.phone_number,
                p.fr_nationality,
                p.id AS FK_person
            FROM ""student"" s
            JOIN ""person"" p
                ON s.""FK_person"" = p.id
            JOIN ""info_Target_languge"" t
                ON s.""FK_info_Target_languge"" = t.id
            JOIN ""languages"" l
                ON l.id = t.""FK_Languge""
            JOIN ""lavel_languge"" ll
                ON ll.id = t.""FK_lavel_languge""
            WHERE p.id = @id;
        ";

        connection.Open();

        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            student = new StudentDTO(
                reader.GetInt64(
                    reader.GetOrdinal("id")),

                reader.GetString(
                    reader.GetOrdinal("FirstName")),

                reader.GetString(
                    reader.GetOrdinal("Last Name")),

                reader.GetString(
                    reader.GetOrdinal("password")),

                reader.GetDateTime(
                    reader.GetOrdinal("birthday")),

                reader.GetDateTime(
                    reader.GetOrdinal("join_date")),

                reader.GetInt64(
                    reader.GetOrdinal("interest")),

                reader.GetString(
                    reader.GetOrdinal("language")),

                reader.GetString(
                    reader.GetOrdinal("lavel_Name")),

                reader.GetString(
                    reader.GetOrdinal("email")),

                reader.GetString(
                    reader.GetOrdinal("phone_number")),

                reader.GetInt64(
                    reader.GetOrdinal("fr_nationality")),

                reader.GetInt64(
                    reader.GetOrdinal("FK_person"))
            );

            return true;
        }

        return false;
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            $"Error getting student by ID: {ex.Message}");

        return false;
    }
}

        public static bool UpdateStudent(StudentDTO student)
        {
            if (student == null || IsExistingStudent(student.Id) == false)
                return false;

            try
            {
                using (var connection = new NpgsqlConnection(
                    SettingDataAccessConnectionString.ConnectionString))
                {
                    connection.Open();

                    using (NpgsqlTransaction transaction = connection.BeginTransaction())
                    {
                        string queryPerson = @"
                            UPDATE ""person""
                            SET
                                ""FirstName"" = @firstName,
                                ""Last Name"" = @lastName,
                                ""email"" = @email,
                                ""date_of_birth"" = @dateOfBirth
                            WHERE id = @id;
                        ";

                        if (Person.UpdatePerson(
                            transaction,
                            connection,
                            queryPerson,
                            student.FirstName,
                            student.LastName,
                            student.Email,
                            student.Birthday,
                            student.Id) == false)
                        {
                            transaction.Rollback();
                            return false;
                        }


                        // Update student information
                        string queryStudent = @"
                            UPDATE ""student""
                            SET
                                ""interest"" = @st,
                                ""password"" = @passWord,
                                ""FK_info_Target_languge"" = @targetLanguageId
                            WHERE id = @id;
                        ";

                        using (var command = new NpgsqlCommand(
                            queryStudent,
                            connection,
                            transaction))
                        {
                            command.Parameters.AddWithValue(
                                "@st",
                                student.Interest);

                            command.Parameters.AddWithValue(
                                "@passWord",
                                student.PassWord);

                            command.Parameters.AddWithValue(
                                "@targetLanguageId",
                                student.Language);

                            command.Parameters.AddWithValue(
                                "@id",
                                student.Id);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                return false;
                            }
                        }

                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error updating student: {ex}");

                return false;
            }
        }

        public static bool AddNewStudent(StudentDTO student)
        {

            return true;
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

        public static bool DeleteStudentById(long id)
        {

            

            return true;
            
        }

        public static bool GetAllStudents(out List<StudentDTO> students)
        {
            students = new List<StudentDTO>();

            try
            {
                using (var connection = new NpgsqlConnection(
                    SettingDataAccessConnectionString.ConnectionString))
                {
                    string query = @"
                        SELECT 
                            s.id,
                            p.""FirstName"",
                            p.""Last Name"",
                            s.password,
                            p.birthday,
                            s.join_date,
                            s.interest,
                            l.language,
                            ll.""lavel_Name"",
                            p.email,
                            p.phone_number,
                            p.fr_nationality,
                            p.id AS FK_person
                        FROM ""student"" s
                        JOIN ""person"" p
                            ON s.""FK_person"" = p.id
                        JOIN ""info_Target_languge"" T
                            ON s.""FK_info_Target_languge"" = T.id
                        JOIN ""languages"" l
                            ON l.id = T.""FK_Languge""
                        JOIN ""lavel_languge"" ll
                            ON ll.id = T.""FK_lavel_languge"";
                    ";

                    connection.Open();

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var student = new StudentDTO(
                                reader.GetInt64(
                                    reader.GetOrdinal("id")),

                                reader.GetString(
                                    reader.GetOrdinal("FirstName")),

                                reader.GetString(
                                    reader.GetOrdinal("Last Name")),
                                
                                reader.GetString(
                                    reader.GetOrdinal("password")),

                                reader.GetDateTime(
                                    reader.GetOrdinal("birthday")),

                                reader.GetDateTime(
                                    reader.GetOrdinal("join_date")),

                                reader.GetInt64(
                                    reader.GetOrdinal("interest")),

                                reader.GetString(
                                    reader.GetOrdinal("language")),

                                reader.GetString(
                                    reader.GetOrdinal("lavel_Name")),

                                reader.GetString(
                                    reader.GetOrdinal("email")),

                                reader.GetString(
                                    reader.GetOrdinal("phone_number")),

                                reader.GetInt64(
                                    reader.GetOrdinal("fr_nationality")),
                                 reader.GetInt64(
                                    reader.GetOrdinal("FK_person"))
                            );

                            students.Add(student);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting students: {ex.Message}");
                return false;
            }
        }
       
    
    }
}