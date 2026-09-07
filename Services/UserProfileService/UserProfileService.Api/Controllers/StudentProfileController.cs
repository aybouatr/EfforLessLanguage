﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer;
using DataAccesLayer;

namespace StudentProfileService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentProfileController : ControllerBase
{
    // ==========================================
    // GET: api/StudentProfile
    // ==========================================

    [HttpGet (Name = "GetStudents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetStudents()
    {
        var students = StudentService.GetStudents();

        if (students.Count == 0)
        {
            return NotFound("No students found.");
        }

        return Ok(students);
    }


    // ==========================================
    // GET: api/StudentProfile/{id}
    // ==========================================

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetStudentById(long id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid student ID.");
        }

        StudentDTO? student = StudentService.Find(id);

        if (student == null)
        {
            return NotFound(
                $"Student with ID {id} not found.");
        }

        return Ok(student);
    }
}