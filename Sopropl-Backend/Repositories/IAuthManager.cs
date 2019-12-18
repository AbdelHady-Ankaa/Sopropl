using System.Threading.Tasks;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface IAuthManager
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string userName, string password);
        string GenerateToken(User user);
    }
}