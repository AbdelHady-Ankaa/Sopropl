using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SoproplDbContext context;
        private readonly INormalizer<string> normalizer;
        public UserRepository(SoproplDbContext context)
        {
            this.context = context;
            this.normalizer = new NameNormalizer();
        }

        public async Task<bool> CreateAsync(User user)
        {
            // Action a = ;
            // a.Invoke();
            user.NormalizedEmail = this.normalizer.Normalize(user.Email);
            user.NormalizedUserName = this.normalizer.Normalize(user.UserName);
            this.context.Users.Add(user);
            return await this.context.SaveChangesAsync() > 0;
            // IDisposable g;
            // g.
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            var user = await this.context.Users.Include(u => u.Photo).FirstOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<IEnumerable<User>> AllAsync()
        {
            return await this.context.Users.Include(u => u.Photo).ToListAsync();
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            email = this.normalizer.Normalize(email);
            var user = await this.context.Users.Include(u => u.Photo).FirstOrDefaultAsync(u => u.NormalizedEmail == email);

            return user;
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            userName = this.normalizer.Normalize(userName);
            var user = await this.context.Users.Include(u => u.Photo).FirstOrDefaultAsync(u => u.NormalizedUserName == userName);

            return user;
        }



        public User FindByName(string userName)
        {
            userName = this.normalizer.Normalize(userName);
            var user = this.context.Users.Include(u => u.Photo).FirstOrDefault(u => u.NormalizedUserName == userName);

            return user;
        }

        public User FindByEmail(string email)
        {
            email = this.normalizer.Normalize(email);
            var user = this.context.Users.Include(u => u.Photo).FirstOrDefault(u => u.NormalizedEmail == email);

            return user;
        }
        public async Task<bool> UserExsistsAsync(string userName)
        {
            var user = await this.FindByNameAsync(userName);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public void UpdateProfile(User user, User newUser)
        {
            user.PostalCode = newUser.PostalCode;
            user.City = newUser.City;
            user.Country = newUser.Country;
            user.Address = newUser.Address;
            user.Bio = newUser.Bio;
            user.Name = newUser.Name;
            this.Update(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public void Update(User user)
        {
            this.context.Users.Update(user);
        }

        public async Task<IEnumerable<User>> SearchAsync(User currentUser, string text)
        {
            text = this.normalizer.Normalize(text);
            var users = await this.context.Users.Where(u =>
            (u.NormalizedUserName.StartsWith(text) ||
            u.NormalizedEmail.StartsWith(text)) &&
            u.NormalizedUserName != currentUser.NormalizedUserName).OrderByDescending(u => u.NormalizedUserName)
            .Take(6)
            .ToListAsync();

            return users;
        }

        private Action _dispose;
        private bool _disposedValue = false; // To detect redundant calls

        // public Disposable(Action dispose)
        // {
        //     _dispose = dispose;
        // }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                // if (disposing)
                // {
                    // _dispose.in
                // }

                // _dispose = null;
                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}