using System;

namespace Sopropl_Backend.DTOs
{
    public class PhotoToReturnDTO
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }
        public string OwnerId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}