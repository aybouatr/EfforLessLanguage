using System;
using System.Collections.Generic;
using DataAccesLayer;

Console.WriteLine("=================================");
Console.WriteLine(" UserProfileService Manual Tests");
Console.WriteLine("=================================");

// =================================
// TEST 1: Get all students
// =================================

Console.WriteLine("\nTEST 1: GetStudentAllStudent()");

try
{
    if (Student.IsExistingStudent (1))
    {
        Console.WriteLine("PASS");
       
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

