namespace DineMetrics.Core.Dto;

public class EmployeeDto
{
    public string Name { get; set; } 

    public string Position { get; set; }

    public string PhoneNumber { get; set; } 

    public DateOnly WorkStart { get; set; }

    public DateOnly? WorkEnd { get; set; } 

    public int ManagerId { get; set; } 
}