using BusinessLogicLayer;
using DataAccesLayer;

class Program
{
    static void Main(string[] args)
    {
        // Create an instance of StudentService
        Student.GetStudentById(
            1,
            out StudentDTO? student);

        if (student != null)
        {
            Console.WriteLine("===== STUDENT INFORMATION =====");

            Console.WriteLine($"Student ID: {student.Id}");
            Console.WriteLine($"Join Date: {student.JoinDate}");
            Console.WriteLine($"Interest: {student.Interest}");

            Console.WriteLine($"First Name: {student.Person.FirstName}");
            // Console.WriteLine($"Last Name: {student.LastName}");

            // Console.WriteLine(
                // $"Date of Birth: {student.DateOfBirth}"
            // );

            // Console.WriteLine(
            //     $"Native Language: {student.NativeLanguage}"
            // );

            // Console.WriteLine(
            //     $"Level Target Language: {student.LevelTargetLanguage}"
            // );

            // Console.WriteLine(
            //     $"Target Language: {student.TargetLanguage}"
            // );

            // Console.WriteLine(
            //     $"Nationality: {student.Nationality}"
            // );

            // Console.WriteLine(
            //     $"Phone Number: {student.PhoneNumber}"
            // );

            Console.WriteLine(
                $"Path To Profil Image : {student.PathToProfileImage}"
            );


            Console.WriteLine(
                $"Pasword: {student.PassWord}"
            );
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
    }
}