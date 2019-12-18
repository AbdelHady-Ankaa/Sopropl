using System;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Models;

namespace Sopropl_Backend.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IOptions<CloudinarySettings> CloudinarySettings;
        private Cloudinary cloudinary;
        private readonly SoproplDbContext context;
        public PhotoRepository(SoproplDbContext context, IOptions<CloudinarySettings> CloudinarySettings)
        {
            this.context = context;
            this.CloudinarySettings = CloudinarySettings;

            Account account = new Account(
                this.CloudinarySettings.Value.CloudName,
                this.CloudinarySettings.Value.ApiKey,
                this.CloudinarySettings.Value.ApiSecret
            );
            this.cloudinary = new Cloudinary(account);
        }

        public void AddPhotoFile(Photo entity, IFormFile file)
        {
            if (file.Length != 0)
            {
                var uploadResult = UploadPhotoToCloudinary(file);
                entity.PublicId = uploadResult.PublicId;
                entity.Url = uploadResult.Uri.ToString();
                entity.DateAdded = DateTime.Now;
                this.context.Add(entity);
            }
        }

        private UploadResult UploadPhotoToCloudinary(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };

                var uploadResult = this.cloudinary.Upload(uploadParams);
                return uploadResult;
            }
        }

        public void Remove(Photo entity)
        {
            if (entity.PublicId != null)
            {
                var result = RemovePhotoFromCloudinary(entity.PublicId);
                if (result.Result == "ok")
                {
                    this.context.Remove(entity);
                }
            }
            else
            {
                this.context.Remove(entity);
            }
        }

        private DeletionResult RemovePhotoFromCloudinary(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = this.cloudinary.Destroy(deletionParams);
            return result;
        }

        public void UpdatePhotoFile(Photo entity, IFormFile file)
        {
            if (entity.PublicId != null)
            {
                var result = RemovePhotoFromCloudinary(entity.PublicId);
                if (result.Result == "ok")
                {
                    var uploadResult = UploadPhotoToCloudinary(file);
                    entity.PublicId = uploadResult.PublicId;
                    entity.Url = uploadResult.Uri.ToString();
                    this.context.Update(entity);
                }
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> FindByIdAsync(string photoId)
        {
            return await this.context.Photos.FindAsync(photoId);
        }
    }
}