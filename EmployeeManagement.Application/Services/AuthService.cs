using EmployeeManagement.Application.Auth;
using EmployeeManagement.Application.Interfaces;

namespace EmployeeManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtService _jwt;

    public AuthService(
        IUserRepository users,
        IPasswordHasher hasher,
        IJwtService jwt)
    {
        _users = users;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _users.GetByUsername(request.Username);

        if (user == null || !_hasher.Verify(request.Password, user.PasswordHash))
            return new LoginResponse();

        var token = _jwt.GenerateToken(user.Username, user.Role);

        return new LoginResponse
        {
            Token = token
        };
    }
}