using System;
using System.Collections.Generic;
using DataAccesLayer;

namespace BusinessLogicLayer
{
    public enum enMode
    {
        AddNew = 1,
        Edit = 2
    }

    public class StudentService
    {
        public enMode _mode;

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PassWord { get; set; }

        public DateTime Birthday { get; set; }
        public DateTime JoinDate { get; set; }
        public long Interest { get; set; }
        public string Language { get; set; }
        public string LevelLanguage { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long Nationality { get; set; }


        public StudentService()
        {
            _mode = enMode.AddNew;

            Id = -1;
            FirstName = string.Empty;
            LastName = string.Empty;
            PassWord = string.Empty;
            Birthday = DateTime.MinValue;
            JoinDate = DateTime.MinValue;
            Interest = 0;
            Language = string.Empty;
            LevelLanguage = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Nationality = 0;
        }


        private StudentService(StudentDTO student)
        {
            _mode = enMode.Edit;

            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            Birthday = student.Birthday;
            JoinDate = student.JoinDate;
            Interest = student.Interest;
            Language = student.Language;
            LevelLanguage = student.LevelLanguage;
            Email = student.Email;
            PhoneNumber = student.PhoneNumber;
            Nationality = student.Nationality;
        }

        public StudentService(long id, string firstName, string lastName, string password, DateTime birthday, DateTime joinDate, long interest, string language, string levelLanguage, string email, string phoneNumber, long nationality)
        {
            _mode = enMode.Edit;

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PassWord = password;
            Birthday = birthday;
            JoinDate = joinDate;
            Interest = interest;
            Language = language;
            LevelLanguage = levelLanguage;
            Email = email;
            PhoneNumber = phoneNumber;
            Nationality = nationality;
        }


        public static List<StudentDTO> GetStudents()
        {
            if (Student.GetAllStudents(
                out List<StudentDTO> students))
            {
                return students;
            }

            Console.WriteLine(
                "Error retrieving students from the database.");

            return new List<StudentDTO>();
        }

        public static StudentService? Find(long id)
        {
            if (Student.GetStudentById(
                id,
                out StudentDTO? student))
            {
                return new StudentService(
                    student.Id,
                    student.FirstName,
                    student.LastName,
                    student.PassWord,
                    student.Birthday,
                    student.JoinDate,
                    student.Interest,
                    student.Language,
                    student.LevelLanguage,
                    student.Email,
                    student.PhoneNumber,
                    student.Nationality);
            }

            return null;
        }

        public static StudentDTO? FindSTO(long id)
        {
            if (Student.GetStudentById(
                id,
                out StudentDTO? student))
            {
                return student;
            }

            return null;
        }


        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(LastName))
                return false;

            if (string.IsNullOrWhiteSpace(PassWord))
                return false;

            if (Birthday == DateTime.MinValue)
                return false;

            if (JoinDate == DateTime.MinValue)
                return false;

            if (Interest <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(Language))
                return false;

            if (string.IsNullOrWhiteSpace(LevelLanguage))
                return false;

            if (string.IsNullOrWhiteSpace(Email))
                return false;

            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return false;

            if (Nationality <= 0)
                return false;

            return true;
        }

        private bool _AddNewStudent()
        {
            if (Student.AddNewStudent(
                new StudentDTO(
                    Id,
                    FirstName,
                    LastName,
                    PassWord,
                    Birthday,
                    JoinDate,
                    Interest,
                    Language,
                    LevelLanguage,
                    Email,
                    PhoneNumber,
                    Nationality,
                    Nationality)))
            {
                this._mode = enMode.Edit;
                return true;
            }



            return false;
        }

        private bool _UpdateStudent()
        {
            if (Student.UpdateStudent(
                new StudentDTO(
                    Id,
                    FirstName,
                    LastName,
                    PassWord,
                    Birthday,
                    JoinDate,
                    Interest,
                    Language,
                    LevelLanguage,
                    Email,
                    PhoneNumber,
                    Nationality,
                    Nationality)))
            {
                return true;
            }

            return false;
        }

        public static bool DeleteStudent(long id)
        {
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