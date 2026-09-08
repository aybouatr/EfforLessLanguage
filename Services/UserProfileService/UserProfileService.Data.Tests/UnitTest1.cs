using Xunit;
using DataAccesLayer;

namespace UserProfileService.Data.Tests;

public class StudentTests
{
    [Fact]
    public void GetStudentById_ExistingStudent_ReturnsStudent()
    {
        // Arrange
        int studentId = 1;

        // Act
        bool result = Student.GetStudentById(
            studentId,
            out StudentDTO student);

        // Assert
        Assert.True(result);
        Assert.NotNull(student);

        Assert.Equal(studentId, student.Id);
        Assert.False(string.IsNullOrWhiteSpace(student.FirstName));
        Assert.False(string.IsNullOrWhiteSpace(student.LastName));
        Assert.False(string.IsNullOrWhiteSpace(student.Language));
        Assert.False(string.IsNullOrWhiteSpace(student.LevelLanguage));
    }

    [Fact]
    public void GetStudentById_NonExistingStudent_ReturnsFalse()
    {
        // Arrange
        int studentId = 1;

        // Act
        bool result = Student.GetStudentById(
            studentId,
            out StudentDTO student);

        // Assert
        Assert.False(result);
        Assert.Null(student);
        Console.WriteLine(student);
    }

    [Fact]

    public void GetStudentById_ExistingStudent_ReturnsCorrectData()
    {
        // Arrange
        int studentId = 1;

        // Act
        bool result = Student.GetStudentById(
            studentId,
            out StudentDTO student);

        // Assert
        Assert.True(result);
        Assert.NotNull(student);

        Assert.Equal(1, student.Id);
        Assert.Equal("Ahmed", student.FirstName);
        Assert.Equal("Ali", student.LastName);

        Assert.Equal(
            new DateTime(2000, 1, 15),
            student.Birthday);

        Assert.Equal(
            new DateTime(2026, 9, 1),
            student.JoinDate);

        // Assert.Equal("Football", student.Interest);
        Assert.Equal("English", student.Language);
        Assert.Equal("B1", student.LevelLanguage);
    }
}