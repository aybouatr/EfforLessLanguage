using System;
using System.Collections.Generic;
using DataAccesLayer;
using BusinessLogicLayer;

Console.WriteLine("=================================");
Console.WriteLine(" UserProfileService Manual Tests");
Console.WriteLine("=================================");

// =================================
// TEST 1: Get all students
// =================================

Console.WriteLine("\nTEST 1: GetStudentAllStudent()");

try
{
     List<StudentDTO> students = StudentService.GetStudents();
    if (students.Count > 0)
    {
        Console.WriteLine("PASS");
        // console.WriteLine($"Retrieved {students.Count} students:");
        foreach (var student in students)
        {
            Console.WriteLine($"ID: {student.Id}, Name: {student.FirstName} {student.LastName}, Birthday: {student.Birthday.ToShortDateString()}, Join Date: {student.JoinDate.ToShortDateString()}, Interest: {student.Interest}, Language: {student.Language}, Level: {student.LevelLanguage}");
        }  
    }
    else
    {
        Console.WriteLine("FAIL");
        Console.WriteLine("Could not retrieve students.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("FAIL");
    Console.WriteLine($"Exception: {ex.Message}");
}


// =================================
// TEST 2: Get student by ID
// =================================

