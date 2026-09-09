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

    // id join_date FK_person interest password FK_info_Target_languge
    public class StudentService
    {
        public enMode _mode;

        public long? Id { get; set; }
        public DateTime JoinDate { get; set; }

        public PersonDTO? Persone { get; set; }
        public long interst { get; set; }

        public string? PassWord { get; set; }

        public InfoTargetLanguageDTO? InfoTargetLanguage { get; set; }

        public StudentService()
        {
            _mode = enMode.AddNew;
            JoinDate = DateTime.Now;
            Persone = null;
            interst = 0;
            PassWord = string.Empty;
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
            InfoTargetLanguage = student.infoTargetLanguage;
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
                out StudentDTO? student) && student is not null)
            {
                return new StudentService(new StudentDTO(
                    student.Id,
                    student.JoinDate,
                    student.Person,
                    student.Interest,
                    student.PassWord,
                    student.infoTargetLanguage));

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
                    
                    return true;
                }

                private bool _AddNewStudent()
                {
                    long? studentId = Student.AddNewStudent(new StudentDTO(
                        Id ?? 0L,
                        JoinDate,
                        Persone,
                        interst,
                        PassWord,
                        InfoTargetLanguage));

                    if (studentId.HasValue)
                    {
                        Id = studentId.Value;
                        _mode = enMode.Edit;
                        return true;
                    }
                   



                    return false;
                }

                private bool _UpdateStudent()
                {
                    if (Student.UpdateStudent(
                        new StudentDTO(
                            Id ?? 0L,
                            JoinDate,
                            Persone,
                            interst,
                            PassWord,
                            InfoTargetLanguage)))
                    {
                        return true;
                    }

                    return false;
                }

                public static bool DeleteStudent(long id)
                {
                    return Student.DeleteStudentById (id);
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
