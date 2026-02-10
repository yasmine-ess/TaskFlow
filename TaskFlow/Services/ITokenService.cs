using TaskFlow.Models; // ton ApplicationUser

namespace TaskFlow.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}