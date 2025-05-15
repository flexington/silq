using System.Collections.Generic;

namespace flx.SILQ.Tests;

public record TestContext
{
    public School School { get; set; }
}

public record School
{
    public string Name { get; set; }
    public string Address { get; set; }
    public int NumberOfStudents => Students.Count;
    public List<Student> Students { get; set; }
}

public record Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Grade { get; set; }
    public School School { get; set; }
}