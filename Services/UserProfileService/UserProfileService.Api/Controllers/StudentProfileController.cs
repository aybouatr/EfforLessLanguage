﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer;
using DataAccesLayer;

namespace StudentProfileService.Api.Controllers;

[ApiController]
[Route("api/Profile")]
public class StudentProfileController : ControllerBase
{
    [HttpGet(Name = "GetStudents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetStudents()
    {
        List<StudentDTO> students = StudentService.GetStudents();

        if (students.Count > 0)
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

        StudentDTO? student = StudentService.FindSTO(id);

        if (student == null)
        {
            return NotFound($"Student with ID {id}  not found.");
        }

        return Ok(student);
    }

    [HttpPut("{id:long}", Name = "UpdateStudent")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateStudent(long id, [FromBody] StudentDTO student)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid student ID.");
        }

        if (student == null)
        {
            return BadRequest("Student data is required.");
        }

        if (student.Id != id)
        {
            return BadRequest("Student ID in the URL does not match the student ID.");
        }

        var existingStudent = StudentService.Find(id);

        if (existingStudent == null)
        {
            return NotFound($"Student with ID {id} not found.");
        }

        existingStudent.FirstName = student.FirstName;
        existingStudent.LastName = student.LastName;
        existingStudent.Email = student.Email;
        existingStudent.PhoneNumber = student.PhoneNumber;
        existingStudent.Birthday = student.Birthday;
        existingStudent.JoinDate = student.JoinDate;
        existingStudent.Interest = student.Interest;
        existingStudent.Language = student.Language;
        existingStudent.LevelLanguage = student.LevelLanguage;


        bool result = existingStudent.Save();

        if (!result)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the student profile.");
        }

        return Ok(student);
    }
}
