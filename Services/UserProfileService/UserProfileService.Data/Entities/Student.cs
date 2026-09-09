
using System;
using System.Collections.Generic;
using Npgsql;
using DataAccessLayerSetting;

namespace DataAccesLayer
{
    public class StudentDTO
    {
        public long Id { get; set; }

        public DateTime JoinDate { get; set; }

        public PersonDTO? Person { get; set; }

        public long Interest { get; set; }

        public string? PassWord { get; set; }

        public InfoTargetLanguageDTO? infoTargetLanguage { get; set; }

        public StudentDTO(
            long id,
            DateTime joinDate,
            PersonDTO? person,
            long interest,
            string? passWord,
            InfoTargetLanguageDTO? infoTargetLanguage)
        {
            Id = id;
            JoinDate = joinDate;
            Interest = interest;
            PassWord = passWord;
            this.infoTargetLanguage = infoTargetLanguage;
            Person = person;
        }
    }

    public class Student
    {
    public static bool GetStudentById(long id, out StudentDTO? student)
    {

            student = null;

            using (var connection = new NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(
                    "SELECT * FROM student WHERE id = @id",
                    connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long studentId = reader.GetInt64(reader.GetOrdinal("id"));
                            DateTime joinDate = reader.GetDateTime(reader.GetOrdinal("join_date"));
                            long personId = reader.GetInt64(reader.GetOrdinal("FK_person"));
                            long interest = reader.GetInt64(reader.GetOrdinal("interest"));
                            string? password = reader.IsDBNull(reader.GetOrdinal("password")) ? null : reader.GetString(reader.GetOrdinal("password"));
                            long targetLanguageId = reader.GetInt64(reader.GetOrdinal("FK_info_Target_languge"));

                            
                            if (!Person.GetPersonById((int)personId, out PersonDTO? person))
                            {
                                return false; 
                            }

                            if (!InfoTargetLanguage.GetInfoTargetLanguageById((int)targetLanguageId, out InfoTargetLanguageDTO? infoTargetLanguage))
                            {
                                return false; 
                            }

                            student = new StudentDTO(studentId, joinDate, person, interest, password, infoTargetLanguage);
                            if (student != null)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
      
    public static bool GetAllStudents(out List<StudentDTO> students)
        {
            students = new List<StudentDTO>();

            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT id
                    FROM student
                    ORDER BY id;
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        List<long> studentIds = new List<long>();

                        while (reader.Read())
                        {
                            studentIds.Add(
                                reader.GetInt64(
                                    reader.GetOrdinal("id")));
                        }

                        foreach (long studentId in studentIds)
                        {
                            if (GetStudentById(
                                    studentId,
                                    out StudentDTO? student)
                                && student != null)
                            {
                                students.Add(student);
                            }
                        }
                    }
                }
            }

            return students.Count > 0;
        }

    public static bool IsExistingStudent(long id)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT EXISTS(
                        SELECT 1
                        FROM student
                        WHERE id = @id
                    );
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    return Convert.ToBoolean(
                        command.ExecuteScalar());
                }
            }
        }

    public static long? AddNewStudent(StudentDTO student)
    {
        using (var connection = new NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {

                    long personID = Person.AddNewPerson(student.Person ?? throw new InvalidOperationException("Person cannot be null."), transaction, connection)
                        ?? throw new InvalidOperationException("Failed to add new person.");
                    if (personID <= 0)
                    {
                        throw new InvalidOperationException("Failed to add new person.");
                    }

                    long infoTargetLanguageId = InfoTargetLanguage.AddNewInfoTargetLanguage(student.infoTargetLanguage?.Language?.Id ?? throw new InvalidOperationException("Language cannot be null."), student.infoTargetLanguage?.Level?.Id ?? throw new InvalidOperationException("Level cannot be null."));
                    if (infoTargetLanguageId <= 0)
                    {
                        throw new InvalidOperationException("Failed to add new info target language.");
                    }

                    string query = @"
                        INSERT INTO student
                        (
                            join_date,
                            interest,
                            password,
                            ""FK_person"",
                            ""FK_info_Target_languge""
                        )
                        VALUES
                        (
                            @joinDate,
                            @interest,
                            @password,
                            @personId,
                            @targetLanguageId
                        )
                        RETURNING id;
                    ";

                    using (var command = new NpgsqlCommand(query, connection,transaction))
                    {
                        command.Parameters.AddWithValue(
                            "@joinDate",
                            student.JoinDate);

                        command.Parameters.AddWithValue(
                            "@personId",
                            personID);

                        command.Parameters.AddWithValue(
                            "@interest",
                            student.Interest);

                        command.Parameters.AddWithValue(
                            "@password",
                            student.PassWord ?? (object)DBNull.Value);

                        command.Parameters.AddWithValue(
                            "@targetLanguageId",
                            infoTargetLanguageId);
                                

                        object? scalar = command.ExecuteScalar();

                        long? studentId =
                            scalar is null || scalar == DBNull.Value
                                ? null
                                : Convert.ToInt64(scalar);

                        transaction.Commit();

                        return studentId;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }

    public static bool UpdateStudent(StudentDTO student)
    {
        using (var connection = new NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {

                    bool resu = Person.UpdatePerson(student.Person ?? throw new InvalidOperationException("Person cannot be null."), transaction, connection);
                    if (!resu)
                    {
                        throw new InvalidOperationException("Failed to update person.");
                    }

                    bool result = InfoTargetLanguage.UpdateInfoTargetLanguage(student.infoTargetLanguage?.Id ?? throw new InvalidOperationException("Info target language cannot be null."), student.infoTargetLanguage?.Language?.Id ?? throw new InvalidOperationException("Language cannot be null."), student.infoTargetLanguage?.Level?.Id ?? throw new InvalidOperationException("Level cannot be null."));
                    if (!result)
                    {
                        throw new InvalidOperationException("Failed to update info target language.");
                    }

                    string query = @"
                        UPDATE student
                        SET
                            join_date = @joinDate,
                            interest = @interest,
                            password = @password,
                            FK_person = @personId,
                            ""FK_info_Target_languge"" = @targetLanguageId
                        WHERE id = @id;
                    ";

                    using (var command = new NpgsqlCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@id", student.Id);
                        command.Parameters.AddWithValue("@joinDate", student.JoinDate);
                        command.Parameters.AddWithValue("@interest", student.Interest);
                        command.Parameters.AddWithValue("@password", student.PassWord ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@targetLanguageId", student.infoTargetLanguage?.Id ?? throw new InvalidOperationException("Target language cannot be null."));
                        command.Parameters.AddWithValue("@personId", student.Person?.id ?? throw new InvalidOperationException("Person cannot be null."));

                        int rowsAffected = command.ExecuteNonQuery();

                        transaction.Commit();

                        return rowsAffected > 0;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    

        }
    
    public static bool DeleteStudentById(long id)
    {
        return false;
    }

    }
}

