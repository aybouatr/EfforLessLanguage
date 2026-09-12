
using System;
using System.Collections.Generic;
using Npgsql;
using DataAccessLayerSetting;
using Microsoft.Extensions.Logging;

namespace DataAccesLayer 
{
    public class StudentDTO
    {
        public long Id { get; set; }

        public DateTime JoinDate { get; set; }

        public PersonDTO? Person { get; set; }

        public long Interest { get; set; }

        public string? PassWord { get; set; }

        public string PathToProfileImage { get; set; } = string.Empty;

        public InfoTargetLanguageDTO? infoTargetLanguage { get; set; }

        public StudentDTO(
            long id,
            DateTime joinDate,
            PersonDTO? person,
            long interest,
            string? passWord,
            string pathToProfileImage,
            InfoTargetLanguageDTO? infoTargetLanguage)
        {
            this.Id = id;
            this.JoinDate = joinDate;
            this.Interest = interest;
            this.PassWord = passWord;   
            this.PathToProfileImage = pathToProfileImage;   
            this.infoTargetLanguage = infoTargetLanguage;
            this.Person = person;
        }
    }

    public class Student
    {
    public static bool GetStudentById(long id, out StudentDTO? student)
{
    student = null;

    using (var connection = new NpgsqlConnection(
        SettingDataAccessConnectionString.ConnectionString))
    {
        connection.Open();

        string query = @"
            SELECT
                s.id AS student_id,
                s.join_date,
                s.interest,
                s.password,
                s.""pathOfImageProfile"",

                p.id AS person_id,
                p.""FirstName"",
                p.""Last Name"",
                p.birthday,
                p.native_language,
                p.email,
                p.phone_number,
                p.fr_nationality,

                itl.id AS info_target_language_id,

                l.id AS language_id,
                l.language,

                lv.id AS level_id,
                lv.""lavel_Name""

            FROM student s

            JOIN person p
                ON s.""FK_person"" = p.id

            JOIN ""info_Target_languge"" itl
                ON s.""FK_info_Target_languge"" = itl.id

            JOIN languages l
                ON itl.""FK_Languge"" = l.id

            JOIN lavel_languge lv
                ON itl.""FK_lavel_languge"" = lv.id

            WHERE s.id = @id;
        ";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // Student
                    long studentId = reader.GetInt64(
                        reader.GetOrdinal("student_id"));

                    DateTime joinDate = reader.GetDateTime(
                        reader.GetOrdinal("join_date"));

                    long interest = reader.GetInt64(
                        reader.GetOrdinal("interest"));

                    string? password = reader.IsDBNull(
                        reader.GetOrdinal("password"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("password"));

                    // Person
                    long personId = reader.GetInt64(
                        reader.GetOrdinal("person_id"));

                    string firstName = reader.GetString(
                        reader.GetOrdinal("FirstName"));

                    string lastName = reader.GetString(
                        reader.GetOrdinal("Last Name"));

                    DateTime birthday = reader.GetDateTime(
                        reader.GetOrdinal("birthday"));
                    // pathOfImageProfile

                    string pathOfImageProfile = reader.IsDBNull(
                        reader.GetOrdinal("pathOfImageProfile"))
                        ? string.Empty
                        : reader.GetString(reader.GetOrdinal("pathOfImageProfile"));
                    
                    long nativeLanguageId = reader.GetInt64(
                        reader.GetOrdinal("native_language"));

                    string? email = reader.IsDBNull(
                        reader.GetOrdinal("email"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("email"));

                    string? phoneNumber = reader.IsDBNull(
                        reader.GetOrdinal("phone_number"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("phone_number"));

                    long nationalityId = reader.GetInt64(
                        reader.GetOrdinal("fr_nationality"));

                    // InfoTargetLanguage
                    long infoTargetLanguageId = reader.GetInt64(
                        reader.GetOrdinal("info_target_language_id"));

                    // Language
                    long languageId = reader.GetInt64(
                        reader.GetOrdinal("language_id"));

                    string languageName = reader.GetString(
                        reader.GetOrdinal("language"));

                    // Level
                    long levelId = reader.GetInt64(
                        reader.GetOrdinal("level_id"));

                    string levelName = reader.GetString(
                        reader.GetOrdinal("lavel_Name"));

                    // Build DTOs
                    LanguageDTO language = new LanguageDTO(
                        languageId,
                        languageName);

                    LevelLanguageDTO level = new LevelLanguageDTO(
                        levelId,
                        levelName);

                    InfoTargetLanguageDTO infoTargetLanguage =
                        new InfoTargetLanguageDTO(
                            infoTargetLanguageId,
                            level,
                            language);
                        
                    NationalityDTO nationality = Nationality.GetNationalityById(nationalityId, out NationalityDTO? nationalityResult) && nationalityResult != null
                        ? nationalityResult
                        : throw new InvalidOperationException($"Nationality with ID {nationalityId} not found.");   


                    PersonDTO person = new PersonDTO(
                        personId,
                        firstName,
                        lastName,
                        language,
                        email,
                        birthday,
                        nationality,
                        phoneNumber
                        );

                    student = new StudentDTO(
                        studentId,
                        joinDate,
                        person,
                        interest,
                        password,
                        pathOfImageProfile, // Assuming PathToProfileImage is not retrieved from the database in this query
                        infoTargetLanguage);

                    return true;
                }
            }
        }
    }

    return false;
}
      
