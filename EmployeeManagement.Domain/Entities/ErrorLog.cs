namespace EmployeeManagement.Domain.Entities;

public class ErrorLog
{
    public int Id { get; set; }

    public int StatusCode { get; set; }

    public string Message { get; set; } = "";

    public string Path { get; set; } = "";

    public string Method { get; set; } = "";

    public string? User { get; set; }

    public string Level { get; set; } = "Error";

    public string TraceId { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}