using System;
using System.Collections.Generic;
using DataAccesLayer;

namespace BusinessLogicLayer
{

public class StudentResponseDTO
{
    public long Id { get; set; }
    public DateTime JoinDate { get; set; }
    public long Interest { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public string? NativeLanguage { get; set; }
    public string? LevelTargetLanguage { get; set; }
    public string? TargetLanguage { get; set; }

    public string? Nationality { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

    public string ? Password { get; set; }

    public string PathToProfileImage { get; set; } = string.Empty;

    public StudentResponseDTO(StudentDTO student)
    {
        Id = student.Id;
        JoinDate = student.JoinDate;
        Interest = student.Interest;

        FirstName = student.Person?.FirstName;
        LastName = student.Person?.LastName;
        DateOfBirth = student.Person?.DateOfBirth;

        NativeLanguage = student.Person?.Native_Language?.Name;
        LevelTargetLanguage = student.infoTargetLanguage?.Level?.LevelName;
        TargetLanguage = student.infoTargetLanguage?.Language?.Name;

        Nationality = student.Person?.Nationality?.Name;
        Password = student.PassWord;
        PathToProfileImage = student.PathToProfileImage;
        PhoneNumber = student.Person?.PhoneNumber;
        Email = student.Person?.Email;

    }

    public StudentResponseDTO()
    {
            
    }

        public StudentResponseDTO(string firstName, string lastName, DateTime? dateOfBirth, string? nativeLanguage, string? levelTargetLanguage, string? targetLanguage, string? nationality, string? phoneNumber, string? email, long id, DateTime joinDate, string PathToProfileImage, long interest)
    {
        Id = id;
        JoinDate =  joinDate;
        Interest =  interest;

        FirstName =  firstName;
        LastName =  lastName;
        DateOfBirth =  dateOfBirth;

        NativeLanguage =  nativeLanguage;
        LevelTargetLanguage =  levelTargetLanguage;
        TargetLanguage =  targetLanguage;

        Nationality =   nationality;
        PhoneNumber =  phoneNumber;
        this.PathToProfileImage = PathToProfileImage;
        Email =     email;
    }



}


    public enum enMode
    {
        AddNew = 1,
        Edit = 2
    }

    // id join_date FK_person interest password FK_info_Target_languge
    public class StudentService
    {
        public enMode _mode;

        public long? Id { get; set; }
        public DateTime JoinDate { get; set; }

        public PersonDTO? Persone { get; set; }
        public long interst { get; set; }

        public string? PassWord { get; set; }

        public string PathToProfileImage { get; set; } = string.Empty;

        public InfoTargetLanguageDTO? InfoTargetLanguage { get; set; }
        // long person_id, string firstName, string lastName, LanguageDTO nativeLanguage, string email, DateTime dateOfBirth, NationalityDTO nationality, string phoneNumber
        private StudentService()
        {
            _mode = enMode.AddNew;
            Id = -1;
            JoinDate = DateTime.Now;
            Persone = new PersonDTO(-1, "deault","deault", new LanguageDTO(-1, "deault"), "deault", DateTime.Now, new NationalityDTO(-1, "dfault"), "dfault");
            interst = 0;
            PassWord = string.Empty;
            PathToProfileImage = string.Empty;
            InfoTargetLanguage = null;

        }

        private StudentService(StudentDTO student)
        {
            _mode = enMode.Edit;
            Id = student.Id;
            JoinDate = student.JoinDate;
            Persone = student.Person;
            interst = student.Interest;
            PassWord = student.PassWord;
            PathToProfileImage = student.PathToProfileImage;
            InfoTargetLanguage = student.infoTargetLanguage;
        }

        private static StudentResponseDTO ConvertToStudentResponseDTO(StudentDTO student)
        {
            return new StudentResponseDTO(student);
        }

        private static List<StudentResponseDTO> ConvertStudentDTOToStudentResponseDTOList(List<StudentDTO> students)
        {
            List<StudentResponseDTO> studentResponseList = new List<StudentResponseDTO>();

            foreach (var student in students)
            {
                StudentResponseDTO studentResponse = ConvertToStudentResponseDTO(student);
                studentResponseList.Add(studentResponse);
            }

            return studentResponseList;
        }

        public static List<StudentResponseDTO>? GetStudents()
        {
            if (Student.GetAllStudents(
                out List<StudentDTO> students))
            {
                return ConvertStudentDTOToStudentResponseDTOList(students);
            }
            return null;
        }

        public static StudentService? Find(long id)
        {
            if (Student.GetStudentById(
                id,
                out StudentDTO? student) && student is not null)
            {
                return new StudentService(new StudentDTO(
                    student.Id,
                    student.JoinDate,
                    student.Person,
                    student.Interest,
                    student.PassWord,
                    student.PathToProfileImage,
                    student.infoTargetLanguage));

            }
            else
            {
                return new StudentService();
            }

        }

        public static StudentResponseDTO? FindSTO(long id)
        {
            if (Student.GetStudentById(
                id,
                out StudentDTO? student) && student is not null)
            {
                return ConvertToStudentResponseDTO(student);
            }
            return null;
        }

        private bool Validation()
        {

            return true;
        }

        private bool _AddNewStudent()
        {
            if (Student.IsExistingStudent(this.Persone?.FirstName,this.Persone?.Email))
            {
                throw new InvalidOperationException($"Student with Email {this.Persone?.Email} already exists.");
            }
            
        
            
           if (Student.AddNewStudent(new StudentDTO(
                    this.Id ?? 0L,
                    this.JoinDate,
                    this.Persone,
                    this.interst,
                    this.PassWord,
                    this.PathToProfileImage,
                    this.InfoTargetLanguage)) > 0)
            {
                return true;
            }

            return false;
        }

        private bool _UpdateStudent()
        {
            if (Student.UpdateStudent( new StudentDTO(
                    this.Id ?? 0L,
                    this.JoinDate,
                    this.Persone,
                    this.interst,
                    this.PassWord,
                    this.PathToProfileImage,
                    this.InfoTargetLanguage)))
            {
                return true;
            }

            return false;
        }

        public static bool Delete(long id)
        {
            if (!Student.IsExistingStudent(id))
            {
                return false;
            }
            return Student.DeleteStudentById(id);
        }

        public bool Save()
        {
            if (Validation() == false)
                return false;


            switch (_mode)
            {
                case enMode.AddNew:
                    return _AddNewStudent();

                case enMode.Edit:
                    return _UpdateStudent();

                default:
                    return false;
            }
        }

    }
}