    public static bool GetAllStudents(out List<StudentDTO> students)
    {
            students = new List<StudentDTO>();

            using (var connection = new NpgsqlConnection(  SettingDataAccessConnectionString.ConnectionString))
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

    public static bool IsExistingStudent(string? FirstName,string? email)
        {
            using (var connection = new NpgsqlConnection(
                SettingDataAccessConnectionString.ConnectionString))
            {
                connection.Open();

                string query = @"
                    SELECT EXISTS(
                        SELECT 1
                        FROM student s
                        JOIN person p ON s.""FK_person"" = p.id
                        WHERE p.""FirstName"" = @FirstName AND p.email = @Email
                    );
                ";

                using (var command = new NpgsqlCommand(
                    query,
                    connection))
                {
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@Email", email);

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

                    Language.GetLanguageByName(student.Person?.Native_Language?.Name, out LanguageDTO? nativeLanguage);
                    if (nativeLanguage == null)
                    {
                        throw new InvalidOperationException($"Native language {student.Person?.Native_Language?.Name} does not exist.");
                    }
                    student.Person!.Native_Language = nativeLanguage;

                    Nationality.GetNationalityByName(
                        student.Person.Nationality?.Name ?? string.Empty,
                        out NationalityDTO? nationality);
                    if (nationality == null)
                    {
                        throw new InvalidOperationException($"Nationality {student.Person.Nationality?.Name} does not exist.");
                    }
                    student.Person.Nationality = nationality;

                    LevelLanguage.GetLevelLanguageByName(student.infoTargetLanguage?.Level?.LevelName, out LevelLanguageDTO? levelLanguage);
                    if (levelLanguage == null)
                    {
                        throw new InvalidOperationException($"Level language {student.infoTargetLanguage?.Level?.LevelName} does not exist.");
                    }
                     
                    InfoTargetLanguageDTO infoTargetLanguageDTO = new InfoTargetLanguageDTO(
                        0,
                        levelLanguage,
                        student.infoTargetLanguage?.Language ?? throw new InvalidOperationException("Language cannot be null.")); 
                    
                    long id = InfoTargetLanguage.AddNewInfoTargetLanguage(infoTargetLanguageDTO, transaction, connection);
                    if (id <= 0)
                    {
                        throw new InvalidOperationException("Failed to add new info target language.");
                    }
                    infoTargetLanguageDTO.Id = id;
                    long infoTargetLanguageId = id;
                    
                    // Add the person and get the generated ID
                    long personID = Person.AddNewPerson(student.Person ?? throw new InvalidOperationException("Person cannot be null."), transaction, connection)
                        ?? throw new InvalidOperationException("Failed to add new person.");
                    if (personID <= 0)
                    {
                        throw new InvalidOperationException("Failed to add new person.");
                    }

                    string query = @"
                        INSERT INTO student
                        (
                            join_date,
                            interest,
                            password,
                            pathOfImageProfile,
                            ""FK_person"",
                            ""FK_info_Target_languge""
                        )
                        VALUES
                        (
                            @joinDate,
                            @interest,
                            @password,
                            @pathOfImageProfile,
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
                            student.PassWord);

                         command.Parameters.AddWithValue(
                            "@pathOfImageProfile",
                            student.PathToProfileImage);
                        
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
                catch (Exception ex)
                {
                    transaction.Rollback();

                        string logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
                            Directory.CreateDirectory(logDirectory);        

                        string logFile = Path.Combine(logDirectory, "er     rors.txt");

                        File.AppendAllText(     
                            logFile,        
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +      
                            $"DATA ERROR: {ex}\n" +     
                            "----------------------------------------\n     ");

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
                    // InfoTargetLanguageDTO infoTargetLanguage,NpgsqlTransaction transaction,NpgsqlConnection connection
                    bool result = InfoTargetLanguage.UpdateInfoTargetLanguage(student.infoTargetLanguage ?? throw new InvalidOperationException("Info target language cannot be null."), transaction, connection);
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
                                ""pathOfImageProfile"" = @pathOfImageProfile,
                                ""FK_person"" = @personId,
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
                        command.Parameters.AddWithValue("@pathOfImageProfile", student.PathToProfileImage ?? (object)DBNull.Value);

                        int rowsAffected = command.ExecuteNonQuery();

                        transaction.Commit();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    string logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
                    Directory.CreateDirectory(logDirectory);

                    string logFile = Path.Combine(logDirectory, "errors.txt");

                    File.AppendAllText(
                         logFile,
                         $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                         $"DATA ERROR: {ex}\n" +
                         "----------------------------------------\n");
                    return false;
                }
            }
        }
    

        }
    
    public static bool DeleteStudentById(long id)
    {
        using (var connection = new NpgsqlConnection(SettingDataAccessConnectionString.ConnectionString))
        {
            connection.Open();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // First, delete the student record
                    string deleteStudentQuery = @"
                        DELETE FROM student
                        WHERE id = @id;
                    ";

                    using (var command = new NpgsqlCommand(deleteStudentQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            // No student found with the given ID
                            transaction.Rollback();
                            return false;
                        }
                    }

                    if(!InfoTargetLanguage.DeleteInfoTargetLanguageById(id, transaction, connection))
                    {
                        transaction.Rollback();
                        return false;
                    }
                    if(!Person.DeletePersonById(id, transaction, connection))
                    {
                        transaction.Rollback();
                        return false;        
                    }

                    transaction.Commit();
                    return true;
                }
                catch  (Exception ex)
                {
                    transaction.Rollback();
                     string logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
                    Directory.CreateDirectory(logDirectory);

                    string logFile = Path.Combine(logDirectory, "errors.txt");

                    File.AppendAllText(
                         logFile,
                         $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                         $"DATA ERROR: {ex}\n" +
                         "----------------------------------------\n");

                    
                }
            }
        }   
        return false;
    }

    }
}

