﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer;
using DataAccesLayer;

namespace StudentProfileService.Api.Controllers;
//                 




[ApiController]
[Route("api/Profile")]
public class StudentProfileController : ControllerBase
{
    [HttpGet(Name = "GetStudents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetStudents()
    {
        List<StudentResponseDTO>? students = StudentService.GetStudents();

        if (students?.Count > 0)
        {
            return Ok(students);
        }

        return NotFound("No students found.");
    }

    [HttpGet("{id:long}", Name = "GetStudentById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetStudentById(long id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid student ID.");
        }

        StudentResponseDTO? student = StudentService.FindSTO(id);

        if (student == null)
        {
            return NotFound($"Student with ID {id}  not found.");
        }

        return Ok(student);
    }


    private void UpdateDataInClass(StudentService existingStudent, StudentResponseDTO student)
    {
    
        if (existingStudent == null || student == null)
        {
            throw new ArgumentNullException(nameof(existingStudent), "Existing student cannot be null.");
        }

        existingStudent.JoinDate = student.JoinDate;
        existingStudent.interst = student.Interest;
        existingStudent.PassWord = student.Password;
        existingStudent.PathToProfileImage = student.PathToProfileImage;
        if (existingStudent.Persone == null)
        {
            throw new InvalidOperationException("Student person data is missing.");
        }
        existingStudent.Persone.FirstName = student.FirstName ?? existingStudent.Persone.FirstName;
        existingStudent.Persone.LastName = student.LastName ?? existingStudent.Persone.LastName;
        existingStudent.Persone.PhoneNumber = student.PhoneNumber ?? existingStudent.Persone.PhoneNumber;
        existingStudent.Persone.Email = student.Email ?? existingStudent.Persone.Email;
        if (existingStudent.Persone.Native_Language != null)
        {
            existingStudent.Persone.Native_Language.Name = student.NativeLanguage ?? existingStudent.Persone.Native_Language.Name;
        }
        if (student.DateOfBirth.HasValue)
        {
            existingStudent.Persone.DateOfBirth = student.DateOfBirth.Value;
        }
       
        if (existingStudent.Persone.Nationality != null)
        {
            existingStudent.Persone.Nationality.Name = student.Nationality ?? existingStudent.Persone.Nationality.Name;
        }

        if (!LevelLanguage.GetLevelLanguageByName(
                student.LevelTargetLanguage,
                out LevelLanguageDTO? level))
        {
            throw new InvalidOperationException(
                $"Level language {student.LevelTargetLanguage} does not exist.");
        }

        if (!Language.GetLanguageByName(
                student.TargetLanguage,
                out LanguageDTO? targetLanguage))
        {
            throw new InvalidOperationException(
                $"Target language {student.TargetLanguage} does not exist.");
        }

        existingStudent.InfoTargetLanguage = new InfoTargetLanguageDTO(
            existingStudent.InfoTargetLanguage?.Id ?? 0,
            level,
            targetLanguage);

    }

    [HttpPut("{id:long}", Name = "UpdateStudent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateStudent(long id, [FromBody] StudentResponseDTO student)
    {
        
        if (id <= 0 || student == null || id != student.Id)
        {
            return BadRequest("Invalid student ID or student data.");
        }

        StudentService? existingStudent = StudentService.Find(id);
        if (existingStudent == null)
        {
            // Console.WriteLine($"Student with ID {id} not found.");
            return NotFound($"Student with ID {id} not found.");
        }
        
        existingStudent.JoinDate = student.JoinDate;

        if (existingStudent.Persone == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Student person data is missing.");
        }
        try
        {
            UpdateDataInClass(existingStudent, student);
            if (existingStudent.Save())
            {
                return Ok(StudentService.FindSTO(id));
                // return Ok("this is a test");
            }
        }
        catch (Exception ex)
        {
            
             string logDirectory = Path.Combine(AppContext.BaseDirectory, "Logs");
                    Directory.CreateDirectory(logDirectory);

                    string logFile = Path.Combine(logDirectory, "errors.txt");

                    System.IO.File.AppendAllText(
                         logFile,
                         $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                         $"DATA ERROR: {ex}\n" +
                         "----------------------------------------\n");
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating student data: {ex.Message}");
        }

        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the student.");


    }


    [HttpDelete("{id:long}", Name = "DeleteStudent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteStudent(long id)
    {
        // if (id <= 0)
        // {
            return BadRequest("i should build all table is id stebd FK.");
        // }

        StudentService? existingStudent = StudentService.Find(id);
        if (existingStudent == null)
        {
            return NotFound($"Student with ID {id} not found.");
        }

        try
        {
            if (StudentService.Delete(id))
            {
                return Ok($"Student with ID {id} has been deleted successfully.");
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, "Failed to delete the student.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting student: {ex.Message}");
        }
    }

    private void AssignDataToStudentService(StudentService? existingStudent, StudentResponseDTO student)
    {
        if (existingStudent == null || student == null)
        {
            throw new ArgumentNullException(nameof(existingStudent), "Existing student cannot be null.");
        }

        existingStudent.JoinDate = student.JoinDate;
        existingStudent.interst = student.Interest;
        existingStudent.PassWord = student.Password;
        existingStudent.PathToProfileImage = student.PathToProfileImage;
        if (existingStudent.Persone == null)
        {
            throw new InvalidOperationException("Student person data is missing.");
        }
        existingStudent.Persone.FirstName = student.FirstName ?? existingStudent.Persone.FirstName;
        existingStudent.Persone.LastName = student.LastName ?? existingStudent.Persone.LastName;
        existingStudent.Persone.PhoneNumber = student.PhoneNumber ?? existingStudent.Persone.PhoneNumber;
        existingStudent.Persone.Email = student.Email ?? existingStudent.Persone.Email;
        if (existingStudent.Persone.Native_Language != null)
        {
            existingStudent.Persone.Native_Language.Name = student.NativeLanguage ?? existingStudent.Persone.Native_Language.Name;
        }
        if (student.DateOfBirth.HasValue)
        {
            existingStudent.Persone.DateOfBirth = student.DateOfBirth.Value;
        }
       
        if (existingStudent.Persone.Nationality != null)
        {
            existingStudent.Persone.Nationality.Name = student.Nationality ?? existingStudent.Persone.Nationality.Name;
        }

        if (!LevelLanguage.GetLevelLanguageByName(
                student.LevelTargetLanguage,
                out LevelLanguageDTO? level))
        {
            throw new InvalidOperationException(
                $"Level language '{student.LevelTargetLanguage}' does not exist.");
        }

        if (!Language.GetLanguageByName(
                student.TargetLanguage,
                out LanguageDTO? targetLanguage))
        {
            throw new InvalidOperationException(
                $"Target language '{student.TargetLanguage}' does not exist.");
        }

        existingStudent.InfoTargetLanguage = new InfoTargetLanguageDTO(
            0,
            level,
            targetLanguage);
    }

    [HttpPost("{id:long}/AddNewStudent", Name = "AddNewStudent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddNewStudent([FromBody] StudentResponseDTO student)
    {
        StudentService? existingStudent = StudentService.Find(-1);
        if (existingStudent == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to initialize the student.");
        }

        try
        {
            AssignDataToStudentService(existingStudent, student);
            if (existingStudent.Save())
            {
                Console.WriteLine(
                    $"Student with ID {existingStudent.Id} has been added successfully.");
                return Ok();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(
                $"Error while saving student with ID {existingStudent.Id}: {ex}");

            return StatusCode(500, $"An error occurred while saving the student: {ex.Message}");
        }
            

        return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the student.");
    }

}
