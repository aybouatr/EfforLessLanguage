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
        public DateTime Birthday { get; set; }
        public DateTime JoinDate { get; set; }
        public long Interest { get; set; }
        public string Language { get; set; }
        public string LevelLanguage { get; set; }

        public StudentService()
        {
            _mode = enMode.AddNew;

            Id = -1;
            FirstName = string.Empty;
            LastName = string.Empty;
            Birthday = DateTime.MinValue;
            JoinDate = DateTime.MinValue;
            Interest = 0;
            Language = string.Empty;
            LevelLanguage = string.Empty;
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
        }

        public static List<StudentDTO> GetStudents()
        {
            if (Student.GetStudentAllStudent(
                out List<StudentDTO> students))
            {
                return students;
            }

            Console.WriteLine(
                "Error retrieving students from the database.");

            return new List<StudentDTO>();
        }

        public static StudentDTO? Find(long id)
        {
            if (Student.GetStudentById(
                id,
                out StudentDTO? student))
            {
                return student;
            }

            return null;
        }


       
        private bool _AddNewStudent()
        {
            // TODO:
            // Implement adding student to database.

            return false;
        }


        private bool _UpdateStudent()
        {
            // TODO:
            // Implement updating student in database.

            return false;
        }


        // ==========================================
        // Delete Student
        // ==========================================

        public static bool DeleteStudent(long id)
        {
            return Student.DeleteStudentById(id);
        }


        // ==========================================
        // Save
        // ==========================================

        public bool Save()
        {
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