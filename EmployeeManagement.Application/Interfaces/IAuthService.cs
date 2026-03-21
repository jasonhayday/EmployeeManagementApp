using EmployeeManagement.Application.Auth;

namespace EmployeeManagement.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}