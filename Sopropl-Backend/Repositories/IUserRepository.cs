using System.Collections.Generic;
using System.Threading.Tasks;
using Sopropl_Backend.Data;
using Sopropl_Backend.DTOs;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CreateAsync(User user);
        Task<bool> SaveChangesAsync();
        void Update(User user);
        Task<User> FindByIdAsync(string userId);
        Task<User> FindByNameAsync(string userName);
        // Task<User> FindByNameAsync(string userName);
        Task<User> FindByEmailAsync(string email);
        Task<IEnumerable<User>> SearchAsync(User currentUser, string text);
        User FindByName(string userName);
        User FindByEmail(string email);
        Task<bool> UserExsistsAsync(string userName);
        void UpdateProfile(User user, User newUser);

    }
}