using DeniMetrics.WebAPI.Attributes;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DeniMetrics.WebAPI.Controllers;

[Authorize]
public class EmployeesController : BaseController
{
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IRepository<User> _userRepository;

    public EmployeesController(IRepository<Employee> employeeRepository, IRepository<User> userRepository)
    {
        _employeeRepository = employeeRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAll()
    {
        var employees = await _employeeRepository.GetAllAsync();
        
        var employeeDtos = employees.Select(employee => new EmployeeDto
        {
            Name = employee.Name,
            Position = employee.Position,
            PhoneNumber = employee.PhoneNumber,
            WorkStart = employee.WorkStart,
            WorkEnd = employee.WorkEnd,
            ManagerId = employee.Manager.Id
        }).ToList();
        
        return employeeDtos;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(int id)
    {
        var result = await _employeeRepository.GetByIdAsync(id);

        if (result is null)
            return BadRequest("Employee not found");
        
        return new EmployeeDto
        {
            Name = result.Name,
            Position = result.Position,
            PhoneNumber = result.PhoneNumber,
            WorkStart = result.WorkStart,
            WorkEnd = result.WorkEnd,
            ManagerId = result.Manager.Id 
        };
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] EmployeeDto dto)
    {
        var manager = await _userRepository.GetByIdAsync(dto.ManagerId);

        if (manager is null)
            return BadRequest("Manager not found");
        
        var employee = new Employee
        {
            Name = dto.Name,
            Position = dto.Position,
            PhoneNumber = dto.PhoneNumber,
            WorkStart = dto.WorkStart,
            WorkEnd = dto.WorkEnd,
            Manager = manager,
        };

        await _employeeRepository.CreateAsync(employee);

        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] EmployeeDto dto)
    {
        var existingEmployee = await _employeeRepository.GetByIdAsync(id);
        if (existingEmployee == null)
            return BadRequest("Employee not found");

        var manager = await _userRepository.GetByIdAsync(dto.ManagerId);
        if (manager == null)
            return BadRequest("Manager not found");

        existingEmployee.Name = dto.Name;
        existingEmployee.Position = dto.Position;
        existingEmployee.PhoneNumber = dto.PhoneNumber;
        existingEmployee.WorkStart = dto.WorkStart;
        existingEmployee.WorkEnd = dto.WorkEnd;
        existingEmployee.Manager = manager;

        await _employeeRepository.UpdateAsync(existingEmployee);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _employeeRepository.RemoveByIdAsync(id);
        
        return Ok();
    }
}
