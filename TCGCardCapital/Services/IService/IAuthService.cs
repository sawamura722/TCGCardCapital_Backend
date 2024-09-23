using TCGCardCapital.DTOs;

namespace TCGCardCapital.Services.IService
{
    public interface IAuthService
    {
        Task<string> Register(UserDTO userDto);
        Task<string> Login(string email, string password);
    }
}
