using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sopropl_Backend.Data;
using Sopropl_Backend.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Sopropl_Backend.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Sopropl_Backend.Repositories
{
    public interface IPhotoRepository
    {
        void AddPhotoFile(Photo entity, IFormFile file);
        void UpdatePhotoFile(Photo entity, IFormFile file);
        Task<Photo> FindByIdAsync(string photoId);
        Task<bool> SaveChangesAsync();
        void Remove(Photo entity);
    }
}