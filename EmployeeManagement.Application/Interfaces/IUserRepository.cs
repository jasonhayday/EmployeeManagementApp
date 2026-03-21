using EmployeeManagement.Domain.Entities;

namespace EmployeeManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsername(string username);
}