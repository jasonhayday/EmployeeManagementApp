using EmployeeManagement.Application.Common;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeesController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAllAsync();

        return Ok(
            new ApiResponse<object>(
                true,
                "Employees retrieved successfully",
                result
            )
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateDto dto)
    {
        var result = await _service.CreateAsync(dto);

        return Ok(
            new ApiResponse<object>(
                true,
                "Employee created successfully",
                result
            )
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EmployeeCreateDto dto)
    {
        await _service.UpdateAsync(id, dto);

        return Ok(
            new ApiResponse<object>(
                true,
                "Employee updated successfully",
                null
            )
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);

        return Ok(
            new ApiResponse<object>(
                true,
                "Employee deleted successfully",
                null
            )
        );
    }
}